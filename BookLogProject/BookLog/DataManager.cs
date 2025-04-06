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

    public void UpdateLibraryEntry(
        LibraryEntry entry,
        string? newTitle = null,
        string? newAuthor = null,
        int? newPageCount = null,
        string? newISBN = null,
        bool? newRead = null,
        bool? newOwned = null,
        DateOnly? newDateFinished = null,
        string? newNote = null
    ) {
        // Update the title (cannot be null)
        if (newTitle != null) entry.Book.Title = newTitle;

        // Update the author (only if explicitly provided)
        if (newAuthor != null) entry.Book.Author = string.IsNullOrWhiteSpace(newAuthor) ? "Unknown" : newAuthor;

        // Update the page count (only if explicitly provided)
        if (newPageCount.HasValue) entry.Book.PageCount = newPageCount.Value;

        // Update the ISBN (only if explicitly provided)
        if (newISBN != null) entry.Book.ISBN = string.IsNullOrWhiteSpace(newISBN) ? "Unknown" : newISBN;

        // Update the read status (only if explicitly provided)
        if (newRead.HasValue) {
            // If the book is marked as read and was previously unread, set DateFinished to today's date
            if (newRead.Value == true && !entry.Read) {
                entry.DateFinished = DateOnly.FromDateTime(DateTime.Now);
            }
            // If the book is marked as unread and was previously read, clear the DateFinished field
            else if (newRead.Value == false && entry.Read) {
                entry.DateFinished = null;
            } 
            entry.Read = newRead.Value;
        }

        // Update the owned status (only if explicitly provided)
        if (newOwned.HasValue) entry.Owned = newOwned.Value;

        // Update the date finished (only if explicitly provided)
        if (newDateFinished.HasValue) entry.DateFinished = newDateFinished;

        // Update the note (only if explicitly provided)
        if (newNote != null) entry.Note = string.IsNullOrWhiteSpace(newNote) ? null : newNote;

        // Synchronize changes to the file
        SynchronizeLibraryEntries();
    }
}