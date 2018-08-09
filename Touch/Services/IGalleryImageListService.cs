using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Touch.Models;

namespace Touch.Services
{
    public interface IGalleryImageListService
    {
        Task<List<ImageFolderList>> RefreshFolderListAsync(FolderList folderList);
        Task<IOrderedEnumerable<ImageMonthGroup>> GroupImageAsync(List<ImageFolderList> imageFolderLists);
        Task<IOrderedEnumerable<ImageMonthGroup>> RefreshImageListAsync(List<ImageFolderList> imageFolderLists);
        Task DeleteAsync(ImageModel imageModel,  List<ImageFolderList> imageFolderLists, IOrderedEnumerable<ImageMonthGroup> imageMonthGroups);
        Task<IOrderedEnumerable<ImageMonthGroup>> FindImagesAsync(IOrderedEnumerable<ImageMonthGroup> imageMonthGroups);
    }
}
