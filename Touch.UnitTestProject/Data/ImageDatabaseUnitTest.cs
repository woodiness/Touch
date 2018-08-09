using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Touch.Data;

namespace Touch.UnitTestProject.Data
{
    /// <summary>
    ///     所有图片 数据库
    /// </summary>
    [TestClass]
    public class ImageDatabaseUnitTest
    {
        private readonly DatabaseHelper _databaseHelper;

        /// <summary>
        ///     所有图片 数据库
        /// </summary>
        public ImageDatabaseUnitTest()
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
            _databaseHelper.ImageDatabase.Drop();
            _databaseHelper.FolderDatabase.Create();
            _databaseHelper.ImageDatabase.Create();
            // 先创建文件夹
            for (var i = 0; i < 3; i++)
                _databaseHelper.FolderDatabase.Insert("folder_path_" + i, "folder_access_token" + i);
            var folderQuery = _databaseHelper.FolderDatabase.GetQuery();
            // 把文件夹号记下
            var folderKeyNos = new List<int>();
            while (folderQuery.Read())
                folderKeyNos.Add(folderQuery.GetInt32(0));
            // 添加图片数据
            foreach (var keyNo in folderKeyNos)
                for (var i = 0; i < 3; i++)
                    _databaseHelper.ImageDatabase.Insert(keyNo, keyNo + "_image_path_" + i,
                        keyNo + "_image_access_token" + i);
            // 读取图片数据
            var imageQuery = _databaseHelper.ImageDatabase.GetQuery();
            var folderCount = 1;
            var imageCount = 0;
            while (imageQuery.Read())
            {
                Assert.AreEqual(folderCount, imageQuery.GetInt32(1));
                Assert.AreEqual(folderCount + "_image_path_" + imageCount, imageQuery.GetString(2));
                Assert.AreEqual(folderCount + "_image_access_token" + imageCount, imageQuery.GetString(3));
                imageCount++;
                if (imageCount < 3)
                    continue;
                folderCount++;
                imageCount = 0;
            }
        }

        /// <summary>
        ///     插入并读出某个文件夹下的数据
        /// </summary>
        [TestMethod]
        public void InsertAndGetFolderTest()
        {
            _databaseHelper.FolderDatabase.Drop();
            _databaseHelper.ImageDatabase.Drop();
            _databaseHelper.FolderDatabase.Create();
            _databaseHelper.ImageDatabase.Create();
            // 先创建文件夹
            for (var i = 0; i < 3; i++)
                _databaseHelper.FolderDatabase.Insert("folder_path_" + i, "folder_access_token" + i);
            var folderQuery = _databaseHelper.FolderDatabase.GetQuery();
            // 把文件夹号记下
            var folderKeyNos = new List<int>();
            while (folderQuery.Read())
                folderKeyNos.Add(folderQuery.GetInt32(0));
            // 添加图片数据
            foreach (var keyNo in folderKeyNos)
                for (var i = 0; i < 3; i++)
                    _databaseHelper.ImageDatabase.Insert(keyNo, "image_path_" + i, "image_access_token" + i);
            // 读取图片数据
            foreach (var keyNo in folderKeyNos)
            {
                var imageQuery = _databaseHelper.ImageDatabase.GetQuery(keyNo);
                var imageCount = 0;
                while (imageQuery.Read())
                {
                    Assert.AreEqual(keyNo, imageQuery.GetInt32(1));
                    Assert.AreEqual("image_path_" + imageCount, imageQuery.GetString(2));
                    Assert.AreEqual("image_access_token" + imageCount, imageQuery.GetString(3));
                    imageCount++;
                    if (imageCount < 3)
                        continue;
                    imageCount = 0;
                }
            }
        }

        ///// <summary>
        /////     插入外键不存在的数据并读出数据
        ///// </summary>
        //[TestMethod]
        //public void InsertWrongAndGetTest()
        //{
        //    _databaseHelper.FolderDatabase.Drop();
        //    _databaseHelper.ImageDatabase.Drop();
        //    _databaseHelper.FolderDatabase.Create();
        //    _databaseHelper.ImageDatabase.Create();
        //    // 先创建文件夹
        //    for (var i = 0; i < 3; i++)
        //        _databaseHelper.FolderDatabase.Insert("folder_path_" + i, "folder_access_token" + i);
        //    // 添加图片数据
        //    for (var j = 1; j < 4; j += 2)
        //    for (var i = 0; i < 3; i++)
        //        _databaseHelper.ImageDatabase.Insert(j, "image_path_" + i, "image_access_token" + i);
        //    // 读取图片数据
        //    var imageQuery = _databaseHelper.ImageDatabase.GetQuery();
        //    var imageCount = 0;
        //    while (imageQuery.Read())
        //    {
        //        // 应该只有文件夹号为1的插进去了
        //        Assert.AreEqual(1, imageQuery.GetInt32(1));
        //        Assert.AreEqual("image_path_" + imageCount, imageQuery.GetString(2));
        //        Assert.AreEqual("image_access_token" + imageCount, imageQuery.GetString(3));
        //        imageCount++;
        //        if (imageCount < 3)
        //            continue;
        //        imageCount = 0;
        //    }
        //}

