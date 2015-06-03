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
using System.Data.SqlClient;

namespace OrixMvc
{
    public partial class WD0101 : PageParent
    {


        public DataTable GridObject
        {
            set { ViewState["GridObject"] = value; }
            get { return (ViewState["GridObject"] == null ? null : (DataTable)ViewState["GridObject"]); }
        }

        

        public String nowRow_Object
        {
            set { ViewState["nowRow_Object"] = value; }
            get { return (ViewState["nowRow_Object"] == null ? "" : (String)ViewState["nowRow_Object"]); }
        }



        private bool bolGridAdd
        {
            set { ViewState["bolGridAdd"] = value; }
            get { return (ViewState["bolGridAdd"] == null ? true : (bool)ViewState["bolGridAdd"]); }
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
            //  this.Master.KeyFields = "FRC_CODE";
            

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
            string strSQL = "";
            DataTable dt;
            DataRow dr;
            SqlDataReader myReader;

            string strAplyNo = this.APLY_NO.Text.Trim();
            
           
            this.Object_DataProcess(false, strAplyNo);


            string strScript = "document.getElementById('trObj0').className='crow';";
            this.setScript(strScript);
        }
        #endregion





        protected void GridFunc_Click(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {

            DataTable dtGridSource;
            string strScript = "";
            string strSQL = "";
            DataView dvObj;
            string strID=((Button)sender).ID;
            string strAPLY = "";
            string strPERIOD = "";
            switch (strID)
            {
                case "btnAdd_Object":
                case "btnUpd_Object":
                    if (((Button)sender).ID == "btnUpd_Object")
                    {
                        this.nowRow_Object = (((RepeaterItem)((Button)sender).Parent).ItemIndex + 1).ToString();
                        strAPLY = e.CommandName.Split(',')[0].ToString();
                        strPERIOD = e.CommandName.Split(',')[1].ToString();
                        this.rptObjDetail.Items[0].FindControl("PERIOD_E").Editing(false);
                        this.setScript("FEE_Change();");

                    }
                    else
                    {
                        double intData = this.GridObject.Compute("max(PERIOD_E)", "").ToString().toNumber() + 1;
                        strPERIOD = intData.ToString();
                        GridObject.Select("APLY_NO=''")[0]["PERIOD_S"] = strPERIOD;

                        this.nowRow_Object = "0";
                    }
                    dvObj = this.GridObject.DefaultView;
                    dvObj.RowFilter = "APLY_NO<>''";

                    this.rptObjGrid.DataSource = dvObj;
                    this.rptObjGrid.DataBind();
                    this.upObjGrid.Update();

                    strScript = "document.getElementById('trObj" + this.nowRow_Object + "').className='crow';";
                    this.setScript(strScript);



                    // this.GridObject.DeleteRows("APLY_NO='" + strAPLY + "' and PERIOD_S="+ strPERIOD);

                    DataView dv = this.GridObject.DefaultView;
                    dv.RowFilter = "APLY_NO='" + strAPLY + "' and PERIOD_S=" + strPERIOD;
                    this.rptObjDetail.DataSource = dv;
                    this.rptObjDetail.DataBind();
                    this.upObjDetail.Update();

                    this.bolGridAdd = (strID == "btnAdd_Object");

                    break;
                case "btnEdit":

                    string strMessage = "";


                    if (this.rptObjDetail.Items[0].FindControl("PERIOD_E").value() == "")
                        strMessage += "[期數迄]";

                    if (strMessage != "")
                    {
                        if (!this.bolGridAdd)
                        {
                            this.setMessageBox(strMessage + "必須輸入！");
                            return;
                        }
                    }

                    if (this.rptObjDetail.Items[0].FindControl("PERIOD_E").value().CompareTo(this.rptObjDetail.Items[0].FindControl("PERIOD_S").value()) <= 0)
                    {
                        this.setMessageBox("[期數迄]必須大於[期數起]！");
                        return;
                    }

                    this.SaveObjectDetail(this.bolGridAdd);


                    strAPLY = e.CommandName.Split(',')[0].ToString();
                    
                        double intDatad = this.GridObject.Compute("max(PERIOD_E)","").ToString().toNumber() + 1;
                        strPERIOD=intDatad.ToString();
                        GridObject.Select("APLY_NO=''")[0]["PERIOD_S"] = strPERIOD;

                        this.nowRow_Object = "0";
                   
                    dvObj = this.GridObject.DefaultView;
                    dvObj.RowFilter = "APLY_NO<>''";

                    this.rptObjGrid.DataSource = dvObj;
                    this.rptObjGrid.DataBind();
                    this.upObjGrid.Update();

                    strScript = "document.getElementById('trObj0').className='crow';\n";
                    strScript += "FEE_Change();";
                    this.setScript(strScript);


                    

                    DataView dvD = this.GridObject.DefaultView;
                    dvD.RowFilter = "APLY_NO='' and PERIOD_S=" + strPERIOD;
                    this.rptObjDetail.DataSource = dvD;
                    this.rptObjDetail.DataBind();
                    this.upObjDetail.Update();

                    this.setProcessMessage("明細已暫存完成!!", false);
                    //this.bolGridAdd = (strID == "btnAdd_Object");
                                       

                    break;
                case "btnDel_Object":
                    strAPLY = e.CommandName.Split(',')[0].ToString();
                    strPERIOD = e.CommandName.Split(',')[1].ToString();

                    this.GridObject.DeleteRows("APLY_NO='" + strAPLY + "' and PERIOD_S="+ strPERIOD);

                    dvObj = this.GridObject.DefaultView;
                    dvObj.RowFilter = "APLY_NO<>''";

                    this.rptObjGrid.DataSource = dvObj;
                    this.rptObjGrid.DataBind();
                    this.upObjGrid.Update();
                    
                    break;

             



            }
        }


        private void SaveObjectDetail(bool bolAdd)
        {
            DataTable dt = this.GridObject;
            DataRow dr;

            string strAplyNo = this.APLY_NO.Text.Trim() ;
            string strPeriod = this.rptObjDetail.Items[0].FindControl("PERIOD_S").value();
            if (bolAdd)
                dr = dt.NewRow();
            else
            {
                if (this.GridObject.Select("APLY_NO='" + strAplyNo + "' and PERIOD_S="+strPeriod ).Length == 0)
                    return;

                dr = this.GridObject.Select("APLY_NO='" + strAplyNo + "' and PERIOD_S=" + strPeriod)[0];
            }



            dr["APLY_NO"] = strAplyNo;
            dr["PERIOD_S"] = strPeriod;
            dr["PERIOD_E"] = this.rptObjDetail.Items[0].FindControl("PERIOD_E").value();
            dr["BASE_RENT_FEE"] = this.rptObjDetail.Items[0].FindControl("BASE_RENT_FEE").value();
            dr["A4_MONO_MONTH_BASE_PAGE"] = this.rptObjDetail.Items[0].FindControl("A4_MONO_MONTH_BASE_PAGE").value();
            dr["A4_MONO_PAGE_FEE"] = this.rptObjDetail.Items[0].FindControl("A4_MONO_PAGE_FEE").value();            
            dr["A4_MONO_PAGE_FEE_FRC"] = this.rptObjDetail.Items[0].FindControl("A4_MONO_PAGE_FEE_FRC").value();                        
            dr["A4_COLOR_MONTH_BASE_PAGE"] = this.rptObjDetail.Items[0].FindControl("A4_COLOR_MONTH_BASE_PAGE").value();
            dr["A4_COLOR_PAGE_FEE"] = this.rptObjDetail.Items[0].FindControl("A4_COLOR_PAGE_FEE").value();            
            dr["A4_COLOR_PAGE_FEE_FRC"] = this.rptObjDetail.Items[0].FindControl("A4_COLOR_PAGE_FEE_FRC").value();                        
            dr["A3_MONO_MONTH_BASE_PAGE"] = this.rptObjDetail.Items[0].FindControl("A3_MONO_MONTH_BASE_PAGE").value();
            dr["A3_MONO_PAGE_FEE"] = this.rptObjDetail.Items[0].FindControl("A3_MONO_PAGE_FEE").value();            
            dr["A3_MONO_PAGE_FEE_FRC"] = this.rptObjDetail.Items[0].FindControl("A3_MONO_PAGE_FEE_FRC").value();                        
            dr["A3_COLOR_MONTH_BASE_PAGE"] = this.rptObjDetail.Items[0].FindControl("A3_COLOR_MONTH_BASE_PAGE").value();
            dr["A3_COLOR_PAGE_FEE"] = this.rptObjDetail.Items[0].FindControl("A3_COLOR_PAGE_FEE").value();            
            dr["A3_COLOR_PAGE_FEE_FRC"] = this.rptObjDetail.Items[0].FindControl("A3_COLOR_PAGE_FEE_FRC").value();

            if (bolAdd)
                dt.Rows.Add(dr);
            

        }

       

      


        private void setObject()
        {
            string strSQL = "";

            RepeaterItem rItem = this.rptObjDetail.Items[0];
            string APLY_NO = rItem.FindControl("OLD_APLY_NO").value();
            string OBJ_CODE = rItem.FindControl("OBJ_CODE").value();
            strSQL = "  select OBJ_LOC_ADDR,a.OTC,a.MAC_NO,a.BRAND,a.FRC_CODE,c.FRC_SNAME";
            strSQL += " from OR_OBJECT  a left join OR_CASE_APLY_OBJ b on a.OBJ_CODE=b.OBJ_CODE ";
            strSQL += "  left join OR_FRC c on a.FRC_CODE=c.FRC_CODE where  a.OBJ_CODE='" + OBJ_CODE + "'";

            string OBJ_LOC_ADDR = rItem.FindControl("OBJ_LOC_ADDR").ClientID;
            string OTC = rItem.FindControl("OTC").ClientID;
            string MAC_NO = rItem.FindControl("MAC_NO").ClientID;
            string BRAND = rItem.FindControl("BRAND").ClientID;
            string FRC_CODE = rItem.FindControl("FRC_CODE").ClientID;
            string FRC_SNAME = "FRC_SNAME";

            DataRow dr = dg.GetDataRow(strSQL);
            string strScript = "document.getElementById('" + OBJ_LOC_ADDR + "').value='" + dr["OBJ_LOC_ADDR"].ToString().Trim() + "';\n";
            strScript += "document.getElementById('" + OTC + "').checked=" + dr["OTC"].ToString().Trim() == "Y" ? "true" : "false" + ";\n";
            strScript += "document.getElementById('" + MAC_NO + "').value='" + dr["MAC_NO"].ToString().Trim() + "';\n";
            strScript += "document.getElementById('" + BRAND + "').value='" + dr["BRAND"].ToString().Trim() + "';\n";
            strScript += "document.getElementById('" + FRC_CODE + "').value='" + dr["FRC_CODE"].ToString().Trim() + "';\n";
            strScript += "document.getElementById('" + FRC_SNAME + "').value='" + dr["FRC_SNAME"].ToString().Trim() + "';\n";
            this.setScript(strScript);

        }
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
                case "Upd":
                case "Appove":
                    if (this.INC_TAX.SelectedValue=="")
                        strMessage += "[是否含稅]";
                                       
                    
                    if (strMessage != "")
                    {
                        this.setMessageBox(strMessage + "必須輸入！");
                        return false;
                    }


                  

                    
                    
                    break;


            }

