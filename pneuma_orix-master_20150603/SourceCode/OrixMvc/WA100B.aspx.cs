﻿using System;
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
    public partial class WA100B : PageParent
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

            this.Master.StatusEvent += new StatusDelegate(Status_Click);
            this.Master.SelectEvent += new SelectDelegate(Select_Command);
            this.Master.QueryEvent += new QueryDelegate(QueryArea_Command);

            if (!IsPostBack)
                this.setParms();
            //***************************end 勿動****************************
        }
        #endregion



        #region setParms：設定公共參數
        /// <summary>
        /// 設定公共參數-->順序不得互換
        /// 1.編輯頁的URL
        /// 2.程式編輯功能(新增,修改,刪除)
        /// 3.雖有編輯功能, 但不顯示修改的欄位
        /// </summary>
        private void setParms()
        {
            //1.
            this.Master.pageDetail = "WA1001.aspx";

            /*
            

            //2.程式編輯功能
            this.Master.setEditingFunction(false, true, false);

            this.Master.showSystemButton(SystemButton.btnSave.ToString(), false);
            this.Master.showSystemButton(SystemButton.btnCancel.ToString(), false);
            this.Master.showPanel(Area.DataArea.ToString(), false);
            */
            this.Master.bolAdd=false;

            /*3.雖有編輯功能, 但不顯示修改的欄位
            this.Master.bolUpd_Show = false;*/


        }
        #endregion



        #region setDefaultValue：欄位預設值
        /// <summary>
        /// 設定欄位預設值
        /// 本頁作業：無作用
        /// </summary>
        private void setDefaultValue()
        {

        }
        #endregion



        #region Select_Command：查詢明細選取後執行該Event
        /// <summary>
        /// 查詢明細選取後執行該Event,
        /// 本頁作業：無  
        /// </summary>
        private void Select_Command()
        {
        }
        #endregion



        #region QueryCheck：查詢前驗證
        /// <summary>
        /// 查詢前的驗證
        /// </summary>
        /// <returns>驗證成功或失敗：true/false</returns>
        private bool QueryCheck()
        {
            string strMessage = "";
            if (this.APLY_NO.Text.Trim() == "")
            {
                strMessage = "\\r\\n申請書編號必須輸入！";
            }

            if (strMessage != "")
            {
                strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                this.setMessageBox(strMessage);
                return false;
            }

            return true;
        }
        #endregion


        #region Status_Click：按下作業狀態鍵(新增／修改／刪除／查詢)後所觸發的Event
        /// <summary>
        /// 按下作業狀態功能鍵後
        /// </summary>
        /// <param name="strStatus">作業狀態名稱</param>
        private void Status_Click(string strStatus)
        {

            string strSQL = "";
            //前面欄位需與GRID對應, 為轉excel用
            strSQL += " select '1案件' POLICY_SUBJECT,'' PROD_NAME,AA.ASUR_TYPE_CODE,AT.ASUR_TYPE_NAME,AA.AMOUNT,";
            strSQL += " isnull((select MAX(ASUR_E_DATE) from OR3_CASE_APLY_POLICY ";
            strSQL += "  where APLY_NO=AA.Aply_no and POLICY_SUBJECT='1' and ASUR_TYPE_CODE=AA.Asur_type_code),'') as ASUR_E_DATE,APLY_NO ";
            strSQL += " from OR_CASE_APLY_ASUR AA inner join OR_ASUR_TYPE AT on AA.ASUR_TYPE_CODE=AT.ASUR_TYPE_CODE";
            strSQL += " where AA.APLY_NO='" + this.APLY_NO.Text.rpsText() + "'";
            strSQL += " union all";
            strSQL += " select '2動產擔保' POLICY_SUBJECT,MO.PROD_NAME,MO.ASUR_TYPE_CODE,AT.ASUR_TYPE_NAME,MO.ASUR_AMOUNT,";
            strSQL += " isnull((select MAX(ASUR_E_DATE) from OR3_CASE_APLY_POLICY ";
            strSQL += " where APLY_NO=MO.Aply_no and POLICY_SUBJECT='2' and ASUR_TYPE_CODE=MO.Asur_type_code),'') as ASUR_E_DATE,APLY_NO ";
            strSQL += " from OR_MOBJECT MO inner join OR_ASUR_TYPE AT on MO.ASUR_TYPE_CODE=AT.ASUR_TYPE_CODE";
            strSQL += " where MO.APLY_NO='" + this.APLY_NO.Text.rpsText() + "'";

          
            this.Master.dqueryString = strSQL;


            switch (strStatus)
            {

                case "Upd":
                   // Session["POLICY_SUBJECT"] = ((HiddenField)this.Master.masterRepeater("POLICY_SUBJECT")).Value;
                  //  Session["ASUR_TYPE_CODE"] = ((HiddenField)this.Master.masterRepeater("ASUR_TYPE_CODE")).Value;
                  //  Session["APLY_NO"] = this.APLY_NO.Text.Trim();

                    this.Master.dqueryString = strSQL;

                    break;

                case "Query": //設定


                    //  this.setScript("openDetail();");
                    //   return;

                    if (!QueryCheck()) //查詢前驗證
                        return;


                    this.Master.queryString = strSQL;

                    this.Master.querySort = "policy_subject";
                    this.Master.setSqlQuery(1);


                    break;
            }

        }
        #endregion


        #region QueryArea_Command：在查詢明細的自訂command物件被觸發時，啟動該event
        /// <summary>
        /// 在查詢明細的自訂command物件被觸發時，啟動該event
        /// </summary>
        /// <param name="sender">ListView </param>
        /// <param name="e">Command Data</param>
        private void QueryArea_Command(object sender, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            
        }
        #endregion


    }
}
