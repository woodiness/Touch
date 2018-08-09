using System.Diagnostics;
using Microsoft.Data.Sqlite;

namespace Touch.Data
{
    /// <summary>
    ///     回忆列表 数据库
    /// </summary>
    public class MemoryListDatabase : DatabaseBase
    {
        /// <summary>
        ///     表名
        /// </summary>
        public const string TableName = "MemoryListTable";

        /// <summary>
        ///     主键名
        /// </summary>
        public const string PrimaryKeyName = "Primary_Key";

        /// <summary>
        ///     回忆名
        /// </summary>
        private const string MemoryNameName = "Memory_Name";

        public MemoryListDatabase(string dbFileName) : base(dbFileName)
        {
        }

        /// <summary>
        ///     创建表
        /// </summary>
        public void Create()
        {
            const string createCommandStr
                = "CREATE TABLE IF NOT EXISTS " + TableName + " ("
                  + PrimaryKeyName + " INTEGER PRIMARY KEY AUTOINCREMENT, "
                  + MemoryNameName + " NVARCHAR(2048) NOT NULL)";
            Create(createCommandStr);
        }

        /// <summary>
        ///     删除表
        /// </summary>
        public void Drop()
        {
            const string dropCommandStr = "DROP TABLE IF EXISTS " + TableName;
            Drop(dropCommandStr);
        }

        /// <summary>
        ///     返回所有记录
        /// </summary>
        /// <returns>所有的记录</returns>
        public SqliteDataReader GetQuery()
        {
            const string selectCommandStr = "SELECT * FROM " + TableName;
            return GetQuery(selectCommandStr);
        }

        /// <summary>
        ///     添加一条记录
        /// </summary>
        /// <param name="memoryName">回忆名字</param>
        public void Insert(string memoryName)
        {
            using (var db = new SqliteConnection("Filename=" + DbFileName))
            {
                db.Open();
                var insertCommand = new SqliteCommand
                {
                    Connection = db,
                    CommandText = "INSERT INTO " + TableName + " VALUES (NULL, @" + MemoryNameName + ")"
                };
                insertCommand.Parameters.AddWithValue("@" + MemoryNameName, memoryName);
                try
                {
                    insertCommand.ExecuteReader();
                }
                catch (SqliteException exception)
                {
                    Debug.WriteLine(exception);
                    throw;
                }
                db.Close();
            }
        }

        /// <summary>
        ///     依据主键号删除一条记录
        /// </summary>
        /// <param name="primaryKey">主键号</param>
        public void Delete(int primaryKey)
        {
            using (var db = new SqliteConnection("Filename=" + DbFileName))
            {
                db.Open();
                var deleteCommand = new SqliteCommand
                {
                    Connection = db,
                    CommandText = "DELETE FROM " + TableName + " WHERE " + PrimaryKeyName + "=@" + PrimaryKeyName
                };
                deleteCommand.Parameters.AddWithValue("@" + PrimaryKeyName, primaryKey);
                try
                {
                    deleteCommand.ExecuteReader();
                }
                catch (SqliteException exception)
                {
                    Debug.WriteLine(exception);
                    throw;
                }
                db.Close();
            }
        }

        /// <summary>
        ///     返回最新记录的keyNo
        /// </summary>
        /// <returns>key号</returns>
        public int GetLastKeyNo()
        {
            var keyNo = 0;
            using (var db = new SqliteConnection("Filename=" + DbFileName))
            {
                db.Open();
                var selectCommand = new SqliteCommand
                {
                    Connection = db,
                    CommandText = "SELECT " + PrimaryKeyName + " from " + TableName
                };
                try
                {
                    var query = selectCommand.ExecuteReader();
                    while (query.Read())
                        keyNo = query.GetInt32(0);
                }
                catch (SqliteException exception)
                {
                    Debug.WriteLine(exception);
                    throw;
                }
                db.Close();
            }
            return keyNo;
        }
    }
}