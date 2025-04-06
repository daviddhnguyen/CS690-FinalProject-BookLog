namespace BookLog;

using System.IO;
using BookLog;

public class FileSaver {
    public string fileName { get; }

    public FileSaver(string fileName) {
        this.fileName = fileName;
        if(!File.Exists(this.fileName)) {
            File.Create(this.fileName).Close();
        }
    }

    public void AppendData(LibraryEntry entry) {
        var book = entry.Book;
        string dateAdded =
         entry.DateAdded?.ToString("yyyy-MM-dd") ?? "N/A";
        string dateFinished =
         entry.DateFinished?.ToString("yyyy-MM-dd") ?? "N/A";
        string line =
         $"{book.Title}:{book.Author}:{book.PageCount}:{book.ISBN}:{entry.DateAdded}:{entry.DateFinished}:{entry.Read}:{entry.Owned}:{entry.Note}";
        File.AppendAllText(this.fileName, line + Environment.NewLine);
    }
}