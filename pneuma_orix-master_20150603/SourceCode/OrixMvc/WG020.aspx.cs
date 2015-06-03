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
    public partial class WG020 : PageParent
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
            this.Master.pageDetail = "WG0201.aspx";

            /*
            

            //2.程式編輯功能 */            
           // this.Master.bolDel = false;
           // this.Master.bolAdd = false;
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



            strSQL += " select a.BANK_NO,BANK_TYPE=(case c.BANK_TYPE when '1' then '本國銀行' when '2' then '票券金融' when '3' then '外商銀行' when '4' then '其他' end),b.BANK_NAME,MTHD_DESC,";
            strSQL += " LOAN_AMT,RATE,LOAN_DATE=dbo.f_DateAddSlash(LOAN_DATE),LOAN_TYPE=dbo.f_ConditionGetDesc('LOAN_TYPE',LOAN_TYPE,'N'),";
            strSQL += " Credit_way_desc=dbo.f_ConditionGetDesc('COLL_MTHD',Credit_way,'N'),Long_Short_LOAN_desc=(case a.Long_Short_LOAN WHEN 1 then '長借' WHEN 2 THEN '短借' ELSE '' END ) ,";
            strSQL+=" IsRecourse,Due_date=dbo.f_DateAddSlash(Due_date),Interest_Cal_desc=dbo.f_ConditionGetDesc('Interest_Cal',Interest_Cal,'N'),";
            strSQL += " a.BANK_ACCT_NO,CRD_AMT,USED_CREDIT,CRD_DATE_TO=dbo.f_DateAddSlash(CRD_DATE_TO),a.REMARK,a.SeqNo,a.LOAN_SEQ,REST_CREDIT=CRD_AMT-USED_CREDIT,";
            strSQL += " Leap_year,Feb_rate_Days,AMT_LONG_SHORT_LOAN=(case c.LONG_SHORT_LOAN WHEN 1 then '綜合' WHEN 2 THEN '長借' ELSE '短借' END ),a.Long_Short_LOAN,Credit_way,INTEREST_CAL,Fixed_interest,a.LOAN_MTHD_CODE,PAY_DIVD_DATE,Interest_Payment, ";
            strSQL += " Fixed_interest_DESC=(case a.Fixed_interest WHEN 1 then '固定' WHEN 2 THEN '浮動' ELSE '' END )";
            strSQL += " from OR_BANK_LOAN_DTL a";
            strSQL += " left join ACC18 b on a.BANK_NO=b.BANK_NO ";
            strSQL += " left join OR_BANK_AMT c on a.SeqNo=c.SeqNo left join or_Loan_Mthd d on a.LOAN_MTHD_CODE=d.LOAN_MTHD_CODE  where 1=1";


            switch (strStatus)
            {
                case "Add":

                case "Upd":
                case "Del":
                case "Copy":
                case "Back":
                    string strSeqNo = "0";
                    string strLOAN_SEQ = "0";
                    if (strStatus != "Add")
                    {
                        string[] aryData = ((HiddenField)this.Master.masterRepeater("hiddenSEQ")).Value.Split(',');
                        strSeqNo = aryData[0].ToString().Trim();
                        strLOAN_SEQ = aryData[1].ToString().Trim();
                    }


                    this.Master.dqueryString = strSQL + " and a.SeqNo=" + strSeqNo + " and a.LOAN_SEQ=" + strLOAN_SEQ + "";

                    if (strStatus=="Back")
                        this.Master.pageDetail = "WG0301.aspx";
                    else
                        this.Master.pageDetail = "WG0201.aspx";

                    break;

                case "Query": //設定


                    if (!QueryCheck()) //查詢前驗證
                        return;

                    if (this.BANK_NO.Text.Trim() != "")
                        strSQL += " and a.BANK_NO= '" + this.BANK_NO.Text.rpsText() + "'";
                    if (this.LOAN_DATE_ST.Text.Trim() != "")
                        strSQL += " and LOAN_DATE >= '" + this.LOAN_DATE_ST.Text.Trim().Replace("/","") + "'";

                    if (this.LOAN_DATE_EN.Text.Trim() != "")
                        strSQL += " and LOAN_DATE <= '" + this.LOAN_DATE_EN.Text.Trim().Replace("/", "") + "'";

                    if (this.LOAN_MTHD_CODE.SelectedValue.Trim() != "")
                        strSQL += " and a.LOAN_MTHD_CODE= '" + this.LOAN_MTHD_CODE.SelectedValue.Trim() + "'";

                    if (this.LOAN_TYPE.SelectedValue.Trim() != "")
                        strSQL += " and LOAN_TYPE= '" + this.LOAN_TYPE.SelectedValue.Trim() + "'";

                    if (this.DUE_DATE_ST.Text.Trim() != "")
                        strSQL += " and DUE_DATE >= '" + this.DUE_DATE_ST.Text.Trim().Replace("/", "") + "'";

                    if (this.DUE_DATE_EN.Text.Trim() != "")
                        strSQL += " and DUE_DATE <= '" + this.DUE_DATE_EN.Text.Trim().Replace("/", "") + "'";

                    if (this.LONG_SHORT_LOAN.SelectedValue.Trim() != "")
                        strSQL += " and a.LONG_SHORT_LOAN= '" + this.LONG_SHORT_LOAN.SelectedValue.Trim() + "'";

                    if (this.IsRecourse.SelectedValue.Trim() != "")
                        strSQL += " and IsRecourse= '" + this.IsRecourse.SelectedValue.Trim() + "'";

                    if (this.CREDIT_WAY.SelectedValue.Trim() != "")
                        strSQL += " and CREDIT_WAY= '" + this.CREDIT_WAY.SelectedValue.Trim() + "'";

                    if (this.Fixed_interest.SelectedValue.Trim() != "")
                        strSQL += " and Fixed_interest ='" + this.Fixed_interest.Text.rpsText() + "'";

                  

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
