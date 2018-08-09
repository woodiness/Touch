using Microsoft.VisualStudio.TestTools.UnitTesting;
using Touch.Data;

namespace Touch.UnitTestProject.Data
{
    /// <summary>
    ///     文件夹 数据库
    /// </summary>
    [TestClass]
    public class FolderDatabaseUnitTest
    {
        private readonly DatabaseHelper _databaseHelper;

        /// <summary>
        ///     文件夹 数据库
        /// </summary>
        public FolderDatabaseUnitTest()
        {
            _databaseHelper = DatabaseHelper.GetInstance();
        }

        /// <summary>
        ///     插入并读出数据
        /// </summary>
        [TestMethod]
        public void InsertAndGetTest()
        {
            _databaseHelper.FolderDatabase.Drop();
            _databaseHelper.FolderDatabase.Create();
            for (var i = 0; i < 3; i++)
                _databaseHelper.FolderDatabase.Insert("test_data_" + i, i.ToString());
            var query = _databaseHelper.FolderDatabase.GetQuery();
            var count = 0;
            while (query.Read())
            {
                Assert.AreEqual("test_data_" + count, query.GetString(1));
                Assert.AreEqual(count + "", query.GetString(2));
                count++;
            }
        }

        /// <summary>
        ///     删除并读出数据
        /// </summary>
        [TestMethod]
        public void DeleteAndGetTest()
        {
            _databaseHelper.FolderDatabase.Drop();
            _databaseHelper.FolderDatabase.Create();
            for (var i = 0; i < 5; i++)
                _databaseHelper.FolderDatabase.Insert("test_data_" + i, i.ToString());
            for (var i = 0; i < 5; i += 2)
                _databaseHelper.FolderDatabase.Delete("test_data_" + i);
            var query = _databaseHelper.FolderDatabase.GetQuery();
            var count = 1;
            while (query.Read())
            {
                Assert.AreEqual("test_data_" + count, query.GetString(1));
                Assert.AreEqual(count + "", query.GetString(2));
                count += 2;
            }
        }
    }
}