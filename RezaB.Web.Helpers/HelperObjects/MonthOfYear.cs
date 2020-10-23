using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RezaB.Web.Helpers.HelperObjects
{
    [ModelBinder(typeof(Binders.MonthOfYearBinder))]
    public class MonthOfYear
    {
        public int? Year { get; set; }

        public int? Month { get; set; }

        public bool IsValid
        {
            get
            {
                return Year.HasValue && Month.HasValue && Year >= 1753 && Month > 0 && Month <= 12;
            }
        }

        public string MonthName
        {
            get
            {
                return Month.HasValue ? DateTimeFormatInfo.GetInstance(Thread.CurrentThread.CurrentCulture).GetMonthName(Month.Value) : null;
            }
        }

        public override string ToString()
        {
            return IsValid ? Year + "-" + Month : null;
        }
    }
}
