using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static GreenLife.CartForm;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ScrollBar;

namespace GreenLife
{
    public partial class CustomerDashboard : Form
    {
        private string connectionString =
           @"Data Source=DESKTOP-RDRJ8S0\SQLEXPRESS;Initial Catalog = GreenLife_Organic; Integrated Security = True; Trust Server Certificate=True;";

        private int currentUserID = 0;
        private string currentFirstName = "";

        private List<CartItem> cartItems = new List<CartItem>();


        public CustomerDashboard(int userId, string firstName)
        {
            InitializeComponent();
            this.currentUserID = userId;
            this.currentFirstName = firstName;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            login loginForm = new login();

            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                currentUserID = loginForm.LoggedInUserID;
                currentFirstName = loginForm.LoggedInFirstName;

                MessageBox.Show("Welcome " + currentFirstName);
            }
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RegisterForm register = new RegisterForm();
            register.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (currentUserID == 0)
            {
                MessageBox.Show("Please login first.");
                return;
            }

            ProductsForm products = new ProductsForm(currentUserID, currentFirstName, cartItems);
            products.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {

            if (currentUserID == 0)
            {
                MessageBox.Show("Please login first.");
                return;
            }

            CartForm cart = new CartForm(currentUserID, cartItems);
            cart.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (currentUserID == 0)
            {
                MessageBox.Show("Please login first.");
                return;
            }

            TrackOrder trackOrder = new TrackOrder(currentUserID);
            trackOrder.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (currentUserID == 0)
            {
                MessageBox.Show("Please login first.");
                return;
            }

            if (cartItems.Count == 0)
            {
                MessageBox.Show("Your cart is empty.");
                return;
            }

            CheckoutForm checkout = new CheckoutForm(currentUserID, cartItems);
            checkout.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (currentUserID == 0)
            {
                MessageBox.Show("Please login first.");
                return;
            }

            if (cartItems.Count == 0)
            {
                MessageBox.Show("Your cart is empty.");
                return;
            }

            CheckoutForm checkout = new CheckoutForm(currentUserID, cartItems);
            checkout.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            currentUserID = 0;
            currentFirstName = "";
            cartItems.Clear();

            MessageBox.Show("Logged out successfully.");
        }
    }
}
