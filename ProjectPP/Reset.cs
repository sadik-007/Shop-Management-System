using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; // <-- Make sure this is added

namespace ProjectPP
{
    public partial class Reset : Form
    {
        private string connectionString = @"Server=SADIK\SQLEXPRESS;Database=Practice Database;Trusted_Connection=True;";

        public Reset()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Get the user input
            string userName = richTextBox2.Text; // User Name from the form
            string gmail = richTextBox1.Text;    // Gmail from the form

            // Basic validation
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(gmail))
            {
                MessageBox.Show("Please enter both User Name and Gmail.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();

                    // Query to check if the username and gmail combination exists
                    string query = "SELECT COUNT(1) FROM Customer WHERE User_Name = @UserName AND Gmail = @Gmail";

                    using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                    {
                        sqlCmd.Parameters.AddWithValue("@UserName", userName);
                        sqlCmd.Parameters.AddWithValue("@Gmail", gmail);

                        int count = (int)sqlCmd.ExecuteScalar();

                        // If count is 1, a user was found with that username and gmail
                        if (count == 1)
                        {
                            // Match found, open the NewPass form
                            // We pass the username to the NewPass form so it knows who to update.
                            NewPass newPassForm = new NewPass(userName);
                            newPassForm.Show();
                            this.Hide(); // Hide the current Reset form
                        }
                        else
                        {
                            // No match found
                            MessageBox.Show("User Name and/or Gmail not found. Please try again.", "Validation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}