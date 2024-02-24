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

    public partial class frameCustomer: Window
    {
        Connection conn = new Connection();
        SqlConnection connection = new SqlConnection();
        private bool update;
        private DataRowView row;
        public frameCustomer()
        {
            InitializeComponent();
            txtFirstName.Focus();
            connection = conn.generateConnection();
        }

        public frameCustomer(bool update, DataRowView row)
        {
            InitializeComponent();
            txtFirstName.Focus();
            connection = conn.generateConnection();
            this.update = update;
            this.row = row;
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();

                if (string.IsNullOrWhiteSpace(txtFirstName.Text) || string.IsNullOrEmpty(txtLastName.Text) || 
                    string.IsNullOrEmpty(txtPhoneNumber.Text) || string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtAddress.Text))
                {
                    throw new Exception("Fields cannot be empty!");
                }

                SqlCommand cmd = new SqlCommand { Connection = connection };
                cmd.Parameters.Add("@firstName", SqlDbType.NVarChar).Value = txtFirstName.Text;
                cmd.Parameters.Add("@lastName", SqlDbType.NVarChar).Value = txtLastName.Text;
                cmd.Parameters.Add("@phoneNumber", SqlDbType.NVarChar).Value = txtPhoneNumber.Text;
                cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = txtEmail.Text;
                cmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = txtAddress.Text;
                if (update)
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = row["ID"];
                    cmd.CommandText = @"update prodavnicakompjuterskeopreme.kupac set kupacIme = @firstName, kupacPrezime = @lastName, 
                                        brojTelefona = @phoneNumber, email = @email, adresaSlanja = @address where kupacID = @id";

                    row = null;
                }
                else
                {
                    cmd.CommandText = @"insert into prodavnicakompjuterskeopreme.kupac(kupacIme, kupacPrezime, brojTelefona, email, adresaSlanja) 
                                        values (@firstName, @lastName, @phoneNumber, @email, @address)";
                }
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                this.Close();
            }
            catch (SqlException)
            {
                MessageBox.Show("Some input values aren't valid!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
