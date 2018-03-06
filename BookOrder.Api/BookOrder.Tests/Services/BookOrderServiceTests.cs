namespace BookOrder.Tests.Services
{
    using BookOrder.Services.Books;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Linq;

    [TestClass]
    public class BookOrderServiceTests
    {
        [TestMethod]
        public void ValidCourseId_ShouldHaveBooks()
        {
            var sut = new BookDataService();

            var result = sut.GenerateBooksForCourse(3);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() > 1);
        }
    }
}
