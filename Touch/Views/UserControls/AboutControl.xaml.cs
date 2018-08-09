using System;
using Windows.ApplicationModel;
using Windows.System;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Touch.Views.UserControls
{
    public sealed partial class AboutControl : UserControl
    {
        public AboutControl()
        {
            InitializeComponent();
            var package = Package.Current;
            var name = package.DisplayName;
            var version = package.Id.Version;
            AppInfoText.Text = name + " " + $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            SendFeedbackButton.Click += async (sender, args) =>
            {
                const string uriToLaunch = @"https://github.com/zhangyin-github/Touch/issues";
                var uri = new Uri(uriToLaunch);
                await Launcher.LaunchUriAsync(uri);
            };
        }
    }
}