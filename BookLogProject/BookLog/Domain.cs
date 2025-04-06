namespace BookLog;

public class Book {
    public string Title { get; set; }
    public string Author { get; set; }
    public int PageCount { get; set;}
    public string ISBN { get; set; }

    public Book(string title, string author, int pageCount, string isbn) {
        this.Title = title;
        this.Author = author;
        this.PageCount = pageCount;
        this.ISBN = isbn;
    }
}

public class Reader {
    public int ReadingGoal { get; }

    public Reader(string name, int readingGoal) {
        this.ReadingGoal = readingGoal;
    }
}
public class LibraryEntry {
    public Book Book { get; set; }
    public DateOnly? DateAdded { get; set; }
    public DateOnly? DateFinished { get; set; }
    public bool Read { get; set; }
    public bool Owned { get; set;}
    public string? Note { get; set;}

    public LibraryEntry(Book book, DateOnly? dateAdded, DateOnly? dateFinished, bool read, bool owned, string? note) {
        this.Book = book;
        this.DateAdded = dateAdded;
        this.DateFinished = dateFinished;
        this.Read = read;
        this.Owned = owned;
        this.Note = note;
    }
}