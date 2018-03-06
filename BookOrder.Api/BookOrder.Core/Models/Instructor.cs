namespace BookOrder.Core.Models
{
    using BookOrder.Core.Enums;
    using BookOrder.Core.Models;
    using System.Collections.Generic;
    public class Instructor
    {
        //TODO other fields as needed
        public string InstructorName { get; set; }
        public int InstructorId { get; set; }
        public string InstructorDept { get; set; }
        public string InstructorUsername { get; set; }
        public IEnumerable<Course> InstructorCourses { get; set; }

        public string InstructorEmail { get; set; }
        
        public int InstructorIsAdmin { get; set; }
        public Instructor()
        {
            
        }
    }
}
