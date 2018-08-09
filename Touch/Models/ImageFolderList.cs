using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage.AccessCache;
using GalaSoft.MvvmLight;
using Touch.Data;

namespace Touch.Models
{
    /// <summary>
    ///     一个文件夹内的图片list
    /// </summary>
    public class ImageFolderList : ObservableObject
    {
        /// <summary>
        ///     数据库集合
        /// </summary>
        private  DatabaseHelper _databaseHelper;

        public DatabaseHelper DatabaseHelper
        {
            get => _databaseHelper;
            set => Set(nameof(DatabaseHelper), ref _databaseHelper, value);
        }

        /// <summary>
        ///     文件夹
        /// </summary>
        private  FolderModel _folderModel;

        public FolderModel FolderModel
        {
            get => _folderModel;
            set => Set(nameof(FolderModel), ref _folderModel, value);
        }

        /// <summary>
        ///     一个文件夹内的图片list
        /// </summary>
        private List<ImageModel> _imageModels;

        public List<ImageModel> ImageModels
        {
            get => _imageModels;
            set => Set(nameof(ImageModels), ref _imageModels, value);
        }

        public ImageFolderList(FolderModel folderModel)
        {
            _folderModel = folderModel;
            _databaseHelper = DatabaseHelper.GetInstance();
            ImageModels = new List<ImageModel>();
        }
    }
}