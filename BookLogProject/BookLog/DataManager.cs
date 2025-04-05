namespace BookLog;

public class DataManager
{
    FileSaver shelfFileSaver;
    FileSaver goalFileSaver;
    public List<Book> Books { get; set; }
    public int ReadingGoal { get; set; }


    public DataManager(){

        shelfFileSaver = new FileSaver("shelf.txt");
        goalFileSaver = new FileSaver("goal.txt");

        Books = new List<Book>();

        // Load the reading goal from goal.txt
        if (File.Exists("goal.txt"))
        {
            string goalContent = File.ReadAllText("goal.txt").Trim(); // Read and trim whitespace
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
        File.WriteAllText("goal.txt", this.ReadingGoal.ToString());
    }
}