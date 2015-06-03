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
using System.Data.SqlClient;

namespace OrixMvc
{
    public partial class WA0501 : PageParent
    {

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

        public bool bolWA070
        {
            set { ViewState["bolWA070"] = value; }
            get { return (ViewState["bolWA070"] == null ? false : (bool)ViewState["bolWA070"]); }
        }

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
            if (Session["bolWA070"] != null)
            {

                WA050 myForm = new WA050();
                string strSQL = myForm.callFromWA070();


                strSQL += " and MAST_CON_NO='" + (String)Session["MAST_CON_NO"] + "'";
                this.Master.queryString = strSQL;

              //  this.Master.Master.nowStatus = "Appove";
                this.bolWA070 = true;
                Session["bolWA070"] = null;
                Session["MAST_CON_NO"] = null;
               
            }


            //3.key fields
              this.Master.KeyFields ="MAST_CON_NO,CUR_STS";
           
            
            //this.setDefaultValue();

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
            
            if (this.Master.Master.nowStatus=="Add" ||this.Master.Master.nowStatus=="Copy")
                this.APLY_DATE.Text = System.DateTime.Now.ToString("yyyy/MM/dd");   

            this.setCUSTOM();

        }
        #endregion


        private void setCUSTOM()
        {
            string strSQL = "";
            string strCustom = this.CUST_NO.Text.Trim();
            strSQL = " select  TAKER,BUILD_DATE=dbo.f_DateAddSlash(BUILD_DATE),BACKGROUND,SALES_RGT_ADDR,";
            strSQL += " CAPT_STR=dbo.f_ConditionGetDesc('CAPT_STR',CAPT_STR,'N'),";
            strSQL += " ORG_TYPE=dbo.f_ConditionGetDesc('ORG_TYPE',ORG_TYPE,'N'),";
            strSQL += " RGT_CAPT_AMT,EMP_PSNS,REAL_CAPT_AMT,MAIN_BUS_ITEM,";
            strSQL += " CREDIT_CUST,JUDGE_LVL,";
            strSQL += " HONEST_AGREEMENT,SECRET_PROMISE";
            strSQL += " from OR_CUSTOM where CUST_NO='" + strCustom + "' ";
            DataTable dt = dg.GetDataTable(strSQL);
            DataRow dr;
            if (dt.Rows.Count == 0)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }

