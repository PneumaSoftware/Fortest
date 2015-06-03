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
    public partial class WE020 : PageParent
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

        protected bool bolQuery
        {
            set { ViewState["bolQuery"] = value; }
            get { return (ViewState["bolQuery"] == null ? true  : (bool)ViewState["bolQuery"]); }
        }

        protected string APLY
        {
            set { ViewState["APLY"] = value; }
            get { return (ViewState["APLY"] == null ? "" : (string)ViewState["APLY"]); }
        }

        protected string OBJ
        {
            set { ViewState["OBJ"] = value; }
            get { return (ViewState["OBJ"] == null ? "" : (string)ViewState["OBJ"]); }
        }

        #region Page_Load 網頁初始設定：宣告MasterPage所有Event，設定公用參數值
        /// <summary>
        /// 網頁初始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //*************************begin 勿動**************************   

            this.Master.DisplayEvent += new displayUPDelegate(Display_Command);

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
            this.Master.bolPostBack = true;

            /*
            

            //2.程式編輯功能*/

           
            
            /*this.Master.setEditingFunction(false, true, false);

            this.Master.showSystemButton(SystemButton.btnSave.ToString(), false);
            this.Master.showSystemButton(SystemButton.btnCancel.ToString(), false);
            this.Master.showPanel(Area.DataArea.ToString(), false);

            //3.雖有編輯功能, 但不顯示修改的欄位
            this.Master.bolUpd_Show = false;*/


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



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void toExcel(object sender, EventArgs e)
        {
            if (((Button)sender).ID == "A_excel")
            {
               
                Session["qryString"] = this.getDisplay();
                this.setScript("exportToExcel('tbAPLY');");
            }
            else
            {
               
                Session["qryString"] = this.getDisplay();
                this.setScript("exportToExcel('tbOBJ');");
            }
        }


        private string getDisplay()
        {


            string strSQL = "";
            if (this.APLY_NO.Text.Trim() != "")
                strSQL += " ,@PAPLY_NO='" + this.APLY_NO.Text.rpsText() + "'";

            if (this.PCONTACT.Text.Trim() != "")
                strSQL += " ,@PCONTACT='" + this.PCONTACT.Text.rpsText() + "'";

            if (this.PCUST_NO.Text.Trim() != "")
                strSQL += " ,@PCUST_NO='" + this.PCUST_NO.Text.rpsText() + "'";

            if (this.PIS_SEARCH.Checked)
                strSQL += " ,@PIS_SEARCH='Y'";

            if (this.PCUST_NAME.Text.Trim() != "")
                strSQL += " ,@PCUST_NAME='" + this.PCUST_NAME.Text.rpsText() + "'";

            if (this.FRC_CODE.Text.Trim() != "")
                strSQL += " ,@PFRC_CODE='" + this.FRC_CODE.Text.rpsText() + "'";

            if (this.FRC_NAME.Text.Trim() != "")
                strSQL += " ,@PFRC_NAME='" + this.FRC_NAME.Text.rpsText() + "'";

            if (this.PCTAC_TEL.Text.Trim() != "")
                strSQL += " ,@PCTAC_TEL='" + this.PCTAC_TEL.Text.rpsText() + "'";

            if (this.PRECV_NAME.Text.Trim() != "")
                strSQL += " ,@PRECV_NAME='" + this.PRECV_NAME.Text.rpsText() + "'";

            if (this.PSEND_ADDR.Text.Trim() != "")
                strSQL += " ,@PSEND_ADDR='" + this.PSEND_ADDR.Text.rpsText() + "'";

            if (this.PADDR.Text.Trim() != "")
                strSQL += " ,@PADDR='" + this.PADDR.Text.rpsText() + "'";

            if (this.PPROD_NAME.Text.Trim() != "")
                strSQL += " ,@PPROD_NAME='" + this.PPROD_NAME.Text.rpsText() + "'";

            if (this.PMAC_NO.Text.Trim() != "")
                strSQL += " ,@PMAC_NO='" + this.PMAC_NO.Text.rpsText() + "'";

            if (this.PAPLY_DATE_S.Text.Trim() != "")
                strSQL += " ,@PAPLY_DATE_S='" + this.PAPLY_DATE_S.Text.rpsText() + "'";

            if (this.PAPLY_DATE_E.Text.Trim() != "")
                strSQL += " ,@PAPLY_DATE_E='" + this.PAPLY_DATE_E.Text.rpsText() + "'";

            if (this.PEMP_CODE.Text.Trim() != "")
                strSQL += " ,@PEMP_CODE='" + this.PEMP_CODE.Text.rpsText() + "'";

            if (this.PCUR_STS.Text.Trim() != "")
                strSQL += " ,@PCUR_STS='" + this.PCUR_STS.SelectedValue + "'";

            if (this.PDEPT_CODE.Text.Trim() != "")
                strSQL += " ,@PDEPT_CODE='" + this.PDEPT_CODE.Text.rpsText() + "'";

            if (this.PCASE_SOUR.Text.Trim() != "")
                strSQL += ",@PCASE_SOUR='" + this.PCASE_SOUR.Text.rpsText() + "'";

            if (this.PIS_DEL.Checked)
                strSQL += ",@PIS_DEL='Y'";

            if (this.PINV_NO.Text.Trim() != "")
                strSQL += ",@PINV_NO='" + this.PINV_NO.Text.rpsText() + "'";


            string strMessage = "";
            if (strSQL == "")
            {
                strMessage = " 請至少輸入一個條件！";
                this.setMessageBox(strMessage);
                return "";
            }

            strSQL = "exec s_WE020_Grid " + strSQL.Trim().Substring(1);


            return strSQL;

        }


        #region
        /// <summary>
        /// 資料查詢
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Display_Command()
        {
           /* @PAPLY_NO varchar(14)=NULL,		--申請書編號
	@PCONTACT nvarchar(12)=NULL,	--連絡人
	@PCUST_NO varchar(10)=NULL,		--客戶代號
	@PIS_SEARCH varchar(1)='N',		--搜尋共同承租人
	@PCUST_NAME nvarchar(80)=NULL,	--客戶名稱
	@PFRC_CODE varchar(10)=NULL,	--經銷商代碼
	@PFRC_NAME nvarchar(30)=NULL,	--經銷商名稱
	@PCTAC_TEL varchar(25)=NULL,	--連絡電話
	@PRECV_NAME nvarchar(12)=NULL,	--收件人
	@PSEND_ADDR nvarchar(80)=NULL,	--存放地址
	@PADDR nvarchar(80)=NULL,		--請款地址
	@PPROD_NAME nvarchar(60)=NULL,	--品名
	@PMAC_NO varchar(20)=NULL,		--機號
	@PAPLY_DATE_S varchar(10)=NULL,	--申請日期(起)	
	@PAPLY_DATE_E varchar(10)=NULL,	--申請日期(迄)	
	@PEMP_CODE varchar(10)=NULL,	--業務員代號
	@PCUR_STS varchar(1)=NULL,		--目前狀況
	@PDEPT_CODE varchar(10)=NULL,	--部門代號
	@PCASE_SOUR varchar(1)=NULL,	--案件來源
	@PIS_DEL varchar(1)='N',		--含作廢駁回
	@PINV_NO varchar(10)=NULL		--發票號碼*/

            this.APLY = "";

            

            DataTable dt = dg.GetDataTable(this.getDisplay());


            if (dt.Rows.Count == 0)
            {
                this.setMessageBox("無符合條件資料！");
                return;
            }
            DataView dvAPLY = dt.DefaultView;
            dvAPLY.RowFilter = "Type_Code='APLY'";

            this.rptAPLY.DataSource = dvAPLY;
            this.rptAPLY.DataBind();


            DataView dvObj = dt.DefaultView;
            dvObj.RowFilter = "Type_Code='Obj'";
            this.rptOBJ.DataSource = dvObj;
            this.rptOBJ.DataBind();

            this.bolQuery = false;
            this.jqTabs();

        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        private void jqTabs()
        {
            string strScript = "document.getElementById('divSearch').style.display='none';";
            strScript += " var maintenancetabs = $(\"#tt\").tabs();\n";
            strScript += "maintenancetabs.tabs('select', 0);";
            this.setScript(strScript);
        }


        #region Exit_Click
        protected void Exit_Click(object sender, EventArgs e)
        {
            string strScript = "document.getElementById('divSearch').style.display='';\n";           
            this.setScript(strScript);
                
            this.bolQuery = true;
            this.APLY = ""; 
            this.rptAPLY.DataSource = null;
            this.rptAPLY.DataBind();
            this.rptOBJ.DataSource = null;
            this.rptOBJ.DataBind();


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


        #region Function_Click：按下功能鍵
        /// <summary>
        /// 按下作業狀態功能鍵後
        /// </summary>
        /// <param name="strStatus">作業狀態名稱</param>
        protected void Function_Click(object sender, CommandEventArgs e)
        {
            string strSQL = "";
            string pageDetail = "";

            string strScript = "";


            

            string strMessage = "";
            if (this.rowAPLY.Text.Trim() == "")
            {
                strMessage = " 請選擇編輯項目！";
                this.setMessageBox(strMessage);
                this.jqTabs();
                return;
            }


            if (this.rowOBJECT.Text.Trim() == "" && e.CommandName == "Function5")
            {
                strMessage = " 請於標的物資料選擇！";
                this.setMessageBox(strMessage);
                this.jqTabs();
                return;
            }

            DataRow dr = dg.GetDataRow("select a.CUST_NO,CUST_NAME,dbo.f_ConditionGetDesc('APLYSTS',CUR_STS,'N') as CUR_STS,CUR_STS as CUR_STS_CODE,b.CUST_BLOC_CODE,c.BLOC_sNAME,b.CUST_SNAME from OR_CASE_APLY_BASE a left join OR_CUSTOM b on a.CUST_NO=b.CUST_NO left join OR_BLOC c on b.CUST_BLOC_CODE=c.BLOC_NO where APLY_NO='" + this.rowAPLY.Text.Trim() + "'");
            Session["APLY_NO"] = this.rowAPLY.Text.Trim();

            switch (e.CommandName)
            {

                case "Function1": //客戶服務資料異動        ok
                    pageDetail = "WE030";
                    break;

                case "Function2"://票據明細維護  
                    Session["CUST_NO"] = dr[0].ToString();
                    Session["CUST_NAME"] = dr[1].ToString();
                    pageDetail = "WE050";
                    break;

                case "Function3"://電話紀錄維護     ok
                    
                    Session["CUST_NO"] = dr[0].ToString();
                    Session["CUST_NAME"] = dr[1].ToString();

                    pageDetail = "WE040";
                    break;

                case "Function4"://解約金試算  hold
                    Session["CUST_NO"] = dr[0].ToString();
                    Session["CUST_NAME"] = dr[1].ToString();

                    pageDetail = "WE110";
                    break;

                case "Function5"://標的物查詢  
                    pageDetail = "WE090";
                    Session["OBJ_CODE"] = this.rowOBJECT.Text.Trim();                 
                    
                    break;

                case "Function6"://客戶歷史交易查詢  
                    Session["CUST_NO"] = dr[0].ToString();
                    Session["CUST_NAME"] = dr[1].ToString();
                    pageDetail = "WE100";
                    break;

                case "Function7"://客戶付款記錄查詢  
                    Session["CUST_NO"] = dr[0].ToString();
                    Session["CUST_NAME"] = dr["CUST_SNAME"].ToString();
                    Session["CUST_BLOC_CODE"] = dr["CUST_BLOC_CODE"].ToString();
                    Session["BLOC_NAME"] = dr["BLOC_sNAME"].ToString();
                    pageDetail = "WE070";
                    break;

                case "Function8"://案件生命週期查詢  
                    break;

                case "Function9"://催收記錄查詢  
                    http://orixtw /Admin/KenLogin.asp


                   // this.setScript("window.open('http://orixtw/admin/Press/PressView.asp?idx=" + this.rowAPLY.Text.Trim() + "&idxno=" + dr[0].ToString() + "&page=','_blank')");
                    this.setScript("window.open('http://orixtw/Admin/KenLogin.asp?txtId="+ this.Master.Master.CorpAcct +"&KenProgram=Press&idx=" + this.rowAPLY.Text.Trim() + "&idxno=" + dr[0].ToString() + "&page=','_blank')");

                //    Response.End();
                    return;
                    break;

                case "Function10"://計張資訊查詢  
                    pageDetail = "WE080";
                    break;

                case "Function11"://客戶應收帳款查詢  
                    break;

                case "Function12"://申請書資料查詢  
                                        
                    Session["CUR_STS"] = dr[3].ToString().Trim()+","+dr[2].ToString().Trim();
                    Session["bolWE020"] = true;
                    pageDetail = "WA0601";


                    break;

            }

  
            strScript = "pageDetail='" + pageDetail + ".aspx?ndt="+ DateTime.Now.ToString("HHmmss")+"';\n";
            strScript += "contentChange('frameDetail');\n";


            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "pageChange", strScript, true);
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
