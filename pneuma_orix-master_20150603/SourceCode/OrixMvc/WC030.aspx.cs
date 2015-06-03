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
    public partial class WC030 : OrixMvc.Pattern.PageParent
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


        private string getDisplay()
        {


            /*  @PBLOC_NO varchar(6)='',
	@PCUST_BLOC_NO varchar(6)='',
	@PFRC_CODE varchar(9)='',
	@PFRC_SNAME varchar(10)='',
	@PCUST_NO varchar(10)='',
	@PCUST_SNAME nvarchar(50)='',
	@PSALES_UNIT varchar(4)='',
	@PCUR_STS varchar(1)='',
	@PCON_DATE_FR_S varchar(10)='',
	@PCON_DATE_FR_E varchar(10)='',
	@PQUERY_Type varchar(1)='1'
            */
            string strSQL = "exec s_WC030_Grid ";
            strSQL += " @PBLOC_NO='" + this.BLOC_NO.Text.rpsText() + "'";
            strSQL += ",@PCUST_BLOC_NO='" + this.CUST_BLOC_CODE.Text.rpsText() + "'";
            strSQL += ",@PFRC_CODE='" + this.FRC_CODE.Text.rpsText() + "'";
            strSQL += ",@PCUST_NO='" + this.CUST_CODE.Text.rpsText() + "'";
            strSQL += ",@PSALES_UNIT='" + this.PSALES_UNIT.Text.rpsText() + "'";
            strSQL += ",@PCUR_STS='" + this.PCUR_STS.Text.rpsText() + "'";
            strSQL += ",@PCON_DATE_FR_S='" + this.PCON_DATE_FR_S.Text.rpsText() + "'";
            strSQL += ",@PCON_DATE_FR_E='" + this.PCON_DATE_FR_E.Text.rpsText() + "'";
            strSQL += ",@PQUERY_Type='" + this.PQUERY_Type.Text.rpsText() + "'";


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
            if ( BLOC_NO.Text.Trim() == ""  && FRC_CODE.Text.Trim()=="" && CUST_CODE.Text.Trim() == "" && CUST_BLOC_CODE.Text.Trim()=="")
            {

                strMessage = "供應商集團、供應商、客戶集團、客戶, 請至少輸入一個條件！";
                this.setMessageBox(strMessage);
                return;

            }


            DataTable dt = dg.GetDataTable(getDisplay());

            this.rptQuery.DataSource = dt;
            this.rptQuery.DataBind();



            //  if (SelectEvent != null)
            //      SelectEvent();



        }

    }
}
