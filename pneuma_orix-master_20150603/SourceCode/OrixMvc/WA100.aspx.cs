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
    public partial class WA100 : OrixMvc.Pattern.PageParent
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


        public String POLICY_SUBJECT
        {
            set { ViewState["POLICY_SUBJECT"] = value; }
            get { return (ViewState["POLICY_SUBJECT"] == null ? "" : (String)ViewState["POLICY_SUBJECT"]); }
        }

        public String ASUR_TYPE_CODE
        {
            set { ViewState["ASUR_TYPE_CODE"] = value; }
            get { return (ViewState["ASUR_TYPE_CODE"] == null ? "" : (String)ViewState["ASUR_TYPE_CODE"]); }
        }

        public String AplyNo
        {
            set { ViewState["AplyNo"] = value; }
            get { return (ViewState["AplyNo"] == null ? "" : (String)ViewState["AplyNo"]); }
        }

        public String nowRow
        {
            set { ViewState["nowRow"] = value; }
            get { return (ViewState["nowRow"] == null ? "" : (String)ViewState["nowRow"]); }
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


        }
        #endregion



        #region setDefaultValue：欄位預設值
        /// <summary>
        /// 設定欄位預設值
        /// 本頁作業：無作用
        /// </summary>
        private void setDefaultValue()
        {
            //2.datagrid datatable
           
        }
        #endregion

        


        /// <summary>
        /// 資料查詢
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Display_Command()
        {


            string strMessage = "";

            if (APLY_NO.Text.Trim() == "")
                strMessage += "申請書編號必須輸入！";


            if (strMessage != "")
            {
                this.AplyNo = "";
                this.rptEdit.DataSource = null;
                this.rptEdit.DataBind();
                this.rptQuery.DataSource = null;
                this.rptQuery.DataBind();
                this.APLY_NO.Focus();
                upGrid2.Update();
                upGrid1.Update();

                this.setMessageBox(strMessage);
                return;
            }

            if (dg.GetDataRow("SELECT * FROM OR_CASE_APLY_BASE WHERE APLY_NO='" + this.APLY_NO.Text.Trim() + "'")[0].ToString().Trim() == "")
            {
                this.AplyNo = "";
                this.rptEdit.DataSource = null;
                this.rptEdit.DataBind();
                this.rptQuery.DataSource = null;
                this.rptQuery.DataBind();
                this.APLY_NO.Focus();
                upGrid2.Update();
                upGrid1.Update();
                this.setMessageBox("申請書編號輸入錯誤, 請重新輸入!!");
                return;
            }

            this.AplyNo = this.APLY_NO.Text.Trim();
            string strSQL = "exec s_WA100_Grid  @APLY_NO='" + AplyNo + "'";

            DataTable dt = dg.GetDataTable(strSQL);

            this.rptQuery.DataSource = dt;
            this.rptQuery.DataBind();
            this.upGrid1.Update();

            ViewState["GridSource"] = null;
            this.rptEdit.DataSource = null;
            this.rptEdit.DataBind();
            addPOLICY_NO.Clearing();
            addASUR_S_DATE.Clearing();
            addASUR_E_DATE.Clearing();
            this.upGrid2.Update();
            this.POLICY_SUBJECT = "";
            this.ASUR_TYPE_CODE = "";
            

           

           // this.APLY_NO.Editing(false);



            //  if (SelectEvent != null)
            //      SelectEvent();


        }

        #region grid function
        protected void GridFunc_Click(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            DataTable dtGridSource;
            switch (((Button)sender).ID)
            {
                case "btnDetail":
                    ViewState["GridSource"] = null;
                    string[] aryData = e.CommandName.Split(',');
                    this.nowRow = (((RepeaterItem)((Button)sender).Parent.Parent.Parent).ItemIndex + 1).ToString();
                    string strScript = "document.getElementById('tr" + nowRow  + "').className='crow';";
                    this.setScript(strScript);
                    
                    this.POLICY_SUBJECT=aryData[0].ToString().Substring(0,1);
                    this.ASUR_TYPE_CODE=aryData[1].ToString();

                    rptEdit.DataSource = this.GetGridSource(false);
                    rptEdit.DataBind();

                    this.upGrid2.Update();


                    break;

                case "btnDel":
                    dtGridSource =  (DataTable)ViewState["GridSource"];
                    string POLICY_NO = e.CommandName;
                    DataRow dr = dtGridSource.Select("POLICY_NO='" + POLICY_NO + "'")[0];
                    dr["STATUS"] = "D";
                    rptEdit.DataSource = dtGridSource;
                    rptEdit.DataBind();
                    this.upGrid2.Update();

                    break;

                case "btnAdd":
                    string strMessage = "";
                    if (this.addPOLICY_NO.Text.Trim() == "")
                        strMessage += "\\r\\n保單號碼必須輸入！";

                    if (this.addASUR_S_DATE.Text.Trim() == "")
                        strMessage += "\\r\\n保險起日必須輸入！";

                    if (this.addASUR_E_DATE.Text.Trim() == "")
                        strMessage += "\\r\\n保險迄日必須輸入！";

                    if (this.addASUR_E_DATE.Text.CompareTo(this.addASUR_S_DATE.Text) <= 0)
                        strMessage += "\\r\\n保險迄日必須大於保險起日！";

                    if (ViewState["GridSource"] != null && this.addASUR_S_DATE.Text.Trim()!="")
                    {
                        dtGridSource = (DataTable)ViewState["GridSource"];
                        if (dtGridSource.Rows.Count > 0)
                        {
                            int intRow = dtGridSource.Rows.Count-1;
                            DataRow drAdd = dtGridSource.Rows[intRow];
                            while (drAdd["Status"].ToString() == "D")
                            {
                                intRow--;
                                if (intRow< 0)
                                    break;
                                drAdd = dtGridSource.Rows[intRow];

                                
                            }
                            if (intRow >= 0)
                            {
                                if (drAdd["ASUR_E_DATE"].ToString().CompareTo(this.addASUR_S_DATE.Text.Trim()) >= 0)
                                    strMessage += "\\r\\n保險起日必須大於前一筆的保險迄日！";
                            }
                        }

                    }
                    if (strMessage != "")
                    {
                        strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                        this.setMessageBox(strMessage);
                        return;
                    }

                    dtGridSource = GetGridSource(true);
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
                    if (nowRow != "")
                    {
                        strScript = "document.getElementById('tr" + nowRow + "').className='crow';";
                        this.setScript(strScript);
                    }

                    addPOLICY_NO.Clearing();
                    addASUR_S_DATE.Clearing();
                    addASUR_E_DATE.Clearing();                    
                    this.upGrid2.Update();
                    break;
            
            }
        }

        private string GetGridSql()
        {
            string strSQL = "";

            strSQL += "exec s_WA1002_Grid";
            strSQL += " @APLY_NO='" + this.AplyNo + "',";
            strSQL += " @POLICY_SUBJECT='" + this.POLICY_SUBJECT + "',";
            strSQL += " @ASUR_TYPE_CODE='" + this.ASUR_TYPE_CODE + "'";

            return strSQL;
        }

        private DataTable GetGridSource(bool bolAdd)
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
                dtGridSource = (DataTable)ViewState["GridSource"];

            if (bolAdd){
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
        #endregion


        protected void Clear_Click(object sender, EventArgs e)
        {
            this.setScript("window.self.location.reload();");          

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
        protected  void Save_Click(object sender,System.Web.UI.WebControls.CommandEventArgs e)
        {
         //   if (!DataCheck(e.CommandName ))
            //    return 0;
            /*START 勿動 更新Grid*/
            this.updateGrid(this.GetGridSource(false), rptEdit);
            /*END 勿動 更新Grid*/
            dts = new VS2008.Module.DataSetToSql();
            DataTable dt = dts.GetTable("OR3_CASE_APLY_POLICY", "APLY_NO='" + AplyNo + "' and ASUR_TYPE_CODE='" + this.ASUR_TYPE_CODE + "' and POLICY_SUBJECT='" + this.POLICY_SUBJECT + "'");
            DataRow[] adr;
            DataRow dr;
            DataRow drF;


            for (int i = 0; i < this.rptEdit.Items.Count; i++)
            {
                string STATUS = this.rptEdit.Items[i].FindControl("STATUS").value();
                string POLICY_NO = this.rptEdit.Items[i].FindControl("POLICY_NO").value();
                string ASUR_S_DATE = this.rptEdit.Items[i].FindControl("ASUR_S_DATE").value().Replace("/", "");
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


                if (i != 0 && ASUR_S_DATE != "")
                {


                    int intRow = i - 1;
                    while (this.rptEdit.Items[intRow].FindControl("STATUS").value() == "D")
                    {
                        intRow--;

                        if (intRow < 0)
                            break;
                    }
                    if (intRow >= 0)
                    {
                        if (this.rptEdit.Items[intRow].FindControl("ASUR_E_DATE").value().Replace("/", "").CompareTo(ASUR_S_DATE) >= 0)
                            strMessage += "\\r\\n保險起日必須大於前一筆的保險迄日！";
                    }
                }

               
                if (strMessage != "")
                {
                    strMessage = "保單號碼:" + POLICY_NO + ", " + (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                    this.setMessageBox(strMessage);
                    return ;
                }


                switch (STATUS)
                {
                    case "A":


                        dr = dt.NewRow();
                        dr["APLY_NO"] = this.AplyNo ;
                        dr["POLICY_SUBJECT"] = POLICY_SUBJECT;
                        dr["ASUR_TYPE_CODE"] = ASUR_TYPE_CODE;
                        dr["POLICY_NO"] = POLICY_NO;
                        dr["ASUR_S_DATE"] = ASUR_S_DATE.Replace("/", "");
                        dr["ASUR_E_DATE"] = ASUR_E_DATE.Replace("/", "");
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
                        dr["ASUR_S_DATE"] = ASUR_S_DATE.Replace("/","");
                        dr["ASUR_E_DATE"] = ASUR_E_DATE.Replace("/", "");
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
                ViewState["GridSource"] = null;
                this.rptEdit.DataSource = this.GetGridSource(false); 
                this.rptEdit.DataBind();
                string strSQL = "exec s_WA100_Grid  @APLY_NO='" + this.AplyNo  + "'";

                DataTable dtQuery = dg.GetDataTable(strSQL);

                this.rptQuery.DataSource = dtQuery;
                this.rptQuery.DataBind();
                this.upGrid1.Update();

                string strScript = "document.getElementById('tr" + nowRow + "').className='crow';";
                this.setScript(strScript);

                //此程式特殊, 需重新載入parent
                // this.setScript("window.parent.self.location.reload();");
            }
            else
            {
                this.setProcessMessage("資料處理失敗!!", true);
              //  return 2;
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

    }
}
