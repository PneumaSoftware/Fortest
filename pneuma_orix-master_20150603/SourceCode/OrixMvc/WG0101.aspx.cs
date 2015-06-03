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
    public partial class WG0101 : PageParent
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



            //3.key fields
            this.Master.KeyFields = "SeqNo";


           



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

            if (this.Master.Master.nowStatus == "Add")
            {
                this.BANK_TYPE.SelectedIndex = 0;
            }

            if (this.COLL_MTHD.SelectedValue.ToLower() != "p")
            {
                this.PDC_PERCENT.bolEnabled = false;
                this.PDC_PERCENT.Text = "0";
            }
            //COLL_Change(this.PDC_PERCENT, null);
            //this.PDC_PERCENT.Editing(false);


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
                case "Upd":
                    if (this.BANK_NO.Text.Trim() == "")
                        strMessage += "[銀行代碼]";

                    if (this.CRD_AMT.Text.Trim() == "")
                        strMessage += "[授信額度]";

                    if (this.COLL_MTHD.Text.Trim() == "")
                        strMessage += "[擔保方式]";

                    if (this.CRD_DATE_TO.Text.Trim() == "")
                        strMessage += "[授信到期日]";

                    
                    if (strMessage != "")
                        strMessage += "必須輸入！";

                    if (this.PDC_PERCENT.Text.toInt()>200)
                        strMessage += "\\r\\n PDC擔保成數必須在0~200之間！";

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void COLL_Change(object sender, System.EventArgs e)
        {
            if (this.COLL_MTHD.SelectedValue.ToLower() == "p")
                this.PDC_PERCENT.Editing(true);
            else
            {
                this.PDC_PERCENT.Editing(false);
                this.PDC_PERCENT.Text = "0";
            }

            this.upPDC.Update();
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


            if (this.SeqNo.Text == "")
                this.SeqNo.Text = "0";

            DataTable dt = dts.GetTable("OR_BANK_AMT", "SeqNo=" + this.SeqNo.Text + "");
            DataRow dr = null;

            switch (strStatus)
            {

                case "Add":
                case "Copy":
                case "Upd":
                    if (dt.Rows.Count == 0)
                    {
                        dr = dt.NewRow();
                        dr["USED_CREDIT"] = "0";
                        dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                    }
                    else
                    {
                        dr = dt.Rows[0];                        
                    }


                    dr["BANK_NO"] = this.BANK_NO.Text;
                    dr["BANK_TYPE"] = this.BANK_TYPE.SelectedValue;
                    dr["LONG_SHORT_LOAN"] = this.LONG_SHORT_LOAN.SelectedValue;
                    dr["ISCYCLE"] = this.ISCYCLE.SelectedValue;                    
                    dr["CRD_AMT"] = this.CRD_AMT.Text;
                    dr["COLL_MTHD"] = this.COLL_MTHD.SelectedValue;
                    dr["PDC_PERCENT"] = this.PDC_PERCENT.Text;
                    dr["BOND_RATE"] = this.BOND_RATE.Text;
                    dr["CRD_DATE_TO"] = this.CRD_DATE_TO.Text.Replace("/", "");
                    dr["CAPT_CODE_DESC"] = this.CAPT_CODE_DESC.Text;
                    dr["REMARK"] = this.REMARK.Text;
                    dr["LAST_CHG_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                    

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
            switch (e.CommandName)
            {
                case "ChangePASS":

                   
                    break;
            }
        }
    }
}