using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
    public partial class frameProductType : Window
    {
        Connection conn = new Connection();
        SqlConnection connection = new SqlConnection();
        private bool update;
        private DataRowView row;
        public frameProductType()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            txtTypeName.Focus();
            connection = conn.generateConnection();
        }

        public frameProductType(bool update, DataRowView row)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            txtTypeName.Focus();
            connection = conn.generateConnection();
            this.update = update;
            this.row = row;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();

                if (string.IsNullOrWhiteSpace(txtTypeName.Text))
                {
                    throw new Exception("Product type cannot be empty!");
                }

                SqlCommand cmd = new SqlCommand()
                {
                    Connection = connection
                };
                cmd.Parameters.Add("@productTypeName", SqlDbType.NVarChar).Value = txtTypeName.Text;
                if (update)
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = row["ID"];
                    cmd.CommandText = @"update prodavnicakompjuterskeopreme.tipproizvoda set nazivTipaProizvoda = @productTypeName where tipProizvodaID= @id";
                }
                else
                {
                    cmd.CommandText = @"insert into prodavnicakompjuterskeopreme.tipproizvoda(nazivTipaProizvoda) values (@productTypeName)";
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
