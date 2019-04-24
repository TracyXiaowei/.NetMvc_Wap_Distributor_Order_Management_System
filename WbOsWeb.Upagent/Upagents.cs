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
using WbOsWeb.Bll;
using WbOsWeb.Entity;
using WbOsWeb.Tools;

namespace WbOsWeb.Upagent
{
    public class Upagents
    {
        public void UpagentUser(int UserId,int LvId)
        {
            UpConditionBll Upbll = new UpConditionBll();
            AgentuserBll userbll = new AgentuserBll();
            AgentuserEntity userentity = userbll.selectById(UserId);
            AgentuserEntity upuserentity = new AgentuserEntity();
            AgentuserUptagBll uptagbll = new AgentuserUptagBll();
            int UUUserPid = userentity.UserPid;

            #region 个人信息 不需要变动的部分（升级时）
            upuserentity.Id = UserId;
            upuserentity.Name = userentity.Name;
            upuserentity.LoginName = userentity.LoginName;
            upuserentity.Password = userentity.Password;
            upuserentity.Email = userentity.Email;
            upuserentity.State = userentity.State;
            upuserentity.ProvinceId = userentity.ProvinceId;
            upuserentity.CityId = userentity.CityId;
            upuserentity.DistrictId = userentity.DistrictId;
            upuserentity.Address = userentity.Address;
          
            upuserentity.Mobile = userentity.Mobile;
            upuserentity.AuthorizationNum = userentity.AuthorizationNum;
            upuserentity.IdentityCard = userentity.IdentityCard;
            upuserentity.IdentityCardPic = userentity.IdentityCardPic;
            upuserentity.HeadPic = userentity.HeadPic;
            upuserentity.WechatId = userentity.WechatId;          
            upuserentity.Type = userentity.Type;
            upuserentity.StarInviter = userentity.StarInviter;
            upuserentity.ShopName = userentity.ShopName;
            #endregion

            #region  升级的人  是总代以下的时候
            if (LvId > 3) 
            {
                upuserentity.Createtime = DateTime.Now;
                UserBalanceBll balbll = new UserBalanceBll();
                UserBalanceEntity blentity = balbll.selectById(UserId);//用户余额

                UpConditionEntity upthreeEntity = new UpConditionEntity();
          
                int UpLvId = 3;
                upthreeEntity = Upbll.selectById(UpLvId);
                double PayMoney = upthreeEntity.PayMoney;
                if (blentity != null)
                {
                    int UserPid = Query(UserId, UpLvId);
                    double BalancePrice = blentity.BalancePrice;                
                    if (BalancePrice >= PayMoney)
                    {
                        upuserentity.Inviter = userentity.Inviter;
                        upuserentity.LvId = UpLvId;
                        upuserentity.UserPid = UserPid;
                        string authorizationpic = "AuthorizationImage.jpg";
                        string filesavePathName = string.Empty;
                        string ex = Path.GetExtension(authorizationpic);
                        filesavePathName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ex;
                        string AuthorizationImage = ConfigurationManager.AppSettings["AuthorizationImage"];
                        string UploadAuthorizationImage = ConfigurationManager.AppSettings["UploadAuthorizationImage"];
                        string localPath = Path.Combine(HttpRuntime.AppDomainAppPath, AuthorizationImage);
                        string savePath = Path.Combine(HttpRuntime.AppDomainAppPath, UploadAuthorizationImage) + filesavePathName;
                        string filePathName = string.Empty;
                        filePathName = localPath + authorizationpic;
                        Image image = Image.FromFile(filePathName);
                        Bitmap bitmap = new Bitmap(image, image.Width, image.Height);
                        bitmap.Save(savePath, ImageFormat.Jpeg);
                        upuserentity.AuthorizationPic = filesavePathName;

                        int lvid = UpLvId;
                        UserlvBll lvbll = new UserlvBll();
                        UserlvEntity lventity = lvbll.selectById(lvid);
                        string time = DateTime.Now.ToString("D");
                        string atime = DateTime.Now.AddYears(1).ToString("D");
                        string strTime = time + " 至 " + atime;

                        upuserentity.AuthorizationTime = strTime;

                        //授权书 重新生成
                        string LvName = lventity.Name;
                        // AuthorizationRefreshAddTestToImg(filesavePathName, upuserentity.Name, upuserentity.LoginName, upuserentity.AuthorizationNum, upuserentity.IdentityCard, upuserentity.WechatId, LvName);
                        string Time = DateTime.Now.ToString("yyyy年MM月dd日");
                        AuthorizationText Addtext = new AuthorizationText();
                        Addtext.AuthorizationRefreshAddText(filesavePathName, upuserentity.Name, upuserentity.Mobile, upuserentity.AuthorizationNum, upuserentity.IdentityCard, upuserentity.WechatId, LvName, Time);
                        //升级
                        userbll.Update(upuserentity, UserId);

                        #region 升级记录
                        AgentuserUptagEntity uptagEntity = new AgentuserUptagEntity();
                        uptagEntity = uptagbll.selectById(UserId);
                        if (uptagEntity != null)
                        {
                            AgentuserUptagEntity upEntitytag = new AgentuserUptagEntity();
                            upEntitytag.UserId = UserId;
                            upEntitytag.UpgradeTime = DateTime.Now;
                            uptagbll.Update(upEntitytag, UserId);
                        }
                        else
                        {
                            AgentuserUptagEntity upEntitytag = new AgentuserUptagEntity();
                            upEntitytag.UserId = UserId;
                            upEntitytag.UpgradeTime = DateTime.Now;
                            uptagbll.add(upEntitytag);
                        }
                        #endregion
                    }
                }
           
            }
            #endregion  

            if (LvId==3)
            {
                upuserentity.Createtime = userentity.Createtime;
                UpConditionEntity upEntity = new UpConditionEntity();
                int Upid = LvId - 1;
                upEntity = Upbll.selectById(Upid);
                int Upthree = upEntity.Upthree;
                string lvthreewhere = "Inviter=" + UserId+ " and LvId=3 and State=1";
                //我邀请的人 且等级为3（总代）
                

                //邀请人 邀请的总代（一层）

                List<AgentuserEntity> listUser = userbll.selectByWhere(lvthreewhere);//直接邀请的人数

                List<AgentuserEntity> JlistUser = AlllvThree(UserId); //间接人数和直接总合

                string otherwhere = "StarInviter=" + UserId + " and LvId<3 and State=1";
                List<AgentuserEntity> listotherUser = userbll.selectByWhere(otherwhere);
                if (listUser != null)
                {
                    int UsrtNum = 0;
                    foreach (var item in listUser)
                    {
                        UsrtNum++;
                    }
                    int UserotherNum = 0;
                    if (listotherUser != null)
                    {
                        foreach (var itemother in listotherUser)
                        {
                            UserotherNum++;
                        }

                    }
                    int UsrtNumTotal = UsrtNum + UserotherNum;

                    if (UsrtNumTotal >= Upthree)
                    {
                        int UserPid = Query(UserId, Upid);
                        upuserentity.LvId = Upid;
                        upuserentity.UserPid = UserPid;
                        upuserentity.Inviter = userentity.Inviter;
                        foreach (var upitem in JlistUser)
                        {
                            AgentuserEntity ppentity = new AgentuserEntity();
                            ppentity.Id = upitem.Id;
                            ppentity.Name = upitem.Name;
                            ppentity.LoginName = upitem.LoginName;
                            ppentity.Password = upitem.Password;
                            ppentity.Email = upitem.Email;
                            ppentity.State = upitem.State;
                            ppentity.ProvinceId = upitem.ProvinceId;
                            ppentity.CityId = upitem.CityId;
                            ppentity.DistrictId = upitem.DistrictId;
                            ppentity.Address = upitem.Address;
                            ppentity.Createtime = upitem.Createtime;
                            ppentity.Inviter = upitem.Inviter;
                            ppentity.AuthorizationNum = upitem.AuthorizationNum;
                            ppentity.AuthorizationPic = upitem.AuthorizationPic;
                            ppentity.IdentityCard = upitem.IdentityCard;
                            ppentity.IdentityCardPic = upitem.IdentityCardPic;
                            ppentity.HeadPic = upitem.HeadPic;
                            ppentity.WechatId = upitem.WechatId;
                            ppentity.LvId = upitem.LvId;
                            ppentity.UserPid = UserId;
                            ppentity.Type = upitem.Type;
                            ppentity.AuthorizationTime = upitem.AuthorizationTime;
                            ppentity.Mobile = upitem.Mobile;
                            ppentity.StarInviter = upitem.StarInviter;
                            ppentity.ShopName = upitem.ShopName;
                            userbll.Update(ppentity, upitem.Id);
                        }
                        string authorizationpic = "AuthorizationImage.jpg"; //图片名称
                        string filesavePathName = string.Empty;
                        string ex = Path.GetExtension(authorizationpic);
                        filesavePathName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ex;
                        string AuthorizationImage = ConfigurationManager.AppSettings["AuthorizationImage"];//母版图片路径
                        string UploadAuthorizationImage = ConfigurationManager.AppSettings["UploadAuthorizationImage"];//授权图片上传保存路径
                        string localPath = Path.Combine(HttpRuntime.AppDomainAppPath, AuthorizationImage);//读取路径
                        string savePath = Path.Combine(HttpRuntime.AppDomainAppPath, UploadAuthorizationImage) + filesavePathName;//保存路径  
                        string filePathName = string.Empty;
                        filePathName = localPath + authorizationpic;//授权母版图片路径
                        Image image = Image.FromFile(filePathName);   //找到图片     
                                                                      //把找到的图片通过GDI  定义成图片对象，分辨率赋给了bitmap  
                        Bitmap bitmap = new Bitmap(image, image.Width, image.Height);
                        bitmap.Save(savePath, ImageFormat.Jpeg);//保存图片：新路径
                                                                //****************这里是处理授权图片的操作结束***************//
                        upuserentity.AuthorizationPic = filesavePathName;
                        int lvid = Upid;
                        UserlvBll lvbll = new UserlvBll();
                        UserlvEntity lventity = lvbll.selectById(lvid);
                        string time = DateTime.Now.ToString("D");
                        string atime = DateTime.Now.AddYears(1).ToString("D");
                        string strTime = time + " 至 " + atime;
                        upuserentity.AuthorizationTime = strTime;
                        string LvName = lventity.Name;
                        string Time = DateTime.Now.ToString("yyyy年MM月dd日");
                        AuthorizationText Addtext = new AuthorizationText();
                        Addtext.AuthorizationRefreshAddText(filesavePathName, upuserentity.Name, upuserentity.Mobile, upuserentity.AuthorizationNum, upuserentity.IdentityCard, upuserentity.WechatId, LvName, Time);

                        userbll.Update(upuserentity, UserId);
                        //添加升级时间记录
                        AgentuserUptagEntity uptagEntity = new AgentuserUptagEntity();
                        uptagEntity = uptagbll.selectById(UserId);
                        if (uptagEntity != null)
                        {
                            AgentuserUptagEntity upEntitytag = new AgentuserUptagEntity();
                            upEntitytag.UserId = UserId;
                            upEntitytag.UpgradeTime = DateTime.Now;
                            uptagbll.Update(upEntitytag, UserId);
                        }
                        else
                        {
                            AgentuserUptagEntity upEntitytag = new AgentuserUptagEntity();
                            upEntitytag.UserId = UserId;
                            upEntitytag.UpgradeTime = DateTime.Now;
                            uptagbll.add(upEntitytag);

                        }
                    }
                }
            }

            if (LvId == 2)
            {
                upuserentity.Createtime = userentity.Createtime;
                UpConditionEntity upEntity = new UpConditionEntity();
                int Upid = LvId - 1;
                upEntity = Upbll.selectById(Upid);
                int Upthree = upEntity.Upthree;
                int Uptwo = upEntity.Uptwo;
                //升大牌  
                //1、自己邀请人 属于大总拉起走
                //2、自己的总代，邀请的总代拉走（其实在升级大总得时候已经拉走，这里不用拉走，邀请发展的总代其实已经是下线了）
                //总结：只拉走大总
                string lvthreewhere = "Inviter=" + UserId + " and LvId=3"; //直线总代
                string twowhere = "Inviter=" + UserId + " and LvId=2 and State=1";//直线大总
                string otherwhere = "StarInviter=" + UserId + " and LvId=1 and State=1";//直线大牌（自己是邀请人 但是已经升级到大牌了）
                List<AgentuserEntity> listthreeUser = userbll.selectByWhere(lvthreewhere);
                List<AgentuserEntity> listtwoUser = userbll.selectByWhere(twowhere);

                List<AgentuserEntity> listotherUser = userbll.selectByWhere(otherwhere);
       
                List<AgentuserEntity> Listtwoall = AllUserTwoandOneEntity(UserId);
                int AllListtwoal = AllUserTwoandOne(UserId);

         
                if (listthreeUser != null && listtwoUser != null)
                {
                    int UserthreeNum = 0;
                    foreach (var itemtree in listthreeUser)
                    {
                        UserthreeNum++;
                    }
                    int UsertwoNum = AllListtwoal;//直线大总个数

                    foreach (var itemtwo in listtwoUser)
                    {
                        UsertwoNum++;
                    }
                    int UserotherNum = 0;//已经升级到大牌个数
                    if (listotherUser != null)
                    {
                        foreach (var itemother in listotherUser)
                        {
                            UserotherNum++;
                        }

                    }

                    int AllUptwo = UserotherNum + UsertwoNum; //自己升级到大总和大牌总和

                    //满足升条件  1、总代+大总 +大牌个数>=10  也就是说 
                    //1、大总和大牌  必须大于0   
                    // 满足升级大牌得条件
                    //总代 直线 9 +1大总或者大牌
                    if (AllUptwo > 0 && UserthreeNum + UserotherNum + UsertwoNum >= Upthree + Uptwo)
                    {



                        upuserentity.LvId = Upid;
                        upuserentity.UserPid = 0;
                        upuserentity.Inviter = UUUserPid;
                        foreach (var upitem in listthreeUser)
                        {
                            AgentuserEntity ppentity = new AgentuserEntity();
                            ppentity.Id = upitem.Id;
                            ppentity.Name = upitem.Name;
                            ppentity.LoginName = upitem.LoginName;
                            ppentity.Password = upitem.Password;
                            ppentity.Email = upitem.Email;
                            ppentity.State = upitem.State;
                            ppentity.ProvinceId = upitem.ProvinceId;
                            ppentity.CityId = upitem.CityId;
                            ppentity.DistrictId = upitem.DistrictId;
                            ppentity.Address = upitem.Address;
                            ppentity.Createtime = upitem.Createtime;
                            ppentity.Inviter = upitem.Inviter;
                            ppentity.AuthorizationNum = upitem.AuthorizationNum;
                            ppentity.AuthorizationPic = upitem.AuthorizationPic;
                            ppentity.IdentityCard = upitem.IdentityCard;
                            ppentity.IdentityCardPic = upitem.IdentityCardPic;
                            ppentity.HeadPic = upitem.HeadPic;
                            ppentity.WechatId = upitem.WechatId;
                            ppentity.LvId = upitem.LvId;
                            ppentity.UserPid = UserId;
                            ppentity.Type = upitem.Type;
                            ppentity.AuthorizationTime = upitem.AuthorizationTime;
                            ppentity.StarInviter = upitem.StarInviter;
                            ppentity.Mobile = upitem.Mobile;
                            ppentity.ShopName = upitem.ShopName;
                            userbll.Update(ppentity, upitem.Id);
                        }

                        foreach (var uptwoitem in listtwoUser)
                        {
                            AgentuserEntity pppentity = new AgentuserEntity();
                            pppentity.Id = uptwoitem.Id;
                            pppentity.Name = uptwoitem.Name;
                            pppentity.LoginName = uptwoitem.LoginName;
                            pppentity.Password = uptwoitem.Password;
                            pppentity.Email = uptwoitem.Email;
                            pppentity.State = uptwoitem.State;
                            pppentity.ProvinceId = uptwoitem.ProvinceId;
                            pppentity.CityId = uptwoitem.CityId;
                            pppentity.DistrictId = uptwoitem.DistrictId;
                            pppentity.Address = uptwoitem.Address;
                            pppentity.Createtime = uptwoitem.Createtime;
                            pppentity.Inviter = uptwoitem.Inviter;
                            pppentity.AuthorizationNum = uptwoitem.AuthorizationNum;
                            pppentity.AuthorizationPic = uptwoitem.AuthorizationPic;
                            pppentity.IdentityCard = uptwoitem.IdentityCard;
                            pppentity.IdentityCardPic = uptwoitem.IdentityCardPic;
                            pppentity.HeadPic = uptwoitem.HeadPic;
                            pppentity.WechatId = uptwoitem.WechatId;
                            pppentity.LvId = uptwoitem.LvId;
                            pppentity.UserPid = UserId;
                            pppentity.Type = uptwoitem.Type;
                            pppentity.AuthorizationTime = uptwoitem.AuthorizationTime;
                            pppentity.Mobile = uptwoitem.Mobile;
                            pppentity.StarInviter = uptwoitem.StarInviter;
                            pppentity.ShopName = uptwoitem.ShopName;
                            userbll.Update(pppentity, uptwoitem.Id);
                        }

                        foreach (var alltwoitem in Listtwoall)
                        {
                            AgentuserEntity ppppentity = new AgentuserEntity();
                            ppppentity.Id = alltwoitem.Id;
                            ppppentity.Name = alltwoitem.Name;
                            ppppentity.LoginName = alltwoitem.LoginName;
                            ppppentity.Password = alltwoitem.Password;
                            ppppentity.Email = alltwoitem.Email;
                            ppppentity.State = alltwoitem.State;
                            ppppentity.ProvinceId = alltwoitem.ProvinceId;
                            ppppentity.CityId = alltwoitem.CityId;
                            ppppentity.DistrictId = alltwoitem.DistrictId;
                            ppppentity.Address = alltwoitem.Address;
                            ppppentity.Createtime = alltwoitem.Createtime;
                            ppppentity.Inviter = alltwoitem.Inviter;
                            ppppentity.AuthorizationNum = alltwoitem.AuthorizationNum;
                            ppppentity.AuthorizationPic = alltwoitem.AuthorizationPic;
                            ppppentity.IdentityCard = alltwoitem.IdentityCard;
                            ppppentity.IdentityCardPic = alltwoitem.IdentityCardPic;
                            ppppentity.HeadPic = alltwoitem.HeadPic;
                            ppppentity.WechatId = alltwoitem.WechatId;
                            ppppentity.LvId = alltwoitem.LvId;
                            ppppentity.UserPid = UserId;
                            ppppentity.Type = alltwoitem.Type;
                            ppppentity.AuthorizationTime = alltwoitem.AuthorizationTime;
                            ppppentity.Mobile = alltwoitem.Mobile;
                            ppppentity.StarInviter = alltwoitem.StarInviter;
                            ppppentity.ShopName = alltwoitem.ShopName;
                            userbll.Update(ppppentity, alltwoitem.Id);
                        }

                        string authorizationpic = "AuthorizationImage.jpg"; //图片名称
                        string filesavePathName = string.Empty;
                        string ex = Path.GetExtension(authorizationpic);
                        filesavePathName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ex;
                        string AuthorizationImage = ConfigurationManager.AppSettings["AuthorizationImage"];//母版图片路径
                        string UploadAuthorizationImage = ConfigurationManager.AppSettings["UploadAuthorizationImage"];//授权图片上传保存路径
                        string localPath = Path.Combine(HttpRuntime.AppDomainAppPath, AuthorizationImage);//读取路径
                        string savePath = Path.Combine(HttpRuntime.AppDomainAppPath, UploadAuthorizationImage) + filesavePathName;//保存路径  
                        string filePathName = string.Empty;
                        filePathName = localPath + authorizationpic;//授权母版图片路径
                        Image image = Image.FromFile(filePathName);   //找到图片     
                                                                      //把找到的图片通过GDI  定义成图片对象，分辨率赋给了bitmap  
                        Bitmap bitmap = new Bitmap(image, image.Width, image.Height);
                        bitmap.Save(savePath, ImageFormat.Jpeg);//保存图片：新路径
                                                                //****************这里是处理授权图片的操作结束***************//
                        upuserentity.AuthorizationPic = filesavePathName;
                        int lvid = Upid;
                        UserlvBll lvbll = new UserlvBll();
                        UserlvEntity lventity = lvbll.selectById(lvid);
                        string time = DateTime.Now.ToString("D");
                        string atime = DateTime.Now.AddYears(1).ToString("D");
                        string strTime = time + " 至 " + atime;
                        upuserentity.AuthorizationTime = strTime;
                        string LvName = lventity.Name;

                        string Time = DateTime.Now.ToString("yyyy年MM月dd日");
                        AuthorizationText Addtext = new AuthorizationText();
                        Addtext.AuthorizationRefreshAddText(filesavePathName, upuserentity.Name, upuserentity.Mobile, upuserentity.AuthorizationNum, upuserentity.IdentityCard, upuserentity.WechatId, LvName, Time);


                        userbll.Update(upuserentity, UserId);
                        //添加升级时间记录
                        AgentuserUptagEntity uptagEntity = new AgentuserUptagEntity();
                        uptagEntity = uptagbll.selectById(UserId);
                        if (uptagEntity != null)
                        {
                            AgentuserUptagEntity upEntitytag = new AgentuserUptagEntity();
                            upEntitytag.UserId = UserId;
                            upEntitytag.UpgradeTime = DateTime.Now;
                            uptagbll.Update(upEntitytag, UserId);
                        }
                        else
                        {
                            AgentuserUptagEntity upEntitytag = new AgentuserUptagEntity();
                            upEntitytag.UserId = UserId;
                            upEntitytag.UpgradeTime = DateTime.Now;
                            uptagbll.add(upEntitytag);

                        }



                    }
                    if (AllListtwoal >= Uptwo && UserthreeNum + UserotherNum + UsertwoNum >= Upthree)
                    {

                        upuserentity.LvId = Upid;
                        upuserentity.UserPid = 0;
                        upuserentity.Inviter = UUUserPid;
                        foreach (var upitem in listthreeUser)
                        {
                            AgentuserEntity ppentity = new AgentuserEntity();
                            ppentity.Id = upitem.Id;
                            ppentity.Name = upitem.Name;
                            ppentity.LoginName = upitem.LoginName;
                            ppentity.Password = upitem.Password;
                            ppentity.Email = upitem.Email;
                            ppentity.State = upitem.State;
                            ppentity.ProvinceId = upitem.ProvinceId;
                            ppentity.CityId = upitem.CityId;
                            ppentity.DistrictId = upitem.DistrictId;
                            ppentity.Address = upitem.Address;
                            ppentity.Createtime = upitem.Createtime;
                            ppentity.Inviter = upitem.Inviter;
                            ppentity.AuthorizationNum = upitem.AuthorizationNum;
                            ppentity.AuthorizationPic = upitem.AuthorizationPic;
                            ppentity.IdentityCard = upitem.IdentityCard;
                            ppentity.IdentityCardPic = upitem.IdentityCardPic;
                            ppentity.HeadPic = upitem.HeadPic;
                            ppentity.WechatId = upitem.WechatId;
                            ppentity.LvId = upitem.LvId;
                            ppentity.UserPid = UserId;
                            ppentity.Type = upitem.Type;
                            ppentity.AuthorizationTime = upitem.AuthorizationTime;
                            ppentity.StarInviter = upitem.StarInviter;
                            ppentity.Mobile = upitem.Mobile;
                            ppentity.ShopName = upitem.ShopName;
                            userbll.Update(ppentity, upitem.Id);
                        }

                        foreach (var uptwoitem in listtwoUser)
                        {
                            AgentuserEntity pppentity = new AgentuserEntity();
                            pppentity.Id = uptwoitem.Id;
                            pppentity.Name = uptwoitem.Name;
                            pppentity.LoginName = uptwoitem.LoginName;
                            pppentity.Password = uptwoitem.Password;
                            pppentity.Email = uptwoitem.Email;
                            pppentity.State = uptwoitem.State;
                            pppentity.ProvinceId = uptwoitem.ProvinceId;
                            pppentity.CityId = uptwoitem.CityId;
                            pppentity.DistrictId = uptwoitem.DistrictId;
                            pppentity.Address = uptwoitem.Address;
                            pppentity.Createtime = uptwoitem.Createtime;
                            pppentity.Inviter = uptwoitem.Inviter;
                            pppentity.AuthorizationNum = uptwoitem.AuthorizationNum;
                            pppentity.AuthorizationPic = uptwoitem.AuthorizationPic;
                            pppentity.IdentityCard = uptwoitem.IdentityCard;
                            pppentity.IdentityCardPic = uptwoitem.IdentityCardPic;
                            pppentity.HeadPic = uptwoitem.HeadPic;
                            pppentity.WechatId = uptwoitem.WechatId;
                            pppentity.LvId = uptwoitem.LvId;
                            pppentity.UserPid = UserId;
                            pppentity.Type = uptwoitem.Type;
                            pppentity.AuthorizationTime = uptwoitem.AuthorizationTime;
                            pppentity.Mobile = uptwoitem.Mobile;
                            pppentity.StarInviter = uptwoitem.StarInviter;
                            pppentity.ShopName = uptwoitem.ShopName;
                            userbll.Update(pppentity, uptwoitem.Id);
                        }

                        foreach (var alltwoitem in Listtwoall)
                        {
                            AgentuserEntity ppppentity = new AgentuserEntity();
                            ppppentity.Id = alltwoitem.Id;
                            ppppentity.Name = alltwoitem.Name;
                            ppppentity.LoginName = alltwoitem.LoginName;
                            ppppentity.Password = alltwoitem.Password;
                            ppppentity.Email = alltwoitem.Email;
                            ppppentity.State = alltwoitem.State;
                            ppppentity.ProvinceId = alltwoitem.ProvinceId;
                            ppppentity.CityId = alltwoitem.CityId;
                            ppppentity.DistrictId = alltwoitem.DistrictId;
                            ppppentity.Address = alltwoitem.Address;
                            ppppentity.Createtime = alltwoitem.Createtime;
                            ppppentity.Inviter = alltwoitem.Inviter;
                            ppppentity.AuthorizationNum = alltwoitem.AuthorizationNum;
                            ppppentity.AuthorizationPic = alltwoitem.AuthorizationPic;
                            ppppentity.IdentityCard = alltwoitem.IdentityCard;
                            ppppentity.IdentityCardPic = alltwoitem.IdentityCardPic;
                            ppppentity.HeadPic = alltwoitem.HeadPic;
                            ppppentity.WechatId = alltwoitem.WechatId;
                            ppppentity.LvId = alltwoitem.LvId;
                            ppppentity.UserPid = UserId;
                            ppppentity.Type = alltwoitem.Type;
                            ppppentity.AuthorizationTime = alltwoitem.AuthorizationTime;
                            ppppentity.Mobile = alltwoitem.Mobile;
                            ppppentity.StarInviter = alltwoitem.StarInviter;
                            ppppentity.ShopName = alltwoitem.ShopName;
                            userbll.Update(ppppentity, alltwoitem.Id);
                        }

                        string authorizationpic = "AuthorizationImage.jpg"; //图片名称
                        string filesavePathName = string.Empty;
                        string ex = Path.GetExtension(authorizationpic);
                        filesavePathName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ex;
                        string AuthorizationImage = ConfigurationManager.AppSettings["AuthorizationImage"];//母版图片路径
                        string UploadAuthorizationImage = ConfigurationManager.AppSettings["UploadAuthorizationImage"];//授权图片上传保存路径
                        string localPath = Path.Combine(HttpRuntime.AppDomainAppPath, AuthorizationImage);//读取路径
                        string savePath = Path.Combine(HttpRuntime.AppDomainAppPath, UploadAuthorizationImage) + filesavePathName;//保存路径  
                        string filePathName = string.Empty;
                        filePathName = localPath + authorizationpic;//授权母版图片路径
                        Image image = Image.FromFile(filePathName);   //找到图片     
                                                                      //把找到的图片通过GDI  定义成图片对象，分辨率赋给了bitmap  
                        Bitmap bitmap = new Bitmap(image, image.Width, image.Height);
                        bitmap.Save(savePath, ImageFormat.Jpeg);//保存图片：新路径
                                                                //****************这里是处理授权图片的操作结束***************//
                        upuserentity.AuthorizationPic = filesavePathName;
                        int lvid = Upid;
                        UserlvBll lvbll = new UserlvBll();
                        UserlvEntity lventity = lvbll.selectById(lvid);
                        string time = DateTime.Now.ToString("D");
                        string atime = DateTime.Now.AddYears(1).ToString("D");
                        string strTime = time + " 至 " + atime;
                        upuserentity.AuthorizationTime = strTime;
                        string LvName = lventity.Name;

                        string Time = DateTime.Now.ToString("yyyy年MM月dd日");
                        AuthorizationText Addtext = new AuthorizationText();
                        Addtext.AuthorizationRefreshAddText(filesavePathName, upuserentity.Name, upuserentity.Mobile, upuserentity.AuthorizationNum, upuserentity.IdentityCard, upuserentity.WechatId, LvName, Time);

                        userbll.Update(upuserentity, UserId);
                        //添加升级时间记录
                        AgentuserUptagEntity uptagEntity = new AgentuserUptagEntity();
                        uptagEntity = uptagbll.selectById(UserId);
                        if (uptagEntity != null)
                        {
                            AgentuserUptagEntity upEntitytag = new AgentuserUptagEntity();
                            upEntitytag.UserId = UserId;
                            upEntitytag.UpgradeTime = DateTime.Now;
                            uptagbll.Update(upEntitytag, UserId);
                        }
                        else
                        {
                            AgentuserUptagEntity upEntitytag = new AgentuserUptagEntity();
                            upEntitytag.UserId = UserId;
                            upEntitytag.UpgradeTime = DateTime.Now;
                            uptagbll.add(upEntitytag);

                        }

                    }

                    //int UpthreeOther = 0;
                    //if (UsertwoNum == 1)
                    //{
                    //    UpthreeOther = Upthree;
                    //}
                    //if (UsertwoNum == 2)
                    //{
                    //    UpthreeOther = Upthree - 1;
                    //}
                    //if (UsertwoNum == 3)
                    //{
                    //    UpthreeOther = Upthree - 2;
                    //}
                    //if (UsertwoNum == 4)
                    //{
                    //    UpthreeOther = Upthree - 3;
                    //}
                    //if (UsertwoNum == 5)
                    //{
                    //    UpthreeOther = Upthree - 4;

                    //}
                    //if (UsertwoNum == 6)
                    //{
                    //    UpthreeOther = Upthree - 5;
                    //}
                    //if (UsertwoNum == 7)
                    //{
                    //    UpthreeOther = Upthree - 6;
                    //}
                    //if (UsertwoNum == 8)
                    //{
                    //    UpthreeOther = Upthree - 7;
                    //}
                    //if (UsertwoNum == 9)
                    //{
                    //    UpthreeOther = Upthree - 8;
                    //}







                }
            }

        }
        public void UpagentUserhaha(int UserId, int LvId)
        {
            UpConditionBll Upbll = new UpConditionBll();
            AgentuserBll userbll = new AgentuserBll();
            AgentuserEntity userentity = userbll.selectById(UserId);
            AgentuserEntity upuserentity = new AgentuserEntity();
            AgentuserUptagBll uptagbll = new AgentuserUptagBll();
            int UUUserPid = userentity.UserPid;

            #region 个人信息 不需要变动的部分（升级时）
            upuserentity.Id = UserId;
            upuserentity.Name = userentity.Name;
            upuserentity.LoginName = userentity.LoginName;
            upuserentity.Password = userentity.Password;
            upuserentity.Email = userentity.Email;
            upuserentity.State = userentity.State;
            upuserentity.ProvinceId = userentity.ProvinceId;
            upuserentity.CityId = userentity.CityId;
            upuserentity.DistrictId = userentity.DistrictId;
            upuserentity.Address = userentity.Address;
            upuserentity.Createtime = userentity.Createtime;
            upuserentity.Mobile = userentity.Mobile;
            upuserentity.AuthorizationNum = userentity.AuthorizationNum;
            upuserentity.IdentityCard = userentity.IdentityCard;
            upuserentity.IdentityCardPic = userentity.IdentityCardPic;
            upuserentity.HeadPic = userentity.HeadPic;
            upuserentity.WechatId = userentity.WechatId;
            upuserentity.Type = userentity.Type;
            upuserentity.StarInviter = userentity.StarInviter;
            upuserentity.ShopName = userentity.ShopName;
            #endregion

            #region  升级的人  是总代以下的时候
            if (LvId > 3)
            {
                UserBalanceBll balbll = new UserBalanceBll();
                UserBalanceEntity blentity = balbll.selectById(UserId);//用户余额

                UpConditionEntity upthreeEntity = new UpConditionEntity();
                int Uthreepid = 3;

                upthreeEntity = Upbll.selectById(Uthreepid);
                double PayMoney = upthreeEntity.PayMoney;
                if (blentity != null)
                {
                    int UserPid = Query(UserId, Uthreepid);
                    double BalancePrice = blentity.BalancePrice;
                    int ReNum = blentity.ReNum;
                    if (BalancePrice >= PayMoney)
                    {
                        upuserentity.Inviter = userentity.Inviter;
                        upuserentity.LvId = Uthreepid;
                        upuserentity.UserPid = UserPid;
                        string authorizationpic = "AuthorizationImage.jpg";
                        string filesavePathName = string.Empty;
                        string ex = Path.GetExtension(authorizationpic);
                        filesavePathName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ex;
                        string AuthorizationImage = ConfigurationManager.AppSettings["AuthorizationImage"];
                        string UploadAuthorizationImage = ConfigurationManager.AppSettings["UploadAuthorizationImage"];
                        string localPath = Path.Combine(HttpRuntime.AppDomainAppPath, AuthorizationImage);
                        string savePath = Path.Combine(HttpRuntime.AppDomainAppPath, UploadAuthorizationImage) + filesavePathName;
                        string filePathName = string.Empty;
                        filePathName = localPath + authorizationpic;
                        Image image = Image.FromFile(filePathName);
                        Bitmap bitmap = new Bitmap(image, image.Width, image.Height);
                        bitmap.Save(savePath, ImageFormat.Jpeg);

                        upuserentity.AuthorizationPic = filesavePathName;
                        int lvid = Uthreepid;
                        UserlvBll lvbll = new UserlvBll();
                        UserlvEntity lventity = lvbll.selectById(lvid);
                        string time = DateTime.Now.ToString("D");
                        string atime = DateTime.Now.AddYears(1).ToString("D");
                        string strTime = time + " 至 " + atime;
                        upuserentity.AuthorizationTime = strTime;
                        string LvName = lventity.Name;

                        string Time = DateTime.Now.ToString("yyyy年MM月dd日");
                        AuthorizationText Addtext = new AuthorizationText();
                        Addtext.AuthorizationRefreshAddText(filesavePathName, upuserentity.Name, upuserentity.Mobile, upuserentity.AuthorizationNum, upuserentity.IdentityCard, upuserentity.WechatId, LvName, Time);

                        userbll.Update(upuserentity, UserId);

                        AgentuserUptagEntity uptagEntity = new AgentuserUptagEntity();
                        uptagEntity = uptagbll.selectById(UserId);
                        if (uptagEntity != null)
                        {
                            AgentuserUptagEntity upEntitytag = new AgentuserUptagEntity();
                            upEntitytag.UserId = UserId;
                            upEntitytag.UpgradeTime = DateTime.Now;
                            uptagbll.Update(upEntitytag, UserId);
                        }
                        else
                        {
                            AgentuserUptagEntity upEntitytag = new AgentuserUptagEntity();
                            upEntitytag.UserId = UserId;
                            upEntitytag.UpgradeTime = DateTime.Now;
                            uptagbll.add(upEntitytag);

                        }
                    }
                }
                else
                {
                    string where = "UserId=" + UserId + " and State=3";
                    OrdersBll Ordersbll = new OrdersBll();
                    OrderItemBll OrderItembll = new OrderItemBll();
                    List<OrdersEntity> orderslist = Ordersbll.selectByWhere(where);
                    List<NumEntity> numentity = new List<NumEntity>();
                    if (orderslist != null)
                    {

                        foreach (var item in orderslist)
                        {
                            string Where = "OrederId=" + item.Id;
                            List<OrderItemEntity> listorderitem = OrderItembll.selectByWhere(Where);
                            int Num = 0;
                            foreach (var orderitem in listorderitem)
                            {
                                Num += orderitem.Num;
                            }

                            NumEntity num = new NumEntity();
                            num.Id = Convert.ToInt32(Num);
                            numentity.Add(num);

                            if (LvId == 8)//满足升级入门代理邀请
                            {
                                UpConditionEntity upEntity = new UpConditionEntity();
                                int Upid = LvId - 1;
                                upEntity = Upbll.selectById(Upid);

                                int PayNum = upEntity.PayNum;
                                if (Num >= PayNum)
                                {

                                    int UserPid = Query(UserId, Upid);
                                    upuserentity.LvId = Upid;
                                    upuserentity.UserPid = UserPid;
                                    upuserentity.Inviter = userentity.Inviter;

                                    string lvwhere = "Inviter=" + UserId + " and LvId=8 and State=1";
                                    List<AgentuserEntity> listUser = userbll.selectByWhere(lvwhere);
                                    if (listUser != null)
                                    {
                                        foreach (var upitem in listUser)
                                        {
                                            AgentuserEntity ppentity = new AgentuserEntity();
                                            ppentity.Id = upitem.Id;
                                            ppentity.Name = upitem.Name;
                                            ppentity.LoginName = upitem.LoginName;
                                            ppentity.Password = upitem.Password;
                                            ppentity.Email = upitem.Email;
                                            ppentity.State = upitem.State;
                                            ppentity.ProvinceId = upitem.ProvinceId;
                                            ppentity.CityId = upitem.CityId;
                                            ppentity.DistrictId = upitem.DistrictId;
                                            ppentity.Address = upitem.Address;
                                            ppentity.Createtime = upitem.Createtime;
                                            ppentity.Inviter = upitem.Inviter;
                                            ppentity.AuthorizationNum = upitem.AuthorizationNum;
                                            ppentity.AuthorizationPic = upitem.AuthorizationPic;
                                            ppentity.IdentityCard = upitem.IdentityCard;
                                            ppentity.IdentityCardPic = upitem.IdentityCardPic;
                                            ppentity.HeadPic = upitem.HeadPic;
                                            ppentity.WechatId = upitem.WechatId;
                                            ppentity.LvId = upitem.LvId;
                                            ppentity.UserPid = UserId;
                                            ppentity.Type = upitem.Type;
                                            ppentity.AuthorizationTime = upitem.AuthorizationTime;
                                            ppentity.Mobile = upitem.Mobile;
                                            ppentity.StarInviter = upitem.StarInviter;
                                            ppentity.ShopName = upitem.ShopName;
                                            userbll.Update(ppentity, upitem.Id);
                                        }
                                    }
                                    string authorizationpic = "AuthorizationImage.jpg"; //图片名称
                                    string filesavePathName = string.Empty;
                                    string ex = Path.GetExtension(authorizationpic);
                                    filesavePathName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ex;
                                    string AuthorizationImage = ConfigurationManager.AppSettings["AuthorizationImage"];//母版图片路径
                                    string UploadAuthorizationImage = ConfigurationManager.AppSettings["UploadAuthorizationImage"];//授权图片上传保存路径
                                    string localPath = Path.Combine(HttpRuntime.AppDomainAppPath, AuthorizationImage);//读取路径
                                    string savePath = Path.Combine(HttpRuntime.AppDomainAppPath, UploadAuthorizationImage) + filesavePathName;//保存路径  
                                    string filePathName = string.Empty;
                                    filePathName = localPath + authorizationpic;//授权母版图片路径
                                    Image image = Image.FromFile(filePathName);   //找到图片     
                                                                                  //把找到的图片通过GDI  定义成图片对象，分辨率赋给了bitmap  
                                    Bitmap bitmap = new Bitmap(image, image.Width, image.Height);
                                    bitmap.Save(savePath, ImageFormat.Jpeg);//保存图片：新路径
                                                                            //****************这里是处理授权图片的操作结束***************//
                                    upuserentity.AuthorizationPic = filesavePathName;
                                    int lvid = Upid;
                                    UserlvBll lvbll = new UserlvBll();
                                    UserlvEntity lventity = lvbll.selectById(lvid);
                                    string time = DateTime.Now.ToString("D");
                                    string atime = DateTime.Now.AddYears(1).ToString("D");
                                    string strTime = time + " 至 " + atime;
                                    upuserentity.AuthorizationTime = strTime;
                                    string LvName = lventity.Name;
                                    string Time = DateTime.Now.ToString("yyyy年MM月dd日");
                                    AuthorizationText Addtext = new AuthorizationText();
                                    Addtext.AuthorizationRefreshAddText(filesavePathName, upuserentity.Name, upuserentity.Mobile, upuserentity.AuthorizationNum, upuserentity.IdentityCard, upuserentity.WechatId, LvName, Time);



                                    userbll.Update(upuserentity, UserId);
                                    //添加升级时间记录
                                    AgentuserUptagEntity uptagEntity = new AgentuserUptagEntity();
                                    uptagEntity = uptagbll.selectById(UserId);
                                    if (uptagEntity != null)
                                    {
                                        AgentuserUptagEntity upEntitytag = new AgentuserUptagEntity();
                                        upEntitytag.UserId = UserId;
                                        upEntitytag.UpgradeTime = DateTime.Now;
                                        uptagbll.Update(upEntitytag, UserId);
                                    }
                                    else
                                    {
                                        AgentuserUptagEntity upEntitytag = new AgentuserUptagEntity();
                                        upEntitytag.UserId = UserId;
                                        upEntitytag.UpgradeTime = DateTime.Now;
                                        uptagbll.add(upEntitytag);

                                    }

                                }
                            }
                            if (LvId == 7)//满足升级入门代理邀请
                            {
                                UpConditionEntity upEntity = new UpConditionEntity();
                                int Upid = LvId - 1;
                                upEntity = Upbll.selectById(Upid);
                                int PayNum = upEntity.PayNum;
                                if (Num >= PayNum)
                                {
                                    int UserPid = Query(UserId, Upid);
                                    upuserentity.LvId = Upid;
                                    upuserentity.UserPid = UserPid;
                                    upuserentity.Inviter = userentity.Inviter;
                                    string lvwhere = "Inviter=" + UserId + " and LvId=7";
                                    List<AgentuserEntity> listUser = userbll.selectByWhere(lvwhere);
                                    if (listUser != null)
                                    {
                                        foreach (var upitem in listUser)
                                        {
                                            AgentuserEntity ppentity = new AgentuserEntity();
                                            ppentity.Id = upitem.Id;
                                            ppentity.Name = upitem.Name;
                                            ppentity.LoginName = upitem.LoginName;
                                            ppentity.Password = upitem.Password;
                                            ppentity.Email = upitem.Email;
                                            ppentity.State = upitem.State;
                                            ppentity.ProvinceId = upitem.ProvinceId;
                                            ppentity.CityId = upitem.CityId;
                                            ppentity.DistrictId = upitem.DistrictId;
                                            ppentity.Address = upitem.Address;
                                            ppentity.Createtime = upitem.Createtime;
                                            ppentity.Inviter = upitem.Inviter;
                                            ppentity.AuthorizationNum = upitem.AuthorizationNum;
                                            ppentity.AuthorizationPic = upitem.AuthorizationPic;
                                            ppentity.IdentityCard = upitem.IdentityCard;
                                            ppentity.IdentityCardPic = upitem.IdentityCardPic;
                                            ppentity.HeadPic = upitem.HeadPic;
                                            ppentity.WechatId = upitem.WechatId;
                                            ppentity.LvId = upitem.LvId;
                                            ppentity.UserPid = UserId;
                                            ppentity.Type = upitem.Type;
                                            ppentity.AuthorizationTime = upitem.AuthorizationTime;
                                            ppentity.Mobile = upitem.Mobile;
                                            ppentity.StarInviter = upitem.StarInviter;
                                            ppentity.ShopName = upitem.ShopName;
                                            userbll.Update(ppentity, upitem.Id);
                                        }
                                    }
                                    string authorizationpic = "AuthorizationImage.jpg"; //图片名称
                                    string filesavePathName = string.Empty;
                                    string ex = Path.GetExtension(authorizationpic);
                                    filesavePathName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ex;
                                    string AuthorizationImage = ConfigurationManager.AppSettings["AuthorizationImage"];//母版图片路径
                                    string UploadAuthorizationImage = ConfigurationManager.AppSettings["UploadAuthorizationImage"];//授权图片上传保存路径
                                    string localPath = Path.Combine(HttpRuntime.AppDomainAppPath, AuthorizationImage);//读取路径
                                    string savePath = Path.Combine(HttpRuntime.AppDomainAppPath, UploadAuthorizationImage) + filesavePathName;//保存路径  
                                    string filePathName = string.Empty;
                                    filePathName = localPath + authorizationpic;//授权母版图片路径
                                    Image image = Image.FromFile(filePathName);   //找到图片     
                                                                                  //把找到的图片通过GDI  定义成图片对象，分辨率赋给了bitmap  
                                    Bitmap bitmap = new Bitmap(image, image.Width, image.Height);
                                    bitmap.Save(savePath, ImageFormat.Jpeg);//保存图片：新路径
                                                                            //****************这里是处理授权图片的操作结束***************//
                                    upuserentity.AuthorizationPic = filesavePathName;
                                    int lvid = Upid;
                                    UserlvBll lvbll = new UserlvBll();
                                    UserlvEntity lventity = lvbll.selectById(lvid);
                                    string time = DateTime.Now.ToString("D");
                                    string atime = DateTime.Now.AddYears(1).ToString("D");
                                    string strTime = time + " 至 " + atime;
                                    upuserentity.AuthorizationTime = strTime;
                                    string LvName = lventity.Name;

                                    string Time = DateTime.Now.ToString("yyyy年MM月dd日");
                                    AuthorizationText Addtext = new AuthorizationText();
                                    Addtext.AuthorizationRefreshAddText(filesavePathName, upuserentity.Name, upuserentity.Mobile, upuserentity.AuthorizationNum, upuserentity.IdentityCard, upuserentity.WechatId, LvName, Time);

                                    userbll.Update(upuserentity, UserId);
                                    //添加升级时间记录
                                    AgentuserUptagEntity uptagEntity = new AgentuserUptagEntity();
                                    uptagEntity = uptagbll.selectById(UserId);
                                    if (uptagEntity != null)
                                    {
                                        AgentuserUptagEntity upEntitytag = new AgentuserUptagEntity();
                                        upEntitytag.UserId = UserId;
                                        upEntitytag.UpgradeTime = DateTime.Now;
                                        uptagbll.Update(upEntitytag, UserId);
                                    }
                                    else
                                    {
                                        AgentuserUptagEntity upEntitytag = new AgentuserUptagEntity();
                                        upEntitytag.UserId = UserId;
                                        upEntitytag.UpgradeTime = DateTime.Now;
                                        uptagbll.add(upEntitytag);

                                    }
                                }

                            }

                            if (LvId == 6)//满足升级入门代理邀请
                            {
                                UpConditionEntity upEntity = new UpConditionEntity();
                                int Upid = LvId - 1;
                                upEntity = Upbll.selectById(Upid);
                                int PayNum = upEntity.PayNum;
                                if (Num >= PayNum)
                                {
                                    int UserPid = Query(UserId, Upid);
                                    upuserentity.LvId = Upid;
                                    upuserentity.UserPid = UserPid;
                                    upuserentity.Inviter = userentity.Inviter;

                                    string lvwhere = "Inviter=" + UserId + " and LvId=6";
                                    List<AgentuserEntity> listUser = userbll.selectByWhere(lvwhere);
                                    if (listUser != null)
                                    {
                                        foreach (var upitem in listUser)
                                        {
                                            AgentuserEntity ppentity = new AgentuserEntity();
                                            ppentity.Id = upitem.Id;
                                            ppentity.Name = upitem.Name;
                                            ppentity.LoginName = upitem.LoginName;
                                            ppentity.Password = upitem.Password;
                                            ppentity.Email = upitem.Email;
                                            ppentity.State = upitem.State;
                                            ppentity.ProvinceId = upitem.ProvinceId;
                                            ppentity.CityId = upitem.CityId;
                                            ppentity.DistrictId = upitem.DistrictId;
                                            ppentity.Address = upitem.Address;
                                            ppentity.Createtime = upitem.Createtime;
                                            ppentity.Inviter = upitem.Inviter;
                                            ppentity.AuthorizationNum = upitem.AuthorizationNum;
                                            ppentity.AuthorizationPic = upitem.AuthorizationPic;
                                            ppentity.IdentityCard = upitem.IdentityCard;
                                            ppentity.IdentityCardPic = upitem.IdentityCardPic;
                                            ppentity.HeadPic = upitem.HeadPic;
                                            ppentity.WechatId = upitem.WechatId;
                                            ppentity.LvId = upitem.LvId;
                                            ppentity.UserPid = UserId;
                                            ppentity.Type = upitem.Type;
                                            ppentity.AuthorizationTime = upitem.AuthorizationTime;
                                            ppentity.Mobile = upitem.Mobile;
                                            ppentity.StarInviter = upitem.StarInviter;
                                            ppentity.ShopName = upitem.ShopName;
                                            userbll.Update(ppentity, upitem.Id);
                                        }
                                    }
                                    string authorizationpic = "AuthorizationImage.jpg"; //图片名称
                                    string filesavePathName = string.Empty;
                                    string ex = Path.GetExtension(authorizationpic);
                                    filesavePathName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ex;
                                    string AuthorizationImage = ConfigurationManager.AppSettings["AuthorizationImage"];//母版图片路径
                                    string UploadAuthorizationImage = ConfigurationManager.AppSettings["UploadAuthorizationImage"];//授权图片上传保存路径
                                    string localPath = Path.Combine(HttpRuntime.AppDomainAppPath, AuthorizationImage);//读取路径
                                    string savePath = Path.Combine(HttpRuntime.AppDomainAppPath, UploadAuthorizationImage) + filesavePathName;//保存路径  
                                    string filePathName = string.Empty;
                                    filePathName = localPath + authorizationpic;//授权母版图片路径
                                    Image image = Image.FromFile(filePathName);   //找到图片     
                                                                                  //把找到的图片通过GDI  定义成图片对象，分辨率赋给了bitmap  
                                    Bitmap bitmap = new Bitmap(image, image.Width, image.Height);
                                    bitmap.Save(savePath, ImageFormat.Jpeg);//保存图片：新路径
                                                                            //****************这里是处理授权图片的操作结束***************//
                                    upuserentity.AuthorizationPic = filesavePathName;
                                    int lvid = Upid;
                                    UserlvBll lvbll = new UserlvBll();
                                    UserlvEntity lventity = lvbll.selectById(lvid);
                                    string time = DateTime.Now.ToString("D");
                                    string atime = DateTime.Now.AddYears(1).ToString("D");
                                    string strTime = time + " 至 " + atime;
                                    upuserentity.AuthorizationTime = strTime;
                                    string LvName = lventity.Name;

                                    string Time = DateTime.Now.ToString("yyyy年MM月dd日");
                                    AuthorizationText Addtext = new AuthorizationText();
                                    Addtext.AuthorizationRefreshAddText(filesavePathName, upuserentity.Name, upuserentity.Mobile, upuserentity.AuthorizationNum, upuserentity.IdentityCard, upuserentity.WechatId, LvName, Time);

                                    userbll.Update(upuserentity, UserId);
                                    //添加升级时间记录
                                    AgentuserUptagEntity uptagEntity = new AgentuserUptagEntity();
                                    uptagEntity = uptagbll.selectById(UserId);
                                    if (uptagEntity != null)
                                    {
                                        AgentuserUptagEntity upEntitytag = new AgentuserUptagEntity();
                                        upEntitytag.UserId = UserId;
                                        upEntitytag.UpgradeTime = DateTime.Now;
                                        uptagbll.Update(upEntitytag, UserId);
                                    }
                                    else
                                    {
                                        AgentuserUptagEntity upEntitytag = new AgentuserUptagEntity();
                                        upEntitytag.UserId = UserId;
                                        upEntitytag.UpgradeTime = DateTime.Now;
                                        uptagbll.add(upEntitytag);

                                    }
                                }

                            }

                            if (LvId == 5)//满足升级入门代理邀请
                            {
                                UpConditionEntity upEntity = new UpConditionEntity();
                                int Upid = LvId - 1;
                                upEntity = Upbll.selectById(Upid);
                                int PayNum = upEntity.PayNum;
                                if (Num > PayNum)
                                {
                                    int UserPid = Query(UserId, Upid);
                                    upuserentity.LvId = Upid;
                                    upuserentity.UserPid = UserPid;
                                    upuserentity.Inviter = userentity.Inviter;
                                    string lvwhere = "Inviter=" + UserId + " and LvId=5";
                                    List<AgentuserEntity> listUser = userbll.selectByWhere(lvwhere);
                                    if (listUser != null)
                                    {
                                        foreach (var upitem in listUser)
                                        {
                                            AgentuserEntity ppentity = new AgentuserEntity();
                                            ppentity.Id = upitem.Id;
                                            ppentity.Name = upitem.Name;
                                            ppentity.LoginName = upitem.LoginName;
                                            ppentity.Password = upitem.Password;
                                            ppentity.Email = upitem.Email;
                                            ppentity.State = upitem.State;
                                            ppentity.ProvinceId = upitem.ProvinceId;
                                            ppentity.CityId = upitem.CityId;
                                            ppentity.DistrictId = upitem.DistrictId;
                                            ppentity.Address = upitem.Address;
                                            ppentity.Createtime = upitem.Createtime;
                                            ppentity.Inviter = upitem.Inviter;
                                            ppentity.AuthorizationNum = upitem.AuthorizationNum;
                                            ppentity.AuthorizationPic = upitem.AuthorizationPic;
                                            ppentity.IdentityCard = upitem.IdentityCard;
                                            ppentity.IdentityCardPic = upitem.IdentityCardPic;
                                            ppentity.HeadPic = upitem.HeadPic;
                                            ppentity.WechatId = upitem.WechatId;
                                            ppentity.LvId = upitem.LvId;
                                            ppentity.UserPid = UserId;
                                            ppentity.Type = upitem.Type;
                                            ppentity.AuthorizationTime = upitem.AuthorizationTime;
                                            ppentity.Mobile = upitem.Mobile;
                                            ppentity.StarInviter = upitem.StarInviter;
                                            ppentity.ShopName = upitem.ShopName;
                                            userbll.Update(ppentity, upitem.Id);
                                        }
                                    }
                                    string authorizationpic = "AuthorizationImage.jpg"; //图片名称
                                    string filesavePathName = string.Empty;
                                    string ex = Path.GetExtension(authorizationpic);
                                    filesavePathName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ex;
                                    string AuthorizationImage = ConfigurationManager.AppSettings["AuthorizationImage"];//母版图片路径
                                    string UploadAuthorizationImage = ConfigurationManager.AppSettings["UploadAuthorizationImage"];//授权图片上传保存路径
                                    string localPath = Path.Combine(HttpRuntime.AppDomainAppPath, AuthorizationImage);//读取路径
                                    string savePath = Path.Combine(HttpRuntime.AppDomainAppPath, UploadAuthorizationImage) + filesavePathName;//保存路径  
                                    string filePathName = string.Empty;
                                    filePathName = localPath + authorizationpic;//授权母版图片路径
                                    Image image = Image.FromFile(filePathName);   //找到图片     
                                                                                  //把找到的图片通过GDI  定义成图片对象，分辨率赋给了bitmap  
                                    Bitmap bitmap = new Bitmap(image, image.Width, image.Height);
                                    bitmap.Save(savePath, ImageFormat.Jpeg);//保存图片：新路径
                                                                            //****************这里是处理授权图片的操作结束***************//
                                    upuserentity.AuthorizationPic = filesavePathName;
                                    int lvid = Upid;
                                    UserlvBll lvbll = new UserlvBll();
                                    UserlvEntity lventity = lvbll.selectById(lvid);
                                    string time = DateTime.Now.ToString("D");
                                    string atime = DateTime.Now.AddYears(1).ToString("D");
                                    string strTime = time + " 至 " + atime;
                                    upuserentity.AuthorizationTime = strTime;
                                    string LvName = lventity.Name;


                                    string Time = DateTime.Now.ToString("yyyy年MM月dd日");
                                    AuthorizationText Addtext = new AuthorizationText();
                                    Addtext.AuthorizationRefreshAddText(filesavePathName, upuserentity.Name, upuserentity.Mobile, upuserentity.AuthorizationNum, upuserentity.IdentityCard, upuserentity.WechatId, LvName, Time);

                                    userbll.Update(upuserentity, UserId);
                                    //添加升级时间记录
                                    AgentuserUptagEntity uptagEntity = new AgentuserUptagEntity();
                                    uptagEntity = uptagbll.selectById(UserId);
                                    if (uptagEntity != null)
                                    {
                                        AgentuserUptagEntity upEntitytag = new AgentuserUptagEntity();
                                        upEntitytag.UserId = UserId;
                                        upEntitytag.UpgradeTime = DateTime.Now;
                                        uptagbll.Update(upEntitytag, UserId);
                                    }
                                    else
                                    {
                                        AgentuserUptagEntity upEntitytag = new AgentuserUptagEntity();
                                        upEntitytag.UserId = UserId;
                                        upEntitytag.UpgradeTime = DateTime.Now;
                                        uptagbll.add(upEntitytag);

                                    }
                                }

                            }

                        }

                        #region
                        //if (LvId == 4)
                        //{


                        //    if (blentity != null)
                        //    {
                        //        double BalancePrice = blentity.BalancePrice;
                        //        int ReNum = blentity.ReNum;
                        //        if (BalancePrice >= PayMoney && ReNum == 1)
                        //        {
                        //            int UserPid = Query(UserId, 3);
                        //            upuserentity.LvId = 3;
                        //            upuserentity.UserPid = UserPid;
                        //            upuserentity.Inviter = userentity.Inviter;
                        //            string lvwhere = "Inviter=" + UserId + " and LvId=4";
                        //            List<AgentuserEntity> listUser = userbll.selectByWhere(lvwhere);
                        //            if (listUser != null)
                        //            {
                        //                foreach (var upitem in listUser)
                        //                {
                        //                    AgentuserEntity ppentity = new AgentuserEntity();
                        //                    ppentity.Id = upitem.Id;
                        //                    ppentity.Name = upitem.Name;
                        //                    ppentity.LoginName = upitem.LoginName;
                        //                    ppentity.Password = upitem.Password;
                        //                    ppentity.Email = upitem.Email;
                        //                    ppentity.State = upitem.State;
                        //                    ppentity.ProvinceId = upitem.ProvinceId;
                        //                    ppentity.CityId = upitem.CityId;
                        //                    ppentity.DistrictId = upitem.DistrictId;
                        //                    ppentity.Address = upitem.Address;
                        //                    ppentity.Createtime = upitem.Createtime;
                        //                    ppentity.Inviter = upitem.Inviter;
                        //                    ppentity.AuthorizationNum = upitem.AuthorizationNum;
                        //                    ppentity.AuthorizationPic = upitem.AuthorizationPic;
                        //                    ppentity.IdentityCard = upitem.IdentityCard;
                        //                    ppentity.IdentityCardPic = upitem.IdentityCardPic;
                        //                    ppentity.HeadPic = upitem.HeadPic;
                        //                    ppentity.WechatId = upitem.WechatId;
                        //                    ppentity.LvId = upitem.LvId;
                        //                    ppentity.UserPid = UserId;
                        //                    ppentity.Type = upitem.Type;
                        //                    ppentity.AuthorizationTime = upitem.AuthorizationTime;
                        //                    ppentity.Mobile = upitem.Mobile;
                        //                    ppentity.StarInviter = upitem.StarInviter;
                        //                    userbll.Update(ppentity, upitem.Id);
                        //                }
                        //            }
                        //            string authorizationpic = "AuthorizationImage.jpg"; //图片名称
                        //            string filesavePathName = string.Empty;
                        //            string ex = Path.GetExtension(authorizationpic);
                        //            filesavePathName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ex;
                        //            string AuthorizationImage = ConfigurationManager.AppSettings["AuthorizationImage"];//母版图片路径
                        //            string UploadAuthorizationImage = ConfigurationManager.AppSettings["UploadAuthorizationImage"];//授权图片上传保存路径
                        //            string localPath = Path.Combine(HttpRuntime.AppDomainAppPath, AuthorizationImage);//读取路径
                        //            string savePath = Path.Combine(HttpRuntime.AppDomainAppPath, UploadAuthorizationImage) + filesavePathName;//保存路径  
                        //            string filePathName = string.Empty;
                        //            filePathName = localPath + authorizationpic;//授权母版图片路径
                        //            Image image = Image.FromFile(filePathName);   //找到图片     
                        //                                                          //把找到的图片通过GDI  定义成图片对象，分辨率赋给了bitmap  
                        //            Bitmap bitmap = new Bitmap(image, image.Width, image.Height);
                        //            bitmap.Save(savePath, ImageFormat.Jpeg);//保存图片：新路径
                        //                                                    //****************这里是处理授权图片的操作结束***************//
                        //            upuserentity.AuthorizationPic = filesavePathName;
                        //            int lvid = 3;
                        //            UserlvBll lvbll = new UserlvBll();
                        //            UserlvEntity lventity = lvbll.selectById(lvid);
                        //            string time = DateTime.Now.ToString("D");
                        //            string atime = DateTime.Now.AddYears(1).ToString("D");
                        //            string strTime = time + " 至 " + atime;
                        //            upuserentity.AuthorizationTime = strTime;
                        //            string LvName = lventity.Name;
                        //            AddTestToImg(filesavePathName, upuserentity.Name, upuserentity.LoginName, upuserentity.AuthorizationNum, upuserentity.IdentityCard, upuserentity.WechatId, LvName, strTime);

                        //            userbll.Update(upuserentity, UserId);
                        //            //添加升级时间记录
                        //            AgentuserUptagEntity uptagEntity = new AgentuserUptagEntity();
                        //            uptagEntity = uptagbll.selectById(UserId);
                        //            if (uptagEntity != null)
                        //            {
                        //                AgentuserUptagEntity upEntitytag = new AgentuserUptagEntity();
                        //                upEntitytag.UserId = UserId;
                        //                upEntitytag.UpgradeTime = DateTime.Now;
                        //                uptagbll.Update(upEntitytag, UserId);
                        //            }
                        //            else
                        //            {
                        //                AgentuserUptagEntity upEntitytag = new AgentuserUptagEntity();
                        //                upEntitytag.UserId = UserId;
                        //                upEntitytag.UpgradeTime = DateTime.Now;
                        //                uptagbll.add(upEntitytag);

                        //            }
                        //        }
                        //    }
                        //}
                        #endregion
                    }
                }
            }
            #endregion  

            if (LvId == 3)
            {
                UpConditionEntity upEntity = new UpConditionEntity();
                int Upid = LvId - 1;
                upEntity = Upbll.selectById(Upid);
                int Upthree = upEntity.Upthree;
                string lvthreewhere = "Inviter=" + UserId + " and LvId=3 and State=1";
                //我邀请的人 且等级为3（总代）


                //邀请人 邀请的总代（一层）

                List<AgentuserEntity> listUser = userbll.selectByWhere(lvthreewhere);//直接邀请的人数

                List<AgentuserEntity> JlistUser = AlllvThree(UserId); //间接人数和直接总合

                string otherwhere = "StarInviter=" + UserId + " and LvId<3 and State=1";
                List<AgentuserEntity> listotherUser = userbll.selectByWhere(otherwhere);
                if (listUser != null)
                {
                    int UsrtNum = 0;
                    foreach (var item in listUser)
                    {
                        UsrtNum++;
                    }
                    int UserotherNum = 0;
                    if (listotherUser != null)
                    {
                        foreach (var itemother in listotherUser)
                        {
                            UserotherNum++;
                        }

                    }
                    int UsrtNumTotal = UsrtNum + UserotherNum;

                    if (UsrtNumTotal >= Upthree)
                    {
                        int UserPid = Query(UserId, Upid);
                        upuserentity.LvId = Upid;
                        upuserentity.UserPid = UserPid;
                        upuserentity.Inviter = userentity.Inviter;
                        foreach (var upitem in JlistUser)
                        {
                            AgentuserEntity ppentity = new AgentuserEntity();
                            ppentity.Id = upitem.Id;
                            ppentity.Name = upitem.Name;
                            ppentity.LoginName = upitem.LoginName;
                            ppentity.Password = upitem.Password;
                            ppentity.Email = upitem.Email;
                            ppentity.State = upitem.State;
                            ppentity.ProvinceId = upitem.ProvinceId;
                            ppentity.CityId = upitem.CityId;
                            ppentity.DistrictId = upitem.DistrictId;
                            ppentity.Address = upitem.Address;
                            ppentity.Createtime = upitem.Createtime;
                            ppentity.Inviter = upitem.Inviter;
                            ppentity.AuthorizationNum = upitem.AuthorizationNum;
                            ppentity.AuthorizationPic = upitem.AuthorizationPic;
                            ppentity.IdentityCard = upitem.IdentityCard;
                            ppentity.IdentityCardPic = upitem.IdentityCardPic;
                            ppentity.HeadPic = upitem.HeadPic;
                            ppentity.WechatId = upitem.WechatId;
                            ppentity.LvId = upitem.LvId;
                            ppentity.UserPid = UserId;
                            ppentity.Type = upitem.Type;
                            ppentity.AuthorizationTime = upitem.AuthorizationTime;
                            ppentity.Mobile = upitem.Mobile;
                            ppentity.StarInviter = upitem.StarInviter;
                            ppentity.ShopName = upitem.ShopName;
                            userbll.Update(ppentity, upitem.Id);
                        }
                        string authorizationpic = "AuthorizationImage.jpg"; //图片名称
                        string filesavePathName = string.Empty;
                        string ex = Path.GetExtension(authorizationpic);
                        filesavePathName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ex;
                        string AuthorizationImage = ConfigurationManager.AppSettings["AuthorizationImage"];//母版图片路径
                        string UploadAuthorizationImage = ConfigurationManager.AppSettings["UploadAuthorizationImage"];//授权图片上传保存路径
                        string localPath = Path.Combine(HttpRuntime.AppDomainAppPath, AuthorizationImage);//读取路径
                        string savePath = Path.Combine(HttpRuntime.AppDomainAppPath, UploadAuthorizationImage) + filesavePathName;//保存路径  
                        string filePathName = string.Empty;
                        filePathName = localPath + authorizationpic;//授权母版图片路径
                        Image image = Image.FromFile(filePathName);   //找到图片     
                                                                      //把找到的图片通过GDI  定义成图片对象，分辨率赋给了bitmap  
                        Bitmap bitmap = new Bitmap(image, image.Width, image.Height);
                        bitmap.Save(savePath, ImageFormat.Jpeg);//保存图片：新路径
                                                                //****************这里是处理授权图片的操作结束***************//
                        upuserentity.AuthorizationPic = filesavePathName;
                        int lvid = Upid;
                        UserlvBll lvbll = new UserlvBll();
                        UserlvEntity lventity = lvbll.selectById(lvid);
                        string time = DateTime.Now.ToString("D");
                        string atime = DateTime.Now.AddYears(1).ToString("D");
                        string strTime = time + " 至 " + atime;
                        upuserentity.AuthorizationTime = strTime;
                        string LvName = lventity.Name;

                        string Time = DateTime.Now.ToString("yyyy年MM月dd日");
                        AuthorizationText Addtext = new AuthorizationText();
                        Addtext.AuthorizationRefreshAddText(filesavePathName, upuserentity.Name, upuserentity.Mobile, upuserentity.AuthorizationNum, upuserentity.IdentityCard, upuserentity.WechatId, LvName, Time);

                        userbll.Update(upuserentity, UserId);
                        //添加升级时间记录
                        AgentuserUptagEntity uptagEntity = new AgentuserUptagEntity();
                        uptagEntity = uptagbll.selectById(UserId);
                        if (uptagEntity != null)
                        {
                            AgentuserUptagEntity upEntitytag = new AgentuserUptagEntity();
                            upEntitytag.UserId = UserId;
                            upEntitytag.UpgradeTime = DateTime.Now;
                            uptagbll.Update(upEntitytag, UserId);
                        }
                        else
                        {
                            AgentuserUptagEntity upEntitytag = new AgentuserUptagEntity();
                            upEntitytag.UserId = UserId;
                            upEntitytag.UpgradeTime = DateTime.Now;
                            uptagbll.add(upEntitytag);

                        }
                    }
                }
            }

            if (LvId == 2)
            {
                UpConditionEntity upEntity = new UpConditionEntity();
                int Upid = LvId - 1;
                upEntity = Upbll.selectById(Upid);
                int Upthree = upEntity.Upthree;
                int Uptwo = upEntity.Uptwo;
                //升大牌  
                //1、自己邀请人 属于大总拉起走
                //2、自己的总代，邀请的总代拉走（其实在升级大总得时候已经拉走，这里不用拉走，邀请发展的总代其实已经是下线了）
                //总结：只拉走大总
                string lvthreewhere = "Inviter=" + UserId + " and LvId=3";
                string twowhere = "Inviter=" + UserId + " and LvId=2 and State=1";
                string otherwhere = "StarInviter=" + UserId + " and LvId<3 and State=1";
                List<AgentuserEntity> listthreeUser = userbll.selectByWhere(lvthreewhere);
                List<AgentuserEntity> listtwoUser = userbll.selectByWhere(twowhere);

                List<AgentuserEntity> listotherUser = userbll.selectByWhere(otherwhere);
                List<AgentuserEntity> Listtwoall = AlllvTwo(UserId);

                if (listthreeUser != null && listtwoUser != null)
                {
                    int UserthreeNum = 0;
                    foreach (var itemtree in listthreeUser)
                    {
                        UserthreeNum++;
                    }

                    int UsertwoNum = 0;
                    foreach (var itemtwo in listtwoUser)
                    {
                        UsertwoNum++;
                    }
                    int UserotherNum = 0;
                    if (listotherUser != null)
                    {
                        foreach (var itemother in listotherUser)
                        {
                            UserotherNum++;
                        }

                    }
                    int UpthreeOther = 0;
                    if (UserotherNum == 1)
                    {
                        UpthreeOther = Upthree;
                    }
                    if (UserotherNum == 2)
                    {
                        UpthreeOther = Upthree - 1;
                    }
                    if (UserotherNum == 3)
                    {
                        UpthreeOther = Upthree - 2;
                    }
                    if (UserotherNum == 4)
                    {
                        UpthreeOther = Upthree - 3;
                    }
                    if (UserotherNum == 5)
                    {
                        UpthreeOther = Upthree - 4;

                    }
                    if (UserotherNum == 6)
                    {
                        UpthreeOther = Upthree - 5;
                    }
                    if (UserotherNum == 7)
                    {
                        UpthreeOther = Upthree - 6;
                    }
                    if (UserotherNum == 8)
                    {
                        UpthreeOther = Upthree - 7;
                    }
                    if (UserotherNum == 9)
                    {
                        UpthreeOther = Upthree - 8;
                    }

                    if (UserthreeNum >= UpthreeOther)
                    {
                        if (UsertwoNum >= Uptwo || UserotherNum >= Uptwo)
                        {


                            upuserentity.LvId = Upid;
                            upuserentity.UserPid = 0;
                            upuserentity.Inviter = UUUserPid;
                            foreach (var upitem in listthreeUser)
                            {
                                AgentuserEntity ppentity = new AgentuserEntity();
                                ppentity.Id = upitem.Id;
                                ppentity.Name = upitem.Name;
                                ppentity.LoginName = upitem.LoginName;
                                ppentity.Password = upitem.Password;
                                ppentity.Email = upitem.Email;
                                ppentity.State = upitem.State;
                                ppentity.ProvinceId = upitem.ProvinceId;
                                ppentity.CityId = upitem.CityId;
                                ppentity.DistrictId = upitem.DistrictId;
                                ppentity.Address = upitem.Address;
                                ppentity.Createtime = upitem.Createtime;
                                ppentity.Inviter = upitem.Inviter;
                                ppentity.AuthorizationNum = upitem.AuthorizationNum;
                                ppentity.AuthorizationPic = upitem.AuthorizationPic;
                                ppentity.IdentityCard = upitem.IdentityCard;
                                ppentity.IdentityCardPic = upitem.IdentityCardPic;
                                ppentity.HeadPic = upitem.HeadPic;
                                ppentity.WechatId = upitem.WechatId;
                                ppentity.LvId = upitem.LvId;
                                ppentity.UserPid = UserId;
                                ppentity.Type = upitem.Type;
                                ppentity.AuthorizationTime = upitem.AuthorizationTime;
                                ppentity.StarInviter = upitem.StarInviter;
                                ppentity.Mobile = upitem.Mobile;
                                ppentity.ShopName = upitem.ShopName;
                                userbll.Update(ppentity, upitem.Id);
                            }

                            foreach (var uptwoitem in listtwoUser)
                            {
                                AgentuserEntity pppentity = new AgentuserEntity();
                                pppentity.Id = uptwoitem.Id;
                                pppentity.Name = uptwoitem.Name;
                                pppentity.LoginName = uptwoitem.LoginName;
                                pppentity.Password = uptwoitem.Password;
                                pppentity.Email = uptwoitem.Email;
                                pppentity.State = uptwoitem.State;
                                pppentity.ProvinceId = uptwoitem.ProvinceId;
                                pppentity.CityId = uptwoitem.CityId;
                                pppentity.DistrictId = uptwoitem.DistrictId;
                                pppentity.Address = uptwoitem.Address;
                                pppentity.Createtime = uptwoitem.Createtime;
                                pppentity.Inviter = uptwoitem.Inviter;
                                pppentity.AuthorizationNum = uptwoitem.AuthorizationNum;
                                pppentity.AuthorizationPic = uptwoitem.AuthorizationPic;
                                pppentity.IdentityCard = uptwoitem.IdentityCard;
                                pppentity.IdentityCardPic = uptwoitem.IdentityCardPic;
                                pppentity.HeadPic = uptwoitem.HeadPic;
                                pppentity.WechatId = uptwoitem.WechatId;
                                pppentity.LvId = uptwoitem.LvId;
                                pppentity.UserPid = UserId;
                                pppentity.Type = uptwoitem.Type;
                                pppentity.AuthorizationTime = uptwoitem.AuthorizationTime;
                                pppentity.Mobile = uptwoitem.Mobile;
                                pppentity.StarInviter = uptwoitem.StarInviter;
                                pppentity.ShopName = uptwoitem.ShopName;
                                userbll.Update(pppentity, uptwoitem.Id);
                            }

                            foreach (var alltwoitem in Listtwoall)
                            {
                                AgentuserEntity ppppentity = new AgentuserEntity();
                                ppppentity.Id = alltwoitem.Id;
                                ppppentity.Name = alltwoitem.Name;
                                ppppentity.LoginName = alltwoitem.LoginName;
                                ppppentity.Password = alltwoitem.Password;
                                ppppentity.Email = alltwoitem.Email;
                                ppppentity.State = alltwoitem.State;
                                ppppentity.ProvinceId = alltwoitem.ProvinceId;
                                ppppentity.CityId = alltwoitem.CityId;
                                ppppentity.DistrictId = alltwoitem.DistrictId;
                                ppppentity.Address = alltwoitem.Address;
                                ppppentity.Createtime = alltwoitem.Createtime;
                                ppppentity.Inviter = alltwoitem.Inviter;
                                ppppentity.AuthorizationNum = alltwoitem.AuthorizationNum;
                                ppppentity.AuthorizationPic = alltwoitem.AuthorizationPic;
                                ppppentity.IdentityCard = alltwoitem.IdentityCard;
                                ppppentity.IdentityCardPic = alltwoitem.IdentityCardPic;
                                ppppentity.HeadPic = alltwoitem.HeadPic;
                                ppppentity.WechatId = alltwoitem.WechatId;
                                ppppentity.LvId = alltwoitem.LvId;
                                ppppentity.UserPid = UserId;
                                ppppentity.Type = alltwoitem.Type;
                                ppppentity.AuthorizationTime = alltwoitem.AuthorizationTime;
                                ppppentity.Mobile = alltwoitem.Mobile;
                                ppppentity.StarInviter = alltwoitem.StarInviter;
                                ppppentity.ShopName = alltwoitem.ShopName;
                                userbll.Update(ppppentity, alltwoitem.Id);
                            }

                            string authorizationpic = "AuthorizationImage.jpg"; //图片名称
                            string filesavePathName = string.Empty;
                            string ex = Path.GetExtension(authorizationpic);
                            filesavePathName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ex;
                            string AuthorizationImage = ConfigurationManager.AppSettings["AuthorizationImage"];//母版图片路径
                            string UploadAuthorizationImage = ConfigurationManager.AppSettings["UploadAuthorizationImage"];//授权图片上传保存路径
                            string localPath = Path.Combine(HttpRuntime.AppDomainAppPath, AuthorizationImage);//读取路径
                            string savePath = Path.Combine(HttpRuntime.AppDomainAppPath, UploadAuthorizationImage) + filesavePathName;//保存路径  
                            string filePathName = string.Empty;
                            filePathName = localPath + authorizationpic;//授权母版图片路径
                            Image image = Image.FromFile(filePathName);   //找到图片     
                                                                          //把找到的图片通过GDI  定义成图片对象，分辨率赋给了bitmap  
                            Bitmap bitmap = new Bitmap(image, image.Width, image.Height);
                            bitmap.Save(savePath, ImageFormat.Jpeg);//保存图片：新路径
                                                                    //****************这里是处理授权图片的操作结束***************//
                            upuserentity.AuthorizationPic = filesavePathName;
                            int lvid = Upid;
                            UserlvBll lvbll = new UserlvBll();
                            UserlvEntity lventity = lvbll.selectById(lvid);
                            string time = DateTime.Now.ToString("D");
                            string atime = DateTime.Now.AddYears(1).ToString("D");
                            string strTime = time + " 至 " + atime;
                            upuserentity.AuthorizationTime = strTime;
                            string LvName = lventity.Name;

                            string Time = DateTime.Now.ToString("yyyy年MM月dd日");
                            AuthorizationText Addtext = new AuthorizationText();
                            Addtext.AuthorizationRefreshAddText(filesavePathName, upuserentity.Name, upuserentity.Mobile, upuserentity.AuthorizationNum, upuserentity.IdentityCard, upuserentity.WechatId, LvName, Time);


                            userbll.Update(upuserentity, UserId);
                            //添加升级时间记录
                            AgentuserUptagEntity uptagEntity = new AgentuserUptagEntity();
                            uptagEntity = uptagbll.selectById(UserId);
                            if (uptagEntity != null)
                            {
                                AgentuserUptagEntity upEntitytag = new AgentuserUptagEntity();
                                upEntitytag.UserId = UserId;
                                upEntitytag.UpgradeTime = DateTime.Now;
                                uptagbll.Update(upEntitytag, UserId);
                            }
                            else
                            {
                                AgentuserUptagEntity upEntitytag = new AgentuserUptagEntity();
                                upEntitytag.UserId = UserId;
                                upEntitytag.UpgradeTime = DateTime.Now;
                                uptagbll.add(upEntitytag);

                            }
                        }
                    }
                }
            }

        }
        public void UpagenttwoUserRefresh(int UserId, int LvId)
        {
            UpConditionBll Upbll = new UpConditionBll();
            AgentuserBll userbll = new AgentuserBll();
            AgentuserEntity userentity = userbll.selectById(UserId);
            AgentuserEntity upuserentity = new AgentuserEntity();
            AgentuserUptagBll uptagbll = new AgentuserUptagBll();

            int UUUserPid = userentity.UserPid;
            upuserentity.Id = UserId;
            upuserentity.Name = userentity.Name;
            upuserentity.LoginName = userentity.LoginName;
            upuserentity.Password = userentity.Password;
            upuserentity.Email = userentity.Email;
            upuserentity.State = userentity.State;
            upuserentity.ProvinceId = userentity.ProvinceId;
            upuserentity.CityId = userentity.CityId;
            upuserentity.DistrictId = userentity.DistrictId;
            upuserentity.Address = userentity.Address;
            upuserentity.Createtime = userentity.Createtime;
            upuserentity.Mobile = userentity.Mobile;
            upuserentity.AuthorizationNum = userentity.AuthorizationNum;
            upuserentity.IdentityCard = userentity.IdentityCard;
            upuserentity.IdentityCardPic = userentity.IdentityCardPic;
            upuserentity.HeadPic = userentity.HeadPic;
            upuserentity.WechatId = userentity.WechatId;
            upuserentity.AuthorizationTime = userentity.AuthorizationTime;
            upuserentity.Type = userentity.Type;
            upuserentity.StarInviter = userentity.StarInviter;
            upuserentity.ShopName = userentity.ShopName;


            string lvthreewhere = "Inviter=" + UserId + " and LvId=3";
           

            List<AgentuserEntity> listthreeUser = userbll.selectByWhere(lvthreewhere);//所有自己邀请的总代

            List<AgentuserEntity> Listthreeall = AlllvThree(UserId);//自已邀请所有总代循环总代
            List<AgentuserEntity> PidListthreeall = PidAlllvThree(UserId);//下级总代邀请总代循环总代

            foreach (var upitem in listthreeUser)
            {
                AgentuserEntity ppentity = new AgentuserEntity();
                ppentity.Id = upitem.Id;
                ppentity.Name = upitem.Name;
                ppentity.LoginName = upitem.LoginName;
                ppentity.Password = upitem.Password;
                ppentity.Email = upitem.Email;
                ppentity.State = upitem.State;
                ppentity.ProvinceId = upitem.ProvinceId;
                ppentity.CityId = upitem.CityId;
                ppentity.DistrictId = upitem.DistrictId;
                ppentity.Address = upitem.Address;
                ppentity.Createtime = upitem.Createtime;
                ppentity.Inviter = upitem.Inviter;
                ppentity.AuthorizationNum = upitem.AuthorizationNum;
                ppentity.AuthorizationPic = upitem.AuthorizationPic;
                ppentity.IdentityCard = upitem.IdentityCard;
                ppentity.IdentityCardPic = upitem.IdentityCardPic;
                ppentity.HeadPic = upitem.HeadPic;
                ppentity.WechatId = upitem.WechatId;
                ppentity.LvId = upitem.LvId;
                ppentity.UserPid = UserId;
                ppentity.Type = upitem.Type;
                ppentity.AuthorizationTime = upitem.AuthorizationTime;
                ppentity.StarInviter = upitem.StarInviter;
                ppentity.Mobile = upitem.Mobile;
                ppentity.ShopName = upitem.ShopName;
                userbll.Update(ppentity, upitem.Id);
            }

            foreach (var upitem in Listthreeall)
            {
                AgentuserEntity ppentity = new AgentuserEntity();
                ppentity.Id = upitem.Id;
                ppentity.Name = upitem.Name;
                ppentity.LoginName = upitem.LoginName;
                ppentity.Password = upitem.Password;
                ppentity.Email = upitem.Email;
                ppentity.State = upitem.State;
                ppentity.ProvinceId = upitem.ProvinceId;
                ppentity.CityId = upitem.CityId;
                ppentity.DistrictId = upitem.DistrictId;
                ppentity.Address = upitem.Address;
                ppentity.Createtime = upitem.Createtime;
                ppentity.Inviter = upitem.Inviter;
                ppentity.AuthorizationNum = upitem.AuthorizationNum;
                ppentity.AuthorizationPic = upitem.AuthorizationPic;
                ppentity.IdentityCard = upitem.IdentityCard;
                ppentity.IdentityCardPic = upitem.IdentityCardPic;
                ppentity.HeadPic = upitem.HeadPic;
                ppentity.WechatId = upitem.WechatId;
                ppentity.LvId = upitem.LvId;
                ppentity.UserPid = UserId;
                ppentity.Type = upitem.Type;
                ppentity.AuthorizationTime = upitem.AuthorizationTime;
                ppentity.StarInviter = upitem.StarInviter;
                ppentity.Mobile = upitem.Mobile;
                ppentity.ShopName = upitem.ShopName;
                userbll.Update(ppentity, upitem.Id);
            }

            foreach (var upitem in PidListthreeall)
            {
                AgentuserEntity ppentity = new AgentuserEntity();
                ppentity.Id = upitem.Id;
                ppentity.Name = upitem.Name;
                ppentity.LoginName = upitem.LoginName;
                ppentity.Password = upitem.Password;
                ppentity.Email = upitem.Email;
                ppentity.State = upitem.State;
                ppentity.ProvinceId = upitem.ProvinceId;
                ppentity.CityId = upitem.CityId;
                ppentity.DistrictId = upitem.DistrictId;
                ppentity.Address = upitem.Address;
                ppentity.Createtime = upitem.Createtime;
                ppentity.Inviter = upitem.Inviter;
                ppentity.AuthorizationNum = upitem.AuthorizationNum;
                ppentity.AuthorizationPic = upitem.AuthorizationPic;
                ppentity.IdentityCard = upitem.IdentityCard;
                ppentity.IdentityCardPic = upitem.IdentityCardPic;
                ppentity.HeadPic = upitem.HeadPic;
                ppentity.WechatId = upitem.WechatId;
                ppentity.LvId = upitem.LvId;
                ppentity.UserPid = UserId;
                ppentity.Type = upitem.Type;
                ppentity.AuthorizationTime = upitem.AuthorizationTime;
                ppentity.StarInviter = upitem.StarInviter;
                ppentity.Mobile = upitem.Mobile;
                ppentity.ShopName = upitem.ShopName;
                userbll.Update(ppentity, upitem.Id);
            }
        }
        public void UpagentoneUserRefresh(int UserId, int LvId)
        {
            UpConditionBll Upbll = new UpConditionBll();
            AgentuserBll userbll = new AgentuserBll();
            AgentuserEntity userentity = userbll.selectById(UserId);
            AgentuserEntity upuserentity = new AgentuserEntity();
            AgentuserUptagBll uptagbll = new AgentuserUptagBll();

            int UUUserPid = userentity.UserPid;
            upuserentity.Id = UserId;
            upuserentity.Name = userentity.Name;
            upuserentity.LoginName = userentity.LoginName;
            upuserentity.Password = userentity.Password;
            upuserentity.Email = userentity.Email;
            upuserentity.State = userentity.State;
            upuserentity.ProvinceId = userentity.ProvinceId;
            upuserentity.CityId = userentity.CityId;
            upuserentity.DistrictId = userentity.DistrictId;
            upuserentity.Address = userentity.Address;
            upuserentity.Createtime = userentity.Createtime;
            upuserentity.Mobile = userentity.Mobile;
            upuserentity.AuthorizationNum = userentity.AuthorizationNum;
            upuserentity.IdentityCard = userentity.IdentityCard;
            upuserentity.IdentityCardPic = userentity.IdentityCardPic;
            upuserentity.HeadPic = userentity.HeadPic;
            upuserentity.WechatId = userentity.WechatId;
            upuserentity.AuthorizationTime = userentity.AuthorizationTime;
            upuserentity.Type = userentity.Type;
            upuserentity.StarInviter = userentity.StarInviter;
            upuserentity.ShopName = userentity.ShopName;

            string lvtworeewhere = "Inviter=" + UserId + " and LvId=2";

       

            List<AgentuserEntity> listtworeeUser = userbll.selectByWhere(lvtworeewhere);//所有自己邀请的大总
         

            List<AgentuserEntity> Listthreeall = AlllvThree(UserId);//自已邀请所有总代循环总代
            List<AgentuserEntity> PidListthreeall = PidAlllvThree(UserId);//下级总代邀请总代循环总代

            List<AgentuserEntity> alltwo = AlllvTwo(UserId);//邀请大总 循环大总

            List<AgentuserEntity> pidalltwo = PidAlllvTwo(UserId);//下级大总 循环大总

            foreach (var upitem in listtworeeUser)
            {
                AgentuserEntity ppentity = new AgentuserEntity();
                ppentity.Id = upitem.Id;
                ppentity.Name = upitem.Name;
                ppentity.LoginName = upitem.LoginName;
                ppentity.Password = upitem.Password;
                ppentity.Email = upitem.Email;
                ppentity.State = upitem.State;
                ppentity.ProvinceId = upitem.ProvinceId;
                ppentity.CityId = upitem.CityId;
                ppentity.DistrictId = upitem.DistrictId;
                ppentity.Address = upitem.Address;
                ppentity.Createtime = upitem.Createtime;
                ppentity.Inviter = upitem.Inviter;
                ppentity.AuthorizationNum = upitem.AuthorizationNum;
                ppentity.AuthorizationPic = upitem.AuthorizationPic;
                ppentity.IdentityCard = upitem.IdentityCard;
                ppentity.IdentityCardPic = upitem.IdentityCardPic;
                ppentity.HeadPic = upitem.HeadPic;
                ppentity.WechatId = upitem.WechatId;
                ppentity.LvId = upitem.LvId;
                ppentity.UserPid = UserId;
                ppentity.Type = upitem.Type;
                ppentity.AuthorizationTime = upitem.AuthorizationTime;
                ppentity.StarInviter = upitem.StarInviter;
                ppentity.Mobile = upitem.Mobile;
                ppentity.ShopName = upitem.ShopName;
                userbll.Update(ppentity, upitem.Id);
            }

            foreach (var upitem in Listthreeall)
            {
                AgentuserEntity ppentity = new AgentuserEntity();
                ppentity.Id = upitem.Id;
                ppentity.Name = upitem.Name;
                ppentity.LoginName = upitem.LoginName;
                ppentity.Password = upitem.Password;
                ppentity.Email = upitem.Email;
                ppentity.State = upitem.State;
                ppentity.ProvinceId = upitem.ProvinceId;
                ppentity.CityId = upitem.CityId;
                ppentity.DistrictId = upitem.DistrictId;
                ppentity.Address = upitem.Address;
                ppentity.Createtime = upitem.Createtime;
                ppentity.Inviter = upitem.Inviter;
                ppentity.AuthorizationNum = upitem.AuthorizationNum;
                ppentity.AuthorizationPic = upitem.AuthorizationPic;
                ppentity.IdentityCard = upitem.IdentityCard;
                ppentity.IdentityCardPic = upitem.IdentityCardPic;
                ppentity.HeadPic = upitem.HeadPic;
                ppentity.WechatId = upitem.WechatId;
                ppentity.LvId = upitem.LvId;
                ppentity.UserPid = UserId;
                ppentity.Type = upitem.Type;
                ppentity.AuthorizationTime = upitem.AuthorizationTime;
                ppentity.StarInviter = upitem.StarInviter;
                ppentity.Mobile = upitem.Mobile;
                ppentity.ShopName = upitem.ShopName;
                userbll.Update(ppentity, upitem.Id);
            }

            foreach (var upitem in PidListthreeall)
            {
                AgentuserEntity ppentity = new AgentuserEntity();
                ppentity.Id = upitem.Id;
                ppentity.Name = upitem.Name;
                ppentity.LoginName = upitem.LoginName;
                ppentity.Password = upitem.Password;
                ppentity.Email = upitem.Email;
                ppentity.State = upitem.State;
                ppentity.ProvinceId = upitem.ProvinceId;
                ppentity.CityId = upitem.CityId;
                ppentity.DistrictId = upitem.DistrictId;
                ppentity.Address = upitem.Address;
                ppentity.Createtime = upitem.Createtime;
                ppentity.Inviter = upitem.Inviter;
                ppentity.AuthorizationNum = upitem.AuthorizationNum;
                ppentity.AuthorizationPic = upitem.AuthorizationPic;
                ppentity.IdentityCard = upitem.IdentityCard;
                ppentity.IdentityCardPic = upitem.IdentityCardPic;
                ppentity.HeadPic = upitem.HeadPic;
                ppentity.WechatId = upitem.WechatId;
                ppentity.LvId = upitem.LvId;
                ppentity.UserPid = UserId;
                ppentity.Type = upitem.Type;
                ppentity.AuthorizationTime = upitem.AuthorizationTime;
                ppentity.StarInviter = upitem.StarInviter;
                ppentity.Mobile = upitem.Mobile;
                ppentity.ShopName = upitem.ShopName;
                userbll.Update(ppentity, upitem.Id);
            }

            foreach (var upitem in alltwo)
            {
                AgentuserEntity ppentity = new AgentuserEntity();
                ppentity.Id = upitem.Id;
                ppentity.Name = upitem.Name;
                ppentity.LoginName = upitem.LoginName;
                ppentity.Password = upitem.Password;
                ppentity.Email = upitem.Email;
                ppentity.State = upitem.State;
                ppentity.ProvinceId = upitem.ProvinceId;
                ppentity.CityId = upitem.CityId;
                ppentity.DistrictId = upitem.DistrictId;
                ppentity.Address = upitem.Address;
                ppentity.Createtime = upitem.Createtime;
                ppentity.Inviter = upitem.Inviter;
                ppentity.AuthorizationNum = upitem.AuthorizationNum;
                ppentity.AuthorizationPic = upitem.AuthorizationPic;
                ppentity.IdentityCard = upitem.IdentityCard;
                ppentity.IdentityCardPic = upitem.IdentityCardPic;
                ppentity.HeadPic = upitem.HeadPic;
                ppentity.WechatId = upitem.WechatId;
                ppentity.LvId = upitem.LvId;
                ppentity.UserPid = UserId;
                ppentity.Type = upitem.Type;
                ppentity.AuthorizationTime = upitem.AuthorizationTime;
                ppentity.StarInviter = upitem.StarInviter;
                ppentity.Mobile = upitem.Mobile;
                ppentity.ShopName = upitem.ShopName;
                userbll.Update(ppentity, upitem.Id);
            }

            foreach (var upitem in pidalltwo)
            {
                AgentuserEntity ppentity = new AgentuserEntity();
                ppentity.Id = upitem.Id;
                ppentity.Name = upitem.Name;
                ppentity.LoginName = upitem.LoginName;
                ppentity.Password = upitem.Password;
                ppentity.Email = upitem.Email;
                ppentity.State = upitem.State;
                ppentity.ProvinceId = upitem.ProvinceId;
                ppentity.CityId = upitem.CityId;
                ppentity.DistrictId = upitem.DistrictId;
                ppentity.Address = upitem.Address;
                ppentity.Createtime = upitem.Createtime;
                ppentity.Inviter = upitem.Inviter;
                ppentity.AuthorizationNum = upitem.AuthorizationNum;
                ppentity.AuthorizationPic = upitem.AuthorizationPic;
                ppentity.IdentityCard = upitem.IdentityCard;
                ppentity.IdentityCardPic = upitem.IdentityCardPic;
                ppentity.HeadPic = upitem.HeadPic;
                ppentity.WechatId = upitem.WechatId;
                ppentity.LvId = upitem.LvId;
                ppentity.UserPid = UserId;
                ppentity.Type = upitem.Type;
                ppentity.AuthorizationTime = upitem.AuthorizationTime;
                ppentity.StarInviter = upitem.StarInviter;
                ppentity.Mobile = upitem.Mobile;
                ppentity.ShopName = upitem.ShopName;
                userbll.Update(ppentity, upitem.Id);
            }
        }


