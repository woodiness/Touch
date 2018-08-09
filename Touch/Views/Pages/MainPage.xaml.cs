using System;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Touch.Models;
using Touch.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Touch.Views.Pages
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public sealed partial class MainPage : Page
    {
        private bool _isRefreshing;
        private int _rotationDegree = 360;

        public MainPage()
        {
            InitializeComponent();
            // 刷新button点击事件
            RefreshButton.Click += async (sender, args) =>
            {
                if (_isRefreshing)
                    return;
                _isRefreshing = true;
                await StartRotationAnimation();
                await GalleryGridViewControl.RefreshAsync();
                _isRefreshing = false;
            };
            // 添加回忆点击事件
            CreateMemoryButton.Click += (sender, args) =>
            {
                var rootFrame = Window.Current.Content as Frame;
                rootFrame?.Navigate(typeof(CreateMemoryPage), ViewModelLocator.Instance.MemoryListViewModel);
                Window.Current.Content = rootFrame;
            };
            // 设置button点击事件
            SettingButton.Click += (sender, args) =>
            {
                var rootFrame = Window.Current.Content as Frame;
                rootFrame?.Navigate(typeof(SettingPage));
                Window.Current.Content = rootFrame;
            };
            // tab切换事件
            MainPivot.SelectionChanged += (sender, args) =>
            {
                var pivot = sender as Pivot;
                CreateMemoryButton.Visibility = pivot?.SelectedIndex == 1 ? Visibility.Visible : Visibility.Collapsed;
            };
        }

        /// <summary>
        ///     旋转动画（新开一个线程）
        /// </summary>
        /// <returns></returns>
        private async Task StartRotationAnimation()
        {
            var centerX = (float) (RefreshIcon.ActualWidth / 2);
            var centerY = (float) (RefreshIcon.ActualHeight / 2);
            await Task.Run(async () =>
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    while (_isRefreshing)
                    {
                        await RefreshIcon.Rotate(_rotationDegree, centerX, centerY, 1000, 0, EasingType.Linear)
                            .StartAsync();
                        _rotationDegree += 360;
                    }
                });
            });
        }

        private void GalleryGridViewControl_OnClickItemStarted(ImageModel imageModel)
        {
            DetailControl.PhotoDetailImageModel = imageModel;
            DetailControl.Visibility = Visibility.Visible;
            DetailControl.Show();
        }

        private void DetailControl_OnHide()
        {
            DetailControl.Visibility = Visibility.Collapsed;
            GalleryGridViewControl.Dismissed();
        }
    }
}