using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace LibraryData
{
    public class BackupHelper
    {
        public void SaveBooksBackup(List<Book> books, string filePath)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(books, options);

            File.WriteAllText(filePath, jsonString);
        }

        public List<Book> ReadBooksBackup(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new List<Book>();
            }

            string jsonString = File.ReadAllText(filePath);
            List<Book> books = JsonSerializer.Deserialize<List<Book>>(jsonString);

            return books;
        }
    }
}
