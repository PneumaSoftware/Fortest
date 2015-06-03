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

namespace OrixMvc
{
    public partial class WC0201 : PageParent
    {

        protected bool bolQuery
        {
            set { ViewState["bolQuery"] = value; }
            get { return (ViewState["bolQuery"] == null ? false : (bool )ViewState["bolQuery"]); }
        }

        protected string  EFFDate
        {
            set { ViewState["EFFDate"] = value; }
            get { return (ViewState["EFFDate"] == null ? "" : (string)ViewState["EFFDate"]); }
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
            this.Master.KeyFields = "SEQ_NO";





        }
        #endregion


        #region 代入營業單位資料
        protected void FRC_DEP_CODE_changed(object obj, EventArgs e)
        {
            DataRow dr = dg.GetDataRow("select TEL,ADDRESS from OR3_FRC_DEP where FRC_CODE='" + this.FRC_CODE.Text.Trim() + "' and FRC_DEP_CODE='" + this.FRC_DEPT_CODE.SelectedValue + "'");
            string strScript = "";
            if (dr[0].ToString() != "" || dr[1].ToString() != "")
            {
                strScript="document.getElementById('"+ this.TEL.ClientID +"').value='"+ dr[0].ToString() +"'\n";
                strScript += "document.getElementById('" + this.ADDRESS.ClientID + "').value='" + dr[1].ToString() + "'\n";
                this.setScript(strScript);
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


            // this.rptFRC_VER.DataSourceID = "sqlFRC_VER";
            //  this.rptFRC_VER.DataBind();


      //      DataTable tb = this.GetGridSource(false);
            
        //    if (tb.Rows.Count > 0)
          //  {
           //     this.EFFDate = tb.Select("STATUS<>'D'", "EFF_DATE desc")[0]["EFF_DATE"].ToString();
            //}
           // this.rptEdit.DataSource = tb;
           // this.rptEdit.DataBind();
           // this.bolQuery = this.Master.Master.nowStatus == "Query" || this.Master.Master.nowStatus == "Del";  
        }

        #endregion

               
       

        #region grid function
        protected void GridFunc_Click(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            this.bolQuery=false;
            DataTable dtGridSource;
            string strScript = "";
            switch (((Button)sender).ID)
            {

                case "btnDel":
                    dtGridSource = (DataTable)ViewState["GridSource"];
                    string strFRC_DEPT_CODE = e.CommandName;
                    DataRow drFR = dtGridSource.Select("FRC_DEP_CODE='" + strFRC_DEPT_CODE  + "'")[0];
                    drFR["STATUS"] = "D";
                    rptEdit.DataSource = dtGridSource;
                    rptEdit.DataBind();
                    this.upDetailEditing.Update();
                    this.setScript("openDetail();");

                    break;

                case "btnAdd":
                    string strMessage = "";
                    if (this.addFRC_DEP_CODE.Text.Trim() == "")
                        strMessage += "[代號 ]";

                    if (this.addFRC_DEP_NAME.Text.Trim() == "")
                        strMessage += "[名稱]";

                    if (this.addFRC_DEP_SNAME.Text.Trim() == "")
                        strMessage += "[簡稱]";


                    if (strMessage != "")
                    {
                        strMessage += " 必須輸入！";

                        strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                        this.setScript("openDetail();");
                        this.setMessageBox(strMessage);
                        return;
                    }

                    dtGridSource = (DataTable)ViewState["GridSource"];
                    if (dtGridSource.Select("FRC_DEP_CODE='" + this.addFRC_DEP_CODE.Text.Trim() + "'").Length > 0)
                    {
                        strMessage += " 代號已存在，請確認！";
                        this.setScript("openDetail();");
                        strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                        this.setMessageBox(strMessage);
                        return;
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



                    this.addFRC_DEP_CODE.Clearing();
                    this.addFRC_DEP_NAME.Clearing();
                    this.addFRC_DEP_SNAME.Clearing();
                    this.addTEL.Clearing();
                    this.addADDRESS.Clearing();
                    this.upDetailEditing.Update();
                    this.setScript("openDetail();");

                    break;

                case "btnEdit":
                    if (SaveGrid())
                    {
                        this.setProcessMessage("資料處理成功!", false);
                        this.setScript("window.parent.closePopUpWindow();");
                    }
                    else
                    {
                        this.setProcessMessage("資料處理失敗!", false);
                    }
                    break;

                case "btnMaintain":
                    if (this.FRC_CODE.Text.Trim() == "")
                    {                        
                       // this.setScript("openDetail();");
                        this.setMessageBox("供應商代號必須輸入！");
                        return;
                    }
                    ViewState["GridSource"] = null;
                    dtGridSource = this.GetGridSource(false);
                    rptEdit.DataSource = dtGridSource;
                    rptEdit.DataBind();
                    this.upDetailEditing.Update();
                    this.setScript("openDetail();");
                    break;
            }
        }



        private bool SaveGrid()
        {



            DataTable dt = dts.GetTable("OR3_FRC_DEP", "FRC_CODE='" + this.FRC_CODE.Text.rpsText() + "'");
            DataRow dr;

            this.updateGrid(this.GetGridSource(false), rptEdit);
            DataTable dtDEP=this.GetGridSource(false);

            /*END 勿動 更新Grid*/


            if (this.rptEdit.Items.Count==0)
            {
                this.setMessageBox("無資料可儲存，請確認！");
                this.setScript("openDetail();");
                return false;
            }

            for (int i = 0; i < this.rptEdit.Items.Count; i++)
            {
                string STATUS = this.rptEdit.Items[i].FindControl("STATUS").value();
                string FRC_DEP_CODE = this.rptEdit.Items[i].FindControl("FRC_DEP_CODE").value();
                string FRC_DEP_NAME = this.rptEdit.Items[i].FindControl("FRC_DEP_NAME").value();
                string FRC_DEP_SNAME = this.rptEdit.Items[i].FindControl("FRC_DEP_SNAME").value();
                string TEL = this.rptEdit.Items[i].FindControl("TEL").value();
                string ADDRESS = this.rptEdit.Items[i].FindControl("ADDRESS").value();

               
                string strMessage = "";
                if (FRC_DEP_CODE == "")
                    strMessage += "[代號]";

                if (FRC_DEP_NAME == "")
                    strMessage += "[名稱]";

                if (FRC_DEP_SNAME == "")
                    strMessage += "[簡稱]";


                if (strMessage != "")
                {
                    strMessage+= " 必須輸入！";

                    strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                    this.setMessageBox(strMessage);
                    this.setScript("openDetail();");
                    return false;
                }

                if (dtDEP.Select("FRC_DEP_CODE='" + FRC_DEP_CODE + "'").Length > 1)
                {
                    strMessage += " 代號重複輸入，請確認！";

                    strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                    this.setMessageBox(strMessage);
                    this.setScript("openDetail();");
                    return false;
                }  

               
                if (strMessage != "")
                {
                    strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                    this.setMessageBox(strMessage);
                    this.setScript("openDetail();");
                    return false;
                }

                switch (STATUS)
                {
                    case "A":


                        dr = dt.NewRow();
                        dr["FRC_CODE"] = FRC_CODE.Text.Trim();
                        dr["FRC_DEP_CODE"] = FRC_DEP_CODE;
                        dr["FRC_DEP_NAME"] = FRC_DEP_NAME;
                        dr["FRC_DEP_SNAME"] = FRC_DEP_SNAME;
                        dr["TEL"] = TEL;
                        dr["ADDRESS"] = ADDRESS;                         
                        dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["ADD_TIME"] = System.DateTime.Now.ToString("HHmmss");
                        dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HHmmss");
                        dt.Rows.Add(dr);

                        

                        break;

                    case "U":
                        dr = dt.Select("FRC_DEP_CODE='" + FRC_DEP_CODE + "'")[0];
                        dr["FRC_DEP_NAME"] = FRC_DEP_NAME;
                        dr["FRC_DEP_SNAME"] = FRC_DEP_SNAME;
                        dr["TEL"] = TEL;
                        dr["ADDRESS"] = ADDRESS;          
                        dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HHmmss");

                        break;

                    case "D":
                        dt.DeleteRows("FRC_DEP_CODE='" + FRC_DEP_CODE + "'");
                        break;
                }
            }

            if (dts.Save())
            {
                this.FrcDept_Click(this.btnDEPT, null);
                
                this.setProcessMessage("資料儲存完成！", false);
            }
            else
                this.setProcessMessage("資料儲存失敗！", true);

            return true;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void FrcDept_Click(object sender, EventArgs e)
        {
            string strValue = this.FRC_DEPT_CODE.SelectedValue; 
            this.FRC_DEPT_CODE.Items.Clear();
            dg.ListBinding(this.FRC_DEPT_CODE, "select FRC_DEP_CODE='',FRC_DEP_NAME=''  union all select FRC_DEP_CODE,FRC_DEP_NAME from OR3_FRC_DEP where FRC_CODE='" + this.FRC_CODE.Text.Trim() + "'");
            //this.FRC_DEPT_CODE.DataBind();

                       
            this.upDEPT.Update();
            try
            {
                this.FRC_DEPT_CODE.SelectedValue = strValue;
            }
            catch { }

        }

        private string GetGridSql()
        {
            string strSQL = "";
            strSQL += "select FRC_CODE,FRC_DEP_CODE,FRC_DEP_NAME,FRC_DEP_SNAME,TEL,ADDRESS from OR3_FRC_DEP where FRC_CODE='" + this.FRC_CODE.Text.Trim() + "'";

            return strSQL;
        }

       

        private DataTable GetGridSource(bool bolAdd)
        {
            DataTable dtGridSource = null;

            if (ViewState["GridSource"] == null)
            {

                dtGridSource = dg.GetDataTable(this.GetGridSql());

                DataColumn DC = new DataColumn();
                DC.ColumnName = "STATUS";
                DC.DefaultValue = "";
                DC.DataType = Type.GetType("System.String");
                dtGridSource.Columns.Add(DC);
               

                ViewState["GridSource"] = dtGridSource;
            }
            else
                dtGridSource = (DataTable)ViewState["GridSource"];

            if (bolAdd)
            {
                dtGridSource = (DataTable)ViewState["GridSource"];
                this.updateGrid(dtGridSource, rptEdit);
                DataRow dtRow = dtGridSource.NewRow();
                dtRow["FRC_CODE"] = this.FRC_CODE.Text.Trim();
                dtRow["FRC_DEP_CODE"] = addFRC_DEP_CODE.Text.Trim();
                dtRow["FRC_DEP_NAME"] = addFRC_DEP_NAME.Text.Trim();
                dtRow["FRC_DEP_SNAME"] = addFRC_DEP_SNAME.Text.Trim();
                dtRow["TEL"] = addTEL.Text.Trim();
                dtRow["ADDRESS"] = addADDRESS.Text.Trim();
                dtRow["Status"] = "A";

                dtGridSource.Rows.Add(dtRow);
                ViewState["GridSource"] = dtGridSource;
            }

            return dtGridSource;
        }
        #endregion


        #region DataCheck：儲存前驗證
        /// <summary>
        /// 儲存前驗證
        /// </summary>
        /// <returns>true/false 成功/失敗 </returns>
        private bool DataCheck(string strStatus)
        {
           
            string strMessage = "";

            
            switch (strStatus)
            {
                case "Add":
                case "Copy":
                case "Upd":
                    if (this.FRC_CODE.Text.Trim() == "")
                        strMessage += "[供應商代號]";

                    if (this.FRC_DEPT_CODE.Text.Trim() == "")
                        strMessage += "[營業單位]";

                    if (this.FRC_SALES_NAME.Text.Trim() == "")
                        strMessage += "[姓名]";
                 

                    if (strMessage != "")
                        strMessage += "必須輸入！";

                    if (strMessage != "")
                    {
                        strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                        this.setMessageBox(strMessage);
                        return false;
                    }

                    break;

                case "Del":
                    break;
            }


            return true;
        }
        #endregion


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

           // if (!this.SaveGrid())
           //     return 0;


            DataTable dt = dts.GetTable("OR3_FRC_SALES", "FRC_CODE='" + this.FRC_CODE.Text.rpsText() + "' and FRC_DEP_CODE='" + hidden_FRC_DEP_CODE.Value + "' and SEQ_NO=" + (SEQ_NO.Text.Trim() == "" ? "0" : SEQ_NO.Text.Trim()) + "");
            

            DataRow dr = null;

            switch (strStatus)
            {

                case "Add":
                case "Copy":
                case "Upd":
                    if (dt.Rows.Count == 0)
                    {
                        dr = dt.NewRow();
                        dr["FRC_CODE"] = this.FRC_CODE.Text.Trim();
                        dr["FRC_DEP_CODE"] = this.FRC_DEPT_CODE.Text.Trim();
                        int intSEQ = dg.GetDataRow("select count(*) from OR3_FRC_SALES where FRC_CODE='" + this.FRC_CODE.Text.rpsText() + "' and FRC_DEP_CODE='" + FRC_DEPT_CODE.SelectedValue + "'")[0].ToString().toInt() + 1;
                        dr["SEQ_NO"] = intSEQ; 
                        dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                    }
                    else
                        dr = dt.Rows[0];

                    dr["FRC_DEP_CODE"] = this.FRC_DEPT_CODE.Text.Trim();
                    dr["FRC_SALES_NAME"] = this.FRC_SALES_NAME.Text.Trim();
                    dr["JOB_NAME"] = this.JOB_NAME.Text.Trim();
                    dr["EXT"] = this.EXT.Text.Trim();
                    dr["LINE"] = this.LINE.Text.Trim();
                    dr["FACSIMILE"] = this.FACSIMILE.Text.Trim();
                    dr["MOBILE"] = this.MOBILE.Text.Trim();
                    dr["CELL2"] = this.CELL2.Text.Trim();
                    dr["EMAIL"] = this.EMAIL.Text.Trim();
                    dr["REMARK"] = this.REMARK.Text.Trim();
                   
                   
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
                return 2;
            }
            else
                return 1;
        }
        #endregion



        
        #region Exit_Click：離開鍵觸發
        /// <summary>
        /// 離開鍵觸發Event,
        /// 本頁作業：無  
        /// </summary>
        private void Exit_Click()
        {
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
            /*switch (e.CommandName)
            {
                case "ChangePASS":

                   
                    break;
            }*/
        }
    }
}