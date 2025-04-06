namespace BookLog.Tests;

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
}