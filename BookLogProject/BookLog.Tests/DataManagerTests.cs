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
    public void Test_AddNewBook()
    {
        Book testBook = new Book("Test Book", "Test Author", 100, "1234567890");
        dataManager.AddNewBook(testBook);

        Assert.Contains(testBook, dataManager.Books);

        // Verify the book is saved to the custom shelf file
        var shelfFileContents = File.ReadAllText(testShelfFileName);
        Assert.Contains("Test Book:Test Author:100:1234567890" + Environment.NewLine, shelfFileContents);

        // Cleanup
        File.Delete(testShelfFileName);
    }
}