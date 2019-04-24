using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WbOsWeb.Bll;
using WbOsWeb.Entity;
using WbOsWeb.WechatApi;

namespace WbOsWeb.WechatSendmessage
{
    public class SendMessage
    {
        /// <summary>
        /// 全部发货推送通知
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="GoodNames"></param>
        /// <param name="EXName"></param>
        /// <param name="ExId"></param>
        /// <param name="OpenId"></param>
        public void DeliverallgoodsSendMessage(string orderId, string GoodNames, string EXName, string ExId, string OpenId)
        {
            DateTime Now = DateTime.Now;
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{\"");
            jsonBuilder.Append("touser");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(OpenId);
            jsonBuilder.Append("\",");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("template_id");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("MogW2zn_o7Hx1FvxIHwjzuu8RVAVXF7-lTNEhbHh0Mc");
            jsonBuilder.Append("\",");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("topcolor");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("#FF0000");
            jsonBuilder.Append("\",");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("data");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("first");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("********【全部发货】********");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("},");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("keyword1");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(orderId);
            jsonBuilder.Append("\"");
            jsonBuilder.Append("},");



            jsonBuilder.Append("\"");
            jsonBuilder.Append("keyword2");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(GoodNames);
            jsonBuilder.Append("\"");
            jsonBuilder.Append("}");
            jsonBuilder.Append(",");



            jsonBuilder.Append("\"");
            jsonBuilder.Append("keyword3");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(EXName);
            jsonBuilder.Append("\"");
            jsonBuilder.Append("}");
            jsonBuilder.Append(",");


            jsonBuilder.Append("\"");
            jsonBuilder.Append("keyword4");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(ExId);
            jsonBuilder.Append("\"");
            jsonBuilder.Append("}");
            jsonBuilder.Append(",");


            jsonBuilder.Append("\"");
            jsonBuilder.Append("keyword5");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(Now);
            jsonBuilder.Append("\"");
            jsonBuilder.Append("},");



            jsonBuilder.Append("\"");
            jsonBuilder.Append("remark");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("温馨提示：请注意关注物流动态，准备收货！欢迎您再次进货!！");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("}");

            jsonBuilder.Append("}");
            jsonBuilder.Append("}");


            WechatBindBll bindbll = new WechatBindBll();

            List<WechatBindEntity> listbind = new List<WechatBindEntity>();
            listbind = bindbll.selectAll();
            int BindId = listbind[0].Id;
            WechatBindEntity bindentity = bindbll.selectById(BindId);

            string AppId = bindentity.AppId;
            string AppSecret = bindentity.Appsecret;

            string jjson = jsonBuilder.ToString();
            SendTemplateMessage sendmessage = new SendTemplateMessage();
            sendmessage.SendMessage(jjson, AppId, AppSecret);


        }

        /// <summary>
        /// 部分发货推送通知
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="GoodNames"></param>
        /// <param name="EXName"></param>
        /// <param name="ExId"></param>
        /// <param name="OpenId"></param>
        public void DeliverpartgoodsSendMessage(string orderId, string GoodNames, string EXName, string ExId, string OpenId)
        {
            DateTime Now = DateTime.Now;
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{\"");
            jsonBuilder.Append("touser");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(OpenId);
            jsonBuilder.Append("\",");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("template_id");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("MogW2zn_o7Hx1FvxIHwjzuu8RVAVXF7-lTNEhbHh0Mc");
            jsonBuilder.Append("\",");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("topcolor");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("#FF0000");
            jsonBuilder.Append("\",");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("data");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("first");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("********【部分发货】********");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("},");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("keyword1");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(orderId);
            jsonBuilder.Append("\"");
            jsonBuilder.Append("},");



            jsonBuilder.Append("\"");
            jsonBuilder.Append("keyword2");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(GoodNames);
            jsonBuilder.Append("\"");
            jsonBuilder.Append("}");
            jsonBuilder.Append(",");



            jsonBuilder.Append("\"");
            jsonBuilder.Append("keyword3");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(EXName);
            jsonBuilder.Append("\"");
            jsonBuilder.Append("}");
            jsonBuilder.Append(",");


            jsonBuilder.Append("\"");
            jsonBuilder.Append("keyword4");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(ExId);
            jsonBuilder.Append("\"");
            jsonBuilder.Append("}");
            jsonBuilder.Append(",");


            jsonBuilder.Append("\"");
            jsonBuilder.Append("keyword5");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(Now);
            jsonBuilder.Append("\"");
            jsonBuilder.Append("},");



            jsonBuilder.Append("\"");
            jsonBuilder.Append("remark");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("温馨提示：请注意关注物流动态，准备收货！欢迎您再次进货!！");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("}");

            jsonBuilder.Append("}");
            jsonBuilder.Append("}");


            WechatBindBll bindbll = new WechatBindBll();

            List<WechatBindEntity> listbind = new List<WechatBindEntity>();
            listbind = bindbll.selectAll();
            int BindId = listbind[0].Id;
            WechatBindEntity bindentity = bindbll.selectById(BindId);

            string AppId = bindentity.AppId;
            string AppSecret = bindentity.Appsecret;

            string jjson = jsonBuilder.ToString();
            SendTemplateMessage sendmessage = new SendTemplateMessage();
            sendmessage.SendMessage(jjson, AppId, AppSecret);


        }