        public class NumEntity
        {
            private int id;

            public int Id
            {
                get { return id; }
                set { id = value; }
            }
        }
        //寻找上级Id

        public int Query(int id, int lvid)
        {
            //id是自己
            AgentuserBll bll = new AgentuserBll();
            AgentuserEntity entity = new AgentuserEntity();
            entity = bll.selectById(id);
            int UserId = 0;
            if (entity.LvId < lvid)
            {
                UserId = id;

            }
            else if (entity.LvId == lvid)
            {

                UserId = entity.UserPid;

            }
            else
            {
                UserId = Query(entity.UserPid, lvid);
            }

            return UserId;

        }
        public int PUser(int id, int lvid)
        {
            AgentuserBll bll = new AgentuserBll();
            AgentuserEntity entity = new AgentuserEntity();
            entity = bll.selectById(id);
            int PUserid = 0;
            if (entity.LvId < lvid)
            {
                PUserid = entity.Id;
            }
            if (entity.LvId == lvid)
            {

                PUserid = entity.UserPid;

            }
            if (entity.LvId > lvid)
            {
                PUser(entity.UserPid, lvid);
            }

            return PUserid;
        }
        //寻找所有Id  总代
        public List<AgentuserEntity> AlllvTwo(int UserId)
        {
            AgentuserBll bll = new AgentuserBll();
            List<AgentuserEntity> ListAll = new List<AgentuserEntity>();
            List<AgentuserEntity> ListOne = new List<AgentuserEntity>();
            string lvthreewhere = "Inviter=" + UserId + " and LvId=2";
            ListOne = bll.selectByWhere(lvthreewhere);
            foreach (var item in ListOne)
            {
                ListAll.Add(item);
                PAlllvTwo(item.Id, ListAll);
            }
            return ListAll;
        }

