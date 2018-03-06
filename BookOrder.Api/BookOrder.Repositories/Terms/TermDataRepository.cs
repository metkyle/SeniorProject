namespace BookOrder.Repositories.Terms
{
    using System;
    using BookOrder.Core.Enums;
    using BookOrder.Core.Models;
    using System.Collections.Generic;
    using MySql.Data.MySqlClient;
    using System.Data;
    using System.Linq;

    public class TermDataRepository
    {
        public IEnumerable<Term> GetAvailableTermsForDept(int deptId)
        {
            List<Term> termList = new List<Term>();

            using (var dbConnection = new MySQLDBConnection(false))
            {
                dbConnection.OpenConnection();

                var cmd = GenerateSqlCommandForConnectionWithText(dbConnection, "getAvailableTerms");

                try
                {
                    //TODO refactor the try/catch block (try only one operation and catch only the expected failures)
                    var rdr = cmd.ExecuteReader();

                    while(rdr.Read())
                    {
                        termList.Add(GenerateTermFromReaderData(rdr));
                    }
                }
                catch (MySqlException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }
            return termList;
        }

        public void SaveTermToDataBase(Term termObj)
        {
            using (var dbConnection = new MySQLDBConnection(false))
            {
                dbConnection.OpenConnection();

                var cmd = GenerateSqlCommandForConnectionWithText(dbConnection, "addAvailableTermYear");
                cmd.Parameters.AddWithValue("termIn", termObj.quarter);
                cmd.Parameters.AddWithValue("yearIn", termObj.year);

                try
                {
                    //TODO refactor the try/catch block (try only one operation and catch only the expected failures)
                    var rdr = cmd.ExecuteReader();
                }
                catch (MySqlException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }
        }

        public decimal DeleteTerm(Term termObj)
        {
            using (var dbConnection = new MySQLDBConnection(false))
            {
                dbConnection.OpenConnection();

                var cmd = GenerateSqlCommandForConnectionWithText(dbConnection, "removeAvailableTermYear");
                cmd.Parameters.AddWithValue("termIn", termObj.quarter);
                cmd.Parameters.AddWithValue("yearIn", termObj.year);

                try
                {
                    //TODO refactor the try/catch block (try only one operation and catch only the expected failures)
                    var rdr = cmd.ExecuteReader();
                }
                catch (MySqlException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }
            //TODO 
            return 0;
        }

        private MySqlCommand GenerateSqlCommandForConnectionWithText(MySQLDBConnection dbConnection, string commandText)
        {
            return new MySqlCommand
            {
                Connection = dbConnection.Connection,
                CommandText = commandText,
                CommandType = CommandType.StoredProcedure
            };
        }

        private Term GenerateTermFromReaderData(MySqlDataReader reader)
         {
             return new Term
             {
                quarter = reader["term"].ToString(),
                year    = reader.GetInt32(reader.GetOrdinal("year"))
             };

         }
    }
}
