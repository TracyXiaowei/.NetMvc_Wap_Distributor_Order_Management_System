using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WbOsWeb.Tools
{
  public  class PhoneType
    {
        public  string CheckAgent()
        {
            string PhoneType = "";
            string agent = HttpContext.Current.Request.UserAgent;
            string[] keywords = { "Android", "iPhone", "iPod", "iPad", "Windows Phone", "MQQBrowser" };

            //排除Window 桌面系统 和 苹果桌面系统 
            if (!agent.Contains("Windows NT") && !agent.Contains("Macintosh"))
            {
                foreach (string item in keywords)
                {
                    if (agent.Contains(item))
                    {
                        PhoneType = item;
                        break;
                    }
                }
            }
            return PhoneType;
        }
    }
}
