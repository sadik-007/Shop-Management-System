using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ProjectPP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // You can add any code here that needs to run when the form first opens.
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Reset resetForm = new Reset();
            resetForm.Show();
            this.Hide();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form2 registrationForm = new Form2();
            registrationForm.Show();
            this.Hide();
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            // --- 1. Get user input and perform basic validation ---
            string username = richTextBox1.Text;
            string password = richTextBox2.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Stop further execution
            }

            // --- 2. Define the connection string with YOUR server details ---
            string connectionString = @"Server=SADIK\SQLEXPRESS;Database=Practice Database;Trusted_Connection=True;";

            // --- 3. Use a try-catch block for database operations ---
            try
            {
                // The 'using' statement ensures the connection is automatically closed even if errors occur.
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();

                    // --- 4. Create a SECURE parameterized query with CORRECT column names ---
                    // Using "User_Name" as you specified.
                    string query = "SELECT COUNT(1) FROM Customer WHERE User_Name=@Username AND Password=@Password";

                    using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                    {
                        // Add parameters to the command
                        sqlCmd.Parameters.AddWithValue("@Username", username);
                        sqlCmd.Parameters.AddWithValue("@Password", password);

                        // --- 5. Execute the query and get the result ---
                        int count = Convert.ToInt32(sqlCmd.ExecuteScalar());

                        // --- 6. Check if the user was found ---
                        if (count == 1)
                        {
                            MessageBox.Show("Login Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            // TODO: Open the main application form here
                        }
                        else
                        {
                            MessageBox.Show("Invalid Username or Password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Catch and display any exceptions during the process
                MessageBox.Show("An error occurred: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}