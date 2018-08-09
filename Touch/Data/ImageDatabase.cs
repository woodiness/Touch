using System.Diagnostics;
using Microsoft.Data.Sqlite;

namespace Touch.Data
{
    /// <summary>
    ///     所有图片 数据库
    /// </summary>
    public class ImageDatabase : DatabaseBase
    {
        /// <summary>
        ///     表名
        /// </summary>
        public const string TableName = "ImageTable";

        /// <summary>
        ///     主键名
        /// </summary>
        public const string PrimaryKeyName = "Primary_Key";

        /// <summary>
        ///     所属文件夹的key号（外键）
        /// </summary>
        private const string FolderKeyNoName = "Folder_Key_No";

        /// <summary>
        ///     图片路径
        /// </summary>
        private const string ImagePathName = "Image_Path";

        /// <summary>
        ///     访问权限
        /// </summary>
        private const string AccessTokenName = "Access_Token";

        ///// <summary>
        /////     图片宽度
        ///// </summary>
        //private const string WidthName = "Width";

        ///// <summary>
        /////     图片高度
        ///// </summary>
        //private const string HeightName = "Height";

        ///// <summary>
        /////     图片纬度（可为空）
        ///// </summary>
        //private const string LatitudeName = "Latitude";

        ///// <summary>
        /////     图片高度（可为空）
        ///// </summary>
        //private const string LongitudeName = "Longitude";

        ///// <summary>
        /////     图片拍摄日期（offset）
        ///// </summary>
        //private const string DateTakenName = "Date_Taken";

        public ImageDatabase(string dbFileName) : base(dbFileName)
        {
        }

