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
    public partial class frameProduct : Window
    {
        Connection conn = new Connection();
        SqlConnection connection = new SqlConnection();
        private bool update;
        private DataRowView row;
        public frameProduct()
        {
            InitializeComponent();
            txtProductName.Focus();
            connection = conn.generateConnection();
            fillComboBox();
        }

        public frameProduct(bool update, DataRowView row)
        {
            InitializeComponent();
            txtProductName.Focus();
            connection = conn.generateConnection();
            fillComboBox();
            this.update = update;
            this.row = row;
        }

        private void fillComboBox()
        {
            try
            {
                connection.Open();
                string returnCustomers = @"select kupacID, kupacIme + ' ' + kupacPrezime as customer from prodavnicakompjuterskeopreme.kupac";
                SqlDataAdapter daCustomer = new SqlDataAdapter(returnCustomers, connection);
                DataTable dtCustomer = new DataTable();
                daCustomer.Fill(dtCustomer);
                cbCustomer.ItemsSource = dtCustomer.DefaultView;
                daCustomer.Dispose();
                dtCustomer.Dispose();

                string returnBrands = @"select markaID, nazivMarke as brand from prodavnicakompjuterskeopreme.marka";
                SqlDataAdapter daBrand = new SqlDataAdapter(returnBrands, connection);
                DataTable dtBrand = new DataTable();
                daBrand.Fill(dtBrand);
                cbBrand.ItemsSource = dtBrand.DefaultView;
                daBrand.Dispose();
                dtBrand.Dispose();
                
                string returnModels = @"select modelID, nazivModela as mode from prodavnicakompjuterskeopreme.model";
                SqlDataAdapter daModel = new SqlDataAdapter(returnModels, connection);
                DataTable dtModel = new DataTable();
                daModel.Fill(dtModel);
                cbModel.ItemsSource = dtModel.DefaultView;
                daModel.Dispose();
                dtModel.Dispose();

                string returnTypes = @"select tipProizvodaID, nazivTipaProizvoda as type from prodavnicakompjuterskeopreme.tipproizvoda";
                SqlDataAdapter daType = new SqlDataAdapter(returnTypes, connection);
                DataTable dtType = new DataTable();
                daType.Fill(dtType);
                cbProductType.ItemsSource = dtType.DefaultView;
                daType.Dispose();
                dtType.Dispose();

                string returnServis = @"select servisID, opisServisa as details from prodavnicakompjuterskeopreme.servis";
                SqlDataAdapter daServis = new SqlDataAdapter(returnServis, connection);
                DataTable dtServis = new DataTable();
                daServis.Fill(dtServis);
                cbServis.ItemsSource = dtServis.DefaultView;
                daServis.Dispose();
                dtServis.Dispose();

                string returnWarranty = @"select garancijaID, uGaranciji as inwarranty from prodavnicakompjuterskeopreme.garancija";
                SqlDataAdapter daOwner = new SqlDataAdapter(returnWarranty, connection);
                DataTable dtOwner = new DataTable();
                daOwner.Fill(dtOwner);
                cbWarranty.ItemsSource = dtOwner.DefaultView;
                daOwner.Dispose();
                dtOwner.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loading error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally { if (connection != null) connection.Close(); }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();

                if (string.IsNullOrWhiteSpace(txtProductName.Text))
                {
                    throw new Exception("Product name cannot be empty!");
                }

                decimal price;
                if (!decimal.TryParse(txtProductPrice.Text, out price))
                {
                    throw new Exception("Product price must be a number!");
                }

                int quantity;
                if (!int.TryParse(txtProductQuantity.Text, out quantity))
                {
                    throw new Exception("Product quantity must be a number!");
                }

                SqlCommand cmd = new SqlCommand { Connection = connection };
                cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = txtProductName.Text;
                cmd.Parameters.Add("@price", SqlDbType.Decimal).Value = txtProductPrice.Text;
                cmd.Parameters.Add("@quantity", SqlDbType.Int).Value = txtProductQuantity.Text;
                cmd.Parameters.Add("@customerID", SqlDbType.Int).Value = cbCustomer.SelectedValue;
                cmd.Parameters.Add("@brandID", SqlDbType.Int).Value = cbBrand.SelectedValue;
                cmd.Parameters.Add("@modelID", SqlDbType.Int).Value = cbModel.SelectedValue;
                cmd.Parameters.Add("@typeID", SqlDbType.Int).Value = cbProductType.SelectedValue;
                cmd.Parameters.Add("@servisID", SqlDbType.Int).Value = cbServis.SelectedValue;
                cmd.Parameters.Add("@warrantyID", SqlDbType.Int).Value = cbWarranty.SelectedValue;
                if (update)
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = row["ID"];
                    cmd.CommandText = @"update prodavnicakompjuterskeopreme.proizvod set ime = @name, cena = @price, kolicinaNaStanju = @quantity ,
                                        kupacID = @customerID, markaID = @brandID, modelID = @modelID, tipProizvodaID = @typeID,  
                                        servisID = @servisID, garancijaID = @warrantyID where proizvodID = @id";

                    row = null;
                }
                else
                {
                    cmd.CommandText = @"insert into prodavnicakompjuterskeopreme.proizvod(ime, cena, kolicinaNaStanju, kupacID, markaID, modelID, tipProizvodaID, servisID, garancijaID) 
                                        values (@name, @price, @quantity, @customerID, @brandID, @modelID, @typeID, @servisID, @warrantyID)";
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
