namespace BookLog;

using System.IO;
using BookLog;

public class FileSaver {
    string fileName;

    public FileSaver(string fileName) {
        this.fileName = fileName;
        File.Create(this.fileName).Close();
    }

    public void AppendLine(string line) {
        File.AppendAllText(this.fileName, line + Environment.NewLine);
    }

    public void AppendData(Book bookDetails) {
        File.AppendAllText(this.fileName, bookDetails.Title  + ":" + bookDetails.Author  + ":" + bookDetails.PageCount  + ":" + bookDetails.ISBN + Environment.NewLine);
    }
}