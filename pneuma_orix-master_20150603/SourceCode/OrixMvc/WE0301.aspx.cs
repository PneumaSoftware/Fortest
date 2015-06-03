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
    public partial class WE0301 : PageParent
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

            if (Session["APLY_NO"] == null)
            {
                this.Response.Write("登錄錯誤!");
                this.Response.End();
                return;
            }

            this.APLY_NO.Text = ((string)Session["APLY_NO"]).Trim();
            this.Master.Master.nowStatus = "Upd";

            string strSQL = "";

            strSQL += " select * from OR_CASE_APLY_BASE";
            strSQL += " SELECT a.APLY_NO,a.EMP_CODE,a.CUST_NO, b.CUST_NAME,a.CONTACT,CTAC_TEL,FAX,";
            strSQL += " c.APRV_PAY_MTHD,a.Mobile,a.PAY_CTAC,a.PAY_TEL";
            strSQL += " FROM OR_CASE_APLY_BASE a left join OR_CUSTOM b on a.CUST_NO=b.CUST_NO";
            strSQL += " left join OR_CASE_APLY_EXE_COND c on c.APLY_NO=a.APLY_NO where APLY_NO='" + this.APLY_NO.Text.rpsText()+ "'";

            this.Master.queryString = strSQL;

            //3.key fields
            this.Master.KeyFields = "";

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


        /*    DataTable tb = this.GetGridSource(false);
            
            if (tb.Rows.Count > 0)
            {
                this.EFFDate = tb.Select("STATUS<>'D'", "EFF_DATE desc")[0]["EFF_DATE"].ToString();
            }
            this.rptEdit.DataSource = tb;
            this.rptEdit.DataBind();
            this.bolQuery = this.Master.Master.nowStatus == "Query" || this.Master.Master.nowStatus == "Del";  */
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

          
           
            DataTable dtBASE = dts.GetTable("OR_CASE_APLY_BASE", "APLY_NO='" + this.APLY_NO.Text.rpsText() + "'");
            DataTable dtPAY = dts.GetTable("OR_CASE_APLY_EXE_COND", "APLY_NO='" + this.APLY_NO.Text.rpsText() + "'");
            DataTable dtOBJ = dts.GetTable("OR_OBJECT", "OR_CODE in (select OR_CODE from OR_CASE_APLY_OBJ where APLY_NO='" + this.APLY_NO.Text.rpsText() + "')");
            DataTable dtMER = dts.GetTable("OR_CASE_INV_MERGE", "MERGE_NO in (SELECT MERGE_NO from OR_CASE_INV_MERGE_SET where APLY_NO='" + this.APLY_NO.Text.rpsText() + "'");
            DataTable dtAPART = dts.GetTable("OR_INV_APART_SET", "APLY_NO='" + this.APLY_NO.Text.rpsText() + "'");
            
            

            DataRow dr = null;

            switch (strStatus)
            {

                
                case "Upd":
                    if (dtBASE.Rows.Count > 0)
                    {
                        dr = dtBASE.Rows[0];

                        dr["CONTACT"] = this.CONTACT.Text.Trim();
                        dr["CTAC_TEL"] = this.CTAC_TEL.Text.Trim();
                        dr["FAX"] = this.FAX.Text.Trim();
                        dr["Mobile"] = this.MOBILE.Text.Trim();
                        dr["PAY_CTAC"] = this.PAY_CTAC.Text.Trim();
                        dr["PAY_TEL"] = this.PAY_TEL.Text.Trim();
                        dr["UPD_USER"] = this.Master.Master.EmployeeName;
                        dr["UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                    }

                    if (dtPAY.Rows.Count > 0)
                    {
                        dr = dtPAY.Rows[0];
                        dr["APRV_PAY_MTHD"] = APRV_PAY_MTHD.SelectedValue;
                        dr["UPD_USER"] = this.Master.Master.EmployeeName;
                        dr["UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                    }
                    DataRow[] aryDr;
                    if (dtOBJ.Rows.Count == 0)
                    {
            //             select b.OBJ_CODE,PROD_NAME,OBJ_LOC_ADDR,OBJ_LOC_CTAC,OBJ_LOC_TEL,OBJ_LOC_FAX FROM OR_CASE_APLY_OBJ a left join OR_OBJECT b on a.OBJ_CODE=b.OBJ_CODE 
        //where APLY_NO=@APLY_NO
                        for (int i=0;i<this.rptOBJ.Items.Count;i++)
                        {
                            aryDr=dtOBJ.Select("OBJ_CODE='" + this.rptOBJ.Items[i].FindControl("OBJ_CODE").value() + "'");
                            if (aryDr.Length > 0)
                            {
                                dr = aryDr[0];
                                dr["OBJ_LOC_ADDR"] = this.rptOBJ.Items[i].FindControl("OBJ_LOC_ADDR").value();
                                dr["OBJ_LOC_CTAC"] = this.rptOBJ.Items[i].FindControl("OBJ_LOC_CTAC").value();
                                dr["OBJ_LOC_TEL"] = this.rptOBJ.Items[i].FindControl("OBJ_LOC_TEL").value();
                                dr["OBJ_LOC_FAX"] = this.rptOBJ.Items[i].FindControl("OBJ_LOC_FAX").value();
                                dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                                dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                                dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                            }
                        }
                    }

                    if (dtMER.Rows.Count == 0)
                    {                       
                        for (int i = 0; i < this.rptMERGE.Items.Count; i++)
                        {
                            aryDr = dtOBJ.Select("MERGE_NO='" + this.rptOBJ.Items[i].FindControl("MERGE_NO").value() + "'");
                            if (aryDr.Length > 0)
                            {
                                dr = aryDr[0];
                                dr["RECVER"] = this.rptOBJ.Items[i].FindControl("RECVER").value();
                                dr["RECVERDEP"] = this.rptOBJ.Items[i].FindControl("RECVERDEP").value();
                                dr["POST_NUM"] = this.rptOBJ.Items[i].FindControl("POST_NUM").value();
                                dr["RECVERADDR"] = this.rptOBJ.Items[i].FindControl("RECVERADDR").value();
                               // dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                              //  dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                               // dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                            }
                        }
                    }
                 //    select ORD_NO,CUST_NO,CUST_NAME,RECV_DEPT,RECV_NAME,ADDR from OR_INV_APART_SET
                    if (dtAPART.Rows.Count > 0)
                    {
                        dr = dtBASE.Rows[0];

                        dr["RECV_DEPT"] = this.rptOBJ.Items[0].FindControl("RECV_DEPT").value();
                        dr["RECV_NAME"] =this.rptOBJ.Items[0].FindControl("RECV_NAME").value();
                        dr["ADDR"] = this.rptOBJ.Items[0].FindControl("ADDR").value();
                        // dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                        //  dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        // dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                    }

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