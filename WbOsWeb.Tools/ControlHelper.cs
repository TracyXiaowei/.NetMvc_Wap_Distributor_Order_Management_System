using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web;
using System.Web.UI.WebControls;


namespace WbOsWeb.Tools
{
    public static class ControlHelper
    {
        #region Initial paging controls, and the Repeater binding
        public static void InitPager(Wuqi.Webdiyer.AspNetPager pager)
        {
            if (pager == null) return;
            pager.AlwaysShow = true;

            //Gets or sets the page index invalid user input (negative or non digital) in the client to display error information.
            pager.InvalidPageIndexErrorMessage = "Input error! It's a number!";
            //Gets or sets the page index is out of range (user input is larger than the maximum page index or less than the minimum page index) in the client to display the error message.
            pager.PageIndexOutOfRangeErrorMessage = string.Format("Input error! It's between 1 and {0}", pager.PageCount);

            pager.FirstPageText = "首页";
            pager.PrevPageText = "上一页";
            pager.NextPageText = "下一页";
            pager.LastPageText = "末页";
            pager.PageSize = 10;
            //pager.CssClass = "pages";

            pager.CurrentPageButtonClass = "cpb";
            pager.ShowCustomInfoSection = Wuqi.Webdiyer.ShowCustomInfoSection.Left;
            pager.LayoutType = Wuqi.Webdiyer.LayoutType.Table;
        }

        /// <summary>
        /// Note: this method is only suitable for single table queries, multi table query please refer to this code to expand!
        /// Call a query, all data in a table, only need to specify the table name: ControlHelper.BindData2Repeater(rptModuleList, pager, "Sys_Menu")
        /// Call two, query a table according to the conditions, the need for additional table name is specified, the conditions and parameters: ControlHelper.BindData2Repeater(rptModuleList, pager, "Sys_Menu", where, ps)
        /// Call three, query a table according to the conditions, the need for additional table name is specified, and parameters, the sort field, ID field: ControlHelper.BindData2Repeater(rptModuleList, pager, "Sys_Menu", where, ps, "OrderNo Desc", "BizID")
        /// </summary>
        /// <param name="rpt"></param>
        /// <param name="pager"></param>
        /// <param name="tableName"></param>
        /// <param name="where"></param>
        /// <param name="ps"></param>
        /// <param name="sortField"></param>
        /// <param name="idField"></param>
        public static void BindData2Repeater(Repeater rpt, Wuqi.Webdiyer.AspNetPager pager, string tableName, string where = "", IList<System.Data.SqlClient.SqlParameter> ps = null, string sortField = "ID", string idField = "ID", string tempSql = null)
        {
            if (where.Length == 0) where = "1=1";
            if (ps == null) ps = new List<System.Data.SqlClient.SqlParameter>();

            if (pager == null)
            {
                string sql = string.Format(@"select * from {0} where {1} order by {3};", tableName, where, idField, sortField);

                DataSet ds = SqlHelper.ExecuteDataset(WebConfigHelper.DefaultConnectionString, CommandType.Text, sql, ps.ToArray());
                rpt.DataSource = ds.Tables[0];
                rpt.DataBind();
            }
            else
            {
                int numFrom = Math.Max(pager.CurrentPageIndex - 1, 0) * pager.PageSize + 1;
                int numTo = pager.CurrentPageIndex * pager.PageSize;

                string sql = string.Format(@"
select count(1) from {0} where {1};
with _T_ (_S_N_,_ID_) as 
(  
    select ROW_NUMBER() OVER(ORDER BY {3}),{2} from {0} where {1} 
)  
select * from _T_ left join {0} t0 on _T_._ID_ = t0.{2}
where _T_._S_N_ BETWEEN {4} AND {5} order by {3};"
                    , tableName, where, idField, sortField, numFrom, numTo);

                DataSet ds = SqlHelper.ExecuteDataset(WebConfigHelper.DefaultConnectionString, CommandType.Text, tempSql + sql, ps.ToArray());
                pager.RecordCount = (int)ds.Tables[0].Rows[0][0];
                rpt.DataSource = ds.Tables[1];
                rpt.DataBind();

                if (pager.RecordCount == 0)
                {
                    pager.CustomInfoHTML = "<font color='red'><b>%CurrentPageIndex%</b></font>/%PageCount%&nbsp;&nbsp;" + "Total:%RecordCount%";
                }
                else
                {
                    pager.CustomInfoHTML = "<font color='red'><b>%CurrentPageIndex%</b></font>/%PageCount%&nbsp;&nbsp;Total:%RecordCount%(%StartRecordIndex%～%EndRecordIndex%)";
                }
            }
        }



        /// <summary>
        /// Note: this method is only suitable for single table queries, multi table query please refer to this code to expand!
        /// Call a query, all data in a table, only need to specify the table name: ControlHelper.BindData2Repeater(rptModuleList, pager, "Sys_Menu")
        /// Call two, query a table according to the conditions, the need for additional table name is specified, the conditions and parameters: ControlHelper.BindData2Repeater(rptModuleList, pager, "Sys_Menu", where, ps)
        /// Call three, query a table according to the conditions, the need for additional table name is specified, and parameters, the sort field, ID field: ControlHelper.BindData2Repeater(rptModuleList, pager, "Sys_Menu", where, ps, "OrderNo Desc", "BizID")
        /// </summary>
        /// <param name="rpt"></param>
        /// <param name="pager"></param>
        /// <param name="tableName"></param>
        /// <param name="where"></param>
        /// <param name="ps"></param>
        /// <param name="sortField"></param>
        /// <param name="idField"></param>
        public static void BindData3Repeater(Repeater rpt, Wuqi.Webdiyer.AspNetPager pager, string tableName, string where, IList<System.Data.SqlClient.SqlParameter> ps, string sortField, string queryFld)
        {
            string sqlCount = string.Format("select count(1) from {0} where {1}", tableName, where);
            int recordCount = int.Parse(SqlHelper.ExecuteScalar(WebConfigHelper.DefaultConnectionString, CommandType.Text, sqlCount, ps.ToArray()).ToString());
            string sql = SqlSplit.GetPageSql(tableName, sortField, queryFld, pager.PageSize, pager.CurrentPageIndex, recordCount, true, where);
            DataSet ds = SqlHelper.ExecuteDataset(WebConfigHelper.DefaultConnectionString, CommandType.Text, sql, ps.ToArray());
            pager.RecordCount = recordCount;
            rpt.DataSource = ds.Tables[0];
            rpt.DataBind();

            if (pager.RecordCount == 0)
            {
                pager.CustomInfoHTML = "<font color='red'><b>%CurrentPageIndex%</b></font>/%PageCount%&nbsp;&nbsp;Total:%RecordCount%";
            }
            else
            {
                pager.CustomInfoHTML = "<font color='red'><b>%CurrentPageIndex%</b></font>/%PageCount%&nbsp;&nbsp;Total:%RecordCount%(%StartRecordIndex%～%EndRecordIndex%)";
            }
        }
        #endregion
    }
}
