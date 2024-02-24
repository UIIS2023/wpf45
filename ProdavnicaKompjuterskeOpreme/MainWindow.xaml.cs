using ProdavnicaKompjuterskeOpreme.authentication;
using ProdavnicaKompjuterskeOpreme.forms;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProdavnicaKompjuterskeOpreme
{
    public partial class MainWindow : Window
    {
        Connection conn = new Connection();
        SqlConnection connection = new SqlConnection();
        private bool update;
        private DataRowView? row;
        private string currentTable;
        #region Select queries
        private static string productSelect = @"select proizvodID as ID, ime as Name, cena as Price, kolicinaNaStanju as Quantity, kupacIme + ' ' + kupacPrezime as Customer, 
                                                nazivMarke as Brand, nazivModela as Model, nazivTipaProizvoda as Type, datumServisa as 'Servis date', duzinaGarancije as 'Warranty length' from prodavnicakompjuterskeopreme.proizvod
                                                join prodavnicakompjuterskeopreme.kupac on proizvod.kupacID = kupac.kupacID
                                                join prodavnicakompjuterskeopreme.marka on proizvod.markaID = marka.markaID
                                                join prodavnicakompjuterskeopreme.model on proizvod.modelID = model.modelID
                                                join prodavnicakompjuterskeopreme.tipproizvoda on proizvod.tipProizvodaID = tipproizvoda.tipProizvodaID
                                                join prodavnicakompjuterskeopreme.servis on proizvod.servisID = servis.servisID
                                                join prodavnicakompjuterskeopreme.garancija on proizvod.garancijaID = garancija.garancijaID;";
        private static string brandSelect = @"select markaID as ID, nazivMarke as Marka from prodavnicakompjuterskeopreme.marka";
        private static string modelSelect = @"select modelID as ID, nazivModela as Model from prodavnicakompjuterskeopreme.model";
        private static string typeSelect = @"select tipProizvodaID as ID, nazivTipaProizvoda as Tip from prodavnicakompjuterskeopreme.tipproizvoda";
        private static string customerSelect = @"select kupacID as ID, kupacIme as Ime, kupacPrezime as Prezime, brojTelefona as Telefon, email, adresaSlanja Adresa 
                                                from prodavnicakompjuterskeopreme.kupac";
        private static string paymentSelect = @"select placanjeID as ID, datumPlacanja as 'Payment date', nacinPlacanja as 'Payment type' from prodavnicakompjuterskeopreme.placanje";
        private static string warrantySelect = @"select garancijaID as ID, pocetakGarancije as 'Warranty start', duzinaGarancije as 'Warranty length', case when uGaranciji = 1 then 'True' else 'False' end as 'In warranty' from prodavnicakompjuterskeopreme.garancija"; 
        private static string servisSelect = @"select servisID as ID, opisServisa as 'Servis details', datumServisa as 'Date of servis' from prodavnicakompjuterskeopreme.servis";
        #endregion
        #region Select with statements
        private static string selectStatementProduct = @"select * from prodavnicakompjuterskeopreme.proizvod where proizvodID=";
        private static string selectStatementBrand = @"select * from prodavnicakompjuterskeopreme.marka where markaID=";
        private static string selectStatementModel= @"select * from prodavnicakompjuterskeopreme.model where modelID=";
        private static string selectStatementType = @"select * from prodavnicakompjuterskeopreme.tipProizvoda where tipProizvodaID=";
        private static string selectStatementCustomer = @"select * from prodavnicakompjuterskeopreme.kupac where kupacID=";
        private static string selectStatementPayment = @"select * from prodavnicakompjuterskeopreme.placanje where placanjeID=";
        private static string selectStatementWarranty = @"select * from prodavnicakompjuterskeopreme.garancija where garancijaID=";
        private static string selectStatementServis = @"select * from prodavnicakompjuterskeopreme.servis where servisID=";
        #endregion
        #region Delete queries
        private static string deleteProduct = @"delete from prodavnicakompjuterskeopreme.proizvod where proizvodID=";
        private static string deleteBrand = @"delete from prodavnicakompjuterskeopreme.marka where markaID=";
        private static string deleteModel = @"delete from prodavnicakompjuterskeopreme.model where modelID=";
        private static string deleteType = @"delete from prodavnicakompjuterskeopreme.tipProizvoda where tipProizvodaID=";
        private static string deleteCustomer = @"delete from prodavnicakompjuterskeopreme.kupac where kupacID=";
        private static string deletePayment = @"delete from prodavnicakompjuterskeopreme.placanje where placanjeID=";
        private static string deleteWarranty = @"delete from prodavnicakompjuterskeopreme.garancija where garancijaID=";
        private static string deleteServis = @"delete from prodavnicakompjuterskeopreme.servis where servisID=";

        #endregion
        public MainWindow()
        {
            InitializeComponent();
            connection = conn.generateConnection();
            loadData(productSelect);
        }

        private void loadData(string selectString)
        {
            try
            {
                connection.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(selectString, connection);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                if (dataGridCenter != null)
                {
                    dataGridCenter.ItemsSource = dataTable.DefaultView;
                }
                currentTable = selectString;
                dataAdapter.Dispose();
                dataTable.Dispose();
            }
            catch (SqlException)
            {
                MessageBox.Show("Error while loading data!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }

        private void btnProducts_Click(object sender, RoutedEventArgs e)
        {
            loadData(productSelect);
        }

        private void btnCustomer_Click(object sender, RoutedEventArgs e)
        {
            loadData(customerSelect);
        }

        private void btnBrands_Click(object sender, RoutedEventArgs e)
        {
            loadData(brandSelect);
        }

        private void btnModels_Click(object sender, RoutedEventArgs e)
        {
            loadData(modelSelect);
        }

        private void btnType_Click(object sender, RoutedEventArgs e)
        {
            loadData(typeSelect);
        }

        private void btnPayment_Click(object sender, RoutedEventArgs e)
        {
            loadData(paymentSelect);
        }

        private void btnWarranty_Click(object sender, RoutedEventArgs e)
        {
            loadData(warrantySelect);
        }

        private void btnServis_Click(object sender, RoutedEventArgs e)
        {
            loadData(servisSelect);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {            
            Window form;
            if (currentTable.Equals(productSelect))
            {
                form = new frameProduct();
                form.ShowDialog();
                loadData(productSelect);
            }
            else if (currentTable.Equals(customerSelect))
            {
                form = new frameCustomer();
                form.ShowDialog();
                loadData(customerSelect);
            }
            else if (currentTable.Equals(brandSelect))
            {
                form = new frameBrand();
                form.ShowDialog();
                loadData(brandSelect);
            }
            else if (currentTable.Equals(modelSelect))
            {
                form = new frameModel();
                form.ShowDialog();
                loadData(modelSelect);
            }
            else if (currentTable.Equals(typeSelect))
            {
                form = new frameProductType();
                form.ShowDialog();
                loadData(typeSelect);
            }
            else if (currentTable.Equals(paymentSelect))
            {
                form = new framePayment();
                form.ShowDialog();
                loadData(paymentSelect);
            }            
            else if (currentTable.Equals(warrantySelect))
            {
                form = new frameWarranty();
                form.ShowDialog();
                loadData(warrantySelect);
            }            
            else if (currentTable.Equals(servisSelect))
            {
                form = new frameServis();
                form.ShowDialog();
                loadData(servisSelect);
            }
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            if (currentTable.Equals(productSelect))
            {
                FillFrame(selectStatementProduct);
                loadData(productSelect);
            }
            else if (currentTable.Equals(customerSelect))
            {
                FillFrame(selectStatementCustomer);
                loadData(customerSelect);
            }
            else if (currentTable.Equals(brandSelect))
            {
                FillFrame(selectStatementBrand);
                loadData(brandSelect);
            }
            else if (currentTable.Equals(modelSelect))
            {
                FillFrame(selectStatementModel);
                loadData(modelSelect);
            }
            else if (currentTable.Equals(typeSelect))
            {
                FillFrame(selectStatementType);
                loadData(typeSelect);
            }
            else if (currentTable.Equals(paymentSelect))
            {
                FillFrame(selectStatementPayment);
                loadData(paymentSelect);
            }
            else if (currentTable.Equals(warrantySelect))
            {
                FillFrame(selectStatementWarranty);
                loadData(warrantySelect);
            }
            else if (currentTable.Equals(servisSelect))
            {
                FillFrame (selectStatementServis);
                loadData(servisSelect);
            }
        }

        private void FillFrame(string selectStatement)
        {
            try
            {
                connection.Open();
                update = true;
                row = (DataRowView)dataGridCenter.SelectedItems[0];
                SqlCommand cmd = new SqlCommand { Connection = connection };
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = row["ID"];
                cmd.CommandText = selectStatement + "@id";
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (currentTable.Equals(productSelect))
                    {
                        frameProduct frameProduct = new frameProduct(update, row);
                        frameProduct.txtProductName.Text = reader["ime"].ToString();
                        frameProduct.txtProductPrice.Text = reader["cena"].ToString();
                        frameProduct.txtProductQuantity.Text = reader["kolicinaNaStanju"].ToString();
                        frameProduct.cbCustomer.SelectedValue = reader["kupacID"].ToString();
                        frameProduct.cbBrand.SelectedValue = reader["markaID"].ToString();
                        frameProduct.cbModel.SelectedValue = reader["modelID"].ToString();
                        frameProduct.cbProductType.SelectedValue = reader["tipProizvodaID"].ToString();
                        frameProduct.cbServis.SelectedValue = reader["servisID"].ToString();
                        frameProduct.cbWarranty.SelectedValue = reader["garancijaID"].ToString();
                        frameProduct.ShowDialog();
                    }
                    else if (currentTable.Equals(customerSelect))
                    {
                        frameCustomer frameCustomer = new frameCustomer(update, row);
                        frameCustomer.txtFirstName.Text = reader["kupacIme"].ToString();
                        frameCustomer.txtLastName.Text = reader["kupacPrezime"].ToString();
                        frameCustomer.txtPhoneNumber.Text = reader["brojTelefona"].ToString();
                        frameCustomer.txtEmail.Text = reader["email"].ToString();
                        frameCustomer.txtAddress.Text = reader["adresaSlanja"].ToString();
                        frameCustomer.ShowDialog();
                    }
                    else if (currentTable.Equals(brandSelect))
                    {
                        frameBrand frameBrand = new frameBrand(update, row);
                        frameBrand.txtBrandName.Text = reader["nazivMarke"].ToString();
                        frameBrand.ShowDialog();
                    }
                    else if (currentTable.Equals(modelSelect))
                    {
                        frameModel frameModel = new frameModel(update, row);
                        frameModel.txtModelName.Text = reader["nazivModela"].ToString();
                        frameModel.ShowDialog();
                    }
                    else if (currentTable.Equals(typeSelect))
                    {
                        frameProductType frameProductType = new frameProductType(update, row);
                        frameProductType.txtTypeName.Text = reader["nazivTipaProizvoda"].ToString();
                        frameProductType.ShowDialog();
                    }
                    else if (currentTable.Equals(paymentSelect))
                    {
                        framePayment framePayment = new framePayment(update, row);
                        framePayment.dpPaymentDate.SelectedDate = (DateTime)reader["datumPlacanja"];
                        framePayment.cbPaymentType.SelectedValue = reader["nacinPlacanja"].ToString();
                        framePayment.ShowDialog();                        
                    }
                    else if (currentTable.Equals(servisSelect))
                    {
                        frameServis frameServis = new frameServis(update, row);
                        frameServis.txtServisDetails.Text = reader["opisServisa"].ToString();
                        frameServis.dpServisDate.SelectedDate = (DateTime)reader["datumServisa"];
                        frameServis.ShowDialog();
                    }
                    else if (currentTable.Equals(warrantySelect))
                    {
                        frameWarranty frameWarranty = new frameWarranty(update, row);
                        frameWarranty.dpWarrantyStart.SelectedDate = (DateTime)reader["pocetakGarancije"];
                        frameWarranty.txtWarrantyLength.Text = reader["duzinaGarancije"].ToString();
                        frameWarranty.cboxInWarranty.IsChecked = Convert.ToBoolean((Int16)reader["uGaranciji"] == 1);
                        frameWarranty.ShowDialog();
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("You didn't select the row!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (currentTable.Equals(productSelect))
            {
                DeleteData(deleteProduct);
                loadData(productSelect);
            }
            else if (currentTable.Equals(customerSelect))
            {
                DeleteData(deleteCustomer);
                loadData(customerSelect);
            }
            else if (currentTable.Equals(brandSelect))
            {
                DeleteData(deleteBrand); 
                loadData(brandSelect);
            }
            else if (currentTable.Equals(modelSelect))
            {
                DeleteData(deleteModel);
                loadData(modelSelect);
            }
            else if (currentTable.Equals(typeSelect))
            {
                DeleteData(deleteType);
                loadData(typeSelect);
            }
            else if (currentTable.Equals(paymentSelect))
            {
                DeleteData(deletePayment);
                loadData(paymentSelect);
            }
            else if (currentTable.Equals(warrantySelect))
            {
                DeleteData(deleteWarranty);
                loadData(warrantySelect);
            }
            else if (currentTable.Equals(servisSelect))
            {
                DeleteData(deleteServis);
                loadData(servisSelect);
            }
        }

        private void DeleteData(string deleteQuery)
        {
            try
            {
                connection.Open();
                row = (DataRowView)dataGridCenter.SelectedItems[0];
                MessageBoxResult result = MessageBox.Show("Are you sure?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand
                    {
                        Connection = connection
                    };
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = row["ID"];
                    cmd.CommandText = deleteQuery + "@id";
                    cmd.ExecuteNonQuery();
                }

            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("You didn't select the row!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (SqlException)
            {
                MessageBox.Show("There is linked data in other tables!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }

        private void LogOff_Click(object sender,  RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to log off?", "Log off" ,MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Login loginWindow = new Login();
                loginWindow.Show();
                this.Close();
            }
        }

        public void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}