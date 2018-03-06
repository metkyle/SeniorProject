namespace BookOrder.Services.Books.Validation.Interfaces
{
    using System;

    public interface IISBNValidator
    {
        bool IsValidISBN(string isbn);
    }
}
