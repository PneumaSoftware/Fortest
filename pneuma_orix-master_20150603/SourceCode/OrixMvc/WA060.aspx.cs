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
    public partial class WA060 : PageParent
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
            this.Master.pageDetail = "WA0601.aspx";

            /*
            

            //2.程式編輯功能 */            
            this.Master.bolDel = false;
            this.Master.bolUpd = false;
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
            strSQL += " select APLY_NO,a.CUST_NO,b.CUST_SNAME,CON_SEQ_NO,APLY_DATE=dbo.f_DateAddSlash(APLY_DATE),dbo.f_ConditionGetDesc('APLYSTS',CUR_STS,'N') as CUR_STS,";
            strSQL += " APLY_APRV_DATE=dbo.f_DateAddSlash(APLY_APRV_DATE),PAPER,c.EMP_NAME,CUR_QUOTA_APLY_NO,STSCODE=CUR_STS ";
            strSQL += " from OR_CASE_APLY_BASE  a left join OR_CUSTOM b on a.CUST_NO=b.CUST_NO";
            strSQL += " left join OR_EMP c on a.EMP_CODE=c.EMP_CODE left join OR_DEPT d on a.dept_code=d.dept_code	where  isnull(d.end_date,'')=''";


            switch (strStatus)
            {
                case "Add":

                case "Upd":
                case "UpdAfter":
                case "Del":
                case "Copy":
                    string strAPLY_NO = "";
                    string strCUR_STS = "";
                    if (strStatus != "Add")
                    {
                        strAPLY_NO = ((HiddenField)this.Master.masterRepeater("hiddenAPLY_NO")).Value;
                        strCUR_STS = ((HiddenField)this.Master.masterRepeater("hiddenCUR_STS")).Value;
                        
                    }
                    if (strStatus == "Upd")
                    {
                        if (dg.GetDataRow("select CUST_STS from OR_CUSTOM where CUST_NO='" + ((HiddenField)this.Master.masterRepeater("hiddenCUST_NO")).Value.Trim() + "'")[0].ToString() == "Y")
                        {
                            this.setMessageBox("此客戶為潛在客戶，請至客戶維護修改。");
                            this.Master.Master.nowStatus = "";
                            return ;
                        }
                    }
                   // Session["APLY_NO"] = strAPLY_NO;
                   // Session["CUR_STS"] = strCUR_STS;

                    //this.Master.dqueryString = strSQL + " and APLY_NO='" + strAPLY_NO + "'";
                    this.setScript("window.parent.frames['frameDetail'].reloadData('"+ strAPLY_NO +"', '"+ strCUR_STS +"');");

                    break;

                case "Query": //設定


                    if (!QueryCheck()) //查詢前驗證
                        return;

                    if (this.APLY_NO.Text.Trim() != "")
                        strSQL += " and APLY_NO like '%" + this.APLY_NO.Text.rpsText() + "%'";
                    if (this.CUR_STS.SelectedValue.Trim() != "" || this.CUR_STS.SelectedItem.Text.Trim()!="")
                        strSQL += " and CUR_STS = '" + this.CUR_STS.SelectedValue.Trim() + "'";
                    if (this.DEPT_CODE.Text.Trim() != "" )
                        strSQL += " and a.DEPT_CODE='" + this.DEPT_CODE.Text.rpsText() + "'";
                    if (this.EMP_CODE.Text.Trim() != "")
                        strSQL += " and a.EMP_CODE like '%" + this.EMP_CODE.Text.rpsText() + "%'";
                    if (this.CUST_NO.Text.Trim() != "")
                        strSQL += " and a.CUST_NO like '%" + this.CUST_NO.Text.rpsText() + "%'";
                    if (this.CUST_SNAME.Text.Trim() != "")
                        strSQL += " and b.CUST_SNAME like '%" + this.CUST_SNAME.Text.rpsText() + "%'";
                    if (this.PAPER.SelectedValue.Trim() != "")
                        strSQL += " and PAPER = '" + this.PAPER.SelectedValue.Trim() + "'";
                    if (this.CUR_QUOTA_APLY_NO.Text.Trim() != "")
                        strSQL += " and CUR_QUOTA_APLY_NO like '%" + this.CUR_QUOTA_APLY_NO.Text.rpsText() + "%'";

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
