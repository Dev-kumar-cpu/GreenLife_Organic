namespace GreenLife
{
    partial class AdminDashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnReports = new System.Windows.Forms.Button();
            this.btnbtnCurrentOrders = new System.Windows.Forms.Button();
            this.btnManageCustomers = new System.Windows.Forms.Button();
            this.btnManageProducts = new System.Windows.Forms.Button();
            this.btnStocks = new System.Windows.Forms.Button();
            this.btnTotalSales = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnManageOrders = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnReports
            // 
            this.btnReports.Location = new System.Drawing.Point(169, 115);
            this.btnReports.Name = "btnReports";
            this.btnReports.Size = new System.Drawing.Size(118, 54);
            this.btnReports.TabIndex = 11;
            this.btnReports.Text = "Reports";
            this.btnReports.UseVisualStyleBackColor = true;
            this.btnReports.Click += new System.EventHandler(this.btnReports_Click);
            // 
            // btnbtnCurrentOrders
            // 
            this.btnbtnCurrentOrders.Location = new System.Drawing.Point(47, 115);
            this.btnbtnCurrentOrders.Name = "btnbtnCurrentOrders";
            this.btnbtnCurrentOrders.Size = new System.Drawing.Size(118, 54);
            this.btnbtnCurrentOrders.TabIndex = 10;
            this.btnbtnCurrentOrders.Text = "Current Orders";
            this.btnbtnCurrentOrders.UseVisualStyleBackColor = true;
            this.btnbtnCurrentOrders.Click += new System.EventHandler(this.btnbtnCurrentOrders_Click);
            // 
            // btnManageCustomers
            // 
            this.btnManageCustomers.Location = new System.Drawing.Point(169, 175);
            this.btnManageCustomers.Name = "btnManageCustomers";
            this.btnManageCustomers.Size = new System.Drawing.Size(118, 54);
            this.btnManageCustomers.TabIndex = 9;
            this.btnManageCustomers.Text = "Manage Customers";
            this.btnManageCustomers.UseVisualStyleBackColor = true;
            this.btnManageCustomers.Click += new System.EventHandler(this.btnManageCustomers_Click);
            // 
            // btnManageProducts
            // 
            this.btnManageProducts.Location = new System.Drawing.Point(48, 175);
            this.btnManageProducts.Name = "btnManageProducts";
            this.btnManageProducts.Size = new System.Drawing.Size(118, 54);
            this.btnManageProducts.TabIndex = 8;
            this.btnManageProducts.Text = "Manage Products";
            this.btnManageProducts.UseVisualStyleBackColor = true;
            this.btnManageProducts.Click += new System.EventHandler(this.btnManageProducts_Click);
            // 
            // btnStocks
            // 
            this.btnStocks.Location = new System.Drawing.Point(169, 55);
            this.btnStocks.Name = "btnStocks";
            this.btnStocks.Size = new System.Drawing.Size(118, 54);
            this.btnStocks.TabIndex = 15;
            this.btnStocks.Text = "Stocks ";
            this.btnStocks.UseVisualStyleBackColor = true;
            this.btnStocks.Click += new System.EventHandler(this.btnStocks_Click);
            // 
            // btnTotalSales
            // 
            this.btnTotalSales.Location = new System.Drawing.Point(47, 55);
            this.btnTotalSales.Name = "btnTotalSales";
            this.btnTotalSales.Size = new System.Drawing.Size(118, 54);
            this.btnTotalSales.TabIndex = 14;
            this.btnTotalSales.Text = "Total Sales";
            this.btnTotalSales.UseVisualStyleBackColor = true;
            this.btnTotalSales.Click += new System.EventHandler(this.btnTotalSales_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(169, 235);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(118, 54);
            this.btnLogout.TabIndex = 17;
            this.btnLogout.Text = "Logout ";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnManageOrders
            // 
            this.btnManageOrders.Location = new System.Drawing.Point(47, 235);
            this.btnManageOrders.Name = "btnManageOrders";
            this.btnManageOrders.Size = new System.Drawing.Size(118, 54);
            this.btnManageOrders.TabIndex = 16;
            this.btnManageOrders.Text = "Manage Orders";
            this.btnManageOrders.UseVisualStyleBackColor = true;
            this.btnManageOrders.Click += new System.EventHandler(this.btnManageOrders_Click);
            // 
            // AdminDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(313, 341);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnManageOrders);
            this.Controls.Add(this.btnStocks);
            this.Controls.Add(this.btnTotalSales);
            this.Controls.Add(this.btnReports);
            this.Controls.Add(this.btnbtnCurrentOrders);
            this.Controls.Add(this.btnManageCustomers);
            this.Controls.Add(this.btnManageProducts);
            this.Name = "AdminDashboard";
            this.Text = "AdminDashboard";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnReports;
        private System.Windows.Forms.Button btnbtnCurrentOrders;
        private System.Windows.Forms.Button btnManageCustomers;
        private System.Windows.Forms.Button btnManageProducts;
        private System.Windows.Forms.Button btnStocks;
        private System.Windows.Forms.Button btnTotalSales;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnManageOrders;
    }
}