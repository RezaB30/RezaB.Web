using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
using System.Web;
using System.Web.Caching;

namespace RezaB.Web.Utilities
{
    /// <summary>
    /// Manages settings for application.
    /// </summary>
    public static class AppSettings
    {

        private static Type _entities;
        private static Type _tableType;
        private static string _keyName;
        private static string _valueName;

        private static ObjectCache memoryCache = MemoryCache.Default;

        public static void _setContext(Type entitiesType, Type settingsTableType, string keyName = "Key", string valueName = "Value")
        {
            _entities = entitiesType;
            _tableType = settingsTableType;
            _keyName = keyName;
            _valueName = valueName;
        }

        public static string Get(string key)
        {
            var current = memoryCache.Get(key);
            if (current != null)
                return current.ToString();
            using (var entities = Activator.CreateInstance(_entities) as DbContext)
            {
                current = entities.Set(_tableType).Find(key);
                var result = (string)current.GetType().GetProperty(_valueName).GetValue(current);
                memoryCache.Add(key, result, new CacheItemPolicy()
                {
                    AbsoluteExpiration = DateTime.UtcNow.AddDays(1),
                    SlidingExpiration = ObjectCache.NoSlidingExpiration
                });
                return result;
            }
        }

        public static void Set(string key, string value)
        {
            using (var entities = Activator.CreateInstance(_entities) as DbContext)
            {
                var current = entities.Set(_tableType).Find(key);
                current.GetType().GetProperty(_valueName).SetValue(current, value);
                entities.Entry(current).State = EntityState.Modified;
                entities.SaveChanges();

                memoryCache.Set(key, value, new CacheItemPolicy()
                {
                    AbsoluteExpiration = DateTime.UtcNow.AddDays(1),
                    SlidingExpiration = ObjectCache.NoSlidingExpiration
                });
            }
        }

        public static void Update(Dictionary<string, string> values)
        {
            using (var entities = Activator.CreateInstance(_entities) as DbContext)
            {
                var dbItems = entities.Set(_tableType).Find(values.Keys.ToArray()) as IEnumerable<object>;
                foreach (var item in dbItems)
                {
                    var key = item.GetType().GetProperty(_keyName).GetValue(item).ToString();
                    item.GetType().GetProperty(_valueName).SetValue(item, values[key]);
                    entities.Entry(item).State = EntityState.Modified;
                    memoryCache.Set(key, values[key], new CacheItemPolicy()
                    {
                        AbsoluteExpiration = DateTime.UtcNow.AddDays(1),
                        SlidingExpiration = ObjectCache.NoSlidingExpiration
                    });
                }

                entities.SaveChanges();
            }
        }
    }
}
