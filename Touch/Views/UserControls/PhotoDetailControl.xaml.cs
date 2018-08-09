using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using CompositionHelper;
using Touch.Models;
using Touch.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Touch.Views.UserControls
{
    // ReSharper disable once RedundantExtendsListEntry
    public sealed partial class PhotoDetailControl : UserControl
    {
        private Compositor _compositor;
        private Visual _detailGridVisual;
        private Visual _infoGridVisual;
        private Visual _shareBtnVisual;
        public ImageModel PhotoDetailImageModel;

        public PhotoDetailControl()
        {
            InitializeComponent();
            InitComposition();
            ToggleDetailGridAnimation(false);
            // 分享
            var dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += (manager, eventArgs) =>
            {
                var request = eventArgs.Request;
                var requestData = request.Data;
                requestData.Properties.Title = new ResourceLoader().GetString("SharePhoto");
                // It's recommended to use both SetBitmap and SetStorageItems for sharing a single image
                // since the target app may only support one or the other.
                var imageItems = new List<IStorageItem>
                {
                    PhotoDetailImageModel.ImageFile
                };
                requestData.SetStorageItems(imageItems);
                var imageStreamRef = RandomAccessStreamReference.CreateFromFile(PhotoDetailImageModel.ImageFile);
                requestData.Properties.Thumbnail = imageStreamRef;
                requestData.SetBitmap(imageStreamRef);
            };
            // 分享button
            ShareBtn.Click += (sender, args) => { DataTransferManager.ShowShareUI(); };
        }

        public event Action OnHide;

        private void InitComposition()
        {
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            _detailGridVisual = ElementCompositionPreview.GetElementVisual(DetailGrid);
            _infoGridVisual = ElementCompositionPreview.GetElementVisual(InfoGrid);
            _shareBtnVisual = ElementCompositionPreview.GetElementVisual(ShareBtn);

            ResetVisualInitState();
        }

        private void ResetVisualInitState()
        {
            _infoGridVisual.Offset = new Vector3(0f, -100f, 0);
            _shareBtnVisual.Offset = new Vector3(150f, 0f, 0f);
            _detailGridVisual.Opacity = 0;
        }

        /// <summary>
        ///     Toggle the enter animation by passing a list item. This control will take care of the rest part.
        /// </summary>
        public void Show()
        {
            DetailImage.Source = PhotoDetailImageModel.ThumbnailImage;
            ToggleDetailGridAnimation(true);
        }

        private void ToggleDetailGridAnimation(bool show)
        {
            var fadeAnimation = _compositor.CreateScalarKeyFrameAnimation();
            fadeAnimation.InsertKeyFrame(1f, show ? 1f : 0f);
            fadeAnimation.Duration = TimeSpan.FromMilliseconds(show ? 700 : 300);
            fadeAnimation.DelayTime = TimeSpan.FromMilliseconds(show ? 400 : 0);

            var batch = _compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
            _detailGridVisual.StartAnimation("Opacity", fadeAnimation);

            if (show)
            {
                ToggleShareBtnAnimation(true);
                ToggleInfoGridAnimation(true);
            }

            batch.Completed += (sender, e) =>
            {
                if (show)
                    return;
                ResetVisualInitState();
                Visibility = Visibility.Collapsed;
                ToggleElementsOpacity(true);
            };

            batch.End();
        }

        private void ToggleShareBtnAnimation(bool show)
        {
            var offsetAnimation = _compositor.CreateVector3KeyFrameAnimation();
            offsetAnimation.InsertKeyFrame(1f, new Vector3(show ? 0f : 150f, 0f, 0f));
            offsetAnimation.Duration = TimeSpan.FromMilliseconds(show ? 1000 : 400);
            offsetAnimation.DelayTime = TimeSpan.FromMilliseconds(show ? 400 : 0);

            _shareBtnVisual.StartAnimation("Offset", offsetAnimation);
        }

        private void ToggleInfoGridAnimation(bool show)
        {
            var offsetAnimation = _compositor.CreateVector3KeyFrameAnimation();
            offsetAnimation.InsertKeyFrame(1f, new Vector3(0f, show ? 0f : -100f, 0f));
            offsetAnimation.Duration = TimeSpan.FromMilliseconds(500);
            offsetAnimation.DelayTime = TimeSpan.FromMilliseconds(show ? 500 : 0);

            _infoGridVisual.StartAnimation("Offset", offsetAnimation);
        }

        private void MaskBorder_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ToggleShareBtnAnimation(false);
            var batch = _compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
            ToggleInfoGridAnimation(false);
            batch.Completed += (s, ex) =>
            {
                var innerBatch = _compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
                innerBatch.Completed += (ss, exx) =>
                {
                    OnHide?.Invoke();
                    ToggleDetailGridAnimation(false);
                    PhotoDetailImageModel = null;
                };
                innerBatch.End();
            };
            batch.End();
        }

        private void DetailGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var grid = sender as Grid;
            if (grid == null)
                return;
            var gridWidth = grid.ActualWidth;
            var gridHeight = grid.ActualHeight;
            DetailImage.Width = gridWidth * 0.618;
            DetailImage.Height = gridHeight * 0.618;
            PhotoGrid.Height = PhotoGrid.ActualWidth / 1.5 + 100;
            PhotoGrid.Clip = new RectangleGeometry
            {
                Rect = new Rect(0, 0, PhotoGrid.ActualWidth, PhotoGrid.Height)
            };
        }

        private void InfoPlaceHolderGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetInfoPlaceholderGridClip(true);
        }

        private void SetInfoPlaceholderGridClip(bool clip)
        {
            if (!clip)
            {
                InfoPlaceHolderGrid.ClearValue(ClipProperty);
                return;
            }
            InfoPlaceHolderGrid.Clip = new RectangleGeometry
            {
                Rect = new Rect(0, 0, InfoPlaceHolderGrid.ActualWidth, InfoPlaceHolderGrid.ActualHeight)
            };
        }

        private void ToggleElementsOpacity(bool show)
        {
            InfoPlaceHolderGrid.GetVisual().Opacity = show ? 1f : 0f;
            OperationSp.GetVisual().Opacity = show ? 1f : 0f;
        }
    }
}