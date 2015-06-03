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
    public partial class WF030 : PageParent
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
            this.Master.pageDetail = "WF0301.aspx";

            /*
            

            //2.程式編輯功能 */
            this.Master.bolAdd = false;
            this.Master.bolCopy = false;
            this.Master.bolDel = false;
          
 
            

            //3.雖有編輯功能, 但不顯示修改的欄位
            //this.Master.bolUpd_Show = false;
            this.setDefaultValue();

        }
        #endregion



        #region setDefaultValue：欄位預設值
        /// <summary>
        /// 設定欄位預設值
        /// 本頁作業：無作用
        /// </summary>
        private void setDefaultValue()
        {
            this.EMP_CODE.Text = this.Master.Master.CorpAcct;
            this.EMP_NAME.Text = this.Master.Master.EmployeeName;

            
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
            string strSQL = "exec s_WF030_Grid ";
            string strMessage = "";

           
            
            

            switch (strStatus)
            {
                case "Add":
                case "Copy":
                    break;

                case "Upd":
                    string strAPLY = "";
                    if (strStatus != "Add")
                        strAPLY = ((HiddenField)this.Master.masterRepeater("hiddenAPLY")).Value;

                   
                    this.Master.dqueryString = strSQL + " @APLY_NO='" + strAPLY + "'"; 

                    break;


                case "Del":
                    break;
                

                case "Query": //設定

                    if (this.EMP_CODE.Text.Trim() == "")
                    {
                        strMessage = "員工代號必須輸入！";
                        this.setMessageBox(strMessage);
                        return;
                    }

                   

                    //前面欄位需與GRID對應, 為轉excel用
                    strSQL += " @PCorp_Acct='" + this.EMP_CODE.Text.rpsText() + "',@PEMP_NAME='" + this.EMP_NAME.Text.rpsText() + "'";

                    this.Master.queryString = strSQL;
                    //  this.setScript("openDetail();");
                    //   return;

                                     

                   
                    this.Master.querySort = "APLY_NO";
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
