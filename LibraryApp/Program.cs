using System;
using System.Collections.Generic;
using LibraryData;

namespace LibraryApp
{
    class Program
    {
        static void Main(string[] args)
        {
            BookRepository bookRepo = new BookRepository();
            MemberRepository memberRepo = new MemberRepository();
            IssuedBookRepository issueRepo = new IssuedBookRepository();
            BackupHelper backupHelper = new BackupHelper();

            bool running = true;

            while (running)
            {
                Console.WriteLine("\n=== LIBRARY MANAGEMENT SYSTEM ===");
                Console.WriteLine("1. Add Book");
                Console.WriteLine("2. View All Books");
                Console.WriteLine("3. Add Member");
                Console.WriteLine("4. View All Members");
                Console.WriteLine("5. Issue Book to Member (Transaction)");
                Console.WriteLine("6. Return Book (Transaction)");
                Console.WriteLine("7. Export Books to JSON");
                Console.WriteLine("8. Import Books from JSON");
                Console.WriteLine("9. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    Console.WriteLine("--- Add Book ---");
                    Console.Write("Enter Book Title: ");
                    string title = Console.ReadLine();

                    Console.Write("Enter Author Name: ");
                    string author = Console.ReadLine();

                    Console.Write("Enter Stock: ");
                    int stock = Convert.ToInt32(Console.ReadLine());

                    Book b = new Book();
                    b.Title = title;
                    b.Author = author;
                    b.Stock = stock;

                    bool check=bookRepo.AddBook(b);
                    if (check)
                    {
                        Console.WriteLine("Book added successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Book is not added.");
                    }
                }

                else if (choice == "2")
                {
                    List<Book> books = bookRepo.ReadBook();
                    Console.WriteLine("--- View All Books ---");

                    if (books.Count == 0)
                    {
                        Console.WriteLine("No books found.");
                    }
                    else
                    {
                        foreach (Book b in books)
                        {
                            Console.WriteLine("ID: " + b.BookId + " | Title: " + b.Title + " | Author: " + b.Author + " | Stock: " + b.Stock);
                        }
                    }
                }

                else if (choice == "3")
                {
                    Console.WriteLine("--- Add Member ---");
                    Console.Write("Enter Member Name: ");
                    string name = Console.ReadLine();

                    Member m = new Member();
                    m.Name = name;

                    bool check =memberRepo.AddMember(m);
                    if (check)
                    {
                        Console.WriteLine("Member added successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Member is not added.");
                    }
                }

                else if (choice == "4")
                {
                    List<Member> members = memberRepo.ReadMembers();
                    Console.WriteLine("--- View All Members ---");

                    if (members.Count == 0)
                    {
                        Console.WriteLine("No members found.");
                    }
                    else
                    {
                        foreach (Member m in members)
                        {
                            Console.WriteLine("ID: " + m.MemberId + " | Name: " + m.Name);
                        }
                    }
                }

                else if (choice == "5")
                {
                    Console.WriteLine("--- Issue Book to Member ---");
                    Console.Write("Enter Book ID: ");
                    int bookId = Convert.ToInt32(Console.ReadLine());

                    Console.Write("Enter Member ID: ");
                    int memberId = Convert.ToInt32(Console.ReadLine());

                    bool result = issueRepo.IssueBook(bookId, memberId);

                    if (result == true)
                    {
                        Console.WriteLine("Book issued successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to issue book.");
                    }
                }

                else if (choice == "6")
                {
                    Console.WriteLine("--- Return Book ---");
                    Console.Write("Enter Issue ID: ");
                    int issueId = Convert.ToInt32(Console.ReadLine());

                    bool result = issueRepo.ReturnBook(issueId);

                    if (result == true)
                    {
                        Console.WriteLine("Book returned successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to return book.");
                    }
                }

                else if (choice == "7")
                {
                    Console.WriteLine("--- Books Backed up ---");
                    Console.Write("Enter file path to save (e.g., books.json): ");
                    string path = Console.ReadLine();

                    List<Book> books = bookRepo.ReadBook();
                    backupHelper.SaveBooksBackup(books, path);

                    Console.WriteLine("Books exported to JSON successfully.");
                }

                else if (choice == "8")
                {
                    Console.Write("Enter file path to read (e.g., books.json): ");
                    string path = Console.ReadLine();

                    List<Book> books = backupHelper.ReadBooksBackup(path);

                    Console.WriteLine("--- Imported Books Backup ---");
                    if (books.Count == 0)
                    {
                        Console.WriteLine("No data found or file does not exist.");
                    }
                    else
                    {
                        foreach (Book b in books)
                        {
                            Console.WriteLine("Title: " + b.Title + " | Stock: " + b.Stock);
                        }
                    }
                }
                else if (choice == "9")
                {
                    running = false;
                    Console.WriteLine("Exiting program...");
                }
                else
                {
                    Console.WriteLine("Invalid option. Please try again.");
                }
            }
        }
    }
}