using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight;

namespace Touch.Models
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    /// <summary>
    ///     文件夹
    /// </summary>
    public class FolderModel : ObservableObject
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        /// <summary>
        /// 文件夹编号
        /// </summary>
        private int _keyNo;

        /// <summary>
        ///     文件夹编号
        /// </summary>
        public int KeyNo
        {
            get => _keyNo;
            set => Set(nameof(KeyNo),ref _keyNo,value);
        }
        /// <summary>
        /// 文件夹路径
        /// </summary>
        private string _folderPath;

        /// <summary>
        ///     文件夹路径
        /// </summary>
        public string FolderPath
        {
            get => _folderPath;
            set => Set(nameof(FolderPath), ref _folderPath, value);
        }
        /// <summary>
        /// 访问权限
        /// </summary>
        private string _accessToken;

        /// <summary>
        ///     访问权限
        /// </summary>
        public string AccessToken
        {
            get => _accessToken;
            set => Set(nameof(AccessToken),ref  _accessToken,value);
        }

        /// <summary>
        ///     图标
        /// </summary>
        private string _itemSymbol;
        /// <summary>
        ///     图标
        /// </summary>
        public string ItemSymbol
        {
            get => _itemSymbol;
            set => Set(nameof(ItemSymbol), ref _itemSymbol, value);
        }

        /// <summary>
        ///     删除按钮是否可见
        /// </summary>
        private Visibility _isDeleteVisibility;

        /// <summary>
        ///     删除按钮是否可见
        /// </summary>
        public Visibility IsDeleteVisibility
        {
            get => _isDeleteVisibility;
            set => Set(nameof(IsDeleteVisibility), ref _isDeleteVisibility, value);
        }

        public FolderModel()
        {
            _isDeleteVisibility = Visibility.Visible;
            _itemSymbol = Application.Current.Resources["Folder"] as string;
        }
#pragma warning disable 659
        public override bool Equals(object obj)
#pragma warning restore 659
        {
            var o = obj as FolderModel;
            return o != null && o.FolderPath == FolderPath;
        }
    }
}