        /// <summary>
        ///     按图片路径删除并读出数据
        /// </summary>
        [TestMethod]
        public void DeletePathAndGetTest()
        {
            _databaseHelper.FolderDatabase.Drop();
            _databaseHelper.ImageDatabase.Drop();
            _databaseHelper.FolderDatabase.Create();
            _databaseHelper.ImageDatabase.Create();
            // 先创建文件夹
            for (var i = 0; i < 3; i++)
                _databaseHelper.FolderDatabase.Insert("folder_path_" + i, "folder_access_token" + i);
            var folderQuery = _databaseHelper.FolderDatabase.GetQuery();
            // 把文件夹号记下
            var folderKeyNos = new List<int>();
            while (folderQuery.Read())
                folderKeyNos.Add(folderQuery.GetInt32(0));
            // 添加图片数据
            foreach (var keyNo in folderKeyNos)
                for (var i = 0; i < 5; i++)
                    _databaseHelper.ImageDatabase.Insert(keyNo, keyNo + "_image_path_" + i,
                        keyNo + "_image_access_token" + i);
            // 删除
            foreach (var keyNo in folderKeyNos)
                for (var i = 0; i < 5; i += 2)
                    _databaseHelper.ImageDatabase.Delete(keyNo + "_image_path_" + i);
            // 读取图片数据
            var imageQuery = _databaseHelper.ImageDatabase.GetQuery();
            var folderCount = 1;
            var imageCount = 1;
            while (imageQuery.Read())
            {
                Assert.AreEqual(folderCount, imageQuery.GetInt32(1));
                Assert.AreEqual(folderCount + "_image_path_" + imageCount, imageQuery.GetString(2));
                Assert.AreEqual(folderCount + "_image_access_token" + imageCount, imageQuery.GetString(3));
                imageCount += 2;
                if (imageCount < 5)
                    continue;
                folderCount++;
                imageCount = 1;
            }
        }

        /// <summary>
        ///     按文件夹号删除并读出数据
        /// </summary>
        [TestMethod]
        public void DeleteFolderAndGetTest()
        {
            _databaseHelper.FolderDatabase.Drop();
            _databaseHelper.ImageDatabase.Drop();
            _databaseHelper.FolderDatabase.Create();
            _databaseHelper.ImageDatabase.Create();
            // 先创建文件夹
            for (var i = 0; i < 5; i++)
                _databaseHelper.FolderDatabase.Insert("folder_path_" + i, "folder_access_token" + i);
            var folderQuery = _databaseHelper.FolderDatabase.GetQuery();
            // 把文件夹号记下
            var folderKeyNos = new List<int>();
            while (folderQuery.Read())
                folderKeyNos.Add(folderQuery.GetInt32(0));
            // 添加图片数据
            foreach (var keyNo in folderKeyNos)
                for (var i = 0; i < 5; i++)
                    _databaseHelper.ImageDatabase.Insert(keyNo, keyNo + "_image_path_" + i,
                        keyNo + "_image_access_token" + i);
            // 删除
            for (var i = 1; i <= 5; i += 2)
                _databaseHelper.ImageDatabase.Delete(i);
            // 读取图片数据
            var imageQuery = _databaseHelper.ImageDatabase.GetQuery();
            var folderCount = 2;
            var imageCount = 0;
            while (imageQuery.Read())
            {
                Assert.AreEqual(folderCount, imageQuery.GetInt32(1));
                Assert.AreEqual(folderCount + "_image_path_" + imageCount, imageQuery.GetString(2));
                Assert.AreEqual(folderCount + "_image_access_token" + imageCount, imageQuery.GetString(3));
                imageCount++;
                if (imageCount < 5)
                    continue;
                folderCount += 2;
                imageCount = 0;
            }
        }
    }
}