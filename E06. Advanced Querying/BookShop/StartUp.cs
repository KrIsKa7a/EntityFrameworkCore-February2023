namespace BookShop
{
    using System.Diagnostics;
    using System.Text;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Models.Enums;

    public class StartUp
    {
        public static void Main()
        {
            // EF 6:
            // AsNoTracking() -> Detach collection/entity from the ChangeTracker
            // Any changes made will not be saved
            // ToArray()/ToList() -> Materialize the query
            // Any code that we write later it will not be executed in the DB as SQl
            // The code after materialization is executed locally on the machine in RAM
            using var dbContext = new BookShopContext();
            //DbInitializer.ResetDatabase(dbContext);

            //string input = Console.ReadLine();
            Stopwatch sw = Stopwatch.StartNew();
            IncreasePrices(dbContext);
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);

            //Console.WriteLine(result);
        }

        // Problem 02
        public static string GetBooksByAgeRestriction(BookShopContext dbContext, string command)
        {
            try
            {
                AgeRestriction ageRestriction = Enum.Parse<AgeRestriction>(command, true);

                string[] bookTitles = dbContext.Books
                    .Where(b => b.AgeRestriction == ageRestriction)
                    .OrderBy(b => b.Title)
                    .Select(b => b.Title)
                    .ToArray();
                return string.Join(Environment.NewLine, bookTitles);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        // Problem 03
        public static string GetGoldenBooks(BookShopContext dbContext)
        {
            string[] bookTitles = dbContext.Books
                .Where(b => b.EditionType == EditionType.Gold &&
                            b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, bookTitles);
        }

        // Problem 06
        public static string GetBooksByCategory(BookShopContext dbContext, string input)
        {
            // In-memory, we are still not approaching the DB
            string[] categories = input
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.ToLower())
                .ToArray();

            string[] bookTitles = dbContext.Books
                .Where(b => b.BookCategories
                    .Any(bc => categories.Contains(bc.Category.Name.ToLower())))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, bookTitles);
        }

        // Problem 08
        public static string GetAuthorNamesEndingIn(BookShopContext dbContext, string input)
        {
            string[] authorNames = dbContext.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .OrderBy(a => a.FirstName)
                .ThenBy(a => a.LastName)
                .Select(a => $"{a.FirstName} {a.LastName}")
                .ToArray();

            return string.Join(Environment.NewLine, authorNames);
        }

        // Problem 12
        public static string CountCopiesByAuthor(BookShopContext dbContext)
        {
            StringBuilder sb = new StringBuilder();
            var authorsWithBookCopies = dbContext.Authors
                .Select(a => new
                {
                    FullName = a.FirstName + " " + a.LastName,
                    TotalCopies = a.Books
                        .Sum(b => b.Copies)
                })
                .ToArray()
                .OrderByDescending(b => b.TotalCopies); // This is optimization

            foreach (var a in authorsWithBookCopies)
            {
                sb.AppendLine($"{a.FullName} - {a.TotalCopies}");
            }

            return sb.ToString().TrimEnd();
        }

        // Problem 13
        public static string GetTotalProfitByCategory(BookShopContext dbContext)
        {
            StringBuilder sb = new StringBuilder();
            var categoriesWithProfit = dbContext.Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    TotalProfit = c.CategoryBooks
                        .Sum(cb => cb.Book.Copies * cb.Book.Price)
                })
                .ToArray()
                .OrderByDescending(c => c.TotalProfit)
                .ThenBy(c => c.CategoryName);

            foreach (var c in categoriesWithProfit)
            {
                sb.AppendLine($"{c.CategoryName} ${c.TotalProfit:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        // Problem 14
        public static string GetMostRecentBooks(BookShopContext dbContext)
        {
            StringBuilder sb = new StringBuilder();

            var categoriesWithMostRecentBooks = dbContext.Categories
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    CategoryName = c.Name,
                    MostRecentBooks = c.CategoryBooks
                        .OrderByDescending(cb => cb.Book.ReleaseDate)
                        .Take(3) // This can lower network load
                        .Select(cb => new
                        {
                            BookTitle = cb.Book.Title,
                            ReleaseYear = cb.Book.ReleaseDate.Value.Year
                        })
                        .ToArray()
                })
                .ToArray();

            foreach (var c in categoriesWithMostRecentBooks)
            {
                sb.AppendLine($"--{c.CategoryName}");

                foreach (var b in c.MostRecentBooks/*.Take(3) This is lowering query complexity*/)
                {
                    sb.AppendLine($"{b.BookTitle} ({b.ReleaseYear})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        // Problem 15
        public static void IncreasePrices(BookShopContext dbContext)
        {
            // Materializing the query does not detach entities from Change Tracker
            Book[] bookReleasedBefore2010 = dbContext
                .Books
                .Where(b => b.ReleaseDate.HasValue &&
                            b.ReleaseDate.Value.Year < 2010)
                .ToArray();

            // Using BatchUpdate from EFCore.Extensions
            //dbContext
            //    .Books
            //    .Where(b => b.ReleaseDate.HasValue &&
            //                b.ReleaseDate.Value.Year < 2010)
            //    .UpdateFromQuery(b => new Book() { Price = b.Price + 5 });

            foreach (var book in bookReleasedBefore2010)
            {
                book.Price += 5;
            }

            // Using SaveChanges() -> 4544ms
            // Using BulkUpdate() -> 3677ms
            dbContext.BulkUpdate(bookReleasedBefore2010);
        }
    }
}


