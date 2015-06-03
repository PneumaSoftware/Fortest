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
    public partial class WE100 : OrixMvc.Pattern.PageParent
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


            this.Master.DisplayEvent += new displayDelegate(Display_Command);

            if (!IsPostBack)
                this.setParms();
            //***************************end 勿動****************************
        }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void toExcel(object sender, EventArgs e)
        {
            if (this.rptQuery.Items.Count==0){
                string strMessage = "無資料可匯出！";
                this.setMessageBox(strMessage);
                return;

            }
            Session["qryString"] = this.getDisplay();
            this.setScript("exportToExcel('tbGrid');");
        }

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


        private string getDisplay(){
           

           
            string strSQL = "exec s_WE100_Grid ";
            strSQL+=" @PBLOC_NO='"+ this.PBLOC_NO.Text.rpsText()+"'";            
            strSQL+=",@PCUST_NO='"+ this.PCUST_NO.Text.rpsText()+"'";
            strSQL += ",@PIncludeClosed='" + this.PIncludeClosed.SelectedValue.rpsText() + "'";
            strSQL += ",@PIncludeBail='" + (this.PIncludeBail.Checked?"Y":"N") + "'";

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
            if (PBLOC_NO.Text.Trim() == "" && PCUST_NO.Text.Trim() == "")
            {

                strMessage = "集團代號及客戶代號, 請至少輸入一個條件！";
                this.setMessageBox(strMessage);
                return;

            }

            if (PBLOC_NO.Text.Trim() != "" && PCUST_NO.Text.Trim() != "")
            {

                strMessage = "集團代號及客戶代號, 只能輸入一個條件！";
                this.setMessageBox(strMessage);
                return;

            }

            if (this.PBLOC_NO.Text.Trim() != "" && this.PIncludeBail.Checked)
            {
                this.PIncludeBail.Checked = false;
                this.setScript("document.getElementById('" + this.PIncludeBail.ClientID + "').checked=false");
            }

            DataTable dt = dg.GetDataTable(this.getDisplay());

            string strKey = "SCUR_RELATION='本戶' and TR<20";
            this.CON_TOL1.Text = dt.Compute("sum(CON_TOL)", strKey).ToString().toNumber().ToString("###,###,##0");
            this.PURS_AMT1.Text = dt.Compute("sum(PURS_AMT)", strKey).ToString().toNumber().ToString("###,###,##0");
            this.TR1.Text = dt.Compute("Sum(TR_TOT1)/Sum(TR_TOT2)", strKey).ToString().toNumber().ToString("###0.###0");
            this.MARG1.Text = dt.Compute("sum(MARG)", strKey).ToString().toNumber().ToString("###,###,##0");
            this.CON_SUR_AMT1.Text = dt.Compute("sum(CON_SUR_AMT)", strKey).ToString().toNumber().ToString("###,###,##0");

            strKey = "SCUR_RELATION='保證' and TR<20";
            this.CON_TOL2.Text = dt.Compute("sum(CON_TOL)", strKey).ToString().toNumber().ToString("###,###,##0");
            this.PURS_AMT2.Text = dt.Compute("sum(PURS_AMT)", strKey).ToString().toNumber().ToString("###,###,##0");
            this.TR2.Text = dt.Compute("Sum(TR_TOT1)/Sum(TR_TOT2)", strKey).ToString().toNumber().ToString("###0.###0");
            this.MARG2.Text = dt.Compute("sum(MARG)", strKey).ToString().toNumber().ToString("###,###,##0");
            this.CON_SUR_AMT2.Text = dt.Compute("sum(CON_SUR_AMT)", strKey).ToString().toNumber().ToString("###,###,##0");

            strKey = "SCUR_RELATION='集團' and TR<20";
            this.CON_TOL3.Text = dt.Compute("sum(CON_TOL)", strKey).ToString().toNumber().ToString("###,###,##0");
            this.PURS_AMT3.Text = dt.Compute("sum(PURS_AMT)", strKey).ToString().toNumber().ToString("###,###,##0");
            this.TR3.Text = dt.Compute("Sum(TR_TOT1)/Sum(TR_TOT2)", strKey).ToString().toNumber().ToString("###0.###0");
            this.MARG3.Text = dt.Compute("sum(MARG)", strKey).ToString().toNumber().ToString("###,###,##0");
            this.CON_SUR_AMT3.Text = dt.Compute("sum(CON_SUR_AMT)", strKey).ToString().toNumber().ToString("###,###,##0");

            this.CON_TOL.Text = dt.Compute("sum(CON_TOL)", "").ToString().toNumber().ToString("###,###,##0");
            this.PURS_AMT.Text = dt.Compute("sum(PURS_AMT)", "").ToString().toNumber().ToString("###,###,##0");
            this.TR.Text = dt.Compute("Sum(TR_TOT1)/Sum(TR_TOT2)", "").ToString().toNumber().ToString("###0.###0");
            this.MARG.Text = dt.Compute("sum(MARG)", "").ToString().toNumber().ToString("###,###,##0");
            this.CON_SUR_AMT.Text = dt.Compute("sum(CON_SUR_AMT)", "").ToString().toNumber().ToString("###,###,##0");

            this.rptQuery.DataSource = dt;
            this.rptQuery.DataBind();
            //if (e.CommandName!="Query")



            //  if (SelectEvent != null)
            //      SelectEvent();


        }

    }
}
