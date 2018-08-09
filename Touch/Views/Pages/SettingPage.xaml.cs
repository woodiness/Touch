using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Touch.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Touch.Views.Pages
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public sealed partial class SettingPage : Page
    {
        public SettingPage()
        {
            InitializeComponent();
            TitleBarControl.SetBackButtonVisibility(Visibility.Visible);
            DataContext = ViewModelLocator.Instance.FolderListViewModel;
        }
    }
}