        public List<AgentuserEntity> PidAlllvTwo(int UserId)
        {
            AgentuserBll bll = new AgentuserBll();
            List<AgentuserEntity> ListAll = new List<AgentuserEntity>();
            List<AgentuserEntity> ListOne = new List<AgentuserEntity>();
            string lvthreewhere = "UserPid=" + UserId + " and LvId=2";
            ListOne = bll.selectByWhere(lvthreewhere);
            foreach (var item in ListOne)
            {

                ListAll.Add(item);
                PAlllvTwo(item.Id, ListAll);
            }

            return ListAll;
        }

        public List<AgentuserEntity> PAlllvTwo(int UserId, List<AgentuserEntity> ListAll)
        {
            AgentuserBll bll = new AgentuserBll();

            List<AgentuserEntity> ListOne = new List<AgentuserEntity>();
            string lvthreewhere = "Inviter=" + UserId + " and LvId=2";
            ListOne = bll.selectByWhere(lvthreewhere);
            foreach (var item in ListOne)
            {

                ListAll.Add(item);
                PAlllvTwo(item.Id, ListAll);
            }

            return ListAll;
        }


        //寻找所有Id  总代
        public List<AgentuserEntity> AlllvThree(int UserId)
        {
            AgentuserBll bll = new AgentuserBll();
            List<AgentuserEntity> ListAll = new List<AgentuserEntity>();
            List<AgentuserEntity> ListOne = new List<AgentuserEntity>();
            string lvthreewhere = "Inviter=" + UserId + " and LvId=3";
            ListOne = bll.selectByWhere(lvthreewhere);
            foreach(var item in ListOne)
            {

                ListAll.Add(item);
                PAlllvThree(item.Id, ListAll);
            }

            return ListAll;
        }

