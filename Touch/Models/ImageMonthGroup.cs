using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Touch.ViewModels;

namespace Touch.Models
{
    /// <summary>
    ///     图片按月份分组
    /// </summary>
    public class ImageMonthGroup : IGrouping<MonthYearDateTime, ImageModel>
    {
        private readonly ObservableCollection<ImageModel> _imageViewModels;

        public ImageMonthGroup(MonthYearDateTime key, IEnumerable<ImageModel> items)
        {
            Key = key;
            _imageViewModels = new ObservableCollection<ImageModel>(items);
        }

        public MonthYearDateTime Key { get; }

        public IEnumerator<ImageModel> GetEnumerator()
        {
            return _imageViewModels.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _imageViewModels.GetEnumerator();
        }
    }
}
