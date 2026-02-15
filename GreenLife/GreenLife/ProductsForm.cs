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
    public partial class ProductsForm : Form
    {
        private string connectionString =
           @"Data Source=DESKTOP-RDRJ8S0\SQLEXPRESS;Initial Catalog = GreenLife_Organic; Integrated Security = True; Trust Server Certificate=True;";

        private int UserID;
        private string FirstName;
        

        private List<CartForm.CartItem> cartItems = new List<CartForm.CartItem>();


       

        public ProductsForm(int UserID, string FirstName, List<CartForm.CartItem> cartItems)
        {
            InitializeComponent();
            this.UserID = UserID;
            this.FirstName = FirstName;
            this.cartItems = cartItems ?? new List<CartForm.CartItem>();

            lblWelcome.Text = "Welcome, " + FirstName;

            LoadProducts();


        }

        private void dgvProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            nudQuantity_ValueChanged(sender, e);
        }

        private void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0) return;

            int productId = Convert.ToInt32(dgvProducts.SelectedRows[0].Cells["ProductID"].Value);
            string name = dgvProducts.SelectedRows[0].Cells["ProductName"].Value.ToString();
            decimal price = Convert.ToDecimal(dgvProducts.SelectedRows[0].Cells["Price"].Value);
            int quantity = (int)nudQuantity.Value;

            if (quantity <= 0)
            {
                MessageBox.Show("Quantity must be at least 1.");
                return;
            }

            // Add or update in cart
            CartForm.CartItem existingItem = cartItems.FirstOrDefault(x => x.ProductID == productId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cartItems.Add(new CartForm.CartItem
                {
                    ProductID = productId,
                    ProductName = name,
                    Price = price,
                    Quantity = quantity
                });
            }


            // Open Cart Form
            CartForm cartForm = new CartForm(UserID,  cartItems);
            cartForm.ShowDialog();

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {

        }

        private void btnProfile_Click(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
           
            string searchText = txtSearch.Text.Trim();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"SELECT 
                                p.ProductID,
                                p.ProductName,
                                c.CategoryName,
                                p.Price,
                                p.StockQuantity,
                                p.DiscountPercent
                             FROM Products p
                             INNER JOIN Categories c 
                                ON p.CategoryID = c.CategoryID
                             WHERE p.ProductName LIKE @s 
                                OR c.CategoryName LIKE @s";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@s", "%" + searchText + "%");

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvProducts.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
            }
        }


        private void nudQuantity_ValueChanged(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count > 0)
            {
                decimal price = Convert.ToDecimal(dgvProducts.SelectedRows[0].Cells["Price"].Value);
                int quantity = (int)nudQuantity.Value;
                decimal total = price * quantity;

                lblTotalPrice.Text = "Total: " + total.ToString("C"); // currency format
            }
        }



        private void LoadProducts()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"SELECT 
                                p.ProductID,
                                p.ProductName,
                                c.CategoryName,
                                p.Price,
                                p.StockQuantity,
                                p.DiscountPercent
                             FROM Products p
                             INNER JOIN Categories c 
                                ON p.CategoryID = c.CategoryID";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvProducts.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
            }
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }


    }
}
