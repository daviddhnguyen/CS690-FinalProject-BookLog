namespace BookLog;

public class DataManager
{
    FileSaver shelfFileSaver;
    FileSaver goalFileSaver;
    public List<Book> Books { get; set; }
    public int ReadingGoal { get; set; }


    public DataManager(string goalFileName = "goal.txt", string shelfFileName = "shelf.txt"){

        shelfFileSaver = new FileSaver(shelfFileName);
        goalFileSaver = new FileSaver(goalFileName);

        Books = new List<Book>();
        
        if(File.Exists(shelfFileName)) {
            var shelfFileContents = File.ReadAllLines(shelfFileName);
            foreach (var line in shelfFileContents)
            {
                var bookDetails = line.Split(':');
                if (bookDetails.Length == 4)
                {
                    string title = bookDetails[0];
                    string author = bookDetails[1];
                    int pageCount = int.Parse(bookDetails[2]);
                    string isbn = bookDetails[3];

                    Book book = new Book(title, author, pageCount, isbn);
                    Books.Add(book);
                }
            }
        }
        // Load the reading goal from goal.txt
        if (File.Exists(goalFileName))
        {
            string goalContent = File.ReadAllText(goalFileName).Trim(); // Read and trim whitespace
            if (string.IsNullOrWhiteSpace(goalContent) || !int.TryParse(goalContent, out int parsedGoal))
            {
                ReadingGoal = 0; // Default to 0 if the file is blank or invalid
            }
            else
            {
                ReadingGoal = parsedGoal;
            }
        }
        else
        {
            ReadingGoal = 0; // Default to 0 if the file does not exist
        }
    }

    // Method to add a new book to the shelf
    public void AddNewBook(Book bookDetails) {
        this.Books.Add(bookDetails);
        this.shelfFileSaver.AppendData(bookDetails);
    }

    public void SynchronizeBooks() {
        File.Delete(shelfFileSaver.fileName);

        // Recreate the file, even if Books is empty
        using (File.Create(shelfFileSaver.fileName)) { }
        
        foreach (var book in Books)
        {
            this.shelfFileSaver.AppendData(book);
        }
    }

    public void RemoveBook(string bookTitle)
    {
        var bookToRemove = Books.FirstOrDefault(book => book.Title == bookTitle);
        if (bookToRemove != null)
        {
            this.Books.Remove(bookToRemove);
            SynchronizeBooks(); // Update the shelf file after removing the book
        }
        else
        {
            Console.WriteLine($"'{bookTitle}' not found on your shelf.");
        }
    }

    // Method to set or update the reading goal
    public void SetReadingGoal(int goal) {
        this.ReadingGoal = goal;
        File.WriteAllText(goalFileSaver.fileName, this.ReadingGoal.ToString());
    }
}