        /// <summary>
        ///     创建表
        /// </summary>
        public void Create()
        {
            //const string createCommandStr
            //    = "CREATE TABLE IF NOT EXISTS " + TableName + " ("
            //      + PrimaryKeyName + " INTEGER PRIMARY KEY AUTOINCREMENT, "
            //      + FolderKeyNoName + " INTEGER NOT NULL, "
            //      + ImagePathName + " NVARCHAR(2048) NOT NULL, "
            //      + AccessTokenName + " NVARCHAR(2048) NOT NULL, "
            //      + WidthName + " UINT32 NOT NULL, "
            //      + HeightName + " UINT32 NOT NULL, "
            //      + LatitudeName + " DOUBLE NULL, "
            //      + LongitudeName + " DOUBLE NULL, "
            //      + DateTakenName + " UINT64 NOT NULL)";
            const string createCommandStr
                = "CREATE TABLE IF NOT EXISTS " + TableName + " ("
                  + PrimaryKeyName + " INTEGER PRIMARY KEY AUTOINCREMENT, "
                  + FolderKeyNoName + " INTEGER NOT NULL, "
                  + ImagePathName + " NVARCHAR(2048) NOT NULL, "
                  + AccessTokenName + " NVARCHAR(2048) NOT NULL, "
                  + "FOREIGN KEY(" + FolderKeyNoName + ") REFERENCES " + FolderDatabase.TableName + "(" +
                  FolderDatabase.PrimaryKeyName + ")on delete cascade)";
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
        ///     返回某个文件夹下的所有记录
        /// </summary>
        /// <param name="folderKeyNo">文件夹号</param>
        /// <returns>某个文件夹的所有记录</returns>
        public SqliteDataReader GetQuery(int folderKeyNo)
        {
            SqliteDataReader query;
            using (var db = new SqliteConnection("Filename=" + DbFileName))
            {
                db.Open();
                var selectCommand = new SqliteCommand
                {
                    Connection = db,
                    CommandText = "SELECT * FROM " + TableName + " WHERE " + FolderKeyNoName + "=@" + FolderKeyNoName
                };
                selectCommand.Parameters.AddWithValue("@" + FolderKeyNoName, folderKeyNo);
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

        ///// <summary>
        ///// 添加一条记录
        ///// </summary>
        ///// <param name="folderKeyNo">文件夹号</param>
        ///// <param name="imagePath">图片路径</param>
        ///// <param name="accessToken">访问权限</param>
        ///// <param name="width">宽度</param>
        ///// <param name="height">高度</param>
        ///// <param name="latitude">纬度</param>
        ///// <param name="longitude">经度</param>
        ///// <param name="dateTaken">拍摄日期</param>
        //public void Insert(int folderKeyNo, string imagePath, string accessToken, uint width, uint height,
        //    double? latitude, double? longitude, ulong dateTaken)
        //{
        //    using (var db = new SqliteConnection("Filename=" + DbFileName))
        //    {
        //        db.Open();
        //        var insertCommand = new SqliteCommand
        //        {
        //            Connection = db,
        //            CommandText = "INSERT INTO " + TableName + " VALUES (NULL, @" + FolderKeyNoName + ", @" +
        //                          ImagePathName + ", @" + AccessTokenName + ", @" + WidthName + ", @" + HeightName +
        //                          ", @" + LatitudeName + ", @" + LongitudeName + ", @" + DateTakenName + ");"
        //        };
        //        insertCommand.Parameters.AddWithValue("@" + FolderKeyNoName, folderKeyNo);
        //        insertCommand.Parameters.AddWithValue("@" + ImagePathName, imagePath);
        //        insertCommand.Parameters.AddWithValue("@" + AccessTokenName, accessToken);
        //        insertCommand.Parameters.AddWithValue("@" + WidthName, width);
        //        insertCommand.Parameters.AddWithValue("@" + HeightName, height);
        //        insertCommand.Parameters.AddWithValue("@" + LatitudeName, latitude);
        //        insertCommand.Parameters.AddWithValue("@" + LongitudeName, longitude);
        //        insertCommand.Parameters.AddWithValue("@" + DateTakenName, dateTaken);
        //        try
        //        {
        //            insertCommand.ExecuteReader();
        //        }
        //        catch (SqliteException exception)
        //        {
        //            Debug.WriteLine(exception);
        //            throw;
        //        }
        //        db.Close();
        //    }
        //}

        /// <summary>
        ///     添加一条记录
        /// </summary>
        /// <param name="folderKeyNo">文件夹号</param>
        /// <param name="imagePath">图片路径</param>
        /// <param name="accessToken">访问权限</param>
        public void Insert(int folderKeyNo, string imagePath, string accessToken)
        {
            using (var db = new SqliteConnection("Filename=" + DbFileName))
            {
                db.Open();
                var insertCommand = new SqliteCommand
                {
                    Connection = db,
                    CommandText = "INSERT INTO " + TableName + " VALUES (NULL, @" + FolderKeyNoName + ", @" +
                                  ImagePathName + ", @" + AccessTokenName + ");"
                };
                insertCommand.Parameters.AddWithValue("@" + FolderKeyNoName, folderKeyNo);
                insertCommand.Parameters.AddWithValue("@" + ImagePathName, imagePath);
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
        ///     依据图片路径删除一条记录
        /// </summary>
        /// <param name="imagePath">图片路径</param>
        public void Delete(string imagePath)
        {
            using (var db = new SqliteConnection("Filename=" + DbFileName))
            {
                db.Open();
                var deleteCommand = new SqliteCommand
                {
                    Connection = db,
                    CommandText = "DELETE FROM " + TableName + " WHERE " + ImagePathName + "=@" + ImagePathName
                };
                deleteCommand.Parameters.AddWithValue("@" + ImagePathName, imagePath);
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
        ///     依据文件夹号删除一系列记录
        /// </summary>
        /// <param name="folderKeyNo">文件夹号</param>
        public void Delete(int folderKeyNo)
        {
            using (var db = new SqliteConnection("Filename=" + DbFileName))
            {
                db.Open();
                var deleteCommand = new SqliteCommand
                {
                    Connection = db,
                    CommandText = "DELETE FROM " + TableName + " WHERE " + FolderKeyNoName + "=@" + FolderKeyNoName
                };
                deleteCommand.Parameters.AddWithValue("@" + FolderKeyNoName, folderKeyNo);
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