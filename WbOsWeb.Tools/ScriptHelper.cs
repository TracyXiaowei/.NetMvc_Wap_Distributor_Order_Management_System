using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;


namespace WbOsWeb.Tools
{
    /// <summary>
    /// For the output script in a page class
    /// </summary>
    public class ScriptHelper
    {
        #region Popup message dialog box
        /// <summary>
        /// Popup message dialog box
        /// </summary>
        /// <param name="page">The current page pointer, usually this</param>
        /// <param name="msg">Prompt information</param>
        public static void ShowAlertScript(Page page, string msg)
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", "<script language='javascript' defer>$(function(){alert('" + HttpUtility.JavaScriptStringEncode(msg) + "');});</script>");
        }
        #endregion

        public static void ShowAlertAndReflushParentScript(Page page, string msg, bool IsClose = true)
        {
            if (IsClose)
                page.ClientScript.RegisterStartupScript(page.GetType(), "message", "<script language='javascript'>$(function(){alert('" + HttpUtility.JavaScriptStringEncode(msg) + "');__closeWindow();});</script>");
            else
                page.ClientScript.RegisterStartupScript(page.GetType(), "message", "<script language='javascript'>$(function(){alert('" + HttpUtility.JavaScriptStringEncode(msg) + "');parent.location.href=parent.location.href;});</script>");

        }

        #region Popup message dialog box, do not specify the Page object

        /// <summary>
        /// A pop-up message prompt dialog box, and jumps back to the previous Page, do not need to specify the Page object 
        /// </summary>
        /// <param name="msg">The news of the display </param>
        public static void ShowAlertScriptAndContinue(string msg)
        {
            HttpContext.Current.Response.Write("<Script Language=Javascript>alert('");
            HttpContext.Current.Response.Write(HttpUtility.JavaScriptStringEncode(msg));
            HttpContext.Current.Response.Write("');");
            HttpContext.Current.Response.Write("history.go(-1);");
            HttpContext.Current.Response.Write("</Script>");
            HttpContext.Current.Response.End();
        }
        /// <summary>
        /// A pop-up message prompt dialog box, do not need to specify the Page object 
        /// </summary>
        /// <param name="msg">The news of the display</param>
        public static void ShowAlertScript(string msg)
        {
            HttpContext.Current.Response.Write("<Script Language=Javascript>alert('");
            HttpContext.Current.Response.Write(HttpUtility.JavaScriptStringEncode(msg));
            HttpContext.Current.Response.Write("');");
            HttpContext.Current.Response.Write("</Script>");
        }
        #endregion

        #region A pop-up message prompt dialog box, and jump to page
        /// <summary>
        /// A pop-up message prompt dialog box, and jump to page 。When the URL for the BACK page will return to the previous page, when the URL is CLOSE, CLOSE the window. 
        /// </summary>
        /// <param name="page">Pointer to the current page, generally for this </param>
        /// <param name="msg">Prompt information </param>
        /// <param name="url">Jump target URL. When the URL for the BACK page will return to the previous page, when the URL is CLOSE, CLOSE the window. </param>
        public static void ShowAlertAndRedirectScript(Page page, string msg, string url)
        {
            StringBuilder Builder = new StringBuilder();
            Builder.Append("<script language='javascript' defer>");
            Builder.AppendFormat("alert('{0}');", HttpUtility.JavaScriptStringEncode(msg));

            switch (url.ToUpper())
            {
                case "BACK":
                    Builder.Append("history.go(-1);");
                    break;

                case "CLOSE":
                    Builder.Append("window.close();");
                    break;

                default:
                    Builder.AppendFormat("top.location.href='{0}'", url);
                    break;
            }


            Builder.Append("</script>");
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", Builder.ToString());

        }

        /// <summary>
        ///A pop-up message prompt dialog box, and jump to page. When the URL for the BACK page will return to the previous page, when the URL is CLOSE, CLOSE the window. 
        /// </summary>
        /// <param name="msg">Prompt information </param>
        /// <param name="url">To jump page. After the information, when the URL for the BACK page will return to the previous page, when the URL is CLOSE, CLOSE the window. </param>
        public static void ShowAlertAndRedirectScript(string msg, string url)
        {
            HttpContext.Current.Response.Write("<Script Language=Javascript>alert('");
            HttpContext.Current.Response.Write(HttpUtility.JavaScriptStringEncode(msg));
            HttpContext.Current.Response.Write("');");

            switch (url.ToUpper())
            {
                case "BACK":
                    HttpContext.Current.Response.Write("history.go(-1);");
                    break;
                case "CLOSE":
                    HttpContext.Current.Response.Write("window.close();");
                    break;
                default:
                    {
                        HttpContext.Current.Response.Write("top.location.href='");
                        HttpContext.Current.Response.Write(url);
                        HttpContext.Current.Response.Write("';");

                        break;
                    }
            }
            HttpContext.Current.Response.Write("</Script>");
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// Pop-up confirmation message prompt dialog box, click on the confirmation page after the jump 
        /// </summary>
        /// <param name="page">Pointer to the current page, generally for this </param>
        /// <param name="msg">Prompt information </param>
        /// <param name="url">Jump target URL. When the URL for the BACK page will return to the previous page, when the URL is CLOSE, CLOSE the window. </param>
        public static void ShowConfirmAndRedirectScript(Page page, string msg, string url)
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append("<script language='javascript' defer>");

            Builder.AppendFormat("if(confirm('{0}'))", HttpUtility.JavaScriptStringEncode(msg));
            Builder.Append("{");
            switch (url.ToUpper())
            {
                case "BACK":
                    Builder.Append("history.go(-1);");
                    break;

                case "CLOSE":
                    Builder.Append("window.close();");
                    break;

                default:
                    Builder.AppendFormat("top.location.href='{0}'", url);
                    break;
            }
            Builder.Append("}");

            Builder.Append("</script>");

            page.ClientScript.RegisterStartupScript(page.GetType(), "message", Builder.ToString());

        }
        /// <summary>
        /// Pop-up confirmation message prompt dialog box, click to confirm or cancel the page after the jump 
        /// </summary>
        /// <param name="page">Pointer to the current page, generally for this </param>
        /// <param name="msg">Prompt information </param>
        /// <param name="url1">Click ok, jump the target URL. When the URL for the BACK page will return to the previous page, when the URL is CLOSE, CLOSE the window. </param>
        /// <param name="url2">Jump target URL. When the URL for the BACK page will return to the previous page, when the URL is CLOSE, CLOSE the window. </param>
        public static void ShowConfirmAndRedirectScript(Page page, string msg, string url1, string url2)
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append("<script language='javascript' defer>");

