namespace BookLog.Tests;

using System.Dynamic;
using BookLog;

public class DataManagerTests
{
    DataManager dataManager;
    string testGoalFileName;
    string testShelfFileName;

    public DataManagerTests()
    {
        testGoalFileName = $"test_goal_{Guid.NewGuid()}.txt";
        testShelfFileName = $"test_shelf_{Guid.NewGuid()}.txt";

        // Ensure the test files do not exist before the test
        File.Delete(testGoalFileName);
        File.Delete(testShelfFileName);

        // Pass the test file names to the DataManager constructor
        dataManager = new DataManager(testGoalFileName, testShelfFileName);
    }

    [Fact]
    public void Test_UpdateReadingGoal()
    {
        int newGoal = 10;
        dataManager.SetReadingGoal(newGoal);
        Assert.Equal(newGoal, dataManager.ReadingGoal);

        // Verify the goal is saved to the file
        var fileContents = File.ReadAllText(testGoalFileName).Trim();
        Assert.Equal(newGoal.ToString(), fileContents);
    }
    
    [Fact]
    public void Test_AddLibraryEntryNull() {
        Book testBook = new Book("Test Book", "Test Author", 100, "1234567890");
        LibraryEntry entry = new LibraryEntry(testBook, new DateOnly(2025, 4, 6), null, false, true, "Test Note");

        dataManager.AddLibraryEntry(entry);

        Assert.Contains(entry, dataManager.LibraryEntries);

        var shelfFileContents = File.ReadAllText(testShelfFileName);
        Assert.Contains("Test Book:Test Author:100:1234567890:04/06/2025::False:True:Test Note", shelfFileContents);
    }

    [Fact]
    public void Test_AddLibraryEntryDate() {
        Book testBook = new Book("Test Book", "Test Author", 100, "1234567890");
        LibraryEntry entry = new LibraryEntry(testBook, new DateOnly(2025, 4, 6), new DateOnly(2026, 4, 6), false, true, "Test Note");

        dataManager.AddLibraryEntry(entry);

        Assert.Contains(entry, dataManager.LibraryEntries);

        var shelfFileContents = File.ReadAllText(testShelfFileName);
        Assert.Contains("Test Book:Test Author:100:1234567890:04/06/2025:04/06/2026:False:True:Test Note", shelfFileContents);
    }

    [Fact]
    public void Test_GetBooksReadCount() {
        Book testBook1 = new Book("Test Book 1", "Test Author 1", 100, "1234567890");
        LibraryEntry entry1 = new LibraryEntry(testBook1, new DateOnly(2025, 4, 6), new DateOnly(2026, 4, 6), true, true, "Test Note");

        Book testBook2 = new Book("Test Book 2", "Test Author 2", 200, "0987654321");
        LibraryEntry entry2 = new LibraryEntry(testBook2, new DateOnly(2025, 4, 6), null, false, true, "Test Note");

        dataManager.AddLibraryEntry(entry1);
        dataManager.AddLibraryEntry(entry2);

        Assert.Equal(1, dataManager.GetBooksReadCount());
    }

