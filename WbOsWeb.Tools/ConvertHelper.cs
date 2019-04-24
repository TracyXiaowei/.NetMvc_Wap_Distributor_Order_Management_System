using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WbOsWeb.Tools
{
    /// <summary>
    /// For the object type data into specific data types
    /// </summary>
    public class ConvertHelper
    {
        #region Methods the object variable to a string variable
        /// <summary>
        /// Methods the object variable to a string variable
        /// </summary>
        /// <param name="obj">Object variable</param>
        /// <returns>String variable</returns>
        public static string GetString(object obj)
        {
            return (obj == DBNull.Value || obj == null) ? "" : obj.ToString();
        }
        #endregion

        #region The object variable to 32 bit integer variables
        /// <summary>
        /// The object variable to 32 bit integer variables
        /// </summary>
        /// <param name="obj">Object variable</param>
        /// <returns>A 32 bit integer variables</returns>
        public static int GetInteger(object obj)
        {
            return ConvertStringToInteger(GetString(obj));
        }
        #endregion

        #region The object variable to 64 bit integer variables
        /// <summary>
        /// The object variable to 64 bit integer variables
        /// </summary>
        /// <param name="obj">Object variable</param>
        /// <returns>A 64 bit integer variables</returns>
        public static long GetLong(object obj)
        {
            return ConvertStringToLong(GetString(obj));
        }
        #endregion

        #region Methods the object variable to double precision floating-point variable
        /// <summary>
        /// Methods the object variable to double precision floating-point variable
        /// </summary>
        /// <param name="obj">Object variable</param>
        /// <returns>Double precision floating-point variable</returns>
        public static double GetDouble(object obj)
        {
            return ConvertStringToDouble(GetString(obj));
        }
        #endregion

        #region Methods the object variable is transformed into a decimal number variables
        /// <summary>
        /// Methods the object variable is transformed into a decimal number variables
        /// </summary>
        /// <param name="obj">Object variable</param>
        /// <returns>Decimal numeric variables</returns>
        public static decimal GetDecimal(object obj)
        {
            return ConvertStringToDecimal(GetString(obj));
        }

        /// <summary>
        /// According to the number of decimal places or
        /// </summary>
        /// <param name="o">Need to choose the number of</param>
        /// <param name="digits">Decimal digits</param>
        /// <returns></returns>
        public static string GetNumericalStringBydigit(object o, int digits)
        {
            if (o == DBNull.Value)
            {
                return string.Empty;
            }
            double i = 0;
            if (double.TryParse(o.ToString(), out i))
            {
                if (digits != 0)
                {
                    return Math.Round(i, digits).ToString();
                }
                return Math.Round(i).ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion

        #region Methods the object variable into a boolean variable
        /// <summary>
        /// Methods the object variable into a boolean variable
        /// </summary>
        /// <param name="obj">Object variable</param>
        /// <returns>Boolean variables</returns>
        public static bool GetBoolean(object obj)
        {
            return (obj == DBNull.Value || obj == null) ? false :
                GetString(obj).Length == 0 ? false : Convert.ToBoolean(obj);
        }
        #endregion

        #region Methods the object variable to date and time type string variables
        /// <summary>
        /// Methods the object variable to date and time type string variables
        /// </summary>
        /// <param name="obj">Object variable</param>
        /// <param name="sFormat">The format character</param>
        /// <returns>Time type string variable</returns>
        public static string GetDateTimeString(object obj, string sFormat)
        {
            return (obj == DBNull.Value || obj == null) ? "" : Convert.ToDateTime(obj).ToString(sFormat);
        }

        /// <summary>
        /// 将对象变量转成日期字符串变量（短）的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>日期字符串变量</returns>
        public static string GetShortDateString(object obj)
        {
            return GetDateTimeString(obj, "yyyy-MM-dd");
        }

        /// <summary>
        /// 将对象变量转成日期字符串变量（长）的方法
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetLongDateString(object obj)
        {
            return GetDateTimeString(obj, "yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 将对象变量转成日期型变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>日期型变量</returns>
        public static DateTime GetDateTime(object obj)
        {
            return ConvertStringToDateTime(GetString(obj));
        }

        /// <summary>
        /// 时间格式转换成yyyy-MM-dd 或者 yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="o">时间对象</param>
        /// <param name="shortime">是否转成yyyy-MM-dd</param>
        /// <returns>时间格式字符串</returns>
        public static string DateTimeFormat(object o, bool shortime)
        {
            if (o == DBNull.Value)
            {
                return string.Empty;
            }
            if (string.IsNullOrEmpty(o.ToString()))
            {
                return string.Empty;
            }
            string convertDateString = string.Empty;

            if (shortime)
            {
                convertDateString = Convert.ToDateTime(o).ToString("yyyy-MM-dd");
            }
            else
            {
                convertDateString = Convert.ToDateTime(o).ToString("yyyy-MM-dd HH:mm:ss");
            }

            return convertDateString.IndexOf("1900") < 0 ? convertDateString : string.Empty;
        }
        #endregion

        #region Private methods

        #region The string into a 32 bit integer variables
        /// <summary>
        /// The string into a 32 bit integer variables
        /// </summary>
        /// <param name="s">Character string</param>
        /// <returns>A 32 bit integer variables</returns>
        private static int ConvertStringToInteger(string s)
        {
            int result = 0;
            int.TryParse(s, out result);
            return result;
        }
        #endregion

        #region The string into a 64 bit integer variables
        /// <summary>
        /// The string into a 64 bit integer variables
        /// </summary>
        /// <param name="s">Character string</param>
        /// <returns>A 64 bit integer variables</returns>
        private static long ConvertStringToLong(string s)
        {
            long result = 0;
            long.TryParse(s, out result);
            return result;
        }
        #endregion

        #region Method to convert a string to double precision floating-point variable
        /// <summary>
        /// Method to convert a string to double precision floating-point variable
        /// </summary>
        /// <param name="s">Character string</param>
        /// <returns>Double precision floating-point variable</returns>
        private static double ConvertStringToDouble(string s)
        {
            double result = 0;
            double.TryParse(s, out result);
            return result;
        }
        #endregion

        #region Method to convert a string to decimal numeric variables
        /// <summary>
        /// Method to convert a string to decimal numeric variables
        /// </summary>
        /// <param name="s">Character string</param>
        /// <returns>Decimal numeric variables</returns>
        private static decimal ConvertStringToDecimal(string s)
        {
            decimal result = 0;
            decimal.TryParse(s, out result);
            return result;
        }
        #endregion

        #region Method to convert a string into a time variable
        /// <summary>
        /// Method to convert a string into a time variable
        /// </summary>
        /// <param name="s">Character string</param>
        /// <returns>Time variable</returns>
        private static DateTime ConvertStringToDateTime(string s)
        {
            DateTime result;
            DateTime.TryParse(s, out result);
            return result;
        }
        #endregion

        #endregion
    }
}