            Builder.AppendFormat("if(confirm('{0}'))", HttpUtility.JavaScriptStringEncode(msg));
            Builder.Append("{");
            switch (url1.ToUpper())
            {
                case "BACK":
                    Builder.Append("history.go(-1);");
                    break;

                case "CLOSE":
                    Builder.Append("window.close();");
                    break;

                default:
                    Builder.AppendFormat("top.location.href='{0}'", url1);
                    break;
            }
            Builder.Append("}else{");
            switch (url2.ToUpper())
            {
                case "BACK":
                    Builder.Append("history.go(-1);");
                    break;

                case "CLOSE":
                    Builder.Append("window.close();");
                    break;

                default:
                    Builder.AppendFormat("top.location.href='{0}'", url2);
                    break;
            }
            Builder.Append("}");

            Builder.Append("</script>");

            page.ClientScript.RegisterStartupScript(page.GetType(), "message", Builder.ToString());

        }

        #endregion

        #region Control click message confirmation box
        /// <summary>
        /// Control click message confirmation box 
        /// </summary>
        /// <param name="Control">The target control </param>
        /// <param name="msg">Prompt information </param>
        public static void ShowConfirmScript(WebControl Control, string msg)
        {
            Control.Attributes.Add("onclick", "return confirm('" + msg + "');");
        }
        #endregion

        #region Click open the form controls
        /// <summary>
        /// Click open the form controls 
        /// </summary>
        /// <param name="Control">The target control </param>
        /// <param name="url">Open the address of the page</param>
        public static void ShowOpenScript(WebControl Control, string url)
        {
            Control.Attributes.Add("onclick", "window.open('" + url + "');");
        }

        /// <summary>
        /// Click open the form controls 
        /// </summary>
        /// <param name="Control">The target control</param>
        /// <param name="url">Open the address of the page</param>
        /// <param name="width">Open the width of the page</param>
        /// <param name="height">Open the height of the page</param>
        public static void ShowOpenScript(WebControl Control, string url, int width, int height)
        {
            Control.Attributes.Add("onclick", "window.open('" + url + "', '', 'width=" + width + ",height=" + height + ",fullscreen=no, toolbar=no, menubar=no, scrollbars=yes, resizable=yes,location=no, status=no');");
        }

        /// <summary>
        /// Click open the form controls 
        /// </summary>
        /// <param name="Control">The target control</param>
        /// <param name="url">Open the address of the page</param>
        public static void ShowOpenMaxScript(WebControl Control, string url)
        {
            Control.Attributes.Add("onclick", "var mywin=window.open('" + url + "');mywin.moveTo(0,0);mywin.resizeTo(screen.availWidth,screen.availHeight);");
        }
        #endregion

        #region Click on the close form control
        /// <summary>
        /// Click on the close form control
        /// </summary>
        /// <param name="Control">The target control</param>
        public static void ShowCloseScript(WebControl Control)
        {
            Control.Attributes.Add("onclick", "window.close();");
        }
        #endregion

        #region Control click message confirmation box, and close the window
        /// <summary>
        /// Control click message confirmation box 
        /// </summary>
        /// <param name="Control">The target control</param>
        /// <param name="msg">Prompt information</param>
        public static void ShowAlertAndCloseScript(WebControl Control, string msg)
        {
            StringBuilder Builder = new StringBuilder();
            Builder.AppendFormat("alert('{0}');", msg);
            Builder.AppendFormat("window.close();");
            Control.Attributes.Add("onclick", Builder.ToString());
        }
        #endregion

        #region Control click message confirmation box, and close the window, refreshes the parent form
        /// <summary>
        /// Control click message confirmation box
        /// </summary>
        /// <param name="Control">The target control</param>
        /// <param name="msg">Prompt information</param>
        /// <param name="url">Parent form address</param>
        public static void ShowAlertAndCloseScript(WebControl Control, string msg, string url)
        {
            StringBuilder Builder = new StringBuilder();
            Builder.AppendFormat("alert('{0}');", HttpUtility.JavaScriptStringEncode(msg));
            Builder.AppendFormat("window.close();");
            if (url != "") Builder.AppendFormat("opener.location.href='{0}';", url);
            Control.Attributes.Add("onclick", Builder.ToString());
        }
        #endregion

        #region The output information custom scripts
        /// <summary>
        /// The output information custom scripts
        /// </summary>
        /// <param name="page">Pointer to the current page, generally for this</param>
        /// <param name="script">The output script</param>
        public static void ShowCustomScript(Page page, string script)
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", "<script language='javascript' defer>" + script + "</script>");
        }
        #endregion
    }
}
