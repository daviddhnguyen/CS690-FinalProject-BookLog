namespace BookLog.Tests;

using BookLog;

public class FileSaverTests
{
    FileSaver fileSaver;
    string testFileName;

    public FileSaverTests()
    {
        testFileName = "testfile.txt";
        File.Delete(testFileName); // Ensure the file does not exist before the test
        fileSaver = new FileSaver(testFileName);
    }

    [Fact]
    public void Test_FileSaver_Append()
    {
        string testString = "This is a test string.";
        
        fileSaver.AppendLine(testString);
        var fileContents = File.ReadAllText(testFileName);
        Assert.Contains(testString+Environment.NewLine, fileContents);
    }

    [Fact]
    public void Test_FileSaver_AppendData()
    {
        Book testBook = new Book("Test Book", "Test Author", 100, "1234567890");
        
        fileSaver.AppendData(testBook);
        var fileContents = File.ReadAllText(testFileName);
        Assert.Contains("Test Book,Test Author,100,1234567890" + Environment.NewLine, fileContents);
    }
}