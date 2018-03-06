namespace BookOrder.Services.Books.Validation
{
    using BookOrder.Services.Books.Validation.Interfaces;
    using System.Text.RegularExpressions;

    public class ISBNValidationService : IISBNValidator
    {
        public bool IsValidISBN(string isbn)
        {
            return new Regex("(\\d{13})").IsMatch(isbn);
        }
    }
}
