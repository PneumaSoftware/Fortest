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
    public partial class WD0401 : PageParent
    {

        protected bool bolWE020
        {
            set { ViewState["bolWE020"] = value; }
            get { return (ViewState["bolWE020"] == null ? false : (bool)ViewState["bolWE020"]); }
        }


        protected DataTable dtPAPER
        {
            set { Session["dtPAPER"] = value; }
            get { return (Session["dtPAPER"] == null ? null : (DataTable)Session["dtPAPER"]); }
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

            this.Master.SaveEvent += new SaveDelegate(Save_Click);
            this.Master.EditEvent += new DataDelegate(Data_Command);
            this.Master.ExitEvent += new ExitDelegate(Exit_Click);
            this.Master.loadEvent += new loadDelegate(setDefaultValue);
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
            
            //1.程式編輯功能
            this.Master.setEditingFunction(false, true, false);

            this.Master.showSystemButton(SystemButton.btnSave.ToString(), false);
            this.Master.showSystemButton(SystemButton.btnCancel.ToString(), false);
            this.Master.showPanel(Area.DataArea.ToString(), false);
            */

            //2.取得前頁的session欄位



            //3.key fields
            this.Master.KeyFields = "APLY_NO";

            if (Session["bolWE020"] != null)
            {
                this.bolWE020 = true;
                Session["bolWE020"] = null;

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
            //1.form
            //this.USER_ID.Text = (String)Session["USER_ID"];


            // this.rptFRC_VER.DataSourceID = "sqlFRC_VER";
            //  this.rptFRC_VER.DataBind();


           

        }


       

        #endregion








        private bool SaveGrid()
        {



            DataTable dtUSE = dts.GetTable("OR3_PAPER_USE_DTL_TMP", "APLY_NO='" + this.APLY_NO.Value + "'");
            DataTable dtINV = dts.GetTable("OR3_PAPER_SUPPLY_INV_TEMP", "APLY_NO='" + this.APLY_NO.Value + "'");
            DataRow dr;


            /*END 勿動 更新Grid*/


            DataTable dtCopy;
            DataRow[] drCopy;

            for (int i = 0; i < this.rptUSE.Items.Count; i++)
            {

                int PERIOD = this.rptUSE.Items[i].FindControl("PERIOD").value().toInt();
                string MAC_NO = this.rptUSE.Items[i].FindControl("MAC_NO").value();
                int txtPERIOD = this.rptUSE.Items[i].FindControl("txtPERIOD").value().toInt();
                string txtMAC_NO = this.rptUSE.Items[i].FindControl("txtMAC_NO").value();
                string txtAPLY_NO = this.rptUSE.Items[i].FindControl("txtAPLY_NO").value();
                double MONTH_FEE = this.rptUSE.Items[i].FindControl("MONTH_FEE").value().toNumber();
                double USE_QTY_COLOR = this.rptUSE.Items[i].FindControl("USE_QTY_COLOR").value().toNumber();
                double USE_QTY_MONO = this.rptUSE.Items[i].FindControl("USE_QTY_MONO").value().toNumber();
                double USE_QTY_A3_COLOR = this.rptUSE.Items[i].FindControl("USE_QTY_A3_COLOR").value().toNumber();
                double USE_QTY_A3_MONO = this.rptUSE.Items[i].FindControl("USE_QTY_A3_MONO").value().toNumber();
                double SUM_A4_COLOR = this.rptUSE.Items[i].FindControl("SUM_A4_COLOR").value().toNumber();
                double SUM_A4_COLOR_LAST = this.rptUSE.Items[i].FindControl("SUM_A4_COLOR_LAST").value().toNumber();
                double SUM_A4_MONO = this.rptUSE.Items[i].FindControl("SUM_A4_MONO").value().toNumber();
                double SUM_A4_MONO_LAST = this.rptUSE.Items[i].FindControl("SUM_A4_MONO_LAST").value().toNumber();
                double SUM_A3_COLOR = this.rptUSE.Items[i].FindControl("SUM_A3_COLOR").value().toNumber();
                double SUM_A3_COLOR_LAST = this.rptUSE.Items[i].FindControl("SUM_A3_COLOR_LAST").value().toNumber();
                double SUM_A3_MONO = this.rptUSE.Items[i].FindControl("SUM_A3_MONO").value().toNumber();
                double SUM_A3_MONO_LAST = this.rptUSE.Items[i].FindControl("SUM_A3_MONO_LAST").value().toNumber();

                
                drCopy = dtUSE.Select("isnull(PERIOD,0)=" + PERIOD + " and MAC_NO='" + MAC_NO + "'");
                if (drCopy.Length > 0)
                {
                    dr = drCopy[0];

                    dr["PERIOD"] = txtPERIOD;
                    dr["MAC_NO"] = txtMAC_NO;
                    dr["APLY_NO"] = txtAPLY_NO;
                    dr["MONTH_FEE"] = MONTH_FEE;
                    dr["USE_QTY_COLOR"] = USE_QTY_COLOR;
                    dr["USE_QTY_MONO"] = USE_QTY_MONO;
                    dr["USE_QTY_A3_COLOR"] = USE_QTY_A3_COLOR;
                    dr["USE_QTY_A3_MONO"] = USE_QTY_A3_MONO;
                    dr["SUM_A4_COLOR"] = SUM_A4_COLOR;
                    dr["SUM_A4_COLOR_LAST"] = SUM_A4_COLOR_LAST;
                    dr["SUM_A4_MONO"] = SUM_A4_MONO;
                    dr["SUM_A4_MONO_LAST"] = SUM_A4_MONO_LAST;
                    dr["SUM_A3_COLOR"] = SUM_A3_COLOR;
                    dr["SUM_A3_COLOR_LAST"] = SUM_A3_COLOR_LAST;
                    dr["SUM_A3_MONO"] = SUM_A3_MONO;
                    dr["SUM_A3_MONO_LAST"] = SUM_A3_MONO_LAST;

                    dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                    dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                    dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                }

            }
            for (int i = 0; i < this.rptINV.Items.Count; i++)
            {

                int PERIOD = this.rptINV.Items[i].FindControl("PERIOD").value().toInt();
                string INV_NO = this.rptINV.Items[i].FindControl("INV_NO").value();
                string INV_DATE = this.rptINV.Items[i].FindControl("INV_DATE").value().Replace("/","");
                string OLD_INV_NO = this.rptINV.Items[i].FindControl("OLD_INV_NO").value();
                double AMOUNT = this.rptINV.Items[i].FindControl("AMOUNT").value().toNumber();
                double TAX = this.rptINV.Items[i].FindControl("TAX").value().toNumber();
                double SUPPLY_INV_AMT = this.rptINV.Items[i].FindControl("SUPPLY_INV_AMT").value().toNumber();

                drCopy = dtINV.Select("isnull(PERIOD,0)=" + PERIOD + " and INV_NO='" + OLD_INV_NO + "'");
                if (drCopy.Length > 0)
                {
                    dr = drCopy[0];

                    dr["INV_NO"] = INV_NO;
                    dr["AMOUNT"] = AMOUNT;
                    dr["TAX"] = TAX;
                    dr["INV_DATE"] = INV_DATE;
                    dr["SUPPLY_INV_AMT"] = SUPPLY_INV_AMT;
                   
                   
                }

            }

            if (!dts.Save())
                return false;

            cts.Execute("exec s_WD030_DataCheck");

            return true;

        }

      

       

        #region DataCheck：儲存前驗證
        /// <summary>
        /// 儲存前驗證
        /// </summary>
        /// <returns>true/false 成功/失敗 </returns>
        private bool DataCheck(string strStatus)
        {
           
            string strMessage = "";

            
            switch (strStatus)
            {
                case "Add":
                case "Copy":
                case "Upd":
                    
                 

                    if (strMessage != "")
                        strMessage += "必須輸入！";

                    if (strMessage != "")
                    {
                        strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                        this.setMessageBox(strMessage);
                        return false;
                    }

                    break;

                case "Del":
                    break;
            }


            return true;
        }
        #endregion


        #region Save_Click：儲存鍵觸發
        /// <summary>
        /// 本頁作業：新增、修改、刪除
        /// </summary>
        /// <param name="strStatus">
        /// 系統作業狀態：
        /// Add.新增。        
        /// Upd.修改，若有在編輯區下Command，則搭配this.Master.SubStatus。
        /// Del.刪除。
        /// </param>
        /// <returns>0:驗證失敗／1.處理失敗／2.處理成功</returns>
        private int Save_Click(string strStatus)
        {
            if (!DataCheck(strStatus))
                return 0;

            if (!this.SaveGrid())
                return 1;
            else            
                return 2;
            


        }
        #endregion



        
        #region Exit_Click：離開鍵觸發
        /// <summary>
        /// 離開鍵觸發Event,
        /// 本頁作業：無  
        /// </summary>
        private void Exit_Click()
        {
           
        }
        #endregion


       

        /// <summary>
        /// 編輯區域裏有設command的按鍵被按下時, 將觸發此event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Data_Command(object sender, FormViewCommandEventArgs e)
        {


        }





        /// <summary>
        /// popup window 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PopUp_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            string strMessage = "";
            /*switch (e.CommandName)
            {
                case "ChangePASS":

                   
                    break;
            }*/
        }
    }
}