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
    public partial class WH020 : PageParent
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
            this.Master.pageDetail = "WH0201.aspx";

            /*
            

            //2.程式編輯功能 */            
           
            

            //3.雖有編輯功能, 但不顯示修改的欄位
            //this.Master.bolUpd_Show = false;

            //4.set default value
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

            string strMessage = "";


            string strSQL = "";

            //前面欄位需與GRID對應, 為轉excel用
            strSQL += " select ";
            strSQL += " num,[DAY]=dbo.f_DateAddSlash([DAY]),customer,term,sales=a.sales,EMP_NAME=b.SALES1,ST,COST,TOTAL,CODE,FUND,EXCULDE,SHARE=SHARE,SHARE_NAME=c.SALES1 ,S_R,[授權類別],TAL,MACHINE,[標的物名稱],";
            strSQL += " TR,CAPITA,DSCRPY_ME,MEMO,DSCRPY,btnUpd=(case when DATEDIFF(MM,GETDATE(),[DAY])<=3 then convert(bit,1) else convert(bit,0) end)  ";
            strSQL += " from sl91 a left join [業務員資料表] b on a.sales=b.SALES left join [業務員資料表] c on a.SHARE=c.SALES where 1=1";


  

            switch (strStatus)
            {
                case "Add":

                case "Upd":
                case "Del":
                case "Copy":
                    string strNUM = "0";
                    if (strStatus != "Add")
                        strNUM = ((HiddenField)this.Master.masterRepeater("hiddenNUM")).Value;



                    this.Master.dqueryString = strSQL + " and NUM='" + strNUM + "'";


                    break;

                case "Import":

                    DataRow dr=dg.GetDataRow("exec s_WH020_Import_Data '"+this.F_NUM.Text.Trim()+"'");
                    

                    strSQL = " select ";
                    strSQL += " num='"+ this.F_NUM.Text.Trim()+"',";
                    strSQL+=" [DAY]='"+ dr["DAY"].ToString().Trim() +"',";
                    strSQL += " customer='" + dr["CUSTOMER"].ToString().Trim() + "',";
                    strSQL += " term='" + dr["TERM"].ToString().Trim() + "',";
                    strSQL += " sales='" + dr["sales"].ToString().Trim() + "',";
                    strSQL += " EMP_NAME=(SELECT SALES1 FROM [業務員資料表]  where SALES='" + dr["sales"].ToString().Trim() + "'),";
                    strSQL+=" ST='',";
                    strSQL += " COST='" + dr["COST"].ToString().Trim() + "',";
                    strSQL += " TOTAL='" + dr["TOTAL"].ToString().Trim() + "',";
                    strSQL += " CODE='" + dr["CODE"].ToString().Trim() + "',";
                    strSQL += " FUND='" + dr["FUND"].ToString().Trim() + "',";
                    strSQL+=" EXCULDE='',";
                    strSQL+=" SHARE='',";
                    strSQL+=" SHARE_NAME='',";
                    strSQL+=" S_R=null,";
                    strSQL += " [授權類別]='" + dr["授權類別"].ToString().Trim() + "',";
                    strSQL += " TAL='" + dr["TAL"].ToString().Trim() + "',";
                    strSQL += " MACHINE='" + dr["MACHINE"].ToString().Trim() + "',";
                    strSQL += " [標的物名稱]='" + dr["標的物名稱"].ToString().Trim() + "',";
                        strSQL += " TR='" + dr["TR"].ToString().Trim() + "',";
                        strSQL += " CAPITA='" + dr["CAPITA"].ToString().Trim() + "',";
                    strSQL+=" DSCRPY_ME='',";
                    strSQL+=" MEMO='',";
                    strSQL += " DSCRPY='" + dr["DSCRPY"].ToString().Trim() + "'";

                    this.Master.Master.nowStatus = "Upd";
                        
                    this.Master.dqueryString = strSQL;
                    /*
                     * 案件類別, SHARE,SHARE比例, DSCRPY_ME,特殊報表排除
                    SELECT [DATE] AS [DAY], 

                     * 
                     * 1 AS CODE, 
NUM, 
CUSTOMER, 
授權類別, 
TERM, 
TR, 
COST, 
TOTAL, 
SALES, 
TAL, 
NEW, 
代號 as MACHINE, 
PROD_NAME as 標的物名稱, 
SAVE_DATE, 
SAVE_TIME, 
FUND, 
資本額 as CAPITA, 
CASE_SOUR, 
現銷價, 
APRV_SERV_CHAR as 手續費, 
case [CASE_SOUR] when '1' then 'N' end  AS DSCRPY
FROM #Sub5;
*/


                    break;

                case "Query": //設定


                    if (!QueryCheck()) //查詢前驗證
                        return;

                    if (this.NUM.Text.Trim()!="")
                        strSQL += " and NUM like '%" + this.NUM.Text.Trim() + "%'";

                    if (this.DATE_S.Text.Trim() != "")
                        strSQL += " and DAY >='" + this.DATE_S.Text.Trim().Replace("/","") + "'";

                    if (this.DATE_E.Text.Trim() != "")
                        strSQL += " and DAY <='" + this.DATE_E.Text.Trim().Replace("/", "") + "'";

                    if (this.EMP_CODE.Text.Trim() != "")
                        strSQL += " and SALES = '" + this.EMP_CODE.Text.Trim() + "'";
                    
                    this.Master.queryString = strSQL;
                    //  this.setScript("openDetail();");
                    //   return;


                    this.Master.querySort = "SALES,NUM";
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
