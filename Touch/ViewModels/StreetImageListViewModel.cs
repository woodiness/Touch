using System.Collections.Generic;
using System.Collections.ObjectModel;
using Touch.Models;

namespace Touch.ViewModels
{
    /// <summary>
    ///     街景图片的VM
    /// </summary>
    public class StreetImageListViewModel : NotificationBase
    {
        private int _selectedIndex;

        /// <summary>
        ///     与view交互的list
        /// </summary>
        public ObservableCollection<ImageModel> ImageModels;

        public StreetImageListViewModel()
        {
            ImageModels = new ObservableCollection<ImageModel>
            {
                null
            };
            _selectedIndex = 0;
        }

        /// <summary>
        ///     当前选中的index
        /// </summary>
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (value < 0 || value >= ImageModels.Count)
                    return;
                if (SetProperty(ref _selectedIndex, value))
                    RaisePropertyChanged(nameof(SelectedImage));
            }
        }

        /// <summary>
        ///     选中的imageVM
        /// </summary>
        public ImageModel SelectedImage
        {
            get { return _selectedIndex >= 0 ? ImageModels[_selectedIndex] : null; }
        }

        /// <summary>
        ///     添加街景图片
        /// </summary>
        /// <param name="imageViewModels"></param>
        public void AddImages(List<ImageModel> imageViewModels)
        {
            ImageModels.Clear();
            foreach (var imageViewModel in imageViewModels)
                ImageModels.Add(imageViewModel);
            SelectedIndex = 0;
        }
    }
}