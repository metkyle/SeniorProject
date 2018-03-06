namespace BookOrder.Api.Controllers
{
    using BookOrder.Core.Models;
    using BookOrder.Services.Books;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    [Route("api/[controller]")]
    public class BookProcessorController
    {
        public BookDataService bookDataService { get; }

        public BookProcessorController()
        {
            bookDataService = new BookDataService();
        }

        [HttpGet]
        public IEnumerable<object> Get()
        {
            return bookDataService.GenerateBooksForInstructor(1);
        }

        //TODO determine if this is the right place to fetch instructor books based on id
        [HttpGet("{id}")]
        public IEnumerable<object> Get(int id)
        {
            return bookDataService.GenerateBooksForInstructor(id);
        }

        [Route("[action]/{id}")]
        [HttpGet("{id}")]
        public IEnumerable<object> GetBooksForCourse(int id)
        {
            return bookDataService.GenerateBooksForCourse(id);
        }

        [Route("[action]/{id}")]
        [HttpGet("{id}")]
        public IEnumerable<object> GetPreviousBooksForCourse(int id)
        {
            return bookDataService.GeneratePreviousBooksForCourse(id);
        }

        [HttpPost]
        public Book Post([FromBody]JObject bookDetailsToSave)
        {
            var book = bookDetailsToSave.SelectToken("Book").ToObject<Book>();
            var courseId = bookDetailsToSave.SelectToken("CourseId").ToObject<int>();//TODO use later with sproc
            bookDataService.SaveBook(book, courseId);
            return book;
        }

        [HttpPut]
        public Book editbook([FromBody]JObject bookDetailsToSave)
        {
            var book = bookDetailsToSave.SelectToken("Book").ToObject<Book>();
            var courseId = bookDetailsToSave.SelectToken("CourseId").ToObject<int>();//TODO use later with sproc
            var originalISBN = bookDetailsToSave.SelectToken("OriginalISBN").ToObject<string>();

            if(!book.BookISBN.Equals((string)originalISBN)) {
                bookDataService.DeleteBook(System.Convert.ToInt64(originalISBN), courseId);
            }
            bookDataService.EditBook(book, courseId);
            return book;
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
            throw new MethodAccessException();
        }

        [HttpDelete]
        public decimal Delete([FromBody]JObject bookDetailsToDelete)
        {
            var bookISBN = bookDetailsToDelete.SelectToken("bookISBN").ToObject<decimal>();
            var courseId = bookDetailsToDelete.SelectToken("courseId").ToObject<int>();
            return bookDataService.DeleteBook(bookISBN, courseId);
        }
        
    }
}
