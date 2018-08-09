using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.AccessCache;
using Touch.Models;

namespace Touch.Services
{
    public class MemoryListService : IMemoryListService
    {
        private readonly IImageModelService _imageModelService;
        /// <summary>
        ///     构造函数。
        /// </summary>
        /// <param name="imageModelService">身份服务。</param>
        public MemoryListService(IImageModelService imageModelService)
        {
            _imageModelService = imageModelService;
        }
        /// <summary>
        ///     异步获取实例
        /// </summary>
        /// <returns>回忆列表</returns>
        public  async Task<MemoryList> GetInstanceAsync()
        {
            var memoryList = new MemoryList();
            // 初始化list
            var query = memoryList.DatabaseHelper.MemoryListDatabase.GetQuery();
            while (query.Read())
            {
                var memoryKeyNo = query.GetInt32(0);
                // 初始化图片list
                var imageModels = new List<ImageModel>();
                var imageQuery = memoryList.DatabaseHelper.MemoryImageDatabase.GetQuery(memoryKeyNo);
                while (imageQuery.Read())
                {
                    var imagePath = imageQuery.GetString(2);
                    var accessToken = imageQuery.GetString(3);
                    var imageModel = await _imageModelService.GetInstanceAsync(imageQuery.GetInt32(1), imagePath, accessToken);
                    // 查下这个图片还在不在
                    if (imageModel != null)
                    {
                        imageModel.KeyNo = imageQuery.GetInt32(0);
                        imageModel = await _imageModelService.GetThumbnailImageAsync(400, imageModel);
                        imageModels.Add(imageModel);
                    }
                    else
                    {
                        // 从数据库里删掉这个图片
                        memoryList.DatabaseHelper.ImageDatabase.Delete(imagePath);
                        // 从使用list里删掉这个文件夹
                        StorageApplicationPermissions.FutureAccessList.Remove(accessToken);
                    }
                }
                // 初始化回忆model
                var memoryModel = new MemoryModel
                {
                    KeyNo = memoryKeyNo,
                    MemoryName = query.GetString(1),
                    ImageModels = imageModels
                };
                memoryModel.CoverImage = memoryModel.ImageModels[0].ThumbnailImage;
                memoryList.MemoryModels.Add(memoryModel);
            }
            return memoryList;
        }
        /// <summary>
        /// 添加回忆
        /// </summary>
        /// <param name="memoryModel"></param>
        /// <param name="memoryList"></param>
        /// <returns></returns>
        public MemoryList Add(MemoryModel memoryModel, MemoryList memoryList)
        {
            if (memoryList.MemoryModels.Contains(memoryModel))
                return memoryList;
            memoryList.MemoryModels.Add(memoryModel);
            memoryList.DatabaseHelper.MemoryListDatabase.Insert(memoryModel.MemoryName);
            var lastKeyNo = memoryList.DatabaseHelper.MemoryListDatabase.GetLastKeyNo();
            // 在回忆图片数据库里加图片
            foreach (var imageModel in memoryModel.ImageModels)
                memoryList.DatabaseHelper.MemoryImageDatabase.Insert(lastKeyNo, imageModel.KeyNo);
            return memoryList;
        }
        /// <summary>
        /// 删除回忆
        /// </summary>
        /// <param name="memoryModel"></param>
        /// <param name="memoryList"></param>
        /// <returns></returns>
        public MemoryList Delete(MemoryModel memoryModel, MemoryList memoryList)
        {
            if (!memoryList.MemoryModels.Contains(memoryModel))
                return memoryList;
            memoryList.MemoryModels.Remove(memoryModel);
            memoryList.DatabaseHelper.MemoryListDatabase.Delete(memoryModel.KeyNo);
            // 在回忆图片数据库里删图片
            memoryList.DatabaseHelper.MemoryImageDatabase.Delete(memoryModel.KeyNo);
            return memoryList;
        }
    }
}
