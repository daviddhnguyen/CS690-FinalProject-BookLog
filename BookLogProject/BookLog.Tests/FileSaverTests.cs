namespace BookLog.Tests;

using BookLog;

public class FileSaverTests
{
    FileSaver fileSaver;
    string testFileName;

    public FileSaverTests()
    {
        testFileName = $"testfile_{Guid.NewGuid()}.txt";
        File.Delete(testFileName); // Ensure the file does not exist before the test
        fileSaver = new FileSaver(testFileName);
    }

    [Fact]
    public void Test_FileSaver_AppendDataNull()
    {
        Book testBook = new Book("Test Book", "Test Author", 100, "1234567890");
        LibraryEntry entry = new LibraryEntry(testBook, new DateOnly(2025, 4, 6), null, false, true, "Test Note");
        
        fileSaver.AppendData(entry);
        var fileContents = File.ReadAllText(testFileName);

        string expectedLine = "Test Book:Test Author:100:1234567890:04/06/2025::False:True:Test Note" + Environment.NewLine;

        // Assert with a custom failure message
        Assert.True(fileContents.Contains(expectedLine), 
            $"Expected line not found in file contents.\nExpected: {expectedLine}\nActual: {fileContents}");
    }

    [Fact]
    public void Test_FileSaver_AppendDataDate()
    {
        Book testBook = new Book("Test Book", "Test Author", 100, "1234567890");
        LibraryEntry entry = new LibraryEntry(testBook, new DateOnly(2025, 4, 6), new DateOnly(2025, 4, 6), false, true, "Test Note");
        
        fileSaver.AppendData(entry);
        var fileContents = File.ReadAllText(testFileName);
        Assert.Contains("Test Book:Test Author:100:1234567890:04/06/2025:04/06/2025:False:True:Test Note" + Environment.NewLine, fileContents);
    }

}