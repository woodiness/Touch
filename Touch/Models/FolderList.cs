using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage.AccessCache;
using GalaSoft.MvvmLight;
using Touch.Data;

namespace Touch.Models
{
    /// <summary>
    ///     文件夹路径的list
    /// </summary>
    public class FolderList : ObservableObject
    {
        /// <summary>
        ///     数据库集合
        /// </summary>
        private  DatabaseHelper _databaseHelper;

        /// <summary>
        /// 取数据库集合
        /// </summary>
        public DatabaseHelper DatabaseHelper
        {
            get => _databaseHelper;
            set => Set(nameof(DatabaseHelper), ref _databaseHelper, value);
        }

        /// <summary>
        ///     文件夹路径的list
        /// </summary>
        private ObservableCollection<FolderModel> _folderModels;
        public ObservableCollection<FolderModel> FolderModels
        {
            get => _folderModels;
            set => Set(nameof(FolderModels), ref _folderModels, value);
        }

        public FolderList()
        {
            _databaseHelper = DatabaseHelper.GetInstance();
            _folderModels = new ObservableCollection<FolderModel>();
        }
#pragma warning disable 659
        public override bool Equals(object obj)
#pragma warning restore 659
        {
            var o = obj as FolderList;
            return o != null && o.FolderModels == FolderModels;
        }
    }
}