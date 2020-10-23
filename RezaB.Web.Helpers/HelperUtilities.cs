using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RezaB.Web.Helpers
{
    public static class HelperUtilities
    {
        /// <summary>
        /// Creates a simple numeric selectlist.
        /// </summary>
        /// <param name="minValue">Starting value to count.</param>
        /// <param name="maxValue">End value to count to.</param>
        /// <param name="selectedValue">Selected value of the list.</param>
        /// <returns>A SelectList to use in dropdowns.</returns>
        public static SelectList CreateNumericSelectList(int minValue, int maxValue, int? selectedValue = null)
        {
            if (minValue > maxValue)
                return null;

            List<ListOption> options = new List<ListOption>();
            while (minValue <= maxValue)
            {
                options.Add(new ListOption(minValue));
                minValue++;
            }

            if (selectedValue.HasValue)
                return new SelectList(options, "Value", "Value", selectedValue.Value);

            return new SelectList(options, "Value", "Value");
        }

        private class ListOption
        {
            public int Value { get; set; }

            public ListOption(int value) { Value = value; }
        }

        public static void MergeAdditionalAttributes(this TagBuilder tag, object htmlAttributes)
        {
            var type = htmlAttributes.GetType();
            var properties = type.GetProperties();
            foreach (var item in properties)
            {
                tag.MergeAttribute(item.Name, item.GetValue(htmlAttributes).ToString());
            }
        }
    }
}