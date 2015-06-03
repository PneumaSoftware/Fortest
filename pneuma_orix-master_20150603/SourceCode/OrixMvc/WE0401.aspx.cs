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
using OrixMvc.Pattern;
using VS2008.Module;
using AjaxControlToolkit;
using System.Net.Mail;

namespace OrixMvc
{
    public partial class WE0401 : PageParent
    {


        protected bool bolWE020
        {
            set { ViewState["bolWE020"] = value; }
            get { return (ViewState["bolWE020"] == null ? false : (bool)ViewState["bolWE020"]); }
        }


        #region 引用Module設定
        //*******************begin 勿動***********************  
        /// <summary>
        /// dss：將dataset存入sql server
        /// cts：將sql command 存入 sql server
        /// dg ：取得sql server的資料
        /// </summary>
        VS2008.Module.DataSetToSql dts = new VS2008.Module.DataSetToSql();
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
            //*************************begin 勿動**************************   

            this.Master.SaveEvent += new SaveDelegate(Save_Click);
            this.Master.EditEvent += new DataDelegate(Data_Command);
            this.Master.ExitEvent += new ExitDelegate(Exit_Click);
            this.Master.loadEvent += new loadDelegate(setDefaultValue);
            if (!IsPostBack)
                this.setParms();
            //***************************end 勿動****************************
        }
        #endregion


       
        #region setParms：設定公共參數
        /// <summary>
        /// 設定公共參數-->順序不得互換
        /// 1.編輯區域Title
        /// 2.程式編輯功能(新增,修改,刪除)
        /// 3.雖有編輯功能, 但不顯示修改的欄位
        /// </summary>
        private void setParms()
        {
            /*
            
            //1.程式編輯功能
            this.Master.setEditingFunction(false, true, false);

            this.Master.showSystemButton(SystemButton.btnSave.ToString(), false);
            this.Master.showSystemButton(SystemButton.btnCancel.ToString(), false);
            this.Master.showPanel(Area.DataArea.ToString(), false);
            */

            //2.取得前頁的session欄位



            //3.key fields
            if (this.Master.Master.nowStatus == "Copy")
                this.Master.KeyFields = "PHONE_DATE,PHONE_TIME";
            else
                this.Master.KeyFields = "CUST_NO,CUST_SNAME,PHONE_DATE,PHONE_TIME";

            if (Session["bolWE020"] != null)
            {
                this.bolWE020 = true;
                Session["bolWE020"] = null;

            }

        }
        #endregion


        #region setDefaultValue：欄位預設值
        /// <summary>
        /// 設定欄位預設值
        /// 本頁作業：無作用
        /// </summary>
        private void setDefaultValue()
        {
            //1.form
            //this.USER_ID.Text = (String)Session["USER_ID"];
            if (this.Master.Master.nowStatus == "Add" || this.Master.Master.nowStatus=="Copy")
            {
                this.PHONE_DATE.Text = System.DateTime.Now.ToString("yyyy/MM/dd");
                this.PHONE_TIME.Text = System.DateTime.Now.ToString("HH:mm");
                this.KEY_USER.Text = this.Master.Master.EmployeeName;
            }

            this.RE_DATE.Text = System.DateTime.Now.ToString("yyyy/MM/dd");
            this.RE_TIME.Text = System.DateTime.Now.ToString("HH:mm");

            //this.fun.Visible = (this.Master.Master.nowStatus == "Upd");
            this.addEMP_NAME.Attributes.Add("readonly", "readonly");
            this.spanClose.Visible = (this.Master.Master.nowStatus == "Upd");

            if (this.bolWE020 && this.Master.Master.nowStatus=="Add")
            {
                this.CUST_NO.Text = ((String)Session["CUST_NO"]);
                this.APLY_NO.Text = ((String)Session["APLY_NO"]);
                this.CUST_SNAME.Text = ((String)Session["CUST_NAME"]);
                this.CUST_NO.Editing(false);
                this.CUST_SNAME.Editing(false);
                Session["APLY_NO"] = null;
                Session["CUST_NO"] = null;
                Session["CUST_NAME"] = null;
            }
        }
        #endregion


