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
    public partial class WE010 : PageParent
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
            this.Master.pageDetail = "WE0101.aspx";

            /*
            

            //2.程式編輯功能 */            
            this.Master.bolDel = false;
            this.Master.bolCopy = false;
            //this.Master.txtUpd = "實行";
 
            

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


        string WF0101_SQL = "";
        public  string callFromWF0101()
        {

            this.Status_Click("WF0101");
            return WF0101_SQL;
        }
        

        #region Status_Click：按下作業狀態鍵(新增／修改／刪除／查詢)後所觸發的Event
        /// <summary>
        /// 按下作業狀態功能鍵後
        /// </summary>
        /// <param name="strStatus">作業狀態名稱</param>
        private  void Status_Click(string strStatus)
        {
            
            string strMessage = "";


            string strSQL = "";
           
            //前面欄位需與GRID對應, 為轉excel用
            strSQL += " select CUST_NO,CUST_SNAME,CUST_BLOC_CODE,BLOC_SNAME,SPEC_COND,CUST_TYPE_CODE,CUST_UNION,NATIONALITY,CUST_NAME,UNIF_NO,";
            strSQL += " EN_NAME,EN_SNAME,Contact,CTAC_TITLE,CTAC_EXT,STOCKCODE,Contact2,CTAC_TITLE2,CTAC_EXT2,TAKER,FLOT_DATE=dbo.f_DateAddSlash(FLOT_DATE),BUILD_DATE=dbo.f_DateAddSlash(BUILD_DATE),";
            strSQL += " PHONE1,PHONE2,FACSIMILE,EMP_PSNS,SALES_RGT_ADDR,RGT_CAPT_AMT,CTAC_ADDR,REAL_CAPT_AMT,IS_COND_AUTH,IS_BIZ_CUST,";
            strSQL+=" HONEST_AGREEMENT,SECRET_PROMISE,IS_TRANSACTION,ORG_TYPE,INVOICE,MAIN_BUS_ITEM,PARENT_COMP_NAME,";
            strSQL += " PARENT_COMP_STOCK_CODE,GEN_CURR_QUOTA=isnull(GEN_CURR_QUOTA,0),VP_CURR_QUOTA=isnull(VP_CURR_QUOTA,0),AR_CURR_QUOTA=isnull(AR_CURR_QUOTA,0),a.ZIP_CODE,c.CITY_CODE,CUST_STS,a.RGT_ZIP_CODE,RGT_CITY_CODE=d.CITY_CODE,BACKGROUND ";
            strSQL += "from OR_CUSTOM a left join OR_BLOC b on a.CUST_BLOC_CODE=b.BLOC_NO  left join (select distinct ZIP_CODE,CITY_CODE from or3_ZIP )c on a.ZIP_CODE=c.ZIP_CODE ";
            strSQL += "  left join (select distinct ZIP_CODE,CITY_CODE from or3_ZIP )d on a.RGT_ZIP_CODE=d.ZIP_CODE where 1=1";
            
                        
            switch (strStatus)
            {
                case "WF0101":
                    this.WF0101_SQL = strSQL;
                    return;

                case "Add":

                case "Upd":
                case "Del":
                case "Copy":
                    string strCUST_NO = "";
                    if (strStatus != "Add")
                        strCUST_NO = ((HiddenField)this.Master.masterRepeater("hiddenCUST_NO")).Value;

                    Session["CUST_NO"] = strCUST_NO;

                    this.Master.dqueryString = strSQL + " and CUST_NO='" + strCUST_NO + "'";


                    break;

                case "Query": //設定

                   
                    if (!QueryCheck()) //查詢前驗證
                        return;

                    if (this.CUST_BLOC_CODE.Text != "")
                        strSQL += " and CUST_BLOC_CODE='" + this.CUST_BLOC_CODE.Text.rpsText() + "'";
                    if (this.CUST_BLOC_SNAME.Text != "")
                        strSQL += " and BLOC_SNAME like '%" + this.CUST_BLOC_SNAME.Text.rpsText() + "%'";
                    if (this.CUST_NO.Text != "")
                        strSQL += " and CUST_NO='" + this.CUST_NO.Text.rpsText() + "'";
                    if (this.CUST_SNAME.Text != "")
                        strSQL += " and CUST_SNAME like '%" + this.CUST_SNAME.Text.rpsText() + "%'";
                    if (this.CUST_STS.SelectedValue!="")
                        strSQL += " and ISNULL(CUST_STS,'') = '" + this.CUST_STS.SelectedValue + "'";
                    if (this.IS_TRANSACTION.SelectedValue != "")
                        strSQL += " and ISNULL(IS_TRANSACTION,'') = '" + this.IS_TRANSACTION.SelectedValue + "'";
                    if (this.SPEC_COND.SelectedValue=="Y")
                        strSQL += " and ISNULL(SPEC_COND,'') != ''";
                    if (this.SPEC_COND.SelectedValue == "N")
                        strSQL += " and ISNULL(SPEC_COND,'') = ''";

                    this.Master.queryString = strSQL;
                    //  this.setScript("openDetail();");
                    //   return;


                    this.Master.querySort = "CUST_NO";
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
