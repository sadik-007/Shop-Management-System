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
    public partial class Form2 : Form
    {
        private string connectionString = @"Server=SADIK\SQLEXPRESS;Database=Practice Database;Trusted_Connection=True;";

        public Form2()
        {
            InitializeComponent();
        }

        private void linkBackToLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form1 loginForm = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            if (loginForm != null)
            {
                loginForm.Show();
            }
            this.Close();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            // The logic for the register button remains the same as before.
            // It provides a final, secure check before inserting the data.
            string fullName = txtFullName.Text;
            string userName = txtUserName.Text;
            string address = txtAddress.Text;
            string gmail = txtGmail.Text;
            string contact = txtContact.Text;
            string ageText = txtAge.Text;
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(address) ||
                string.IsNullOrEmpty(gmail) || string.IsNullOrEmpty(contact) || string.IsNullOrEmpty(ageText) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Please fill out all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(ageText, out int age))
            {
                MessageBox.Show("Please enter a valid number for Age.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();

                    string checkUserQuery = "SELECT COUNT(1) FROM Customer WHERE User_Name = @UserName";
                    using (SqlCommand checkUserCmd = new SqlCommand(checkUserQuery, sqlCon))
                    {
                        checkUserCmd.Parameters.AddWithValue("@UserName", userName);
                        int userCount = (int)checkUserCmd.ExecuteScalar();

                        if (userCount > 0)
                        {
                            MessageBox.Show("This User Name is already taken. Please choose another.", "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    string insertQuery = "INSERT INTO Customer (Full_Name, User_Name, Address, Gmail, Contact_Number, Age, Password) " +
                                         "VALUES (@FullName, @UserName, @Address, @Gmail, @Contact, @Age, @Password)";

                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, sqlCon))
                    {
                        insertCmd.Parameters.AddWithValue("@FullName", fullName);
                        insertCmd.Parameters.AddWithValue("@UserName", userName);
                        insertCmd.Parameters.AddWithValue("@Address", address);
                        insertCmd.Parameters.AddWithValue("@Gmail", gmail);
                        insertCmd.Parameters.AddWithValue("@Contact", contact);
                        insertCmd.Parameters.AddWithValue("@Age", age);
                        insertCmd.Parameters.AddWithValue("@Password", password);

                        insertCmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Registration Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    linkBackToLogin_LinkClicked(sender, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred during registration: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- NEW METHOD FOR REAL-TIME USERNAME CHECK ---
        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;

            // If the textbox is empty, clear the status label
            if (string.IsNullOrWhiteSpace(userName))
            {
                lblUserNameStatus.Text = "";
                return;
            }

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    string checkUserQuery = "SELECT COUNT(1) FROM Customer WHERE User_Name = @UserName";
                    using (SqlCommand checkUserCmd = new SqlCommand(checkUserQuery, sqlCon))
                    {
                        checkUserCmd.Parameters.AddWithValue("@UserName", userName);
                        int userCount = (int)checkUserCmd.ExecuteScalar();

                        if (userCount > 0)
                        {
                            // Username exists
                            lblUserNameStatus.Text = "User name is taken";
                            lblUserNameStatus.ForeColor = Color.Red;
                        }
                        else
                        {
                            // Username is available
                            lblUserNameStatus.Text = "User name is available";
                            lblUserNameStatus.ForeColor = Color.Green;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // In case of a database error, you can show a generic message
                lblUserNameStatus.Text = "Error checking username";
                lblUserNameStatus.ForeColor = Color.OrangeRed;
                // You might want to log the full error: Console.WriteLine(ex.Message);
            }
        }
    }
}