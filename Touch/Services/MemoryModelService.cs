using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using Touch.Models;
using Touch.ViewModels;

namespace Touch.Services
{
    public class MemoryModelService : IMemoryModelService
    {
        private readonly IImageModelService _imageModelService;

        public MemoryModelService(IImageModelService imageModelService)
        {
            _imageModelService = imageModelService;
        }
        /// <summary>
        /// 从数据库中获取回忆实例
        /// </summary>
        /// <param name="memoryModel"></param>
        /// <returns></returns>
        public async Task<MemoryModel> GetInstanceAsync(MemoryModel memoryModel)
        {
            memoryModel.ImageModels = new List<ImageModel>();
            if (memoryModel?.ImageModels == null)
                return memoryModel;
            foreach (var imageModel in memoryModel.ImageModels)
            {
                var imageViewModel = await _imageModelService.GetThumbnailImageAsync(400, imageModel);
                memoryModel.ImageModels.Add(imageViewModel);
            }
            if (memoryModel.ImageModels != null && memoryModel.ImageModels.Count > 0)
                memoryModel.CoverImage = memoryModel.ImageModels[0].ThumbnailImage;
            else
                memoryModel.CoverImage = new BitmapImage(new Uri("ms-appx:///Assets/Gray.png"));
            return memoryModel;
        }
        /// <summary>
        /// 新建回忆
        /// </summary>
        /// <param name="memoryModel"></param>
        /// <param name="lastKeyNo"></param>
        /// <param name="memoryName"></param>
        /// <param name="imageModels"></param>
        /// <returns></returns>
        public MemoryModel GetNewMemoryModel(MemoryModel memoryModel, int lastKeyNo, string memoryName, List<ImageModel> imageModels)
        {
            memoryModel.KeyNo = lastKeyNo + 1;
            memoryModel.MemoryName = memoryName;
            memoryModel.ImageModels = imageModels;
            memoryModel.CoverImage = memoryModel.ImageModels[0].ThumbnailImage;
            return memoryModel;
        }
    }
}
