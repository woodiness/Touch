using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Composition;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Touch.Models;
using Touch.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Touch.Views.Pages
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public sealed partial class MemoryDetailPage : Page
    {
        private MemoryModel _memoryModel;

        public MemoryDetailPage()
        {
            InitializeComponent();
            InitVisual();
            TitleBarControl.SetBackButtonVisibility(Visibility.Visible);
            ShowButton.Click += (sender, args) =>
            {
                // 进入街景界面
                var rootFrame = Window.Current.Content as Frame;
                rootFrame?.Navigate(typeof(StreetViewPage), _memoryModel);
                Window.Current.Content = rootFrame;
                Debug.WriteLine("进入街景界面");
            };
            AudioButton.Click += async (sender, args) =>
            {
                var filePicker = new FileOpenPicker();
                filePicker.FileTypeFilter.Add(".mp3");
                filePicker.SuggestedStartLocation = PickerLocationId.MusicLibrary;
                var file = await filePicker.PickSingleFileAsync();
                if (file == null)
                    return;
                var newFile = await file.CopyAsync(ApplicationData.Current.LocalFolder);
                await newFile.RenameAsync(_memoryModel.KeyNo.ToString(), NameCollisionOption.ReplaceExisting);
                // 显示成功通知
                var flyout = FlyoutBase.GetAttachedFlyout((FrameworkElement) sender);
                flyout.ShowAt((FrameworkElement) sender);
                await Task.Run(async () =>
                {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        await Task.Delay(1000);
                        flyout.Hide();
                    });
                });
            };
            DeleteButton.Click += (sender, args) =>
            {
                var rootFrame = Window.Current.Content as Frame;
                //_memoryListViewModel.Delete(_memoryViewModel);

                ViewModelLocator.Instance.MemoryListViewModel.DeleteCommand.Execute(_memoryModel);
                rootFrame?.GoBack();
            };
        }

        private void InitVisual()
        {
            // TODO 14393 15063
            var compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            // 动画结束后要盖一层图片，不然会闪
            var coverImageCoverOpacityAnimation = compositor.CreateScalarKeyFrameAnimation();
            coverImageCoverOpacityAnimation.DelayTime = TimeSpan.FromSeconds(0.5);
            coverImageCoverOpacityAnimation.Duration = TimeSpan.FromMilliseconds(1);
            coverImageCoverOpacityAnimation.Target = "Opacity";
            coverImageCoverOpacityAnimation.InsertKeyFrame(0, 0);
            coverImageCoverOpacityAnimation.InsertKeyFrame(1, 1);
            ElementCompositionPreview.SetIsTranslationEnabled(CoverImageCover, true);
            ElementCompositionPreview.GetElementVisual(CoverImageCover);
            ElementCompositionPreview.SetImplicitShowAnimation(CoverImageCover, coverImageCoverOpacityAnimation);

            // Add a translation animation that will play when this element is shown
            var topBorderOpacityAnimation = compositor.CreateScalarKeyFrameAnimation();
            topBorderOpacityAnimation.DelayTime = TimeSpan.FromSeconds(0.5);
            topBorderOpacityAnimation.Duration = TimeSpan.FromSeconds(1);
            topBorderOpacityAnimation.Target = "Opacity";
            topBorderOpacityAnimation.InsertKeyFrame(0, 0);
            topBorderOpacityAnimation.InsertKeyFrame(1, 1);
            ElementCompositionPreview.SetIsTranslationEnabled(TopBorder, true);
            ElementCompositionPreview.GetElementVisual(TopBorder);
            ElementCompositionPreview.SetImplicitShowAnimation(TopBorder, topBorderOpacityAnimation);

            // Add an opacity and translation animation that will play when this element is shown
            var mainContentTranslationAnimation = compositor.CreateScalarKeyFrameAnimation();
            mainContentTranslationAnimation.DelayBehavior = AnimationDelayBehavior.SetInitialValueBeforeDelay;
            mainContentTranslationAnimation.DelayTime = TimeSpan.FromSeconds(0.2);
            mainContentTranslationAnimation.Duration = TimeSpan.FromSeconds(0.45);
            mainContentTranslationAnimation.Target = "Translation.Y";
            mainContentTranslationAnimation.InsertKeyFrame(0, 50.0f);
            mainContentTranslationAnimation.InsertKeyFrame(1, 0);

            var mainContentOpacityAnimation = compositor.CreateScalarKeyFrameAnimation();
            mainContentOpacityAnimation.Duration = TimeSpan.FromSeconds(0.4);
            mainContentOpacityAnimation.Target = "Opacity";
            mainContentOpacityAnimation.InsertKeyFrame(0, 0);
            mainContentOpacityAnimation.InsertKeyFrame(0.25f, 0);
            mainContentOpacityAnimation.InsertKeyFrame(1, 1);

            var mainContentShowAnimations = compositor.CreateAnimationGroup();
            mainContentShowAnimations.Add(mainContentTranslationAnimation);
            mainContentShowAnimations.Add(mainContentOpacityAnimation);

            ElementCompositionPreview.SetIsTranslationEnabled(MainContent, true);
            ElementCompositionPreview.GetElementVisual(MainContent);
            ElementCompositionPreview.SetImplicitShowAnimation(MainContent, mainContentShowAnimations);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            // connected animation
            MemoryModel memoryDetailParameters = e.Parameter as MemoryModel;
            _memoryModel = memoryDetailParameters;
            
            var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("CoverImage");
            animation?.TryStart(CoverImage);

            
            PhotoGridView.MemoryModel = _memoryModel;
            //var s = 1;
        }
    }
}