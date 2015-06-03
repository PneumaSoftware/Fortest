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
    public partial class WB020 : OrixMvc.Pattern.PageParent
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

            this.Master.PrintEvent += new PrintDelegate(Print_Click);


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

        }
        #endregion


        #region PrintCheck：列印前驗證
        /// <summary>
        /// 查詢前的驗證
        /// </summary>
        /// <returns>驗證成功或失敗：true/false</returns>
        private bool PrintCheck()
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
                return false;
            }

            return true;
        }
        #endregion


        #region Print_Click：列印鍵觸發
        /// <summary>
        /// 儲存鍵觸發Event,
        /// 本頁作業：無  
        /// </summary>
        private void Print_Click()
        {

            if (!PrintCheck())
                return;


            //傳參數給報表
            string PRTSERVER = ConfigurationManager.AppSettings["RPTSERVER"].ToString();
            string PRJCODE = ConfigurationManager.AppSettings["PRJCODE"].ToString();
            
            string FILENAME = this.Master.Master.ProgramId;
            string SYS = this.Master.Master.ProgramId.Substring(0, 2);

            string URL = "http://" + PRTSERVER + "/Smart-Query/squery.aspx?Path=" + PRJCODE + "&filename=" + FILENAME + "&sys=" + SYS;
            URL += "&Parameter1=" + this.APLY_NO.Text.Trim();
            URL += "&Parameter2=" + this.PrintType.SelectedValue;
        //    URL += "&Parameter3=" + (this.chkHistory.Checked?"Y":"N");


            string js = "window.open('" + URL + "','','height=600,width=1024,status=yes,toolbar=yes,menubar=yes,location=no,Resizable = yes','')";

            //指向報表頁面
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "openReport", js, true);

        }
        #endregion



    }
}
