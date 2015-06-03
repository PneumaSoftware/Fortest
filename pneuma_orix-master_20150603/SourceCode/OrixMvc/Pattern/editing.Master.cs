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
using VS2008.Module;
using System.IO;

namespace OrixMvc.Pattern
{

    /// <summary>
    /// Alinta：按下離開後
    /// </summary>       
    public delegate void ExitDelegate();

    /// <summary>
    /// Alinta：create delegate for Save events
    /// </summary>
    /// <param name="strStatus">作業狀態</param>       
    public delegate int SaveDelegate(string strStatus);

 
    /// <summary>
    /// Alinta：create delegate for DataArea's command events
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void DataDelegate(object sender, FormViewCommandEventArgs e);


    /// <summary>
    /// Alinta：create delegate for DataArea's command events
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void loadDelegate();


   

    public partial class editing : System.Web.UI.MasterPage
    {


        /// <summary>
        /// 將MasterPage的Save event傳回主網頁
        /// </summary>
        public event SaveDelegate SaveEvent;


        /// <summary>
        /// 將MasterPage的Data event傳回主網頁
        /// </summary>
        public event DataDelegate EditEvent;

        
        /// <summary>
        /// 將MasterPage的Exit event傳回主網頁
        /// </summary>
        public event ExitDelegate ExitEvent;


        /// <summary>
        /// 將setDefaultValue的masterLoadEvent傳回主網頁
        /// </summary>
        public event loadDelegate loadEvent;


      
        /// <summary>
        /// 取得/設定 key fields
        /// </summary>
        /// <returns></returns>
        public string KeyFields
        {
            set { ViewState["KeyFields"] = value; }
            get { return (ViewState["KeyFields"] == null ? "" : (String)ViewState["KeyFields"]); }
        }


        /// <summary>
        /// 取得query String
        /// </summary>
        /// <returns></returns>
        public string queryString
        {
            set { ViewState["queryString"] = value; }
            get { return (ViewState["queryString"] == null ? "" : (String)ViewState["queryString"]); }
        }


        /// <summary>
        /// 取得目前Grid的編輯列
        /// </summary>
        /// <returns></returns>
        public int GridIndex
        {
            set { ViewState["GridIndex"] = value; }
            get { return (ViewState["GridIndex"] == null ? 0 : (int)ViewState["GridIndex"]); }
        }

        /// <summary>
        /// 是否有返回功能
        /// </summary>
        /// <returns></returns>
        public bool bolExit
        {
            set { ViewState["bolExit"] = value; }
            get { return (ViewState["bolExit"] == null ? true  : (bool)ViewState["bolExit"]); }
        }

        /// <summary>
        /// 是否有儲存功能
        /// </summary>
        /// <returns></returns>
        public bool bolSave
        {
            set { ViewState["bolSave"] = value; }
            get { return (ViewState["bolSave"] == null ? true : (bool)ViewState["bolSave"]); }
        }

        /// <summary>
        /// 目前狀態中文名
        /// </summary>
        /// <returns></returns>
        public string nowStatusName
        {
            set { ViewState["nowStatusName"] = value; }
            get { return (ViewState["nowStatusName"] == null ? "" : (String)ViewState["nowStatusName"]); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack)
            {
                if (this.queryString == "")
                {
                    if (Session["dqueryString"] != null)
                        this.queryString = ((String)Session["dqueryString"]).Trim();
                }

                if (this.Master.nowStatus == "Detail")
                    this.bolSave = false;

                this.setParms(this.Master.nowStatus);



            }
            else
            {
                Session["queryString"] = this.queryString;

            }
        }


        


