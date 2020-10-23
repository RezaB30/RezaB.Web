using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezaB.Web.Helpers.ColorPallete.ColorPallete
{
    public static class ColorsElements
    {
        public static KeyValuePair<string, string>[] GetSortedEntries()
        {
            return Colors.ResourceManager.GetResourceSet(CultureInfo.CreateSpecificCulture("en-US"), true, true).Cast<DictionaryEntry>().ToDictionary(e => e.Key.ToString(), e => e.Value.ToString()).OrderBy(e => e.Key).ToArray();
        }
    }
}
