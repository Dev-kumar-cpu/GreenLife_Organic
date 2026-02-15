using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GreenLife
{
    public partial class Reports : Form
    {

        private string connectionString =
            @"Data Source=DESKTOP-RDRJ8S0\SQLEXPRESS;Initial Catalog=GreenLife_Organic;Integrated Security=True;TrustServerCertificate=True;";

        private PrintDocument pd = new PrintDocument();

        public Reports()
        {
            InitializeComponent();
            InitializeComponent();
            pd.PrintPage += new PrintPageEventHandler(PrintPageHandler);
        }

        private void Reports_Load(object sender, EventArgs e)
        {


        }

        private void label1_Click(object sender, EventArgs e)
        {
            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.Show();
            this.Close();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Please select a report type.");
                return;
            }

            if (radioButton1.Checked)
            {
                ExportCSV(); // Pass selected report type if needed
            }
            else if (radioButton2.Checked)
            {
                PrintReport();
            }
            else
            {
                MessageBox.Show("Please select CSV or PDF.");
            }
        }


        private void ExportCSV()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Join Orders and Users to get Customer Names
                    string query = @"
                        SELECT o.OrderID, u.FirstName + ' ' + u.LastName AS CustomerName,
                               o.TotalAmount, o.OrderStatus, o.PaymentStatus, o.OrderDate
                        FROM Orders o
                        INNER JOIN Users u ON o.UserID = u.UserID";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No orders to export.");
                        return;
                    }

                    // Save CSV
                    SaveFileDialog sfd = new SaveFileDialog
                    {
                        Filter = "CSV Files (*.csv)|*.csv",
                        FileName = "OrdersReport.csv"
                    };

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        using (StreamWriter sw = new StreamWriter(sfd.FileName))
                        {
                            // Write header
                            sw.WriteLine(string.Join(",", dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName)));

                            // Write rows
                            foreach (DataRow row in dt.Rows)
                            {
                                sw.WriteLine(string.Join(",", row.ItemArray));
                            }
                        }

                        MessageBox.Show("CSV Exported Successfully!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exporting CSV: " + ex.Message);
            }
        }

        private void PrintReport()
        {
            PrintDialog printDialog = new PrintDialog
            {
                Document = pd
            };

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                pd.Print();
            }
        }

        private void PrintPageHandler(object sender, PrintPageEventArgs e)
        {
            int y = 50;
            Font font = new Font("Arial", 12);

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT o.OrderID, u.FirstName + ' ' + u.LastName AS CustomerName,
                               o.TotalAmount, o.OrderStatus, o.PaymentStatus, o.OrderDate
                        FROM Orders o
                        INNER JOIN Users u ON o.UserID = u.UserID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    e.Graphics.DrawString("Orders Report", new Font("Arial", 14, FontStyle.Bold), Brushes.Black, 50, y);
                    y += 40;

                    while (reader.Read())
                    {
                        string line = $"OrderID: {reader["OrderID"]} | " +
                                      $"Customer: {reader["CustomerName"]} | " +
                                      $"Total: {reader["TotalAmount"]} | " +
                                      $"Order Status: {reader["OrderStatus"]} | " +
                                      $"Payment Status: {reader["PaymentStatus"]} | " +
                                      $"Date: {Convert.ToDateTime(reader["OrderDate"]).ToString("yyyy-MM-dd")}";
                        e.Graphics.DrawString(line, font, Brushes.Black, 50, y);
                        y += 30;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating PDF report: " + ex.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Optional: clear previous results or reset form
            dgvReport.DataSource = null;

            string selectedReport = comboBox1.SelectedItem.ToString();

            switch (selectedReport)
            {
                case "Total Sales":
                    LoadTotalSales();
                    break;

                case "Stock":
                    LoadStockReport();
                    break;

                case "Order History":
                    LoadOrderHistory();
                    break;
            }
        }
        private void LoadTotalSales()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                SELECT u.FirstName + ' ' + u.LastName AS CustomerName,
                       SUM(oi.Quantity * oi.UnitPrice) AS TotalSpent
                FROM Orders o
                INNER JOIN Users u ON o.UserID = u.UserID
                INNER JOIN OrderItems oi ON o.OrderID = oi.OrderID
                GROUP BY u.FirstName, u.LastName";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvReport.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading total sales: " + ex.Message);
            }
        }

        private void LoadStockReport()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                SELECT p.ProductName, c.CategoryName, p.StockQuantity, p.Price
                FROM Products p
                INNER JOIN Categories c ON p.CategoryID = c.CategoryID";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvReport.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading stock report: " + ex.Message);
            }
        }

        private void LoadOrderHistory()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                SELECT o.OrderID, u.FirstName + ' ' + u.LastName AS CustomerName,
                       o.TotalAmount, o.OrderStatus, o.PaymentStatus, o.OrderDate
                FROM Orders o
                INNER JOIN Users u ON o.UserID = u.UserID";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvReport.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading order history: " + ex.Message);
            }
        }




    }
}
