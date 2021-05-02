using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezaB.Web.Helpers.DataTypes
{
    public class DateWithTime
    {
        public DateTime? InternalValue { get; set; }

        public static implicit operator DateWithTime(DateTime? value)
        {
            return new DateWithTime() { InternalValue = value };
        }

        public static implicit operator DateTime?(DateWithTime value)
        {
            return value.InternalValue;
        }

        public override string ToString()
        {
            return InternalValue?.ToString();
        }

        public string ToString(string format)
        {
            return InternalValue?.ToString(format);
        }
    }
}
