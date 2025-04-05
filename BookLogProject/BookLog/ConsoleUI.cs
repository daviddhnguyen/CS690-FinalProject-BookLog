namespace BookLog;

using BookLog;
using Spectre.Console;

public class ConsoleUI {
    DataManager dataManager;

    public ConsoleUI() {
        dataManager = new DataManager();
    }

    public void Show() {

        // Home page
        var mode = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select a mode:")
                .AddChoices(new[] {
                    "Book Shelf", "Reading Progress"
                    })
        );

        if (mode == "Book Shelf") {
            // Logic to view the shelf
            Console.WriteLine("Displaying your shelf...");

            string bookCmd;

            do {
                // shelf end
                bookCmd = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Select a book command:")
                        .AddChoices(new[] {
                            "Add a Book", "Edit a book", "View notes for a book", "Remove a book", "See shelf", "End"
                        })
                );
                
                if (bookCmd == "Add a book") {
                    string bookName = AskForInput("Enter the name of the book:");

                    string authorName = AskForInput("Enter the author of the book (optional, press enter to skip):");
                    if (string.IsNullOrWhiteSpace(authorName)) {
                        authorName = "Unknown"; // Default to "Unknown" if no author is provided
                    }

                    string pageCountInput = AskForInput("Enter the page count of the book (optional, press enter to skip):");
                    int pageCount = int.TryParse(pageCountInput, out int parsedPageCount) ? parsedPageCount : 0; // Default to 0 if invalid

                    string isbnInput = AskForInput("Enter the ISBN of the book (optional, press enter to skip):");
                    string isbn = string.IsNullOrWhiteSpace(isbnInput) ? "Unknown" : isbnInput; // Default to "Unknown" if no ISBN is provided 
                    // Logic to add a book the bookdetails to the shelf

                    Book bookDetails = new Book(bookName, authorName, pageCount, isbn);

                    dataManager.AddNewBook(bookDetails);

                    Console.WriteLine($"'{bookName}' added to your shelf.");
                } else if (bookCmd == "Edit a book") {
                    string bookName = AskForInput("Enter the name of the book to edit a book:");
                    // Logic to edit a book on the shelf

                } else if (bookCmd == "View notes for a book") {
                    string bookName = AskForInput("Enter the name of the book to view its notes:");
                    // Logic to view the view notes for a book
                    Console.WriteLine($"Displaying view notes for a book for the book '{bookName}':");
                    // Here you would typically fetch and display the view notes for a book for the book

                } else if (bookCmd == "Remove a book") {
                    string bookName = AskForInput("Enter the name of the book to remove a book:");
                    // Logic to remove a book the book from the shelf
                    Console.WriteLine($"Book '{bookName}' removed from your shelf.");

                } else if (bookCmd == "End") {
                    continue;

                } else if (bookCmd == "See shelf") {
                    // Logic to see the shelf
                    Console.WriteLine("Displaying your shelf...");
                    // Here you would typically fetch and display the list of books on the shelf

                }else {
                    Console.WriteLine("Invalid command.");
                    continue;
                }

            } while (bookCmd != "End");
        } 
        else if (mode == "Reading Progress") {
            // Logic to view the reading progress
            string bookCmd;

            do {
                // Show the reading progress
                AnsiConsole.Write(new BarChart()
                        .Width(60)
                        .Label("[green bold underline]Reading Progress[/]")
                        .CenterLabel()
                        .AddItem("Books Read", 2, Color.Green)
                        .AddItem("Annual Reading Goal", dataManager.ReadingGoal, Color.Red)
                        .AddItem("Books Left", dataManager.ReadingGoal - 2, Color.Blue));
                
                bookCmd = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Select a book command:")
                        .AddChoices(new[] {
                            "Update Annual Reading Goal", "End"
                        })
                );
                
                if (bookCmd == "Update Annual Reading Goal") {
                    Console.WriteLine($"Your current annual reading goal is {dataManager.ReadingGoal}");
                    string goalInput = AskForInput("Enter your new annual reading goal (optional, press enter to skip):");
                    
                    // If the user skips (blank input), use the existing reading goal
                    if (string.IsNullOrWhiteSpace(goalInput)) {
                        Console.WriteLine($"No changes made. Keeping the existing reading goal of {dataManager.ReadingGoal}.");
                    }
                    else if (int.TryParse(goalInput, out int parsedGoal) && parsedGoal > 0) {
                        // If the input is valid and greater than 0, update the reading goal
                        dataManager.SetReadingGoal(parsedGoal);
                        Console.WriteLine($"Your new annual reading goal is {dataManager.ReadingGoal}");
                    }
                    else {
                        // If the input is invalid, keep the existing reading goal
                        Console.WriteLine($"Invalid input. Keeping the existing reading goal of {dataManager.ReadingGoal}.");
                    }

                } else if (bookCmd == "End") {
                    continue;

                }else {
                    Console.WriteLine("Invalid command.");
                    continue;
                }

            } while (bookCmd != "End");
        }
    }

    public static string AskForInput(string message) 
    {
        return AnsiConsole.Prompt(new TextPrompt<string>(message).AllowEmpty()
        );
        /* Console.Write(message);
        string input = Console.ReadLine()?.Trim().ToLower() ?? string.Empty;
        return input; */
    }
}
