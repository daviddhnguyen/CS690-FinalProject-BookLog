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

    public void AppendData(Book book) {
        File.AppendAllText(this.fileName, book.Title  + ":" + book.Author  + ":" + book.PageCount  + ":" + book.ISBN + Environment.NewLine);
    }
}