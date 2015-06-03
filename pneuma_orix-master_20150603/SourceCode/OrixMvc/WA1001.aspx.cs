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
    public partial class WA1001 : PageParent
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
            //this.Master.KeyFields = "USER_ID";




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
            //this.POLICY_SUBJECT.Value = (String)Session["POLICY_SUBJECT"];
            //this.ASUR_TYPE_CODE.Value = (String)Session["ASUR_TYPE_CODE"];
            //this.APLY_NO.Value = (String)Session["APLY_NO"];



            //2.datagrid datatable
            rptEdit.DataSource = this.GetGridSource();
            rptEdit.DataBind();




        }
        #endregion


        protected void GridFunc_Click(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            DataTable dtGridSource;
            switch (((Button)sender).ID)
            {
                case "btnDel":
                    dtGridSource = GetGridSource();
                    string POLICY_NO = e.CommandName;
                    DataRow dr = dtGridSource.Select("POLICY_NO='" + POLICY_NO + "'")[0];
                    dr["STATUS"] = "D";

                    break;

                case "btnAdd":
                    string strMessage = "";
                    if (this.addPOLICY_NO.Text.Trim() == "")                    
                        strMessage+= "\\r\\n保單號碼必須輸入！";

                    if (this.addASUR_S_DATE.Text.Trim() == "")
                        strMessage += "\\r\\n保險起日必須輸入！";

                    if (this.addASUR_E_DATE.Text.Trim() == "")
                        strMessage += "\\r\\n保險迄日必須輸入！";

                    if (this.addASUR_E_DATE.Text.CompareTo(this.addASUR_S_DATE.Text)<= 0)
                        strMessage += "\\r\\n保險迄日必須大於保險起日！";
                    

                    if (strMessage != "")
                    {
                        strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                        this.setMessageBox(strMessage);
                        return ;
                    }

                    dtGridSource = GetGridSource();
                    if (ViewState["GridSource"] != null)
                    {
                        dtGridSource = (DataTable)ViewState["GridSource"];
                        rptEdit.DataSource = dtGridSource;
                        rptEdit.DataBind();                       
                        rptEdit.Visible = true;
                    }
                    else
                    {
                        rptEdit.DataSource = null;
                        rptEdit.DataBind();
                        rptEdit.Visible = false;
                    }
                    addPOLICY_NO.Clearing();
                    addASUR_S_DATE.Clearing();
                    addASUR_E_DATE.Clearing();
                    this.upGrid.Update();
                    break;
            }
        }

        private string GetGridSql()
        {
            string strSQL = "";

            strSQL+="exec s_WA1002_Grid";
            strSQL+=" @APLY_NO='"+ this.APLY_NO.Value +"',";
            strSQL += " @POLICY_SUBJECT='" + this.POLICY_SUBJECT.Value + "',";
            strSQL += " @ASUR_TYPE_CODE='" + this.ASUR_TYPE_CODE.Value + "'";    

            return strSQL;
        }

        private DataTable GetGridSource()
        {
            DataTable dtGridSource = null;

            if (ViewState["GridSource"] == null)
            {
                                
                dtGridSource = dg.GetDataTable(this.GetGridSql());
                DataColumn DC = new DataColumn();
                DC.ColumnName = "STATUS";
                DC.DefaultValue = "";
                DC.DataType = Type.GetType("System.String");
                dtGridSource.Columns.Add(DC);
                ViewState["GridSource"] = dtGridSource;
            }
            else
            {
                dtGridSource = (DataTable)ViewState["GridSource"];
                this.updateGrid(dtGridSource, rptEdit);
                DataRow dtRow = dtGridSource.NewRow();
                dtRow["POLICY_NO"] = addPOLICY_NO.Text.Trim();
                dtRow["ASUR_S_DATE"] = addASUR_S_DATE.Text.Trim();
                dtRow["ASUR_E_DATE"] = addASUR_E_DATE.Text.Trim();
                 dtRow["Status"] = "A";

                dtGridSource.Rows.Add(dtRow);
                ViewState["GridSource"] = dtGridSource;
            }

            return dtGridSource;
        }


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
            /*START 勿動 更新Grid*/
            GetGridSource();
            /*END 勿動 更新Grid*/

            DataTable dt = dts.GetTable("OR_CASE_APLY_ASUR", "APLY_NO='" + this.APLY_NO.Value.Trim() + "'");         
            DataRow[] adr;
            DataRow dr;
            DataRow drF;


            for (int i = 0; i < this.rptEdit.Items.Count; i++)
            {
                string STATUS = this.rptEdit.Items[i].FindControl("STATUS").value();
                string POLICY_NO = this.rptEdit.Items[i].FindControl("POLICY_NO").value();
                string ASUR_S_DATE = this.rptEdit.Items[i].FindControl("ASUR_S_DATE").value().Replace("/","");
                string ASUR_E_DATE = this.rptEdit.Items[i].FindControl("ASUR_E_DATE").value().Replace("/", "");

                string strMessage = "";
                if (POLICY_NO == "")
                    strMessage += "\\r\\n保單號碼必須輸入！";

                if (ASUR_S_DATE == "")
                    strMessage += "\\r\\n保險起日必須輸入！";

                if (ASUR_E_DATE == "")
                    strMessage += "\\r\\n保險迄日必須輸入！";

                if (ASUR_E_DATE.CompareTo(ASUR_S_DATE) <= 0)
                    strMessage += "\\r\\n保險迄日必須大於保險起日！";

                if (strMessage != "")
                {
                    strMessage = ("第"+i.ToString()+"筆, "+ strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                    this.setMessageBox(strMessage);
                    return 0;
                }


                switch (STATUS)
                {
                    case "A":
                       

                        dr = dt.NewRow();
                        dr["APLY_NO"] = APLY_NO;
                        dr["POLICY_SUBJECT"] = POLICY_SUBJECT.Value;
                        dr["ASUR_TYPE_CODE"] = ASUR_TYPE_CODE.Value;
                        dr["POLICY_NO"] = POLICY_NO;
                        dr["ASUR_S_DATE"] = ASUR_S_DATE;
                        dr["ASUR_E_DATE"] = ASUR_E_DATE;
                        dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["ADD_TIME"] = System.DateTime.Now.ToString("HHmmss");
                        dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HHmmss");
                        dt.Rows.Add(dr);

                       

                        break;

                    case "U":
                        dr = dt.Select("POLICY_NO='" + POLICY_NO + "'")[0];
                        dr["POLICY_NO"] = POLICY_NO;
                        dr["ASUR_S_DATE"] = ASUR_S_DATE;
                        dr["ASUR_E_DATE"] = ASUR_E_DATE;
                        dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HHmmss");

                        break;

                    case "D":
                        dt.DeleteRows("POLICY_NO='" + POLICY_NO + "'");
                        break;
                }

            }

            if (dts.Save())
            {
                this.setProcessMessage("資料處理成功!!", false);

                rptEdit.DataSource = this.GetGridSource();
                rptEdit.DataBind();              
                this.upGrid.Update();
                return 1;
                //this.rptEdit.DataBind();
                //此程式特殊, 需重新載入parent
                // this.setScript("window.parent.self.location.reload();");
            }
            else
            {
                this.setProcessMessage("資料處理失敗!!", true);
                return 2;
            }
        }
        #endregion



       



        #region DataCheck：儲存前驗證
        /// <summary>
        /// 儲存前驗證
        /// </summary>
        /// <returns>true/false 成功/失敗 </returns>
        private bool DataCheck(string strStatus)
        {
            string strSQL = "";
            string strMessage = "";


            switch (strStatus)
            {
                case "Add":
                case "Copy":
                   

                    if (strMessage != "")
                    {
                        strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                        this.setMessageBox(strMessage);
                        return false;
                    }

                    break;
            }


            return true;
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
           
        }
    }
}