using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml.Media.Imaging;
using GalaSoft.MvvmLight;

namespace Touch.Models
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    /// <summary>
    ///     图片
    /// </summary>
    public class ImageModel : ObservableObject, IComparable<ImageModel>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public ImageModel()
        {

        }

        /// <summary>
        ///     图片编号
        /// </summary>
        private int _keyNo;

        public int KeyNo
        {
            get => _keyNo;
            set => Set(nameof(KeyNo), ref _keyNo, value);
        }

        /// <summary>
        ///     所属文件夹编号
        /// </summary>
        private int _folderKeyNo;

        public int FolderKeyNo
        {
            get => _folderKeyNo;
            set => Set(nameof(FolderKeyNo), ref _folderKeyNo, value);
        }

        /// <summary>
        ///     图片路径
        /// </summary>
        private string _imagePath;

        public string ImagePath
        {
            get => _imagePath;
            set => Set(nameof(ImagePath), ref _imagePath, value);
        }

        /// <summary>
        ///     访问权限
        /// </summary>
        private string _accessToken;

        public string AccessToken
        {
            get => _accessToken;
            set => Set(nameof(AccessToken), ref _accessToken, value);
        }

        /// <summary>
        ///     图片宽度
        /// </summary>
        private uint _width;
        public uint Width
        {
            get => _width;
            set => Set(nameof(Width), ref _width, value);
        }

        /// <summary>
        ///     图片长度
        /// </summary>
        private uint _height;

        public uint Height
        {
            get => _height;
            set => Set(nameof(Height), ref _height, value);
        }

        /// <summary>
        ///     图片纬度
        /// </summary>
        private double? _latitude;

        public double? Latitude
        {
            get => _latitude;
            set => Set(nameof(Latitude), ref _latitude, value);
        }

        /// <summary>
        ///     图片经度
        /// </summary>
        private double? _longitude;

        public double? Longitude
        {
            get => _longitude;
            set => Set(nameof(Longitude), ref _longitude, value);
        }

        /// <summary>
        ///     拍摄日期
        /// </summary>
        private DateTime _dateTime;

        public DateTime DateTaken
        {
            get => _dateTime;
            set => Set(nameof(DateTaken), ref _dateTime, value);
        }

        /// <summary>
        ///     只有年和月的拍摄日期
        /// </summary>
        private MonthYearDateTime _monthYearDate;

        public MonthYearDateTime MonthYearDate
        {
            get => _monthYearDate= new MonthYearDateTime(DateTaken);
            set => Set(nameof(MonthYearDate), ref _monthYearDate, value);

        }
        //MonthYearDate => new MonthYearDateTime(DateTaken);
        /// <summary>
        ///     图片文件
        /// </summary>
        private StorageFile _imageFile;

        public StorageFile ImageFile
        {
            get => _imageFile;
            set => Set(nameof(ImageFile), ref _imageFile, value);
        }

        /// <summary>
        ///     缩略图
        /// </summary>
        private BitmapImage _thumbnailImage;
        public BitmapImage ThumbnailImage
        {
            get => _thumbnailImage;
            set => Set(nameof(ThumbnailImage), ref _thumbnailImage, value);
        }
        /// <summary>
        /// 是否有街景
        /// </summary>
        private string _status;
        public string Status
        {
            get => _status;
            set => Set(nameof(Status), ref _status, value);
        }
        /// <summary>
        ///     排序比较函数实现
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(ImageModel other)
        {
            if (DateTaken < other.DateTaken)
                return -1;
            return DateTaken == other.DateTaken ? 0 : 1;
        }





#pragma warning disable 659
        public override bool Equals(object obj)
#pragma warning restore 659
        {
            var o = obj as ImageModel;
            return o != null && o.ImagePath == ImagePath;
        }
    }
}