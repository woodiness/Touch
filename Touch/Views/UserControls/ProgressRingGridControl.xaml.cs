using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Touch.Views.UserControls
{
    // ReSharper disable once RedundantExtendsListEntry
    public sealed partial class ProgressRingGridControl : UserControl
    {
        public ProgressRingGridControl()
        {
            InitializeComponent();
            ToggleAnimation(false);
        }

        /// <summary>
        ///     显示
        /// </summary>
        public void Show()
        {
            ToggleAnimation(true);
        }

        /// <summary>
        ///     隐藏
        /// </summary>
        public void Hide()
        {
            ToggleAnimation(false);
        }

        // TODO 复用
        private void ToggleAnimation(bool show)
        {
            var compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            var detailGridVisual = ElementCompositionPreview.GetElementVisual(RootGrid);

            var fadeAnimation = compositor.CreateScalarKeyFrameAnimation();
            fadeAnimation.InsertKeyFrame(1f, show ? 1f : 0f);
            fadeAnimation.Duration = TimeSpan.FromMilliseconds(700);

            detailGridVisual.StartAnimation("Opacity", fadeAnimation);
        }
    }
}