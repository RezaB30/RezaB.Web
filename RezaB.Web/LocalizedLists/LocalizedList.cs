//using System;
//using System.Resources;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Globalization;

//namespace RezaB.Web.LocalizedLists
//{
//    /// <summary>
//    /// Creates a localized enum with a resource.
//    /// </summary>
//    /// <typeparam name="TEnum">Type of enum</typeparam>
//    /// <typeparam name="TResource">Type of resource associated with the enum</typeparam>

//    public class LocalizedList<TEnum, TResource> : LocalizedList where TEnum : IComparable, IConvertible, IFormattable
//    {
//        private Dictionary<int, string> ListData
//        {
//            get
//            {
//                var list = new Dictionary<int, string>();
//                foreach (var item in Enum.GetValues(typeof(TEnum)))
//                {
//                    list.Add((int)item, item.ToString());
//                }
//                return list;
//            }
//        }

//        public override Dictionary<int, string> GetList(CultureInfo culture = null)
//        {
//            culture = culture ?? Thread.CurrentThread.CurrentCulture;
//            var source = ListData;
//            var list = new Dictionary<int, string>();
//            var rm = new ResourceManager(typeof(TResource));
//            foreach (var item in source)
//            {
//                list.Add(item.Key, rm.GetString(item.Value, culture));
//            }

//            return list;
//        }

//        public override string GetDisplayText(int? value, CultureInfo cultur = null)
//        {
//            if (value.HasValue)
//            {
//                var list = GetList(cultur);
//                string returnValue;
//                if (list.TryGetValue(value.Value, out returnValue))
//                    return returnValue;
//            }
//            return "-";
//        }
//    }

//    public abstract class LocalizedList
//    {
//        public abstract Dictionary<int, string> GetList(CultureInfo culture = null);

//        public abstract string GetDisplayText(int? value, CultureInfo culture = null);

//        public ListItem[] GenericList
//        {
//            get
//            {
//                return GetList().Select(l => new ListItem() { ID = l.Key, Name = l.Value }).ToArray();
//            }
//        }

//        public class ListItem
//        {
//            public int ID { get; set; }

//            public string Name { get; set; }
//        }
//    }
//}
