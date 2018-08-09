using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Touch.Views.UserControls
{
    // ReSharper disable once RedundantExtendsListEntry
    public sealed partial class TitleBarControl : UserControl
    {
        public TitleBarControl()
        {
            InitializeComponent();

            // 设置TitleBar
            Window.Current.SetTitleBar(TitleBar);
            // 显示软件名
            TitleText.Text = Package.Current.DisplayName;
            BackButton.Click += (sender, args) =>
            {
                var rootFrame = Window.Current.Content as Frame;
                rootFrame?.GoBack();
            };
        }

        /// <summary>
        ///     设置是否显示返回按钮
        /// </summary>
        /// <param name="visibility">是否显示</param>
        public void SetBackButtonVisibility(Visibility visibility)
        {
            BackButton.Visibility = visibility;
        }
    }
}