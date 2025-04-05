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

    // Method to set or update the reading goal
    public void SetReadingGoal(int goal) {
        this.ReadingGoal = goal;
        File.WriteAllText(goalFileSaver.fileName, this.ReadingGoal.ToString());
    }
}