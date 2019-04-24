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
    public class DowngradeVerification
    {
        public void DowngradeVerifyEntity(int UserId)
        {

            AgentuserBll userbll = new AgentuserBll();
            AgentuserEntity userentity = userbll.selectById(UserId);
            int LvId = userentity.LvId;
            if (LvId <= 2)
            {
                int YesOrNo = DowngradeVerify(UserId, LvId);
                //添加降级列表  进入降级列表里面
                if (YesOrNo == 0)
                {
                    DowngradeAgentsBll downBll = new DowngradeAgentsBll();
                    DowngradeAgentsEntity Yesentity = downBll.selectByUserId(UserId);
                    int NowLvId = LvId;
                    int DowngradeLvId = LvId + 1;
                    if (Yesentity == null)
                    {
                        DowngradeAgentsEntity entity = new DowngradeAgentsEntity();
                        UserlvBll lvBll = new UserlvBll();
                        UserlvEntity NowLvEntity = lvBll.selectById(NowLvId);
                        UserlvEntity DowngradeLvEntity = lvBll.selectById(DowngradeLvId);
                        entity.Name = userentity.Name;
                        entity.UserId = UserId;
                        entity.Mobile = userentity.LoginName;
                        entity.NowLvId = NowLvId;
                        entity.NowLvName = NowLvEntity.Name;
                        entity.DowngradeLvId = DowngradeLvId;
                        entity.DowngradeLvName = DowngradeLvEntity.Name;
                        downBll.add(entity);
                    }
                }
            }
        }

        /// <summary>
        /// 判断是否符合当前的等级条件  如果是0  说明不满足
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="LvId">当前等级Id</param>
        /// <returns></returns>
        private int DowngradeVerify(int UserId, int LvId)
        {
            UpConditionBll Upbll = new UpConditionBll();
            AgentuserBll userbll = new AgentuserBll();
            AgentuserEntity userentity = userbll.selectById(UserId);
            int YeworNo = 0;
            if (LvId == 2)
            {
                UpConditionEntity upEntity = new UpConditionEntity();
                upEntity = Upbll.selectById(LvId);
                int Upthree = upEntity.Upthree;
                string threewhere = "Inviter=" + UserId + " and LvId=3 and State=1";
                List<AgentuserEntity> listUser = userbll.selectByWhere(threewhere);//直接邀请的人数 总代人数
                string otherwhere = "StarInviter=" + UserId + " and LvId<3 and State=1"; //自己邀请 且等级在总代以上的人员
                List<AgentuserEntity> listotherUser = userbll.selectByWhere(otherwhere);
                int UsrtNum = listUser.Count;
                int UserotherNum = listotherUser.Count;
                int UsrtNumTotal = UsrtNum + UserotherNum;
                if (UsrtNumTotal >= Upthree)
                {
                    YeworNo = 1;
                }
            }

            if (LvId == 1)
            {

                UpConditionEntity upEntity = new UpConditionEntity();
                upEntity = Upbll.selectById(LvId);
                int Upthree = upEntity.Upthree;//总代个数 条件
                int Uptwo = upEntity.Uptwo;//大总个数 条件
                string threewhere = "Inviter=" + UserId + " and LvId=3 and State=1";
                string twowhere = "Inviter=" + UserId + " and LvId=2 and State=1";
                string otherwhere = "StarInviter=" + UserId + " and LvId=1 and State=1";

                List<AgentuserEntity> listthreeUser = userbll.selectByWhere(threewhere); //直线总代个数
                List<AgentuserEntity> listtwoUser = userbll.selectByWhere(twowhere);//直线大总个数

                List<AgentuserEntity> listotherUser = userbll.selectByWhere(otherwhere);//已经升级到大牌
                int TotalOneandtwo = TotalUserOneandtwo(UserId);
                int UserthreeNum = listthreeUser.Count;//直线总代
                int UsertwoNum = listtwoUser.Count;//直线大总
                int UserotherNum = listotherUser.Count;//自己邀请  且是大牌
                int AllUptwo = UserotherNum + UsertwoNum; //自己升级到大总和大牌总和


                if (AllUptwo > 0 && UserthreeNum + UserotherNum + UsertwoNum >= Upthree + Uptwo)
                {
                    YeworNo = 1;
                }
                if (TotalOneandtwo >= Uptwo && UserthreeNum + UserotherNum + UsertwoNum >= Upthree)
                {
                    YeworNo = 1;
                }
            }
            return YeworNo;
        }


        private List<AgentuserEntity> PAlllvThree(int UserId, List<AgentuserEntity> ListAll)
        {
            AgentuserBll bll = new AgentuserBll();

            List<AgentuserEntity> ListOne = new List<AgentuserEntity>();
            string lvthreewhere = "Inviter=" + UserId + " and LvId=3 and State=1";
            ListOne = bll.selectByWhere(lvthreewhere);
            foreach (var item in ListOne)
            {

                ListAll.Add(item);
                PAlllvThree(item.Id, ListAll);
            }

            return ListAll;
        }

        private int TotalUserOneandtwo(int UserId)
        {
            int Num = 0;
            List<AgentuserEntity> UserList = StarInviterListEntity(UserId);
            foreach (var item in UserList)
            {
                if (item.LvId <= 2)
                {
                    Num++;
                }
            }
            return Num;
        }

        //整理逻辑 1、与我有关系的代理   如果代理是大牌  满足条件 +1 但是不能进行遍历  直下的是人就与自己没有关系
        //2、大总及以下进行遍历  
        private List<AgentuserEntity> StarInviterListEntity(int StarInviter)
        {
            AgentuserBll Bll = new AgentuserBll();
            string whereStarInviter = " State=1 and StarInviter=" + "'" + StarInviter + "'";
            List<AgentuserEntity> listUserEntity = Bll.selectByWhere(whereStarInviter);//自己邀请所有的代理      
            List<AgentuserEntity> MyUserList = new List<AgentuserEntity>();
            foreach (var item in listUserEntity)
            {
                if (item.LvId > 1)
                {
                    MyUserList.Add(item);
                    StarInviterIdingListEntity(item.Id, MyUserList);
                }
                else
                {
                    MyUserList.Add(item);//大牌
                }
            }

            return MyUserList;
        }

        private List<AgentuserEntity> StarInviterIdingListEntity(int StarInviter, List<AgentuserEntity> MyUserList)
        {

            string whereStarInviter = " State=1 and StarInviter=" + "'" + StarInviter + "'";
            AgentuserBll Bll = new AgentuserBll();
            List<AgentuserEntity> listUserEntity = Bll.selectByWhere(whereStarInviter);
            foreach (var item in listUserEntity)
            {
                if (item.LvId > 1)
                {
                    MyUserList.Add(item);
                    StarInviterIdingListEntity(item.Id, MyUserList);
                }
                else
                {
                    MyUserList.Add(item); //大牌
                }
            }
            return MyUserList;
        }
    }


    public class DowngradeUserEntity
    {
        public void DowngradeVerify(int UserId, int NowLvId,int DowngradeLvId)
        {
            AgentuserBll userbll = new AgentuserBll();
            AgentuserEntity userentity = userbll.selectById(UserId);
            AgentuserEntity UpEntity = new AgentuserEntity();

            UpEntity.Id = userentity.Id;
            UpEntity.Name = userentity.Name;
            UpEntity.LoginName = userentity.LoginName;
            UpEntity.Password = userentity.Password;
            UpEntity.Email = userentity.Email;
            UpEntity.State = userentity.State;
            UpEntity.ProvinceId = userentity.ProvinceId;
            UpEntity.CityId = userentity.CityId;
            UpEntity.DistrictId = userentity.DistrictId;
            UpEntity.Address = userentity.Address;
            UpEntity.Createtime = userentity.Createtime;
            UpEntity.Inviter = userentity.Inviter;
            UpEntity.AuthorizationNum = userentity.AuthorizationNum;
       
            UpEntity.IdentityCard = userentity.IdentityCard;
            UpEntity.IdentityCardPic = userentity.IdentityCardPic;
            UpEntity.HeadPic = userentity.HeadPic;
            UpEntity.WechatId = userentity.WechatId;
            UpEntity.LvId = DowngradeLvId;//降级后的等级
       
            UpEntity.Type = userentity.Type;
            UpEntity.AuthorizationTime = userentity.AuthorizationTime;
            UpEntity.StarInviter = userentity.StarInviter;
            UpEntity.Mobile = userentity.Mobile;
            UpEntity.ShopName = userentity.ShopName;

            if (NowLvId == 1) //现在是大牌  降级为大总
            {
                int UserPid = userentity.Inviter;//原来的大牌的邀请人 就是降级后的上级
                //1.自己旗下的总代 包括间接总代 都不用变
                //2.自己旗下的大总  大总的上级 全部变成UserPid  其他信息不变
                //3.自己明一下的大牌 邀请人变成 UserPid; 
                //1.查询自己旗下的大总（条件） "UserPid=UserId and LvId=2"
                string MylvTwo = "LvId=2 and State=1 and UserPid=" + "'" + UserId + "'"; //旗下所有大总  邀请人不变             
                string MylvOne = "Inviter=" + UserId + " and LvId=1 and State=1";// 旗下大牌 
                List<AgentuserEntity> MyTwoListEntity = userbll.selectByWhere(MylvTwo);
                List<AgentuserEntity> MyOneListEntity = userbll.selectByWhere(MylvOne);

                foreach (var item in MyTwoListEntity)
                {
                    AgentuserEntity mytwoentity = new AgentuserEntity();
                    mytwoentity.Id = item.Id;
                    mytwoentity.Name = item.Name;
                    mytwoentity.LoginName = item.LoginName;
                    mytwoentity.Password = item.Password;
                    mytwoentity.Email = item.Email;
                    mytwoentity.State = item.State;
                    mytwoentity.ProvinceId = item.ProvinceId;
                    mytwoentity.CityId = item.CityId;
                    mytwoentity.DistrictId = item.DistrictId;
                    mytwoentity.Address = item.Address;
                    mytwoentity.Createtime = item.Createtime;
                    mytwoentity.Inviter = item.Inviter;
                    mytwoentity.AuthorizationNum = item.AuthorizationNum;
                    mytwoentity.AuthorizationPic = item.AuthorizationPic;
                    mytwoentity.IdentityCard = item.IdentityCard;
                    mytwoentity.IdentityCardPic = item.IdentityCardPic;
                    mytwoentity.HeadPic = item.HeadPic;
                    mytwoentity.WechatId = item.WechatId;
                    mytwoentity.LvId = item.LvId;
                    mytwoentity.UserPid = UserPid;
                    mytwoentity.Type = item.Type;
                    mytwoentity.AuthorizationTime = item.AuthorizationTime;
                    mytwoentity.StarInviter = item.StarInviter;
                    mytwoentity.Mobile = item.Mobile;
                    mytwoentity.ShopName = item.ShopName;
                    userbll.Update(mytwoentity, item.Id);
                }

                foreach (var item in MyOneListEntity)
                {
                    AgentuserEntity myoneentity = new AgentuserEntity();
                    myoneentity.Id = item.Id;
                    myoneentity.Name = item.Name;
                    myoneentity.LoginName = item.LoginName;
                    myoneentity.Password = item.Password;
                    myoneentity.Email = item.Email;
                    myoneentity.State = item.State;
                    myoneentity.ProvinceId = item.ProvinceId;
                    myoneentity.CityId = item.CityId;
                    myoneentity.DistrictId = item.DistrictId;
                    myoneentity.Address = item.Address;
                    myoneentity.Createtime = item.Createtime;
                    myoneentity.Inviter = UserPid;
                    myoneentity.AuthorizationNum = item.AuthorizationNum;
                    myoneentity.AuthorizationPic = item.AuthorizationPic;
                    myoneentity.IdentityCard = item.IdentityCard;
                    myoneentity.IdentityCardPic = item.IdentityCardPic;
                    myoneentity.HeadPic = item.HeadPic;
                    myoneentity.WechatId = item.WechatId;
                    myoneentity.LvId = item.LvId;
                    myoneentity.UserPid = item.UserPid;
                    myoneentity.Type = item.Type;
                    myoneentity.AuthorizationTime = item.AuthorizationTime;
                    myoneentity.StarInviter = item.StarInviter;
                    myoneentity.Mobile = item.Mobile;
                    myoneentity.ShopName = item.ShopName;
                    userbll.Update(myoneentity, item.Id);
                }

                 UpEntity.UserPid = UserPid;//降级后的上级
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
                Bitmap bitmap = new Bitmap(image, image.Width, image.Height);
                bitmap.Save(savePath, ImageFormat.Jpeg);//保存图片：新路径                                                        //****************这里是处理授权图片的操作结束***************//
                UpEntity.AuthorizationPic = filesavePathName;              
                UserlvBll lvbll = new UserlvBll();
                UserlvEntity lventity = lvbll.selectById(DowngradeLvId);
                string time = DateTime.Now.ToString("D");
                string atime = DateTime.Now.AddYears(1).ToString("D");
                string strTime = time + " 至 " + atime;
                UpEntity.AuthorizationTime = strTime;
                string LvName = lventity.Name;

                string Time = DateTime.Now.ToString("yyyy年MM月dd日");
                AuthorizationText Addtext = new AuthorizationText();
                Addtext.AuthorizationRefreshAddText(filesavePathName, userentity.Name, userentity.Mobile, userentity.AuthorizationNum, userentity.IdentityCard, userentity.WechatId, LvName, Time);

                // AuthorizationRefreshAddTestToImg(filesavePathName, userentity.Name, userentity.LoginName, userentity.AuthorizationNum, userentity.IdentityCard, userentity.WechatId, LvName);

                userbll.Update(UpEntity, UserId);

            }

            //逻辑分析  1.降级之前是大总   降级的部分 

            //1、原来大总 名下的  总代转移给上级（大牌）
            //2、原来的大总邀请的大总  本来只是自己的邀请 上级并不是自己 所以并不需要进行改变
            //3、总结：只需要转移旗下的总代  把总代的上级 转移给上级  至于小代理 本来就是自己
            //  旗下的 所有不发生任何变化  间接发展的总代  其实也通过我的下级查询已经转移了 所有
            //  只需把旗下总代的上级Id由自己变成为我原来的上级；



            if (NowLvId == 2) //现在是大总  降级为总代
            {
                int UserPid = userentity.UserPid;

                string MyallSqlWhere = "LvId=3 and State=1 and UserPid=" + "'" + UserId + "'"; //旗下所有大总  邀请人不变       
                List<AgentuserEntity> MyListEntity = userbll.selectByWhere(MyallSqlWhere);
                foreach (var item in MyListEntity)
                {
                    AgentuserEntity myentity = new AgentuserEntity();
                    myentity.Id = item.Id;
                    myentity.Name = item.Name;
                    myentity.LoginName = item.LoginName;
                    myentity.Password = item.Password;
                    myentity.Email = item.Email;
                    myentity.State = item.State;
                    myentity.ProvinceId = item.ProvinceId;
                    myentity.CityId = item.CityId;
                    myentity.DistrictId = item.DistrictId;
                    myentity.Address = item.Address;
                    myentity.Createtime = item.Createtime;
                    myentity.Inviter = item.Inviter;
                    myentity.AuthorizationNum = item.AuthorizationNum;
                    myentity.AuthorizationPic = item.AuthorizationPic;
                    myentity.IdentityCard = item.IdentityCard;
                    myentity.IdentityCardPic = item.IdentityCardPic;
                    myentity.HeadPic = item.HeadPic;
                    myentity.WechatId = item.WechatId;
                    myentity.LvId = item.LvId;
                    myentity.UserPid = UserPid;
                    myentity.Type = item.Type;
                    myentity.AuthorizationTime = item.AuthorizationTime;
                    myentity.StarInviter = item.StarInviter;
                    myentity.Mobile = item.Mobile;
                    myentity.ShopName = item.ShopName;
                    userbll.Update(myentity, item.Id);
                }
                UpEntity.UserPid = UserPid;//降级后的上级
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
                Bitmap bitmap = new Bitmap(image, image.Width, image.Height);
                bitmap.Save(savePath, ImageFormat.Jpeg);//保存图片：新路径                                                        //****************这里是处理授权图片的操作结束***************//
                UpEntity.AuthorizationPic = filesavePathName;
                UserlvBll lvbll = new UserlvBll();
                UserlvEntity lventity = lvbll.selectById(DowngradeLvId);
                string time = DateTime.Now.ToString("D");
                string atime = DateTime.Now.AddYears(1).ToString("D");
                string strTime = time + " 至 " + atime;
                UpEntity.AuthorizationTime = strTime;
                string LvName = lventity.Name;

                string Time = DateTime.Now.ToString("yyyy年MM月dd日");
                AuthorizationText Addtext = new AuthorizationText();
                Addtext.AuthorizationRefreshAddText(filesavePathName, userentity.Name, userentity.Mobile, userentity.AuthorizationNum, userentity.IdentityCard, userentity.WechatId, LvName, Time);


                userbll.Update(UpEntity, UserId);

            }

        }




    }
}
