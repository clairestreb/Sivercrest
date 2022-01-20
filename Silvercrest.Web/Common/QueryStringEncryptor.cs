using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Text;
using System.Reflection;

namespace Silvercrest.Web.Common
{
    public class QueryStringEncryptor
    {
        public static void base64Encode(System.Web.HttpRequestBase Request)
        {
            NameValueCollection queryString = Request.QueryString;
            foreach(var k in queryString.AllKeys)
            {
                PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                isreadonly.SetValue(Request.QueryString, false, null);
                Request.QueryString.Set(k, System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(queryString[k])));
                isreadonly.SetValue(Request.QueryString, true, null);
            }
        }

    }
}