        /// <summary>
        /// 余货全部发完
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="GoodNames"></param>
        /// <param name="EXName"></param>
        /// <param name="ExId"></param>
        /// <param name="OpenId"></param>
        public void DeliverpartedgoodsSendMessage(string orderId, string GoodNames, string EXName, string ExId, string OpenId)
        {
            DateTime Now = DateTime.Now;
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{\"");
            jsonBuilder.Append("touser");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(OpenId);
            jsonBuilder.Append("\",");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("template_id");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("MogW2zn_o7Hx1FvxIHwjzuu8RVAVXF7-lTNEhbHh0Mc");
            jsonBuilder.Append("\",");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("topcolor");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("#FF0000");
            jsonBuilder.Append("\",");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("data");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("first");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("********【余货-全部发货】********");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("},");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("keyword1");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(orderId);
            jsonBuilder.Append("\"");
            jsonBuilder.Append("},");



            jsonBuilder.Append("\"");
            jsonBuilder.Append("keyword2");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(GoodNames);
            jsonBuilder.Append("\"");
            jsonBuilder.Append("}");
            jsonBuilder.Append(",");



            jsonBuilder.Append("\"");
            jsonBuilder.Append("keyword3");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(EXName);
            jsonBuilder.Append("\"");
            jsonBuilder.Append("}");
            jsonBuilder.Append(",");


            jsonBuilder.Append("\"");
            jsonBuilder.Append("keyword4");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(ExId);
            jsonBuilder.Append("\"");
            jsonBuilder.Append("}");
            jsonBuilder.Append(",");


            jsonBuilder.Append("\"");
            jsonBuilder.Append("keyword5");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(Now);
            jsonBuilder.Append("\"");
            jsonBuilder.Append("},");



            jsonBuilder.Append("\"");
            jsonBuilder.Append("remark");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("温馨提示：请注意关注物流动态，准备收货！欢迎您再次进货!！");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("}");

            jsonBuilder.Append("}");
            jsonBuilder.Append("}");


            WechatBindBll bindbll = new WechatBindBll();

            List<WechatBindEntity> listbind = new List<WechatBindEntity>();
            listbind = bindbll.selectAll();
            int BindId = listbind[0].Id;
            WechatBindEntity bindentity = bindbll.selectById(BindId);

            string AppId = bindentity.AppId;
            string AppSecret = bindentity.Appsecret;

            string jjson = jsonBuilder.ToString();
            SendTemplateMessage sendmessage = new SendTemplateMessage();
            sendmessage.SendMessage(jjson, AppId, AppSecret);


        }


