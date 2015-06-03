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
    public partial class WG040 : OrixMvc.Pattern.PageParent
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

            /*
            @PBANK_NO varchar(10)='',--銀行代碼
	@PINTEREST_YM varchar(6)='',--繳息年月
	@PRED_DATE varchar(8)='',--還款日期
	@PDUE_DATE_S varchar(8)='',--借款到期日_起
	@PDUE_DATE_E varchar(8)='',--借款到期日_迄
	@PLOAN_MTHD_CODE varchar(5)='',--借款方式
	@PCredit_way varchar(1)='',--授信方式
	@PLong_Short_LOAN varchar(1)='',--長短借
	@PREPAY_way varchar(1)=''--一次/多次攤還
             */

            string strSQL = "exec s_WG040_Grid ";
            strSQL += " @PBANK_NO='" + this.PBANK_NO.Text.rpsText() + "'";
            strSQL += ",@PINTEREST_YM='" + this.PINTEREST_YM.Text.rpsText() + "'";
            strSQL += ",@PRED_DATE='" + this.PRED_DATE.Text.rpsText() + "'";
            strSQL += ",@PDUE_DATE_S='" + this.PDUE_DATE_S.Text.rpsText() + "'";
            strSQL += ",@PDUE_DATE_E='" + this.PDUE_DATE_E.Text.rpsText().Replace("/", "") + "'";
            strSQL += ",@PLOAN_MTHD_CODE='" + this.PLOAN_MTHD_CODE.SelectedValue.rpsText() + "'";
            strSQL += ",@PCredit_way='" + this.PCredit_way.Text.rpsText() + "'";
            strSQL += ",@PLong_Short_LOAN='" + this.PLong_Short_LOAN.SelectedValue + "'";

            strSQL += ",@PREPAY_way='" + this.PREPAY_way.SelectedValue + "'";

            return strSQL;

        }

        /// <summary>
        /// 資料查詢
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Display_Command()
        {
                    




            DataTable dt = dg.GetDataTable(this.getDisplay());


           

            this.rptQuery.DataSource = dt;
            this.rptQuery.DataBind();
            //if (e.CommandName!="Query")


        }

    }
}
