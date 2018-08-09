using Microsoft.VisualStudio.TestTools.UnitTesting;
using Touch.Data;

namespace Touch.UnitTestProject.Data
{
    /// <summary>
    ///     回忆的所有图片 数据库
    /// </summary>
    [TestClass]
    public class MemoryImageDatabaseUnitTest
    {
        private readonly DatabaseHelper _databaseHelper;

        /// <summary>
        ///     回忆的所有图片 数据库
        /// </summary>
        public MemoryImageDatabaseUnitTest()
        {
            _databaseHelper = DatabaseHelper.GetInstance();
        }

        /// <summary>
        ///     插入并读出数据
        /// </summary>
        [TestMethod]
        public void InsertAndGetTest()
        {
            // 初始化
            _databaseHelper.ImageDatabase.Drop();
            _databaseHelper.ImageDatabase.Create();
            _databaseHelper.MemoryListDatabase.Drop();
            _databaseHelper.MemoryListDatabase.Create();
            _databaseHelper.MemoryImageDatabase.Drop();
            _databaseHelper.MemoryImageDatabase.Create();
            // 创建图片数据
            for (var i = 1; i <= 6; i++)
                _databaseHelper.ImageDatabase.Insert(i, "image_path_" + i, "access_token_" + i);
            // 创建回忆列表
            for (var i = 1; i <= 6; i++)
                _databaseHelper.MemoryListDatabase.Insert("memory_name_" + i);
            // 创建回忆图片
            for (var i = 1; i <= 3; i++)
            {
                _databaseHelper.MemoryImageDatabase.Insert(i, i);
                _databaseHelper.MemoryImageDatabase.Insert(i, i + 1);
            }
            // 读取回忆图片
            for (var i = 1; i <= 3; i++)
            {
                var query = _databaseHelper.MemoryImageDatabase.GetQuery(i);
                while (query.Read())
                {
                    Assert.AreEqual(i, query.GetInt32(1));
                    Assert.AreEqual("image_path_" + i, query.GetString(2));
                    Assert.AreEqual("access_token_" + i, query.GetString(3));
                    query.Read();
                    Assert.AreEqual(i + 1, query.GetInt32(1));
                    Assert.AreEqual("image_path_" + (i + 1), query.GetString(2));
                    Assert.AreEqual("access_token_" + (i + 1), query.GetString(3));
                }
            }
        }

        /// <summary>
        ///     删除并读出数据
        /// </summary>
        [TestMethod]
        public void DeleteAndGetTest()
        {
            // 初始化
            _databaseHelper.ImageDatabase.Drop();
            _databaseHelper.ImageDatabase.Create();
            _databaseHelper.MemoryListDatabase.Drop();
            _databaseHelper.MemoryListDatabase.Create();
            _databaseHelper.MemoryImageDatabase.Drop();
            _databaseHelper.MemoryImageDatabase.Create();
            // 创建图片数据
            for (var i = 1; i <= 6; i++)
                _databaseHelper.ImageDatabase.Insert(i, "image_path_" + i, "access_token_" + i);
            // 创建回忆列表
            for (var i = 1; i <= 6; i++)
                _databaseHelper.MemoryListDatabase.Insert("memory_name_" + i);
            // 创建回忆图片
            for (var i = 1; i <= 5; i++)
                _databaseHelper.MemoryImageDatabase.Insert(i, i + 1);
            // 删除回忆
            for (var i = 1; i <= 3; i++)
                _databaseHelper.MemoryImageDatabase.Delete(i);
            // 读取回忆图片
            for (var i = 1; i <= 3; i++)
            {
                var query = _databaseHelper.MemoryImageDatabase.GetQuery(i);
                while (query.Read())
                    // 应该不存在
                    Assert.AreEqual(1, 2);
            }
            for (var i = 4; i <= 5; i++)
            {
                var query = _databaseHelper.MemoryImageDatabase.GetQuery(i);
                while (query.Read())
                {
                    Assert.AreEqual(i + 1, query.GetInt32(1));
                    Assert.AreEqual("image_path_" + (i + 1), query.GetString(2));
                    Assert.AreEqual("access_token_" + (i + 1), query.GetString(3));
                }
            }
        }
    }
}