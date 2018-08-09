using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Touch.Models;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Media.Imaging;
using Touch.ViewModels;

namespace Touch.Services
{
    public class ImageModelService : IImageModelService
    {
        public async Task<StorageFile> GetImageFileAsync(string accessToken)
        {
            try
            {
                return await StorageApplicationPermissions.FutureAccessList.GetFileAsync(accessToken);
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }
        /// <summary>
        /// 从数据库中获取实例
        /// </summary>
        /// <param name="folderKeyNo"></param>
        /// <param name="imagePath"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public async Task<ImageModel> GetInstanceAsync(int folderKeyNo, string imagePath, string accessToken)
        {
            ImageModel myImage =new ImageModel();
            myImage.AccessToken = accessToken;
            myImage.FolderKeyNo = folderKeyNo;
            myImage.ImagePath = imagePath;
            myImage.ImageFile = await GetImageFileAsync(accessToken);
//            var myImage = new ImageModel(folderKeyNo, imagePath, accessToken)
//            {
//                
//                ImageFile = await GetImageFileAsync(accessToken)
//            };
            if (myImage.ImageFile == null)
                return null;
            var imageProperties = await myImage.ImageFile.Properties.GetImagePropertiesAsync();
            var basicProperties = await myImage.ImageFile.GetBasicPropertiesAsync();
            myImage.Width = imageProperties.Width;
            myImage.Height = imageProperties.Height;
            myImage.Latitude = imageProperties.Latitude;
            myImage.Longitude = imageProperties.Longitude;
            // 如果图片的拍摄时间为空，返回文件的修改时间
            myImage.DateTaken = imageProperties.DateTaken.Year <= 1601
                ? basicProperties.DateModified.LocalDateTime
                : imageProperties.DateTaken.LocalDateTime;
            return myImage;
        }

        /// <summary>
        /// 获取缩略图
        /// </summary>
        /// <param name="requiredSize"></param>
        /// <param name="imageModel"></param>
        /// <returns></returns>
        public async Task<ImageModel> GetThumbnailImageAsync(uint requiredSize, ImageModel imageModel)
        {
            var fileThumbnail = await imageModel.ImageFile.GetThumbnailAsync(ThumbnailMode.SingleItem, requiredSize);
            var bitmap = new BitmapImage();
            bitmap.SetSource(fileThumbnail);
            imageModel.ThumbnailImage = bitmap;
            return imageModel;
        }
    }
}