        public List<AgentuserEntity> PidAlllvThree(int UserId)
        {
            AgentuserBll bll = new AgentuserBll();
            List<AgentuserEntity> ListAll = new List<AgentuserEntity>();
            List<AgentuserEntity> ListOne = new List<AgentuserEntity>();
            string lvthreewhere = "UserPid=" + UserId + " and LvId=3";
            ListOne = bll.selectByWhere(lvthreewhere);
            foreach (var item in ListOne)
            {

                ListAll.Add(item);
                PAlllvThree(item.Id, ListAll);
            }

            return ListAll;
        }

        public List<AgentuserEntity> PAlllvThree(int UserId, List<AgentuserEntity> ListAll)
        {
            AgentuserBll bll = new AgentuserBll();
         
            List<AgentuserEntity> ListOne = new List<AgentuserEntity>();
            string lvthreewhere = "Inviter=" + UserId + " and LvId=3";
            ListOne = bll.selectByWhere(lvthreewhere);
            foreach (var item in ListOne)
            {

                ListAll.Add(item);
                PAlllvThree(item.Id, ListAll);
            }

            return ListAll;
        }



    

        public int AllUserTwoandOne(int UserId)
        {
            int Num = 0;
            List<AgentuserEntity> UserList = StarInviterId(UserId);
            foreach (var item in UserList)
            {
                if (item.LvId <= 2)
                {
                    Num++;
                }
            }
            return Num;
        }
        public List<AgentuserEntity> StarInviterId(int StarInviter)
        {
            //第一步 寻找自己邀请的所有代理
            string whereStarInviter = " StarInviter=" + "'" + StarInviter + "'";
            AgentuserBll Bll = new AgentuserBll();
            List<AgentuserEntity> listUserEntity = Bll.selectByWhere(whereStarInviter);//自己邀请的所有代理 包含小代理
            List<AgentuserEntity> AlllistUserEntity = new List<AgentuserEntity>();
            foreach (var item in listUserEntity)
            {
                if (item.LvId>1)
                {
                    AlllistUserEntity.Add(item);
                    SStarInviterId(item.Id, AlllistUserEntity);
                }
                else
                {
                    AlllistUserEntity.Add(item);
                }
              
            }
            //第二步 寻找自己邀请代理 所发展的代理（总代和总代以上）
            return AlllistUserEntity;
        }
        public List<AgentuserEntity> SStarInviterId(int StarInviter, List<AgentuserEntity> AlllistUserEntity)
        {
            string whereStarInviter = " StarInviter=" + "'" + StarInviter + "'";
            AgentuserBll Bll = new AgentuserBll();
            List<AgentuserEntity> listUserEntity = Bll.selectByWhere(whereStarInviter);
            foreach (var item in listUserEntity)
            {
                if (item.LvId >1)
                {
                    AlllistUserEntity.Add(item);
                    SStarInviterId(item.Id, AlllistUserEntity);
                }
                else
                {
                    AlllistUserEntity.Add(item);
                }
            }
            return AlllistUserEntity;
        }

