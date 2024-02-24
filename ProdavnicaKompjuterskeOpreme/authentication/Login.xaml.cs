using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
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

namespace ProdavnicaKompjuterskeOpreme.authentication
{
    public partial class Login : Window
    {
        public Login()
        { 
            InitializeComponent();
            txtUsername.Focus();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Username and Password are required.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (SqlConnection conn = new Connection().generateConnection())
            {
                try
                {
                    conn.Open();

                    string query = "select count(1) from prodavnicakompjuterskeopreme.nalozi where username = @Username AND password = @Password";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", HashPassword(password));

                    int result = (int)cmd.ExecuteScalar();
                    if (result > 0)
                    {
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();

                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    builder.Append(bytes[i].ToString("x2")); // Convert byte to hex string
                }
                return builder.ToString();
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            var registrationPage = new Registration();
            registrationPage.Show();
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
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
