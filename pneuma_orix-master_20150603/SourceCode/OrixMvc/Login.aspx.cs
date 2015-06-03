using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using VS2008.Module;


namespace OrixMvc
{
	/// <summary>
	/// Program And Name      ：
	/// Author                  ：
	/// Build Date              ：
	/// Modify Date/Description ：
	///
	/// </summary>
	public partial class Login : Pattern.PageParent {


        /// <summary>
        /// 取得CORP ACCT
        /// </summary>
        /// <returns></returns>
        protected string CorpAcct
        {
            set { ViewState["CorpAcct"] = value; }
            get { return (ViewState["CorpAcct"] == null ? "" : (String)ViewState["CorpAcct"]); }
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

		#region 引用Module設定
		//*******************begin 勿動***********************
		/// <summary>
		/// dss：將dataset存入sql server
		/// cts：將sql command 存入 sql server
		/// dg ：取得sql server的資料
		/// </summary>
        VS2008.Module.DataSetToSql dss = new VS2008.Module.DataSetToSql();
        VS2008.Module.CommandToSql cts = new VS2008.Module.CommandToSql();
        VS2008.Module.DataGetting dg = new VS2008.Module.DataGetting();

		//**********************end 勿動***********************
		#endregion

		#region Page_Load 網頁初始設定：宣告MasterPage所有Event，設定公用參數值
		/// <summary>
		/// 網頁初始
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            

        }
		#endregion

		#region LoginCheck：登入驗證
		/// <summary>
		/// 驗證員工資料
		/// 1.員工代號是否存在
		/// </summary>
		/// <returns></returns>
		public string LoginCheck() {

            string strScript = "";

            DataRow dr = dg.GetDataRow("select USER_PASS,USER_NAME,PWD_SETTING_DATE,EMP_CODE from OR3_USERS where USER_ID='" + this.USER_ID.rpsText() + "'");
  
            if (dr[0].ToString()!="")
            {
                DataRow dr1 = dg.GetDataRow("select a.EMP_CODE,a.EMP_NAME,a.CORP_ACCT from OR_EMP a,OR3_USERS b where a.EMP_CODE=b.EMP_CODE and b.USER_ID='" + this.USER_ID.rpsText() + "'");

                this.CorpAcct = dr1["CORP_ACCT"].ToString().Trim();
                this.EmployeeId = dr1["EMP_CODE"].ToString().Trim();
                this.EmployeeName = dr1["EMP_NAME"].ToString().Trim();

                if (dr["USER_PASS"].ToString().Trim() != this.USER_PASS.Text.Trim().GetMD5())
                {
                    strScript += (strScript == "" ? "" : "<br>") + "使用者密碼輸入錯誤！";
                    this.CorpAcct = "";
                    this.EmployeeId ="";
                    this.EmployeeName = "";
                }
                else
                {
                    int intDays = System.Configuration.ConfigurationManager.AppSettings["ExpiredDays"].ToString().toInt();
                    DateTime sDate = dr["PWD_SETTING_DATE"].ToString().toDate().AddDays(intDays) ;
                                        
                    if (sDate.ToString("yyyyMMdd").CompareTo(DateTime.Now.ToString("yyyyMMdd"))<=0)//重設密碼後, 尚未修改密碼
                    {
                        strScript += (strScript == "" ? "" : "<br>") + "密碼已過期, 請重新設定密碼！";
                        //call 修改密碼視窗

                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "change", "changeForm('confirm');", true);
                    
                        return strScript;
                    }
                }
                
            }
            else
                strScript += (strScript == "" ? "" : "<br>") + "使用者ID不存在！";
                

			return strScript;
		}

