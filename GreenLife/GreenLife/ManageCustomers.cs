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
    public partial class ManageCustomers : Form
    {
        private string connectionString =
             @"Data Source=DESKTOP-RDRJ8S0\SQLEXPRESS;Initial Catalog = GreenLife_Organic; Integrated Security = True; Trust Server Certificate=True;";
        public ManageCustomers()
        {
            InitializeComponent();
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT UserID, FirstName, LastName, Email, Phone, Status 
                                     FROM Users 
                                     WHERE RoleID = 2"; // Only customers
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvCustomers.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"INSERT INTO Users (RoleID, FirstName, LastName, Email, Phone, PasswordHash)
                                     VALUES (2, 'New', 'Customer', 'example@example.com', '0710000000', 'default_hash')";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("New customer added! Update details in the table.");
                LoadCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding customer: " + ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.CurrentRow == null) return;

            try
            {
                int userId = Convert.ToInt32(dgvCustomers.CurrentRow.Cells["UserID"].Value);
                string firstName = dgvCustomers.CurrentRow.Cells["FirstName"].Value.ToString();
                string lastName = dgvCustomers.CurrentRow.Cells["LastName"].Value.ToString();
                string email = dgvCustomers.CurrentRow.Cells["Email"].Value.ToString();
                string phone = dgvCustomers.CurrentRow.Cells["Phone"].Value.ToString();
                string status = dgvCustomers.CurrentRow.Cells["Status"].Value.ToString();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"UPDATE Users 
                                     SET FirstName=@FirstName, LastName=@LastName, Email=@Email, Phone=@Phone, Status=@Status 
                                     WHERE UserID=@UserID AND RoleID=2";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@LastName", lastName);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Phone", phone);
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Customer updated successfully!");
                LoadCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating customer: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.CurrentRow == null) return;

            try
            {
                int userId = Convert.ToInt32(dgvCustomers.CurrentRow.Cells["UserID"].Value);
                DialogResult result = MessageBox.Show("Are you sure you want to delete this customer?", "Confirm Delete", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "DELETE FROM Users WHERE UserID=@UserID AND RoleID=2";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Customer deleted successfully!");
                    LoadCustomers();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting customer: " + ex.Message);
            }
        }

        // Go back to Admin Dashboard
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.Show();
            this.Close();
        }
    }
}

