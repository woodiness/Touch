using System.Diagnostics;
using Microsoft.Data.Sqlite;

namespace Touch.Data
{
    /// <summary>
    ///     回忆的所有图片 数据库
    /// </summary>
    public class MemoryImageDatabase : DatabaseBase
    {
        /// <summary>
        ///     表名
        /// </summary>
        private const string TableName = "MemoryImageTable";

        /// <summary>
        ///     主键名
        /// </summary>
        private const string PrimaryKeyName = "Primary_Key";

        /// <summary>
        ///     所属回忆的key号（外键）
        /// </summary>
        private const string MemoryKeyNoName = "Memory_Key_No";

        /// <summary>
        ///     图片的key号（外键）
        /// </summary>
        private const string ImageKeyNoName = "Image_Key_No";

        public MemoryImageDatabase(string dbFileName) : base(dbFileName)
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
                  + MemoryKeyNoName + " INTEGER NOT NULL, "
                  + ImageKeyNoName + " INTEGER NOT NULL, "
                  + "FOREIGN KEY(" + MemoryKeyNoName + ") REFERENCES " + MemoryListDatabase.TableName + "(" +
                  MemoryListDatabase.PrimaryKeyName + ") ON DELETE CASCADE, "
                  + "FOREIGN KEY(" + ImageKeyNoName + ") REFERENCES " + ImageDatabase.TableName + "(" +
                  ImageDatabase.PrimaryKeyName + ") ON DELETE CASCADE)";
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
        ///     返回某个回忆下的所有图片记录
        /// </summary>
        /// <param name="memoryKeyNo">回忆号</param>
        /// <returns>某个回忆下的所有图片记录</returns>
        public SqliteDataReader GetQuery(int memoryKeyNo)
        {
            SqliteDataReader query;
            using (var db = new SqliteConnection("Filename=" + DbFileName))
            {
                db.Open();
                var selectCommand = new SqliteCommand
                {
                    Connection = db,
                    CommandText
                        = "SELECT " + ImageDatabase.TableName + ".* FROM "
                          + TableName + ", " + MemoryListDatabase.TableName + ", " + ImageDatabase.TableName
                          + " WHERE " + TableName + "." + MemoryKeyNoName + " = " + MemoryListDatabase.TableName + "." +
                          MemoryListDatabase.PrimaryKeyName + " and "
                          + TableName + "." + ImageKeyNoName + " = " + ImageDatabase.TableName + "." +
                          ImageDatabase.PrimaryKeyName + " and "
                          + MemoryKeyNoName + "=@" + MemoryKeyNoName
                };
                selectCommand.Parameters.AddWithValue("@" + MemoryKeyNoName, memoryKeyNo);
                try
                {
                    query = selectCommand.ExecuteReader();
                }
                catch (SqliteException exception)
                {
                    Debug.WriteLine(exception);
                    throw;
                }
                db.Close();
            }
            return query;
        }

        /// <summary>
        ///     添加一条记录
        /// </summary>
        /// <param name="memoryKeyNo">回忆号</param>
        /// <param name="imageKeyNo">图片号</param>
        public void Insert(int memoryKeyNo, int imageKeyNo)
        {
            using (var db = new SqliteConnection("Filename=" + DbFileName))
            {
                db.Open();
                var insertCommand = new SqliteCommand
                {
                    Connection = db,
                    CommandText = "INSERT INTO " + TableName + " VALUES (NULL, @" + MemoryKeyNoName + ", @" +
                                  ImageKeyNoName + ")"
                };
                insertCommand.Parameters.AddWithValue("@" + MemoryKeyNoName, memoryKeyNo);
                insertCommand.Parameters.AddWithValue("@" + ImageKeyNoName, imageKeyNo);
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
        ///     依据回忆号删除一系列记录
        /// </summary>
        /// <param name="memoryKeyNo">回忆号</param>
        public void Delete(int memoryKeyNo)
        {
            using (var db = new SqliteConnection("Filename=" + DbFileName))
            {
                db.Open();
                var deleteCommand = new SqliteCommand
                {
                    Connection = db,
                    CommandText = "DELETE FROM " + TableName + " WHERE " + MemoryKeyNoName + "=@" + MemoryKeyNoName
                };
                deleteCommand.Parameters.AddWithValue("@" + MemoryKeyNoName, memoryKeyNo);
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
    }
}