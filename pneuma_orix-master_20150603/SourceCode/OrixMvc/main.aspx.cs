using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Web.Services;

namespace OrixMvc
{
    public partial class main : System.Web.UI.Page
    {
        /// <summary>
        /// 取得Program Id
        /// </summary>
        /// <returns></returns>
        protected string ProgramId
        {
            set { ViewState["ProgramId"] = value; }
            get { return (ViewState["ProgramId"] == null ? "" : (String)ViewState["ProgramId"]); }
        }

        /// <summary>
        /// 取得Program Name
        /// </summary>
        /// <returns></returns>
        protected string ProgramName
        {
            set { ViewState["ProgramName"] = value; }
            get { return (ViewState["ProgramName"] == null ? "" : (String)ViewState["ProgramName"]); }
        }

        /// <summary>
        /// 取得Employee Id
        /// </summary>
        /// <returns></returns>
        protected string EmployeeId
        {
            set { ViewState["EmployeeId"] = value; }
            get { return (ViewState["EmployeeId"] == null ? "" : (String)ViewState["EmployeeId"]); }
        }

        /// <summary>
        /// 取得Employee Name
        /// </summary>
        /// <returns></returns>
        protected string EmployeeName
        {
            set { ViewState["EmployeeName"] = value; }
            get { return (ViewState["EmployeeName"] == null ? "" : (String)ViewState["EmployeeName"]); }
        }

        /// <summary>
        /// 取得是否為MVC架構
        /// </summary>
        /// <returns></returns>
        protected bool MVC
        {
            set { ViewState["MVC"] = value; }
            get { return (ViewState["MVC"] == null ? false : (bool)ViewState["MVC"]); }
        }

        public VS2008.Module.DataGetting dg = new VS2008.Module.DataGetting();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["EmployeeId"] == null || Session["EmployeeName"] == null || Session["ProgramId"] == null || Session["ProgramName"] == null)
                {
                    this.Page.Response.Redirect("login.aspx");
                }
                else
                {
                    this.EmployeeId = ((String)Session["EmployeeId"]).Trim();
                    this.EmployeeName = ((String)Session["EmployeeName"]).Trim();
                    this.ProgramId = ((String)Session["ProgramId"]).Trim();
                    this.ProgramName = ((String)Session["ProgramName"]).Trim();
                    this.MVC = ((bool)Session["Mvc"]);  //MVC模式
                }

                //修正權限異常，依EMP_NO可能有多個登入帳號，應直接取得登入者帳號
                //this.UserId.Value = dg.GetDataRow("select USER_ID from OR3_USERS where EMP_CODE='" + this.EmployeeId + "'")[0].ToString().Trim();
                this.UserId.Value = ((String)Session["UserId"]).Trim();
            }
            else
            {
                Session["EmployeeId"] = this.EmployeeId;
                Session["EmployeeName"] = this.EmployeeName;
                Session["ProgramId"] = this.ProgramId;
                Session["ProgramName"] = this.ProgramName;
                Session["Mvc"] = this.MVC;  //MVC模式
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Open_Click(object sender, EventArgs e)
        {
            this.ProgramId = this.progID.Text.Trim();
            this.ProgramName = this.progNM.Text.Trim();

            Session["EmployeeId"] = this.EmployeeId;
            Session["EmployeeName"] = this.EmployeeName;
            Session["ProgramId"] = this.ProgramId;
            Session["ProgramName"] = this.ProgramName;
            Session["Mvc"] = false; //MVC模式

            string strScript = "contentChange('frameContent');\nwindow.open('" + this.ProgramId.Trim() + ".aspx','frameContent')";

            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "MyScript" + System.DateTime.Now.ToString("mmSS"), strScript, true);


        }

        [WebMethod]
        //設定Session
        public static void SetSession(string ProgramId, string ProgramName)
        {
            HttpContext.Current.Session["ProgramId"] = ProgramId;
            HttpContext.Current.Session["ProgramName"] = ProgramName;
            HttpContext.Current.Session["Mvc"] = true;
        }

        //登出功能
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();

        	Response.Redirect("Login.aspx"); 
        }
    }
}
