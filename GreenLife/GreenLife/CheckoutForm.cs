using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GreenLife
{
    public partial class CheckoutForm : Form
    {
        private string connectionString =
           @"Data Source=DESKTOP-RDRJ8S0\SQLEXPRESS;Initial Catalog = GreenLife_Organic; Integrated Security = True; Trust Server Certificate=True;";

        private int currentUserId;
        private List<CartForm.CartItem> cartItems;


        public CheckoutForm(int currentUserId, List<CartForm.CartItem> cartItems)
        {
            InitializeComponent();
            this.currentUserId = currentUserId;

            cmbPaymentMethod.Items.Add("Cash on Delivery");
            cmbPaymentMethod.Items.Add("Card Payment");
            cmbPaymentMethod.SelectedIndex = 0; // Default

            LoadCart();
        }

        private void ToggleCardFields()
        {
            bool isCard = cmbPaymentMethod.SelectedItem.ToString() == "Card Payment";

            txtName.Enabled = isCard;
            txtCardNumber.Enabled = isCard;
            txtCVV.Enabled = isCard;
            txtExpiry.Enabled = isCard;
        }

        // Load cart items into DataGridView
        private void LoadCart()
        {
            dgvCheckout.Columns.Clear();
            dgvCheckout.Columns.Add("Name", "Product Name");
            dgvCheckout.Columns.Add("Price", "Price");
            dgvCheckout.Columns.Add("Quantity", "Quantity");
            dgvCheckout.Columns.Add("Total", "Total");

            dgvCheckout.Rows.Clear();
            decimal grandTotal = 0;

            foreach (var item in cartItems)
            {
                decimal total = item.Price * item.Quantity;
                dgvCheckout.Rows.Add(item.ProductName, item.Price.ToString("C"), item.Quantity, total.ToString("C"));
                grandTotal += total;
            }

            lblTotal.Text = "Grand Total: " + grandTotal.ToString("C");

        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleCardFields();
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            decimal grandTotal = cartItems.Sum(x => x.Price * x.Quantity);
            string paymentMethod = cmbPaymentMethod.SelectedItem.ToString();
            string orderStatus = paymentMethod == "Cash on Delivery" ? "Pending" : "Paid";


            // Card verification if card selected
            if (paymentMethod == "Card Payment")

            {
                if (string.IsNullOrWhiteSpace(txtName.Text) ||
                    string.IsNullOrWhiteSpace(txtCardNumber.Text) ||
                    string.IsNullOrWhiteSpace(txtCVV.Text) ||
                    string.IsNullOrWhiteSpace(txtExpiry.Text))
                {
                    MessageBox.Show("Please fill in all card details.");
                    return;
                }

                if (txtCardNumber.Text.Length != 16 || !txtCardNumber.Text.All(char.IsDigit))
                {
                    MessageBox.Show("Card number must be 16 digits.");
                    return;
                }

                if (txtCVV.Text.Length != 3 || !txtCVV.Text.All(char.IsDigit))
                {
                    MessageBox.Show("CVV must be 3 digits.");
                    return;
                }

                if (!DateTime.TryParseExact("01/" + txtExpiry.Text, "dd/MM/yy", null, System.Globalization.DateTimeStyles.None, out _))
                {
                    MessageBox.Show("Expiry date must be in MM/YY format.");
                    return;
                }

                MessageBox.Show("Card verified successfully!");
            }

            // Save order in DB
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Insert order
                    string insertOrder = "INSERT INTO Orders (CustomerID, TotalAmount, Status, PaymentMethod, OrderDate) " +
                                         "VALUES (@c, @total, @status, @payment, @date); SELECT SCOPE_IDENTITY();";

                    SqlCommand cmdOrder = new SqlCommand(insertOrder, conn);
                    cmdOrder.Parameters.AddWithValue("@c", currentUserId);
                    cmdOrder.Parameters.AddWithValue("@total", grandTotal);
                    cmdOrder.Parameters.AddWithValue("@status", orderStatus);
                    cmdOrder.Parameters.AddWithValue("@payment", paymentMethod);
                    cmdOrder.Parameters.AddWithValue("@date", DateTime.Now);

                    int orderId = Convert.ToInt32(cmdOrder.ExecuteScalar());

                    // Insert order items
                    foreach (var item in cartItems)
                    {
                        string insertItem = "INSERT INTO OrderItems (OrderID, ProductID, Quantity, Price) " +
                                            "VALUES (@o, @p, @q, @pr)";
                        SqlCommand cmdItem = new SqlCommand(insertItem, conn);
                        cmdItem.Parameters.AddWithValue("@o", orderId);
                        cmdItem.Parameters.AddWithValue("@p", item.ProductID);
                        cmdItem.Parameters.AddWithValue("@q", item.Quantity);
                        cmdItem.Parameters.AddWithValue("@pr", item.Price);
                        cmdItem.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Order placed successfully!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();

        }
    }
}
