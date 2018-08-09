using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Touch.Models;
using Touch.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Touch.Views.UserControls
{
    public sealed partial class FolderSourceControl : UserControl
    {

        public FolderSourceControl()
        {
            InitializeComponent();
        }

        private async void FolderSourceControl_OnLoaded(object sender, RoutedEventArgs e)
        {
//           _folderListViewModel =  ViewModelLocator.Instance.FolderListViewModel;
            //DataContext = ViewModelLocator.Instance.FolderListViewModel;
            //SourceList.ItemsSource = ViewModelLocator.Instance.FolderListViewModel.FolderList.FolderModels;
            //SourceList.DataContext = ViewModelLocator.Instance.FolderListViewModel;
            
        }


        private void SourceList_OnItemClick(object sender, ItemClickEventArgs e)
        {
            //            ((FolderListViewModel)DataContext).SelectedFolderModel = (FolderModel)e.ClickedItem;
            //            ((FolderListViewModel)DataContext).OpenCommand.Execute(
            //                ((FolderListViewModel)DataContext).SelectedFolderModel);
            FolderModel selectedFolderModel = (FolderModel)e.ClickedItem;
            ((FolderListViewModel)DataContext).OpenCommand.Execute(
                selectedFolderModel);

        }
    }
}
//using Windows.UI.Xaml;
//using Windows.UI.Xaml.Controls;
//using Touch.ViewModels;
//
//// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236
//
//namespace Touch.Views.UserControls
//{
//    public sealed partial class FolderSourceControl : UserControl
//    {
//        //private FolderListViewModel _folderListViewModel;
//
//        public FolderSourceControl()
//        {
//            InitializeComponent();
//        }
//
//        private  void FolderSourceControl_OnLoaded(object sender, RoutedEventArgs e)
//        {
//
//        }
//    }
//}