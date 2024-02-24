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
    public partial class frameModel : Window
    {
        Connection conn = new Connection();
        SqlConnection connection = new SqlConnection();
        private bool update;
        private DataRowView row;
        public frameModel()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            txtModelName.Focus();
            connection = conn.generateConnection();
        }

        public frameModel(bool update, DataRowView row)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            txtModelName.Focus();
            connection = conn.generateConnection();
            this.update = update;
            this.row = row;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();

                if (string.IsNullOrWhiteSpace(txtModelName.Text))
                {
                    throw new Exception("Model name cannot be empty!");
                }

                SqlCommand cmd = new SqlCommand()
                {
                    Connection = connection
                };
                cmd.Parameters.Add("@modelName", SqlDbType.NVarChar).Value = txtModelName.Text;
                if (update)
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = row["ID"];
                    cmd.CommandText = @"update prodavnicakompjuterskeopreme.model set nazivModela = @modelName where modelID= @id";
                }
                else
                {
                    cmd.CommandText = @"insert into prodavnicakompjuterskeopreme.model(nazivModela) values (@modelName)";
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
