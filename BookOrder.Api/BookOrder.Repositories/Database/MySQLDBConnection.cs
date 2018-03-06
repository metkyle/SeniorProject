using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace BookOrder.Repositories
{
    public class MySQLDBConnection : IDisposable
    {
        private MySqlConnection connection;

        public MySqlConnection Connection { get => connection; set => connection = value; }

        public MySQLDBConnection(Boolean admin)
        {
            string server;
            string port;
            string database;
            string uid;
            string password;
            string connectionString = "";

            if (admin)
            {

            }
            else
            {
                //server = "localhost";
                server = "146.187.134.48";
                database = "Book_Order_System";
                uid = "faculty";
                port = "3306";
                password = "dev";
                //password = "9xT-3V$mm*";
                connectionString = "SERVER=" + server + ";" + "Port=" + port + ";" + "DATABASE=" + database + ";"
                    + "Uid=" + uid + ";" + "Pwd=" + password + ";SslMode=none;";
            }

            connection = new MySqlConnection(connectionString);
        }

        /*
                //initialize connection string
                private MySqlConnection InitializeConnection()
                {
                    server = "146.187.134.48/phpmyadmin";
                    database = "Book_Order_System";
                    uid = "root";
                    password = "9xT-3V$mm*";
                    string connectionString;
                    connectionString = "SERVER=" + server + ";" + "PORT=3306;" + "DATABASE=" + database + ";" + "Uid=" + uid + ";" + "Pwd=" + password + ";";
                    //Console.WriteLine("Connection String: " + connectionString);
                    return new MySqlConnection(connectionString);
                }

        */
        //Open Connection
        public bool OpenConnection()
        {
            try
            {
                //Console.WriteLine(Connection.ConnectionString);
                this.Connection.Open();
                //Console.WriteLine("Success");
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        //Console.WriteLine("Cannot Connect to Server");
                        //Console.WriteLine(ex.Message);
                        return false;

                    case 1045:
                        //Console.WriteLine("Invalid username or password");
                        return false;


                }
                //Console.WriteLine("{0} Exception caught.", ex);
                return false;
            }
            catch (Exception e)
            {
                //Console.WriteLine("{0} Exception caught.", e);
                return false;
            }
        }

        //Close connection
        public bool CloseConnection()
        {
            try
            {
                Connection.Close();
                //Console.WriteLine("Connection Closed");
                return true;
            }
            catch (MySqlException ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        public void Dispose()
        {
            this.CloseConnection();
        }
    }
}
