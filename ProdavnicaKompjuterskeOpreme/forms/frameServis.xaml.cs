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
    public partial class frameServis : Window
    {
        Connection conn = new Connection();
        SqlConnection connection = new SqlConnection();
        private bool update;
        private DataRowView row;
        public frameServis()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            txtServisDetails.Focus();
            connection = conn.generateConnection();
        }

        public frameServis(bool update, DataRowView row)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            txtServisDetails.Focus();
            connection = conn.generateConnection();
            this.update = update;
            this.row = row;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();

                if (string.IsNullOrWhiteSpace(txtServisDetails.Text))
                {
                    throw new Exception("Servis details cannot be empty!");
                }

                string date = ((DateTime)dpServisDate.SelectedDate).ToString("yyyy-MM-dd");
                SqlCommand cmd = new SqlCommand { Connection = connection };
                cmd.Parameters.Add("@servisDetails", SqlDbType.NVarChar).Value = txtServisDetails.Text;
                cmd.Parameters.Add("@servisDate", SqlDbType.DateTime).Value = date;
                if (update)
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = row["ID"];
                    cmd.CommandText = @"update prodavnicakompjuterskeopreme.servis set opisServisa = @servisDetails, datumServisa = @servisDate where servisID = @id";

                    row = null;
                }
                else
                {
                    cmd.CommandText = @"insert into prodavnicakompjuterskeopreme.servis(opisServisa, datumServisa) values (@servisDetails, @servisDate)";
                }
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                this.Close();
            }
            catch (SqlException)
            {
                MessageBox.Show("Some input values aren't valid!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (FormatException)
            {
                MessageBox.Show("There was an error while converting data!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Date wasn't selected!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