        /// <summary>
        /// 订单审核不通过
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="OpenId"></param>
        /// <param name="Reasoning"></param>
        public void NoexamineSendMessage(string orderId, string OpenId, string Reasoning)
        {


            DateTime Now = DateTime.Now;



            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{\"");
            jsonBuilder.Append("touser");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(OpenId);
            jsonBuilder.Append("\",");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("template_id");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("WCxbtSdfOiT1yAJkgij7UMePJ7ZmSiSYh0h7YxQUIx8");
            jsonBuilder.Append("\",");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("topcolor");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("#FF0000");
            jsonBuilder.Append("\",");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("data");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("first");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("您好，你的进货单审核结果！");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("},");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("keyword1");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(orderId);
            jsonBuilder.Append("\"");
            jsonBuilder.Append("},");



            jsonBuilder.Append("\"");
            jsonBuilder.Append("keyword2");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(Now);
            jsonBuilder.Append("\"");
            jsonBuilder.Append("}");
            jsonBuilder.Append(",");



            jsonBuilder.Append("\"");
            jsonBuilder.Append("keyword3");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("【不通过】  原因:" + Reasoning + " 请重新下单!");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("}");
            jsonBuilder.Append(",");








            jsonBuilder.Append("\"");
            jsonBuilder.Append("remark");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("温馨提示：我们将随时为您服务！给你带来的不便请您谅解！！！");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("}");

            jsonBuilder.Append("}");
            jsonBuilder.Append("}");


            WechatBindBll bindbll = new WechatBindBll();

            List<WechatBindEntity> listbind = new List<WechatBindEntity>();
            listbind = bindbll.selectAll();
            int BindId = listbind[0].Id;
            WechatBindEntity bindentity = bindbll.selectById(BindId);

            string AppId = bindentity.AppId;
            string AppSecret = bindentity.Appsecret;

            string jjson = jsonBuilder.ToString();
            SendTemplateMessage sendmessage = new SendTemplateMessage();
            sendmessage.SendMessage(jjson, AppId, AppSecret);


        }


        /// <summary>
        /// 订单审核通过
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="OpenId"></param>
        public void examineSendMessage(string orderId, string OpenId)
        {


            DateTime Now = DateTime.Now;



            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{\"");
            jsonBuilder.Append("touser");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(OpenId);
            jsonBuilder.Append("\",");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("template_id");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("WCxbtSdfOiT1yAJkgij7UMePJ7ZmSiSYh0h7YxQUIx8");
            jsonBuilder.Append("\",");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("topcolor");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("#FF0000");
            jsonBuilder.Append("\",");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("data");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("first");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("您好，你的进货单审核结果！");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("},");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("keyword1");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(orderId);
            jsonBuilder.Append("\"");
            jsonBuilder.Append("},");



            jsonBuilder.Append("\"");
            jsonBuilder.Append("keyword2");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(Now);
            jsonBuilder.Append("\"");
            jsonBuilder.Append("}");
            jsonBuilder.Append(",");



            jsonBuilder.Append("\"");
            jsonBuilder.Append("keyword3");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("【通过】");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("}");
            jsonBuilder.Append(",");








            jsonBuilder.Append("\"");
            jsonBuilder.Append("remark");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("温馨提示：你的进货单已经审核通过，我们将在通过后及时为您安排发货!(审核之后就不能退换)");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("}");

            jsonBuilder.Append("}");
            jsonBuilder.Append("}");


            WechatBindBll bindbll = new WechatBindBll();

            List<WechatBindEntity> listbind = new List<WechatBindEntity>();
            listbind = bindbll.selectAll();
            int BindId = listbind[0].Id;
            WechatBindEntity bindentity = bindbll.selectById(BindId);

            string AppId = bindentity.AppId;
            string AppSecret = bindentity.Appsecret;

            string jjson = jsonBuilder.ToString();
            SendTemplateMessage sendmessage = new SendTemplateMessage();
            sendmessage.SendMessage(jjson, AppId, AppSecret);


        }

