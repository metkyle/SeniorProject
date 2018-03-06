namespace BookOrder.Repositories.Courses //changed parser to course
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using FileHelpers;
    

    [FileHelpers.DelimitedRecord(",")]
    public class CSVCourse
    {
        public string courseDept;
        public string courseID;
        public string courseSection;
        public string courseYear;
        public string courseTerm;
        public string courseInstructor;
        
    }
}