        //所有相关的大总 列表
        public List<AgentuserEntity> AllUserTwoandOneEntity(int UserId)
        {
            
            List<AgentuserEntity> UserList = StarInviterIdEntity(UserId);
            List<AgentuserEntity> ListEntity = new List<AgentuserEntity>();
            foreach (var item in UserList)
            {
                if (item.LvId == 2)
                {
                    ListEntity.Add(item);
                }
            }
            return ListEntity;
        }

        // 与我有关系 所大总
        public List<AgentuserEntity> StarInviterIdEntity(int StarInviter)
        {
            //第一步 寻找自己邀请的所有代理
            string whereStarInviter = " State=1 and StarInviter=" + "'" + StarInviter + "'";
            AgentuserBll Bll = new AgentuserBll();
            List<AgentuserEntity> listUserEntity = Bll.selectByWhere(whereStarInviter);//自己邀请的所有代理 包含小代理
            List<AgentuserEntity> AlllistUserEntity = new List<AgentuserEntity>();
            foreach (var item in listUserEntity)
            {
                if (item.LvId > 1)
                {
                    AlllistUserEntity.Add(item);
                    SStarInviterIdEntity(item.Id, AlllistUserEntity);
                }
            }

            //第二步 寻找自己邀请代理 所发展的代理（总代和总代以上）

            return AlllistUserEntity;
        }
        public List<AgentuserEntity> SStarInviterIdEntity(int StarInviter, List<AgentuserEntity> AlllistUserEntity)
        {

            string whereStarInviter = " State=1 and StarInviter=" + "'" + StarInviter + "'";
            AgentuserBll Bll = new AgentuserBll();
            List<AgentuserEntity> listUserEntity = Bll.selectByWhere(whereStarInviter);
            foreach (var item in listUserEntity)
            {
                if (item.LvId > 1)
                {
                    AlllistUserEntity.Add(item);
                    SStarInviterIdEntity(item.Id, AlllistUserEntity);
                }
            }


            return AlllistUserEntity;
        }
    }
}
