using System;
using System.Diagnostics;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Touch.Models;
using Touch.ViewModels;
using Touch.Views.Pages;

// ReSharper disable CompareOfFloatsByEqualityOperator

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Touch.Views.UserControls
{
    // ReSharper disable once RedundantExtendsListEntry
    public sealed partial class MemoryGridViewControl : UserControl
    {
        private bool _isLoaded;

        /// <summary>
        ///     回忆VM
        /// </summary>

        public MemoryGridViewControl()
        {
            InitializeComponent();
            _isLoaded = false;
        }

        private async void MemoryGridViewControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (_isLoaded)
                return;


            await ViewModelLocator.Instance.MemoryListViewModel.GetInstanceAsync();
            DataContext = ViewModelLocator.Instance.MemoryListViewModel;
            MemoryGridView.ItemsSource = ViewModelLocator.Instance.MemoryListViewModel.MemoryList.MemoryModels;
            //SetTipGrid();
            MemoryGridView.DataContext = ViewModelLocator.Instance.MemoryListViewModel;
            _isLoaded = true;
            Debug.WriteLine("MemoryGridViewControl_OnLoaded");
        }

        private void ItemGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var rootGrid = sender as Grid;
            if (rootGrid == null)
                return;
            var maskBorder = rootGrid.Children[1] as FrameworkElement;
            var maskVisual = ElementCompositionPreview.GetElementVisual(maskBorder);
            maskVisual.Opacity = 0f;
        }

//        public void SetTipGrid()
//        {
//            TipGrid.Visibility = !MemoryListViewModel.MemoryViewModels.Any() ? Visibility.Visible : Visibility.Collapsed;
//        }

        // TODO 可以复用
        /// <summary>
        ///     item大小变化时需要对内容裁剪
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridViewItem_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var rootGrid = sender as Grid;
            if (rootGrid != null)
                rootGrid.Clip = new RectangleGeometry
                {
                    Rect = new Rect(0, 0, rootGrid.ActualWidth, rootGrid.ActualHeight)
                };
        }

        /// <summary>
        ///     鼠标进入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridViewItem_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var rootGrid = sender as Grid;
            if (rootGrid == null)
                return;
            var img = rootGrid.Children[0] as FrameworkElement;
            var maskBorder = rootGrid.Children[1] as FrameworkElement;
            ToggleItemPointAnimation(maskBorder, img, true);
        }

        /// <summary>
        ///     鼠标移出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridViewItem_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            var rootGrid = sender as Grid;
            if (rootGrid == null)
                return;
            var img = rootGrid.Children[0] as FrameworkElement;
            var maskBorder = rootGrid.Children[1] as FrameworkElement;
            ToggleItemPointAnimation(maskBorder, img, false);
        }

        private void ToggleItemPointAnimation(FrameworkElement mask, FrameworkElement img, bool show)
        {
            var maskVisual = ElementCompositionPreview.GetElementVisual(mask);
            var imgVisual = ElementCompositionPreview.GetElementVisual(img);

            var fadeAnimation = CreateFadeAnimation(show);
            var scaleAnimation = CreateScaleAnimation(show);

            if (imgVisual.CenterPoint.X == 0 && imgVisual.CenterPoint.Y == 0)
                imgVisual.CenterPoint = new Vector3((float) mask.ActualWidth / 2, (float) mask.ActualHeight / 2, 0f);

            maskVisual.StartAnimation("Opacity", fadeAnimation);
            imgVisual.StartAnimation("Scale.x", scaleAnimation);
            imgVisual.StartAnimation("Scale.y", scaleAnimation);
        }

        private ScalarKeyFrameAnimation CreateFadeAnimation(bool show)
        {
            var compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            var fadeAnimation = compositor.CreateScalarKeyFrameAnimation();
            fadeAnimation.InsertKeyFrame(1f, show ? 1f : 0f);
            fadeAnimation.Duration = TimeSpan.FromMilliseconds(1000);
            return fadeAnimation;
        }

        private ScalarKeyFrameAnimation CreateScaleAnimation(bool show)
        {
            var compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            var scaleAnimation = compositor.CreateScalarKeyFrameAnimation();
            scaleAnimation.InsertKeyFrame(1f, show ? 1.1f : 1f);
            scaleAnimation.Duration = TimeSpan.FromMilliseconds(1000);
            scaleAnimation.StopBehavior = AnimationStopBehavior.LeaveCurrentValue;
            return scaleAnimation;
        }

        private void MemoryGridView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            MemoryModel item = e.ClickedItem as MemoryModel;
            if (item == null)
                return;
            // 进入街景界面
            var rootFrame = Window.Current.Content as Frame;
            // connected animation
            ConnectedAnimationService.GetForCurrentView().DefaultDuration = TimeSpan.FromSeconds(0.5);
            var gridView = sender as GridView;
            // TODO 14393 15063
            gridView?.PrepareConnectedAnimation("CoverImage", item, "CoverImage");
            rootFrame?.Navigate(typeof(MemoryDetailPage), item);
            Window.Current.Content = rootFrame;
        }
    }
}