using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace ProdavnicaKompjuterskeOpreme.authentication
{
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
            txtRegUsername.Focus();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string username = txtRegUsername.Text.Trim();
            string password = txtRegPassword.Password.Trim();
            string confirmPassword = txtConfirmPassword.Password.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("All fields are required.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (SqlConnection conn = new Connection().generateConnection())
            {
                try
                {
                    conn.Open();

                    string query = "insert into prodavnicakompjuterskeopreme.nalozi (username, password) values (@Username, @Password)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", HashPassword(password));

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Registration successful.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        var loginPage = new Login();
                        loginPage.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Registration failed.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            var loginPage = new Login();
            loginPage.Show();
            this.Close();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        public void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
