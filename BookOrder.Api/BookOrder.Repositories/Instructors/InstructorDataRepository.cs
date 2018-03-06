namespace BookOrder.Repositories.Instructors
{
    using System.Collections.Generic;
    using BookOrder.Core.Models;
    using System;
    using BookOrder.Repositories.Books;
    using BookOrder.Core.Enums;
    using MySql.Data.MySqlClient;
    using System.Data;
    using BookOrder.Repositories.Courses;

    public class InstructorDataRepository
    {
        //TODO wire up this behavior so that it pulls from a database, make the object type into a concrete type,
        //and abstract behavior into an interface

        CourseDataRepository courseDataRepository = new CourseDataRepository();
        public IEnumerable<Instructor> GetListOfInstructorsFromDepartmentId(int departmentId)
        {
          List<Instructor> instructors = new List<Instructor>();
 
             var sut = new MySQLDBConnection(false);
             sut.OpenConnection();
 
             bool hasData = false;
             MySqlCommand cmd = new MySqlCommand();
             cmd.Connection = sut.Connection;
             cmd.CommandText = "faculty";
             cmd.CommandType = System.Data.CommandType.StoredProcedure;
 
             try
             {
                 var rdr = cmd.ExecuteReader();
 
                 while (rdr.Read())
                 {
                     Instructor temp = GenerateInstructorFromReader(rdr);
                     hasData = true;
                     instructors.Add(temp);
                 }
 
                 if (!hasData)
                 {
 
                 }
             }
             catch (MySqlException ex)
             {
                 System.Diagnostics.Debug.WriteLine(ex);
             }
 
             sut.CloseConnection();
             return instructors;
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
 
         public Instructor GenerateInstructorFromReader(MySqlDataReader rdr)
         {
            Instructor instructor = new Instructor {
                InstructorId = rdr.GetInt32(rdr.GetOrdinal("ID")),
                InstructorName = rdr["fName"].ToString() + " " + rdr["lName"].ToString(),
                // InstructorDept = "DEPARTMENT",
                InstructorEmail = rdr["email"].ToString(),
                InstructorIsAdmin = rdr.GetInt32(rdr.GetOrdinal("admin")),
                InstructorUsername = rdr["userName"].ToString(),
                InstructorDept = rdr["department"].ToString()
             }; 
             instructor.InstructorCourses = courseDataRepository.GetListOfCoursesFromInstructorId(instructor.InstructorId); 
             return instructor;
         }
    }
}
