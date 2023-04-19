//Ty Larson
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Ty Larson
/// </summary>
namespace TheWritersCompanion
{
    class MySQLAuthorizer : IAuthorizer
    {
        private string username = "";
        private string password = "";

        private string connectionString;
        private bool authorized;

        /// <summary>
        /// Constructor for MySQLAuthorizer with Username and Password
        /// </summary>
        /// <param name="usernameIn"></param>
        /// <param name="passwordIn"></param>
        public MySQLAuthorizer(string usernameIn, string passwordIn)
        {
            username = usernameIn;
            password = passwordIn;
        }

        /// <summary>
        /// Attempts to log the user in
        /// </summary>
        /// <param name="username">Username for login</param>
        /// <param name="password">Password for login</param>
        /// <returns></returns>
        public bool Login(string username, string password)
        {
            CreateConnectionString();
            if (TestMySQLConnection())
            {
                authorized = true;
                return true;
            }
            authorized = false;
            return false;
        }

        /// <summary>
        /// Tests the connection to the database
        /// </summary>
        /// <returns></returns>
        private bool TestMySQLConnection()
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();
                connection.Close();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Creates the connection string using the user credentials
        /// </summary>
        private void CreateConnectionString()
        {
            connectionString = @"server=127.0.0.1;user=";
            connectionString = connectionString + username + ";password=" + 
                                password + ";database=" + username + ";port=3306";
        }

        //MIGHT NEED THESE AS PROPERTIES BUT GET APPROVAL FIRST
        public String ConnectionString
        {
            get
            {
                return connectionString;
            }
            set
            {
                connectionString = value;
            }
        }

        public bool Authorized
        {
            get
            {
                return authorized;
            }
            set
            {
                authorized = value;
            }
        }
    }
}
