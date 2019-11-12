﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace bookPjt
{
    class BookDAO
    {
        private static BookDAO bookDAO = null;
        private string dbInfo = "Server=localhost;Database=library;Uid=root;Pwd=apmsetup;";
        public static BookDAO getInstance()
        {
            if (bookDAO == null)
                bookDAO = new BookDAO();
            return bookDAO;
        }

        private BookDAO()
        {

        }

        public bool deleteBook(int bookid)
        {
            MySqlConnection mySqlConnection = new MySqlConnection(dbInfo);

            try
            {
                mySqlConnection.Open();
                string sql = "delete from book where b_idx = " + bookid;
                MySqlCommand mysqlCommand = new MySqlCommand(sql, mySqlConnection);
                mysqlCommand.ExecuteNonQuery();
                sql = "delete from category where ct_b_id = " + bookid;
                mysqlCommand = new MySqlCommand(sql, mySqlConnection);
                mysqlCommand.ExecuteNonQuery(); 
                sql = "delete from publisher where p_b_id = " + bookid;
                mysqlCommand = new MySqlCommand(sql, mySqlConnection);
                mysqlCommand.ExecuteNonQuery();
                mySqlConnection.Close();
            }
            catch (Exception e)
            {
                return false;
                MessageBox.Show(e.Message);
            }
            return true;
        }

        public bool updateBook(int bookid, int stock)
        {
            MySqlConnection mySqlConnection = new MySqlConnection(dbInfo);

            string sql = "update book set b_stock = " + stock + " where b_idx = " + bookid;

            try
            {
                mySqlConnection.Open();
                MySqlCommand mysqlCommand = new MySqlCommand(sql, mySqlConnection);
                mysqlCommand.ExecuteNonQuery();

                mySqlConnection.Close();
            }
            catch (Exception e)
            {
                return false;
                MessageBox.Show(e.Message);
            }
            return true;
        }

        public BookDTO selectBook(int bookid)
        {
            MySqlConnection mySqlConnection = new MySqlConnection(dbInfo);
            BookDTO bookDTO = null;

            string sql = "select b_idx ,b_name,b_stock,b_author, p_n_name , c_n_name , b_summary , b_img";
            sql += " from book, category, categoryName, publisher, publisherName";
            sql += " where book.b_idx = category.ct_b_id and book.b_idx = publisher.p_b_id";
            sql += " and category.ct_idx = categoryName.c_n_idx and publisher.p_idx = publisherName.p_n_idx and b_idx = " + bookid;

            try
            {
                mySqlConnection.Open();
                MySqlCommand mysqlCommand = new MySqlCommand(sql, mySqlConnection);
                MySqlDataReader rdr = mysqlCommand.ExecuteReader();
                while (rdr.Read())
                {
                    bookDTO = new BookDTO(
                        Convert.ToInt32(rdr[0]),
                        Convert.ToString(rdr[1]),
                        Convert.ToInt32(rdr[2]),
                        Convert.ToString(rdr[3]),
                        Convert.ToString(rdr[4]),
                        Convert.ToString(rdr[5]),
                        Convert.ToString(rdr[6]),
                        Convert.ToString(rdr[7])
                        );
                }
                mySqlConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return bookDTO;
        }

        private int resultPublisher(string publisher)
        {
            MySqlConnection mySqlConnection = new MySqlConnection(dbInfo);
            int result = 0;

            string sql = "select p_n_idx from publisherName where p_n_name = '" + publisher + "'";
            try
            {
                mySqlConnection.Open();
                MySqlCommand mysqlCommand = new MySqlCommand(sql, mySqlConnection);
                MySqlDataReader rdr = mysqlCommand.ExecuteReader();
                while (rdr.Read())
                {
                    result = Convert.ToInt32(rdr[0]);
                }
                mySqlConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return result;
        }

        private int resultCategory(string category)
        {
            MySqlConnection mySqlConnection = new MySqlConnection(dbInfo);
            int result = 0;

            string sql = "select c_n_idx from categoryname where c_n_name = '" + category + "'";
            try
            {
                mySqlConnection.Open();
                MySqlCommand mysqlCommand = new MySqlCommand(sql, mySqlConnection);
                MySqlDataReader rdr = mysqlCommand.ExecuteReader();
                while (rdr.Read())
                {
                    result = Convert.ToInt32(rdr[0]);
                }
                mySqlConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return result;
        }

        public bool insertBook(BookDTO bookDTO)
        {
            MySqlConnection mySqlConnection = new MySqlConnection(dbInfo);

            string b_name = bookDTO.B_name;
            string b_author = bookDTO.B_author;
            string b_publisher = bookDTO.B_puBlisher;
            string b_category = bookDTO.B_category;
            string b_summery = bookDTO.B_summary;
            string b_img = bookDTO.B_img;
            int b_stock = bookDTO.B_stock;

            try
            {
                string sql = "insert into book values (NULL,'" + b_name + "'," + b_stock + ",'" + b_author + "','" + b_summery + "','" + b_img + "')";

                mySqlConnection.Open();
                MySqlCommand mysqlCommand = new MySqlCommand(sql, mySqlConnection);
                mysqlCommand.ExecuteNonQuery();

                int categoryNum = resultCategory(b_category);
                int publisherNum = resultPublisher(b_publisher);

                sql = "insert into category values (" + categoryNum + ",NULL)";
                mysqlCommand = new MySqlCommand(sql, mySqlConnection);
                mysqlCommand.ExecuteNonQuery();

                sql = "insert into publisher values (" + publisherNum + ",NULL)";
                mysqlCommand = new MySqlCommand(sql, mySqlConnection);
                mysqlCommand.ExecuteNonQuery();

                mySqlConnection.Close();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
            return true;
        }
        public List<string> getPublisherList()
        {
            List<string> list = new List<string>();
            MySqlConnection mySqlConnection = new MySqlConnection(dbInfo);

            string sql = "SELECT p_n_name FROM publisherName";
            try
            {
                mySqlConnection.Open();
                MySqlCommand mysqlCommand = new MySqlCommand(sql, mySqlConnection);
                MySqlDataReader rdr = mysqlCommand.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(rdr[0].ToString());
                }
                mySqlConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return list;
        }

        public List<string> getCategoryList()
        {
            List<string> list = new List<string>();
            MySqlConnection mySqlConnection = new MySqlConnection(dbInfo);

            string sql = "SELECT c_n_name FROM categoryName";
            try
            {
                mySqlConnection.Open();
                MySqlCommand mysqlCommand = new MySqlCommand(sql, mySqlConnection);
                MySqlDataReader rdr = mysqlCommand.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(rdr[0].ToString());
                }
                mySqlConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return list;
        }

        public List<BookDTO> selectList()
        {
            List<BookDTO> list = new List<BookDTO>();
            MySqlConnection mySqlConnection = new MySqlConnection(dbInfo);

            string sql = "select b_idx ,b_name,b_stock,b_author, p_n_name , c_n_name, b_summary, b_img";
            sql += " from book, category, categoryName, publisher, publisherName";
            sql += " where book.b_idx = category.ct_b_id and book.b_idx = publisher.p_b_id";
            sql += " and category.ct_idx = categoryName.c_n_idx and publisher.p_idx = publisherName.p_n_idx";
            try
            {
                mySqlConnection.Open();
                MySqlCommand mysqlCommand = new MySqlCommand(sql, mySqlConnection);
                MySqlDataReader rdr = mysqlCommand.ExecuteReader();
                while (rdr.Read())
                {
                    BookDTO bookDTO = new BookDTO(
                        Convert.ToInt32(rdr[0]),
                        Convert.ToString(rdr[1]),
                        Convert.ToInt32(rdr[2]),
                        Convert.ToString(rdr[3]),
                        Convert.ToString(rdr[4]),
                        Convert.ToString(rdr[5]),
                        Convert.ToString(rdr[6]),
                        Convert.ToString(rdr[7]));
                    list.Add(bookDTO);
                }
                mySqlConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return list;
        }

        public bool insertCategory(string categoryName)
        {
            MySqlConnection mySqlConnection = new MySqlConnection(dbInfo);
            try
            {
                mySqlConnection.Open();
                string sql = "insert into categoryName values (NULL,'" + categoryName + "')";
                MySqlCommand mysqlCommand = new MySqlCommand(sql, mySqlConnection);
                mysqlCommand.ExecuteNonQuery();
                mySqlConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
            return true;
        }

        public bool insertPublisher(string publisherName)
        {
            MySqlConnection mySqlConnection = new MySqlConnection(dbInfo);
            try
            {
                mySqlConnection.Open();
                string sql = "insert into publisherName values (NULL,'" + publisherName + "')";
                MySqlCommand mysqlCommand = new MySqlCommand(sql, mySqlConnection);
                mysqlCommand.ExecuteNonQuery();
                mySqlConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
            return true;
        }
    }
}