        #region grid function
        protected void GridFunc_Click(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
  
            DataTable dtGridSource;
            string strScript = "";
            switch (((Button)sender).ID)
            {

                case "btnDel":
                    dtGridSource = (DataTable)ViewState["GridSource"];
                    string strEMP_CODE = e.CommandName;
                    
                    dtGridSource.DeleteRows("EMP_CODE='" + strEMP_CODE + "'");
                    rptEdit.DataSource = dtGridSource;
                    rptEdit.DataBind();
                    this.upDetailEditing.Update();


                    break;

                case "btnAdd":
                    string strMessage = "";
                    if (this.addEMP_CODE.Text.Trim() == "")
                        strMessage += "[員工代號 ]";

                    

                    if (strMessage != "")
                    {
                        strMessage += " 必須輸入！";

                        strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                        this.setMessageBox(strMessage);
                        return;
                    }

                    dtGridSource = (DataTable)ViewState["GridSource"];
                    if (dtGridSource != null)
                    {
                        if (dtGridSource.Select("EMP_CODE='" + this.addEMP_CODE.Text.Trim() + "'").Length > 0)
                        {
                            strMessage += " 員工代號已存在，請確認！";

                            strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                            this.setMessageBox(strMessage);
                            return;
                        }
                    }

                    dtGridSource = GetGridSource(true);
                    if (ViewState["GridSource"] != null)
                    {
                        dtGridSource = (DataTable)ViewState["GridSource"];
                        rptEdit.DataSource = dtGridSource;
                        rptEdit.DataBind();
                    }
                    else
                    {
                        rptEdit.DataSource = null;
                        rptEdit.DataBind();

                    }



                    this.addEMP_CODE.Clearing();
                    this.addEMP_NAME.Clearing();
                    
                    this.upDetailEditing.Update();
                    break;

               
              
            }
        }


        //send mail
        private bool SaveGrid()
        {
            /*

            //發MAIL通知
            MailMessage message = new MailMessage("abc@yahoo.com.tw", address);//MailMessage(寄信者, 收信者)

            message.IsBodyHtml = true;

            message.BodyEncoding = System.Text.Encoding.UTF8;//E-mail編碼
            message.Subject = mysubject;//E-mail主旨
            message.Body = msg;//E-mail內容



            SmtpClient smtpClient = new SmtpClient("127.0.0.1");//設定E-mail Server和port

            smtpClient.Send(message);



            //  if (dts.Save())
            //      this.setProcessMessage("版次資料儲存完成！", false);
            //  else
            //      this.setProcessMessage("版次資料儲存失敗！", true);
            */
            return true;

        }


        private string GetGridSql()
        {
            string strSQL = "";
            strSQL += "select space(30) EMP_CODE,space(30) EMP_NAME  where 1=2";

            return strSQL;
        }



        private DataTable GetGridSource(bool bolAdd)
        {
            DataTable dtGridSource = null;

            if (ViewState["GridSource"] == null)
            {

                dtGridSource = dg.GetDataTable(this.GetGridSql());               

                ViewState["GridSource"] = dtGridSource;
            }
            else
                dtGridSource = (DataTable)ViewState["GridSource"];

            if (bolAdd)
            {
                dtGridSource = (DataTable)ViewState["GridSource"];
                //this.updateGrid(dtGridSource, rptEdit); 不存檔
                DataRow dtRow = dtGridSource.NewRow();
                dtRow["EMP_CODE"] = this.addEMP_CODE.Text.Trim();
                dtRow["EMP_NAME"] = this.addEMP_NAME.Text.Trim();
                

                dtGridSource.Rows.Add(dtRow);
                ViewState["GridSource"] = dtGridSource;
            }

            return dtGridSource;
        }
        #endregion

