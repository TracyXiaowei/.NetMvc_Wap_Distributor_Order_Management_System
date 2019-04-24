using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WbOsWeb.Bll;
using WbOsWeb.Entity;

namespace WbOsWeb.WechatApi
{
    public class ResponseMessage
    {
        #region 接收的类型
        public static string GetText(string FromUserName, string ToUserName, string Content,string EventKey)
        {
           // CommonMethod.WriteTxt(Content);//接收的文本消息
          //  CommonMethod.WriteTxt(EventKey);//接收的文本消息
            //关键字回复

            string XML = "";
        

            switch (Content)
            {
                case "subscribe":
                    XML = Followreply(FromUserName, ToUserName);
                    break;
                case "CLICK":

                    XML = Clickreply(FromUserName, ToUserName, EventKey);
                    break;
                default:
                   // XML = ReText(FromUserName, ToUserName, EventKey);
                   XML = ReText(FromUserName, ToUserName, "欢迎关注成都薇莱商贸！");
                    break;
            }


            return XML;
        }
        //群发消息 BasicApi.GetTokenSession(AppID, AppSecret);
        public static List<string> SentGoup(string AppID, string AppSecret)
        {
            string access_token=   BasicApi.GetTokenSession(AppID, AppSecret);
            List<string> openidList= OAuth2.GetOpenIDs(access_token);
            return openidList;
        }

        public static string KeywordsreplyGetText(string FromUserName, string ToUserName, string Content, string EventKey)
        {
            CommonMethod.WriteTxt(Content);

            string XML = "";
       
                XML = Keywordsreply(FromUserName, ToUserName, Content);      
            return XML;
        }


    

        //自动回复
        public static string Automaticreply(string FromUserName, string ToUserName)
        {
            string XML = "";
            WechatMessageBll bll = new WechatMessageBll();
            int Where = 3;//1、关注回复 2、关键词回复  3、自动回复
            WechatMessageEntity entity = bll.selectByReply_Type(Where);

   
         
            if (entity!=null)
            {

        

                //1.判断消息类型
                int Message_Type = entity.Message_Type;   //1、图文  2、文字
                if (Message_Type == 1)
                {
                    //1、 判断是多图文还是单图文
                    WechatMessage_ImageBll imageBll = new WechatMessage_ImageBll();
                    string pidwhere = "Pid=" + entity.WechatMessageId;
                    List<WechatMessage_ImageEntity> list = new List<WechatMessage_ImageEntity>();
                    WechatMessage_ImageEntity one = imageBll.selectById(entity.WechatMessageId);

                    list = imageBll.selectByWhere(pidwhere);

                    List<WechatMessage_ImageEntity> arlist = new List<WechatMessage_ImageEntity>();
                    arlist.Add(one);
                    foreach (var item in list)
                    {
                        arlist.Add(item);
                    }
                    int Num = arlist.Count;
                    if (Num > 1)
                    {
                        int ArticleCount = 0;
                        foreach (var item in arlist)
                        {
                            ArticleCount++;

                        }
                        DataTable dt = ToDataTable(arlist);
                        XML = ReArticle(FromUserName, ToUserName, ArticleCount, dt);
                    }
                    else
                    {
                        XML = ReArticle(FromUserName, ToUserName, one.Title, one.Description, one.PicUrl, one.Url);

                    }
                }
                else
                {
                    WechatMessage_TextBll textbll = new WechatMessage_TextBll();
                    WechatMessage_TextEntity textentity = textbll.selectById(entity.WechatMessageId);

                    XML = ReText(FromUserName, ToUserName, textentity.Con);

                }
            }
            else
            {
                XML = ReText(FromUserName, ToUserName, "欢迎关注成都薇莱商贸！");
            }
            return XML;
        }

