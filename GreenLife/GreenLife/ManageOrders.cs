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
    public partial class ManageOrders : Form
    {
        private string connectionString =
           @"Data Source=DESKTOP-RDRJ8S0\SQLEXPRESS;Initial Catalog=GreenLife_Organic;Integrated Security=True;TrustServerCertificate=True;";
        public ManageOrders()
        {
            InitializeComponent();
            LoadOrders();
        }

        private void LoadOrders()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                SELECT 
                    o.OrderID,
                    u.FirstName + ' ' + u.LastName AS CustomerName,
                    o.TotalAmount,
                    o.OrderStatus,
                    p.PaymentMethod,
                    o.OrderDate
                FROM Orders o
                INNER JOIN Users u ON o.UserID = u.UserID
                LEFT JOIN Payments p ON o.OrderID = p.OrderID
                ORDER BY o.OrderDate DESC";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvOrderDetails.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
            }
        }


        private void ManageOrdersForm_Load(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.ShowDialog();
            this.Close();
        }

        private void btnUpdateStatus_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Update Order Status clicked");
        }

        private void btnViewItems_Click(object sender, EventArgs e)
        {
            if (dgvOrderDetails.CurrentRow != null)
            {
                int orderId = Convert.ToInt32(dgvOrderDetails.CurrentRow.Cells["OrderID"].Value);

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        string query = @"
                    SELECT 
                        p.ProductName,
                        oi.Quantity,
                        oi.UnitPrice,
                        (oi.Quantity * oi.UnitPrice) AS TotalPrice
                    FROM OrderItems oi
                    INNER JOIN Products p ON oi.ProductID = p.ProductID
                    WHERE oi.OrderID = @OrderID";

                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@OrderID", orderId);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        dgvOrderDetails.DataSource = dt;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database Error: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please select an order to view its items.");
            }
        }


        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dgvOrders_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}