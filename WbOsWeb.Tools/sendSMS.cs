using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace WbOsWeb.Tools
{
    /********************************************************************************************************/
    //全文模板短信数据发送
    /********************************************************************************************************/
    public class sendSMS {
      
        //代理审核时候短信推送
        public void Examineedsend(string mobile, string value)
        {
            sendSMS SendSms = new sendSMS();
            string sendurl = "http://api.yunduanxin.com/sms/";
            // string mobile = "15982165484,13402818480";  //发送号码
            StringBuilder strTemp = new StringBuilder();
            string code = "news";
            // string value = "我在测试云短信通知，看看能不能收到短信";
            strTemp.Append("{\"" + code + "\":\"" + value + "\"}");

            string strContent = strTemp.ToString();
            StringBuilder sbTemp = new StringBuilder();
            string uid = "weilai2018";
            string pwd = "weilai2018";
            string Pass = FormsAuthentication.HashPasswordForStoringInConfigFile(pwd + uid, "MD5"); //密码进行MD5加密                                                                                                  
            sbTemp.Append("?ac=send&uid=" + uid + "&pwd=" + Pass + "&template=503770&ignore=1&mobile=" + mobile + "&content=" + strContent);
            byte[] bTemp = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(sbTemp.ToString());
            String postReturn = doPostRequest(sendurl, bTemp);

            WriteTxt("Post response is: " + postReturn);  //测试返回结果
        }

        public void Groupsend(string mobile, string value)
        {
            sendSMS SendSms = new sendSMS();
            string sendurl = "http://api.yunduanxin.com/sms/";
            // string mobile = "15982165484,13402818480";  //发送号码
            StringBuilder strTemp = new StringBuilder();
            string code = "news";
            // string value = "我在测试云短信通知，看看能不能收到短信";
            strTemp.Append("{\"" + code + "\":\"" + value + "\"}");

            string strContent = strTemp.ToString();
            StringBuilder sbTemp = new StringBuilder();
            string uid = "weilai2018";
            string pwd = "weilai2018";
            string Pass = FormsAuthentication.HashPasswordForStoringInConfigFile(pwd + uid, "MD5"); //密码进行MD5加密                                                                                                  
            sbTemp.Append("?ac=send&uid=" + uid + "&pwd=" + Pass + "&template=503770&ignore=1&mobile=" + mobile + "&content=" + strContent);
            byte[] bTemp = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(sbTemp.ToString());
            String postReturn = doPostRequest(sendurl, bTemp);

            WriteTxt("Post response is: " + postReturn);  //测试返回结果
        }
        public void OrdersNoExamineSend(string mobile, string value)
        {
            sendSMS SendSms = new sendSMS();
            string sendurl = "http://api.yunduanxin.com/sms/";

            StringBuilder strTemp = new StringBuilder();
            string code = "new";

            strTemp.Append("{\"" + code + "\":\"" + value + "\"}");

            string strContent = strTemp.ToString();
            StringBuilder sbTemp = new StringBuilder();
            string uid = "weilai2018";
            string pwd = "weilai2018";
            string Pass = FormsAuthentication.HashPasswordForStoringInConfigFile(pwd + uid, "MD5"); //密码进行MD5加密                                                                                                    
            sbTemp.Append("?ac=send&uid=" + uid + "&pwd=" + Pass + "&template=503891&ignore=1&mobile=" + mobile + "&content=" + strContent);
            byte[] bTemp = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(sbTemp.ToString());
            String postReturn = doPostRequest(sendurl, bTemp);

            SendSms.WriteTxt("Post response is: " + postReturn);  //测试返回结果
        }
        //验证手机号码合法性
        public bool IsHandset(string str_handset)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_handset, @"^[1]+[3,4,5,6,7,8,9]+\d{9}");
        }

        //过滤特殊字符
        public  string RemoveSpecialCharacterToupper(string hexData)
        {
            //下文中的‘\\’表示转义
            return Regex.Replace(hexData, "[ \\[ \\] \\^ \\-_*×――(^)|'$%~!@#$…&%￥—+=<>《》!！??？:：•`·、。，；,.;\"‘’“”-]", "").ToUpper();
        }

        //POST方式发送得结果
        private static String doPostRequest(string url, byte[] bData)
        {
            System.Net.HttpWebRequest hwRequest;
            System.Net.HttpWebResponse hwResponse;

            string strResult = string.Empty;
            try
            {
                hwRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                hwRequest.Timeout = 5000;
                hwRequest.Method = "POST";
                hwRequest.ContentType = "application/x-www-form-urlencoded";
                hwRequest.ContentLength = bData.Length;

                System.IO.Stream smWrite = hwRequest.GetRequestStream();
                smWrite.Write(bData, 0, bData.Length);
                smWrite.Close();
            }
            catch (System.Exception err)
            {
                WriteErrLog(err.ToString());
                return strResult;
            }

            //get response
            try
            {
                hwResponse = (HttpWebResponse)hwRequest.GetResponse();
                StreamReader srReader = new StreamReader(hwResponse.GetResponseStream(), Encoding.ASCII);
                strResult = srReader.ReadToEnd();
                srReader.Close();
                hwResponse.Close();
            }
            catch (System.Exception err)
            {
                WriteErrLog(err.ToString());
            }
            return strResult;
        }
        private static void WriteErrLog(string strErr)
        {
            Console.WriteLine(strErr);
            System.Diagnostics.Trace.WriteLine(strErr);
        }
        public void WriteTxt(string str)
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
       
    }
}
