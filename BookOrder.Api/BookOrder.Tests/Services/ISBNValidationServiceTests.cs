namespace BookOrder.Tests.Services
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using BookOrder.Services.Books.Validation;

    [TestClass]
    public class ISBNValidationServiceTests
    {
        [TestMethod]
        public void ValidISBN_ShouldReturnTrue()
        {
            var sut = new ISBNValidationService();
            var validISBN = "9871231234321";

            Assert.IsTrue(sut.IsValidISBN(validISBN));
        }

        [TestMethod]
        public void InvalidISBN_ShouldReturnFalse()
        {
            var sut = new ISBNValidationService();
            var validISBN = "asdfnotanisbn";

            Assert.IsFalse(sut.IsValidISBN(validISBN));
        }

        [TestMethod]
        public void ISBN10_ShouldReturnFalse()
        {
            var sut = new ISBNValidationService();
            var isbn10 = "9781231234";

            Assert.IsFalse(sut.IsValidISBN(isbn10));
        }
    }
}
