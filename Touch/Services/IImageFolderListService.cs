using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Touch.Models;

namespace Touch.Services
{
    public interface IImageFolderListService
    {
        Task<ImageFolderList> GetInstanceAsync(FolderModel folderModel);
        Task<ImageFolderList> RefreshAsync(ImageFolderList imageFolderList);
    }
}
