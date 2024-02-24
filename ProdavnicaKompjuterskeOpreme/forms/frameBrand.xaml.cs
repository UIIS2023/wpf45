using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace ProdavnicaKompjuterskeOpreme.forms
{
    public partial class frameBrand : Window
    {
        Connection conn = new Connection();
        SqlConnection connection = new SqlConnection();
        private bool update;
        private DataRowView row;
        public frameBrand()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            txtBrandName.Focus();
            connection = conn.generateConnection();
        }

        public frameBrand(bool update, DataRowView row)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            txtBrandName.Focus();
            connection = conn.generateConnection();
            this.update = update;
            this.row = row;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();

                if (string.IsNullOrWhiteSpace(txtBrandName.Text))
                {
                    throw new Exception("Brand name cannot be empty!");
                }

                SqlCommand cmd = new SqlCommand()
                {
                    Connection = connection
                };

                cmd.Parameters.Add("@nazivMarke", SqlDbType.NVarChar).Value = txtBrandName.Text;

                if (update)
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = row["ID"];
                    cmd.CommandText = @"update prodavnicakompjuterskeopreme.marka set nazivMarke = @nazivMarke where markaID= @id";
                }
                else
                {
                    cmd.CommandText = @"insert into prodavnicakompjuterskeopreme.marka(nazivMarke) values (@nazivMarke)";
                }

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
