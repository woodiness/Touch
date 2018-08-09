using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using Touch.Models;
using Touch.ViewModels;

namespace Touch.Services
{
    public interface IImageModelService
    {
        Task<StorageFile> GetImageFileAsync(string accessToken);
        Task<ImageModel> GetInstanceAsync(int folderKeyNo, string imagePath, string accessToken);
        Task<ImageModel> GetThumbnailImageAsync(uint requiredSize, ImageModel imageModel);
    }
}
