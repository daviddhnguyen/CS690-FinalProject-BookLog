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
}