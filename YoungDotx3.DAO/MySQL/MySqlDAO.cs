using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;
using MySql.Data.Types;

namespace YoungDotx3.DAO.MySQL
{
    public abstract class MySqlDAO
    {
        protected delegate void ReadAllDelegate<in T>(T type, MySqlDataReader reader);
        protected MySqlConnection connection;
        protected MySqlTransaction transaction;

        public MySqlConnection Connection
        {
            get { return connection; }
            set { this.connection = value; }
        }

        public void Open(string connString)
        {
            if (Connection == null)
                connection = new MySqlConnection(connString);
            if (Connection.State != ConnectionState.Open)
                Connection.Open();
        }

        public void Close()
        {
            if (Connection != null && Connection.State != ConnectionState.Closed)
                Connection.Close();
        }

        public void BeginTransaction()
        {
            if (Connection.State != ConnectionState.Open)
                Connection.Open();
            transaction = Connection.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void RollBackTransaction()
        {
            if (transaction != null)
            {
                transaction.Rollback();
                transaction = null;
            }
        }

        public void CommitTransaction()
        {
            transaction.Commit();
            transaction = null;
        }

        private IDictionary<MySqlDataReader, IDictionary<string, int>> cache =
            new Dictionary<MySqlDataReader, IDictionary<string, int>>();

        private int GetIndex(MySqlDataReader reader, string tablename, string columnName)
        {
            bool isContainReader = cache.ContainsKey(reader);
            if (!isContainReader)
                cache.Add(reader, new Dictionary<string, int>());
            IDictionary<string, int> cachedData = cache[reader];
            if (cachedData.ContainsKey(tablename + columnName))
                return cachedData[tablename + columnName];

            DataTable table = reader.GetSchemaTable();
            int index = -1;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (table.Rows[i]["BaseTableName"].ToString() == tablename &&
                    table.Rows[i]["ColumnName"].ToString() == columnName)
                {
                    index = ((int)table.Rows[i]["ColumnOrdinal"]) - 1;
                    break;
                }
            }
            cachedData.Add(tablename + columnName, index);
            return index;
        }

        protected bool GetBool(MySqlDataReader reader, string tablename, string columnName)
        {
            int index = GetIndex(reader, tablename, columnName);
            return reader.GetBoolean(index);
        }

        protected DateTime GetDateTime(MySqlDataReader reader, string tablename, string columnName)
        {
            int index = GetIndex(reader, tablename, columnName);
            if (index < 0)
                return DateTime.MinValue;
            //MySQL Bug List #29100
            try
            {
                if (reader.IsDBNull(index))
                    return DateTime.MinValue;
            }
            catch
            {
                return DateTime.MinValue;
            }

            MySqlDateTime time = reader.GetMySqlDateTime(index);
            if (time.IsValidDateTime)
                return time.GetDateTime();
            return DateTime.MinValue;
        }

        protected string GetString(MySqlDataReader reader, string tablename, string columnName)
        {
            int index = GetIndex(reader, tablename, columnName);
            //string name = reader.GetName(index);
            //int ord = reader.GetOrdinal(name);

            return reader[index] == DBNull.Value ? null : reader[index].ToString();
        }

        protected int? GetInt(MySqlDataReader reader, string tablename, string columnName)
        {
            int index = GetIndex(reader, tablename, columnName);
            if (reader[index] == DBNull.Value)
                return null;
            return Int32.Parse(reader[index].ToString());
        }

        protected char GetChar(MySqlDataReader reader, string tablename, string columnName)
        {
            int index = GetIndex(reader, tablename, columnName);
            if (reader[index] != DBNull.Value)
            {
                if (reader[index].ToString().Trim() != "")
                    return reader.GetChar(index);
                return Char.MinValue;
            }
            return Char.MinValue;
        }

