using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WbOsWeb.Tools
{
    public class AuthorizationText
    {
        public void AuthorizationRefreshAddText(string ImgName, string Name, string Mobile, string AuthorizationNum, string IdentityCard, string WechatId, string LvName, string strTime)
        {
            if (Mobile == string.Empty || AuthorizationNum == string.Empty || IdentityCard == string.Empty)
            {
                return;
            }
            string authorizationpic = "AuthorizationImage.jpg"; //图片名称
            string filesavePathName = string.Empty;
            filesavePathName = ImgName;
            string AuthorizationImage = ConfigurationManager.AppSettings["AuthorizationImage"];//母版图片路径
            string UploadAuthorizationImage = ConfigurationManager.AppSettings["UploadAuthorizationImage"];//授权图片上传保存路径
            string localPath = Path.Combine(HttpRuntime.AppDomainAppPath, AuthorizationImage);//读取路径
            string savePath = Path.Combine(HttpRuntime.AppDomainAppPath, UploadAuthorizationImage) + filesavePathName;//保存路径       
            string filePathName = string.Empty;

            filePathName = localPath + authorizationpic;//授权母版图片路径



            Image image = Image.FromFile(filePathName);
            Bitmap bitmap = new Bitmap(image, image.Width, image.Height);
            Graphics g = Graphics.FromImage(bitmap);


            MemoryStream ms = new MemoryStream();
            float fontSize = 8.0f;    //字体大小  
            float NameWidth = Mobile.Length * fontSize;  //真实姓名文本的长度  
            float MobileWidth = Mobile.Length * fontSize;  //手机文本的长度  
            float AuthorizationNumWidth = AuthorizationNum.Length * fontSize;  //授权编码文本的长度  
            float IdentityCardWidth = IdentityCard.Length * fontSize;  //身份证文本的长度  

            //定义手机位置
            float MobilerectX = 130;
            float MobilerectY = 253;
            float MobilerectWidth = Mobile.Length * (fontSize + 8);
            float MobilerectHeight = fontSize + 8;

            //定义身份证
            float IdentityCardrectX = 275;
            float IdentityCardrectY = 253;
            float IdentityCardrectWidth = IdentityCard.Length * (fontSize + 8);
            float IdentityCardrectHeight = fontSize + 8;

            //定义代理名称
            float LvNamerectX = 335;
            float LvNamerectY = 235;
            float LvNamerectWidth = LvName.Length * (fontSize + 8);
            float LvNamerectHeight = fontSize + 8;


            //定义真实姓名位置
            float NamerectX = 165;
            float NamerectY = 235;
            float NamerectWidth = Name.Length * (fontSize + 8);
            float NamerectHeight = fontSize + 8;

            //定义微信位置
            float WechatIdrectX = 518;
            float WechatIdrectY = 235;
            float WechatIdrectWidth = WechatId.Length * (fontSize + 8);
            float WechatIdrectHeight = fontSize + 8;

            //授权编号位置信息           
            float AuthorizationNumrectX = 195;
            float AuthorizationNumrectY = 300;
            float AuthorizationNumectWidth = AuthorizationNum.Length * (fontSize + 8);
            float AuthorizationNumrectHeight = fontSize + 8;

            //授权日期
            //string time = DateTime.Now.ToString("D");
            //string atime = DateTime.Now.AddYears(1).ToString("D");
            //string strTime = time + " 至 " + atime;
            float datatimeNumrectX = 195;
            float datatimeNumrectY = 316;
            float datatimeNumectWidth = strTime.Length * (fontSize + 8);
            float datatimeNumrectHeight = fontSize + 8;

            Font font = new Font("黑体", fontSize, FontStyle.Bold);   //定义字体  

            Brush whiteBrush = new SolidBrush(Color.Black);   //白笔刷，画文字用  
            Brush blackBrush = new SolidBrush(Color.FromArgb(0, Color.Black)); ;  //背景笔刷，画背景用透明  


            //下面定义一个矩形区域，以后在这个矩形里画上白底黑字  
            g.FillRectangle(blackBrush, 0, 0, 800, 575);
            //声明矩形域  
            RectangleF textAreaMobile = new RectangleF(MobilerectX, MobilerectY, MobilerectWidth, MobilerectHeight);//手机
            RectangleF textAreaName = new RectangleF(NamerectX, NamerectY, NamerectWidth, NamerectHeight);//真实姓名
            RectangleF textAreaAuthorization = new RectangleF(AuthorizationNumrectX, AuthorizationNumrectY, AuthorizationNumectWidth, AuthorizationNumrectHeight);//授权编号
            RectangleF textAreaTime = new RectangleF(datatimeNumrectX, datatimeNumrectY, datatimeNumectWidth, datatimeNumrectHeight);//授权时间                                                                                                                                                     //  RectangleF textAreaTime = new RectangleF(datatimeNumrectX, datatimeNumrectY, datatimeNumectWidth, datatimeNumrectHeight);//授权时间
            RectangleF textAreaIdentityCard = new RectangleF(IdentityCardrectX, IdentityCardrectY, IdentityCardrectWidth, IdentityCardrectHeight);//身份证
            RectangleF textAreaWechatId = new RectangleF(WechatIdrectX, WechatIdrectY, WechatIdrectWidth, WechatIdrectHeight);//微信号
            RectangleF textAreaLvName = new RectangleF(LvNamerectX, LvNamerectY, LvNamerectWidth, LvNamerectHeight);//代理名称



            g.DrawString(Name, font, whiteBrush, textAreaName);//写入真实姓名
            g.DrawString(Mobile, font, whiteBrush, textAreaMobile);//写入手机号
            g.DrawString(AuthorizationNum, font, whiteBrush, textAreaAuthorization);//写入授权号码
            g.DrawString(strTime, font, whiteBrush, textAreaTime);//写入授权时间
            g.DrawString(IdentityCard, font, whiteBrush, textAreaIdentityCard);//写入授权时间
            g.DrawString(WechatId, font, whiteBrush, textAreaWechatId);//写入微信号
            g.DrawString(LvName, font, whiteBrush, textAreaLvName);//写入微信号



            bitmap.Save(savePath, ImageFormat.Jpeg);
            g.Dispose();
            bitmap.Dispose();
            image.Dispose();
        }

    }
}
