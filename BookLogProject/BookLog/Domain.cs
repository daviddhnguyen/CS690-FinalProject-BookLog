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
    public string Name { get; }
    public int ReadingGoal { get; }
    public int BooksRead { get; private set; }
    public Shelf Shelf { get; }

    public Reader(string name, int readingGoal) {
        this.Name = name;
        this.ReadingGoal = readingGoal;
        this.BooksRead = 0; // Initialize to 0
        this.Shelf = new Shelf(); // Initialize the shelf
    }

    // Method to add read book to BooksRead count
    public void MarkBookAsRead(UserBook userBook) {
        if (Shelf.Books.Contains(userBook) && userBook.ReadingStatus == "Read") {
            BooksRead++;
        }
    }
}

public class Shelf {
    public List<UserBook> Books { get; }

    public Shelf() {
        this.Books = new List<UserBook>();
    }
}

public class UserBook {
    public Book Book { get; }
    public DateOnly? StartDate { get; }
    public DateOnly? CompletionDate { get; }
    public string ReadingStatus { get; }
    public string OwnershipStatus { get; }
    public string? Notes { get; }

    public UserBook(Book book, DateOnly startDate, DateOnly completionDate, string readingStatus, string ownershipStatus, string notes) {
        this.Book = book;
        this.StartDate = startDate;
        this.CompletionDate = completionDate;
        this.ReadingStatus = readingStatus;
        this.OwnershipStatus = ownershipStatus;
        this.Notes = notes;
    }
}