            return true;
        }
        #endregion


        #region Object dataprocess 基本資料
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strType"></param>
        private void Object_DataProcess(bool bolSave, string strAplyNo)
        {
            string strSQL = "";
            SqlDataReader myReader = null;
            DataTable dt;
            DataRow dr;

            if (!bolSave)
            {


                strSQL += " select	APLY_NO,PERIOD_S,PERIOD_E,BASE_RENT_FEE,";
                strSQL += " A4_MONO_MONTH_BASE_PAGE,A4_MONO_PAGE_FEE,A4_MONO_PAGE_FEE_FRC,";                
                strSQL += " A4_COLOR_MONTH_BASE_PAGE,A4_COLOR_PAGE_FEE,A4_COLOR_PAGE_FEE_FRC,";
                strSQL += " A3_MONO_MONTH_BASE_PAGE,A3_MONO_PAGE_FEE,A3_MONO_PAGE_FEE_FRC,";
                strSQL += " A3_COLOR_MONTH_BASE_PAGE,A3_COLOR_PAGE_FEE,A3_COLOR_PAGE_FEE_FRC";                
                strSQL += " from OR3_PAPER_QUOTATION_VALUATION where APLY_NO='" + strAplyNo + "'";


                dt = dg.GetDataTable(strSQL);


                dr = dt.NewRow();
                dr["APLY_NO"] = "";
                double intData = dt.Compute("max(PERIOD_E)", "").ToString().toNumber() + 1;
                string strPERIOD = intData.ToString();
                dr["PERIOD_S"] = strPERIOD;
                dt.Rows.Add(dr);

                this.GridObject = dt;

                DataView dv = this.GridObject.DefaultView;
                dv.RowFilter = "APLY_NO<>''";
                this.rptObjGrid.DataSource = GridObject;
                this.rptObjGrid.DataBind();

                dv.RowFilter = "";
                dv = this.GridObject.DefaultView;
                dv.RowFilter = "APLY_NO=''";
                this.rptObjDetail.DataSource = dv;
                this.rptObjDetail.DataBind();



            }
            else
            {


                //this.GridObject.AcceptChanges();
                string strCodeKey = "";

                DataTable dtObj = dts.GetTable("OR3_PAPER_QUOTATION_VALUATION", "APLY_NO='" + strAplyNo + "'");
                DataTable dtCopy = dtObj.Copy();
                DataRow[] drCopy;
                dtObj.DeleteRows();
                for (int i = 0; i < this.GridObject.Rows.Count; i++)
                {
                    if (GridObject.Rows[i].RowState != DataRowState.Deleted)
                    {

                        string strAPLY_NO = GridObject.Rows[i]["APLY_NO"].ToString();
                        string strPeriod = GridObject.Rows[i]["PERIOD_S"].ToString();
                        if (strAPLY_NO != "")
                        {

                            drCopy = dtCopy.Select("APLY_NO='" + APLY_NO + "' and PERIOD_S=" + strPeriod + "");
                            dr = dtObj.NewRow();
                            

                            dr["APLY_NO"] = strAplyNo;
                            dr["PERIOD_S"] = strPeriod;
                            dr["PERIOD_E"] = GridObject.Rows[i]["PERIOD_E"].ToString();
                            dr["BASE_RENT_FEE"] = GridObject.Rows[i]["BASE_RENT_FEE"].ToString();
                            dr["A4_MONO_MONTH_BASE_PAGE"] = GridObject.Rows[i]["A4_MONO_MONTH_BASE_PAGE"].ToString();
                            dr["A4_MONO_PAGE_FEE"] = GridObject.Rows[i]["A4_MONO_PAGE_FEE"].ToString();
                            dr["A4_MONO_PAGE_FEE_FRC"] = GridObject.Rows[i]["A4_MONO_PAGE_FEE_FRC"].ToString();
                            dr["A4_COLOR_MONTH_BASE_PAGE"] = GridObject.Rows[i]["A4_COLOR_MONTH_BASE_PAGE"].ToString();
                            dr["A4_COLOR_PAGE_FEE"] = GridObject.Rows[i]["A4_COLOR_PAGE_FEE"].ToString();
                            dr["A4_COLOR_PAGE_FEE_FRC"] = GridObject.Rows[i]["A4_COLOR_PAGE_FEE_FRC"].ToString();
                            dr["A3_MONO_MONTH_BASE_PAGE"] = GridObject.Rows[i]["A3_MONO_MONTH_BASE_PAGE"].ToString();
                            dr["A3_MONO_PAGE_FEE"] = GridObject.Rows[i]["A3_MONO_PAGE_FEE"].ToString();
                            dr["A3_MONO_PAGE_FEE_FRC"] = GridObject.Rows[i]["A3_MONO_PAGE_FEE_FRC"].ToString();
                            dr["A3_COLOR_MONTH_BASE_PAGE"] = GridObject.Rows[i]["A3_COLOR_MONTH_BASE_PAGE"].ToString();
                            dr["A3_COLOR_PAGE_FEE"] = GridObject.Rows[i]["A3_COLOR_PAGE_FEE"].ToString();
                            dr["A3_COLOR_PAGE_FEE_FRC"] = GridObject.Rows[i]["A3_COLOR_PAGE_FEE_FRC"].ToString();

                            if (drCopy.Length > 0)
                            {
                                dr["ADD_USER_ID"] = drCopy[0]["ADD_USER_ID"].ToString();
                                dr["ADD_DATE"] = drCopy[0]["ADD_DATE"].ToString();
                                dr["ADD_TIME"] = drCopy[0]["ADD_TIME"].ToString();
                            }
                            else
                            {
                                dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                                dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                                dr["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                            }
                            dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                            dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                            dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");

                            dtObj.Rows.Add(dr);
                        }
                    }

                }



            }
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



            string strAplyNo = this.APLY_NO.Text.rpsText();

            DataTable dt = dts.GetTable("OR3_PAPER_QUOTATION", "APLY_NO='" + strAplyNo + "'");
            DataRow dr = null;

            switch (strStatus)
            {

                    
               
                case "Upd":
                    if (dt.Rows.Count == 0)
                    {
                        dr = dt.NewRow();
                        dr["APLY_NO"] = strAplyNo;
                        dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                    }
                    else
                        dr = dt.Rows[0];


                    dr["INC_TAX"] = this.INC_TAX.SelectedValue.Trim();
                    dr["CUST_NO"] = this.CUST_NO.Text.Trim();
                    dr["CTAC"] = this.CONTACT.Text.Trim();
                    dr["CTAC_TEL"] = this.CTAC_TEL.Text.Trim();
                    dr["FACSIMILE"] = this.FAX.Text.Trim();
                    dr["SUPL_CODE"] = this.FRC_CODE.Text.Trim();
                    dr["FRC_DEP_CODE"] = this.SALES_UNIT.Text.Trim();
                    dr["SALES_NAME"] = this.SALES_NAME.Text.Trim();
                    dr["SALES_TEL"] = this.SALES_TEL.Text.Trim();
                    dr["REMARK"] = this.REMARK.Text.Trim();
                    dr["COEFFICIENT"] = this.COEFFICIENT.Text.Trim();
                    dr["ESTIMATE"] = this.ESTIMATE.Text.Trim();
                    dr["XEROX_INPUT"] = this.XEROX_INPUT.Text.Trim();
                    dr["INCREASE"] = this.chkINCREASE.Checked ? "Y" : "N";
                    dr["EXCEED_FEE_A4_MONO"] = this.EXCEED_FEE_A4_MONO.Text.Trim();
                    dr["EXCEED_FEE_A4_COLOR"] = this.EXCEED_FEE_A4_COLOR.Text.Trim();
                    dr["EXCEED_FEE_A3_MONO"] = this.EXCEED_FEE_A3_MONO.Text.Trim();
                    dr["EXCEED_FEE_A3_COLOR"] = this.EXCEED_FEE_A3_COLOR.Text.Trim();
                    dr["MISPRINT_RATE"] = this.MISPRINT_RATE.Text.Trim();
                    if (!this.chkINCREASE.Checked)
                        dr["EXCEED_FEE_A3_COLOR"] = this.EXCEED_FEE_A3_COLOR.Text.Trim();
                    else
                        dr["EXCEED_FEE_A3_COLOR"] = this.INCREASE_FEE_A3_COLOR.Text.Trim().toNumber() + this.EXCEED_FEE_A3_MONO.Text.toNumber(); 

                    dr["NEW_CUST"] = "";
                    dr["PACKAGE"] = "";
                    dr["CASHIER"] = "";
                    dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                    dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                    dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                   
                   // this.SaveObjectDetail(this.bolGridAdd);                    
                    
                    if (dt.Rows.Count == 0)
                        dt.Rows.Add(dr);

                    this.Object_DataProcess(true, strAplyNo);

                    break;

            }

            // this.SaveGrid();
            if (dts.Save())
            {
                //  this.resetVerGrid();

                return 2;
            }
            else
                return 1;

            return 1;
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