using System.Diagnostics;
using Microsoft.Data.Sqlite;

namespace Touch.Data
{
    /// <summary>
    ///     数据库类的基类
    /// </summary>
    public class DatabaseBase
    {
        protected readonly string DbFileName;

        public DatabaseBase(string dbFileName)
        {
            DbFileName = dbFileName;
        }

        /// <summary>
        ///     创建表
        /// </summary>
        /// <param name="createCommandStr">创建表指令</param>
        protected void Create(string createCommandStr)
        {
            using (var db = new SqliteConnection("Filename=" + DbFileName))
            {
                db.Open();
                var createCommand = new SqliteCommand(createCommandStr, db);
                try
                {
                    createCommand.ExecuteReader();
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
        ///     删除表
        /// </summary>
        /// <param name="dropCommandStr">删除表指令</param>
        protected void Drop(string dropCommandStr)
        {
            using (var db = new SqliteConnection("Filename=" + DbFileName))
            {
                db.Open();
                var dropCommand = new SqliteCommand(dropCommandStr, db);
                try
                {
                    dropCommand.ExecuteReader();
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
        ///     返回记录
        /// </summary>
        /// <returns>SqliteDataReader类型的SQL记录</returns>
        protected SqliteDataReader GetQuery(string selectCommandStr)
        {
            SqliteDataReader query;
            using (var db = new SqliteConnection("Filename=" + DbFileName))
            {
                db.Open();
                var selectCommand = new SqliteCommand(selectCommandStr, db);
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
    }
}