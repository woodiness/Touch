using System.Diagnostics;
using Microsoft.Data.Sqlite;

namespace Touch.Data
{
    /// <summary>
    ///     文件夹 数据库
    /// </summary>
    public class FolderDatabase : DatabaseBase
    {
        /// <summary>
        ///     表名
        /// </summary>
        public const string TableName = "FolderTable";

        /// <summary>
        ///     主键名
        /// </summary>
        public const string PrimaryKeyName = "Primary_Key";

        /// <summary>
        ///     文件夹路径
        /// </summary>
        private const string FolderPathName = "Folder_Path";

        /// <summary>
        ///     访问权限
        /// </summary>
        private const string AccessTokenName = "Access_Token";

        public FolderDatabase(string dbFileName) : base(dbFileName)
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
                  + FolderPathName + " NVARCHAR(2048) NOT NULL, "
                  + AccessTokenName + " NVARCHAR(2048) NOT NULL)";
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
        /// <param name="folderPath">文件夹路径</param>
        /// <param name="accessToken">访问token</param>
        public void Insert(string folderPath, string accessToken)
        {
            using (var db = new SqliteConnection("Filename=" + DbFileName))
            {
                db.Open();
                var insertCommand = new SqliteCommand
                {
                    Connection = db,
                    CommandText = "INSERT INTO " + TableName + " VALUES (NULL, @" + FolderPathName + ", @" +
                                  AccessTokenName + ")"
                };
                // Use parameterized query to prevent SQL injection attacks
                insertCommand.Parameters.AddWithValue("@" + FolderPathName, folderPath);
                insertCommand.Parameters.AddWithValue("@" + AccessTokenName, accessToken);
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
        ///     依据文件夹路径删除一条记录
        /// </summary>
        /// <param name="folderPath">文件夹路径</param>
        public void Delete(string folderPath)
        {
            using (var db = new SqliteConnection("Filename=" + DbFileName))
            {
                db.Open();
                var deleteCommand = new SqliteCommand
                {
                    Connection = db,
                    CommandText
                        = "PRAGMA FOREIGN_KEYS = ON; "
                          + "DELETE FROM " + TableName + " WHERE " + FolderPathName + "=@" + FolderPathName + ";"
                };
                deleteCommand.Parameters.AddWithValue("@" + FolderPathName, folderPath);
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