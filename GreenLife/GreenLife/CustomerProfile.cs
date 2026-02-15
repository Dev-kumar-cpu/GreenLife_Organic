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
using System.Xml.Linq;

namespace GreenLife
{
    public partial class CustomerProfile : Form
    {

        private string connectionString =
           @"Data Source=DESKTOP-RDRJ8S0\SQLEXPRESS;Initial Catalog = GreenLife_Organic; Integrated Security = True; Trust Server Certificate=True;";

        private int currentUserId;

        public CustomerProfile(int UserID)
        {
            InitializeComponent();
            currentUserId = UserID;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtFirstName.Text.Trim() == "" || txtEmail.Text.Trim() == "")
            {
                MessageBox.Show("First Name and Email cannot be empty.");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // 1️⃣ Update Users table
                    string userQuery = @"UPDATE Users 
                                 SET FirstName = @fn,
                                     LastName = @ln,
                                     Email = @em,
                                     Phone = @ph
                                 WHERE UserID = @id";

                    SqlCommand userCmd = new SqlCommand(userQuery, conn);
                    userCmd.Parameters.AddWithValue("@fn", txtFirstName.Text.Trim());
                    userCmd.Parameters.AddWithValue("@ln", txtLastName.Text.Trim());
                    userCmd.Parameters.AddWithValue("@em", txtEmail.Text.Trim());
                    userCmd.Parameters.AddWithValue("@ph", txtPhone.Text.Trim());
                    userCmd.Parameters.AddWithValue("@id", currentUserId);

                    userCmd.ExecuteNonQuery();


                    // 2️⃣ Update Address table
                    string addressQuery = @"UPDATE Addresses 
                                    SET AddressLine1 = @addr
                                    WHERE UserID = @id";

                    SqlCommand addressCmd = new SqlCommand(addressQuery, conn);
                    addressCmd.Parameters.AddWithValue("@addr", txtAddress.Text.Trim());
                    addressCmd.Parameters.AddWithValue("@id", currentUserId);
                    addressCmd.ExecuteNonQuery();

                    MessageBox.Show("Profile Updated Successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
    }
}
