namespace BookLog;

public class Book {
    public string Title { get; }
    public string Author { get; }
    public int PageCount { get; }
    public string ISBN { get; }

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
    public Book Book { get; }
    public DateOnly? DateAdded { get; }
    public DateOnly? DateFinished { get; }
    public bool Read { get; }
    public bool Owned { get; }
    public string? Note { get; }

    public LibraryEntry(Book book, DateOnly dateAdded, DateOnly dateFinished, bool read, bool owned, string note) {
        this.Book = book;
        this.DateAdded = dateAdded;
        this.DateFinished = dateFinished;
        this.Read = read;
        this.Owned = owned;
        this.Note = note;
    }
}