        protected double GetDouble(MySqlDataReader reader, string tablename, string columnName)
        {
            int index = GetIndex(reader, tablename, columnName);
            return reader[index] == DBNull.Value ? Double.NaN : (double)reader.GetDecimal(index);
        }

        protected static bool HasColumn(MySqlDataReader Reader, string ColumnName)
        {
            foreach (DataRow row in Reader.GetSchemaTable().Rows)
            {
                if (row["ColumnName"].ToString() == ColumnName)
                    return true;
            } //Still here? Column not found. 
            return false;
        }

        /// <summary>
        /// 用指定的指令選出複數的資料類別
        /// </summary>
        /// <typeparam name="T">所要選出的類別, 此類別需要有可用的0參數建構式</typeparam>
        /// <param name="commandText">用來選出資料所用到的指令</param>
        /// <param name="parameters">指令中用到的所有參數，如果不需任何參數就給null值</param>
        /// <param name="readAllDelegate">用來將DB查出來的資料轉換指定類別的方法，需符合ReadAllDelegate的格式</param>
        /// <returns></returns>
        protected List<T> ReadMultiple<T>(string commandText, MySqlParameter[] parameters, ReadAllDelegate<T> readAllDelegate) where T : new()
        {
            if (String.IsNullOrEmpty(commandText) || readAllDelegate == null)
            {
                return null;
            }

            var result = new List<T>();
            var command = connection.CreateCommand();
            command.CommandText = commandText;
            if (parameters != null && parameters.Any())
            {
                command.Parameters.AddRange(parameters);
            }

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var type = new T();
                    readAllDelegate(type, reader);
                    result.Add(type);
                }

                reader.Close();
            }
            return result;
        }

        /// <summary>
        /// 選出在單一表單中所有指定id的資料類別，此為簡單的通用函數
        /// </summary>
        /// <param name="ids">所有要查詢的id</param>
        /// <param name="tableName">表單的名字</param>
        /// <param name="idColumnName">表單中id欄位的名字</param>
        /// <param name="readAllDelegate">用來將DB查出來的資料轉換指定類別的方法，需符合ReadAllDelegate的格式</param>
        /// <returns></returns>
        protected List<T> GenericSelectById<T>(List<string> ids, string tableName, string idColumnName, ReadAllDelegate<T> readAllDelegate) where T : new()
        {
            var commandText = "SELECT * FROM {0} WHERE {1} IN ({2})";
            var parameters = new MySqlParameter[ids.Count];

            for (var i = 0; i < ids.Count; i++)
            {
                parameters[i] = new MySqlParameter(string.Format("?{0}{1}", idColumnName, i), ids[i]);
            }

            commandText = string.Format(commandText, tableName, idColumnName, string.Join(",", parameters.Select(p => p.ParameterName)));

            return ReadMultiple(commandText, parameters, readAllDelegate);
        }

        /// <summary>
        /// 刪掉表單中所有指定id的資料，此為簡單的通用函數
        /// </summary>
        /// <param name="ids">所有要刪掉的資料id</param>
        /// <param name="tableName">表單的名字</param>
        /// <param name="idColumnName">表單中id欄位的名字</param>
        /// <returns>受影響的資料筆數</returns>
        protected int GenericDelete(List<string> ids, string tableName, string idColumnName)
        {
            var command = connection.CreateCommand();
            const string commandText = "DELETE FROM {0} WHERE {1} IN ({2})";

            for (var i = 0; i < ids.Count; i++)
            {
                command.Parameters.AddWithValue(string.Format("?{0}{1}", idColumnName, i), ids[i]);
            }

            command.CommandText = string.Format(
                commandText,
                tableName,
                idColumnName,
                string.Join(",", command.Parameters.Cast<MySqlParameter>().Select(p => p.ParameterName)));

            return command.ExecuteNonQuery();
        }

        public void SetConnections(MySqlDAO[] to)
        {
            foreach (var aDAO in to)
            {
                aDAO.Connection = Connection;
            }
        }
    }
}
