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
    public partial class login : Form
    {

        private string connectionString =
       @"Data Source=DESKTOP-RDRJ8S0\SQLEXPRESS;Initial Catalog=GreenLife_Organic; Integrated Security=True; Trust Server Certificate=True;";

        public int LoggedInUserID { get; private set; }
        public string LoggedInFirstName { get; private set; }



        public login()
        {
            InitializeComponent();
           


        }
        

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
          
                if (txtEmail.Text.Trim() == "" || txtPassword.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter Email and Password.");
                    return;
                }

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        string query = @"SELECT UserID, FirstName 
                                 FROM Users
                                 WHERE Email=@Email AND PasswordHash=@Password";

                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());

                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            LoggedInUserID = Convert.ToInt32(reader["UserID"]);
                            LoggedInFirstName = reader["FirstName"].ToString();

                            MessageBox.Show("Login Successful!");

                        if (radioButton1.Checked)
                        {
                            CustomerDashboard userDashboard = new CustomerDashboard(LoggedInUserID, LoggedInFirstName);
                            userDashboard.Show();
                        }
                        else if (radioButton2.Checked)
                        {
                            AdminDashboard adminDashboard = new AdminDashboard();
                            adminDashboard.Show();
                        }
                        else
                        {
                            MessageBox.Show("Please select a role.");
                            return;
                        }

                        this.Hide();
                    }
                        else
                        {
                            MessageBox.Show("Invalid email or password.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database Error: " + ex.Message);
                }
            
        }

             

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !checkBox1.Checked;
        }

        private void login_Load(object sender, EventArgs e)
        {

        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            RegisterForm register = new RegisterForm();
            register.Show();
            this.Hide();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
