namespace BookLog;

public class DataManager
{
FileSaver shelfFileSaver;
    FileSaver goalFileSaver;
    public List<LibraryEntry> LibraryEntries { get; set; } // Replace Books with LibraryEntries
    public int ReadingGoal { get; set; }

    public DataManager(string goalFileName = "goal.txt", string shelfFileName = "shelf.txt") {
        shelfFileSaver = new FileSaver(shelfFileName);
        goalFileSaver = new FileSaver(goalFileName);

        LibraryEntries = new List<LibraryEntry>();

        if (File.Exists(shelfFileName)) {
            var shelfFileContents = File.ReadAllLines(shelfFileName);
            foreach (var line in shelfFileContents) {
                var entryDetails = line.Split(':');
                if (entryDetails.Length == 9) {
                    string title = entryDetails[0];
                    string author = entryDetails[1];
                    int pageCount = int.Parse(entryDetails[2]);
                    string isbn = entryDetails[3];
                    DateOnly dateAdded = DateOnly.Parse(entryDetails[4]);
                    DateOnly? dateFinished = string.IsNullOrWhiteSpace(entryDetails[5]) || entryDetails[5] == "N/A" 
                        ? null 
                        : DateOnly.Parse(entryDetails[5]);

                    bool read = bool.Parse(entryDetails[6]);
                    bool owned = bool.Parse(entryDetails[7]);
                    string note = entryDetails[8];

                    Book book = new Book(title, author, pageCount, isbn);
                    LibraryEntry entry = new LibraryEntry(book, dateAdded, dateFinished, read, owned, note);
                    LibraryEntries.Add(entry);
                }
            }
        }

        if (File.Exists(goalFileName)) {
            string goalContent = File.ReadAllText(goalFileName).Trim();
            if (string.IsNullOrWhiteSpace(goalContent) || !int.TryParse(goalContent, out int parsedGoal)) {
                ReadingGoal = 0;
            } else {
                ReadingGoal = parsedGoal;
            }
        } else {
            ReadingGoal = 0;
        }
    }

    public void AddLibraryEntry(LibraryEntry entry) {
        LibraryEntries.Add(entry);
        shelfFileSaver.AppendData(entry);
    }

    public void SynchronizeLibraryEntries() {
        File.Delete(shelfFileSaver.fileName);

        using (File.Create(shelfFileSaver.fileName)) { }

        foreach (var entry in LibraryEntries) {
            shelfFileSaver.AppendData(entry);
        }
    }

    public void RemoveLibraryEntry(string bookTitle) {
        var entryToRemove = LibraryEntries.FirstOrDefault(entry => entry.Book.Title == bookTitle);
        if (entryToRemove != null) {
            LibraryEntries.Remove(entryToRemove);
            SynchronizeLibraryEntries();
        } else {
            Console.WriteLine($"'{bookTitle}' not found on your shelf.");
        }
    }

    // Method to set or update the reading goal
    public void SetReadingGoal(int goal) {
        this.ReadingGoal = goal;
        File.WriteAllText(goalFileSaver.fileName, this.ReadingGoal.ToString());
    }

        public int GetBooksCount() {
        return LibraryEntries.Count;
    }

    public int GetBooksReadCount() {
        return LibraryEntries.Count(entry => entry.Read);
    }

    public int GetBooksOwnedCount() {
        return LibraryEntries.Count(entry => entry.Owned);
    }
}