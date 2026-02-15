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
    public partial class TrackOrder : Form
    {

        private string connectionString =
           @"Data Source=DESKTOP-RDRJ8S0\SQLEXPRESS;Initial Catalog = GreenLife_Organic; Integrated Security = True; Trust Server Certificate=True;";

        private int UserID;

        public TrackOrder(int userID)
        {
            InitializeComponent();
            UserID = userID;
            LoadOrders();
        }

        private void LoadOrders()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"SELECT OrderID, OrderDate, PaymentMethod, 
                                            TotalAmount, Status
                                     FROM Orders
                                     WHERE CustomerID = @cid
                                     ORDER BY OrderDate DESC";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@cid", UserID);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvOrders.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading orders: " + ex.Message);
            }
        }

        private void dgvOrders_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvOrders.SelectedRows.Count == 0)
                return;

            int orderId = Convert.ToInt32(
                dgvOrders.SelectedRows[0].Cells["OrderID"].Value);

            MessageBox.Show("Selected Order ID: " + orderId);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadOrders(); // reload orders
        
        }
       

        

        private void button1_Click(object sender, EventArgs e)
        {

        }

        
        
    }
}
