using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Effects;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Microsoft.Graphics.Canvas.Effects;
using Touch.Models;
using Touch.ViewModels;
using Touch.Views.Pages;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Touch.Views.UserControls
{
    // ReSharper disable once RedundantExtendsListEntry
    public sealed partial class CreateMemoryControl : UserControl
    {
        private readonly Compositor _compositor;

        private readonly SpriteVisual _destinationSprite;

        private bool _isLoaded;
        private CompositionScopedBatch _scopeBatch;

        public CreateMemoryControl()
        {
            InitializeComponent();
            _isLoaded = false;

            #region 模糊特效

            // Get the current compositor
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            // Create the destinatio sprite, sized to cover the entire list
            _destinationSprite = _compositor.CreateSpriteVisual();
            _destinationSprite.Size = new Vector2((float)GalleryGridView.ActualWidth,
                (float)GalleryGridView.ActualHeight);
            // Start out with the destination layer invisible to avoid any cost until necessary
            _destinationSprite.IsVisible = false;
            ElementCompositionPreview.SetElementChildVisual(GalleryGridView, _destinationSprite);
            if (_compositor != null)
            {
                IGraphicsEffect graphicsEffect = new GaussianBlurEffect
                {
                    BlurAmount = 20,
                    Source = new CompositionEffectSourceParameter("ImageSource"),
                    Optimization = EffectOptimization.Balanced,
                    BorderMode = EffectBorderMode.Hard
                };

                // Create the effect factory and instantiate a brush
                var effectFactory = _compositor.CreateEffectFactory(graphicsEffect, null);
                var brush = effectFactory.CreateBrush();

                // Set the destination brush as the source of the image content
                brush.SetSourceParameter("ImageSource", _compositor.CreateBackdropBrush());

                // Update the destination layer with the fully configured brush
                _destinationSprite.Brush = brush;
            }

            #endregion

            GalleryGridView.ItemClick += (sender, e) =>
            {
                var thumbnail = (ImageModel)e.ClickedItem;

                // If we're in the middle of an animation, cancel it now
                if (_scopeBatch != null)
                    CleanupScopeBatch();

                // We're starting our transition, show the destination sprite
                _destinationSprite.IsVisible = true;

                // Animate from transparent to fully opaque
                var showAnimation = _compositor.CreateScalarKeyFrameAnimation();
                showAnimation.InsertKeyFrame(0f, 0f);
                showAnimation.InsertKeyFrame(1f, 1f);
                showAnimation.Duration = TimeSpan.FromMilliseconds(1500);
                _destinationSprite.StartAnimation("Opacity", showAnimation);

                OnClickItemStarted?.Invoke(thumbnail);
            };
            GalleryGridView.SizeChanged += (sender, e) =>
            {
                if (_destinationSprite != null)
                    _destinationSprite.Size = e.NewSize.ToVector2();
            };
            AddSourcesButton.Click += (sender, args) =>
            {
                var rootFrame = Window.Current.Content as Frame;
                rootFrame?.Navigate(typeof(SettingPage));
                Window.Current.Content = rootFrame;
            };
        }

        /// <summary>
        ///     选中的viewmodels
        /// </summary>
        public List<ImageModel> SelectedImageModels
        {
            get { return GalleryGridView.SelectedItems.Cast<ImageModel>().ToList(); }
        }

        public event Action<ImageModel> OnClickItemStarted;

        public void Dismissed()
        {
            // Start a scoped batch so we can register to completion event and hide the destination layer
            _scopeBatch = _compositor.CreateScopedBatch(CompositionBatchTypes.Animation);

            // Start the hide animation to fade out the destination effect
            var hideAnimation = _compositor.CreateScalarKeyFrameAnimation();
            hideAnimation.InsertKeyFrame(0f, 1f);
            hideAnimation.InsertKeyFrame(1.0f, 0f);
            hideAnimation.Duration = TimeSpan.FromMilliseconds(1000);
            _destinationSprite.StartAnimation("Opacity", hideAnimation);

            //Scoped batch completed event
            _scopeBatch.Completed += ScopeBatch_Completed;
            _scopeBatch.End();
        }

        private void ScopeBatch_Completed(object sender, CompositionBatchCompletedEventArgs args)
        {
            // Scope batch completion event has fired, hide the destination sprite and cleanup the batch
            _destinationSprite.IsVisible = false;

            CleanupScopeBatch();
        }

        private void CleanupScopeBatch()
        {
            if (_scopeBatch == null)
                return;
            _scopeBatch.Completed -= ScopeBatch_Completed;
            _scopeBatch.Dispose();
            _scopeBatch = null;
        }

        /// <summary>
        ///     刷新图库
        /// </summary>
        public async Task RefreshAsync()
        {
                
            Debug.WriteLine("RefreshAsync---" + "_galleryImageListViewModel.RefreshFolderListAsync()");
            await ViewModelLocator.Instance.GalleryImageListViewModel.RefreshFolderListAsync();
            Debug.WriteLine("RefreshAsync---" + "_galleryImageListViewModel.RefreshImageListAsync()");
            await ViewModelLocator.Instance.GalleryImageListViewModel.RefreshImageListAsync();
            Debug.WriteLine("RefreshAsync---" + "Cvs.Source");
            SetTipGrid();
            Cvs.Source = ViewModelLocator.Instance.GalleryImageListViewModel.ImageMonthGroups;
        }

        private void SetTipGrid()
        {
            TipGrid.Visibility = !ViewModelLocator.Instance.GalleryImageListViewModel.ImageMonthGroups.Any()
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        /// <summary>
        ///     把gridview设置为多选
        /// </summary>
        public void SetGridViewMultipleSelection()
        {
            GalleryGridView.SelectionMode = ListViewSelectionMode.Multiple;
            GalleryGridView.IsItemClickEnabled = false;
        }

        /// <summary>
        ///     把gridview设置为点击
        /// </summary>
        public void SetGridViewClickable()
        {
            GalleryGridView.SelectionMode = ListViewSelectionMode.None;
            GalleryGridView.IsItemClickEnabled = true;
        }

        private async void CreateMemoryControl_OnLoading(FrameworkElement sender, object args)
        {
            if (_isLoaded)
                return;
            Debug.WriteLine("GalleryGridViewControl_OnLoading---" + "GalleryImageListViewModel.GetInstanceAsync()");
            await ViewModelLocator.Instance.GalleryImageListViewModel.GetInstanceAsync();
            await ViewModelLocator.Instance.GalleryImageListViewModel.FindImagesAsync();
            
            Debug.WriteLine("GalleryGridViewControl_OnLoading---" + "Cvs.Source");
            SetTipGrid();

            Cvs.Source = ViewModelLocator.Instance.GalleryImageListViewModel.MemoryImageMonthGroups;
            _isLoaded = true;
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
            imgVisual.CenterPoint = new Vector3((float)img.ActualWidth / 2, (float)img.ActualHeight / 2, 0f);
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