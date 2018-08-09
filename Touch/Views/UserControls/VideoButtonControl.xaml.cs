using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Touch.Views.UserControls
{
    // ReSharper disable once RedundantExtendsListEntry
    public sealed partial class VideoButtonControl : UserControl
    {
        public VideoButtonControl()
        {
            InitializeComponent();
            ToggleAnimation(false);
            PlayButton.Click += (sender, arg) => { OnPlayButtonClicked?.Invoke(); };
            ReplayButton.Click += (sender, arg) => { OnReplayButtonClicked?.Invoke(); };
        }

        public event Action OnPlayButtonClicked;
        public event Action OnReplayButtonClicked;

        /// <summary>
        ///     显示播放按钮
        /// </summary>
        public void ShowPlayButton()
        {
            ReplayButton.Visibility = Visibility.Collapsed;
            PlayButton.Visibility = Visibility.Visible;
            ToggleAnimation(true);
        }

        /// <summary>
        ///     显示重播按钮
        /// </summary>
        public void ShowReplayButton()
        {
            PlayButton.Visibility = Visibility.Collapsed;
            ReplayButton.Visibility = Visibility.Visible;
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