        /// <summary>
        /// 订单反审核
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="OpenId"></param>
        public void AntiauditSendMessage(string orderId, string OpenId)
        {


            DateTime Now = DateTime.Now;
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{\"");
            jsonBuilder.Append("touser");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(OpenId);
            jsonBuilder.Append("\",");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("template_id");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("WCxbtSdfOiT1yAJkgij7UMePJ7ZmSiSYh0h7YxQUIx8");
            jsonBuilder.Append("\",");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("topcolor");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("#FF0000");
            jsonBuilder.Append("\",");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("data");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("first");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("您好，您的进货单正在进行反审核！");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("},");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("keyword1");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(orderId);
            jsonBuilder.Append("\"");
            jsonBuilder.Append("},");



            jsonBuilder.Append("\"");
            jsonBuilder.Append("keyword2");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(Now);
            jsonBuilder.Append("\"");
            jsonBuilder.Append("}");
            jsonBuilder.Append(",");



            jsonBuilder.Append("\"");
            jsonBuilder.Append("keyword3");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("经系统排查，你的订单审核出现异常！您的进货单已经恢复到审核状态，我们将安排工作人员尽快为您重新审核！");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("}");
            jsonBuilder.Append(",");








            jsonBuilder.Append("\"");
            jsonBuilder.Append("remark");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("温馨提示：给您带来的不便，请您谅解！");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("}");

            jsonBuilder.Append("}");
            jsonBuilder.Append("}");


            WechatBindBll bindbll = new WechatBindBll();

            List<WechatBindEntity> listbind = new List<WechatBindEntity>();
            listbind = bindbll.selectAll();
            int BindId = listbind[0].Id;
            WechatBindEntity bindentity = bindbll.selectById(BindId);

            string AppId = bindentity.AppId;
            string AppSecret = bindentity.Appsecret;

            string jjson = jsonBuilder.ToString();
            SendTemplateMessage sendmessage = new SendTemplateMessage();
            sendmessage.SendMessage(jjson, AppId, AppSecret);


        }

        /// <summary>
        /// 订单提交成功
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="UserName"></param>
        /// <param name="Money"></param>
        /// <param name="GoodNames"></param>
        /// <param name="ExId"></param>
        /// <param name="OpenId"></param>
        public void OrderSuccessSendMessage(string orderId,string UserName, string Money, string GoodNames, string OpenId)
        {
            DateTime Now = DateTime.Now;
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{\"");
            jsonBuilder.Append("touser");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(OpenId);
            jsonBuilder.Append("\",");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("template_id");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("KQMxanfUN0XISKCTg5Ri0d__sN7Hhh2Rzihnk_4Z1V4");
            jsonBuilder.Append("\",");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("topcolor");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("#FF0000");
            jsonBuilder.Append("\",");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("data");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("first");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("订单支付成功！");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("},");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("keyword1");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(UserName);
            jsonBuilder.Append("\"");
            jsonBuilder.Append("},");



            jsonBuilder.Append("\"");
            jsonBuilder.Append("keyword2");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(orderId);
            jsonBuilder.Append("\"");
            jsonBuilder.Append("}");
            jsonBuilder.Append(",");



            jsonBuilder.Append("\"");
            jsonBuilder.Append("keyword3");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(Money);
            jsonBuilder.Append("\"");
            jsonBuilder.Append("}");
            jsonBuilder.Append(",");


            jsonBuilder.Append("\"");
            jsonBuilder.Append("keyword4");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(GoodNames);
            jsonBuilder.Append("\"");
            jsonBuilder.Append("}");
            jsonBuilder.Append(",");



            jsonBuilder.Append("\"");
            jsonBuilder.Append("remark");
            jsonBuilder.Append("\"");
            jsonBuilder.Append(":{");

            jsonBuilder.Append("\"");
            jsonBuilder.Append("value");
            jsonBuilder.Append("\":");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("温馨提示：我们将及时为您审核，为您发货！请耐心等待");
            jsonBuilder.Append("\"");
            jsonBuilder.Append("}");

            jsonBuilder.Append("}");
            jsonBuilder.Append("}");


            WechatBindBll bindbll = new WechatBindBll();

            List<WechatBindEntity> listbind = new List<WechatBindEntity>();
            listbind = bindbll.selectAll();
            int BindId = listbind[0].Id;
            WechatBindEntity bindentity = bindbll.selectById(BindId);

            string AppId = bindentity.AppId;
            string AppSecret = bindentity.Appsecret;

            string jjson = jsonBuilder.ToString();
            SendTemplateMessage sendmessage = new SendTemplateMessage();
            sendmessage.SendMessage(jjson, AppId, AppSecret);


        }

    }
}
