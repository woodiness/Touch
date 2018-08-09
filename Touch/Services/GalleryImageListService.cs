using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Touch.Models;
using Touch.ViewModels;

namespace Touch.Services
{
    public class GalleryImageListService : IGalleryImageListService
    {
        private readonly IImageFolderListService _imageFolderListService;
        private readonly IFolderListService _folderListService;
        private readonly IImageModelService _imageModelService;
        public GalleryImageListService(IImageFolderListService imageFolderListService,
            IFolderListService folderListService,
            IImageModelService imageModelService)
        {
            _imageModelService = imageModelService;
            _imageFolderListService = imageFolderListService;
            _folderListService = folderListService;
        }
        /// <summary>
        /// 预计实现在图库页面删除图片操作，传进来的参数正确，只需要正确的删除数据库中的ImageModel即可
        /// 下面的函数实现略有问题
        /// </summary>
        /// <param name="imageModel"></param>
        /// <param name="imageFolderLists"></param>
        /// <returns></returns>
        public async Task DeleteAsync(ImageModel imageModel,  List<ImageFolderList> imageFolderLists, IOrderedEnumerable<ImageMonthGroup> imageMonthGroups)
        {

//            var id = 0;
//            for (id = 0; id < imageFolderLists.Count; id++)
//            {
//                if(imageFolderLists[id].ImageModels.Contains(imageModel))
//                    break;
//            }
//
//            if (id < imageFolderLists.Count)
//            {
//                imageFolderLists[id].ImageModels.Remove(imageModel);
//                imageFolderLists[id].DatabaseHelper.ImageDatabase.Delete(imageModel.ImagePath);
//            }
//            imageMonthGroups = await GroupImageAsync(imageFolderLists);
//            return imageMonthGroups;
        }

        /// <summary>
        /// 从图片库中筛选出既有GPS又有Google街景的图片集，用作生成回忆的备选图片集
        /// </summary>
        /// <param name="imageMonthGroups"></param>
        /// <returns></returns>
        public async Task<IOrderedEnumerable<ImageMonthGroup>> FindImagesAsync(IOrderedEnumerable<ImageMonthGroup> imageMonthGroups)
        {
            var imageModels = new List<ImageModel>();
            foreach (ImageMonthGroup imageMonthGroup in imageMonthGroups)
            {
                foreach (ImageModel imageModel in imageMonthGroup)
                {
                    if (imageModel.Longitude.HasValue && imageModel.Latitude.HasValue)
                    {

                        var x = imageModel.Latitude.Value
                            .ToString(CultureInfo.CurrentCulture);
                        var y = imageModel.Longitude.Value
                            .ToString(CultureInfo.CurrentCulture);
                        var status = await StreetViewMetadata.GetStreetViewStutas(x, y);
                        Debug.WriteLine(status);
                        if (status == "OK")
                        {
                            imageModels.Add(imageModel);
                        }
                    }
                }
            }

            IOrderedEnumerable<ImageMonthGroup> MemoryImageMonthGroups;
            MemoryImageMonthGroups = imageModels
                .GroupBy(m => m.MonthYearDate, (key, list) => new ImageMonthGroup(key, list))
                .OrderByDescending(m => m.Key.WholeDateTime.Year).ThenByDescending(m => m.Key.WholeDateTime.Month);
            return MemoryImageMonthGroups;
        }



        /// <summary>
        /// 更新图片库
        /// </summary>
        /// <param name="imageFolderLists"></param>
        /// <returns></returns>
        public async Task<IOrderedEnumerable<ImageMonthGroup>> GroupImageAsync(List<ImageFolderList> imageFolderLists)
        {
            // 先初始化所有图片成一个list
            
            IOrderedEnumerable<ImageMonthGroup> ImageMonthGroups;
            // 先初始化所有图片成一个list
            var imageModels = new List<ImageModel>();
            foreach (var imageFolderList in imageFolderLists)
            foreach (var imageModel in imageFolderList.ImageModels)
            {
                var iimageModel = await _imageModelService.GetThumbnailImageAsync(400, imageModel);
                imageModels.Add(iimageModel);
            }
            ImageMonthGroups = imageModels
                .GroupBy(m => m.MonthYearDate, (key, list) => new ImageMonthGroup(key, list))
                .OrderByDescending(m => m.Key.WholeDateTime.Year).ThenByDescending(m => m.Key.WholeDateTime.Month);
            return ImageMonthGroups;
        }
        /// <summary>
        /// 更新文件夹列表
        /// </summary>
        /// <param name="folderList"></param>
        /// <returns></returns>
        public async Task<List<ImageFolderList>> RefreshFolderListAsync(FolderList folderList)
        {
            List<ImageFolderList> imageFolderLists = new List<ImageFolderList>();
            imageFolderLists.Clear();
            //folderList =  _folderListService.GetInstanceAsync();

            folderList = _folderListService.RemoveAdd(folderList);
            foreach (var folderModel in folderList.FolderModels)
            {
                var imageFolderList = await _imageFolderListService.GetInstanceAsync(folderModel);
                imageFolderLists.Add(imageFolderList);
            }

            folderList = _folderListService.BackAdd(folderList);
            return imageFolderLists;
        }

        /// <summary>
        /// 更新照片库
        /// </summary>
        /// <param name="imageFolderLists"></param>
        /// <returns></returns>
        public async Task<IOrderedEnumerable<ImageMonthGroup>> RefreshImageListAsync(List<ImageFolderList> imageFolderLists)
        {
            foreach (var imageFolderList in imageFolderLists)
                //await imageFolderList.RefreshAsync();
                await _imageFolderListService.RefreshAsync(imageFolderList);
            IOrderedEnumerable<ImageMonthGroup> imageMonthGroups =await GroupImageAsync(imageFolderLists);
            return imageMonthGroups;
        }
    }
}
