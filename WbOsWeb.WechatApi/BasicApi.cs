using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace WbOsWeb.WechatApi
{

    /// <summary>
    /// 基础接口
    /// </summary>
    public class BasicApi
    {


        public static string SessionAccessToken = "";//access_token缓存 其他接口的通行证

        public BasicApi() { }

        #region 获取access_token缓存
        public static string GetTokenSession(string AppID, string AppSecret)
        {
            string TokenSession = "";

            if (System.Web.HttpContext.Current.Session[SessionAccessToken] == null)
            {
                TokenSession = AddTokenSession(AppID, AppSecret);
            }
            else
            {
                TokenSession = System.Web.HttpContext.Current.Session[SessionAccessToken].ToString();
            }

            return TokenSession;
        }


        /// <summary>
        /// 检查签名是否正确:
        /// http://mp.weixin.qq.com/wiki/index.php?title=%E6%8E%A5%E5%85%A5%E6%8C%87%E5%8D%97
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="token">AccessToken</param>
        /// <returns>
        /// true: check signature success
        /// false: check failed, 非微信官方调用!
        /// </returns>
        public static bool CheckSignature(string signature, string timestamp, string nonce, string token, out string ent)
        {
            var arr = new[] { token, timestamp, nonce }.OrderBy(z => z).ToArray();
            var arrString = string.Join("", arr);
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var sha1Arr = sha1.ComputeHash(Encoding.UTF8.GetBytes(arrString));
            StringBuilder enText = new StringBuilder();
            foreach (var b in sha1Arr)
            {
                enText.AppendFormat("{0:x2}", b);
            }
            ent = enText.ToString();
            return signature == enText.ToString();
        }

        /// <summary>
        /// 添加AccessToken缓存
        /// </summary>
        /// <param name="AppID"></param>
        /// <param name="AppSecret"></param>
        /// <returns></returns>
        public static string AddTokenSession(string AppID, string AppSecret)
        {
            //获取AccessToken
            string AccessToken = GetAccessToken(AppID, AppSecret);
            HttpContext.Current.Session[SessionAccessToken] = AccessToken;
            HttpContext.Current.Session.Timeout = 7200;
            return AccessToken;
        }

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="AppID"></param>
        /// <param name="AppSecret"></param>
        /// <returns></returns>
        public static string GetAccessToken(string AppID, string AppSecret)
        {
            JavaScriptSerializer Jss = new JavaScriptSerializer();
            string respText = CommonMethod.WebRequestPostOrGet(string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", AppID, AppSecret), "");
            Dictionary<string, object> respDic = (Dictionary<string, object>)Jss.DeserializeObject(respText);
            string accessToken = respDic["access_token"].ToString();
            return accessToken;
        }
        #endregion
    }
}

