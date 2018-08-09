using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.AccessCache;
using Touch.Models;

namespace Touch.Services
{
    public class ImageFolderListService : IImageFolderListService
    {
        private readonly IFolderModelService _folderModelService;
        private readonly IImageModelService _imageModelService;
        public ImageFolderListService(IFolderModelService folderModelService,IImageModelService imageModelService)
        {
            _folderModelService = folderModelService;
            _imageModelService = imageModelService;
        }
        /// <summary>
        ///     异步获得实例
        /// </summary>
        /// <returns></returns>
        public  async Task<ImageFolderList> GetInstanceAsync(FolderModel folderModel)
        {
            var imageFolderList = new ImageFolderList(folderModel);
            // 初始化list
            var query = imageFolderList.DatabaseHelper.ImageDatabase.GetQuery(folderModel.KeyNo);
            while (query.Read())
            {
                var imagePath = query.GetString(2);
                var accessToken = query.GetString(3);
                var imageModel =
                    await _imageModelService.GetInstanceAsync(query.GetInt32(1), imagePath, accessToken);
                // 查下这个图片还在不在
                if (imageModel != null)
                {
                    imageModel.KeyNo = query.GetInt32(0);
                    imageFolderList.ImageModels.Add(imageModel);
                }
                else
                {
                    // 从数据库里删掉这个图片
                    imageFolderList.DatabaseHelper.ImageDatabase.Delete(imagePath);
                    // 从使用list里删掉这个文件夹
                    StorageApplicationPermissions.FutureAccessList.Remove(accessToken);
                }
            }
            return imageFolderList;
        }
        /// <summary>
        /// 刷新照片列表
        /// </summary>
        /// <param name="imageFolderList"></param>
        /// <returns></returns>
        public async Task<ImageFolderList> RefreshAsync(ImageFolderList imageFolderList)
        {
            var folder = await _folderModelService.GetFolder(imageFolderList.FolderModel.AccessToken);
            // TODO 这里文件夹可能为空
            // 找到全部文件
            var files = await folder.GetFilesAsync();
            foreach (var file in files)
            {
                // 确认后缀名必须是图片
                if (file.FileType != ".jpg" && file.FileType != ".png" && file.FileType != ".jpeg")
                    continue;
                var accessToken = StorageApplicationPermissions.FutureAccessList.Add(file);
                var imageModel = await _imageModelService.GetInstanceAsync(imageFolderList.FolderModel.KeyNo, file.Path, accessToken);
                if (imageFolderList.ImageModels.Contains(imageModel))
                    continue;
                imageFolderList.DatabaseHelper.ImageDatabase.Insert(imageModel.FolderKeyNo, imageModel.ImagePath, imageModel.AccessToken);
                imageFolderList.ImageModels.Add(imageModel);
            }

            var x = imageFolderList.GetHashCode();
            return imageFolderList;
        }
    }
}
