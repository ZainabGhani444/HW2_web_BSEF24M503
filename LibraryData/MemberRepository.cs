using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryData
{
    public class MemberRepository
    {
        private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=LibraryDb;Integrated Security=True";

        public bool AddMember(Member member)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            string query = "insert into members(name) values(@n)";
            SqlParameter name = new SqlParameter("n", member.Name);
            SqlCommand sqlCommand = new SqlCommand(query, conn);
            sqlCommand.Parameters.Add(name);
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

        public List<Member> ReadMembers()
        {
            List<Member> list = new List<Member>();
            SqlConnection conn = new SqlConnection(connectionString);
            string query = "select * from members";
            SqlCommand sqlCommand = new SqlCommand(query, conn);
            conn.Open();
            SqlDataReader dr = sqlCommand.ExecuteReader();
            while (dr.Read())
            {
                Member m = new Member
                {
                    MemberId = Convert.ToInt32(dr["MemberId"]),
                    Name = dr["Name"].ToString()
                };
                list.Add(m);
            }
            conn.Close();
            return list;
        }
    }
}
