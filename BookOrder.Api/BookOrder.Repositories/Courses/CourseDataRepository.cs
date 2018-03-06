namespace BookOrder.Repositories.Courses
{
    using System;
    using BookOrder.Repositories.Books;
    using BookOrder.Core.Enums;
    using BookOrder.Core.Models;
    using System.Collections.Generic;
    using MySql.Data.MySqlClient;
    using System.Data;
    using System.Linq;
    using System.Data.SqlClient;

    public interface ICourseDataRepository
    {
        IEnumerable<Course> GetListOfCoursesFromInstructorId(int instructorId);
    }

    public class CourseDataRepository : ICourseDataRepository
    {
        //TODO wire up this behavior so that it pulls from a database, make the object type into a concrete type,
        //and abstract behavior into an interface
        BookDataRepository bookDataRepository = new BookDataRepository();
        public IEnumerable<Course> GetListOfCoursesFromInstructorId(int instructorId)
        {
            List<Course> courses = new List<Course>();

            var sut = new MySQLDBConnection(false);
            sut.OpenConnection();

            bool hasData = false;
            var cmd = new MySqlCommand
            {
                Connection = sut.Connection,
                CommandText = "courseByInstructorID",
                CommandType = System.Data.CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("input", instructorId);

            try
            {
                var rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Course temp = GenerateCourseFromReader(rdr);
                    courses.Add(temp);
                }
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            sut.CloseConnection();
            return courses;
        }

        //returns the id of the newly created course
        public int SaveCourseToDatabase(Course courseObj, int instructorId)
        {
            int id = -1;

            using (var dbConnection = new MySQLDBConnection(false))
            {
                dbConnection.OpenConnection();

                var cmd = GenerateSqlCommandForConnectionWithText(dbConnection, "addCourseForInstructor");
                cmd.Parameters.AddWithValue("abbreviationIn", courseObj.CourseNumber);
                cmd.Parameters.AddWithValue("sectionIn", courseObj.Section);
                cmd.Parameters.AddWithValue("termIn", courseObj.Term);
                cmd.Parameters.AddWithValue("yearIn", courseObj.Year);
                cmd.Parameters.AddWithValue("deptIn", courseObj.Department);
                cmd.Parameters.AddWithValue("instructorIdIn", instructorId);

                try
                {
                    //TODO refactor the try/catch block (try only one operation and catch only the expected failures)
                    id = (int)cmd.ExecuteScalar();
                    
                }
                catch (MySqlException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }
            return id;
        }

        //TODO: Figure out the parameters
        public void SaveCourseToDatabase(CSVCourse[] csvCourse)
        {
            using (var dbConnection = new MySQLDBConnection(false))
            {
                
                dbConnection.OpenConnection();

                var cmd = GenerateSqlCommandForConnectionWithText(dbConnection, "addCourseForCSV");

                //Project the csv courses into a collection with the database connection, then build and execute the database calls
                csvCourse
                    .Select(course => new { Course = course, Connection = dbConnection})
                    .Select(BuildCommandForCsvCourseAdd).ToList()
                    .ForEach(ExecuteCsvDatabaseCourseAdd);
            }
        }

        public void submitCourse(int courseId)
        {
            using (var dbConnection = new MySQLDBConnection(false))
            {
                dbConnection.OpenConnection();

                var cmd = GenerateSqlCommandForConnectionWithText(dbConnection, "submitBooksForCourse");
                cmd.Parameters.AddWithValue("i_courseId", courseId);
                

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    //TODO implement this
                }
            }
        
        }

        public string DeleteCourseByName(string courseName, int instructorId)
        {
            using (var dbConnection = new MySQLDBConnection(false))
            {
                dbConnection.OpenConnection();

                var cmd = GenerateSqlCommandForConnectionWithText(dbConnection, "deleteCourseByName");
                cmd.Parameters.AddWithValue("courseName", courseName);
                cmd.Parameters.AddWithValue("instructorId", instructorId);

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    //TODO implement this
                }
            }
            return courseName;
        }

        public void DeleteCourseById(int courseId)
        {
            using (var dbConnection = new MySQLDBConnection(false))
            {
                dbConnection.OpenConnection();

                var cmd = GenerateSqlCommandForConnectionWithText(dbConnection, "deleteCourseById");
                cmd.Parameters.AddWithValue("courseIdIn", courseId);

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    //TODO implement this
                }
            }
        }

        public void SetCourseSubmitted(Course course)
        {
            using (var dbConnection = new MySQLDBConnection(false))
            {
                dbConnection.OpenConnection();
                char submittedChar = 'N';
                if(course.IsSubmitted) {
                    submittedChar = 'Y';
                }

                var cmd = GenerateSqlCommandForConnectionWithText(dbConnection, "setCourseSubmitted");
                cmd.Parameters.AddWithValue("courseIdIn", course.CourseId);
                cmd.Parameters.AddWithValue("isSubmittedIn", submittedChar);
                cmd.Parameters.AddWithValue("bookOrderOptionIn", course.MyBookOrderOption);


                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    //TODO implement this
                }
            }
        }

        private MySqlCommand BuildCommandForCsvCourseAdd(dynamic courseObject)
        {
            var command = GenerateSqlCommandForConnectionWithText(courseObject.Connection, "addCourseForCSV");
            command.Parameters.AddWithValue("i_dept", courseObject.Course.courseDept);
            command.Parameters.AddWithValue("i_course", courseObject.Course.courseID);
            command.Parameters.AddWithValue("i_section", courseObject.Course.courseSection);
            command.Parameters.AddWithValue("i_year", courseObject.Course.courseYear);
            command.Parameters.AddWithValue("i_session", courseObject.Course.courseTerm);
            command.Parameters.AddWithValue("i_instructor", courseObject.Course.courseInstructor);

            return command;
        }

        private void ExecuteCsvDatabaseCourseAdd(MySqlCommand courseToGenerate)
        {
            try
            {
                courseToGenerate.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                throw new InvalidOperationException("Unable to store the csv book to the database", ex);
            }
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

        public Course GenerateCourseFromReader(MySqlDataReader rdr)
         {
             Course course = new Course {
                 CourseId           = rdr.GetInt32(rdr.GetOrdinal("courseId")),
                 CourseNumber       = rdr["course"].ToString(),
                 Instructor         = rdr["fName"].ToString() + " " + rdr["lName"].ToString(),
                 Name               = rdr["course"].ToString(),
                 Department         = rdr["dept"].ToString(),
                 Section            = rdr["section"].ToString(),
                 IsSubmitted        = ((string)(rdr["isSubmitted"].ToString())).Equals("Y"), //Gross
                 MyBookOrderOption  = (BookOrderOption) Enum.Parse<BookOrderOption>(rdr["bookOrderOption"].ToString()),
                 Term               = rdr["term"].ToString(),
                 Year               = rdr.GetInt32(rdr.GetOrdinal("year"))
             };
 
             course.BookArray = bookDataRepository.GetListOfBooksFromCourseId(course.CourseId);
             return course;
         }

        //TODO make a mock injected service when the database is unreachable
        public IEnumerable<Course> GetMockListOfCoursesFromInstructorId(int instructorId)
        {
            return new[] { new Course
                {
                    BookArray = new List<Book>(),
                    MyBookOrderOption = BookOrderOption.NoOptionSelected,
                    CourseNumber = "CSCD101",
                    CourseId = 1,
                    Department = "CS",
                    Instructor = "TC",
                    IsSubmitted = false,
                    Name = "Whatever",
                    Section = "01",
                    Term = "Fall",
                    Year = 2018
                },
                new Course
                {
                    BookArray = new List<Book>(),
                    MyBookOrderOption = BookOrderOption.NoOptionSelected,
                    CourseNumber = "CSCD102",
                    CourseId = 2,
                    Department = "CS",
                    Instructor = "TC",
                    IsSubmitted = false,
                    Name = "Whatever2",
                    Section = "02",
                    Term = "Fall",
                    Year = 2018
                }
            };
        }
    }
}
