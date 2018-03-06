//TODO: Decide what  attributes make up a book (rename these) 

export class Book {

    public bookId: string; //Unique id for each book possibly
    public bookTitle: string;
    public bookAuthors: string;
    public bookISBN: string;
    public bookPublisher: string;
    public bookEdition: string;
    public bookRequired: boolean;

    constructor(){
    }
}