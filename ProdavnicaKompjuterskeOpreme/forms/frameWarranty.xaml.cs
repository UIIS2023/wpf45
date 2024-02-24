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
    public partial class frameWarranty : Window
    {
        Connection conn = new Connection();
        SqlConnection connection = new SqlConnection();
        private bool update;
        private DataRowView row;
        private bool? inWarranty;
        public frameWarranty()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            txtWarrantyLength.Focus();
            connection = conn.generateConnection();
            dpWarrantyStart.SelectedDateChanged += UpdateInWarranty;
            txtWarrantyLength.TextChanged += UpdateInWarranty;
        }

        public frameWarranty(bool update, DataRowView row)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            txtWarrantyLength.Focus();
            connection = conn.generateConnection();
            dpWarrantyStart.SelectedDateChanged += UpdateInWarranty;
            txtWarrantyLength.TextChanged += UpdateInWarranty;
            this.update = update;
            this.row = row;
        }

        private void UpdateInWarranty(object sender, EventArgs e)
        {
            if (dpWarrantyStart.SelectedDate.HasValue && !string.IsNullOrEmpty(txtWarrantyLength.Text))
            {
                try
                {
                    string date = ((DateTime)dpWarrantyStart.SelectedDate).ToString("yyyy-MM-dd");
                    inWarranty = DateTime.Now < DateTime.Parse(date).AddMonths(int.Parse(txtWarrantyLength.Text));

                    cboxInWarranty.IsChecked = inWarranty;
                }
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Date wasn't selected!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                cboxInWarranty.IsChecked = null;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();

                if (string.IsNullOrWhiteSpace(txtWarrantyLength.Text))
                {
                    throw new Exception("Warranty length cannot be empty!");
                }

                string date = ((DateTime)dpWarrantyStart.SelectedDate).ToString("yyyy-MM-dd");
                bool inWarranty = DateTime.Now < DateTime.Parse(date).AddMonths(int.Parse(txtWarrantyLength.Text));
                SqlCommand cmd = new SqlCommand { Connection = connection };
                cmd.Parameters.Add("@warrantyStart", SqlDbType.DateTime).Value = date;
                cmd.Parameters.Add("@warrantyLength", SqlDbType.NVarChar).Value = txtWarrantyLength.Text;
                cmd.Parameters.Add("@inWarranty", SqlDbType.Bit).Value = Convert.ToInt32(inWarranty);
                if (update)
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = row["ID"];
                    cmd.CommandText = @"update prodavnicakompjuterskeopreme.garancija set pocetakGarancije = @warrantyStart, duzinaGarancije = @warrantyLength, uGaranciji = @inWarranty where garancijaID = @id";

                    row = null;
                }
                else
                {
                    cmd.CommandText = @"insert into prodavnicakompjuterskeopreme.garancija(pocetakGarancije, duzinaGarancije, uGaranciji) values (@warrantyStart, @warrantyLength, @inWarranty)";
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
