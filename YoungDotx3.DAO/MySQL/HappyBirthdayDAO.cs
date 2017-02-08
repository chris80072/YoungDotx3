using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using YoungDotx3.Domain.HappyBirthday;

namespace YoungDotx3.DAO.MySQL
{
    public class HappyBirthdayDAO : MySqlDAO
    {
        public static void ReadAll(Message message, MySqlDataReader reader)
        {
            message.Id = reader["Id"].ToString();
            message.Nickname = reader["Nickname"].ToString();
            message.Content = reader["Content"].ToString();
            message.ColorCode = reader["ColorCode"].ToString();
            message.IpAddress = reader["IpAddress"].ToString();
            message.Date = Convert.ToDateTime(reader["Date"]);
            message.CreateDate = Convert.ToDateTime(reader["CreateDate"]);
            message.IsDelete = Convert.ToBoolean(reader["IsDelete"]);
        }

        public bool Insert(Message message)
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO happybirthday (NickName, Content, ColorCode, IpAddress, Date) VALUES(?NickName, ?Content, ?ColorCode, ?IpAddress, ?Date);";
            command.Parameters.AddWithValue("?NickName", message.Nickname);
            command.Parameters.AddWithValue("?Content", message.Content);
            command.Parameters.AddWithValue("?ColorCode", message.ColorCode);
            command.Parameters.AddWithValue("?IpAddress", message.IpAddress);
            command.Parameters.AddWithValue("?Date", message.Date);
            int num = command.ExecuteNonQuery();
            message.Id = command.LastInsertedId.ToString();
            return num > 0;
        }

        public Message Selete(string id)
        {
            Message result = null;
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM happybirthday WHERE Id = ?Id;";
            command.Parameters.AddWithValue("?Id", id);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result = new Message();
                    ReadAll(result, reader);
                }
                reader.Close();
            }
            return result;
        }

        public List<Message> SeleteAll()
        {
            List<Message> result = new List<Message>();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM happybirthday WHERE IsDelete = false;";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Message message = new Message();
                    ReadAll(message, reader);
                    result.Add(message);
                }
                reader.Close();
            }
            return result;
        }
    }
}
