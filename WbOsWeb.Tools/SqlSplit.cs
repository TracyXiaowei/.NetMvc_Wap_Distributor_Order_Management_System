using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace WbOsWeb.Tools
{
    public static class SqlSplit
    {
        #region 取得分页SQL
        /// <summary>
        /// 数据分页 (得到指定页记录的SQL语句)
        /// </summary>
        /// <param name="tblName">表名</param>
        /// <param name="sortFieldName">字段名(排序)</param>
        /// <param name="queryFld">查询字段(多个字段以逗号隔开,查询全部字段用星号)</param>
        /// <param name="pageSize">面尺寸 一页显示的记录数</param>
        /// <param name="pageIndex">页码 当前页</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="orderType">排序类型 true 升 false 降</param>
        /// <param name="strWhere">查询的条件(注 不带WHERE)</param>
        /// <returns>返回SQL语句</returns>
        public static string GetPageSql(string tblName, string sortFieldName, string queryFld, int pageSize, int pageIndex, int recordCount, bool orderType, string strWhere)
        {
            //用于返回的取记录的SQL字符串   
            StringBuilder sqlString = new StringBuilder();

            //获得页面总数   
            int pageCount = recordCount % pageSize == 0 ? recordCount / pageSize : recordCount / pageSize + 1;

            //获得中间页的索引   
            int middleIndex = pageCount / 2;

            //设置第1页和最后一页的索引   
            int lastIndex = pageCount;

            sqlString.Append(string.Format("SELECT * ", queryFld));
            sqlString.Append("FROM(");
            sqlString.Append(
                string.Format(
                    " SELECT ROW_NUMBER() OVER(ORDER BY {1} {2}) AS 序号,{0} FROM {3}{4}"
                    , queryFld
                    , sortFieldName
                    , orderType ? "ASC" : "DESC"
                    , tblName
                    , " WHERE " + strWhere
                )
            );
            sqlString.Append(") As myTable");
            sqlString.Append(string.Format(" WHERE 序号 BETWEEN {0} AND {1}", Math.Max(pageIndex - 1, 0) * pageSize + 1, pageIndex * pageSize));


            return sqlString.ToString();
        }
        #endregion       

        /// <summary>
        /// 数据分页 (得到指定页记录的SQL语句)MS人员用
        /// </summary>
        /// <param name="tblName">表名</param>
        /// <param name="sortFieldName">字段名(排序)</param>
        /// <param name="queryFld">查询字段(多个字段以逗号隔开,查询全部字段用星号)</param>
        /// <param name="pageSize">面尺寸 一页显示的记录数</param>
        /// <param name="pageIndex">页码 当前页</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="orderType">排序类型 true 升 false 降</param>
        /// <param name="strWhere">查询的条件(注 不带WHERE)</param>
        /// <returns>返回SQL语句</returns>
        public static string GetPageSqlMS(string tblName, string sortFieldName, string queryFld, int pageSize, int pageIndex, int recordCount, bool orderType, string strWhere)
        {
            //用于返回的取记录的SQL字符串   
            StringBuilder sqlString = new StringBuilder();

            //获得页面总数   
            int pageCount = recordCount % pageSize == 0 ? recordCount / pageSize : recordCount / pageSize + 1;

            //获得中间页的索引   
            int middleIndex = pageCount / 2;

            //设置第1页和最后一页的索引   
            int lastIndex = pageCount;

            sqlString.Append(string.Format("SELECT * ", queryFld));
            sqlString.Append("FROM(");
            sqlString.Append(
                string.Format(
                    " SELECT {0}, ROW_NUMBER() OVER(ORDER BY {1} {2}) AS rowNum FROM {3}{4}"
                    , queryFld
                    , sortFieldName
                    , orderType ? "ASC" : "DESC"
                    , tblName
                    , strWhere
                )
            );
            sqlString.Append(") As myTable");
            sqlString.Append(string.Format(" WHERE rowNum BETWEEN {0} AND {1}", Math.Max(pageIndex - 1, 0) * pageSize + 1, pageIndex * pageSize));


            return sqlString.ToString();
        }
    }
}
