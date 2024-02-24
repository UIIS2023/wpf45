using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ProdavnicaKompjuterskeOpreme
{
    public class Connection
    {
        public SqlConnection generateConnection()
        {
            SqlConnectionStringBuilder connBuilder = new SqlConnectionStringBuilder
            {
                DataSource = @"DESKTOP-Q4S2930\SQLEXPRESS",
                InitialCatalog = "prodavnicakompjuterskeopreme",
                IntegratedSecurity = true
            };
            SqlConnection connection = new SqlConnection(connBuilder.ToString());
            return connection;
        }
    }
}
