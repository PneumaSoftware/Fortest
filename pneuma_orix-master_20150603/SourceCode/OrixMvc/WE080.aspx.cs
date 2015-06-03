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
    public partial class WE080 : OrixMvc.Pattern.PageParent
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

        public String nowRow
        {
            set { ViewState["nowRow"] = value; }
            get { return (ViewState["nowRow"] == null ? "" : (String)ViewState["nowRow"]); }
        }

        protected String strPeriod
        {
            set { ViewState["strPeriod"] = value; }
            get { return (ViewState["strPeriod"] == null ? "" : (String)ViewState["strPeriod"]); }
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
            if (Session["APLY_NO"]!=null )
            {
                this.bolWE020 = true;
               
                this.APLY_NO.Text = ((String)Session["APLY_NO"]);
                //this.CUST_NO.Editing(false);
                //this.CUST_NAME.Editing(false);
                Session["APLY_NO"] = null;

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





        protected void GridFunc_Click(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
           
            switch (((Button)sender).ID)
            {
                case "btnDetail":

                    this.nowRow = (((RepeaterItem)((Button)sender).Parent.Parent.Parent).ItemIndex + 1).ToString();
                    string strScript = "document.getElementById('trA" + this.nowRow + "').className='crow';";
                    this.setScript(strScript);

                    strPeriod = e.CommandName;
                    string strSQL = "exec s_WE080_Grid2";
                    strSQL += " @PAPLY_NO='" + this.APLY_NO.Text.rpsText() + "'";
                    strSQL += ",@PPERIOD=" + strPeriod + "";


                   DataTable  dt = dg.GetDataTable(strSQL);

                    this.rptDetail.DataSource = dt;
                    this.rptDetail.DataBind();
                    this.upGrid2.Update();

                    break;

                case "btbAdd":


                    break;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void toExcel(object sender, EventArgs e)
        {
            if (((Button)sender).ID == "M_excel")
            {
                if (this.rptQuery.Items.Count == 0)
                {
                    string strMessage = "無資料可匯出！";
                    this.setMessageBox(strMessage);
                    return;

                }

                Session["qryString"] = this.getDisplay();
                this.setScript("exportToExcel('tbGridM');");
            }
            else
            {
                if (this.rptDetail.Items.Count == 0)
                {
                    string strMessage = "無資料可匯出！";
                    this.setMessageBox(strMessage);
                    return;

                }
                string strSQL = "exec s_WE080_Grid2";
                strSQL += " @PAPLY_NO='" + this.APLY_NO.Text.rpsText() + "'";
                strSQL += ",@PPERIOD=" + strPeriod + "";

                Session["qryString"] = strSQL;
                this.setScript("exportToExcel('tbGridD');");
            }
        }


        private string getDisplay()
        {


            string strSQL = "exec s_WE080_Grid ";
            strSQL += " @PAPLY_NO='" + this.APLY_NO.Text.rpsText() + "'";
            strSQL += ",@PPERIOD=" + this.Period.Text.rpsText() + "";

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
            if (this.APLY_NO.Text.Trim() == "")
                strMessage += "[申請書編號]";

            

            if (strMessage != "")
                strMessage += "必須輸入！";

            if (strMessage != "")
            {
                strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                this.setMessageBox(strMessage);
                return ;
            }


           


            DataTable dt = dg.GetDataTable(this.getDisplay());

            this.rptQuery.DataSource = dt;
            this.rptQuery.DataBind();

            this.rptDetail.DataSource = null;
            this.rptDetail.DataBind();
            this.upGrid2.Update();


         /*   strSQL = "exec s_WE080_Grid2";
            strSQL += " @PAPLY_NO='" + this.APLY_NO.Text.rpsText() + "'";
            strSQL += ",@PPERIOD=" + this.Period.Text.rpsText() + "";


            dt = dg.GetDataTable(strSQL);

            this.rptQuery2.DataSource = dt;
            this.rptQuery2.DataBind();*/

        }

    }
}
