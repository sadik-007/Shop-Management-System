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
    public partial class NewPass : Form
    {
        private string _userName;
        private string connectionString = @"Server=SADIK\SQLEXPRESS;Database=Practice Database;Trusted_Connection=True;";

        public NewPass(string userName)
        {
            InitializeComponent();
            _userName = userName;
            this.Text = "Set New Password for " + _userName;
        }

        private void btnUpdatePassword_Click(object sender, EventArgs e)
        {
            string newPassword = txtNewPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Please fill both password fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Both password should be same.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    string query = "UPDATE Customer SET Password = @NewPassword WHERE User_Name = @UserName";

                    using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                    {
                        sqlCmd.Parameters.AddWithValue("@NewPassword", newPassword);
                        sqlCmd.Parameters.AddWithValue("@UserName", _userName);

                        int rowsAffected = sqlCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Password updated successfully..", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            Form resetForm = Application.OpenForms.OfType<Reset>().FirstOrDefault();
                            if (resetForm != null)
                            {
                                resetForm.Close();
                            }

                            Form1 loginForm = Application.OpenForms.OfType<Form1>().FirstOrDefault();
                            if (loginForm != null)
                            {
                                loginForm.Show();
                            }

                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Password could not be updated. User not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- NEW METHOD that handles BOTH eye buttons ---
        private void togglePasswordVisibility_Click(object sender, EventArgs e)
        {
            // First, figure out which button was clicked.
            Button clickedButton = sender as Button;

            // Check the name of the button to decide which textbox to change.
            if (clickedButton.Name == "btnShowNewPassword")
            {
                // Toggle the 'New Password' textbox
                txtNewPassword.UseSystemPasswordChar = !txtNewPassword.UseSystemPasswordChar;
            }
            else if (clickedButton.Name == "btnShowConfirmPassword")
            {
                // Toggle the 'Confirm Password' textbox
                txtConfirmPassword.UseSystemPasswordChar = !txtConfirmPassword.UseSystemPasswordChar;
            }
        }
    }
}