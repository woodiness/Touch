using System;
using Windows.Globalization.DateTimeFormatting;

// ReSharper disable EqualExpressionComparison

namespace Touch.Models
{
    /// <summary>
    ///     只有年和月的日期
    /// </summary>
    public class MonthYearDateTime : IComparable<MonthYearDateTime>
    {
        private readonly string _monthYearDate;
        public DateTime WholeDateTime;

        public MonthYearDateTime(DateTime dateTime)
        {
            WholeDateTime = dateTime;
            _monthYearDate = new DateTimeFormatter("month year").Format(dateTime);
        }

        public int CompareTo(MonthYearDateTime other)
        {
            if (WholeDateTime.Year < other.WholeDateTime.Year)
                return -1;
            if (WholeDateTime.Year > WholeDateTime.Year)
                return 1;
            if (WholeDateTime.Month < other.WholeDateTime.Month)
                return -1;
            return WholeDateTime.Month > WholeDateTime.Month ? 1 : 0;
        }

        public override bool Equals(object obj)
        {
            var o = obj as MonthYearDateTime;
            return o != null && o._monthYearDate == _monthYearDate;
        }

        public override string ToString()
        {
            return _monthYearDate;
        }

        public override int GetHashCode()
        {
            return _monthYearDate.GetHashCode();
        }
    }
}