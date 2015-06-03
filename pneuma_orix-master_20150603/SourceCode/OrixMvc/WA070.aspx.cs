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
    public partial class WA070 : OrixMvc.Pattern.PageParent
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


        public string AplyNo
        {
            set { ViewState["AplyNo"] = value; }
            get { return (ViewState["AplyNo"] == null ? "" : (string)ViewState["AplyNo"]); }
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

            this.Master.ProcessEvent += new ProcessDelegate(Save_Click);


            if (!IsPostBack)
                this.setParms();
            //***************************end 勿動****************************
        }
        #endregion


        protected void myCase_Change(object sender, EventArgs e)
        {
            this.setParms();
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
           
            // [dbo].[s_WA070_Grid](@PEmp_code varchar(10),@PMode smallint, @PMy_Case char(1))

            string strSQL = "exec s_WA070_Grid @PEmp_code='"+ this.Master.Master.EmployeeId +"',@PMode=#MMode#, @PMy_Case='"+ (this.myCase.Checked?"Y":"N")+"'";

            DataTable dt;

            for (int i = 1; i <= 5;i++ )
            {
                dt = dg.GetDataTable(strSQL.Replace("#MMode#", i.ToString()));
                Repeater rpt = (Repeater)this.Master.Master.masterFindControl("rptEdit" + i.ToString());
                rpt.DataSource = dt;
                rpt.DataBind();
            }

            this.Master.bolSave = false;
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

           // this.Master.editSave = false;

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


        protected void appove_Click(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {

            string pageDetail = "";
 
            switch (((Button)sender).ID)
            {
                case "appove1": //額度申請 WB010
                  

                    string[] aryWB010 = e.CommandName.Split(',');
                    Session["QUOTA_APLY_NO"] = aryWB010[0].ToString().Trim();
                    Session["CUR_STS"] = aryWB010[2].ToString().Trim();
                    Session["bolWA070"] = true;
                    pageDetail = "WB0101";

                    break;

                case "appove2": //一般案件申請WA060
                    string[] aryWA060 = e.CommandName.Split(',');
                    Session["APLY_NO"] = aryWA060[0].ToString().Trim();
                    Session["CUR_STS"] = aryWA060[1].ToString().Trim() +"," + aryWA060[2].ToString().Trim();
                    Session["bolWA070"] = true;                    
                    pageDetail = "WA0601";
                    break;

                case "appove3": //主約申請WA050
                case "appove31": //主約申請WA050
                    if (((Button)sender).ID == "appove3")
                    {
                        Session["nowStatus"] = "Appove";
                        Session["nowStatusName"] = "核淮";
                    }
                    else {
                        Session["nowStatus"] = "Cancel";
                        Session["nowStatusName"] = "作廢";
                    }
                    string[] aryWA050 = e.CommandName.Split(',');
                    Session["MAST_CON_NO"] = aryWA050[0].ToString().Trim();
                 //   Session["CUR_STS"] = aryWA050[1].ToString().Trim() + "," + aryWA050[2].ToString().Trim();
                    Session["bolWA070"] = true;
                    pageDetail = "WA0501";
                    break;

                case "appove4": //先行出合約申請WA020
                case "appove41": //先行出合約申請WA020
                    if (((Button)sender).ID == "appove4")
                    {
                        Session["nowStatus"] = "Appove";
                        Session["nowStatusName"] = "核淮";
                    }
                    else
                    {
                        Session["nowStatus"] = "Cancel";
                        Session["nowStatusName"] = "作廢";
                    }
                    string[] aryWA020 = e.CommandName.Split(',');
                    Session["APLY_NO"] = aryWA020[0].ToString().Trim();
                    Session["FAST_STS"] = aryWA020[2].ToString().Trim();
                    Session["bolWA070"] = true;
                    pageDetail = "WA0201";

                    break;

                case "appove5": //供應商審核WC010
                    string[] aryWC010 = e.CommandName.Split(',');
                    Session["FRC_CODE"] = aryWC010[0].ToString().Trim();                
                    Session["bolWA070"] = true;
                    pageDetail = "WC0101";
                    break;

              

            }

           string strScript = "pageDetail='" + pageDetail + ".aspx?ndt=" + DateTime.Now.ToString("HHmmss") + "';\n";
            strScript += "contentChange('frameDetail');\n";


            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "pageChange", strScript, true);

        }  




        #region Save_Click：儲存鍵觸發
        /// <summary>
        /// 儲存鍵觸發Event,
        /// 本頁作業：無  
        /// </summary>
        private void Save_Click(string strType)
        {

            
        }
        #endregion


        /// <summary>
        /// 資料查詢
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Status_Click(object sender, CommandEventArgs e)
        {

            string strMessage = "";
            
            switch (e.CommandName)
            {
                case "Query":
                    

                    break;

              

            }

            //if (e.CommandName!="Query")



            //  if (SelectEvent != null)
            //      SelectEvent();


        }

    }
}
