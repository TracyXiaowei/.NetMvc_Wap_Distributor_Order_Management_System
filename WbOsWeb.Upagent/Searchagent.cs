using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WbOsWeb.Bll;
using WbOsWeb.Entity;

namespace WbOsWeb.Upagent
{
    public class Searchagent
    {

        //如果是大牌UserID=1  LVid=1；查询大总 LVId=2；直线条件就是  LVID=2，UserPid=1
        //也就是下级一级 只可能存在 直线 下二级及更下才会出现间接团队人数

        string UploadImages = ConfigurationManager.AppSettings["UploadImages"];
        string UploadAuthorizationImage = ConfigurationManager.AppSettings["UploadAuthorizationImage"];
        //查询直接下线的代理人数（直线）
        public int AgentNum(int LvId, int UserId)
        {
            AgentuserBll bll = new AgentuserBll();
            List<AgentuserEntity> listEntity = new List<AgentuserEntity>();
            string Where = " LvId=" + LvId + "and State=1 and Inviter=" + UserId;
            listEntity = bll.selectByWhere(Where);
            int Num = 0;
            foreach (var item in listEntity)
            {
                Num++;
            }
            return Num;
        }

        //查询下级所有代理
        public AgentuserListViewModel AgentList(int UserId)
        {

            AgentuserBll ueserbll = new AgentuserBll();

            List<AgentuserEntity> userListentity = new List<AgentuserEntity>();
            string whereSql = " State=1" + " and UserPid=" + UserId;//id是上级ID state是已经审核状态
            userListentity = ueserbll.selectByWhere(whereSql);

            AgentuserListViewModel listAgentuser = new AgentuserListViewModel();
            listAgentuser.AgentuserList = new List<AgentuserViewModel>();
      

            foreach (var item in userListentity)
            {
                AgentuserViewModel viewModelUser = new AgentuserViewModel();
                viewModelUser.Id = item.Id;//下级Id
                viewModelUser.LvId = item.LvId;//下级等级Id
          
                SelectAuditList(item.Id, listAgentuser);
                listAgentuser.AgentuserList.Add(viewModelUser);
            }

            return listAgentuser;
        }




        public void SelectAuditList(int Id, AgentuserListViewModel listAgentuser)
        {
            AgentuserBll ueserbll = new AgentuserBll();
            List<AgentuserEntity> userentity = new List<AgentuserEntity>();
            string whereSql = " State=1" + " and UserPid=" + Id;//id是上级ID state是已经审核状态
            userentity = ueserbll.selectByWhere(whereSql);
    

            foreach (var item in userentity)
            {
                AgentuserViewModel viewModel = new AgentuserViewModel();
                viewModel.Id = item.Id;//下级Id              
                viewModel.LvId = item.LvId;//下级等级Id            
                SelectAuditList(item.Id, listAgentuser);
                listAgentuser.AgentuserList.Add(viewModel);
            }

        }


        public List<AgentuserEntity> AgentListEntity(int UserId)
        {

            AgentuserBll ueserbll = new AgentuserBll();

            List<AgentuserEntity> userListentity = new List<AgentuserEntity>();
            string whereSql = " State=1" + " and UserPid=" + UserId;//id是上级ID state是已经审核状态
            userListentity = ueserbll.selectByWhere(whereSql);
            List<AgentuserEntity> listAgentuser = new List<AgentuserEntity>();
            foreach (var item in userListentity)
            {
                AgentuserEntity viewModelUser = new AgentuserEntity();


                SelectAuditListEntity(item.Id, listAgentuser);
                listAgentuser.Add(item);
            }

            return listAgentuser;
        }
        public void SelectAuditListEntity(int Id, List<AgentuserEntity> listAgentuser)
        {
            AgentuserBll ueserbll = new AgentuserBll();
            List<AgentuserEntity> userentity = new List<AgentuserEntity>();
            string whereSql = " State=1" + " and UserPid=" + Id;//id是上级ID state是已经审核状态
            userentity = ueserbll.selectByWhere(whereSql);


            foreach (var item in userentity)
            {

                SelectAuditListEntity(item.Id, listAgentuser);
                listAgentuser.Add(item);
            }

        }

