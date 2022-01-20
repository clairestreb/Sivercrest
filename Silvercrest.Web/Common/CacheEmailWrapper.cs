using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace Silvercrest.Web.Common
{
    public class CacheEmailWrapper
    {
        //private static DateTime _Expiration = DateTime.Now.AddHours(12);
        //private DateTime _Expiration = DateTime.Now.AddMinutes(10);

        public static void Insert(string key, object objectToCache)
        {
            var _Expiration = DateTime.Now.AddMinutes(10);
            if (objectToCache != null && key != null)
            {
                HttpRuntime.Cache.Add(key, objectToCache, null, _Expiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
            }
        }

        public static void Insert(string key, object objectToCache, double cachingTime)
        {
            if (objectToCache != null && key != null)
            {
                HttpRuntime.Cache.Add(key, objectToCache, null, DateTime.Now.AddHours(cachingTime), Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
            }
        }

        public static object Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            return HttpRuntime.Cache.Get(key);
        }

        public static void Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }
            HttpRuntime.Cache.Remove(key);
        }
    }
}