		/// <summary>
		/// 系統登入：確認/Login
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void Login_Click(object sender, System.Web.UI.WebControls.CommandEventArgs e) {

            string myScript = "";

            switch (e.CommandName)
            {

                case "login":
                case "confirm":
                    if (this.USER_ID.Text.Trim() == "")                    
                        myScript = "便用者ID必須輸入！";                                             
                    
                    if (this.USER_PASS.Text.Trim() == "")
                        myScript += (myScript == "" ? "" : "<br>") + "使用者密碼必須輸入！";     
                    
                   if (myScript=="")                    
                       myScript = this.LoginCheck();

                   if (myScript == "")
                   {
                       if (e.CommandName == "login")
                           this.setLogin();
                       else
                           ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "change", "changeForm('confirm');", true);
                   }

                   if (myScript != "")
                   {
                       ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "MyScript", "setWarning('" + myScript + "');", true);
                       return;
                   }

                    
                    break;
                case "reset":
                                        

                    if (this.USER_PASSN.Text.Trim() == "")
                        myScript += (myScript == "" ? "" : "<br>") + "新密碼必須輸入！";

                    if (this.USER_PASSNA.Text.Trim() == "")
                        myScript += (myScript == "" ? "" : "<br>") + "確認新密碼必須輸入！";

                    if (this.USER_PASSN.Text.Trim() != this.USER_PASSNA.Text.Trim() && this.USER_PASSN.Text.Trim() != "" && this.USER_PASSNA.Text.Trim() != "")
                        myScript += (myScript == "" ? "" : "<br>") + "密碼輸入不一致！";
                    else
                        if (this.USER_PASS.Text.Trim() == this.USER_PASSN.Text.Trim()  )
                            myScript += (myScript == "" ? "" : "<br>") + "新密碼不得與舊密相同！";

                    if (myScript != "")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "MyScript", "setWarning('" + myScript + "');", true);
                        return;
                    }

                    cts.Execute("update OR3_USERS set USER_PASS='" + this.USER_PASSNA.Text.Trim().GetMD5() + "',PWD_SETTING_DATE='"+ DateTime.Now.ToString("yyyyMMdd") +"' where USER_ID='" + this.USER_ID.rpsText() + "'");
                    this.setLogin();
                    break;

                case "cancel":
                    break;

            }

            
		}
		#endregion



        void setLogin()
        {
            Session["EmployeeId"]=this.EmployeeId;
            Session["EmployeeName"]=this.EmployeeName;
            Session["CorpAcct"] = this.CorpAcct;

            string ProgId = "WA070";
            #region 判斷是否有WA070的權限
            string strMenu = "select UF.FUNC_ID from OR3_USER_FUNC as UF inner join " +
                                    "(select UF.FUNC_ID, MAX(case UF.USER_ID when '" + this.USER_ID.Text.Trim() + "' then 1 else 0 end) as F2 from OR3_USER_FUNC as UF " +
                                    "inner join (select USER_ID,GROUP_ID from OR3_USERS where USER_ID='" + this.USER_ID.Text.Trim() + "') as U " +
                                    "on UF.USER_ID in (U.USER_ID,U.GROUP_ID) " +
                                    "group by UF.FUNC_ID) as SUB " +
                                    "on UF.FUNC_ID=SUB.FUNC_ID and ((SUB.F2=1 and UF.USER_ID='" + this.USER_ID.Text.Trim() + "') or (SUB.F2=0 and UF.USER_ID=(select GROUP_ID from OR3_USERS where USER_ID='" + this.USER_ID.Text.Trim() + "')))" +
                                    "inner join OR3_FUNCTION as F on UF.FUNC_ID=F.FUNC_ID " +
                                    "where F.FUNC_TYPE='P' and UF.FUNC_ID = '" + ProgId + "'";
            DataRow dr = dg.GetDataRow(strMenu);

            if (dr == null || dr[0].ToString() == "")
            {
                ProgId = "Welcome";
            }
            #endregion

            Session["Mvc"] = false;
            Session["ProgramId"] =ProgId;
            Session["ProgramName"]=dg.GetDataRow("select Func_Name from OR3_Function where Func_ID='"+ ProgId +"'")[0].ToString();
            Session["UserId"] = this.USER_ID.Text.Trim();


            this.Page.openProgram("main.aspx");
        }
	}
}

