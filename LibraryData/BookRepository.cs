using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;

namespace LibraryData
{
    public class BookRepository
    {
        private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=LibraryDb;Integrated Security=True";

        public bool AddBook(Book book)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            string query = "insert into books(title,author,stock) values(@t,@a,@s)";
            SqlParameter title = new SqlParameter("t", book.Title);
            SqlParameter author = new SqlParameter("a", book.Author);
            SqlParameter stock = new SqlParameter("s", book.Stock);
            SqlCommand sqlCommand = new SqlCommand(query, conn);
            sqlCommand.Parameters.Add(title);
            sqlCommand.Parameters.Add(author);
            sqlCommand.Parameters.Add(stock);
            conn.Open();
            int i = sqlCommand.ExecuteNonQuery();
            conn.Close();
            if (i > 0)
            {
                // Console.WriteLine("Book added successfully");
                return true;
            }
            else
            {
                //Console.WriteLine("Book is not added");
                return false;
            }
        }

        public List<Book> ReadBook()
        {
            List<Book> list = new List<Book>();
            SqlConnection conn = new SqlConnection(connectionString);
            string query = "select * from books";
            SqlCommand sqlCommand = new SqlCommand(query, conn);
            conn.Open();
            SqlDataReader dr = sqlCommand.ExecuteReader();
            while (dr.Read())
                {
                    Book b = new Book
                    {
                        BookId = Convert.ToInt32(dr["BookId"]),
                        Title = dr["Title"].ToString(),
                        Author = dr["Author"].ToString(),
                        Stock = Convert.ToInt32(dr["Stock"])
                    };
                    list.Add(b);
                }
            conn.Close();
            return list;
        }

        public bool UpdateStock(int s,int d)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            string query = "update books set stock=@s where BookId=@i";
            SqlParameter id= new SqlParameter("i", d);
            SqlParameter stock = new SqlParameter("s", s);
            SqlCommand sqlCommand = new SqlCommand(query, conn);
            sqlCommand.Parameters.Add(id);
            sqlCommand.Parameters.Add(stock);
            conn.Open();
            int i = sqlCommand.ExecuteNonQuery();
            conn.Close();
            if (i > 0)
            {
                //Console.WriteLine("Stock updated successfully");
                return true;
            }
            else
            {
                //Console.WriteLine("Stock is not updated");
                return false;
            }
        }

        public bool RemoveBook(int d)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            string query = "delete from books where BookId=@i";
            SqlParameter id= new SqlParameter("i", d);
            SqlCommand sqlCommand = new SqlCommand(query, conn);
            sqlCommand.Parameters.Add(id);
            try
            {
                conn.Open();
                int i = sqlCommand.ExecuteNonQuery();
                conn.Close();
                return i > 0;
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                return false;
            }
            
        }
    }
}
