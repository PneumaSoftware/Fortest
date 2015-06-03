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
    public partial class WE040 : PageParent
    {

        protected bool bolWE020
        {
            set { ViewState["bolWE020"] = value; }
            get { return (ViewState["bolWE020"] == null ? false : (bool)ViewState["bolWE020"]); }
        }


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
            this.Master.pageDetail = "WE0401.aspx";

            /*            

            //2.程式編輯功能
            this.Master.setEditingFunction(false, true, false);

            this.Master.showSystemButton(SystemButton.btnSave.ToString(), false);
            this.Master.showSystemButton(SystemButton.btnCancel.ToString(), false);
            this.Master.showPanel(Area.DataArea.ToString(), false);

            //3.雖有編輯功能, 但不顯示修改的欄位
            this.Master.bolUpd_Show = false;*/
            this.Master.bolDel = false;

            //判斷是否由we020帶入
            if (Session["APLY_NO"] != null && Session["CUST_NO"] != null && Session["CUST_NAME"] != null)
            {
                this.bolWE020 = true;
                this.CUST_NO.Text = ((String)Session["CUST_NO"]);
                this.APLY_NO.Text = ((String)Session["APLY_NO"]);
                this.CUST_NAME.Text = ((String)Session["CUST_NAME"]);
                this.CUST_NO.Editing(false);
                this.CUST_NAME.Editing(false);
                Session["APLY_NO"] = null;
                Session["CUST_NO"] = null;
                Session["CUST_NAME"] = null;
                this.Status_Click("Query");
            }
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
            strSQL += " select STS=(case SRV_REC_STS when '9' then '結案' else '待追蹤' end), a.CUST_NO,CUST_NAME,APLY_NO,";
            strSQL += " PHONE_DATE=dbo.f_DateAddSlash(PHONE_DATE),PHONE_TIME,KEY_USER,REC_TITLE,SRV_REC_STS=(case when SRV_REC_STS='1' then 'Y' else '' end),RE_DATE=dbo.f_DateAddSlash(RE_DATE),RE_TIME,REC_CONTENT,";
            strSQL += " CLOSED_DATE=dbo.f_DateAddSlash(CLOSED_DATE),CUST_SNAME,btnUpd=(case when SRV_REC_STS='9' and  dbo.f_DateAddSlash(CLOSED_DATE)<=convert(char(10),dateadd(month,-1,getdate()),111) then convert(bit,0) else convert(bit,1) end ),KEY_NAME=c.EMP_NAME";
            strSQL += " from OR_SRV_REC a left join OR_CUSTOM b on a.CUST_NO=b.CUST_NO left join OR_EMP c on dbo.f_CorpAcctToEmpID(KEY_USER)=c.EMP_CODE where 1=1";


            switch (strStatus)
            {
                case "Add":

                case "Upd":
                case "Del":
                case "Copy":
                    string[] aryCUST_NO;
                    if (strStatus != "Add")
                    {
                        aryCUST_NO = ((HiddenField)this.Master.masterRepeater("hiddenCUST_NO")).Value.ToString().Trim().Split(',');
                        this.Master.dqueryString = strSQL + " and a.CUST_NO='" + aryCUST_NO[0].ToString() + "' and PHONE_DATE='" + aryCUST_NO[1].ToString().Replace("/","") + "' and PHONE_TIME='" + aryCUST_NO[2].ToString() + "'";
                    }
                    else
                        this.Master.dqueryString = strSQL + " and a.CUST_NO=''";

                    if (bolWE020)
                    {
                        Session["bolWE020"] = true;
                        Session["CUST_NO"] = this.CUST_NO.Text;
                        Session["APLY_NO"] = this.APLY_NO.Text;
                        Session["CUST_NAME"] = this.CUST_NAME.Text;
                    }

                    break;

                case "Query": //設定


                    if (!QueryCheck()) //查詢前驗證
                        return;

                    if (this.CUST_NO.Text != "")
                        strSQL += " and a.CUST_NO='" + this.CUST_NO.Text.rpsText() + "'";
                    if (this.CUST_NAME.Text != "")
                        strSQL += " and CUST_NAME like '%" + this.CUST_NAME.Text.rpsText() + "%'";                    
                    if (this.SRV_REC_STS.SelectedValue!= "")
                        strSQL += " and SRV_REC_STS = '" + this.SRV_REC_STS.SelectedValue  +"'";
                    
                    if (this.chkAPLY.Checked && this.APLY_NO.Text != "")
                        strSQL += " and isnull(APLY_NO,'') in ('','" + this.APLY_NO.Text.rpsText() + "') ";

                    if (this.chkAPLY.Checked && this.APLY_NO.Text == "")
                        strSQL += " and isnull(APLY_NO,'')='' ";

                    if (!this.chkAPLY.Checked && this.APLY_NO.Text != "")
                        strSQL += " and isnull(APLY_NO,'')='" + this.APLY_NO.Text.rpsText() + "' ";

                    if (this.KEY_USER.Text != "")
                        strSQL += " and KEY_USER like '%" + this.KEY_USER.Text.rpsText() + "%'";                    

                
                    if (this.REC_TITLE.Text != "")
                        strSQL += " and REC_TITLE like '%" + this.REC_TITLE.Text.rpsText() + "%'";        
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
