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
using OrixMvc.ocxControl;
using VS2008.Module;
using AjaxControlToolkit;

namespace OrixMvc
{
    public partial class WE110 : OrixMvc.Pattern.PageParent
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


        protected bool bolWE020
        {
            set { ViewState["bolWE020"] = value; }
            get { return (ViewState["bolWE020"] == null ? false : (bool)ViewState["bolWE020"]); }
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


            this.Master.DisplayEvent += new displayDelegate(Display_Command);

            if (!IsPostBack)
                this.setParms();
            //***************************end 勿動****************************
        }
        #endregion



        #region setParms：設定公共參數
        /// <summary>
        /// 設定公共參數-->順序不得互換
        /// 1.編輯區域Title
        /// 2.程式編輯功能(新增,修改,刪除)
        /// 3.雖有編輯功能, 但不顯示修改的欄位
        /// </summary>
        private void setParms()
        {
           
            /*
            //1.
            this.Master.DataAreaName = "";

            //2.程式編輯功能
            this.Master.setEditingFunction(false, true, false);

            this.Master.showSystemButton(SystemButton.btnSave.ToString(), false);
            this.Master.showSystemButton(SystemButton.btnCancel.ToString(), false);
            this.Master.showPanel(Area.DataArea.ToString(), false);

            //3.雖有編輯功能, 但不顯示修改的欄位
            this.Master.bolUpd_Show = false;*/

            //判斷是否由we020帶入
            if (Session["APLY_NO"] != null && Session["CUST_NO"] != null && Session["CUST_NAME"] != null)
            {
                this.bolWE020 = true;
                this.PCUST_NO.Text = ((String)Session["CUST_NO"]);
                this.PAPLY_NO.Text = ((String)Session["APLY_NO"]);
                this.PCUST_NAME.Text = ((String)Session["CUST_NAME"]);
                Session["APLY_NO"] = null;
                Session["CUST_NO"] = null;
                Session["CUST_NAME"] = null;
                this.Display_Command();
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void toExcel(object sender, EventArgs e)
        {
            if (this.rptQuery.Items.Count == 0)
            {
                string strMessage = "無資料可匯出！";
                this.setMessageBox(strMessage);
                return;

            }
            Session["qryString"] = this.getDisplay();
            this.setScript("exportToExcel('tbGrid');");
        }


        public string addMonths(int i)
        {
            if (this.PCANCEL_DATE.Text == "")
                return "";

            DateTime strDate = this.PCANCEL_DATE.Text.toDate();

            return strDate.AddMonths(i).ToString("yyyyMM");

        }

        private string getDisplay()
        {

            //exec s_WE110_Grid @PBLoc_no='G00024',@pcancel_date='20140505',@pmonths=12,@pdiscount_rate=3.2211,
	//@pdiscount_way='1'

            string strSQL = "exec s_WE110_Grid ";
            strSQL += " @PBLOC_NO='" + this.PBLOC_NO.Text.rpsText() + "'";
                   strSQL += ",@PCUST_NO='" + this.PCUST_NO.Text.rpsText() + "'";
            strSQL += ",@PFRC_CODE='" + this.PFRC_CODE.Text.rpsText() + "'";
            strSQL += ",@PAPLY_NO='" + this.PAPLY_NO.Text.rpsText() + "'";
            strSQL += ",@PCANCEL_DATE='" + this.PCANCEL_DATE.Text.rpsText().Replace("/","") + "'";
            strSQL += ",@PMONTHS=" + this.PMONTH.Text.rpsText() + "";
            strSQL += ",@PDiscount_Rate=" + this.PDiscount_Rate.Text.rpsText() + "";
            strSQL += ",@PDiscount_Way='" + (this.@PDiscount_Way.SelectedIndex==0 ? "1" : "2") + "'";


            return strSQL;

        }

        /// <summary>
        /// 資料查詢
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Display_Command()
        {



            string strMessage = "";
            if (this.PAPLY_NO.Text.Trim()=="" & PBLOC_NO.Text.Trim() == "" && PCUST_NO.Text.Trim() == "" && this.PFRC_CODE.Text.Trim()=="")
            {

                strMessage = "申請書編號及集團代號及客戶代號及供應商代號, 請至少輸入一個條件！";
                this.setMessageBox(strMessage);
                return;

            }


            if (this.PCANCEL_DATE.Text.Trim() == "" )
            {

                strMessage = "解約日期必須輸入！";
                this.setMessageBox(strMessage);
                return;

            }

            if (this.PMONTH.Text.Trim() == "0")
            {

                strMessage = "月數必須輸入！";
                this.setMessageBox(strMessage);
                return;

            }

            if (this.PMONTH.Text.toNumber()>12)
            {

                strMessage = "月數不得大於12！";
                this.setMessageBox(strMessage);
                return;

            }

            if (this.PDiscount_Rate.Text.Trim() == "0")
            {

                strMessage = "折扣值必須輸入！";
                this.setMessageBox(strMessage);
                return;

            }

         

         
            DataTable dt = dg.GetDataTable(this.getDisplay());


            int intMonths = this.PMONTH.Text.toInt();
            for (int i = intMonths + 1; i <= 12;i++ )
            {
                dt.Columns.Add("Month_" + i.ToString());
            }

            this.rptQuery.DataSource = dt;
            this.rptQuery.DataBind();
            //if (e.CommandName!="Query")


        }

    }
}
