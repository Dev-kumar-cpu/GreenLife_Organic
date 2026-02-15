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
using static GreenLife.login;

namespace GreenLife
{
    public partial class CartForm : Form
    {

        private string connectionString =
           @"Data Source=DESKTOP-RDRJ8S0\SQLEXPRESS;Initial Catalog = GreenLife_Organic; Integrated Security = True; Trust Server Certificate=True;";

        private int currentUserID;
        private List<CartItem> cartItems;

        public class CartItem
        {
            public int ProductID { get; set; }
            public string ProductName { get; set; } // Renamed from Name to match ProductsForm
            public decimal Price { get; set; }
            public int Quantity { get; set; }
            public decimal TotalPrice => Price * Quantity;
        }




        public CartForm(int userId, List<CartItem> cartItems)
        {
            InitializeComponent();

            currentUserID = userId;
            this.cartItems = cartItems;

            // Setup DataGridView columns
            dgvCart.Columns.Clear();
            dgvCart.Columns.Add("ProductID", "Product ID");
            dgvCart.Columns.Add("ProductName", "Name");
            dgvCart.Columns.Add("Price", "Price");
            dgvCart.Columns.Add("Quantity", "Quantity");
            dgvCart.Columns.Add("TotalPrice", "Total");

            LoadCart();

        }


        private void LoadCart()
        {
            dgvCart.Rows.Clear();
            decimal grandTotal = 0;

            foreach (var item in cartItems)
            {
                decimal total = item.TotalPrice;
                dgvCart.Rows.Add(item.ProductID, item.ProductName, item.Price, item.Quantity, total);
                grandTotal += total;
            }

            lblGrandTotal.Text = "Total: " + grandTotal.ToString("C");
        }

        

        private void lblGrandTotal_Click(object sender, EventArgs e)
        {

        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            if (cartItems.Count == 0)
            {
                MessageBox.Show("Cart is empty!");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    decimal grandTotal = cartItems.Sum(x => x.TotalPrice);

                    // 1️⃣ Insert into Orders
                    string insertOrder = @"INSERT INTO Orders 
                               (UserID OrderDate, TotalAmount, Status)
                               VALUES (@c, @date, @total, @status);
                               SELECT SCOPE_IDENTITY();";

                    SqlCommand cmdOrder = new SqlCommand(insertOrder, conn);
                    cmdOrder.Parameters.AddWithValue("@c", currentUserID);
                    cmdOrder.Parameters.AddWithValue("@date", DateTime.Now);
                    cmdOrder.Parameters.AddWithValue("@total", grandTotal);
                    cmdOrder.Parameters.AddWithValue("@status", "Pending");

                    int orderId = Convert.ToInt32(cmdOrder.ExecuteScalar());

                    // 2️⃣ Insert into OrderItems + Reduce Stock
                    foreach (var item in cartItems)
                    {
                        // Insert order item
                        string insertItem = @"INSERT INTO OrderItems 
                                  (OrderID, ProductID, Quantity, UnitPrice)
                                  VALUES (@o, @p, @q, @pr)";

                        SqlCommand cmdItem = new SqlCommand(insertItem, conn);
                        cmdItem.Parameters.AddWithValue("@o", orderId);
                        cmdItem.Parameters.AddWithValue("@p", item.ProductID);
                        cmdItem.Parameters.AddWithValue("@q", item.Quantity);
                        cmdItem.Parameters.AddWithValue("@pr", item.Price);
                        cmdItem.ExecuteNonQuery();

                        // Reduce stock
                        string updateStock = @"UPDATE Products
                                   SET StockQuantity = StockQuantity - @qty
                                   WHERE ProductID = @pid";

                        SqlCommand cmdStock = new SqlCommand(updateStock, conn);
                        cmdStock.Parameters.AddWithValue("@qty", item.Quantity);
                        cmdStock.Parameters.AddWithValue("@pid", item.ProductID);
                        cmdStock.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Order placed successfully!");
                cartItems.Clear();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
            }


        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (dgvCart.SelectedRows.Count > 0)
            {
                int index = dgvCart.SelectedRows[0].Index;
                cartItems.RemoveAt(index);
                LoadCart();
            }
        }

        private void dgvCart_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
