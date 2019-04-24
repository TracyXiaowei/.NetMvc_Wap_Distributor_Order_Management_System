using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WbOsWeb.Bll;
using WbOsWeb.Entity;

namespace WbOsWeb.Upagent
{
    public class OrdersAgent
    {
        public void OrderUser(string orderId,int UserId,int OrderLvId)
        {
            AgentuserBll userBll = new AgentuserBll();
            OrdersAgentsBll bll = new OrdersAgentsBll();
            OrdersAgentsEntity OAEntity = new OrdersAgentsEntity();
           
            OAEntity.OrderId = orderId;
            OAEntity.UserId = UserId;
            int ThreeId = 0;
            int TwoId = 0;
            int OneId = 0;
            int InOneId = 0;
            if (OrderLvId==3)
            {
                AgentuserEntity UserEntity = userBll.selectById(UserId);
                int UserPid = UserEntity.UserPid;
                AgentuserEntity PidEntity = userBll.selectById(UserPid);
                int PidLv = PidEntity.LvId;
                if (PidLv==2)//如果上级是大总
                {
                    ThreeId = UserId;//总代
                    TwoId = UserPid;//大总
                    OneId = PidEntity.UserPid;//大牌
                    AgentuserEntity PPidEntity = userBll.selectById(PidEntity.UserPid);//大牌信息
                    InOneId = PPidEntity.Inviter;//大牌邀请人
                }
                else//如果上级是大牌
                {
                    ThreeId = UserId;
                    TwoId = 0;
                    OneId = UserPid;//大牌
                    InOneId = PidEntity.Inviter;//大牌邀请人
                }
            }
            if (OrderLvId == 2)
            {
                AgentuserEntity UserEntity = userBll.selectById(UserId);
                int UserPid = UserEntity.UserPid;
                AgentuserEntity PidEntity = userBll.selectById(UserPid);
                ThreeId = 0 ;
                TwoId = UserId;
                OneId = UserPid;//大牌
                InOneId = PidEntity.Inviter;//大牌邀请人

            }
            if (OrderLvId == 1)
            {
                AgentuserEntity UserEntity = userBll.selectById(UserId);
                ThreeId = 0;
                TwoId = 0;
                OneId = UserId;
                InOneId = UserEntity.Inviter;
            }
            OAEntity.ThreeId = ThreeId;
            OAEntity.TwoId = TwoId;
            OAEntity.OneId = OneId;
            OAEntity.InOneId = InOneId;

            bll.add(OAEntity);
        }
    }
}
