namespace BookLog;

using System.IO;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine();
        string mode = AskForInput("Please select mode (booklist or reading progress):");

        if (mode == "booklist") {
            // Logic to view the booklist
            Console.WriteLine("Displaying the booklist...");
            string bookCmd = AskForInput("Please enter the book command (add/edit/notes/remove):");
            if (bookCmd == "add") {
                string bookName = AskForInput("Enter the name of the book to add:");
                string authorName = AskForInput("Enter the author of the book (optional, press enter to skip):");
                if (string.IsNullOrWhiteSpace(authorName)) {
                    authorName = "Unknown"; // Default to "Unknown" if no author is provided
                }
                string pageCountInput = int.Parse(AskForInput("Enter the page count of the book (optional, press enter to skip):"));
                if (string.IsNullOrWhiteSpace(pageCountInput) || !int.TryParse(pageCountInput, out pageCount)) {
                    pageCount = 0; // Default to 0 if no valid page count is provided
                }
                string isbnInput = AskForInput("Enter the ISBN of the book (optional, press enter to skip):");
                string isbn = string.IsNullOrWhiteSpace(isbnInput) ? "Unknown" : isbnInput; // Default to "Unknown" if no ISBN is provided 
                // Logic to add the bookdetails to the booklist
                Console.WriteLine($"Book '{bookName}' added to the booklist.");
            } else if (bookCmd == "edit") {
                string bookName = AskForInput("Enter the name of the book to edit:");
                // Logic to edit the book in the booklist
            }
            } else if (bookCmd == "notes") {
                string bookName = AskForInput("Enter the name of the book to view notes:");
                // Logic to view the notes
                Console.WriteLine($"Displaying notes for the book '{bookName}':");
                // Here you would typically fetch and display the notes for the book
            } else if (bookCmd == "remove") {
                string bookName = AskForInput("Enter the name of the book to remove:");
                // Logic to remove the book from the booklist
                Console.WriteLine($"Book '{bookName}' removed from the booklist.");
            } else {
                Console.WriteLine("Invalid command.");
            }
        }
    }

    public static string AskForInput(string message) 
    {
        Console.Write(message);
        string input = Console.ReadLine()?.Trim().ToLower() ?? string.Empty;
        return input;
    }
}
