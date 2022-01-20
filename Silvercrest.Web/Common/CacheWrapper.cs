using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace Silvercrest.Web.Common
{
    public static class CacheWrapper
    {
        private static double _CacheDuration = 15.0; // 15 minutes of Cache since Last Used
        private static DateTime _LastRefresh = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static Dictionary<string, object> ObjectCache = new Dictionary<string, object>();

        public static void Insert(string key, object objectToCache)
        {
            if (objectToCache != null && key != null)
            {
                ObjectCache.Add(key, objectToCache);
                _LastRefresh = DateTime.UtcNow;
            }
        }

        public static object Get(string key)
        {
            object cache = new object();
            if (string.IsNullOrEmpty(key)) { return null; }

            //If Cached Object Stale, Force Refresh
            if(_LastRefresh.AddMinutes(_CacheDuration) < DateTime.UtcNow)
            {
                Remove(key);
                return null;
            }

            //Every time you get the object, it is assumed to be okay, so we dont need to refresh
            _LastRefresh = DateTime.UtcNow;
            
            cache = ObjectCache[key];
            return cache != null ? cache : null;
            //return ObjectCache[key];
        }

        public static void Remove(string key)
        {
            if (string.IsNullOrEmpty(key)) { return; }
            ObjectCache.Remove(key);
        }
    }
}