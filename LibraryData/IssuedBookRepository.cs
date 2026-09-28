using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace LibraryData
{
    public class IssuedBookRepository
    {
        private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=LibraryDb;Integrated Security=True";

        public bool IssueBook(int bookId, int memberId)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            string query1 = "select stock from books where BookId=@i";
            string query2 = "update books set stock=stock-1 where BookId=@i";
            string query3 = "insert into IssuedBooks(BookId,MemberID) values(@book,@member)";
            SqlParameter id = new SqlParameter("i", bookId);
            SqlParameter id2 = new SqlParameter("i", bookId);
            SqlParameter book = new SqlParameter("book", bookId);
            SqlParameter member = new SqlParameter("member", memberId);

            conn.Open();
            SqlTransaction sqlTransaction = conn.BeginTransaction();
            try
            {
                SqlCommand sqlCommand1 = new SqlCommand(query1, conn, sqlTransaction);
                sqlCommand1.Parameters.Add(id);
                SqlCommand sqlCommand2 = new SqlCommand(query2, conn, sqlTransaction);
                sqlCommand2.Parameters.Add(id2);
                SqlCommand sqlCommand3 = new SqlCommand(query3, conn, sqlTransaction);
                sqlCommand3.Parameters.Add(book);
                sqlCommand3.Parameters.Add(member);

                object result = sqlCommand1.ExecuteScalar();
                if (result != null && Convert.ToInt32(result) > 0)
                {
                    sqlCommand2.ExecuteNonQuery();
                    sqlCommand3.ExecuteNonQuery();
                    sqlTransaction.Commit();
                    conn.Close();
                    return true;
                }
                else
                {
                    sqlTransaction.Rollback();
                    conn.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                sqlTransaction.Rollback();
                conn.Close();
                return false;
            }
        }

        public bool ReturnBook(int issueId)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            string query1 = "select BookId,ReturnDate from IssuedBooks where IssueId=@i";
            string query2 = "update IssuedBooks set ReturnDate=GETDATE() where IssueId=@i";
            string query3 = "update books set stock=stock+1 where BookId=@i";
            SqlParameter id = new SqlParameter("i", issueId);
            SqlParameter id2 = new SqlParameter("i", issueId);
            conn.Open();
            SqlTransaction sqlTransaction = conn.BeginTransaction();
            try
            {
                SqlCommand sqlCommand1 = new SqlCommand(query1, conn, sqlTransaction);
                sqlCommand1.Parameters.Add(id);
                SqlCommand sqlCommand2 = new SqlCommand(query2, conn, sqlTransaction);
                sqlCommand2.Parameters.Add(id2);

                SqlCommand sqlCommand3 = new SqlCommand(query3, conn, sqlTransaction);
                SqlDataReader dr = sqlCommand1.ExecuteReader();

                int bookIdToUpdate = 0;
                bool canReturn = false;

                if (dr.Read())
                {
                    if (dr["ReturnDate"] == DBNull.Value)
                    {
                        bookIdToUpdate = Convert.ToInt32(dr["BookId"]);
                        canReturn = true;
                    }
                }
                dr.Close();
                SqlParameter id3 = new SqlParameter("i", bookIdToUpdate);
                sqlCommand3.Parameters.Add(id3);

                if (canReturn) {
                    try
                    {
                        sqlCommand2.ExecuteNonQuery();
                        sqlCommand3.ExecuteNonQuery();
                        sqlTransaction.Commit();
                        conn.Close();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        sqlTransaction.Rollback();
                        conn.Close();
                        return false;
                    }
                }
                else
                {
                    sqlTransaction.Rollback();
                    conn.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                sqlTransaction.Rollback();
                conn.Close();
                return false;
            }
        }   
    }
}