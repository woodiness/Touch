using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Touch.Models;
using Touch.Services;

namespace Touch.ViewModels
{
    /// <summary>
    ///     图库图片list
    /// </summary>
    public class GalleryImageListViewModel : ViewModelBase
    {
        private readonly IGalleryImageListService _galleryImageListService;
        private readonly IImageModelService _imageModelService;
        private readonly IFolderListService _folderListService;
        private readonly IImageFolderListService _imageFolderListService;
        private int _flag = 0;

        public int Flag
        {
            get => _flag;
            set => Set(nameof(Flag), ref _flag, value);
        }
        /// <summary>
        ///     所有的文件夹里的图片
        /// </summary>
        private  List<ImageFolderList> _imageFolderLists;

        public List<ImageFolderList> ImageFolderLists
        {
            get => _imageFolderLists;
            set => Set(nameof(ImageFolderLists), ref _imageFolderLists, value);
        }

        /// <summary>
        ///     所有文件夹
        /// </summary>
        private FolderList _folderList;

        public FolderList FolderList
        {
            get => _folderList;
            set => Set(nameof(FolderList), ref _folderList, value);
        }
        /// <summary>
        ///     按月份分好类的图片list
        /// </summary>
        private IOrderedEnumerable<ImageMonthGroup> _imageMonthGroups;

        public IOrderedEnumerable<ImageMonthGroup> ImageMonthGroups
        {
            get => _imageMonthGroups;
            set => Set(nameof(ImageMonthGroups), ref _imageMonthGroups, value);
        }
        /// <summary>
        /// 可用于制作回忆的照片：有GPS && 有街景
        /// </summary>
        private IOrderedEnumerable<ImageMonthGroup> _memoryImageMonthGroups;

        public IOrderedEnumerable<ImageMonthGroup> MemoryImageMonthGroups
        {
            get => _memoryImageMonthGroups;
            set => Set(nameof(MemoryImageMonthGroups), ref _memoryImageMonthGroups, value);
        }
        public GalleryImageListViewModel(IGalleryImageListService galleryImageListService,IImageModelService imageModelService,
            IFolderListService folderListService,IImageFolderListService imageFolderListService)
        {
            _imageFolderListService = imageFolderListService;
            _folderListService = folderListService;
            _galleryImageListService = galleryImageListService;
            _imageModelService = imageModelService;
            _imageFolderLists = new List<ImageFolderList>();
            _flag = 0;
            MemoryImageMonthGroups = ImageMonthGroups;
        }
        /// <summary>
        ///     删除点击操作
        /// </summary>
        public ICommand DeleteCommand
        {
            
            get
            {
                return new CommandHandler(async imageModel =>
                    await _galleryImageListService.DeleteAsync(imageModel as ImageModel, ImageFolderLists,ImageMonthGroups));
            }
        }
        /// <summary>
        ///     异步获取实例
        /// </summary>
        /// <returns></returns>
        public  async Task GetInstanceAsync()
        {
            await RefreshFolderListAsync();
            await GroupImageAsync();
        }

        public async Task FindImagesAsync()
        {

            MemoryImageMonthGroups = await _galleryImageListService.FindImagesAsync(ImageMonthGroups);
        }

        /// <summary>
        ///     刷新文件夹list，但不刷新图片list
        /// </summary>
        /// <returns></returns>
        public async Task RefreshFolderListAsync()
        {
            FolderList = _folderListService.GetInstanceAsync();
            //var s = FolderList.GetHashCode();
            ImageFolderLists = await _galleryImageListService.RefreshFolderListAsync(FolderList);
        }
        /// <summary>
        ///     刷新当前文件夹list里的所有图片（不刷新文件夹list）
        /// </summary>
        /// <returns></returns>
        public async Task RefreshImageListAsync()
        {
            ImageMonthGroups = await _galleryImageListService.RefreshImageListAsync(ImageFolderLists);
        }

        /// <summary>
        ///     按月份分类图片
        /// </summary>
        private async Task GroupImageAsync()
        {
            ImageMonthGroups = await _galleryImageListService.GroupImageAsync(ImageFolderLists);
        }
    
    }
}