using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WbOsWeb.Bll;
using WbOsWeb.Entity;

namespace WbOsWeb.Upagent
{
    public class SqlAgentuserString
    {
        public string ListUser(string OneList)
        {
            string sqlWhere = "State=1 and UserPid in (" + OneList + ")";
            AgentuserBll userBll = new AgentuserBll();
            List<AgentuserEntity> ListoneEntity = AgentList(OneList);
            string ListUserId = string.Empty;
            ListUserId += OneList + ",";
            foreach (var item in ListoneEntity)
            {

                
                ListUserId += item.Id + ",";

            }

            ListUserId = ListUserId.Substring(0, ListUserId.Length - 1);
            return ListUserId;
        }
  
        public List<AgentuserEntity> AgentList(string OneList)
        {

            AgentuserBll ueserbll = new AgentuserBll();
    
            List<AgentuserEntity> userListentity = new List<AgentuserEntity>();
            string whereSql = " State=1" + " and UserPid in (" + OneList + ")";//id是上级ID state是已经审核状态
            userListentity = ueserbll.selectByWhere(whereSql);

            List<AgentuserEntity> listAgentuser = new List<AgentuserEntity>();



            foreach (var item in userListentity)
            {
                AgentuserEntity viewModelUser = new AgentuserEntity();
                viewModelUser.Id = item.Id;//下级Id
                viewModelUser.LvId = item.LvId;//下级等级Id

                SelectAuditList(item.Id, listAgentuser);
                listAgentuser.Add(viewModelUser);
            }

            return listAgentuser;
        }


        public void SelectAuditList(int Id, List<AgentuserEntity> listAgentuser)
        {
            AgentuserBll ueserbll = new AgentuserBll();
            List<AgentuserEntity> userentity = new List<AgentuserEntity>();
            string whereSql = " State=1" + " and UserPid=" + Id;//id是上级ID state是已经审核状态
            userentity = ueserbll.selectByWhere(whereSql);


            foreach (var item in userentity)
            {
                AgentuserEntity viewModel = new AgentuserEntity();
                viewModel.Id = item.Id;//下级Id              
                viewModel.LvId = item.LvId;//下级等级Id            
                SelectAuditList(item.Id, listAgentuser);
                listAgentuser.Add(viewModel);
            }

        }
    }
}
