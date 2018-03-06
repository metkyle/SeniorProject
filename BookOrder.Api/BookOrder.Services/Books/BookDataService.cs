namespace BookOrder.Services.Books
{
    using BookOrder.Core.Models;
    using BookOrder.Repositories.Books;
    using System.Collections.Generic;

    public class BookDataService
    {
        private BookDataRepository bookDataRepository { get; }

        //TODO wire up dependency injection, discuss what this means to the team
        public BookDataService()
        {
            bookDataRepository = new BookDataRepository();
        }

        //TODO change the object type to a concrete type, make this behavior into an interface
        public IEnumerable<object> GenerateBooksForInstructor(int instructorId)
        {
            return bookDataRepository.GetListOfBooksFromInstructorId(instructorId);
        }

        public IEnumerable<Book> GenerateBooksForCourse(int courseId)
        {
            return bookDataRepository.GetListOfBooksFromCourseId(courseId);
        }

        public IEnumerable<Book> GeneratePreviousBooksForCourse(int id)
        {
            return bookDataRepository.GetPreviousBooksForCourse(id);
        }

        public void SaveBook(Book bookObj, int courseId)
        {
            bookDataRepository.SaveBookToDatabase(bookObj, courseId);
        }

        public void EditBook(Book bookObj, int courseId)
        {
            bookDataRepository.EditBook(bookObj, courseId);
        }

        public decimal DeleteBook(decimal bookId, int courseId)
        {
            //TODO allow this if authenticated
            return bookDataRepository.DeleteBookById(bookId, courseId);
        }
    }
}
