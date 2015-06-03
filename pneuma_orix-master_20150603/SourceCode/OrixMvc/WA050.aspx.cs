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
    public partial class WA050 : PageParent
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
            this.Master.pageDetail = "WA0501.aspx";

            /*
            

            //2.程式編輯功能 */            
            this.Master.bolDel = false;
            this.Master.bolDetail = true;
            //this.Master.txtUpd = "實行";
 
            

            //3.雖有編輯功能, 但不顯示修改的欄位
            //this.Master.bolUpd_Show = false;


        }
        #endregion

        string WA070_SQL = "";
        public string callFromWA070()
        {

            this.Status_Click("WA070");
            return WA070_SQL;
        }

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

            //[修改]:狀態=1時
//[上午 10:51:59] Louis: [核准]:狀態=1時
//上午 10:52:34] Louis: [作廢]:狀態=1 or 2時

            string strSQL = "";
            string strMAST_CON_NO = "";
           // 與GRID對應, 為轉excel用
            strSQL += " select	dbo.f_ConditionGetDesc('MASTER_STS',CUR_STS,'N') as CUR_STS,MAST_CON_NO,b.CUST_SNAME,c.DEPT_NAME,d.EMP_NAME,";
            strSQL += " APLY_DATE=dbo.f_DateAddSlash(APLY_DATE),APRV_DATE=dbo.f_DateAddSlash(APRV_DATE),PRE_EXPIRY_DATE=dbo.f_DateAddSlash(PRE_EXPIRY_DATE),EXPIRY_DATE=dbo.f_DateAddSlash(EXPIRY_DATE),";
            strSQL += " b.CUST_NO,c.DEPT_CODE,EMP_CODE=d.CORP_ACCT,a.SIGNED_NO,a.CASE_TYPE_CODE,FILE_SEQ,btnUpd=(case when CUR_STS='1' then convert(bit,1) else convert(bit,0) end),b.CUST_NAME,e.CASE_TYPE_NAME,CUR_STS_CODE=CUR_STS";
            strSQL += " from OR3_MASTER_CONTRACT a left join OR_CUSTOM b on a.CUST_NO=b.CUST_NO ";
            strSQL += " left join OR_DEPT c on a.DEPT_CODE=c.DEPT_CODE";
            strSQL += " left join OR_EMP d on a.EMP_CODE=d.EMP_CODE ";
            strSQL += " left join OR_CASE_TYPE e on a.CASE_TYPE_CODE=e.CASE_TYPE_CODE";
            strSQL+="  where 1=1";


            switch (strStatus)
            {
                case "WA070":                
                    this.WA070_SQL = strSQL;
                    return;

                case "Add":
                case "Upd":
                case "Copy":
                case "Detail":
                case "Appove":
                case "Cancel":

                    if (strStatus != "Add")
                    {
                        strMAST_CON_NO = ((HiddenField)this.Master.masterRepeater("hiddenMAST_CON_NO")).Value;
                    }
                    //前面欄位需
                    this.Master.dqueryString = strSQL + " and MAST_CON_NO='" + strMAST_CON_NO + "'";


                    break;

                case "Query": //設定


                    if (!QueryCheck()) //查詢前驗證
                        return;

                    if (this.MAST_CON_NO.Text.Trim() != "")
                        strSQL += " and MAST_CON_NO like '%" + this.MAST_CON_NO.Text.rpsText() + "%'";
                    if (this.CUR_STS.SelectedValue.Trim() != "" || this.CUR_STS.SelectedItem.Text.Trim() != "")
                        strSQL += " and CUR_STS = '" + this.CUR_STS.SelectedValue.Trim() + "'";
                    if (this.DEPT_CODE.Text.Trim() != "")
                        strSQL += " and a.DEPT_CODE='" + this.DEPT_CODE.Text.rpsText() + "'";
                    if (this.EMP_CODE.Text.Trim() != "")
                        strSQL += " and a.EMP_CODE like '%" + this.EMP_CODE.Text.rpsText() + "%'";
                    if (this.CUST_NO.Text.Trim() != "")
                        strSQL += " and a.CUST_NO like '%" + this.CUST_NO.Text.rpsText() + "%'";
                    if (this.CUST_SNAME.Text.Trim() != "")
                        strSQL += " and b.CUST_SNAME like '%" + this.CUST_SNAME.Text.rpsText() + "%'";
                    if (this.APLY_DATE_ST.Text.Trim() != "")
                        strSQL += " and APLY_DATE >= '" + this.APLY_DATE_ST.Text.rpsText().Replace("/","") + "'";
                    if (this.APLY_DATE_EN.Text.Trim() != "")
                        strSQL += " and APLY_DATE <= '" + this.APLY_DATE_EN.Text.rpsText().Replace("/", "") + "'";

                    this.Master.queryString = strSQL;
                    //  this.setScript("openDetail();");
                    //   return;


                    this.Master.querySort = "MAST_CON_NO";
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
