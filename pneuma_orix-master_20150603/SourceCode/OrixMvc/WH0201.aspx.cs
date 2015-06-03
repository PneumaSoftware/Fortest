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
    public partial class WH0201 : PageParent
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
            this.Master.KeyFields = "NUM";


            


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


            if (this.NUM.Text.Trim() != "")
                this.NUM.Editing(false);

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

            //day,code,sales,num,customer,

            switch (strStatus)
            {
                case "Add":
                case "Copy":
                case "Upd":
                case "Import":
                    if (this.DAY.Text.Trim() == "")
                        strMessage += "[日期]";

                    if (this.CODE.SelectedValue.Trim() == "")
                        strMessage += "[件數]";
                                      
                    if (this.SALES.Text.Trim() == "")
                        strMessage += "[營業人員]";

              
                    if (this.NUM.Text.Trim() == "")
                        strMessage += "[契約編號]";

                    if (this.CUSTOMER.Text.Trim() == "")
                        strMessage += "[客戶名稱]";

                    

                    if (strMessage != "")
                    {
                        this.setMessageBox(strMessage + "必須輸入！");
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


            
            DataTable dt = dts.GetTable("SL91", "NUM='" + this.NUM.Text + "'");
            DataRow dr = null;

            // strSQL += " num,[DAY],customer,term,sales,b.EMP_NAME,ST,COST,TOTAL,CODE,FUND,EXCULDE,SHARE,SHARE_NAME=c.EMP_NAME ,S_R,[授權類別],TAL,MACHINE,[標的物名稱],";
           // strSQL += " TR,CAPITA,DSCRPY_ME,MEMO,DSCRPY ";

            switch (strStatus)
            {

                case "Add":
                case "Import":
                case "Copy":
                case "Upd":
                    if (dt.Rows.Count == 0)
                    {
                        dr = dt.NewRow();
                        dr["NUM"] = this.NUM.Text.Trim();
                    }
                    else
                    {
                        dr = dt.Rows[0];
                        dr["NUM"] = this.NUM.Text.Trim();
                    }

                    //this.convertToEmployeeCode(this.EMP_CODE.Text);
                    dr["DAY"] = this.DAY.Text.Replace("/","");
                    dr["customer"] = this.CUSTOMER.Text.Trim();
                    dr["term"] = this.TERM.Text;
                    dr["sales"] =this.SALES.Text.Trim();
                    dr["ST"] = this.ST.SelectedValue;
                    dr["COST"] = this.COST.Text;
                    dr["TOTAL"] = this.TOTAL.Text ;
                    dr["TAL"] = this.TAL.Text;
                    dr["CODE"] = this.CODE.SelectedValue.Trim();
                    dr["FUND"] = this.FUND.Text;
                    dr["EXCULDE"] = this.EXCULDE.SelectedValue.Trim();
                    dr["SHARE"] = this.SHARE.Text.Trim();
                    dr["S_R"] = this.S_R.Text;
                    dr["授權類別"] = this.授權類別.SelectedValue;
                    dr["MACHINE"] = this.MACHINE.SelectedValue;
                    dr["標的物名稱"] = this.標的物名稱.Text;
                    dr["TR"] = this.TR.Text;
                    dr["CAPITA"] = this.CAPITA.Text;
                    dr["DSCRPY_ME"] = this.DSCRPY_ME.Text.Trim();
                    dr["MEMO"] = this.MEMO.SelectedValue;
                    dr["DSCRPY"] = this.DSCRPY.Text;
                    dr["NEW"] = "N";
                    dr["FIX"] = 0;
                    
                                        
                    dr["SAVE_DATE"] = System.DateTime.Now;
                    dr["SAVE_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                    if (dt.Rows.Count == 0)
                        dt.Rows.Add(dr);

                    

                    break;
                case "Del":
                    dt.DeleteRows();

                    break;

            }

            if (dts.Save())
            {

                cts.Execute("EXEC S_WH020_UPDATE");
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