        #endregion
        public static string Keywordsreply(string FromUserName, string ToUserName,string Keywords)
        {
            string XML = "";

            WechatMessageBll bll = new WechatMessageBll();
           
            string Where = "Reply_Type=2 and Keywords=" +"'"+ Keywords+"'";//1、关注回复 2、关键词回复  3、自动回复
            List<WechatMessageEntity> listentity = bll.selectByWhere(Where);
            if (listentity.Count > 0)
            {

                WechatMessageEntity entity = listentity[0];

                //1.判断消息类型
                int Message_Type = entity.Message_Type;   //1、图文  2、文字
                if (Message_Type == 1)
                {
                    //1、 判断是多图文还是单图文
                    WechatMessage_ImageBll imageBll = new WechatMessage_ImageBll();
                    string pidwhere = "Pid=" + entity.WechatMessageId;
                    List<WechatMessage_ImageEntity> list = new List<WechatMessage_ImageEntity>();
                    WechatMessage_ImageEntity one = imageBll.selectById(entity.WechatMessageId);

                    list = imageBll.selectByWhere(pidwhere);

                    List<WechatMessage_ImageEntity> arlist = new List<WechatMessage_ImageEntity>();
                    arlist.Add(one);
                    foreach (var item in list)
                    {
                        arlist.Add(item);
                    }
                    int Num = arlist.Count;
                    if (Num > 1)
                    {
                        int ArticleCount = 0;
                        foreach (var item in arlist)
                        {
                            ArticleCount++;

                        }
                        DataTable dt = ToDataTable(arlist);
                        XML = ReArticle(FromUserName, ToUserName, ArticleCount, dt);
                    }
                    else
                    {
                        XML = ReArticle(FromUserName, ToUserName, one.Title, one.Description, one.PicUrl, one.Url);

                    }
                }
                else
                {
                    WechatMessage_TextBll textbll = new WechatMessage_TextBll();
                    WechatMessage_TextEntity textentity = textbll.selectById(entity.WechatMessageId);

                    XML = ReText(FromUserName, ToUserName, textentity.Con);

                }
            }
            else
            {
                XML = ReText(FromUserName, ToUserName, "欢迎关注成都薇莱商贸！");
            }
            return XML;
        }

        //关注回复  XML  followreply
        public static string Followreply(string FromUserName, string ToUserName)
        {
            string XML = "";
            WechatMessageBll bll = new WechatMessageBll();
            int Where = 1;//1、关注回复 2、关键词回复  3、自动回复
            WechatMessageEntity entity = bll.selectByReply_Type(Where);
            //1.判断消息类型
            int Message_Type = entity.Message_Type;   //1、图文  2、文字
            if (Message_Type == 1)
            {
                //1、 判断是多图文还是单图文
                WechatMessage_ImageBll imageBll = new WechatMessage_ImageBll();
                string pidwhere = "Pid=" + entity.WechatMessageId;
                List<WechatMessage_ImageEntity> list = new List<WechatMessage_ImageEntity>();
                WechatMessage_ImageEntity one = imageBll.selectById(entity.WechatMessageId);

                list = imageBll.selectByWhere(pidwhere);

                List<WechatMessage_ImageEntity> arlist = new List<WechatMessage_ImageEntity>();
                arlist.Add(one);
                foreach (var item in list)
                {
                    arlist.Add(item);
                }
                int Num = arlist.Count;
                if (Num > 1)
                {
                    int ArticleCount = 0;
                    foreach (var item in arlist)
                    {
                        ArticleCount++;

                    }
                    DataTable dt = ToDataTable(arlist);
                    XML = ReArticle(FromUserName, ToUserName, ArticleCount, dt);
                }
                else
                {
                    XML = ReArticle(FromUserName, ToUserName, one.Title, one.Description,one.PicUrl,one.Url);

                }
            }
            else
            {
                WechatMessage_TextBll textbll = new WechatMessage_TextBll();
                WechatMessage_TextEntity textentity = textbll.selectById(entity.WechatMessageId);

                XML = ReText(FromUserName, ToUserName,textentity.Con);

            }
            return XML;
        }


        public static string Clickreply(string FromUserName, string ToUserName,string EventKey)
        {
            string XML = "";

            int menuId = Convert.ToInt32(EventKey);
            WechatMenuBll menuBll = new WechatMenuBll();
            WechatMenuEntity MunuEntity = menuBll.selectById(menuId);
            int Message_Type = 1;
            int Text = MunuEntity.Text;
            int Image = MunuEntity.Image;
            int MessageId = 1;
            if (Image > 0)
            {
                Message_Type = 1;
                MessageId = Image;
            }
            else {
                Message_Type = 2;
                MessageId = Text;
            }
            //1.判断消息类型
        
            if (Message_Type == 1)
            {
                //1、 判断是多图文还是单图文
                WechatMessage_ImageBll imageBll = new WechatMessage_ImageBll();
                string pidwhere = "Pid=" + MessageId;
                List<WechatMessage_ImageEntity> list = new List<WechatMessage_ImageEntity>();
                WechatMessage_ImageEntity one = imageBll.selectById(MessageId);

                list = imageBll.selectByWhere(pidwhere);

                List<WechatMessage_ImageEntity> arlist = new List<WechatMessage_ImageEntity>();
                arlist.Add(one);
                foreach (var item in list)
                {
                    arlist.Add(item);
                }
                int Num = arlist.Count;
                if (Num > 1)
                {
                    int ArticleCount = 0;
                    foreach (var item in arlist)
                    {
                        ArticleCount++;

                    }
                    DataTable dt = ToDataTable(arlist);
                    XML = ReArticle(FromUserName, ToUserName, ArticleCount, dt);
                }
                else
                {
                    XML = ReArticle(FromUserName, ToUserName, one.Title, one.Description, one.PicUrl, one.Url);

                }
            }
            else
            {
                WechatMessage_TextBll textbll = new WechatMessage_TextBll();
                WechatMessage_TextEntity textentity = textbll.selectById(MessageId);

                XML = ReText(FromUserName, ToUserName, textentity.Con);

            }
            return XML;
        }