        public  void setParms(string strStatus)
        {


            if (this.Master.nowStatus != "Upd" && this.Master.nowStatus != "Add" && this.Request.Url.ToString().ToLower().IndexOf("wa1301") == -1 && this.Request.Url.ToString().ToLower().IndexOf("wa0601") == -1 && this.Request.Url.ToString().ToLower().IndexOf("wb0101") == -1)
                this.btnEdit.Text = this.Master.nowStatusName;

            if (this.queryString == "")
                return;

            this.QuerySqlDataSource.SelectCommand = this.queryString;

            switch (strStatus)
            {
                case "Add":

                    this.masterFormView.ChangeMode(FormViewMode.Insert);
                    this.masterFormView.PageIndex = 0;
                    this.masterFormView.DataBind();
                    loadEvent();
                    
                        
                    break;
                /*case "Detail": 
                case "Upd":
                case "Copy":
                case "Del":*/
                default:
                    this.masterFormView.ChangeMode(FormViewMode.Edit);
                    this.masterFormView.PageIndex = 0;
                    this.masterFormView.DataBind();

                    if (this.Master.nowStatus == "Detail")
                        this.bolSave = false;

                    if (this.Master.nowStatus == "Del" || this.Master.nowStatus == "Detail" )
                    {
                        this.masterFormView.Editing(false);
                    }
                    else
                    {
                        if (this.Master.nowStatus == "Cancel")
                            this.masterFormView.Editing(false);

                     //   if (this.Master.nowStatus != "Upd")
                      //      this.btnEdit.Text = this.Master.nowStatusName;

                        string[] aryKeys = this.KeyFields.Split(',');
                        for (int i = 0; i < aryKeys.Length; i++)
                        {
                            if (aryKeys[i].ToString() != "")
                            {
                                if (this.Master.nowStatus == "Upd")
                                    this.masterFormView.FindControl("editingArea").FindControl(aryKeys[i].ToString()).Editing(false);
                                else if (this.Master.nowStatus == "Copy")
                                    this.masterFormView.FindControl("editingArea").FindControl(aryKeys[i].ToString()).Clearing();                                
                            }
                        }
                    }
                    loadEvent();
                    break;


            }
        }

       
        /// <summary>
        /// 儲存鍵
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Save_Click(object sender, CommandEventArgs e)
        {

           
            int intReturn = 0;

           
            if (SaveEvent != null)
            {

               

                string strProcess="";
                
                

                if (this.nowStatusName != "")
                {
                    this.nowStatusName = ((Button)sender).Text.Trim();
                    intReturn = SaveEvent(e.CommandName);
                }
                else
                {
                    switch (this.Master.nowStatus)
                    {
                        case "Add":

                            this.nowStatusName = "新增";                            
                            break;

                        case "Copy":
                            this.nowStatusName = "複製";
                            break;

                        case "Upd":
                            this.nowStatusName = "修改";
                            break;

                        case "Del":
                            this.nowStatusName = "刪除";
                            break;
                    }
                    intReturn = SaveEvent(this.Master.nowStatus);
                }

                if (intReturn == 1)
                    strProcess = "處理失敗!!";
                else
                {
                    strProcess = "處理成功!!";
                }

                if (intReturn == 0)
                {
                    this.nowStatusName = "";
                    return;
                }
                this.setProcessMessage(this.Master.nowStatusName+strProcess, intReturn == 1);
                if (intReturn==2)
                    returnPage();
            }

            this.nowStatusName = "";
            //this.returnPage();   
        }


        /// <summary>
        /// 設定狀態的訊息列
        /// </summary>
        /// <param name="strMessage">訊息</param>
        /// <param name="bolError">是否為Error訊息,如處理失敗..等</param>
        private void setProcessMessage(string strMessage, bool bolError)
        {
            //string strScript = "window.parent.document.getElementById('divMessage').innerHTML=\"" + strMessage + "\";\n";
            //strScript += "window.parent.document.getElementById('divMessage').style.color=\"" + (bolError ? "red" : "") + "\";\n";
            string strScript = "";
            if (bolError)    
            {
                strScript = "try{window.parent.errorMessage('錯誤訊息','" + strMessage + "');}\n";
                strScript += "catch(er){window.parent.parent.errorMessage('錯誤訊息','" + strMessage + "');}\n";
                strScript += "finally{}\n";
            }
            else
            {
                strScript = "try{window.parent.slideMessage('處理訊息','" + strMessage + "');}\n";
                strScript += "catch(er){window.parent.parent.slideMessage('處理訊息','" + strMessage + "');}\n";
                strScript += "finally{}\n";
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "MyProcessScript", strScript, true);

        }

        protected void Exit_Click(object sender, EventArgs e)
        {
            if (ExitEvent != null)
                ExitEvent();

            this.returnPage();
        }

        /// <summary>
        /// 回查詢頁
        /// </summary>
        public void returnPage()
        {
            Session["nowStatus"] = "Query";
            string strScript = "contentChange('frameContent');";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "pageChange", strScript, true);
        }



        /// <summary>
        /// 編輯區域裏有設command的按鍵被按下時, 將觸發此event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DataArea_onCommand(object sender, FormViewCommandEventArgs e)
        {
                      

            if (EditEvent != null)
                EditEvent(sender, e);

        }


        /// <summary>
        /// 取得master page上任何在頁面的物件
        /// </summary>
        /// <param name="strID">物件ID</param>
        /// <returns>物件本身</returns>
        public object masterFindControl(string strID)
        {
            return this.masterFindControl(strID);
        }

    }
}
