using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace OS.Web
{
    public static class CookieHelper
    {

        public static object GetCookieValue(HttpCookieCollection cookies, string cookieName, string valueName, out string message)
        {
            message = "ok";
            StringBuilder sb = new StringBuilder("OK");
            HttpCookie myCookie = null;

            // GET COOKIE
            myCookie = cookies[cookieName];
            if (myCookie == null)
            {
                sb.Clear();
                sb.AppendFormat("Cookie with name '{0}' does not exist", cookieName);
                message = sb.ToString();
                return null;
            }

            object cookieValue = myCookie.Values[valueName];
            if(cookieValue == null)
            {
                sb.Clear();
                sb.AppendFormat("Cookie value {0} does not exist", valueName);
                message = sb.ToString();
                return null;
            }


            message = sb.ToString();
            return cookieValue;

        }
    }
}