        public static DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable dt = new DataTable();
            for (int i = 0; i < properties.Count; i++)
            {
                PropertyDescriptor property = properties[i];
                dt.Columns.Add(property.Name, property.PropertyType);
            }
            object[] values = new object[properties.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = properties[i].GetValue(item);
                }
                dt.Rows.Add(values);
            }
            return dt;
        }




        #region 回复方式
        /// <summary>
        /// 回复文本
        /// </summary>
        /// <param name="FromUserName">发送给谁(openid)</param>
        /// <param name="ToUserName">来自谁(公众账号ID)</param>
        /// <param name="Content">回复类型文本</param>
        /// <returns>拼凑的XML</returns>
        public static string ReText(string FromUserName, string ToUserName, string Content)
        {
            string XML = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName>";//发送给谁(openid)，来自谁(公众账号ID)
            XML += "<CreateTime>" + CommonMethod.ConvertDateTimeInt(DateTime.Now) + "</CreateTime>";//回复时间戳
            XML += "<MsgType><![CDATA[text]]></MsgType>";//回复类型文本
            XML += "<Content><![CDATA[" + Content + "]]></Content><FuncFlag>0</FuncFlag></xml>";//回复内容 FuncFlag设置为1的时候，自动星标刚才接收到的消息，适合活动统计使用
            return XML;
        }

        /// <summary>
        /// 回复单图文
        /// </summary>
        /// <param name="FromUserName">发送给谁(openid)</param>
        /// <param name="ToUserName">来自谁(公众账号ID)</param>
        /// <param name="Title">标题</param>
        /// <param name="Description">详情</param>
        /// <param name="PicUrl">图片地址</param>
        /// <param name="Url">地址</param>
        /// <returns>拼凑的XML</returns>
        public static string ReArticle(string FromUserName, string ToUserName, string Title, string Description, string PicUrl, string Url)
        {
            string XML = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName>";//发送给谁(openid)，来自谁(公众账号ID)
            XML += "<CreateTime>" + CommonMethod.ConvertDateTimeInt(DateTime.Now) + "</CreateTime>";//回复时间戳
            XML += "<MsgType><![CDATA[news]]></MsgType><Content><![CDATA[]]></Content><ArticleCount>1</ArticleCount><Articles>";
            XML += "<item><Title><![CDATA[" + Title + "]]></Title><Description><![CDATA[" + Description + "]]></Description><PicUrl><![CDATA[" + PicUrl + "]]></PicUrl><Url><![CDATA[" + Url + "]]></Url></item>";
            XML += "</Articles><FuncFlag>0</FuncFlag></xml>";
            return XML;
        }

        /// <summary>
        /// 多图文回复
        /// </summary>
        /// <param name="FromUserName">发送给谁(openid)</param>
        /// <param name="ToUserName">来自谁(公众账号ID)</param>
        /// <param name="ArticleCount">图文数量</param>
        /// <param name="dtArticle"></param>
        /// <returns></returns>
        public static string ReArticle(string FromUserName, string ToUserName, int ArticleCount, System.Data.DataTable dtArticle)
        {
            string XML = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName>";//发送给谁(openid)，来自谁(公众账号ID)
            XML += "<CreateTime>" + CommonMethod.ConvertDateTimeInt(DateTime.Now) + "</CreateTime>";//回复时间戳
            XML += "<MsgType><![CDATA[news]]></MsgType><Content><![CDATA[]]></Content><ArticleCount>" + ArticleCount + "</ArticleCount><Articles>";
            foreach (System.Data.DataRow Item in dtArticle.Rows)
            {
                XML += "<item><Title><![CDATA[" + Item["Title"] + "]]></Title><Description><![CDATA[" + Item["Description"] + "]]></Description><PicUrl><![CDATA[" + Item["PicUrl"] + "]]></PicUrl><Url><![CDATA[" + Item["Url"] + "]]></Url></item>";
            }
            XML += "</Articles><FuncFlag>0</FuncFlag></xml>";
            return XML;
        }
        #endregion

    }
}