            this.rptBaseCustom.DataSource = dt;
            this.rptBaseCustom.DataBind();
        }

        


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
                case "Upd":
                    if (this.APLY_DATE.Text.Trim() == "")
                        strMessage += "[申請日期]";

                    if (this.DEPT_CODE.Text.Trim() == "")
                        strMessage += "[部門代號]";

                    if (this.EMP_CODE.Text.Trim() == "")
                        strMessage += "[員工代號]";

                    if (this.CUST_NO.Text.Trim() == "")
                        strMessage += "[客戶代號]";

                    if (this.CASE_TYPE_CODE.Text.Trim() == "")
                        strMessage += "[案件類別]";

                    if (this.PRE_EXPIRY_DATE.Text.Trim() == "")
                        strMessage += "[預計到期日]";


                    

                    if (strMessage != "")
                    {                        
                         this.setMessageBox(strMessage+"必須輸入！");
                        return false;
                    }

                    if (this.PRE_EXPIRY_DATE.Text.ToString().CompareTo(this.APLY_DATE.Text.ToString()) <= 0)
                        strMessage += "[案件類別]";

                    if (strMessage != "")
                    {
                        this.setMessageBox("[預計到期日期]不得小於等於[申請日期]");
                        return false;
                    }

                    break;

                case "Del":
                    break;
            }


            return true;
        }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="E"></param>
        protected void Reload_Custom(object sender, System.EventArgs E)
        {
            this.setCUSTOM();
            this.upCustom.Update();
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
          //  if (this.bolWA070)
          //  {
          //      this.Master.Master.nowStatus = "Appove";
         //       strStatus = this.Master.Master.nowStatus;
          //  }

            if (!DataCheck(strStatus))
                return 0;



            string strMAST_CON_NO = "";

            string strDEPT = this.DEPT_CODE.Text.Trim();
            string strDate = this.APLY_DATE.Text.Trim().Replace("/","");

            if (strStatus == "Add" || strStatus == "Copy")
            {
                string STS = dg.GetDataRow("select CASE_STS from OR_CASE_TYPE where CASE_TYPE_CODE='" + this.CASE_TYPE_CODE.Text.Trim() + "'")[0].ToString();
                strMAST_CON_NO = dg.GetDataRow("exec s_GetNumber 'H','" + strDEPT + "" + STS + "','" + strDate.Substring(0, 4) + "','" + strDate.Substring(4, 2) + "'")[0].ToString();
            }
            else
                strMAST_CON_NO = this.MAST_CON_NO.Text.rpsText();

            string strSEQ = this.FILE_SEQ.Seq;
            DataTable dtF = dts.GetTable("OR3_FILE_STORE", "FILE_SEQ='" + strSEQ + "'");
            DataTable dt = dts.GetTable("OR3_MASTER_CONTRACT", "MAST_CON_NO='" + strMAST_CON_NO + "'");

            byte[] BINARY_FILE1 = this.FILE_SEQ.bImage;
            string strExtName1 = this.FILE_SEQ.ExtName;

            DataRow dr = null;

            switch (strStatus)
            {

                case "Cancel":
                    dr = dt.Rows[0];
                    dr["CUR_STS"] = "3";

                    dr["EXPIRY_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");

                    dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                    dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                    dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                    break;



                case "Add":
                case "Copy":
                case "Upd":
                case "Detail":
                case "Appove":
                    if (dt.Rows.Count == 0)
                    {
                        dr = dt.NewRow();
                        dr["MAST_CON_NO"] = strMAST_CON_NO;
                        dr["CUR_STS"] = "1";
                        dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                    }
                    else
                    {
                        dr = dt.Rows[0];
                    }
                    if (strStatus == "Appove")
                    {
                        dr["CUR_STS"] = "2";
                        dr["APRV_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                    }

                    dr["APLY_DATE"] = this.APLY_DATE.Text.Trim().Replace("/","");
                    dr["CASE_TYPE_CODE"] = this.CASE_TYPE_CODE.Text.Trim();
                    dr["DEPT_CODE"] = this.DEPT_CODE.Text.Trim();
                    
                    dr["EMP_CODE"] =dg.GetDataRow("select dbo.f_CorpAcctToEmpID('"+  this.EMP_CODE.Text.Trim()+"')")[0].ToString();
                    dr["CUST_NO"] = this.CUST_NO.Text.Trim();
                    dr["PRE_EXPIRY_DATE"] = this.PRE_EXPIRY_DATE.Text.Trim().Replace("/", "");
                    dr["SIGNED_NO"] = this.SIGNED_NO.Text.Trim();
                    dr["FILE_SEQ"] = this.FILE_SEQ.Seq;


                    dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                    dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                    dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                    if (dt.Rows.Count == 0)
                        dt.Rows.Add(dr);


                    if (strExtName1 != "")
                    {

                        if (dtF.Rows.Count == 0)
                        {
                            dr = dtF.NewRow();
                            dr["FILE_SEQ"] = this.FILE_SEQ.Seq;
                            dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                            dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                            dr["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                        }
                        else
                            dr = dtF.Rows[0];
                        dr["FILE_TYPE"] = strExtName1;
                        dr["BINARY_FILE"] = BINARY_FILE1;
                        
                        dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");

                        if (dtF.Rows.Count == 0)                        
                            dtF.Rows.Add(dr);
                    }

                    break;
              

            }

           // this.SaveGrid();
            if (dts.Save())
            {
              //  this.resetVerGrid();

                return 2;
            }
            else
                return 1;
            
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