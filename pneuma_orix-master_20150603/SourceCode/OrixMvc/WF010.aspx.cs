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
    public partial class WF010 : PageParent
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
            this.Master.pageDetail = "WF0101.aspx";

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
            this.PCorp_Acct.Text = this.Master.Master.CorpAcct;
            this.PEMP_NAME.Text = this.Master.Master.EmployeeName;

            DateTime now = DateTime.Now;

            int dayOfWeek;
            dayOfWeek = (int)now.DayOfWeek - (int)System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek + 1;

            //拜訪日期:預設系統日當週一 ~下週六
            DateTime dt = now.AddDays(0 - dayOfWeek + 2);
            DateTime dte = dt.AddDays(12);

            this.PVISIT_DAT_S.Text = dt.ToString("yyyy/MM/dd");
            this.PVISIT_DAT_E.Text = dte.ToString("yyyy/MM/dd");
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

            if (this.PVISIT_DAT_S.Text.Trim() == "")
                strMessage += "[拜訪日期起值]";

            if (this.PVISIT_DAT_E.Text.Trim() == "")
                strMessage += "[拜訪日期迄值]";


            if (strMessage != "")
                strMessage += "必須輸入！";
            else
            {
                if (this.PVISIT_DAT_E.Text.CompareTo(this.PVISIT_DAT_S.Text) <= 0)
                {
                    strMessage += "\\r\\n 拜訪日期迄期必須大於拜訪日期起值！";
                }
            }

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
            //strSQL += " exec s_WF010_Grid ";
            strSQL += "select	EMP_CODE=dbo.f_EmpIDToCorpAcct(VR.EMP_CODE),--員工代號(社內帳號) \n";
            strSQL += "         EMP_NAME,--員工姓名\n";
            strSQL += "         SUPL_CODE,--供應商代號\n";
            strSQL += "         F.FRC_NAME,--供應商名稱\n";
            strSQL += "         CUST_CODE,--客戶代號\n";
            strSQL += "         C.CUST_NAME,--客戶名稱\n";
            strSQL += "         CTAC,--聯絡人\n";
            strSQL += "         VISIT_DAT=dbo.f_DateAddSlash(VISIT_DAT),--拜訪日期\n";
            strSQL += "         Mes_TStart,--起時\n";
            strSQL += "         Mes_TStop,--迄時\n";
            strSQL += "         FORECAST_APLY_AMT,--預估往來金額\n";
            strSQL += "         dbo.f_ConditionGetDesc('INTERVIEW_TOPIC',INTERVIEW_TOPIC,'N') as Topic_Desc,--類別\n";
            strSQL += "         dbo.f_ConditionGetDesc('TRANSPORTATION',TRANSPORTATION,'N') as Trans_Desc,--交通工具\n";
            strSQL += "         TRANSPORTATION,--交通工具\n";
            strSQL += "         INTERVIEW_TOPIC,--類別\n";
            strSQL += "         APLY_FEE, --交通費\n";
            strSQL += "         Rec_Meat, --訪談內容\n";
            strSQL += "         CASE_SOUR, --新舊戶\n";
            strSQL += "         REC_TITLE, --主旨\n";
            strSQL += "         RUL_PLACE, --地點\n";
            strSQL += "         RUL_OTHER, --交通說明\n";
            strSQL += "         ATM_NUM, --外出單號\n";
            strSQL += "         SEQNo\n";
            strSQL += "         from OR3_VISIT_REC VR \n";
            strSQL += "         left join OR_EMP E on VR.EMP_CODE=E.EMP_CODE \n";
            strSQL += "         left join OR_FRC F on VR.SUPL_CODE=F.FRC_CODE\n";
            strSQL += "         left join OR_CUSTOM C on VR.CUST_CODE=C.CUST_NO\n";
            strSQL += "         Where 1=1 \n";

            switch (strStatus)
            {
                case "Add":

                case "Upd":
                case "Del":
                case "Copy":
                    string strSeqNo = "0";
                    if (strStatus != "Add")
                        strSeqNo = ((HiddenField)this.Master.masterRepeater("hiddenSeqNo")).Value;



                    //this.Master.dqueryString = strSQL + " @SeqNo=" + strSeqNo + "";
                    this.Master.dqueryString = strSQL + " and SeqNo=" + strSeqNo + "";

                    break;

                case "Query": //設定


                    if (!QueryCheck()) //查詢前驗證
                        return;

                    //strSQL += " @PCorp_Acct='" + this.PCorp_Acct.Text + "'";
                    //strSQL += " ,@PEMP_NAME='" + this.PEMP_NAME.Text + "'";
                    //strSQL += " ,@PVISIT_DAT_S='" + this.PVISIT_DAT_S.Text + "'";
                    //strSQL += " ,@PVISIT_DAT_E='" + this.PVISIT_DAT_E.Text + "'";
                    //strSQL += " ,@PSUPL_CODE='" + this.PSUPL_CODE.Text + "'";
                    //strSQL += " ,@PFRC_SNAME='" + this.PFRC_SNAME.Text + "'";
                    //strSQL += " ,@PCUST_CODE='" + this.PCUST_CODE.Text + "'";
                    //strSQL += " ,@PCUST_SNAME='" + this.PCUST_SNAME.Text + "'";

                    if (this.PCorp_Acct.Text != "")
                        strSQL += " and VR.EMP_CODE = dbo.f_CorpAcctToEmpID('" + this.PCorp_Acct.Text.rpsText() + "')";
                    if (this.PEMP_NAME.Text != "")
                        strSQL += " and EMP_NAME = '" + this.PEMP_NAME.Text.rpsText() + "'";
                    if (this.PVISIT_DAT_S.Text != "")
                        strSQL += " and VISIT_DAT >= '" + this.PVISIT_DAT_S.Text.rpsText().Replace("/", "") + "'";
                    if (this.PVISIT_DAT_E.Text != "")
                        strSQL += " and VISIT_DAT <= '" + this.PVISIT_DAT_E.Text.rpsText().Replace("/", "") + "'";
                    if (this.PSUPL_CODE.Text != "")
                        strSQL += " and SUPL_CODE = '" + this.PSUPL_CODE.Text + "'";
                    if (this.PFRC_SNAME.Text != "")
                        strSQL += " and FRC_SNAME like '%" + this.PFRC_SNAME.Text.rpsText() + "%'";
                    if (this.PCUST_CODE.Text != "")
                        strSQL += " and CUST_CODE = '" + this.PCUST_CODE.Text + "'";
                    if (this.PCUST_SNAME.Text != "")
                        strSQL += " and CUST_SNAME like '%" + this.PCUST_SNAME.Text.rpsText() + "%'";

                    this.Master.queryString = strSQL;
                    //  this.setScript("openDetail();");
                    //   return;


                     this.Master.querySort = "EMP_CODE,VISIT_DAT,Mes_TStart";
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
