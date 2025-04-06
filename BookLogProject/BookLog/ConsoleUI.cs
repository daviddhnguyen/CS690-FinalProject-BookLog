namespace BookLog;

using BookLog;
using Spectre.Console;

public class ConsoleUI {
    DataManager dataManager;

    public ConsoleUI() {
        dataManager = new DataManager();
    }

    public void Show() {
        string mode;
        
        do {
            // Home page
            mode = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select a mode:")
                    .AddChoices(new[] {
                        "Book Shelf", "Reading Progress", "End"
                    })
            );

            if (mode == "Book Shelf") {

                string bookCmd;

                do {
                    // Logic to view the book shelf
                    DisplayBooks(dataManager);

                    // shelf end
                    bookCmd = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("Select a book command:")
                            .AddChoices(new[] {
                                "Add a book", /* "Edit a book", "View notes for a book", */ "Remove a book", "Home"
                            })
                    );
                    
                    if (bookCmd == "Add a book") {
                        string bookName = AskForInput("Enter the name of the book:");
                        string authorName = AskForInput("Enter the author of the book (optional, press enter to skip):");
                        if (string.IsNullOrWhiteSpace(authorName)) authorName = "Unknown";

                        string pageCountInput = AskForInput("Enter the page count of the book (optional, press enter to skip):");
                        int pageCount = int.TryParse(pageCountInput, out int parsedPageCount) ? parsedPageCount : 0;

                        string isbnInput = AskForInput("Enter the ISBN of the book (optional, press enter to skip):");
                        string isbn = string.IsNullOrWhiteSpace(isbnInput) ? "Unknown" : isbnInput;

                        DateOnly dateAdded = DateOnly.FromDateTime(DateTime.Now);
                        bool read = AnsiConsole.Confirm("Have you read this book?");
                        bool owned = AnsiConsole.Confirm("Do you own this book?");
                        
                        string dateFinishedInput = AskForInput("Enter the date you finished reading this book (yyyy-MM-dd, optional, press enter to skip):");
                        DateOnly? dateFinished = string.IsNullOrWhiteSpace(dateFinishedInput) ? null : DateOnly.Parse(dateFinishedInput);
                        
                        string? note = AskForInput("Add a note for this book (optional, press enter to skip):");

                        Book bookDetails = new Book(bookName, authorName, pageCount, isbn);
                        LibraryEntry entry = new LibraryEntry(bookDetails, dateAdded, dateFinished, read, owned, note);

                        dataManager.AddLibraryEntry(entry);

                        Console.WriteLine($"'{bookName}' added to your shelf.");
                    } else if (bookCmd == "Edit a book") {
                        string bookName =
                        AskForInput("Enter the name of the book to edit a book:");
                        // Logic to edit a book on the shelf

                    } else if (bookCmd == "View notes for a book") {
                        string bookName =
                        AskForInput("Enter the name of the book to view its notes:");
                        // Logic to view the view notes for a book
                        Console.WriteLine($"Displaying view notes for a book for the book '{bookName}':");
                        // Here you would typically fetch and display the view notes for a book for the book

                    } else if (bookCmd == "Remove a book") {
                        if (dataManager.LibraryEntries.Count == 0) {
                            Console.WriteLine("No books available to remove.");
                            return;
                        }

                        string bookTitleToRemove = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("Select the book to remove:")
                                .AddChoices(dataManager.LibraryEntries.Select(entry => entry.Book.Title))
                        );

                        try {
                            dataManager.RemoveLibraryEntry(bookTitleToRemove);
                            Console.WriteLine($"Book '{bookTitleToRemove}' has been removed from your shelf.");
                        } catch (ArgumentException ex) {
                            Console.WriteLine(ex.Message);
                        }
                    } else if (bookCmd == "End") {
                        continue;

                    }else {
                        Console.WriteLine("Invalid command.");
                        continue;
                    }

                } while (bookCmd != "Home");
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
                                "Update Annual Reading Goal", "Home"
                            })
                    );
                    
                    if (bookCmd == "Update Annual Reading Goal") {
                        Console.WriteLine($"Your current annual reading goal is {dataManager.ReadingGoal}");
                        string goalInput =
                        AskForInput("Enter your new annual reading goal (optional, press enter to skip):");
                        
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

                } while (bookCmd != "Home");
            }
        } while (mode != "End");
    }
    public static string AskForInput(string message) 
    {
        return AnsiConsole.Prompt(new TextPrompt<string>(message).AllowEmpty()
        );
    }

    public void DisplayBooks(DataManager dataManager) {
        var table = new Table();
        table.AddColumn("Title");
        table.AddColumn("Author");
        table.AddColumn("Page Count");
        table.AddColumn("ISBN");
        table.AddColumn("Read");
        table.AddColumn("Owned");
        table.AddColumn("Date Added");
        table.AddColumn("Date Finished");
        table.AddColumn("Note");


        foreach (var entry in dataManager.LibraryEntries) {
            var book = entry.Book;
            table.AddRow(
                book.Title,
                book.Author,
                book.PageCount.ToString(),
                book.ISBN,
                entry.Read ? "Yes" : "No",
                entry.Owned ? "Yes" : "No",
                entry.DateAdded?.ToString() ?? "Unknown",
                entry.DateFinished?.ToString() ?? "Not Finished",
                entry.Note ?? "No Note"
            );
        }

        AnsiConsole.Write(table);
    }
}
