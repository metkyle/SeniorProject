namespace BookOrder.Core.Models
{
    using System.Collections.Generic;
    public class Book
    {
        //TODO other fields as needed
        public string BookId { get; set; }
        public string BookTitle { get; set; }
        public string BookAuthors { get; set ;}
        public string BookISBN { get; set; }
        public string BookPublisher { get; set; }
        public string BookEdition { get; set; }
        public bool BookRequired { get; set; }

        public Book()
        {
        }
    }
}
