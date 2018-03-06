namespace BookOrder.Core.Models
{
    using BookOrder.Core.Enums;
    using BookOrder.Core.Models;
    using System.Collections.Generic;
    public class Term
    {
        public string quarter { get; set; }
        public  int year { get; set; }
        //TODO get a date type in here?
        // private Date deadline;

        //TODO 
        //Change default constructor
        public Term()
        {
            quarter = "Fall";
            year = 2018;
        }

        public Term(string quarter, int year)
        {
            this.quarter = quarter;
            this.year = year;
        }
    }
}
