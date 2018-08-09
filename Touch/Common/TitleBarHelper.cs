using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Microsoft.Toolkit.Uwp;

namespace Touch.Common
{
    /// <summary>
    ///     TitleBar设置
    /// </summary>
    public static class TitleBarHelper
    {
        /// <summary>
        ///     设置透明TitleBar
        /// </summary>
        public static void SetTransparentTitleBar()
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = Colors.Transparent;
            titleBar.InactiveBackgroundColor = Colors.Transparent;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonHoverBackgroundColor = "#35000000".ToColor();
            titleBar.ButtonHoverForegroundColor = Colors.Black;
            titleBar.ButtonPressedBackgroundColor = "#64000000".ToColor();
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveForegroundColor = Colors.Black;
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
        }
    }
}