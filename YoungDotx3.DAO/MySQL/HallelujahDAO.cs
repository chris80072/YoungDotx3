using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using YoungDotx3.Domain.Hallelujah;

namespace YoungDotx3.DAO.MySQL
{
    public class HallelujahDAO : MySqlDAO
    {
        public static void ReadAll(Message message, MySqlDataReader reader)
        {
            message.Id = reader["Id"].ToString();
            message.Nickname = reader["Nickname"].ToString();
            message.Content = reader["Content"].ToString();
            message.IpAddress = reader["IpAddress"].ToString();
            message.CreateDate = Convert.ToDateTime(reader["CreateDate"]);
            message.IsDelete = Convert.ToBoolean(reader["IsDelete"]);
        }

        public bool Insert(Message message)
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO hallelujah (NickName, Content, IpAddress) VALUES(?NickName, ?Content, ?IpAddress);";
            command.Parameters.AddWithValue("?NickName", message.Nickname);
            command.Parameters.AddWithValue("?Content", message.Content);
            command.Parameters.AddWithValue("?IpAddress", message.IpAddress);
            int num = command.ExecuteNonQuery();
            message.Id = command.LastInsertedId.ToString();
            return num > 0;
        }

        public bool InsertBatch(List<Message> messages)
        {
            if (messages.Count.Equals(0))
                return false;

            MySqlCommand command = connection.CreateCommand();
            StringBuilder builder = new StringBuilder();
            builder.Append("INSERT INTO hallelujah (NickName, Content, IpAddress, CreateDate) VALUES ");

            for (int i = 0; i < messages.Count; i++)
            {
                builder.Append($"(?NickName{i}, ?Content{i}, ?IpAddress{i}, ?CreateDate{i}),");
                command.Parameters.AddWithValue($"?NickName{i}", messages[i].Nickname);
                command.Parameters.AddWithValue($"?Content{i}", messages[i].Content);
                command.Parameters.AddWithValue($"?IpAddress{i}", messages[i].IpAddress);
                command.Parameters.AddWithValue($"?CreateDate{i}", messages[i].CreateDate);
            }

            var strSQL = builder.ToString();
            command.CommandText = strSQL.Substring(0, strSQL.Length - 1);
            int num = command.ExecuteNonQuery();
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

        public List<Message> SeleteByPage(string id)
        {
            List<Message> result = new List<Message>();
            MySqlCommand command = connection.CreateCommand();
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT * FROM hallelujah WHERE IsDelete = FALSE ");
            if (!string.IsNullOrEmpty(id))
            {
                builder.Append("AND Id < ?Id ");
                command.Parameters.AddWithValue("?Id", id);
            }
            
            builder.Append("ORDER BY Id DESC ");
            builder.Append("LIMIT 20;");
            
            command.CommandText = builder.ToString();
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
