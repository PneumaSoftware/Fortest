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
    public partial class WG0301 : PageParent
    {

        protected bool bolQuery
        {
            set { ViewState["bolQuery"] = value; }
            get { return (ViewState["bolQuery"] == null ? false : (bool )ViewState["bolQuery"]); }
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
            this.Master.KeyFields = "SeqNo,LOAN_SEQ";




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


            //this.Reload_ACCTNO(null, null);
            //this.BANK_ACCT_NO. = this.BANK_ACCT_NO.ToolTip; 
            

            DataTable tb = this.GetGridSource("DTL", false);
            this.rptDTL.DataSource = tb;
            this.rptDTL.DataBind();

            tb = this.GetGridSource("DTL_E", false);
            this.rptDTL_E.DataSource = tb;
            this.rptDTL_E.DataBind();
           
            this.bolQuery = this.Master.Master.nowStatus == "Query" || this.Master.Master.nowStatus == "Del";
            this.myPanel.Editing(false);


        }

        #endregion



        protected void Reload_ACCTNO(object sender, System.EventArgs e)
        {
            string strSQL = " select '' as BANK_ACCT_NO union all select BANK_ACCT_NO  from  ACC12 a where BANK_NO='" + this.BANK_NO.Text.Trim() + "' ";
            dg.ListBinding(this.BANK_ACCT_NO, strSQL);
            //this.BANK_ACCT_NO.Focus();

            //if (IsPostBack)
            //    this.upBANK_ACCT_NO.DataBind();
        }

        #region grid function
        protected void GridFunc_Click(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            this.bolQuery=false;
            DataTable dtGridSource;
            string strScript = "";
            string strMessage = "";
            switch (((Button)sender).ID)
            {


                case "btnDel_DTL":
                    dtGridSource = (DataTable)ViewState["GridSourceDTL"];
                    string RED_DATE = e.CommandName;
                    DataRow drDTL = dtGridSource.Select("RED_DATE='" + RED_DATE + "'")[0];
                    drDTL["STATUS"] = "D";
                    rptDTL.DataSource = dtGridSource;
                    rptDTL.DataBind();
                    this.upDTL.Update();
                    break;



                case "btnAdd_DTL":
                    
                    if (this.addRED_DATE.Text.Trim() == "")
                        strMessage += "[還款日期]";

                    if (this.addRED_AMT.Text.Trim() == "0")
                        strMessage += "[還款金額]";


                    if (strMessage != "")
                    {
                        strMessage += " 必須輸入！";

                        strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                        this.setMessageBox(strMessage);
                        return;
                    }
                    /*

                    if (ViewState["GridSourceDTL"] != null && this.addRED_DATE.Text.Trim() != "")
                    {
                        dtGridSource = (DataTable)ViewState["GridSourceDTL"];

                        if (dtGridSource.Select("RED_DATE='" + this.addRED_DATE.Text.Trim() + "'").Length > 0)
                            strMessage += "\\r\\n還款日期不得重複！";
                    }*/

                    if (strMessage != "")
                    {
                        strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                        this.setMessageBox(strMessage);
                        return;
                    }

                    dtGridSource = GetGridSource("DTL", true);
                   
                        rptDTL.DataSource = dtGridSource;
                        rptDTL.DataBind();
                        rptDTL.Visible = true;
                   



                    addRED_DATE.Clearing();
                    addRED_AMT.Clearing();
                    this.upDTL.Update();
                    break;

                
            }
        }


        private bool SaveGrid(string strType)
        {

            DataTable dt;
            string strSQL = "";
            DataRow dr;
            string strMessage = "";
            switch (strType)
            {
                

                case "DTL":
                    
                    this.updateGrid(this.GetGridSource(strType, false), rptDTL);

                    dt = dts.GetTable("OR3_BORROW_REAL_REPAY_DTL", "SeqNo='" + this.SeqNo.Text + "' and LOAN_SEQ=" + this.LOAN_SEQ.Text);
                    DataTable dtAMT = dts.GetTable("OR_BANK_AMT", "SeqNo=" + this.SeqNo.Text);

                    for (int i = 0; i < this.rptDTL.Items.Count; i++)
                    {
                        string STATUS = this.rptDTL.Items[i].FindControl("STATUS").value();
                        string RED_DATE = this.rptDTL.Items[i].FindControl("RED_DATE").value().Replace("/", "");
                        double RED_AMT = this.rptDTL.Items[i].FindControl("RED_AMT").value().toNumber();
                        string SEQ_NO = this.rptDTL.Items[i].FindControl("SEQ_NO").value();
                        
                        if (RED_DATE == "")
                            strMessage += "[還款日期]";
                        
                        if (RED_AMT == 0)
                            strMessage += "[還款金額]";

                        if (strMessage != "")
                        {
                            strMessage += " 必須輸入！";

                            strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                            this.setMessageBox(strMessage);
                            return false;
                        }


                        DataRow drAMT = dtAMT.Rows[0];
                                               
                        

                        switch (STATUS)
                        {
                            case "A":
                            case "U":
                                if (STATUS == "A")
                                {
                                    dr = dt.NewRow();
                                    dr["SeqNo"] = this.SeqNo.Text;
                                    dr["LOAN_SEQ"] = this.LOAN_SEQ.Text;
                                    dr["SEQ_NO"] = SEQ_NO;
                                    dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                                    dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                                    dr["ADD_TIME"] = System.DateTime.Now.ToString("HHmmss");
                                    if (drAMT["isCycle"].ToString().Trim()=="Y")
                                        drAMT["USED_CREDIT"] = drAMT["USED_CREDIT"].ToString().toNumber() - RED_AMT;                                    
                                }
                                else
                                {
                                    dr = dt.Select("SEQ_NO='" + SEQ_NO + "'")[0];

                                    if (drAMT["isCycle"].ToString().Trim() == "Y")
                                        drAMT["USED_CREDIT"] = drAMT["USED_CREDIT"].ToString().toNumber() + dr["RED_AMT"].ToString().toNumber() - RED_AMT;
                                }
                                dr["RED_DATE"] = RED_DATE.Replace("/", "");
                                dr["RED_AMT"] = RED_AMT;
                               

                                dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                                dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                                dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HHmmss");

                                if (STATUS == "A")
                                    dt.Rows.Add(dr);

                                break;

                            case "D":
                                dt.DeleteRows("RED_DATE='" + RED_DATE + "'");
                                drAMT["USED_CREDIT"] = drAMT["USED_CREDIT"].ToString().toNumber() + RED_AMT;
                                break;
                        }
                    }
                    break;

             
            }


            

          

            return true;

        }


        private string GetGridSql(string strType)
        {
            string strSQL = "";

            string strSEQ = this.SeqNo.Text;
            string strLSEQ = this.LOAN_SEQ.Text;

            if (strSEQ == "")
                strSEQ = "0";

            if (strLSEQ == "")
                strLSEQ = "0";

            switch (strType)
            {                
                    
                case "DTL":
                    strSQL = "select SEQ_NO,RED_DATE=dbo.f_DateAddSlash(RED_DATE),RED_AMT from OR3_BORROW_REAL_REPAY_DTL where SeqNo='" + strSEQ + "' and LOAN_SEQ=" + strLSEQ;
                    break;

                case "DTL_E":
                    strSQL = "select RED_DATE=dbo.f_DateAddSlash(RED_DATE),RED_AMT,INTEREST_YN from OR3_BORROW_EXP_REPAY_DTL where SeqNo='" + strSEQ + "' and LOAN_SEQ=" + strLSEQ;
                    break;
            }
            
            return strSQL;
        }



        private DataTable GetGridSource(string strType, bool bolAdd)
        {
            DataTable dtGridSource = null;

            if (ViewState["GridSource" + strType] == null)
            {

                dtGridSource = dg.GetDataTable(this.GetGridSql(strType));

                DataColumn DC = new DataColumn();
                DC.ColumnName = "STATUS";
                DC.DefaultValue = "";
                DC.DataType = Type.GetType("System.String");
                dtGridSource.Columns.Add(DC);


                ViewState["GridSource" + strType] = dtGridSource;
            }
            else
                dtGridSource = (DataTable)ViewState["GridSource" + strType];

            if (bolAdd)
            {
                dtGridSource = (DataTable)ViewState["GridSource" + strType];
                DataRow dtRow=null;
                switch (strType)
                {
                   

                    case "DTL":
                        this.updateGrid(dtGridSource, rptDTL);
                        dtRow = dtGridSource.NewRow();
                        dtRow["RED_DATE"] = addRED_DATE.Text.Trim();
                        dtRow["RED_AMT"] = addRED_AMT.Text.Trim();
                        dtRow["SEQ_NO"] = dtGridSource.Compute("MAX(SEQ_NO)","").ToString().toNumber() + 1;
                        break;

                
                }

                dtRow["Status"] = "A";
                dtGridSource.Rows.Add(dtRow);
                ViewState["GridSource" + strType] = dtGridSource;
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
                    
                    if (this.rptDTL.Items.Count==0)
                    {
                        this.setMessageBox("還款資料至少需輸入一筆！");
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


        
            if (!this.SaveGrid("DTL"))
                return 0;

        

                                    
            
            

            DataRow dr = null;

            switch (strStatus)
            {

                case "Add":
                case "Copy":
                case "Upd":
                   

                   

                    break;
                case "Del":
                    
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