        //逻辑 1、假如是总代  寻找下级   自己邀请  切上级是自己  
        public List<AgentuserEntity> InvListEntity(int UserId)
        {
            AgentuserBll ueserbll = new AgentuserBll();
            List<AgentuserEntity> userListentity = new List<AgentuserEntity>();
            string whereSql = " State=1" + " and UserPid=" + UserId;//id是上级ID state是已经审核状态
            userListentity = ueserbll.selectByWhere(whereSql);
            List<AgentuserEntity> listAgentuser = new List<AgentuserEntity>();
            foreach (var item in userListentity)
            {
                AgentuserEntity viewModelUser = new AgentuserEntity();

                SelectInListEntity(item.Id, listAgentuser);


                listAgentuser.Add(item);
            }
            return listAgentuser;
        }

        private void SelectInListEntity(int Id, List<AgentuserEntity> listAgentuser)
        {
            AgentuserBll ueserbll = new AgentuserBll();
            List<AgentuserEntity> userentity = new List<AgentuserEntity>();
            string whereSql = " State=1" + " and UserPid=" + Id;//id是上级ID state是已经审核状态
            userentity = ueserbll.selectByWhere(whereSql);
            foreach (var item in userentity)
            {
                SelectInListEntity(item.Id, listAgentuser);
                listAgentuser.Add(item);
            }
        }



        //查询间接下线总人数 间接人数
        //例如 大牌总代查询 高级间接人数  lvid=4，UserPid=大总 和 总代；
        public int PidAgent(int LvId, int UserId)
        {

            AgentuserListViewModel listmodel = new AgentuserListViewModel();
            listmodel = AgentList(UserId);

            int Num = 0;
            foreach (var item in listmodel.AgentuserList)
            {
                if(item.LvId== LvId)
                {
                    Num++;
                }
                          
            }


            return Num;
        }


     
        public List<AgentuserEntity> PidAgent(int LvEid, int UserId, List<AgentuserEntity> list)
        {
            //1.得到与用户相关的代理信息
            //下级代理
            //第一步查询所有能大总高级的代理 UserId和总代；
            //********条件**************

            string Where = "LvId>" + LvEid + " and State=1 and UserPid=" + UserId;
            AgentuserBll bll = new AgentuserBll();
            List<AgentuserEntity> listEntity = new List<AgentuserEntity>();

            listEntity = bll.selectByWhere(Where);
            foreach (var item in listEntity)
            {
                int UserPid = item.Id;
                list.Add(item);
                PidAgent(LvEid, UserPid, list);

            }
            return list;
        }

   



    



        public class AgentuserViewModel
        {

            public int Id { get; set; }

            public string Name { get; set; }

            public string UpLvName { get; set; }

            public string UpName { get; set; }

            public string LoginName { get; set; }

            public string Password { get; set; }

            public string YesPassword { get; set; }
            public string Email { get; set; }

            public string Mobile { get; set; }

            public int State { get; set; }

            public string StateName { get; set; }


            public int ProvinceId { get; set; }

            public int CityId { get; set; }

            public int DistrictId { get; set; }
            public string ProvinceName { get; set; }

            public string CityName { get; set; }

            public string DistrictName { get; set; }

            public string Address { get; set; }

            public string LvName { get; set; }

            public string HeadPic { get; set; }

            public DateTime Createtime { get; set; }

            public int LvId { get; set; }

            public int Inviter { get; set; }

            public string InviterName { get; set; }
            public int UserPid { get; set; }

            public string AuthorizationNum { get; set; }

            public string AuthorizationPic { get; set; }

            public string IdentityCard { get; set; }
            public string AuthorizationTime { get; set; }
            public string WechatId { get; set; }

            public int Type { get; set; }
        }

        public class AgentuserListViewModel
        {
            public List<AgentuserViewModel> AgentuserList { get; set; }
        }

    }
}
