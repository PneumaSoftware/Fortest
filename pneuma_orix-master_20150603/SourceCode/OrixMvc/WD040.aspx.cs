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
    public partial class WD040 : PageParent
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
            this.Master.pageDetail = "WD0401.aspx";

            /*
            

          
            //2.程式編輯功能 */            
            this.Master.bolDel = false;
            this.Master.bolAdd = false;
            
           // this.Master.bolDetail = true;
 
            

            //3.雖有編輯功能, 但不顯示修改的欄位
            //this.Master.bolUpd_Show = false;


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

            string strMessage = "";


            string strSQL = "";

            //前面欄位需與GRID對應, 為轉excel用
            strSQL += " select * from (select ORI_APLY_NO,ORI_PERIOD,";
            strSQL += " APLY_NO,PERIOD,YEAR_MONTH=MAX(YEAR_MONTH),";
            strSQL += " IMPORT_DATE=dbo.f_DateAddSlash(MAX(IMPORT_DATE)),STAR_DATE=dbo.f_DateAddSlash(MAX(STAR_DATE)),MSG=MAX(MSG)";
            strSQL += " from OR3_PAPER_USE_DTL_TMP where 1=1";
            if (strStatus == "Query")
            {
                if (this.INVO_NO.Text.Trim() != "")
                    strSQL += " and INVO_NO ='" + this.INVO_NO.Text.Trim() + "'";

                if (this.INV_DATE.Text.Trim() != "")
                    strSQL += " and INV_DATE ='" + this.INV_DATE.Text.Trim().Replace("/","") + "'";

                if (this.MAC_NO.Text.Trim() != "")
                    strSQL += " and MAC_NO ='" + this.MAC_NO.Text.Trim() + "'";
            }

            strSQL += " group by ORI_APLY_NO,ORI_PERIOD,APLY_NO,PERIOD) s where 1=1";            




            switch (strStatus)
            {
                case "Add":
                case "Del":
                case "Copy":
                    break;

                case "Upd":

                    string strAplyNo = "", strPAplyNo = "";
                    if (strStatus != "Add")
                        strPAplyNo = ((HiddenField)this.Master.masterRepeater("hiddenPAPLY_NO")).Value;

                    if (strStatus != "Add")
                        strAplyNo = ((HiddenField)this.Master.masterRepeater("hiddenAPLY_NO")).Value;

                    this.Master.dqueryString = strSQL + " and ORI_APLY_NO='" + strPAplyNo + "' and APLY_NO='"+ strAplyNo+"'";

                    
                    break;

                case "Query": //設定


                    if (!QueryCheck()) //查詢前驗證
                        return;

                    if (this.PAPLY_NO.Text.Trim()!="")
                        strSQL += " and ORI_APLY_NO like '%" + this.PAPLY_NO.Text.Trim() + "%'";

                    if (this.APLY_NO.Text.Trim() != "")
                        strSQL += " and APLY_NO like '%" + this.APLY_NO.Text.Trim() + "%'";

                    if (this.PERIOD_ST.Text.Trim() != "0")
                        strSQL += " and ORI_PERIOD >=" + this.PERIOD_ST.Text.Trim() + "";

                    if (this.PERIOD_EN.Text.Trim() != "0")
                        strSQL += " and ORI_PERIOD <=" + this.PERIOD_EN.Text.Trim() + "";

                    if (this.YEAR_MONTH.Text.Trim() != "")
                        strSQL += " and YEAR_MONTH ='" + this.YEAR_MONTH.Text.Trim().Replace("/", "") + "'";

                    if (this.IMPORT_DATE.Text.Trim() != "")
                        strSQL += " and IMPORT_DATE ='" + this.IMPORT_DATE.Text.Trim() + "'";
                                        
                 

                    this.Master.queryString = strSQL;
                    //  this.setScript("openDetail();");
                    //   return;


                    this.Master.querySort = "ORI_APLY_NO,ORI_PERIOD,APLY_NO,PERIOD";
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
