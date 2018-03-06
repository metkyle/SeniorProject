namespace BookOrder.Repositories.Books
{
    using System;
    using BookOrder.Core.Enums;
    using BookOrder.Core.Models;
    using System.Collections.Generic;
    using MySql.Data.MySqlClient;
    using System.Data;
    using System.Linq;

    public class BookDataRepository
    {
        public IEnumerable<Book> GetListOfBooksFromInstructorId(int instructorId)
        {
            return new[] { new Book(), new Book() };
        }

        public IEnumerable<Book> GetListOfBooksFromCourseId(int courseId)
        {
            List<Book> books = new List<Book>();

            using (var dbConnection = new MySQLDBConnection(false))
            {
                dbConnection.OpenConnection();
                bool hasData = false;

                var cmd = GenerateSqlCommandForConnectionWithText(dbConnection, "bookByCourseId");
                cmd.Parameters.AddWithValue("courseIdIn", courseId);

                try
                {
                    var rdr = cmd.ExecuteReader();
                    
                    while (rdr.Read())
                    {
                        hasData = true;
                        books.Add(GenerateBookFromReaderData(rdr));
                    }

                    if (!hasData)
                    {

                    }
                }
                catch (MySqlException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }

            return books;
        }

        public IEnumerable<Book> GetPreviousBooksForCourse(int courseId)
        {
            List<Book> books = new List<Book>();

            using (var dbConnection = new MySQLDBConnection(false))
            {
                dbConnection.OpenConnection();
                bool hasData = false;

                var cmd = GenerateSqlCommandForConnectionWithText(dbConnection, "previousBooksForCourseId");
                cmd.Parameters.AddWithValue("courseIdIn", courseId);

                try
                {
                    var rdr = cmd.ExecuteReader();
                    
                    while (rdr.Read())
                    {
                        hasData = true;
                        books.Add(GenerateBookFromReaderData(rdr));
                    }

                    if (!hasData)
                    {

                    }
                }
                catch (MySqlException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }

            return books;
        }

        public void SaveBookToDatabase(Book bookObj, int courseId)
        {
            var books = new List<Book>();

            using (var dbConnection = new MySQLDBConnection(false))
            {
                bool hasData = false;
                dbConnection.OpenConnection();

                var cmd = GenerateSqlCommandForConnectionWithText(dbConnection, "addBookComplete");
                cmd.Parameters.AddWithValue("courseID", courseId);
                cmd.Parameters.AddWithValue("ISBN", bookObj.BookISBN);
                cmd.Parameters.AddWithValue("edition", bookObj.BookEdition);
                cmd.Parameters.AddWithValue("publisher", bookObj.BookPublisher);
                cmd.Parameters.AddWithValue("title", bookObj.BookTitle);

                //TODO fix author 

                //cmd.Parameters.AddWithValue("authorName", bookObj.BookAuthors?.Aggregate((previous, newest) => previous + ", " + newest) ?? "No author");
                cmd.Parameters.AddWithValue("authorName", bookObj.BookAuthors);
                cmd.Parameters.AddWithValue("required", bookObj.BookRequired);//TODO implement this

                try
                {//TODO refactor the try/catch block (try only one operation and catch only the expected failures)
                    var rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        hasData = true;
                        books.Add(GenerateBookFromReaderData(rdr));
                    }

                    if (!hasData)
                    {

                    }
                }
                catch (MySqlException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }
        }

        public void EditBook(Book bookObj, int courseId)
        {
            var books = new List<Book>();

            using (var dbConnection = new MySQLDBConnection(false))
            {
                bool hasData = false;
                dbConnection.OpenConnection();

                var cmd = GenerateSqlCommandForConnectionWithText(dbConnection, "updateBookForCourse");
                cmd.Parameters.AddWithValue("courseIdIn", courseId);
                cmd.Parameters.AddWithValue("isbnIn", bookObj.BookISBN);
                cmd.Parameters.AddWithValue("editionIn", bookObj.BookEdition);
                cmd.Parameters.AddWithValue("publisherIn", bookObj.BookPublisher);
                cmd.Parameters.AddWithValue("titleIn", bookObj.BookTitle);

                //TODO fix author 

                //cmd.Parameters.AddWithValue("authorName", bookObj.BookAuthors?.Aggregate((previous, newest) => previous + ", " + newest) ?? "No author");
                cmd.Parameters.AddWithValue("authorIn", bookObj.BookAuthors);
                char required;
                if(bookObj.BookRequired) {
                    required = 'Y';
                }
                else {
                    required = 'N';
                }
                cmd.Parameters.AddWithValue("requiredIn", required);//TODO implement this

                try
                {//TODO refactor the try/catch block (try only one operation and catch only the expected failures)
                    var rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        hasData = true;
                        books.Add(GenerateBookFromReaderData(rdr));
                    }

                    if (!hasData)
                    {

                    }
                }
                catch (MySqlException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }
        }

        public decimal DeleteBookById(decimal bookISBN, int courseId)
        {
            using (var dbConnection = new MySQLDBConnection(false))
            {
                dbConnection.OpenConnection();

                var cmd = GenerateSqlCommandForConnectionWithText(dbConnection, "removeBookToCourse");
                cmd.Parameters.AddWithValue("ISBN", bookISBN);
                cmd.Parameters.AddWithValue("courseID", courseId);

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    //TODO implement this
                }
            }
            return bookISBN;
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

        private Book GenerateBookFromReaderData(MySqlDataReader reader)
         {
             return new Book
             {
                 BookAuthors = reader["author"].ToString(),
                 BookISBN = padISBN(reader["ISBN"].ToString()),
                 BookTitle = reader["title"].ToString(),
                 BookEdition = reader["edition"].ToString(),
                 BookPublisher = reader["publisher"].ToString(),
                 BookRequired = ((string)(reader["required"].ToString())).Equals("Y"), //Gross
             };
         }
 
         private IEnumerable<string> ParseAuthors(string authorString)
         {
             authorString.Trim();
             string[] authorArray = authorString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
             
             return new List<string>(authorArray);
         }

         private string padISBN(string prevISBN)
         {
             string ret = "";

             for(int i = 0; i < 13 - prevISBN.Length; i++) {
                 ret += "0";
             }
             ret += prevISBN;
             return ret;
         }
    }

}
