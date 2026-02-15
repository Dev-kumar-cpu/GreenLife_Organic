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
    public partial class ManageProducts : Form
    {
        private string connectionString =
             @"Data Source=DESKTOP-RDRJ8S0\SQLEXPRESS;Initial Catalog = GreenLife_Organic; Integrated Security = True; Trust Server Certificate=True;";


        public ManageProducts()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void LoadProducts()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT ProductID, Name, Category, Price, Stock, Supplier, Discount, FinalPrice FROM Products";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvProducts.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Products (Name, Category, Price, Stock, Supplier, Discount, FinalPrice) " +
                                   "VALUES ('New Product', 'Default', 0, 0, 'Supplier', 0, 0)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("New product added! Update the details in the table.");
                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding product: " + ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null) return;

            try
            {
                int productId = Convert.ToInt32(dgvProducts.CurrentRow.Cells["ProductID"].Value);
                string name = dgvProducts.CurrentRow.Cells["Name"].Value.ToString();
                string category = dgvProducts.CurrentRow.Cells["Category"].Value.ToString();
                decimal price = Convert.ToDecimal(dgvProducts.CurrentRow.Cells["Price"].Value);
                int stock = Convert.ToInt32(dgvProducts.CurrentRow.Cells["Stock"].Value);
                string supplier = dgvProducts.CurrentRow.Cells["Supplier"].Value.ToString();
                decimal discount = Convert.ToDecimal(dgvProducts.CurrentRow.Cells["Discount"].Value);

                decimal finalPrice = price - (price * discount / 100);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Products SET Name=@Name, Category=@Category, Price=@Price, Stock=@Stock, Supplier=@Supplier, Discount=@Discount, FinalPrice=@FinalPrice WHERE ProductID=@ProductID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Category", category);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@Stock", stock);
                    cmd.Parameters.AddWithValue("@Supplier", supplier);
                    cmd.Parameters.AddWithValue("@Discount", discount);
                    cmd.Parameters.AddWithValue("@FinalPrice", finalPrice);
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Product updated successfully!");
                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating product: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null) return;

            try
            {
                int productId = Convert.ToInt32(dgvProducts.CurrentRow.Cells["ProductID"].Value);

                DialogResult result = MessageBox.Show("Are you sure you want to delete this product?", "Confirm Delete", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "DELETE FROM Products WHERE ProductID=@ProductID";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Product deleted successfully!");
                    LoadProducts();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting product: " + ex.Message);
            }
        }
        

        private void btnApplyDiscount_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null) return;

            if (!decimal.TryParse(txtDiscount.Text, out decimal discount))
            {
                MessageBox.Show("Enter a valid discount percentage.");
                return;
            }

            try
            {
                int productId = Convert.ToInt32(dgvProducts.CurrentRow.Cells["ProductID"].Value);
                decimal price = Convert.ToDecimal(dgvProducts.CurrentRow.Cells["Price"].Value);
                decimal finalPrice = price - (price * discount / 100);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Products SET Discount=@Discount, FinalPrice=@FinalPrice WHERE ProductID=@ProductID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Discount", discount);
                    cmd.Parameters.AddWithValue("@FinalPrice", finalPrice);
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Discount applied successfully!");
                txtDiscount.Clear();
                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error applying discount: " + ex.Message);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.Show();
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dgvProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}