    [Fact]
    public void Test_GetBooksOwnedCount() {
        Book testBook1 = new Book("Test Book 1", "Test Author 1", 100, "1234567890");
        LibraryEntry entry1 = new LibraryEntry(testBook1, new DateOnly(2025, 4, 6), new DateOnly(2026, 4, 6), true, true, "Test Note");

        Book testBook2 = new Book("Test Book 2", "Test Author 2", 200, "0987654321");
        LibraryEntry entry2 = new LibraryEntry(testBook2, new DateOnly(2025, 4, 6), null, false, false, "Test Note");

        dataManager.AddLibraryEntry(entry1);
        dataManager.AddLibraryEntry(entry2);

        Assert.Equal(1, dataManager.GetBooksOwnedCount());
    }
    [Fact]
    public void Test_GetBooksCount() {
        Book testBook1 = new Book("Test Book 1", "Test Author 1", 100, "1234567890");
        LibraryEntry entry1 = new LibraryEntry(testBook1, new DateOnly(2025, 4, 6), new DateOnly(2026, 4, 6), true, true, "Test Note");

        Book testBook2 = new Book("Test Book 2", "Test Author 2", 200, "0987654321");
        LibraryEntry entry2 = new LibraryEntry(testBook2, new DateOnly(2025, 4, 6), null, false, false, "Test Note");

        Book testbook3 = new Book("Test Book 3", "Test Author 3", 300, "1122334455");
        LibraryEntry entry3 = new LibraryEntry(testbook3, new DateOnly(2025, 4, 6), null, false, true, "Test Note");

        dataManager.AddLibraryEntry(entry1);
        dataManager.AddLibraryEntry(entry2);
        dataManager.AddLibraryEntry(entry3);

        Assert.Equal(3, dataManager.GetBooksCount());
    }
    [Fact]
    public void Test_UpdateLibraryEntry_PartialUpdate() {
        Book testBook = new Book("Test Book", "Test Author", 100, "1234567890");
        LibraryEntry entry = new LibraryEntry(testBook, new DateOnly(2025, 4, 6), null, false, true, "Test Note");

        dataManager.AddLibraryEntry(entry);

        // Update only the title and read status
        dataManager.UpdateLibraryEntry(entry, newTitle: "Updated Title");

        // Verify that only the specified fields are updated
        Assert.Equal("Updated Title", entry.Book.Title);
        Assert.False(entry.Read); // Unchanged
        Assert.Equal("Test Author", entry.Book.Author); // Unchanged
        Assert.Equal(100, entry.Book.PageCount); // Unchanged
        Assert.Equal("1234567890", entry.Book.ISBN); // Unchanged
        Assert.Null(entry.DateFinished); // Unchanged
        Assert.Equal("Test Note", entry.Note); // Unchanged
    }

    [Fact]
    public void Test_UpdateLibraryEntry() {
        Book testBook = new Book("Test Book", "Test Author", 100, "1234567890");
        LibraryEntry entry = new LibraryEntry(testBook, new DateOnly(2025, 4, 6), null, false, true, "Test Note");

        dataManager.AddLibraryEntry(entry);

        // Verify the entry is added
        var shelfFileContents = File.ReadAllText(testShelfFileName);
        Assert.Contains("Test Book:Test Author:100:1234567890:04/06/2025::False:True:Test Note", shelfFileContents);

        // Update the entry
        dataManager.UpdateLibraryEntry(entry, newDateFinished: new DateOnly(2026, 4, 7), newRead: true);

        // Verify the entry is updated in memory
        Assert.Contains(entry, dataManager.LibraryEntries);
        Assert.Equal(new DateOnly(2026, 4, 7), entry.DateFinished);
        Assert.True(entry.Read);

        // Verify the entry is updated in the file
        shelfFileContents = File.ReadAllText(testShelfFileName);
        Assert.Contains("Test Book:Test Author:100:1234567890:04/06/2025:04/07/2026:True:True:Test Note", shelfFileContents);
    }

    [Fact]
    public void Test_UpdateLibraryEntry_AutoUpdateDateFinished() {
        Book testBook = new Book("Test Book", "Test Author", 100, "1234567890");
        LibraryEntry entry = new LibraryEntry(testBook, new DateOnly(2025, 4, 6), new DateOnly(2025, 4, 6), false, true, "Test Note");

        dataManager.AddLibraryEntry(entry);

        // Capture today's date
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        // Mark the book as read
        dataManager.UpdateLibraryEntry(entry, newRead: true);

        // Verify that DateFinished is set to today's date'
        Assert.True(entry.Read);
        Assert.Equal(today, entry.DateFinished);

        // Mark the book as unread
        dataManager.UpdateLibraryEntry(entry, newRead: false);

        // Verify that DateFinished is cleared
        Assert.False(entry.Read);
        Assert.Null(entry.DateFinished);
    }
}