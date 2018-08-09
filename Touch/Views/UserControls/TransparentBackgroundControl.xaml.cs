using System.Numerics;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Touch.Views.UserControls
{
    // ReSharper disable once RedundantExtendsListEntry
    public sealed partial class TransparentBackgroundControl : UserControl
    {
        private Compositor _compositor;
        private SpriteVisual _hostSprite;

        public TransparentBackgroundControl()
        {
            InitializeComponent();
            // 模糊效果
            ApplyAcrylicAccent(MainGrid);
            MainGrid.SizeChanged += (sender, args) =>
            {
                if (_hostSprite != null)
                    _hostSprite.Size = args.NewSize.ToVector2();
            };
        }

        /// <summary>
        ///     背景模糊特性
        /// </summary>
        /// <param name="panel"></param>
        private void ApplyAcrylicAccent(FrameworkElement panel)
        {
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            _hostSprite = _compositor.CreateSpriteVisual();
            _hostSprite.Size = new Vector2((float) panel.ActualWidth, (float) panel.ActualHeight);

            ElementCompositionPreview.SetElementChildVisual(panel, _hostSprite);
            // TODO 14393 15063
            _hostSprite.Brush = _compositor.CreateHostBackdropBrush();
        }
    }
}