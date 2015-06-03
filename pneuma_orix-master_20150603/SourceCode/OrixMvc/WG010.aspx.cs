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
    public partial class WG010 : PageParent
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
            this.Master.pageDetail = "WG0101.aspx";

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
          //  this.PCorp_Acct.Text = this.Master.Master.EmployeeId;
          //  this.PEMP_NAME.Text = this.Master.Master.EmployeeName;

            

          //  this.PVISIT_DAT_S.Text = dt.ToString("yyyy/MM/dd");
          //  this.PVISIT_DAT_E.Text = dte.ToString("yyyy/MM/dd");
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
/*
            if (this.PVISIT_DAT_S.Text.Trim() == "")
                strMessage += "[拜訪日期起值]";

            if (this.PVISIT_DAT_E.Text.Trim() == "")
                strMessage += "[拜訪日期迄值]";
            */

            if (strMessage != "")
                strMessage += "必須輸入!!";

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
            strSQL += "select a.BANK_NO,b.BANK_NAME,LONG_SHORT_LOAN_DESC=(case LONG_SHORT_LOAN WHEN 1 then '綜合' WHEN 2 THEN '長借' ELSE '短借' END ),CRD_AMT,USED_CREDIT,REST_CREDIT=(CRD_AMT-USED_CREDIT),CRD_DATE_TO=dbo.f_DateAddSlash(CRD_DATE_TO),COLL_MTHD_DESC=dbo.f_ConditionGetDesc('COLL_MTHD',COLL_MTHD,'N'),BOND_RATE";
            strSQL+= " ,SEQNO,BANK_TYPE,ISCYCLE,LONG_LOAN=(case when LONG_SHORT_LOAN='1' then convert(numeric(10,1),(USED_CREDIT/CRD_AMT)*100) else 0 end) ";
            strSQL+= " ,SHORT_LOAN=(case when LONG_SHORT_LOAN='2' then convert(numeric(10,1),(USED_CREDIT/CRD_AMT)*100) else 0 end) ";
            strSQL += " ,PDC_PERCENT,LAST_CHG_DATE=dbo.f_DateAddSlash(LAST_CHG_DATE),CAPT_CODE_DESC,REMARK,btnDel=(case  when isnull((select top 1 'Y' from OR_BANK_LOAN_DTL c where c.SeqNo=a.SeqNo ),'')='Y' then convert(bit,0) else convert(bit,1) end),LONG_SHORT_LOAN,COLL_MTHD ";
            strSQL+=" from OR_BANK_AMT a left join ACC18 b on a.BANK_NO=b.BANK_NO where 1=1 ";

            

            switch (strStatus)
            {
                case "Add":

                case "Upd":
                case "Del":
                case "Copy":
                    string strSeqNo = "0";
                    if (strStatus != "Add")
                        strSeqNo = ((HiddenField)this.Master.masterRepeater("hiddenSeqNo")).Value;
                    

                    this.Master.dqueryString = strSQL + "and SeqNo=" + strSeqNo + "";


                    break;

                case "Query": //設定
                    

                    if (!QueryCheck()) //查詢前驗證
                        return;

                   
                    if (this.BANK_NO.Text.Trim()!="")
                        strSQL+=" and a.BANK_NO='"+ this.BANK_NO.Text.rpsText() +"'";

                    if (this.BANK_NAME.Text.Trim()!="")
                        strSQL+=" and BANK_NAME like '%"+ this.BANK_NAME.Text.rpsText() +"%'";

             

                    this.Master.queryString = strSQL;
                    //  this.setScript("openDetail();");
                    //   return;


                    this.Master.querySort = "BANK_NO";
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
