namespace BookLog.Tests;

using BookLog;

public class FileSaverTests
{
    FileSaver fileSaver;
    string testFileName;

    public FileSaverTests()
    {
        testFileName = "testfile.txt";
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
}