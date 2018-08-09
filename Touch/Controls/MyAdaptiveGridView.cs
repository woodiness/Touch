using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// ReSharper disable CompareOfFloatsByEqualityOperator

namespace Touch.Controls
{
    /// <summary>
    ///     根据宽度自适应GridView
    /// </summary>
    public class MyAdaptiveGridView : GridView
    {
        public MyAdaptiveGridView()
        {
            if (ItemContainerStyle == null)
                ItemContainerStyle = new Style(typeof(GridViewItem));

            ItemContainerStyle.Setters.Add(new Setter(HorizontalContentAlignmentProperty, HorizontalAlignment.Stretch));

            Loaded += (s, a) =>
            {
                if (ItemsPanelRoot != null)
                    InvalidateMeasure();
            };
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var panel = ItemsPanelRoot as ItemsWrapGrid;
            if (panel == null)
                return base.MeasureOverride(availableSize);
            if (MinItemWidth == 0)
                throw new DivideByZeroException("You need to have a MinItemWidth greater than zero");

            var availableWidth = availableSize.Width - (Padding.Right + Padding.Left);

            var numColumns = Math.Floor(availableWidth / MinItemWidth);
            numColumns = numColumns == 0 ? 1 : numColumns;
            if (Items != null)
            {
                // ReSharper disable once UnusedVariable
                var numRows = Math.Ceiling(Items.Count / numColumns);
            }

            var itemWidth = availableWidth / numColumns;
            var aspectRatio = MinItemHeight / MinItemWidth;
            var itemHeight = itemWidth * aspectRatio;

            panel.ItemWidth = itemWidth;
            panel.ItemHeight = itemHeight;

            return base.MeasureOverride(availableSize);
        }

        #region DependencyProperties

        /// <summary>
        ///     Minimum height for item
        /// </summary>
        public double MinItemHeight
        {
            get { return (double) GetValue(MinItemHeightProperty); }
            set { SetValue(MinItemHeightProperty, value); }
        }

        public static readonly DependencyProperty MinItemHeightProperty =
            DependencyProperty.Register(
                "MinItemHeight",
                typeof(double),
                typeof(MyAdaptiveGridView),
                new PropertyMetadata(1.0, (s, a) =>
                {
                    if (!double.IsNaN((double) a.NewValue))
                        ((MyAdaptiveGridView) s).InvalidateMeasure();
                }));

        /// <summary>
        ///     Minimum width for item (must be greater than zero)
        /// </summary>
        public double MinItemWidth
        {
            get { return (double) GetValue(MinimumItemWidthProperty); }
            set { SetValue(MinimumItemWidthProperty, value); }
        }

        public static readonly DependencyProperty MinimumItemWidthProperty =
            DependencyProperty.Register(
                "MinItemWidth",
                typeof(double),
                typeof(MyAdaptiveGridView),
                new PropertyMetadata(1.0, (s, a) =>
                {
                    if (!double.IsNaN((double) a.NewValue))
                        ((MyAdaptiveGridView) s).InvalidateMeasure();
                }));

        #endregion
    }
}