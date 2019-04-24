using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WbOsWeb.Bll;
using WbOsWeb.Entity;
namespace WbOsWeb.Upagent
{
    public class UpagentsToEntity
    {
        /// <summary>
        /// 是否满足升级条件
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="LvId"></param>
        /// <returns></returns>
        public int judgeUpagentUser(int UserId, int LvId)
        {
            UpConditionBll Upbll = new UpConditionBll();
            AgentuserBll userbll = new AgentuserBll();
            AgentuserEntity userentity = userbll.selectById(UserId);
            AgentuserEntity upuserentity = new AgentuserEntity();
            AgentuserUptagBll uptagbll = new AgentuserUptagBll();
            UserlvBll lvbll = new UserlvBll();
            UpAgentsEntity TableEntity = new UpAgentsEntity();
            int YeworNo = 0;
            if (LvId > 3)
            {
                UserBalanceBll balbll = new UserBalanceBll();
                UserBalanceEntity blentity = balbll.selectById(UserId);
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
                        YeworNo = 1;
                    }
                }
            }
            if (LvId == 3)
            {
                UpConditionEntity upEntity = new UpConditionEntity();
                int Upid = LvId - 1;
                upEntity = Upbll.selectById(Upid);
                int Upthree = upEntity.Upthree;
                string lvthreewhere = "Inviter=" + UserId + " and LvId=3 and State=1";
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

                        YeworNo = 1;
                    }
                }

            }

            if (LvId == 2)
            {

                UpConditionEntity upEntity = new UpConditionEntity();
                int Upid = LvId - 1;
                upEntity = Upbll.selectById(Upid);
                int Upthree = upEntity.Upthree;//总代个数 条件
                int Uptwo = upEntity.Uptwo;//大总个数 条件
                string lvthreewhere = "Inviter=" + UserId + " and LvId=3 and State=1";
                string twowhere = "Inviter=" + UserId + " and LvId=2 and State=1";
                string otherwhere = "StarInviter=" + UserId + " and LvId=1 and State=1";
                List<AgentuserEntity> listthreeUser = userbll.selectByWhere(lvthreewhere); //直线总代个数
                List<AgentuserEntity> listtwoUser = userbll.selectByWhere(twowhere);//直线大总个数

                List<AgentuserEntity> listotherUser = userbll.selectByWhere(otherwhere);//直线邀请人  已经升级到大牌

                List<AgentuserEntity> Listtwoall = AlllvTwo(UserId);
                int AllListtwoal = AllUserTwoandOne(UserId);

                if (listthreeUser != null && listtwoUser != null)
                {
                    int UserthreeNum = 0; //直线总代个数
                    foreach (var itemtree in listthreeUser)
                    {
                        UserthreeNum++;
                    }

                    int UsertwoNum = 0;//已经升级到大总个数
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
                        YeworNo = 1;
                    }
                    if (AllListtwoal >= Uptwo && UserthreeNum + UserotherNum + UsertwoNum >= Upthree)
                    {
                        YeworNo = 1;
                    }
                }

            }
            return YeworNo;
        }
        /// <summary>
        /// 把升级信息写入数据库
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="LvId"></param>
        /// <returns></returns>
        public UpAgentsEntity UpagentUserEntity(int UserId, int LvId)
        {
            UpConditionBll Upbll = new UpConditionBll();
            AgentuserBll userbll = new AgentuserBll();
            AgentuserEntity userentity = userbll.selectById(UserId);
            UserBalanceBll balbll = new UserBalanceBll();
            
            AgentuserUptagBll uptagbll = new AgentuserUptagBll();
            UserlvBll lvbll = new UserlvBll();
            UpAgentsEntity TableEntity = new UpAgentsEntity();
            if (LvId > 3)
            {
                int Uppid = 3;
                UserBalanceEntity blentity = balbll.selectById(UserId);
                UpConditionEntity upthreeEntity = new UpConditionEntity();
                upthreeEntity = Upbll.selectById(Uppid);
                double PayMoney = upthreeEntity.PayMoney;
                if (blentity != null)
                {
                    double BalancePrice = blentity.BalancePrice;
                    int ReNum = blentity.ReNum;
                    if (BalancePrice >= PayMoney)
                    {
                    
                        int UserPid = Query(UserId, Uppid);
                        UserlvEntity nowlvEntity = lvbll.selectById(LvId);
                        UserlvEntity pulvEntity = lvbll.selectById(Uppid);
                        TableEntity.LvName = nowlvEntity.Name;
                        TableEntity.UpingName = pulvEntity.Name;
                        TableEntity.Name = userentity.Name;
                        TableEntity.Mobile = userentity.Mobile;
                        TableEntity.UserId = UserId;
                        TableEntity.UpingLvid = Uppid;

                        AgentReupEntityUserId(UserId,userentity.Inviter, UserPid);
                    }
                }
             
            }

            if (LvId == 3)
            {              
                int Upid = LvId - 1;
                int UserPid = Query(UserId, Upid);              
                UserlvEntity nowlvEntity = lvbll.selectById(LvId);
                UserlvEntity pulvEntity = lvbll.selectById(Upid);              
                TableEntity.LvName = nowlvEntity.Name;
                TableEntity.UpingName = pulvEntity.Name;
                TableEntity.Name = userentity.Name;
                TableEntity.Mobile = userentity.Mobile;
                TableEntity.UserId = UserId;
                TableEntity.UpingLvid = Upid;
            }
            if (LvId == 2)
            {
                int Upid = LvId - 1;        
                UserlvEntity nowlvEntity = lvbll.selectById(LvId);
                UserlvEntity pulvEntity = lvbll.selectById(Upid);           
                TableEntity.LvName = nowlvEntity.Name;
                TableEntity.UpingName = pulvEntity.Name;
                TableEntity.Name = userentity.Name;
                TableEntity.Mobile = userentity.Mobile;
                TableEntity.UserId = UserId;
                TableEntity.UpingLvid = Upid;   
            }
            return TableEntity;
        }

        public void AgentReupEntityUserId(int UserId, int Inviter, int UserPid)
        {

            AgentuserBll bll = new AgentuserBll();
            AgentuserEntity entity = bll.selectById(UserId);
            AgentReupBll Reupbll = new AgentReupBll();

            AgentReupEntity renupEntity = new AgentReupEntity();
            renupEntity.InviterId = Inviter;
            AgentuserEntity InviterEntity = bll.selectById(Inviter);//上级信息
            renupEntity.InviterLvId = InviterEntity.LvId;
            renupEntity.UserId = entity.Id;
            renupEntity.ThreeId = entity.Id;

            AgentuserEntity PidEntity = bll.selectById(UserPid);//上级信息
            if (PidEntity.LvId == 2)//如果上级是大总
            {
                renupEntity.TwoId = UserPid;//大总
                renupEntity.OneId = PidEntity.UserPid;//大牌
                AgentuserEntity PPidEntity = bll.selectById(PidEntity.UserPid);
                renupEntity.InoneId = PPidEntity.Inviter;
            }
            if (PidEntity.LvId == 1)// 如果上级是大牌
            {
                renupEntity.TwoId = 0;//大总
                renupEntity.OneId = UserPid;//大牌              
                renupEntity.InoneId = PidEntity.Inviter;
            }
            Reupbll.add(renupEntity);

        }

        public int Query(int id, int lvid)
        {
            AgentuserBll bll = new AgentuserBll();
            AgentuserEntity entity = new AgentuserEntity();
            entity = bll.selectById(id);
            int UserId = 0;
            if (entity.LvId < lvid)
            {
                UserId = id;

            }else  if (entity.LvId == lvid)
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
            else if (entity.LvId == lvid)
            {

                PUserid = entity.UserPid;

            }
            else
            {
                PUser(entity.UserPid, lvid);
            }

            return PUserid;
        }


        public List<AgentuserEntity> AlllvTwo(int UserId)
        {
            AgentuserBll bll = new AgentuserBll();
            List<AgentuserEntity> ListAll = new List<AgentuserEntity>();
            List<AgentuserEntity> ListOne = new List<AgentuserEntity>();
            string lvthreewhere = "Inviter=" + UserId + " and LvId=2 and State=1";
            ListOne = bll.selectByWhere(lvthreewhere);
            foreach (var item in ListOne)
            {

                ListAll.Add(item);
                PAlllvThree(item.Id, ListAll);
            }

            return ListAll;
        }

        public List<AgentuserEntity> PAlllvTwo(int UserId, List<AgentuserEntity> ListAll)
        {
            AgentuserBll bll = new AgentuserBll();

            List<AgentuserEntity> ListOne = new List<AgentuserEntity>();
            string lvthreewhere = "Inviter=" + UserId + " and LvId=2 and State=1";
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
            string lvthreewhere = "Inviter=" + UserId + " and LvId=3 and State=1";
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
            string lvthreewhere = "Inviter=" + UserId + " and LvId=3 and State=1";
            ListOne = bll.selectByWhere(lvthreewhere);
            foreach (var item in ListOne)
            {

                ListAll.Add(item);
                PAlllvThree(item.Id, ListAll);
            }

            return ListAll;
        }

        

        //统计有关代理 大总和大牌得个数 作为代理升级条件
        public int AllUserTwoandOne(int UserId)
        {
            int Num=0;
            List<AgentuserEntity> UserList = StarInviterId(UserId);
            foreach (var item in UserList)
            {
                if (item.LvId<=2)
                {
                    Num++;
                }
            }
            return Num;       
        }
        //找到所有与自己有关的代理
        public List<AgentuserEntity> StarInviterId(int StarInviter)
        {
            //第一步 寻找自己邀请的所有代理
            string whereStarInviter = " State=1 and StarInviter=" + "'"+StarInviter+"'"; 
            AgentuserBll Bll = new AgentuserBll();
            List<AgentuserEntity> listUserEntity = Bll.selectByWhere(whereStarInviter);//自己邀请的所有代理 包含小代理
            List<AgentuserEntity> AlllistUserEntity = new List<AgentuserEntity>();
            foreach (var item in listUserEntity)
            {
                if (item.LvId >1)
                {
                    AlllistUserEntity.Add(item);
                    SStarInviterId(item.Id, AlllistUserEntity);//自己邀请的所有代理 包含小代理 他们要求的所有代理包含小代理
                }
                else
                {
                    AlllistUserEntity.Add(item);
                }                              
            }
       
            return AlllistUserEntity;
        }
        public List<AgentuserEntity> SStarInviterId(int StarInviter, List<AgentuserEntity> AlllistUserEntity)
        {
            
            string whereStarInviter = " State=1 and StarInviter=" + "'" + StarInviter + "'"; 
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
                    AlllistUserEntity.Add(item);//自己邀请的代理 下面的代理  当时大牌得时候记录  升级条件  但是不在进行遍历查询 
                }
            }
          

            return AlllistUserEntity;
        }

    }
}
