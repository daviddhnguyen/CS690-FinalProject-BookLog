namespace BookLog;

public class DataManager
{
    FileSaver fileSaver;
    public List<Book> Book { get; set; }

    public DataManager(){
        fileSaver = new FileSaver("shelf.txt");

        Book = new List<Book>();
    }
    public void AddNewBook(Book bookDetails) {
    this.Book.Add(bookDetails);
    this.fileSaver.AppendData(bookDetails);
    }
}