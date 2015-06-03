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
    public partial class WC020 : PageParent
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
            this.Master.pageDetail = "WC0201.aspx";

            /*
            

            //2.程式編輯功能 */            
            //this.Master.bolDel = false;
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void FrcDept_Click(object sender, EventArgs e)
        {
            this.FRC_DEPT_CODE.Items.Clear();
            dg.ListBinding(this.FRC_DEPT_CODE, "select FRC_DEP_CODE='',FRC_DEP_NAME='全部' UNION ALL select FRC_DEP_CODE,FRC_DEP_NAME from OR3_FRC_DEP where FRC_CODE='" + this.FRC_CODE.Text.Trim() + "'");
            //this.FRC_DEPT_CODE.DataBind();
            this.upDEPT.Update();
        }

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
            strSQL += " select a.FRC_CODE,FRC_SNAME,a.FRC_DEP_CODE, ";
            strSQL += " b.FRC_DEP_SNAME,a.FRC_SALES_NAME,JOB_NAME,TEL,EXT,LINE,a.FACSIMILE,MOBILE,";
            strSQL += " CELL2,Email,ADDRESS,SEQ_NO,REMARK";
            strSQL += " from OR3_FRC_SALES  a left join OR3_FRC_DEP b";
            strSQL += " on a.FRC_CODE=b.FRC_CODE and a.FRC_DEP_CODE=b.FRC_DEP_CODE left join OR_FRC c";
            strSQL += " on a.FRC_CODE=c.FRC_CODE where 1=1";

            switch (strStatus)
            {
                case "Add":

                case "Upd":
                case "Del":
                case "Copy":
                    string[] arySALES ;
                    if (strStatus != "Add")
                    {
                        arySALES = ((HiddenField)this.Master.masterRepeater("hiddenSALES")).Value.Split(',');
                        this.Master.dqueryString = strSQL + " and a.FRC_CODE='" + arySALES[0].ToString() + "' and a.FRC_DEP_CODE='"+ arySALES[1].ToString()+"' and SEQ_NO="+ arySALES[2].ToString() +"";
                    }
                    else
                        this.Master.dqueryString = strSQL + " and SEQ_NO=999999";
                    

                    break;

                case "Query": //設定


                    if (!QueryCheck()) //查詢前驗證
                        return;

                    if (this.FRC_CODE.Text.Trim() != "")
                        strSQL += " and a.FRC_CODE= '" + this.FRC_CODE.Text.rpsText() + "'";
                    if (this.FRC_SALES_NAME.Text.Trim() != "")
                        strSQL += " and a.FRC_SALES_NAME like '%" + this.FRC_SALES_NAME.Text.rpsText() + "%'";

                    if (this.EXT.Text.Trim() != "")
                        strSQL += " and EXT= '" + this.EXT.Text.rpsText() + "'";
                    if (this.MOBILE.Text.Trim() != "")
                        strSQL += " and (MOBILE = '" + this.MOBILE.Text.rpsText() + "' or CELL2='"+ this.MOBILE.Text.rpsText()+"')";

                    if (this.FRC_DEPT_CODE.Text.Trim() != "")
                        strSQL += " and a.FRC_DEP_CODE = '" + this.FRC_DEPT_CODE.Text.rpsText() + "'";




                    this.Master.queryString = strSQL;
                    //  this.setScript("openDetail();");
                    //   return;


                    this.Master.querySort = "FRC_CODE";
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
