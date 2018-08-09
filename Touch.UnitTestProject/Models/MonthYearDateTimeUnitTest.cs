using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Touch.Models;

namespace Touch.UnitTestProject.Models
{
    [TestClass]
    public class MonthYearDateTimeUnitTest
    {
        [TestMethod]
        public void OrderByTest()
        {
            var list = new List<MonthYearDateTime>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    DateTime time = new DateTime(2015 + i, 7 + j, 1);
                    MonthYearDateTime monthYear = new MonthYearDateTime(time);
                    list.Add(monthYear);
                }
            }
            list.OrderBy(m => m);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    DateTime time = new DateTime(2015 + i, 7 + j, 1);
                    MonthYearDateTime monthYear = new MonthYearDateTime(time);
                    Assert.AreEqual(monthYear, list[i * 4 + j]);
                }
            }
        }

        [TestMethod]
        public void OrderByDescendingTest()
        {
            var list = new List<MonthYearDateTime>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    DateTime time = new DateTime(2015 + i, 7 + j, 1);
                    MonthYearDateTime monthYear = new MonthYearDateTime(time);
                    list.Add(monthYear);
                }
            }
            list = list.OrderByDescending(m => m.WholeDateTime.Year).ThenByDescending(m => m.WholeDateTime.Month).ToList();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    DateTime time = new DateTime(2017 - i, 10 - j, 1);
                    MonthYearDateTime monthYear = new MonthYearDateTime(time);
                    Assert.AreEqual(monthYear, list[i * 4 + j]);
                }
            }
        }
    }
}
