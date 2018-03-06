namespace BookOrder.Core.Models
{
    using System;
    using BookOrder.Core.Enums;
    using BookOrder.Core.Models;
    using System.Collections.Generic;

    public class Course
    {
        //TODO other fields as needed
        public int CourseId { get; set; }
        public string CourseNumber { get; set; }
        public string Instructor { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public bool IsSubmitted { get; set; }
        public BookOrderOption MyBookOrderOption { get; set; }
        public IEnumerable<Book> BookArray { get; set; }
        public string Term  { get; set; }
        public int Year { get; set; }

        public Course()
        {
        }

    }
}
