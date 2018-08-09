using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Media.Imaging;
using GalaSoft.MvvmLight;
using Touch.ViewModels;

namespace Touch.Models
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    /// <summary>
    ///     回忆
    /// </summary>
    public class MemoryModel : ObservableObject
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        /// <summary>
        ///     回忆图片里的第一个图片作为封面
        /// </summary>
        private BitmapImage _coverImage;

        public BitmapImage CoverImage
        {
            get => _coverImage;
            set => Set(nameof(CoverImage), ref _coverImage, value);
        }
        /// <summary>
        ///     回忆编号
        /// </summary>
        private int _keyNo;

        public int KeyNo
        {
            get => _keyNo;
            set => Set(nameof(KeyNo), ref _keyNo, value);
        }

        /// <summary>
        ///     回忆名字
        /// </summary>
        private string _memoryName;

        public string MemoryName
        {
            get => _memoryName;
            set => Set(nameof(MemoryName), ref _memoryName, value);
        }

        /// <summary>
        ///     回忆里的图片
        /// </summary>
        private List<ImageModel> _imageModels;

        public List<ImageModel> ImageModels
        {
            get => _imageModels;
            set => Set(nameof(ImageModels), ref _imageModels, value);
        }

#pragma warning disable 659
        public override bool Equals(object obj)
#pragma warning restore 659
        {
            var o = obj as MemoryModel;
            return o != null && o.KeyNo == KeyNo;
        }
    }
}