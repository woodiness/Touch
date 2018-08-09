using System;
using Windows.Globalization.DateTimeFormatting;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
    public sealed partial class CreateMemoryPage : Page
    {
        public CreateMemoryPage()
        {
            InitializeComponent();

            TitleBarControl.SetBackButtonVisibility(Visibility.Visible);
            // 设置为多选
            GalleryGridViewControl.SetGridViewMultipleSelection();
            // 标题box
            TitleBox.Text = new DateTimeFormatter("longdate").Format(DateTime.Now);
            // 完成button
            DoneButton.Click += async (sender, args) =>
            {
                MemoryModel memoryModel = await ViewModelLocator.Instance.MemoryListViewModel.CreateMemoryAsync(
                    ViewModelLocator.Instance.MemoryListViewModel.MemoryList.LastKeyNo,
                    TitleBox.Text, GalleryGridViewControl.SelectedImageModels);
                ViewModelLocator.Instance.MemoryListViewModel.AddCommand.Execute(memoryModel);
                GoBack();
            };
            // 取消button
            CancelButton.Click += (sender, args) => { GoBack(); };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
//            _memoryListView = e.Parameter as MemoryListViewModel;
        }

        /// <summary>
        ///     返回上一页
        /// </summary>
        private void GoBack()
        {
            var rootFrame = Window.Current.Content as Frame;
            rootFrame?.GoBack();
        }
    }
}