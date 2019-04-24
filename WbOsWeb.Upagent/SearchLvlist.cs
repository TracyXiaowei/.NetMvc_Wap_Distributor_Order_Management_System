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

namespace WbOsWeb.Upagent
{
    public class SearchLvlist
    {
        public LvUserIdEntity SearchLvlistEntity(int UserId)
        {
            AgentuserBll userBll = new AgentuserBll();
            AgentuserEntity userEntity = new AgentuserEntity();
            userEntity = userBll.selectById(UserId);
            int UserLvId = userEntity.LvId;
            int UserPid = userEntity.UserPid;
            int LvtwoUserId = 0;
            int LvoneUserId = 0;
            int InlvoneUserId = 0;

            if (UserLvId <= 3)
            {
               
                if (UserLvId == 3)
                {
                    AgentuserEntity userPidEntity = userBll.selectById(UserPid);//上级信息
                    int UserPidLvid = userPidEntity.LvId;//上级等级Id
            
                    if (UserPidLvid == 2)//如果上级是大总   //三个参数  大总 Id 大牌Id 和大牌邀请人Id
                    {

                        //说明：
                        //1、userPidEntity 是上级（大总）信息  
                        //2、userPPidEntity 是大总上级信息（大牌）
                        //3、UserPid 大总Id   UserPidPid是大总得上级Id（大牌）    
                        int UserPidPid = userPidEntity.UserPid;//上级的上级Id  userPPidEntity.Inviter是大牌邀请人Id
                        AgentuserEntity userPPidEntity = userBll.selectById(UserPidPid);//查询大总得上级（大牌）                                         
                        LvtwoUserId = UserPid;//大总Id
                        LvoneUserId = UserPidPid;//大牌Id
                        InlvoneUserId = userPPidEntity.Inviter;//大牌邀请人Id
                    }
                    if (UserPidLvid == 1)//如果上级是大牌  大牌Id 大牌邀请人Id
                    {
                                      
                             
                        LvoneUserId = UserPid;//大牌Id
                        InlvoneUserId = userPidEntity.Inviter;//大牌邀请人Id
                    }

                }
                if (UserLvId == 2) //大牌Id 大牌邀请人Id
                {
                    AgentuserEntity userPidEntity = userBll.selectById(UserPid);//上级信息(大牌)
                    LvoneUserId = UserPid;//大牌Id
                    InlvoneUserId = userPidEntity.Inviter;//大牌邀请人Id
                }
            }


            LvUserIdEntity lvUser = new LvUserIdEntity();
            lvUser.UserId = UserId;
            lvUser.LvtwoUserId = LvtwoUserId;
            lvUser.LvoneUserId = LvoneUserId;
            lvUser.InlvoneUserId = InlvoneUserId;
            return lvUser;
        }
 
            
    }
}