        protected void Closed_Click(object sender,EventArgs e)
        {
            this.Master.nowStatusName = ((Button)sender).Text;
            this.Master.Save_Click(sender, new CommandEventArgs("Close",null));
            
            //this.Save_Click("Close");
        }

        #region Save_Click：儲存鍵觸發
        /// <summary>
        /// 本頁作業：新增、修改、刪除
        /// </summary>
        /// <param name="strStatus">
        /// 系統作業狀態：
        /// Add.新增。        
        /// Upd.修改，若有在編輯區下Command，則搭配this.Master.SubStatus。
        /// Del.刪除。
        /// </param>
        /// <returns>0:驗證失敗／1.處理失敗／2.處理成功</returns>
        private int Save_Click(string strStatus)
        {
            if (!DataCheck(strStatus))
                return 0;

            this.SaveGrid();


            DataTable dt = dts.GetTable("OR_SRV_REC", "CUST_NO='" + this.CUST_NO.Text.Trim() + "' and PHONE_DATE='" + this.PHONE_DATE.Text.Trim().Replace("/", "") + "' and PHONE_TIME='" + this.PHONE_TIME.Text.Trim().Replace("/", "") + "'");
            DataRow dr;
            switch (strStatus)
            {

                case "Close":
                case "Add":
                case "Copy":
                case "Upd":
                    if (dt.Rows.Count == 0)
                    {
                        dr = dt.NewRow();
                        dr["CUST_NO"] = this.CUST_NO.Text.Trim();
                        
                        
                        dr["KEY_USER"] = this.Master.Master.CorpAcct;
                        dr["SRV_REC_STS"] = (this.chkSTS.Checked  ? "1" : "");
                        dr["PHONE_DATE"] = this.PHONE_DATE.Text.Trim().Replace("/", "");
                        dr["PHONE_TIME"] = this.PHONE_TIME.Text.Trim();
                     

                        dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                    }
                    else
                        dr = dt.Rows[0];

                    dr["REC_TITLE"] = this.REC_TITLE.Text.Trim();
                    dr["APLY_NO"] = this.APLY_NO.Text.Trim();
                    dr["RE_DATE"] = this.RE_DATE.Text.Trim().Replace("/", "");
                    dr["RE_TIME"] = this.RE_TIME.Text.Trim();
                    dr["REC_CONTENT"] = this.REC_CONTENT.Text.Trim();
                    if (strStatus == "Close" || (strStatus=="Add" && !this.chkSTS.Checked) )
                    {
                        dr["CLOSED_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["SRV_REC_STS"] = "9";
                    }

                    if (strStatus != "Close" && this.chkSTS.Checked)
                    {
                        dr["CLOSED_DATE"] = "";
                        dr["SRV_REC_STS"] = "1";
                    }



                    dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                    dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                    dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                    if (dt.Rows.Count == 0)
                        dt.Rows.Add(dr);



                    break;
                case "Del":
                    dt.DeleteRows();


                    break;

            }

            if (dts.Save())
            {
                if (chkSupport.Checked)
                {
                    /*報修人帳號--->登入者帳號
報修人姓名--->登入者中文姓名
報修日期--->當天日期
報修主旨--->補發票影本○+合約編號+○客戶名稱
           範例:  補發票影本 105O2009040125 中鼎工程
報修內容--->客服電話記錄內容
附檔--->空白
建檔人帳號--->登入者帳號
處理狀態--->0
標示刪除--->N
報修類別代號--->14營業型;15非營業型
主管簽核--->直屬主管帳號
核準日期--->當天日期
營業員--->該合約經辦
契約編號---> 合約編號*/
                    DataSetToSql dtsO = new DataSetToSql("orixConn");
                    DataTable dtO=dtsO.GetTable("Service", "1=2");

                    string boss = dg.GetDataRow("SELECT dbo.f_EmpIDToCorpAcct(b.EMP_CODE)  FROM OR_EMP a left join OR_EMP b on a.UPPER_BOSS=b.EMP_CODE WHERE a.EMP_CODE='" + this.Master.Master.EmployeeId + "'")[0].ToString().Trim();
                    string emp = dg.GetDataRow("select b.EMP_NAME from OR_CASE_APLY_BASE a left join OR_EMP b on a.EMP_CODE=b.EMP_CODE where APLY_NO='" + this.APLY_NO.Text.Trim() + "'")[0].ToString().Trim();
                    dr = dtO.NewRow();
                    dr["報修人帳號"] = this.Master.Master.UserId;
                    dr["報修人姓名"] = this.Master.Master.EmployeeName;
                    dr["報修日期"] = DateTime.Now;
                    dr["報修主旨"] ="補發票影本 "+this.APLY_NO.Text.Trim()+" "+this.CUST_SNAME.Text.Trim();
                    dr["報修內容"] = this.REC_CONTENT.Text.Trim();
                    dr["附檔"] = "";
                    dr["建檔人帳號"] = this.Master.Master.UserId; 
                    dr["處理狀態"] = "0";
                    dr["標示刪除"] = "N";
                    dr["報修類別代號"] = this.SupportType.SelectedValue;
                    dr["主管簽核"] = boss;
                    dr["核準日期"] = DateTime.Now;
                    dr["營業員"] = emp;
                    dr["契約編號"] = this.APLY_NO.Text.Trim();
                    dtO.Rows.Add(dr);
                    dtsO.Save();
                    
                }
                if (bolWE020)
                {
                    Session["CUST_NO"] = this.CUST_NO.Text.Trim();
                    Session["APLY_NO"] = this.APLY_NO.Text.Trim();                    
                    Session["CUST_NAME"] = this.CUST_SNAME.Text;

                    this.setProcessMessage(this.Master.nowStatusName + "資料處理成功!!", false);
                    this.setScript("window.location.replace('WE040.aspx');");
                    
                    return 0;
                }
                return 2;
            }
            else
            {
                if (bolWE020)
                {                 
                    this.setProcessMessage(this.Master.nowStatusName + "資料處理失敗!!", false);                 
                    return 0;
                }
                return 1;
            }
          
        }
        #endregion
             



        #region DataCheck：儲存前驗證
        /// <summary>
        /// 儲存前驗證
        /// </summary>
        /// <returns>true/false 成功/失敗 </returns>
        private bool DataCheck(string strStatus)
        {
            string strSQL = "";
            string strMessage = "";


            switch (strStatus)
            {
                case "Add":
                case "Copy":
                case "Close":
                    
                
                    if (this.CUST_NO.Text.Trim() == "")
                        strMessage += "[客戶代號]";

                    if (this.REC_TITLE.Text.Trim() == "")
                        strMessage += "[記錄標題]";


                    if (strMessage != "")
                        strMessage += "必須輸入！";

                    
                    if (strMessage != "")
                    {
                        strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                        this.setMessageBox(strMessage);
                        return false;
                    }

                    break;
            }


            return true;
        }
        #endregion

  

        #region Exit_Click：離開鍵觸發
        /// <summary>
        /// 離開鍵觸發Event,
        /// 本頁作業：無  
        /// </summary>
        private void Exit_Click()
        {
            if (bolWE020)
            {
                Session["CUST_NO"] = this.CUST_NO.Text.Trim();
                Session["APLY_NO"] = this.APLY_NO.Text.Trim();
                Session["CUST_NAME"] = this.CUST_SNAME.Text;

                this.Response.Redirect("WE040.aspx");
            }
        }
        #endregion


     
        /// <summary>
        /// 編輯區域裏有設command的按鍵被按下時, 將觸發此event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Data_Command(object sender, FormViewCommandEventArgs e)
        {


        }


       




        /// <summary>
        /// popup window 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PopUp_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            string strMessage = "";
           
        }
    }
}