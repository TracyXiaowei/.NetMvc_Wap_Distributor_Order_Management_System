using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace WbOsWeb.WechatApi
{
    public class CommonMethod
    {
        #region Post/Get提交调用抓取
        /// <summary>
        /// Post/get 提交调用抓取
        /// </summary>
        /// <param name="url">提交地址</param>
        /// <param name="param">参数</param>
        /// <returns>string</returns>
        public static string WebRequestPostOrGet(string posturl, string postData)
        {


            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Encoding encoding = Encoding.UTF8;
            byte[] data = null;
            if (postData.Length > 0) //有值代表创建菜单
            {
                data = encoding.GetBytes(postData);
            }

            // 准备请求...
            try
            {
                // 设置参数
                request = WebRequest.Create(posturl) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                if (postData.Length > 0)
                {
                    request.Method = "POST"; //创建菜单
                }
                else
                {
                    request.Method = "GET"; //删除菜单
                }

                request.ContentType = "application/x-www-form-urlencoded";

                if (postData.Length > 0) //有值代表创建菜单
                {
                    request.ContentLength = data.Length;
                    outstream = request.GetRequestStream();
                    outstream.Write(data, 0, data.Length);
                    outstream.Close();
                }

                //发送请求并获取相应回应数据
                response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, encoding);
                //返回结果网页（html）代码
                string content = sr.ReadToEnd();
                string err = string.Empty;
                return content;
            }
            catch (Exception ex)
            {
                return ex.Message;//url错误时候回报错
            }
        }


        private void CreateMenu(string MenuJson, string AppID, string AppSecret)
        {
            string access_token = BasicApi.GetTokenSession(AppID, AppSecret);
            string i = GetPage("https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + access_token, MenuJson);

        }


    

        /// <summary>
        /// 获取自定义菜单
        /// </summary>
        /// <param name="posturl">自定义菜单请求的地址</param>
        /// <param name="postData">自定义菜单内容</param>
        /// <returns></returns>
        private string GetPage(string posturl, string postData)
        {
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Encoding encoding = Encoding.UTF8;
            byte[] data = null;
            if (postData.Length > 0) //有值代表创建菜单
            {
                data = encoding.GetBytes(postData);
            }

            // 准备请求...
            try
            {
                // 设置参数
                request = WebRequest.Create(posturl) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                if (postData.Length > 0)
                {
                    request.Method = "POST"; //创建菜单
                }
                else
                {
                    request.Method = "GET"; //删除菜单
                }

                request.ContentType = "application/x-www-form-urlencoded";

                if (postData.Length > 0) //有值代表创建菜单
                {
                    request.ContentLength = data.Length;
                    outstream = request.GetRequestStream();
                    outstream.Write(data, 0, data.Length);
                    outstream.Close();
                }

                //发送请求并获取相应回应数据
                response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, encoding);
                //返回结果网页（html）代码
                string content = sr.ReadToEnd();
                string err = string.Empty;
                return content;
            }
            catch (Exception ex)
            {
                string err = ex.Message;

                return string.Empty;
            }
        }





        #endregion Post/Get提交调用抓取

        #region unix/datatime 时间转换
        /// <summary>
        /// unix时间转换为datetime
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime UnixTimeToTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>
        /// datetime转换为unixtime
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }
        #endregion

        #region 记录bug，以便调试
        /// <summary>
        /// 记录bug，以便调试
        /// </summary>
        public static bool WriteTxt(string str)
        {
            try
            {
                string LogPath = HttpContext.Current.Server.MapPath("/err_log/");
                if (!Directory.Exists(LogPath))
                {
                    Directory.CreateDirectory(LogPath);
                }
                FileStream FileStream = new FileStream(System.Web.HttpContext.Current.Server.MapPath("/err_log//weilai_" + DateTime.Now.ToLongDateString() + "_.txt"), FileMode.Append);
                StreamWriter StreamWriter = new StreamWriter(FileStream);
                //开始写入
                StreamWriter.WriteLine(str);
                //清空缓冲区
                StreamWriter.Flush();
                //关闭流
                StreamWriter.Close();
                FileStream.Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region 生成随机字符
        /// <summary>
        /// 生成随机字符
        /// </summary>
        /// <param name="iLength">生成字符串的长度</param>
        /// <returns>返回随机字符串</returns>
        public static string GetRandCode(int iLength)
        {
            string sCode = "";
            if (iLength == 0)
            {
                iLength = 4;
            }
            string codeSerial = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            string[] arr = codeSerial.Split(',');
            int randValue = -1;
            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < iLength; i++)
            {
                randValue = rand.Next(0, arr.Length - 1);
                sCode += arr[randValue];
            }
            return sCode;
        }
        #endregion

        #region 根据ip获取地点
        /// 获取Ip归属地
        /// </summary>
        /// <param name="ip">ip</param>
        /// <returns>归属地</returns>
        public static string GetIpAddress(string ip)
        {
            JavaScriptSerializer Jss = new JavaScriptSerializer();
            //http://int.dpool.sina.com.cn/iplookup/iplookup.php?format=js&ip=218.192.3.42 调用新浪的接口
            string address = string.Empty;
            try
            {
                string reText = WebRequestPostOrGet("http://int.dpool.sina.com.cn/iplookup/iplookup.php?format=js&ip=" + ip, "");
                Dictionary<string, object> DicText = (Dictionary<string, object>)Jss.DeserializeObject(reText);
                address = DicText["city"].ToString();
                WriteTxt("city:" + address);
            }
            catch { }
            return address;
        }
        #endregion

        //***************

        #region 请求Url，不发送数据
        /// <summary>
        /// 请求Url，不发送数据
        /// </summary>
        public static string RequestUrl(string url)
        {
            return RequestUrl(url, "POST");
        }
        #endregion

        #region 请求Url，不发送数据
        /// <summary>
        /// 请求Url，不发送数据
        /// </summary>
        public static string RequestUrl(string url, string method)
        {
            // 设置参数
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            request.Method = method;
            request.ContentType = "text/html";
            request.Headers.Add("charset", "utf-8");

            //发送请求并获取相应回应数据
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream, Encoding.UTF8);
            //返回结果网页（html）代码
            string content = sr.ReadToEnd();
            return content;
        }
        #endregion

        #region 请求Url，发送数据
        /// <summary>
        /// 请求Url，发送数据
        /// </summary>
        public static string PostUrl(string url, string postData)
        {
            byte[] data = Encoding.UTF8.GetBytes(postData);

            // 设置参数
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            Stream outstream = request.GetRequestStream();
            outstream.Write(data, 0, data.Length);
            outstream.Close();

            //发送请求并获取相应回应数据
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream instream = response.GetResponseStream();
            StreamReader sr = new StreamReader(instream, Encoding.UTF8);
            //返回结果网页（html）代码
            string content = sr.ReadToEnd();
            return content;
        }
        #endregion

        /// <summary>
        /// 获取Json字符串某节点的值
        /// </summary>
        public static string GetJsonValue(string jsonStr, string key)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(jsonStr))
            {
                key = "\"" + key.Trim('"') + "\"";
                int index = jsonStr.IndexOf(key) + key.Length + 1;
                if (index > key.Length + 1)
                {
                    //先截逗号，若是最后一个，截“｝”号，取最小值
                    int end = jsonStr.IndexOf(',', index);
                    if (end == -1)
                    {
                        end = jsonStr.IndexOf('}', index);
                    }

                    result = jsonStr.Substring(index, end - index);
                    result = result.Trim(new char[] { '"', ' ', '\'' }); //过滤引号或空格
                }
            }

            return result;

        }

        //********************



    }
}
