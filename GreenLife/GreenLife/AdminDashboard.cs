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
    public partial class AdminDashboard : Form
    {
        private string connectionString =
           @"Data Source=DESKTOP-RDRJ8S0\SQLEXPRESS;Initial Catalog = GreenLife_Organic; Integrated Security = True; Trust Server Certificate=True;";

        public AdminDashboard()
        {
            InitializeComponent();
            LoadDashboardData();
        }


        private void LoadDashboardData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // 1️⃣ Total Sales
                    string totalSalesQuery = "SELECT ISNULL(SUM(TotalAmount),0) FROM Orders";
                    SqlCommand cmdSales = new SqlCommand(totalSalesQuery, conn);
                    decimal totalSales = Convert.ToDecimal(cmdSales.ExecuteScalar());
                    btnTotalSales.Text = "Total Sales: " + totalSales.ToString("C");

                    // 2️⃣ Current Orders
                    string currentOrdersQuery = "SELECT COUNT(*) FROM Orders WHERE Status = 'Pending'";
                    SqlCommand cmdOrders = new SqlCommand(currentOrdersQuery, conn);
                    int currentOrders = Convert.ToInt32(cmdOrders.ExecuteScalar());
                    btnbtnCurrentOrders.Text = "Current Orders: " + currentOrders;

                    // 3️⃣ Low Stock Products
                    string lowStockQuery = "SELECT COUNT(*) FROM Products WHERE Stock < 10";
                    SqlCommand cmdStock = new SqlCommand(lowStockQuery, conn);
                    int lowStockCount = Convert.ToInt32(cmdStock.ExecuteScalar());
                    btnStocks.Text = "Low Stock: " + lowStockCount;

                    // Optional: Show alert if any product is low
                    if (lowStockCount > 0)
                        MessageBox.Show("⚠ Some products are low in stock! Please restock.", "Low Stock Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading dashboard: " + ex.Message);
            }
        }

        private void AdminDashboard_Load(object sender, EventArgs e)
        {
            // Optional: call here if not called in constructor
            // LoadDashboardData();
        }


        private void CheckLowStock()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Name, Stock FROM Products WHERE Stock < 10";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    MessageBox.Show("⚠  products are low in stock! Please   check and restock.");
                }
            }
        }
        private void btnTotalSales_Click(object sender, EventArgs e)
        {

        }

        private void btnStocks_Click(object sender, EventArgs e)
        {

        }

        private void btnbtnCurrentOrders_Click(object sender, EventArgs e)
        {

        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            Reports reports = new Reports();
            reports.ShowDialog();
        }

        private void btnManageProducts_Click(object sender, EventArgs e)
        {
            ManageProducts products = new ManageProducts();
            products.ShowDialog();
        }

        private void btnManageCustomers_Click(object sender, EventArgs e)
        {
            ManageCustomers customers = new ManageCustomers();
            customers.ShowDialog();
        }

        private void btnManageOrders_Click(object sender, EventArgs e)
        {
            ManageOrders orders = new ManageOrders();
            orders.ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
