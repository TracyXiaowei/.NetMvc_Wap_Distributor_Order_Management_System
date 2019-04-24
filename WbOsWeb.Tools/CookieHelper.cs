using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace WbOsWeb.Tools
{
    public class CookieHelper
    {
        /// <summary>
        /// Remove the specified Cookie
        /// </summary>
        /// <param name="cookiename">The cookie name</param>
        public static void ClearCookie(string cookiename)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookiename];

            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddYears(-3);

                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        /// <summary>
        /// Access to the specified Cookie value
        /// </summary>
        /// <param name="cookiename">The cookie name</param>
        /// <returns></returns>
        public static string GetCookieValue(string cookiename)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookiename];

            string str = string.Empty;

            if (cookie != null)
            {
                str = cookie.Value;
            }
            return str;
        }

        /// <summary>
        /// Add a Cookie (7days)
        /// </summary>
        /// <param name="cookiename">The cookie name</param>
        /// <param name="cookievalue">The value of cookie</param>
        public static void SetCookie(string cookiename, string cookievalue)
        {
            SetCookie(cookiename, cookievalue, DateTime.Now.AddDays(1.0));
        }

        /// <summary>
        /// Add a Cookie
        /// </summary>
        /// <param name="cookiename">Cookie name</param>
        /// <param name="cookievalue">The value of cookie</param>
        /// <param name="expires">Expiration time DateTime</param>
        public static void SetCookie(string cookiename, string cookievalue, DateTime expires)
        {
            HttpCookie cookie = new HttpCookie(cookiename)
            {
                Value = cookievalue,

                Expires = expires
            };
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}
