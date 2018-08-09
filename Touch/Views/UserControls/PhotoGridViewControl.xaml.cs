using System;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Touch.Models;
using Touch.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Touch.Views.UserControls
{
    // ReSharper disable once RedundantExtendsListEntry
    public sealed partial class PhotoGridViewControl : UserControl
    {
        public MemoryModel MemoryModel;

        public PhotoGridViewControl()
        {
            InitializeComponent();
        }

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
            ToggleItemPointAnimation(img, true);
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
            ToggleItemPointAnimation(img, false);
        }

        private void ToggleItemPointAnimation(FrameworkElement img, bool show)
        {
            var imgVisual = ElementCompositionPreview.GetElementVisual(img);
            var scaleAnimation = CreateScaleAnimation(show);
            imgVisual.CenterPoint = new Vector3((float) img.ActualWidth / 2, (float) img.ActualHeight / 2, 0f);
            imgVisual.StartAnimation("Scale.x", scaleAnimation);
            imgVisual.StartAnimation("Scale.y", scaleAnimation);
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
    }
}