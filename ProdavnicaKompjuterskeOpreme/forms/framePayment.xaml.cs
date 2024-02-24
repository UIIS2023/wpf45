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
    public partial class framePayment : Window
    {
        Connection conn = new Connection();
        SqlConnection connection = new SqlConnection();
        private bool update;
        private DataRowView row;
        public framePayment()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            connection = conn.generateConnection();
            fillComboBox();
        }

        public framePayment(bool update, DataRowView row)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            connection = conn.generateConnection();
            this.update = update;
            this.row = row;
            fillComboBox();
        }

        private void fillComboBox()
        {
            cbPaymentType.Items.Add("Cash");
            cbPaymentType.Items.Add("Card");

            if (row != null)
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand { Connection = connection };
                    string returnPaymentType = @"select nacinPlacanja from prodavnicakompjuterskeopreme.placanje where placanjeID=" + row["ID"];
                    cmd.CommandText = returnPaymentType;

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        string paymentType = result.ToString();
                        int selectedIndex = cbPaymentType.Items.IndexOf(paymentType);
                        if (selectedIndex != -1)
                        {
                            cbPaymentType.SelectedIndex = selectedIndex;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();
                string date = ((DateTime)dpPaymentDate.SelectedDate).ToString("yyyy-MM-dd");
                if (cbPaymentType.SelectedItem == null)
                {
                    throw new Exception("Payment type must be selected!");
                }

                SqlCommand cmd = new SqlCommand { Connection = connection };
                cmd.Parameters.Add("@paymentDate", SqlDbType.DateTime).Value = date;
                
                cmd.Parameters.Add("@paymentType", SqlDbType.NVarChar).Value = cbPaymentType.SelectedItem.ToString();
                if (update)
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = row["ID"];
                    cmd.CommandText = @"update prodavnicakompjuterskeopreme.placanje set datumPlacanja = @paymentDate, nacinPlacanja = @paymentType where placanjeID = @id";

                    row = null;
                }
                else
                {
                    cmd.CommandText = @"insert into prodavnicakompjuterskeopreme.placanje(datumPlacanja, nacinPlacanja) values (@paymentDate, @paymentType)";
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
