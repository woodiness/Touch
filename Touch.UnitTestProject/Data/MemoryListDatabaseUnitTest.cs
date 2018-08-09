using Microsoft.VisualStudio.TestTools.UnitTesting;
using Touch.Data;

namespace Touch.UnitTestProject.Data
{
    /// <summary>
    ///     回忆列表 数据库
    /// </summary>
    [TestClass]
    public class MemoryListDatabaseUnitTest
    {
        private readonly DatabaseHelper _databaseHelper;

        /// <summary>
        ///     文件夹 数据库
        /// </summary>
        public MemoryListDatabaseUnitTest()
        {
            _databaseHelper = DatabaseHelper.GetInstance();
        }

        /// <summary>
        ///     插入并读出数据
        /// </summary>
        [TestMethod]
        public void InsertAndGetTest()
        {
            _databaseHelper.MemoryListDatabase.Drop();
            _databaseHelper.MemoryListDatabase.Create();
            for (var i = 0; i < 3; i++)
                _databaseHelper.MemoryListDatabase.Insert("test_data_" + i);
            var query = _databaseHelper.MemoryListDatabase.GetQuery();
            var count = 0;
            while (query.Read())
            {
                Assert.AreEqual("test_data_" + count, query.GetString(1));
                count++;
            }
        }

        /// <summary>
        ///     删除并读出数据
        /// </summary>
        [TestMethod]
        public void DeleteAndGetTest()
        {
            _databaseHelper.MemoryListDatabase.Drop();
            _databaseHelper.MemoryListDatabase.Create();
            for (var i = 0; i < 5; i++)
                _databaseHelper.MemoryListDatabase.Insert("test_data_" + i);
            for (var i = 1; i <= 5; i += 2)
                _databaseHelper.MemoryListDatabase.Delete(i);
            var query = _databaseHelper.MemoryListDatabase.GetQuery();
            var count = 1;
            while (query.Read())
            {
                Assert.AreEqual("test_data_" + count, query.GetString(1));
                count += 2;
            }
        }
    }
}