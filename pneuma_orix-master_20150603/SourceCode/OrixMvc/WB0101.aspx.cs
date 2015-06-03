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
using System.Collections.Generic;
using Microsoft.VisualBasic;

namespace OrixMvc
{
    public partial class WB0101 : PageParent
    {


        public DataTable GridObject
        {
            set { Session["GridObject"] = value; }
            get { return (Session["GridObject"] == null ? null : (DataTable)Session["GridObject"]); }
        }

        public DataTable GridAccs
        {
            set { Session["GridAccs"] = value; }
            get { return (Session["GridAccs"] == null ? null : (DataTable)Session["GridAccs"]); }
        }

        public DataTable exGridAccs
        {
            set { Session["exGridAccs"] = value; }
            get { return (Session["exGridAccs"] == null ? null : (DataTable)Session["exGridAccs"]); }
        }

        public String nowRow_Object
        {
            set { Session["nowRow_Object"] = value; }
            get { return (Session["nowRow_Object"] == null ? "" : (String)Session["nowRow_Object"]); }
        }



        private bool bolGridAdd
        {
            set { ViewState["bolGridAdd"] = value; }
            get { return (ViewState["bolGridAdd"] == null ? true : (bool)ViewState["bolGridAdd"]); }
        }

        public String STSCODE
        {
            set { ViewState["STSCODE"] = value; }
            get { return (ViewState["STSCODE"] == null ? "" : (String)ViewState["STSCODE"]); }
        }


        public String STS
        {
            set { ViewState["STS"] = value; }
            get { return (ViewState["STS"] == null ? "" : (String)ViewState["STS"]); }
        }

        protected bool bolWE020
        {
            set { ViewState["bolWE020"] = value; }
            get { return (ViewState["bolWE020"] == null ? false : (bool)ViewState["bolWE020"]); }
        }

        protected bool bolWA070
        {
            set { ViewState["bolWA070"] = value; }
            get { return (ViewState["bolWA070"] == null ? false : (bool)ViewState["bolWA070"]); }
        }

        bool bolSave;
        DataTable dtSURPLUS;
        DataTable dtRef;

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

            if (Session["nowStatus"] != null && Session["bolWA070"] == null)
            {
                this.Master.Master.nowStatus = (String)Session["nowStatus"];
                this.Master.Master.nowStatusName = (String)Session["nowStatusName"];
            }
            Session["bolWA070"] = null;
            //***************************end 勿動****************************
        }
        #endregion


        protected void reload_data(object sender, EventArgs e)
        {
            this.txtOpin.Text = "";
            if (this.QUOTA_APLY_NO.Text == "")
                this.Master.Master.nowStatus = "Add";
            //else
            //    this.Master.Master.nowStatus = "Upd";
            if (this.CUR_STS.Text.Trim() != "")
            {
                string[] arySTS = this.CUR_STS.Text.Trim().Split(',');
                this.STSCODE = arySTS[0].ToString();
                this.CUR_STS.Text = arySTS[1].ToString();
            }
            else
            {
                this.CUR_STS.Text = "申請中";
            }
            this.setDefaultValue();
            this.checkObj.Text = "N";
            this.setScript("window.setTimeout('window.parent.closeLoading()', '500');");
            // this.setScript("init();");
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
            
            //1.程式編輯功能
            this.Master.setEditingFunction(false, true, false);

            this.Master.showSystemButton(SystemButton.btnSave.ToString(), false);
            this.Master.showSystemButton(SystemButton.btnCancel.ToString(), false);
            this.Master.showPanel(Area.DataArea.ToString(), false);
            */

            //2.取得前頁的session欄位


            if (this.Request.QueryString["sts"] == null)
            {

                //3.key fields
                //  this.Master.KeyFields = "FRC_CODE";
                if (Session["QUOTA_APLY_NO"] == null || Session["CUR_STS"] == null)
                {
                    this.Response.Redirect("WA060.aspx");
                }

                if (Session["bolWA070"] != null)
                {
                    this.Master.Master.nowStatus = "UpdAfter";
                    this.bolWA070 = true;
                    //Session["bolWA070"] = null;
                    this.status = this.Master.Master.nowStatus;
                }

                this.QUOTA_APLY_NO.Text = (string)Session["QUOTA_APLY_NO"];
                this.CUR_STS.Text = (string)Session["CUR_STS"];
                this.setDefaultValue();
            }
            else
            {
                this.QUOTA_APLY_NO.Text = "";
                this.CUR_STS.Text = "";
                this.setDefaultValue();


            }
        }
        #endregion


        public string status = "";
        public string empname = "";
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

            this.PARENT_COMP.Text = "";
            this.PARENT_COMP_SHAREHOLDING.Text = "";
            this.PARENT_COMP_INVEST_YEAR.Text = "";
            this.PARENT_COMP_INVEST_INCOME.Text = "";
            this.PREV_REAL_VAL.Text = "0";
            this.UPPER_LIMIT.Text = "0";


            string strAplyNo = this.QUOTA_APLY_NO.Text.Trim();

            this.tab01_DataProcess(false, strAplyNo);

            this.tab02_DataProcess(false, strAplyNo);

            this.tab10_DataProcess(false, strAplyNo);

            this.tab03_DataProcess(false, strAplyNo);

            this.tab04_DataProcess(false, strAplyNo);

            this.tab05_DataProcess(false, strAplyNo);

            this.tab06_DataProcess(false, strAplyNo);

            this.tab07_DataProcess(false, strAplyNo);

            this.tab08_DataProcess(false, strAplyNo);

            this.tab09_DataProcess(false, strAplyNo);


            this.status = this.Master.Master.nowStatus;
            this.empname = this.Master.Master.EmployeeName;
            this.Master.bolSave = true;
            switch (this.Master.Master.nowStatus)
            {
                case "Add":

                    ((CheckBox)this.rptObjDetail.Items[0].FindControl("OTC")).Checked = true;

                    this.CUR_STS.Text = "申請中";
                    this.rptRequest.Editing(true);
                    this.rptRun.Editing(false);

                    break;

                case "Copy":
                    this.QUOTA_APLY_NO.Text = "";
                    this.CUR_STS.Text = "申請中";

                    ((ocxControl.ocxDate)this.rptBase.Items[0].FindControl("APLY_DATE")).Text = System.DateTime.Now.ToString("yyyy/MM/dd");
                    break;

                case "Upd":
                    this.rptRequest.Editing(true);
                    this.rptRun.Editing(false);
                    if (this.STS == "1")
                        this.Master.bolSave = false;
                    break;

                case "UpdAfter":
                    this.rptRequest.Editing(false);
                    this.rptRun.Editing(true);
                    break;
            }


            this.btnPREV_REAL_VAL.Editing((this.STS == "0" || this.STS == "1" || this.STS == ""));
        }
        #endregion


        protected void AUD_Reload(object sender, EventArgs e)
        {
            this.setAUD(this.txtCaseType.Text);
        }


        private void setAUD(string strCaseType)
        {
            if (IsPostBack)
            {
                string strSQL = "exec s_WA060_Grid_AUD '" + this.QUOTA_APLY_NO.Text.Trim() + "','" + strCaseType + "'";
                SqlDataReader myReader = dg.openSqlReader(strSQL);
                //dt = dg.GetDataTable(strSQL);
                this.rptAud.DataSource = myReader;
                this.rptAud.DataBind();
                dg.closeSqlReader(myReader);
                this.upAud.Update();
            }
        }


        #region zipcode
        protected void SetAddress(object sender, EventArgs e)
        {
            DropDownList REQU_ZIP = (DropDownList)this.rptBase.Items[0].FindControl("REQU_ZIP");
            DropDownList CITY_CODE = (DropDownList)this.rptBase.Items[0].FindControl("CITY_CODE");
            TextBox REQ_PAY_ADDR = (TextBox)this.rptBase.Items[0].FindControl("REQ_PAY_ADDR");

            if (REQ_PAY_ADDR.Text == "" && CITY_CODE.SelectedValue != "" && REQU_ZIP.SelectedValue != "")
                REQ_PAY_ADDR.Text = CITY_CODE.SelectedItem.Text.Trim() + REQU_ZIP.SelectedItem.Text.Trim().Substring(0, REQU_ZIP.SelectedItem.Text.Trim().Length - 4);

        }





        #endregion


        private void setRETK()
        {
            string strFRC_CODE = this.rptObjDetail.Items[0].FindControl("FRC_CODE").value();
            if (strFRC_CODE == "")
                return;

            string strSQL = " select RETK_DURN_FR,RETK_DURN_TO,DUE_BUY_RATE from OR3_FRC_VER a left join OR_FRC_OBJ_BUY_SET b";
            strSQL += " on a.FRC_CODE=b.FRC_CODE and a.VER=b.VER and a.FRC_STS='1'";
            strSQL += " where a.FRC_CODE='" + strFRC_CODE + "'";
            this.rptRETK.DataSource = dg.GetDataTable(strSQL);
            this.rptRETK.DataBind();
            this.upRETK.Update();

        }

        protected void APPType_Change(object sender, EventArgs e)
        {
            if (((DropDownList)sender).ClientID.IndexOf("rptRequest") != -1)
                setPREV_APLY_NO("btnTab3");
            else
                setPREV_APLY_NO("btnTab");
        }

        private void setPREV_APLY_NO(string strType)
        {
            string strAPPTYPE = "";
            Repeater rpt = null;
            UpdatePanel up = null;
            if (strType == "btnTab3")
            {
                strAPPTYPE = this.rptRequest.Items[0].FindControl("APLY_APPR_TYPE").value();
                rpt = this.rptFORMER;
                up = this.upFORMER;
            }
            else
            {
                strAPPTYPE = this.rptRun.Items[0].FindControl("APRV_APPR_TYPE").value();
                rpt = this.rptFORMER1;
                up = this.upFORMER1;
            }

            //string strSQL = " select QAB.QUOTA_APLY_NO,QAC.APRV_TOT_QUOTA,chkSelect=(case when isnull(c.QUOTA_APLY_NO,'')='' then 'N' else 'Y' end)";
            string strSQL = " select QAB.QUOTA_APLY_NO as PREV_APLY_NO,QAC.APRV_TOT_QUOTA,chkSelect=(case when isnull(c.QUOTA_APLY_NO,'')='' then 'N' else 'Y' end)";
            strSQL += " from OR3_QUOTA_APLY_BASE as QAB inner join OR3_QUOTA_APLY_APRV_COND as QAC on QAB.QUOTA_APLY_NO=QAC.QUOTA_APLY_NO";
            strSQL += " left join OR3_QUOTA_CONTAIN_FORMER_" + (strType == "btnTab3" ? "APLY" : "APRV") + " c on QAB.QUOTA_APLY_NO=c.QUOTA_APLY_NO ";
            strSQL += " where CUR_STS='2' and DUE_DATE>=CONVERT(char(112),getdate(),111)";
            strSQL += " and CUST_NO='" + this.rptBase.Items[0].FindControl("CUST_NO").value() + "' and APRV_APPR_TYPE='" + strAPPTYPE + "'"; //畫面動撥型態 
            strSQL += " order by QAB.QUOTA_APLY_NO";
            DataTable dt = dg.GetDataTable(strSQL);

            rpt.DataSource = dt;
            rpt.DataBind();


        }
        protected void GridFunc_Click(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            if (this.bolWA070)
            {
                this.Master.Master.nowStatus = "UpdAfter";
            }


            DataTable dtGridSource;
            string strScript = "";
            string strSQL = "";
            DataTable dt;
            string strMessage = "";
            DataView dv;
            DataRow[] adr;

            DataView dvObj;


            string strID = ((Button)sender).ID;
            switch (strID)
            {


                case "btnTab3":
                case "btnTab5":

                    setPREV_APLY_NO(strID);

                    break;

                case "btnDel_Object":

                    this.GridAccs.DeleteRows("OBJ_KEY='" + e.CommandName + "'");
                    this.GridObject.DeleteRows("OBJ_KEY='" + e.CommandName + "'");
                    DataView dvObjD = this.GridObject.DefaultView;
                    dvObjD.RowFilter = "OBJ_KEY<>''";
                    this.rptObjGrid.DataSource = dvObjD;
                    this.rptObjGrid.DataBind();
                    this.upObjGrid.Update();

                    break;

                case "btnEdit":


                    if (this.rptObjDetail.Items[0].FindControl("PROD_NAME").value() == "")
                        strMessage += "[品名]";


                    if (strMessage != "")
                    {
                        this.setMessageBox(strMessage + "必須輸入！");
                        return;
                    }

                    this.SaveObjectDetail(this.bolGridAdd);

                    strScript = "document.getElementById('trObj" + this.nowRow_Object + "').className='';";
                    this.nowRow_Object = "0";


                    strScript += "document.getElementById('trObj" + this.nowRow_Object + "').className='crow';";
                    this.setScript(strScript);

                    dv = this.GridObject.DefaultView;
                    dv.RowFilter = "";

                    dv.RowFilter = "OBJ_KEY=''";
                    this.rptObjDetail.DataSource = dv;
                    this.rptObjDetail.DataBind();
                    this.upObjDetail.Update();

                    this.exGridAccs = null;
                    this.exGridAccs = this.GridAccs.Clone();
                    adr = this.GridAccs.Select("OBJ_KEY=''");
                    for (int i = 0; i < adr.Length; i++)
                    {
                        this.exGridAccs.ImportRow(adr[i]);
                    }


                    if (this.exGridAccs != null)
                    {
                        dv = this.exGridAccs.DefaultView;
                        dv.RowFilter = "OBJ_KEY=''";
                        this.rptAccs.DataSource = dv;
                        this.rptAccs.DataBind();
                        this.addACCS_NAME.Text = "";
                        this.upAccs.Update();
                    }

                    dvObj = this.GridObject.DefaultView;
                    dvObj.RowFilter = "";
                    dvObj.RowFilter = "OBJ_KEY<>''";

                    this.rptObjGrid.DataSource = dvObj;
                    this.rptObjGrid.DataBind();
                    this.upObjGrid.Update();

                    if (this.bolGridAdd)
                        this.GridFunc_Click(btnAdd_Object, null);

                    this.setProcessMessage("明細已暫存完成!!", false);
                    break;
                case "btnMobile":
                    this.setAGENT();
                    break;

                case "btnAdd_Object":
                case "btnUpd_Object":




                    //   SALES_RGT_ADDR    OBJ_LOC_ADDR

                    if (((Button)sender).ID == "btnUpd_Object")
                        this.nowRow_Object = (((RepeaterItem)((Button)sender).Parent).ItemIndex + 1).ToString();

                    else
                        this.nowRow_Object = "0";

                    dvObj = this.GridObject.DefaultView;
                    dvObj.RowFilter = "";
                    dvObj.RowFilter = "OBJ_KEY<>''";

                    this.rptObjGrid.DataSource = dvObj;
                    this.rptObjGrid.DataBind();
                    this.upObjGrid.Update();

                    strScript = "document.getElementById('trObj" + this.nowRow_Object + "').className='crow';";
                    this.setScript(strScript);
                    //2014/6/29
                    string kcode = "";

                    if (e != null)
                        kcode = e.CommandName;
                    if (((Button)sender).ID == "btnAdd_Object" && this.GridObject.Rows.Count > 1)
                    {

                        DataRow[] aadr = this.GridObject.Select("OBJ_KEY<>''", "OBJ_KEY");
                        kcode = aadr[aadr.Length - 1]["OBJ_KEY"].ToString();
                    }
                    dv = this.GridObject.DefaultView;
                    dv.RowFilter = "";
                    dv.RowFilter = "OBJ_KEY='" + kcode + "'";

                    this.rptObjDetail.DataSource = dv;
                    this.rptObjDetail.DataBind();
                    if (strID == "btnAdd_Object" && this.GridObject.Rows.Count == 1)
                    {
                        ((TextBox)this.rptObjDetail.Items[0].FindControl("OBJ_LOC_ADDR")).Text = this.rptBaseCustom.Items[0].FindControl("SALES_RGT_ADDR").value();
                        ((CheckBox)this.rptObjDetail.Items[0].FindControl("otc")).Checked = true;
                        ((RadioButtonList)this.rptObjDetail.Items[0].FindControl("Is_spec_repo")).SelectedValue = "Y";
                    }
                    if (strID == "btnAdd_Object")
                    {
                        ((TextBox)this.rptObjDetail.Items[0].FindControl("OBJ_CODE")).Text = "";
                        ((TextBox)this.rptObjDetail.Items[0].FindControl("MAC_NO")).Text = "";
                    }
                    this.upObjDetail.Update();
                    setRETK();

                    if (strID == "btnAdd_Object")
                        kcode = "";
                    if (((Button)sender).ID == "btnAdd_Object" && this.GridObject.Rows.Count > 1)
                    {

                        DataRow[] aadr = this.GridObject.Select("OBJ_KEY<>''", "OBJ_KEY");
                        kcode = aadr[aadr.Length - 1]["OBJ_KEY"].ToString();
                    }
                    this.exGridAccs = null;
                    this.exGridAccs = this.GridAccs.Clone();
                    adr = this.GridAccs.Select("OBJ_KEY='" + kcode + "'");
                    for (int i = 0; i < adr.Length; i++)
                    {
                        this.exGridAccs.ImportRow(adr[i]);
                    }


                    if (this.exGridAccs != null)
                    {
                        dv = this.exGridAccs.DefaultView;
                        dv.RowFilter = "OBJ_KEY='" + kcode + "'";
                        this.rptAccs.DataSource = dv;
                        this.rptAccs.DataBind();
                        this.addACCS_NAME.Text = "";
                        this.upAccs.Update();
                    }

                    this.bolGridAdd = (strID == "btnAdd_Object");

                    break;

                case "btnAdd_Accs":
                    this.SaveAccs(true, "0");
                    dv = this.exGridAccs.DefaultView;
                    dv.RowFilter = "";
                    this.rptAccs.DataSource = dv;

                    this.rptAccs.DataBind();
                    this.upAccs.Update();
                    break;

                case "btnUpd_Accs":
                    this.SaveAccs(false, e.CommandName);
                    this.rptAccs.DataSource = this.exGridAccs;
                    this.rptAccs.DataBind();
                    this.upAccs.Update();
                    break;

                case "btnDel_Accs":
                    // this.SaveAccs(false);
                    this.exGridAccs.DeleteRows("ACCS_SEQ='" + e.CommandName + "'");

                    this.rptAccs.DataSource = this.exGridAccs;
                    this.rptAccs.DataBind();
                    this.upAccs.Update();
                    dv = this.GridObject.DefaultView;
                    dv.RowFilter = "";
                    dv.RowFilter = "OBJ_KEY=''";
                    this.rptObjDetail.DataSource = dv;
                    this.rptObjDetail.DataBind();
                    this.upObjDetail.Update();

                    break;




                case "btnSet_Ref":

                    /*  ALTER PROCEDURE [dbo].[s_WA060_PermisCalc]			
  (	@PCur_STS varchar(5),	目前狀態	
      @PCAUD_CASE_TYPE varchar(5),	審查案件類別	型態標的物
      @PDEPT_CODE varchar(10),	部門	基本資料
      @PAll_TOT money,	契約餘額合計	往來實績
      @PAPLY_REAL_TR money,	申請條件實質TR	申請條件
      @PAPLY_PERD decimal(3,0),	期數	申請條件
      @PAPLY_DATE varchar(10),	申請日期	基本資料
      @PBUILD_DATE varchar(10),	客戶設立日期	型態標的物
      @PSET_CHECK varchar(100)	自訂權限 0/1 exp:'000110101'	權限條件
                      */
                    if (this.Master.Master.nowStatus == "UpdAfter")
                        break;
                    dt = new DataTable("Ref");
                    for (int i = 1; i <= 16; i++)
                    {
                        dt.Columns.Add("S" + i.ToString());
                        dt.Columns.Add("SN" + i.ToString());
                    }

                    strSQL = "exec s_WA060_PermisCalc ";
                    strSQL += " @PCur_STS='" + this.STS + "'";
                    string DEPT_CODE = this.rptBase.Items[0].FindControl("DEPT_CODE").value();
                    strSQL += " ,@PDEPT_CODE='" + DEPT_CODE + "'";
                    strSQL += " ,@PCAUD_CASE_TYPE='" + this.txtCaseType.Text.Trim() + "'";

                    string PALL_TOT = this.rptContact.Items[0].FindControl("All_TOT").value().toNumber().ToString();
                    strSQL += " ,@PAll_TOT=" + PALL_TOT + "";

                    string APLY_REAL_TR = this.rptRequest.Items[0].FindControl("APLY_REAL_TR").value();
                    strSQL += " ,@PAPLY_REAL_TR=" + APLY_REAL_TR + "";

                    string iAPLY_PERD = this.rptRequest.Items[0].FindControl("APLY_DURN_M").value();
                    strSQL += " ,@PAPLY_PERD=" + iAPLY_PERD + "";

                    string APLY_DATE = this.rptBase.Items[0].FindControl("APLY_DATE").value();
                    strSQL += " ,@PAPLY_DATE='" + APLY_DATE + "'";

                    string BUILD_DATE = this.rptBaseCustom.Items[0].FindControl("BUILD_DATE").value();
                    strSQL += " ,@PBUILD_DATE='" + BUILD_DATE + "'";

                    string strCheck = "";
                    for (int i = 0; i < this.rptAud.Items.Count; i++)
                    {
                        strCheck += (((CheckBox)this.rptAud.Items[i].FindControl("SEL")).Checked ? "1" : "0");
                    }
                    strSQL += " ,@PSET_CHECK='" + strCheck + "'";
                    DataTable tbRef = dg.GetDataTable(strSQL);
                    if (tbRef.Columns[0].ColumnName == "ErrMsg")
                    {
                        this.setMessageBox(tbRef.Rows[0][0].ToString());
                        return;
                    }

                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);

                    for (int i = 0; i < tbRef.Rows.Count; i++)
                    {
                        dr["s" + tbRef.Rows[i]["seq"].ToString()] = tbRef.Rows[i]["AUD_LVL_CODE"].ToString();
                        dr["sn" + tbRef.Rows[i]["seq"].ToString()] = tbRef.Rows[i]["AUD_LVL_NAME"].ToString();
                    }


                    this.rptRef.DataSource = dt;
                    this.rptRef.DataBind();
                    this.upRef.Update();
                    this.dtRef = dt;
                    if (bolSave && (this.STS == "" || this.STS == "0"))
                        return;

                    if (tbRef.Select("SEQ=16").Length > 0)
                    {

                        strSQL = "exec s_WA060_Grid_AUD_List ";
                        strSQL += " @PCAUD_CASE_TYPE='" + this.txtCaseType.Text.Trim() + "'";
                        strSQL += " ,@PDEPT_CODE='" + DEPT_CODE + "'";
                        dr = tbRef.Select("SEQ=16")[0];
                        strSQL += " ,@PAUD_LVL_CODE='" + dr["AUD_LVL_CODE"].ToString() + "'";
                        strSQL += " ,@PAPLY_OR_AUD=" + (this.STS.Trim() == "" || this.STS.Trim() == "A" ? "1" : "2") + "";
                        strSQL += " ,@PUserID='" + this.Master.Master.UserId + "'";
                        strSQL += " ,@PUserName='" + this.Master.Master.EmployeeName + "'";
                        DataTable tbOpin = dg.GetDataTable(strSQL);

                        this.rptOpin.DataSource = tbOpin;
                        this.rptOpin.DataBind();
                        this.upOpin.Update();

                    }



                    break;
                case "btnSURPLUS":

                    //s_WA060_RealCalc
                    /*
                     * @PCust_No varchar(10),			--客戶代號
	@PMthd varchar(1),				--方法:(1)一般案件申請; (2)額度申請 "1"
	@PAply_No varchar(14)='',		--申請書編號(一般案件申請)
	@PQuota_Aply_No varchar(15)='',	--額度申請書編號(額度申請)
	@PCur_Quota_Aply_No varchar(15)='',	--目前使用的額度申請書編號(一般案件申請) //CUR_QUOTA_APLY_NO
	@P收入總額 money=0,				--收入總額(一般案件申請)  APLY_INCM_TOL
	@P頭期款 money=0,				--頭期款(一般案件申請) APRV_DEPS
	@P額度申請總額 money=0				--額度申請總額(額度申請)
                     * 
                     * 
                     * */
                    strSQL = "exec s_WA060_RealCalc";
                    strSQL += " @PCust_No='" + this.rptBase.Items[0].FindControl("CUST_NO").value() + "',";
                    strSQL += " @PMthd='2',";
                    strSQL += " @PQuota_Aply_No='" + this.QUOTA_APLY_NO.Text.Trim() + "',";
                    strSQL += " @P額度申請總額=" + this.rptRequest.Items[0].FindControl("APLY_TOT_QUOTA").value() + "";


                    dt = dg.GetDataTable(strSQL);
                    this.dtSURPLUS = dt;
                    this.rptContact.DataSource = dt;
                    this.rptContact.DataBind();
                    this.upContact.Update();

                    break;

                case "btnPREV_REAL_VAL":
                    strSQL = " select qu.cust_no,sum(REAL_VAL)  as REAL_VAL ";
                    strSQL += "from OR3_QUOTA_APLY_USER QU ";
                    strSQL += "inner join or_case_aply_base B on B.CUR_QUOTA_APLY_NO = QU.QUOTA_APLY_NO ";
                    strSQL += "left join OR_CON_VAL_BAL V on V.aply_no=B.aply_no ";
                    strSQL += "where QU.QUOTA_APLY_NO in (畫面所挑之舊額度) ";
                    strSQL += "group by cust_no ";
                    break;

            }
        }

        private void setAGENT()
        {
            /* 找 OR3_FRC_SALES.FRC_SALES_NAME=介紹人姓名 符合之資料
 將 OR3_FRC_SALES.MOBILE 帶至右方欄位:其他
 此介紹人之供應商&營業單位名稱 為 新增第一筆標的物之預設值*/
            string strSQL = "select MOBILE from OR3_FRC_SALES where OR3_FRC_SALES.FRC_SALES_NAME='" + this.rptObjMain.Items[0].FindControl("AGENT").value() + "'";
            this.setScript("document.getElementById('" + this.rptObjMain.Items[0].FindControl("OTHER").ClientID + "').value='" + dg.GetDataRow(strSQL)[0].ToString().Trim() + "';");

        }

        private void SaveObjectDetail(bool bolAdd)
        {

            DataTable dt = this.GridObject;
            DataRow dr;

            string strCode = this.rptObjDetail.Items[0].FindControl("OBJ_CODE").value();
            string strKey = this.rptObjDetail.Items[0].FindControl("OBJ_KEY").value();

            if (bolAdd)
            {
                dr = dt.NewRow();
                strKey = "ZZZ" + DateTime.Now.ToString("mmssfff");
            }
            else
            {
                if (this.GridObject.Select("OBJ_KEY='" + strKey + "'").Length == 0)
                    return;

                dr = this.GridObject.Select("OBJ_KEY='" + strKey + "'")[0];
            }

            dr["OBJ_KEY"] = strKey;
            dr["OBJ_CODE"] = strCode;
            dr["OBJ_LOC_ADDR"] = this.rptObjDetail.Items[0].FindControl("OBJ_LOC_ADDR").value();
            dr["PROD_NAME"] = this.rptObjDetail.Items[0].FindControl("PROD_NAME").value();
            dr["OTC"] = this.rptObjDetail.Items[0].FindControl("OTC").value();
            dr["SPEC"] = this.rptObjDetail.Items[0].FindControl("SPEC").value();
            dr["BRAND"] = this.rptObjDetail.Items[0].FindControl("BRAND").value();
            dr["OBJ_DUE_OWNER"] = this.rptObjDetail.Items[0].FindControl("OBJ_DUE_OWNER").value();
            dr["MAC_NO"] = this.rptObjDetail.Items[0].FindControl("MAC_NO").value();
            dr["YEAR"] = this.rptObjDetail.Items[0].FindControl("YEAR").value();
            dr["CAR_NO"] = this.rptObjDetail.Items[0].FindControl("CAR_NO").value();
            dr["REAL_BUY_PRC"] = this.rptObjDetail.Items[0].FindControl("REAL_BUY_PRC").value();
            dr["BUDG_LEASE_AMT"] = this.rptObjDetail.Items[0].FindControl("BUDG_LEASE_AMT").value();
            dr["SELF_RATE"] = this.rptObjDetail.Items[0].FindControl("SELF_RATE").value();
            dr["BUDG_LEASE"] = this.rptObjDetail.Items[0].FindControl("BUDG_LEASE").value();
            dr["BUY_RATE"] = this.rptObjDetail.Items[0].FindControl("BUY_RATE").value();
            dr["OBJ_ASUR_TYPE"] = this.rptObjDetail.Items[0].FindControl("OBJ_ASUR_TYPE").value();
            dr["INV_AMT_I_IB"] = this.rptObjDetail.Items[0].FindControl("INV_AMT_I_IB").value();
            dr["RV_AMT"] = this.rptObjDetail.Items[0].FindControl("RV_AMT").value();
            dr["OBJ_LOC_TEL"] = this.rptObjDetail.Items[0].FindControl("OBJ_LOC_TEL").value();
            dr["OBJ_LOC_FAX"] = this.rptObjDetail.Items[0].FindControl("OBJ_LOC_FAX").value();
            dr["OBJ_LOC_CTAC"] = this.rptObjDetail.Items[0].FindControl("OBJ_LOC_CTAC").value();
            dr["BUY_WAY"] = this.rptObjDetail.Items[0].FindControl("BUY_WAY").value();
            dr["BUY_PROMISE"] = this.rptObjDetail.Items[0].FindControl("BUY_PROMISE").value();
            dr["FRC_CODE"] = this.rptObjDetail.Items[0].FindControl("FRC_CODE").value();
            dr["FRC_SNAME"] = base.Request.Form["FRC_SNAME"].ToString();
            dr["SALES_UNIT"] = this.rptObjDetail.Items[0].FindControl("SALES_UNIT").value();
            dr["OBJ_ONUS"] = this.rptObjDetail.Items[0].FindControl("OBJ_ONUS").value();


            if (bolAdd)
            {
                dt.Rows.Add(dr);

            }

            this.GridAccs.DeleteRows("OBJ_KEY='" + strKey + "'");
            dt = this.GridAccs;
            int iC = 1;
            for (int i = 0; i < this.rptAccs.Items.Count; i++)
            {
                string strNAME = this.rptAccs.Items[i].FindControl("ACCS_NAME").value();
                if (strNAME != "")
                {
                    dr = dt.NewRow();
                    dr["OBJ_KEY"] = strKey;
                    dr["OBJ_CODE"] = strCode;
                    dr["ACCS_SEQ"] = iC;
                    dr["ACCS_NAME"] = strNAME;
                    dt.Rows.Add(dr);
                    iC++;
                }
            }
        }

        private void SaveAccs(bool bolAdd, string strSEQ)
        {
            string strKey = this.rptObjDetail.Items[0].FindControl("OBJ_KEY").value();
            string strCode = this.rptObjDetail.Items[0].FindControl("OBJ_CODE").value();

            DataTable dd;
            dd = this.GridAccs.Clone();
            if (this.exGridAccs == null)
            {
                this.exGridAccs = this.GridAccs.Copy();
            }
            // this.exGridAccs.DeleteRows("OBJ_CODE='" + strCode + "'");

            DataTable dt = this.exGridAccs;
            DataRow dr;
            int iC = 1;


            if (bolAdd)
            {
                dr = dt.NewRow();
                dr["ACCS_SEQ"] = dt.Rows.Count + 1;
            }
            else
                dr = this.exGridAccs.Select("ACCS_SEQ=" + strSEQ + "")[0];


            if (this.addACCS_NAME.Text.Trim() == "")
            {
                this.setMessageBox("配件名稱必須輸入！");
                return;
            }


            dr["OBJ_KEY"] = strKey;
            dr["OBJ_CODE"] = strCode;
            dr["ACCS_NAME"] = this.addACCS_NAME.Text.Trim();

            this.addACCS_NAME.Clearing();

            if (bolAdd)
                dt.Rows.Add(dr);


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
                case "UpdAfter":

                    if (strStatus == "Add" || strStatus == "Upd")
                    {
                        if (STSCODE != "R" && STSCODE.CompareTo("1") > 0)
                        {
                            this.setMessageBox("申請書已核准過不可更改!");
                            return false;
                        }
                    }
                    if (strStatus == "UpdAfter")
                    {
                        if (STSCODE != "R" && STSCODE.CompareTo("2") > 0)
                        {
                            this.setMessageBox("申請書已核准過不可更改!");
                            return false;
                        }
                    }

                    if (this.rptBase.Items[0].FindControl("APLY_DATE").value() == "")
                        strMessage += "[申請日期]";

                    if (this.rptBase.Items[0].FindControl("DEPT_CODE").value() == "")
                        strMessage += "[申請單位]";

                    if (this.rptBase.Items[0].FindControl("EMP_CODE").value() == "")
                        strMessage += "[經辦代號]";

                    if (this.rptBase.Items[0].FindControl("CUST_NO").value() == "")
                        strMessage += "[客戶代號]";




                    if (this.rptBase.Items[0].FindControl("CONTACT").value() == "")
                        strMessage += "[連絡人]";

                    if (this.rptBase.Items[0].FindControl("CTAC_TEL").value() == "")
                        strMessage += "[電話]";




                    if (strMessage != "")
                    {
                        this.setMessageBox(strMessage + "必須輸入！");
                        return false;
                    }


                    if (this.rptObjMain.Items[0].FindControl("AUD_CASE_TYPE").value() == "")
                        strMessage += "[審查案件類別]";


                    if (strMessage != "")
                    {
                        this.setMessageBox(strMessage + "必須輸入！");
                        return false;
                    }

                    bool chk = false;
                    for (int i = 0; i < this.rptCASETYPE.Items.Count; i++)
                    {
                        if (this.rptCASETYPE.Items[i].FindControl("CASE_TYPE_CODE").value() == "Y")
                        {
                            chk = true;
                            break;
                        }
                    }

                    if (!chk)
                    {
                        this.setMessageBox("案件類別至少需勾選一筆！");
                        return false;
                    }



                    if (this.rptObjMain.Items[0].FindControl("AUD_CASE_TYPE").value() == "5")
                    {
                        if (rptBaseCustom.Items[0].FindControl("BUILD_DATE").value() == "")
                        {
                            this.setMessageBox("公司設立日期未建！");
                            return false;
                        }
                    }





                    if (strStatus == "Add" || strStatus == "Copy" || strStatus == "Upd")
                    {

                        if (this.rptRequest.Items[0].FindControl("APLY_TOT_QUOTA").value() == "0")
                            strMessage += "[申請條件:總額度]";

                        if (this.rptRequest.Items[0].FindControl("APLY_APPR_TYPE").value() == "")
                            strMessage += "[申請條件:動撥型態]";


                        if (this.rptRequest.Items[0].FindControl("APLY_QUOTA_MONTHS").value() == "0")
                            strMessage += "[申請條件:專案額度期間(月)]";

                        if (this.rptRequest.Items[0].FindControl("APLY_DURN_M").value() == "0")
                            strMessage += "[申請條件:逐筆動撥期間(最長月數)]";

                        if (this.rptRequest.Items[0].FindControl("APLY_PAY_PERD").value() == "0")
                            strMessage += "[申請條件:付款週期(月)]";


                        if (this.rptRequest.Items[0].FindControl("APLY_REAL_TR").value() == "0")
                            strMessage += "[申請條件:實值TR]";



                    }

                    if (strStatus == "UpdAfter")
                    {
                        if (this.rptRun.Items[0].FindControl("APRV_TOT_QUOTA").value() == "0")
                            strMessage += "[實行條件:總額度]";

                        if (this.rptRun.Items[0].FindControl("APRV_APPR_TYPE").value() == "")
                            strMessage += "[實行條件:動撥型態]";

                        if (this.rptRun.Items[0].FindControl("APRV_QUOTA_MONTHS").value() == "0")
                            strMessage += "[實行條件:專案額度期間(月)]";

                        if (this.rptRun.Items[0].FindControl("APRV_DURN_M").value() == "0")
                            strMessage += "[實行條件:逐筆動撥期間(最長月數)]";

                        if (this.rptRun.Items[0].FindControl("APRV_PAY_PERD").value() == "0")
                            strMessage += "[實行條件:付款週期(月)]";

                        if (this.rptRun.Items[0].FindControl("APRV_REAL_TR").value() == "0")
                            strMessage += "[實行條件:實值TR]";



                    }

                    if (strMessage != "")
                    {
                        this.setMessageBox(strMessage + "必須輸入！");
                        return false;
                    }

                    if (this.rptScur.Items.Count == 0)
                    {
                        this.setMessageBox("保證人請至少輸入一筆資料!");
                        return false;
                    }


                    if (this.rptASUR.Items.Count == 0)
                    {
                        this.setMessageBox("保險請至少輸入一筆資料!");
                        return false;
                    }

                    int intSel = 0;

                    for (int i = 0; i < this.rptAud.Items.Count; i++)
                    {
                        if (((CheckBox)this.rptAud.Items[i].FindControl("Sel")).Checked)
                            intSel++;
                    }
                    if (intSel == 0)
                    {
                        this.setMessageBox("條件權限尚未勾選!");
                        return false;
                    }

                    if (intSel > 1)
                    {
                        this.setMessageBox("條件權限只能勾選一筆!");
                        return false;
                    }

                    // if (this.rptObjGrid.Items.Count == 0)
                    // {
                    //    this.setMessageBox("標的物至少要有一筆資料!");
                    //    return false;
                    // }
                    break;

                case "Del":
                    break;
            }


            return true;
        }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="E"></param>
        protected void Reload_Custom(object sender, System.EventArgs E)
        {
            this.setCUSTOM();

        }

        private void setCUSTOM()
        {
            //binding rptBaseCustom
            string strCustom = this.rptBase.Items[0].FindControl("CUST_NO").value();
            string strSQL = " select  TAKER,BUILD_DATE=dbo.f_DateAddSlash(BUILD_DATE),BACKGROUND,SALES_RGT_ADDR,";
            strSQL += " CAPT_STR=dbo.f_ConditionGetDesc('CAPT_STR',CAPT_STR,'N'),";
            strSQL += " ORG_TYPE=dbo.f_ConditionGetDesc('ORG_TYPE',ORG_TYPE,'N'),";
            strSQL += " RGT_CAPT_AMT,EMP_PSNS,REAL_CAPT_AMT,MAIN_BUS_ITEM,";
            strSQL += " CREDIT_CUST,JUDGE_LVL,";
            strSQL += " HONEST_AGREEMENT,SECRET_PROMISE,CUST_SNAME";
            strSQL += " from OR_CUSTOM where CUST_NO='" + strCustom + "' ";
            SqlDataReader myReader = dg.openSqlReader(strSQL);
            DataTable dt = new DataTable();
            dt.Load(myReader);
            dg.closeSqlReader(myReader);
            DataRow dr;
            if (dt.Rows.Count == 0)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }

            this.rptBaseCustom.DataSource = dt;
            this.rptBaseCustom.DataBind();
            this.upCustomBase.Update();
            if (dt.Rows.Count > 0)
            {
                dr = dt.Rows[0];
                string strScript = "setCustom('" + strCustom + "', '" + dr["CUST_SNAME"].ToString().Trim() + "', '" + dr["REAL_CAPT_AMT"].ToString().toNumber().ToString("###,###,###,##0") + "');";
                //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setCustom", strScript, true);
                this.setScript(strScript);
            }
        }

        #region tab01 dataprocess 基本資料
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strType"></param>
        private void tab01_DataProcess(bool bolSave, string strAplyNo)
        {
            string strSQL = "";
            SqlDataReader myReader = null;
            DataTable dt;
            DataRow dr;

            if (!bolSave)
            {
                //binding rptBase
                strSQL = " select	a.CUR_STS,APLY_DATE=dbo.f_DateAddSlash(a.APLY_DATE), ";
                strSQL += " a.DEPT_CODE,b.DEPT_NAME,EMP_CODE=dbo.f_EmpIDToCorpAcct(a.EMP_CODE) ,c.EMP_NAME, ";
                strSQL += " a.CUST_NO,d.CUST_SNAME,d.CUST_EMAIL_ADDR,InitContactDate=dbo.f_DateAddSlash(InitContactDate),  ";
                strSQL += " PROJECT=(case when d.GEN_CURR_QUOTA+d.VP_CURR_QUOTA+d.AR_CURR_QUOTA>0 then '是額度客戶' else '' end), ";
                strSQL += " a.CONTACT,a.CTAC_TEL,DUE_DATE=dbo.f_DateAddSlash(a.DUE_DATE),APRV_DATE=dbo.f_DateAddSlash(a.APRV_DATE),a.BIZ_PROMO_PROC,a.SupplierBackground,a.MainCondition ";
                strSQL += " from	OR3_QUOTA_APLY_BASE a left join OR_DEPT b on a.DEPT_CODE=b.DEPT_CODE ";
                strSQL += " left join V_OR_EMP c on a.EMP_CODE=c.EMP_CODE ";
                strSQL += " left join OR_CUSTOM d on a.CUST_NO=d.CUST_NO ";
                strSQL += " where a.QUOTA_APLY_NO='" + strAplyNo + "'";
                dt = dg.GetDataTable(strSQL);

                if (dt.Rows.Count == 0)
                {
                    dr = dt.NewRow();
                    dr["APLY_DATE"] = System.DateTime.Now.ToString("yyyy/MM/dd");
                    dr["CUR_STS"] = "";
                    dt.Rows.Add(dr);
                }
                if (this.Master.Master.nowStatus == "Copy")
                {
                    dt.Rows[0]["APLY_DATE"] = System.DateTime.Now.ToString("yyyy/MM/dd");
                    dt.Rows[0]["CUR_STS"] = "";
                }
                this.rptBase.DataSource = dt;
                this.rptBase.DataBind();
                this.STS = dt.Rows[0]["CUR_STS"].ToString();




                //binding rptBaseCustom
                this.setCUSTOM();
            }
            else
            {
                dt = dts.GetTable("OR3_QUOTA_APLY_BASE", "QUOTA_APLY_NO='" + strAplyNo + "'");
                if (dt.Rows.Count == 0)
                {

                    dr = dt.NewRow();
                    dr["QUOTA_APLY_NO"] = strAplyNo;
                    dr["CUR_STS"] = "0";
                    dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                    dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                    dr["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                }
                else
                    dr = dt.Rows[0];

                //   dr["PAPER"] = this.rptCon.Items[0].FindControl("PAPER").value();
                dr["APLY_DATE"] = this.rptBase.Items[0].FindControl("APLY_DATE").value().Replace("/", "");
                dr["DEPT_CODE"] = this.rptBase.Items[0].FindControl("DEPT_CODE").value();
                dr["EMP_CODE"] = dg.GetDataRow("select dbo.f_CorpAcctToEmpID('" + this.rptBase.Items[0].FindControl("EMP_CODE").value() + "')")[0].ToString();
                dr["CUST_NO"] = this.rptBase.Items[0].FindControl("CUST_NO").value();
                dr["InitContactDate"] = this.rptBase.Items[0].FindControl("InitContactDate").value().Replace("/", "");


                dr["CONTACT"] = this.rptBase.Items[0].FindControl("CONTACT").value();
                dr["CTAC_TEL"] = this.rptBase.Items[0].FindControl("CTAC_TEL").value();

                dr["CUR_STS"] = this.STSCODE;

                if (this.Master.Master.nowStatus == "UpdAfter")
                {

                    if (this.rptOpin.Items[this.rptOpin.Items.Count - 1].FindControl("IF_PASS").value() != "")
                    {
                        if (this.rptOpin.Items[this.rptOpin.Items.Count - 1].FindControl("IF_PASS").value() == "0")
                            dr["CUR_STS"] = "R";
                        else
                            dr["CUR_STS"] = "2";
                    }
                    else
                    {
                        for (int i = 0; i <= this.rptOpin.Items.Count - 2; i++)
                        {
                            if (this.rptOpin.Items[i].FindControl("IF_PASS").value() != "")
                            {
                                dr["CUR_STS"] = "1";
                                break;
                            }
                        }
                    }

                }


                if (this.Master.Master.nowStatus == "Del")
                {
                    dr["CUR_STS"] = "F";
                }
                if (this.Master.Master.nowStatus == "Upd")
                {
                    dr["CUR_STS"] = "0";
                }
                dr["BIZ_PROMO_PROC"] = this.rptBase.Items[0].FindControl("BIZ_PROMO_PROC").value();
                dr["SupplierBackground"] = this.rptBase.Items[0].FindControl("SupplierBackground").value();
                dr["MainCondition"] = this.rptBase.Items[0].FindControl("MainCondition").value();
                //     dr["OTHER_CONDITION"] = this.OTHER_CONDITION.Text.Trim();
                //   dr["EXPECT_AR_DATE"] = this.EXPECT_AR_DATE.Text.Trim().Replace("/","");
                //  dr["FINANCIAL_PURPOSE"] = this.FINANCIAL_PURPOSE.Text.Trim();



                dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                if (dt.Rows.Count == 0)
                    dt.Rows.Add(dr);
            }
        }
        #endregion

        #region tab02 dataprocess 型態標的物
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strType"></param>
        private void tab02_DataProcess(bool bolSave, string strAplyNo)
        {
            string strSQL = "";
            SqlDataReader myReader = null;
            DataTable dt;
            DataRow dr;

            if (!bolSave)
            {

                strSQL = "  select a.CASE_STS,a.CASE_DESC,Q_CASE_STS=(case when isnull(b.CASE_STS,'')!='' then CONVERT(BIT,1) else CONVERT(BIT,0) END)  from ";
                strSQL += " (select CASE_STS,max(TypeDesc) as CASE_DESC,max(ATCH_SEQ) as ATCH_SEQ ";
                strSQL += " from OR_CASE_TYPE inner join OR3_COND_DEF on CASE_STS=TypeCode and TypeField='CASE_STS' group by CASE_STS";
                strSQL += " ) a left join OR3_QUOTA_CASE_TYPE b on a.CASE_STS=b.CASE_STS and b.QUOTA_APLY_NO='" + strAplyNo + "'          ";
                strSQL += " order by a.ATCH_SEQ, a.CASE_STS";
                dt = dg.GetDataTable(strSQL);

                this.rptCASETYPE.DataSource = dt;
                this.rptCASETYPE.DataBind();


                strSQL = " select a.AUD_CASE_TYPE ,d.CAL_NAME,";
                strSQL += " a.CASE_SOUR,a.AGENT,a.OTHER ";
                strSQL += " from OR3_QUOTA_APLY_APLY_COND  a  ";
                strSQL += " left join OR_CASE_CAL  d on a.AUD_CASE_TYPE=d.CAL_NO";
                strSQL += " where a.QUOTA_APLY_NO='" + strAplyNo + "'";
                dt = dg.GetDataTable(strSQL);

                if (dt.Rows.Count == 0)
                {
                    dr = dt.NewRow();
                    dr["AUD_CASE_TYPE"] = "9";
                    dr["CAL_NAME"] = dg.GetDataRow("select CAL_NAME from OR_CASE_CAL where CAL_NO='9'")[0].ToString();
                    dt.Rows.Add(dr);
                }

                this.rptObjMain.DataSource = dt;
                this.rptObjMain.DataBind();


                strSQL = "  select OBJ_KEY=a.OBJ_CODE,a.OBJ_CODE,OBJ_LOC_ADDR,PROD_NAME,a.OTC,a.SPEC,a.BRAND,";
                strSQL += " a.OBJ_DUE_OWNER,a.MAC_NO,a.YEAR,a.CAR_NO,a.REAL_BUY_PRC,a.BUDG_LEASE_AMT,a.SELF_RATE,a.BUDG_LEASE,a.BUY_RATE,a.OBJ_ASUR_TYPE,";
                strSQL += " a.INV_AMT_I_IB,a.RV_AMT,a.OBJ_LOC_TEL,a.OBJ_LOC_FAX,a.OBJ_LOC_CTAC,a.BUY_WAY,a.BUY_PROMISE,a.FRC_CODE,c.FRC_SNAME,";
                strSQL += " a.SALES_UNIT,a.OBJ_ONUS";
                strSQL += " from OR3_QUOTA_APLY_OBJ a";
                strSQL += " left join OR_FRC c on a.FRC_CODE=c.FRC_CODE";
                strSQL += " where a.QUOTA_APLY_NO='" + strAplyNo + "'";
                dt = dg.GetDataTable(strSQL);
                dr = dt.NewRow();
                dr["OBJ_KEY"] = "";
                dr["OBJ_CODE"] = "";
                dr["PROD_NAME"] = "";
                dr["FRC_CODE"] = "";
                dr["SALES_UNIT"] = "";
                dr["REAL_BUY_PRC"] = 0;

                dt.Rows.Add(dr);

                this.GridObject = dt;

                DataView dv = this.GridObject.DefaultView;
                dv.RowFilter = "OBJ_CODE<>''";
                this.rptObjGrid.DataSource = GridObject;
                this.rptObjGrid.DataBind();

                dv.RowFilter = "";
                dv = this.GridObject.DefaultView;
                dv.RowFilter = "OBJ_CODE=''";
                this.rptObjDetail.DataSource = dv;
                this.rptObjDetail.DataBind();
                if (dt.Rows.Count == 1)
                {
                    ((TextBox)this.rptObjDetail.Items[0].FindControl("OBJ_LOC_ADDR")).Text = this.rptBaseCustom.Items[0].FindControl("SALES_RGT_ADDR").value();
                    ((CheckBox)this.rptObjDetail.Items[0].FindControl("otc")).Checked = true;
                }





                strSQL = " select  OBJ_KEY=a.OBJ_CODE,a.OBJ_CODE,a.ACCS_NAME,a.ACCS_SEQ from OR3_QUOTA_APLY_OBJ_ACCS a   ";
                strSQL += " where QUOTA_APLY_NO='" + strAplyNo + "'";
                dt = dg.GetDataTable(strSQL);

                this.GridAccs = dt;
                dv.RowFilter = "";
                dv = this.GridAccs.DefaultView;
                dv.RowFilter = "OBJ_CODE=''";
                this.rptAccs.DataSource = dv;
                this.rptAccs.DataBind();

                this.nowRow_Object = "0";
                string strScript = "document.getElementById('trObj0').className='crow';";
                this.setScript(strScript);
            }
            else
            {
                DataTable dtCopy;
                DataRow[] drCopy;

                dt = dts.GetTable("OR3_QUOTA_CASE_TYPE", "QUOTA_APLY_NO='" + strAplyNo + "'");

                dtCopy = dt.Copy();

                dt.DeleteRows();

                for (int i = 0; i < this.rptCASETYPE.Items.Count; i++)
                {
                    CheckBox chk = (CheckBox)this.rptCASETYPE.Items[i].FindControl("CASE_TYPE_CODE");
                    if (chk.Checked)
                    {
                        drCopy = dtCopy.Select("CASE_STS='" + chk.CssClass + "'");

                        dr = dt.NewRow();
                        dr["QUOTA_APLY_NO"] = strAplyNo;

                        dr["CASE_STS"] = chk.CssClass;

                        dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                        if (drCopy.Length > 0)
                        {
                            dr["LAST_UPD_USER_ID"] = drCopy[0]["LAST_UPD_USER_ID"].ToString();
                            dr["LAST_UPD_DATE"] = drCopy[0]["LAST_UPD_DATE"].ToString();
                            dr["LAST_UPD_TIME"] = drCopy[0]["LAST_UPD_TIME"].ToString();
                        }
                        else
                        {
                            dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                            dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                            dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                        }

                        dt.Rows.Add(dr);
                    }

                }


                dt = dts.GetTable("OR3_QUOTA_APLY_APLY_COND", "QUOTA_APLY_NO='" + strAplyNo + "'");
                if (dt.Rows.Count == 0)
                {

                    dr = dt.NewRow();
                    dr["QUOTA_APLY_NO"] = strAplyNo;
                    dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                    dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                    dr["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                }
                else
                    dr = dt.Rows[0];



                // dr["CASE_TYPE_CODE"] = this.rptObjMain.Items[0].FindControl("CASE_TYPE_CODE").value();
                dr["AUD_CASE_TYPE"] = this.rptObjMain.Items[0].FindControl("AUD_CASE_TYPE").value();
                dr["CASE_SOUR"] = this.rptObjMain.Items[0].FindControl("CASE_SOUR").value();

                dr["AGENT"] = this.rptObjMain.Items[0].FindControl("AGENT").value();
                dr["OTHER"] = this.rptObjMain.Items[0].FindControl("OTHER").value();


                dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                if (dt.Rows.Count == 0)
                    dt.Rows.Add(dr);


                string strCodeKey = "";
                int intCNT = this.dg.GetDataRow("select count(*)  from OR3_QUOTA_APLY_OBJ where QUOTA_APLY_NO='" + strAplyNo + "'")[0].ToString().toInt();
                for (int i = 0; i < this.GridObject.Rows.Count; i++)
                {
                    if (this.GridObject.Rows[i].RowState != DataRowState.Deleted)
                    {

                        string strKey = this.GridObject.Rows[i]["OBJ_KEY"].ToString();
                        string strCode = this.GridObject.Rows[i]["OBJ_CODE"].ToString();
                        if (strKey != "")
                        {
                            if (this.dg.GetDataRow("select 'Y' from OR3_QUOTA_APLY_OBJ where OBJ_CODE='" + strCode + "'  and obj_code!=''")[0].ToString() != "Y")
                            {
                                string strKeyChk = strCode.PadLeft(3, ' ').Substring(0, 3).ToUpper();
                                intCNT++;
                                if (strKeyChk != "OTC" && strKeyChk != "ORX" && strKeyChk != "XRC")
                                {


                                    strCode = strAplyNo.Substring(0, 4) + intCNT.ToString().PadLeft(3, '0') + strAplyNo.Substring(6, 2) + "00" + strAplyNo.Substring(8, 3);
                                }


                            }

                            DataRow[] aryDr = this.GridObject.Select("OBJ_KEY='" + strKey + "'");
                            for (int j = 0; j < aryDr.Length; j++)
                            {
                                aryDr[j]["OBJ_CODE"] = strCode;
                            }
                            aryDr = this.GridAccs.Select("OBJ_KEY='" + strKey + "'");
                            for (int j = 0; j < aryDr.Length; j++)
                            {
                                aryDr[j]["OBJ_CODE"] = strCode;
                            }
                            string text = strCodeKey;
                            strCodeKey = string.Concat(new string[] { text, (strCodeKey == "") ? "" : ",", "'", strCode, "'" });
                        }
                    }
                }



                if (strCodeKey != "")
                {
                    DataTable dtACCS = this.dts.GetTable("OR3_QUOTA_APLY_OBJ_ACCS", "QUOTA_APLY_NO='" + strAplyNo + "'");
                    dtCopy = dtACCS.Copy();
                    for (int i = 0; i < this.GridAccs.Rows.Count; i++)
                    {
                        string strCode = this.GridAccs.Rows[i]["OBJ_CODE"].ToString();
                        if (strCode != "")
                        {
                            string strSeq = (i + 1).ToString();
                            drCopy = dtCopy.Select("OBJ_CODE='" + strCode + "' and ACCS_SEQ=" + strSeq);
                            string strKeyChk = strCode.PadLeft(3, ' ').Substring(0, 3).ToUpper();
                            if (strKeyChk != "OTC" && strKeyChk != "ORX" && strKeyChk != "XRC")
                            {
                                dtACCS.DeleteRows("OBJ_CODE='" + strCode + "' and ACCS_SEQ=" + strSeq);
                                dr = dtACCS.NewRow();
                                dr["QUOTA_APLY_NO"] = strAplyNo;

                                dr["OBJ_CODE"] = strCode;
                                dr["ACCS_SEQ"] = strSeq;
                                dr["ACCS_NAME"] = this.GridAccs.Rows[i]["ACCS_NAME"].ToString();
                                dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                                dr["ADD_DATE"] = DateTime.Now.ToString("yyyyMMdd");
                                dr["ADD_TIME"] = DateTime.Now.ToString("HH:mm:ss");
                                if (drCopy.Length > 0)
                                {
                                    dr["LAST_UPD_USER_ID"] = drCopy[0]["LAST_UPD_USER_ID"].ToString();
                                    dr["LAST_UPD_DATE"] = drCopy[0]["LAST_UPD_DATE"].ToString();
                                    dr["LAST_UPD_TIME"] = drCopy[0]["LAST_UPD_TIME"].ToString();
                                }
                                else
                                {
                                    dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                                    dr["LAST_UPD_DATE"] = DateTime.Now.ToString("yyyyMMdd");
                                    dr["LAST_UPD_TIME"] = DateTime.Now.ToString("HH:mm:ss");
                                }
                                dtACCS.Rows.Add(dr);
                            }
                        }
                    }
                    if (this.rptObjGrid.Items.Count != 0)
                    {

                        DataTable dtOBJECT = dts.GetTable("OR3_QUOTA_APLY_OBJ", "QUOTA_APLY_NO='" + strAplyNo + "'");
                        dtCopy = dtOBJECT.Copy();


                        for (int i = 0; i < this.GridObject.Rows.Count; i++)
                        {
                            if (this.GridObject.Rows[i].RowState != DataRowState.Deleted)
                            {

                                string strCode = this.GridObject.Rows[i]["OBJ_CODE"].ToString().Trim();
                                if (strCode == "")
                                    continue;

                                drCopy = dtCopy.Select("OBJ_CODE='" + strCode + "'");
                                string strKeyChk = strCode.PadLeft(3, ' ').Substring(0, 3).ToUpper();
                                if (strKeyChk != "OTC" && strKeyChk != "ORX" && strKeyChk != "XRC")
                                {
                                    dtOBJECT.DeleteRows("OBJ_CODE='" + strCode + "'");
                                    dr = dtOBJECT.NewRow();

                                    dr["QUOTA_APLY_NO"] = strAplyNo;
                                    dr["OBJ_CODE"] = strCode;
                                    dr["OBJ_LOC_ADDR"] = GridObject.Rows[i]["OBJ_LOC_ADDR"].ToString();
                                    dr["PROD_NAME"] = GridObject.Rows[i]["PROD_NAME"].ToString();
                                    dr["OTC"] = GridObject.Rows[i]["OTC"].ToString();
                                    dr["SPEC"] = GridObject.Rows[i]["SPEC"].ToString();
                                    dr["BRAND"] = GridObject.Rows[i]["BRAND"].ToString();
                                    dr["OBJ_DUE_OWNER"] = GridObject.Rows[i]["OBJ_DUE_OWNER"].ToString();
                                    dr["MAC_NO"] = GridObject.Rows[i]["MAC_NO"].ToString();
                                    dr["YEAR"] = GridObject.Rows[i]["YEAR"].ToString();
                                    dr["CAR_NO"] = GridObject.Rows[i]["CAR_NO"].ToString();
                                    dr["REAL_BUY_PRC"] = GridObject.Rows[i]["REAL_BUY_PRC"].ToString().toNumber();
                                    dr["BUDG_LEASE_AMT"] = GridObject.Rows[i]["BUDG_LEASE_AMT"].ToString().toNumber();
                                    dr["SELF_RATE"] = GridObject.Rows[i]["SELF_RATE"].ToString().toNumber();
                                    dr["BUDG_LEASE"] = GridObject.Rows[i]["BUDG_LEASE"].ToString();
                                    dr["BUY_RATE"] = GridObject.Rows[i]["BUY_RATE"].ToString().toNumber();
                                    dr["OBJ_ASUR_TYPE"] = GridObject.Rows[i]["OBJ_ASUR_TYPE"].ToString();
                                    dr["INV_AMT_I_IB"] = GridObject.Rows[i]["INV_AMT_I_IB"].ToString().toNumber();
                                    dr["RV_AMT"] = GridObject.Rows[i]["RV_AMT"].ToString().toNumber();
                                    dr["OBJ_LOC_TEL"] = GridObject.Rows[i]["OBJ_LOC_TEL"].ToString();
                                    dr["OBJ_LOC_FAX"] = GridObject.Rows[i]["OBJ_LOC_FAX"].ToString();
                                    dr["OBJ_LOC_CTAC"] = GridObject.Rows[i]["OBJ_LOC_CTAC"].ToString();
                                    dr["BUY_WAY"] = GridObject.Rows[i]["BUY_WAY"].ToString();
                                    dr["BUY_PROMISE"] = GridObject.Rows[i]["BUY_PROMISE"].ToString();
                                    dr["FRC_CODE"] = GridObject.Rows[i]["FRC_CODE"].ToString();
                                    dr["SALES_UNIT"] = GridObject.Rows[i]["SALES_UNIT"].ToString();
                                    dr["OBJ_ONUS"] = GridObject.Rows[i]["OBJ_ONUS"].ToString();

                                    if (drCopy.Length > 0)
                                    {
                                        dr["ADD_USER_ID"] = drCopy[0]["LAST_UPD_USER_ID"].ToString();
                                        dr["ADD_DATE"] = drCopy[0]["LAST_UPD_DATE"].ToString();
                                        dr["ADD_TIME"] = drCopy[0]["LAST_UPD_TIME"].ToString();
                                    }
                                    else
                                    {
                                        dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                                        dr["ADD_DATE"] = DateTime.Now.ToString("yyyyMMdd");
                                        dr["ADD_TIME"] = DateTime.Now.ToString("HH:mm:ss");
                                    }
                                    dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                                    dr["LAST_UPD_DATE"] = DateTime.Now.ToString("yyyyMMdd");
                                    dr["LAST_UPD_TIME"] = DateTime.Now.ToString("HH:mm:ss");
                                    dtOBJECT.Rows.Add(dr);
                                }
                                else
                                {
                                    dr = dtOBJECT.Select("OBJ_CODE='" + strCode + "'")[0];
                                    dr["RV_AMT"] = this.GridObject.Rows[i]["RV_AMT"].ToString().toNumber();
                                    dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                                    dr["LAST_UPD_DATE"] = DateTime.Now.ToString("yyyyMMdd");
                                    dr["LAST_UPD_TIME"] = DateTime.Now.ToString("HH:mm:ss");

                                }
                            }
                            else
                            {
                                string strCode = this.GridObject.Rows[i]["OBJ_CODE"].ToString().Trim();
                                if (strCode == "")
                                    continue;
                                dtOBJECT.DeleteRows("OBJ_CODE='" + strCode + "'");
                            }

                        }
                    }

                }


            }
        }
        #endregion

        #region tab10 dataprocess 額度使用者

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strType"></param>
        private void tab10_DataProcess(bool bolSave, string strAplyNo)
        {
            string strSQL = "";
            SqlDataReader myReader = null;

            DataRow dr;

            if (!bolSave)
            {

                string bCUST = this.rptBase.Items[0].FindControl("CUST_NO").value();
                strSQL = " select case QU.CUST_NO when '" + bCUST + "' then 1 else 2 end as SN";
                strSQL += " , QU.CUST_NO,C.CUST_SNAME,QU.PARENT_COMP ,PARENT_COMP_SHAREHOLDING,";
                strSQL += " QU.PARENT_COMP_INVEST_YEAR,QU.PARENT_COMP_INVEST_INCOME,QU.PREV_REAL_VAL,QU.UPPER_LIMIT,REAL_CAPT_AMT";
                strSQL += " from OR3_QUOTA_APLY_USER QU";
                strSQL += " inner join OR_CUSTOM C on C.CUST_NO=QU.CUST_NO";
                strSQL += " where QUOTA_APLY_NO='" + this.QUOTA_APLY_NO.Text.Trim() + "'";
                strSQL += " order by sn,QU.CUST_NO";


                myReader = dg.openSqlReader(strSQL);
                DataTable dt = new DataTable();
                dt.Load(myReader);
                dg.closeSqlReader(myReader);
                string strCUST_NO = "";
                if (dt.Rows.Count > 0)
                {
                    dr = dt.Rows[0];
                    strCUST_NO = dr["CUST_NO"].ToString().Trim();
                    string strScript = "setCustom('" + dr["CUST_NO"].ToString().Trim() + "', '" + dr["CUST_SNAME"].ToString().Trim() + "', '" + dr["REAL_CAPT_AMT"].ToString().toNumber().ToString("###,###,###,##0") + "');";
                    //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setCustom", strScript, true);
                    this.setScript(strScript);
                    //Response.Write(strScript);
                }
                int scnt = dt.Rows.Count;
                int ecnt = 9;
                for (int i = scnt; i <= ecnt; i++)
                {
                    dr = dt.NewRow();
                    dr["SN"] = i+1;
                    dr["CUST_NO"] = "";
                    dr["CUST_SNAME"] = "";
                    dr["REAL_CAPT_AMT"] = 0;
                    dt.Rows.Add(dr);
                }

                if (dt.Select("CUST_NO='" + strCUST_NO + "'").Length > 0)
                {
                    dr = dt.Select("CUST_NO='" + strCUST_NO + "'")[0];
                    this.PARENT_COMP.Text = dr["PARENT_COMP"].ToString();
                    this.PARENT_COMP_SHAREHOLDING.Text = dr["PARENT_COMP_SHAREHOLDING"].ToString();
                    this.PARENT_COMP_INVEST_YEAR.Text = dr["PARENT_COMP_INVEST_YEAR"].ToString();
                    this.PARENT_COMP_INVEST_INCOME.Text = dr["PARENT_COMP_INVEST_INCOME"].ToString();
                    this.PREV_REAL_VAL.Text = dr["PREV_REAL_VAL"].ToString(); ;
                    this.UPPER_LIMIT.Text = dr["UPPER_LIMIT"].ToString(); ;

                }

                DataView dv = dt.DefaultView;
                if (strCUST_NO != "")
                    dv.RowFilter = "CUST_NO<>'" + strCUST_NO + "'";

                this.rptUSER.DataSource = dv;
                this.rptUSER.DataBind();
            }
            else
            {
                DataTable dtUSER = dts.GetTable("OR3_QUOTA_APLY_USER", "QUOTA_APLY_NO='" + strAplyNo + "'");

                DataTable dtCopy = dtUSER.Copy();
                DataRow[] drCopy;
                // dtUSER.DeleteRows();
                cts.Execute("delete from OR3_QUOTA_APLY_USER where QUOTA_APLY_NO='" + strAplyNo + "'");

                drCopy = dtCopy.Select("CUST_NO='" + this.rptBase.Items[0].FindControl("CUST_NO").value() + "'");
                dr = dtUSER.NewRow();
                dr["QUOTA_APLY_NO"] = strAplyNo;
                dr["CUST_NO"] = this.rptBase.Items[0].FindControl("CUST_NO").value();
                dr["PARENT_COMP"] = this.PARENT_COMP.Text;
                dr["PARENT_COMP_SHAREHOLDING"] = this.PARENT_COMP_SHAREHOLDING.Text;
                dr["PARENT_COMP_INVEST_YEAR"] = this.PARENT_COMP_INVEST_YEAR.Text.toNumber();
                dr["PARENT_COMP_INVEST_INCOME"] = this.PARENT_COMP_INVEST_INCOME.Text;
                dr["UPPER_LIMIT"] = this.UPPER_LIMIT.Text;
                dr["PREV_REAL_VAL"] = this.PREV_REAL_VAL.Text;
                dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                dr["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                if (drCopy.Length > 0)
                {
                    dr["LAST_UPD_USER_ID"] = drCopy[0]["LAST_UPD_USER_ID"].ToString();
                    dr["LAST_UPD_DATE"] = drCopy[0]["LAST_UPD_DATE"].ToString();
                    dr["LAST_UPD_TIME"] = drCopy[0]["LAST_UPD_TIME"].ToString();
                }
                else
                {
                    dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                    dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                    dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                }

                dtUSER.Rows.Add(dr);

                for (int i = 0; i < this.rptUSER.Items.Count; i++)
                {
                    string strCUST = this.rptUSER.Items[i].FindControl("CUST_NO").value();
                    if (strCUST != "")
                    {
                        drCopy = dtCopy.Select("CUST_NO='" + strCUST + "'");
                        dr = dtUSER.NewRow();
                        dr["QUOTA_APLY_NO"] = strAplyNo;
                        dr["CUST_NO"] = this.rptUSER.Items[i].FindControl("CUST_NO").value();
                        dr["PARENT_COMP"] = this.rptUSER.Items[i].FindControl("PARENT_COMP").value();
                        dr["PARENT_COMP_SHAREHOLDING"] = this.rptUSER.Items[i].FindControl("PARENT_COMP_SHAREHOLDING").value().toNumber();
                        dr["PARENT_COMP_INVEST_YEAR"] = this.rptUSER.Items[i].FindControl("PARENT_COMP_INVEST_YEAR").value().toNumber();
                        dr["PARENT_COMP_INVEST_INCOME"] = this.rptUSER.Items[i].FindControl("PARENT_COMP_INVEST_INCOME").value().toNumber();
                        dr["UPPER_LIMIT"] = this.rptUSER.Items[i].FindControl("UPPER_LIMIT").value().toNumber();
                        dr["PREV_REAL_VAL"] = this.rptUSER.Items[i].FindControl("PREV_REAL_VAL").value().toNumber();
                        dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                        if (drCopy.Length > 0)
                        {
                            dr["LAST_UPD_USER_ID"] = drCopy[i]["LAST_UPD_USER_ID"].ToString();
                            dr["LAST_UPD_DATE"] = drCopy[i]["LAST_UPD_DATE"].ToString();
                            dr["LAST_UPD_TIME"] = drCopy[i]["LAST_UPD_TIME"].ToString();
                        }
                        else
                        {
                            dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                            dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                            dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                        }

                        dtUSER.Rows.Add(dr);

                    }

                }

            }
        }
        #endregion

        #region tab03 dataprocess 申請條件
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strType"></param>
        private void tab03_DataProcess(bool bolSave, string strAplyNo)
        {
            string strSQL = "";

            DataTable dt;
            DataRow dr;

            if (!bolSave)
            {
                string strPCase_Type = this.rptObjMain.Items[0].FindControl("AUD_CASE_TYPE").value();
                this.txtCaseType.Text = strPCase_Type;
                this.setAUD(strPCase_Type);

                strSQL = " select a.APLY_TOT_QUOTA,a.APLY_APPR_TYPE,APLY_FIRST_APPR_DATE=dbo.f_DateAddSlash(a.APLY_FIRST_APPR_DATE),a.APLY_QUOTA_MONTHS,a.APLY_DURN_M,";
                strSQL += " a.APLY_PAY_PERD,a.APLY_B_RECEIVE,a.APLY_A_RECEIVE,";
                strSQL += " APLY_SURF_TR,APLY_REAL_TR,APLY_SERV_CHAR,ISU_FACTOR,APLY_OTH_APLY_COND,APLY_FINA_PURP ";
                strSQL += " from OR3_QUOTA_APLY_APLY_COND a";
                strSQL += " where a.QUOTA_APLY_NO='" + strAplyNo + "'";
                dt = dg.GetDataTable(strSQL);

                if (dt.Rows.Count == 0)
                {
                    dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }

                this.rptRequest.DataSource = dt;
                this.rptRequest.DataBind();




                this.setPREV_APLY_NO("btnTab3");



                strSQL = " select AUD_LVL_CODE,B_TR,DURN FROM OR3_QUOTA_APRV_CALSS_APLY ";
                strSQL += " where QUOTA_APLY_NO='" + strAplyNo + "'";
                dt = dg.GetDataTable(strSQL);

                int scnt = dt.Rows.Count;
                int ecnt = 9;
                for (int i = scnt; i <= ecnt; i++)
                {
                    dr = dt.NewRow();
                    dr["AUD_LVL_CODE"] = "";
                    dr["B_TR"] = 0;
                    dr["DURN"] = 0;
                    dt.Rows.Add(dr);
                }

                this.rptAPRV.DataSource = dt;
                this.rptAPRV.DataBind();



            }
            else
            {
                DataTable dtRequest = dts.GetTable("OR3_QUOTA_APLY_APLY_COND", "QUOTA_APLY_NO='" + strAplyNo + "'");


                if (dtRequest.Rows.Count == 0)
                {

                    dr = dtRequest.NewRow();
                    dr["QUOTA_APLY_NO"] = strAplyNo;
                    dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                    dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                    dr["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                }
                else
                    dr = dtRequest.Rows[0];



                dr["APLY_TOT_QUOTA"] = this.rptRequest.Items[0].FindControl("APLY_TOT_QUOTA").value();
                dr["APLY_APPR_TYPE"] = this.rptRequest.Items[0].FindControl("APLY_APPR_TYPE").value();
                dr["APLY_FIRST_APPR_DATE"] = this.rptRequest.Items[0].FindControl("APLY_FIRST_APPR_DATE").value().Replace("/", "");
                dr["APLY_QUOTA_MONTHS"] = this.rptRequest.Items[0].FindControl("APLY_QUOTA_MONTHS").value();
                dr["APLY_DURN_M"] = this.rptRequest.Items[0].FindControl("APLY_DURN_M").value();
                dr["APLY_PAY_PERD"] = this.rptRequest.Items[0].FindControl("APLY_PAY_PERD").value();
                if (this.rptRequest.Items[0].FindControl("APLY_B_RECEIVE").value() == "Y")
                {
                    dr["APLY_B_RECEIVE"] = "Y";
                    dr["APLY_A_RECEIVE"] = "N";
                }
                else
                {
                    dr["APLY_B_RECEIVE"] = "N";
                    dr["APLY_A_RECEIVE"] = "Y";
                }

                dr["APLY_SURF_TR"] = this.rptRequest.Items[0].FindControl("APLY_SURF_TR").value();
                dr["APLY_REAL_TR"] = this.rptRequest.Items[0].FindControl("APLY_REAL_TR").value();
                dr["APLY_SERV_CHAR"] = this.rptRequest.Items[0].FindControl("APLY_SERV_CHAR").value();
                dr["ISU_FACTOR"] = this.rptRequest.Items[0].FindControl("ISU_FACTOR").value();
                dr["APLY_OTH_APLY_COND"] = this.rptRequest.Items[0].FindControl("APLY_OTH_APLY_COND").value();
                dr["APLY_FINA_PURP"] = this.rptRequest.Items[0].FindControl("APLY_FINA_PURP").value();

                dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                if (dtRequest.Rows.Count == 0)
                    dtRequest.Rows.Add(dr);



                dt = dts.GetTable("OR3_QUOTA_CONTAIN_FORMER_APLY", "QUOTA_APLY_NO='" + strAplyNo + "'");

                DataTable dtCopy = dt.Copy();
                dt.DeleteRows();
                DataRow[] drCopy;
                for (int i = 0; i < this.rptFORMER.Items.Count; i++)
                {
                    drCopy = dtCopy.Select("PREV_APLY_NO='" + this.rptFORMER.Items[0].FindControl("PREV_APLY_NO").value() + "'");

                    string chkSelect = this.rptFORMER.Items[0].FindControl("chkSelect").value();
                    if (chkSelect == "Y")
                    {
                        dr = dt.NewRow();
                        dr["QUOTA_APLY_NO"] = strAplyNo;
                        dr["PREV_APLY_NO"] = this.rptFORMER.Items[i].FindControl("PREV_APLY_NO").value();
                        dr["USED_QUOTA"] = this.rptFORMER.Items[i].FindControl("USED_QUOTA").value();

                        dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                        if (drCopy.Length > 0)
                        {
                            dr["LAST_UPD_USER_ID"] = drCopy[0]["LAST_UPD_USER_ID"].ToString();
                            dr["LAST_UPD_DATE"] = drCopy[0]["LAST_UPD_DATE"].ToString();
                            dr["LAST_UPD_TIME"] = drCopy[0]["LAST_UPD_TIME"].ToString();
                        }
                        else
                        {
                            dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                            dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                            dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                        }

                        dt.Rows.Add(dr);
                    }

                }

                dt = dts.GetTable("OR3_QUOTA_APRV_CALSS_APLY", "QUOTA_APLY_NO='" + strAplyNo + "'");

                dtCopy = dt.Copy();
                dt.DeleteRows();

                for (int i = 0; i < this.rptAPRV.Items.Count; i++)
                {
                    string strCODE = this.rptAPRV.Items[i].FindControl("AUD_LVL_CODE").value();
                    if (strCODE == "")
                        continue;
                    drCopy = dtCopy.Select("AUD_LVL_CODE='" + strCODE + "'");

                    dr = dt.NewRow();

                    dr["QUOTA_APLY_NO"] = strAplyNo;
                    dr["AUD_LVL_CODE"] = this.rptAPRV.Items[i].FindControl("AUD_LVL_CODE").value();
                    dr["B_TR"] = this.rptAPRV.Items[i].FindControl("B_TR").value();
                    dr["DURN"] = this.rptAPRV.Items[i].FindControl("DURN").value();

                    dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                    dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                    dr["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                    if (drCopy.Length > 0)
                    {
                        dr["LAST_UPD_USER_ID"] = drCopy[0]["LAST_UPD_USER_ID"].ToString();
                        dr["LAST_UPD_DATE"] = drCopy[0]["LAST_UPD_DATE"].ToString();
                        dr["LAST_UPD_TIME"] = drCopy[0]["LAST_UPD_TIME"].ToString();
                    }
                    else
                    {
                        dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                    }

                    dt.Rows.Add(dr);
                }

            }


        }
        #endregion




        #region tab04 dataprocess 擔保條件
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strType"></param>
        private void tab04_DataProcess(bool bolSave, string strAplyNo)
        {
            string strSQL = "";
            SqlDataReader myReader = null;
            DataTable dt;
            DataRow dr;

            if (!bolSave)
            {
                strSQL = " select	SCUR_NATUR,SCUR_ID,SCUR_NAME,SCUR_RELATION ";
                strSQL += " from	OR_CASE_APLY_SCUR";
                strSQL += " where APLY_NO='" + strAplyNo + "'";
                myReader = dg.openSqlReader(strSQL);
                dt = new DataTable();
                dt.Load(myReader);
                dg.closeSqlReader(myReader);
                int scnt = dt.Rows.Count;
                int ecnt = 15;
                for (int i = scnt; i <= 15; i++)
                {
                    dr = dt.NewRow();
                    dr["SCUR_ID"] = "";
                    dr["SCUR_NATUR"] = "";
                    dr["SCUR_NAME"] = "";
                    dt.Rows.Add(dr);
                }
                this.rptScur.DataSource = dt;
                this.rptScur.DataBind();

                strSQL = " select	ASUR_TYPE_CODE,ONUS,AMOUNT ";
                strSQL += " from	OR_CASE_APLY_ASUR ";
                strSQL += " where APLY_NO='" + strAplyNo + "'";
                myReader = dg.openSqlReader(strSQL);
                dt = new DataTable();
                dt.Load(myReader);
                dg.closeSqlReader(myReader);
                scnt = dt.Rows.Count;
                ecnt = 5;
                for (int i = scnt; i <= ecnt; i++)
                {
                    dr = dt.NewRow();
                    dr["ASUR_TYPE_CODE"] = "";
                    dt.Rows.Add(dr);
                }
                this.rptASUR.DataSource = dt;
                this.rptASUR.DataBind();

                strSQL = " select	PROD_NAME,SPEC,BRAND,MAC_NO,CAR_NO,YEAR,RV_AMT,ASUR_TYPE_CODE,ASUR_PAYER,ASUR_AMOUNT,Memos,End_Date=dbo.f_DateAddSlash(End_Date) ";
                strSQL += " from	OR_MOBJECT  ";
                strSQL += " where APLY_NO='" + strAplyNo + "'";
                myReader = dg.openSqlReader(strSQL);
                dt = new DataTable();
                dt.Load(myReader);
                dg.closeSqlReader(myReader);
                scnt = dt.Rows.Count;
                ecnt = 10;
                for (int i = scnt; i <= ecnt; i++)
                {
                    dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }
                this.rptObject.DataSource = dt;
                this.rptObject.DataBind();

                strSQL = " select	RLTY_DESC,BUILDING_NO,LAND_AREA,BUILDING_AREA,DECD_PRC,COLL_SUR,CASE_FLAG,PRICE_BASE=PRCICE_BASE,[ORDER],";
                strSQL += " SET_AMT,ORDER1,ORDER1_AMT,ORDER2,ORDER2_AMT,ORDER3,ORDER3_AMT";
                strSQL += " from	OR_CASE_APLY_RLTY_IMP  ";
                strSQL += " where APLY_NO='" + strAplyNo + "'";
                myReader = dg.openSqlReader(strSQL);
                dt = new DataTable();
                dt.Load(myReader);
                dg.closeSqlReader(myReader);
                scnt = dt.Rows.Count;
                ecnt = 10;
                for (int i = scnt; i <= ecnt; i++)
                {
                    dr = dt.NewRow();
                    dr["PRICE_BASE"] = "";
                    dt.Rows.Add(dr);
                }
                this.rptIMP.DataSource = dt;
                this.rptIMP.DataBind();
            }
            else
            {
                DataTable dtSCUR = dts.GetTable("OR_CASE_APLY_SCUR", "APLY_NO='" + strAplyNo + "'");


                DataTable dtCopy = dtSCUR.Copy();
                DataRow[] drCopy;
                dtSCUR.DeleteRows();

                for (int i = 0; i < this.rptScur.Items.Count; i++)
                {
                    if (this.rptScur.Items[i].FindControl("SCUR_ID").value() != "")
                    {
                        string strCode = (i + 1).ToString();
                        drCopy = dtCopy.Select("ORD_NO='" + strCode + "'");
                        dr = dtSCUR.NewRow();

                        dr["APLY_NO"] = strAplyNo;
                        dr["ORD_NO"] = strCode;
                        dr["SCUR_NATUR"] = this.rptScur.Items[i].FindControl("SCUR_NATUR").value();
                        dr["SCUR_ID"] = this.rptScur.Items[i].FindControl("SCUR_ID").value();
                        dr["SCUR_NAME"] = this.rptScur.Items[i].FindControl("SCUR_NAME").value();
                        dr["SCUR_RELATION"] = this.rptScur.Items[i].FindControl("SCUR_RELATION").value();
                        dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                        if (drCopy.Length > 0)
                        {
                            dr["LAST_UPD_USER_ID"] = drCopy[0]["LAST_UPD_USER_ID"].ToString();
                            dr["LAST_UPD_DATE"] = drCopy[0]["LAST_UPD_DATE"].ToString();
                            dr["LAST_UPD_TIME"] = drCopy[0]["LAST_UPD_TIME"].ToString();
                        }
                        else
                        {
                            dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                            dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                            dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                        }

                        dtSCUR.Rows.Add(dr);
                    }
                }

                DataTable dtASUR = dts.GetTable("OR_CASE_APLY_ASUR", "APLY_NO='" + strAplyNo + "'");
                dtCopy = dtASUR.Copy();
                dtASUR.DeleteRows();

                for (int i = 0; i < this.rptASUR.Items.Count; i++)
                {

                    string strCode = this.rptASUR.Items[i].FindControl("ASUR_TYPE_CODE").value();
                    if (strCode != "")
                    {
                        drCopy = dtCopy.Select("ASUR_TYPE_CODE='" + strCode + "'");
                        dr = dtASUR.NewRow();

                        dr["APLY_NO"] = strAplyNo;
                        dr["ASUR_TYPE_CODE"] = strCode;
                        dr["ONUS"] = this.rptASUR.Items[i].FindControl("ONUS").value();
                        dr["AMOUNT"] = this.rptASUR.Items[i].FindControl("AMOUNT").value();
                        dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                        if (drCopy.Length > 0)
                        {
                            dr["LAST_UPD_USER_ID"] = drCopy[0]["LAST_UPD_USER_ID"].ToString();
                            dr["LAST_UPD_DATE"] = drCopy[0]["LAST_UPD_DATE"].ToString();
                            dr["LAST_UPD_TIME"] = drCopy[0]["LAST_UPD_TIME"].ToString();
                        }
                        else
                        {
                            dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                            dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                            dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                        }

                        dtASUR.Rows.Add(dr);
                    }
                }

                DataTable dtOBJ = dts.GetTable("OR_MOBJECT", "APLY_NO='" + strAplyNo + "'");
                dtCopy = dtOBJ.Copy();
                dtOBJ.DeleteRows();

                for (int i = 0; i < this.rptObject.Items.Count; i++)
                {

                    if (this.rptObject.Items[i].FindControl("PROD_NAME").value() != "")
                    {
                        string strCode = (i + 1).ToString();
                        drCopy = dtCopy.Select("MOBJ_SN='" + strCode + "'");
                        dr = dtOBJ.NewRow();
                        dr["APLY_NO"] = strAplyNo;
                        dr["MOBJ_SN"] = strCode;
                        dr["PROD_NAME"] = this.rptObject.Items[i].FindControl("PROD_NAME").value();
                        dr["SPEC"] = this.rptObject.Items[i].FindControl("SPEC").value();
                        dr["BRAND"] = this.rptObject.Items[i].FindControl("BRAND").value();
                        dr["MAC_NO"] = this.rptObject.Items[i].FindControl("MAC_NO").value();
                        dr["CAR_NO"] = this.rptObject.Items[i].FindControl("CAR_NO").value();
                        dr["YEAR"] = this.rptObject.Items[i].FindControl("YEAR").value();
                        dr["RV_AMT"] = this.rptObject.Items[i].FindControl("RV_AMT").value();
                        dr["ASUR_TYPE_CODE"] = this.rptObject.Items[i].FindControl("ASUR_TYPE_CODE").value();
                        dr["ASUR_PAYER"] = this.rptObject.Items[i].FindControl("ASUR_PAYER").value();
                        dr["ASUR_AMOUNT"] = this.rptObject.Items[i].FindControl("ASUR_AMOUNT").value();
                        dr["Memos"] = this.rptObject.Items[i].FindControl("Memos").value();
                        dr["End_Date"] = this.rptObject.Items[i].FindControl("End_Date").value().Replace("/", "");
                        dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                        if (drCopy.Length > 0)
                        {
                            dr["LAST_UPD_USER_ID"] = drCopy[0]["LAST_UPD_USER_ID"].ToString();
                            dr["LAST_UPD_DATE"] = drCopy[0]["LAST_UPD_DATE"].ToString();
                            dr["LAST_UPD_TIME"] = drCopy[0]["LAST_UPD_TIME"].ToString();
                        }
                        else
                        {
                            dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                            dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                            dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                        }

                        dtOBJ.Rows.Add(dr);
                    }
                }

                DataTable dtIMP = dts.GetTable("OR_CASE_APLY_RLTY_IMP", "APLY_NO='" + strAplyNo + "'");
                dtCopy = dtOBJ.Copy();
                dtOBJ.DeleteRows();

                for (int i = 0; i < this.rptIMP.Items.Count; i++)
                {

                    if (this.rptIMP.Items[i].FindControl("RLTY_DESC").value() != "")
                    {
                        string strCode = (i + 1).ToString();
                        drCopy = dtCopy.Select("SEQ_NO='" + strCode + "'");
                        dr = dtOBJ.NewRow();
                        dr["APLY_NO"] = strAplyNo;
                        dr["SEQ_NO"] = strCode;
                        dr["RLTY_DESC"] = this.rptIMP.Items[i].FindControl("RLTY_DESC").value();
                        dr["BUILDING_NO"] = this.rptIMP.Items[i].FindControl("BUILDING_NO").value();
                        dr["LAND_AREA"] = this.rptIMP.Items[i].FindControl("LAND_AREA").value();
                        dr["BUILDING_AREA"] = this.rptIMP.Items[i].FindControl("BUILDING_AREA").value();
                        dr["DECD_PRC"] = this.rptIMP.Items[i].FindControl("DECD_PRC").value();
                        dr["COLL_SUR"] = this.rptIMP.Items[i].FindControl("COLL_SUR").value();
                        dr["CASE_FLAG"] = this.rptIMP.Items[i].FindControl("CASE_FLAG").value();
                        dr["PRICE_BASE"] = this.rptIMP.Items[i].FindControl("PRICE_BASE").value();
                        dr["ORDER"] = this.rptIMP.Items[i].FindControl("ORDER").value();
                        dr["SET_AMT"] = this.rptIMP.Items[i].FindControl("SET_AMT").value();
                        dr["ORDER1"] = this.rptIMP.Items[i].FindControl("ORDER1").value();
                        dr["ORDER1_AMT"] = this.rptIMP.Items[i].FindControl("ORDER1_AMT").value();
                        dr["ORDER2"] = this.rptIMP.Items[i].FindControl("ORDER2").value();
                        dr["ORDER2_AMT"] = this.rptIMP.Items[i].FindControl("ORDER2_AMT").value();
                        dr["ORDER3"] = this.rptIMP.Items[i].FindControl("ORDER3").value();
                        dr["ORDER3_AMT"] = this.rptIMP.Items[i].FindControl("ORDER3_AMT").value();
                        dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                        if (drCopy.Length > 0)
                        {
                            dr["LAST_UPD_USER_ID"] = drCopy[0]["LAST_UPD_USER_ID"].ToString();
                            dr["LAST_UPD_DATE"] = drCopy[0]["LAST_UPD_DATE"].ToString();
                            dr["LAST_UPD_TIME"] = drCopy[0]["LAST_UPD_TIME"].ToString();
                        }
                        else
                        {
                            dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                            dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                            dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                        }

                        dtOBJ.Rows.Add(dr);
                    }
                }

            }
        }
        #endregion

        #region tab05 dataprocess 申請條件
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strType"></param>
        private void tab05_DataProcess(bool bolSave, string strAplyNo)
        {

            string strSQL = "";

            DataTable dt;
            DataRow dr;

            if (!bolSave)
            {
                string strPCase_Type = this.rptObjMain.Items[0].FindControl("AUD_CASE_TYPE").value();
                this.txtCaseType.Text = strPCase_Type;
                this.setAUD(strPCase_Type);

                strSQL = " select a.APRV_TOT_QUOTA,a.APRV_APPR_TYPE,APRV_FIRST_APPR_DATE=dbo.f_DateAddSlash(a.APRV_FIRST_APPR_DATE),a.APRV_QUOTA_MONTHS,a.APRV_DURN_M,";
                strSQL += " a.APRV_PAY_PERD,a.APRV_B_RECEIVE,a.APRV_A_RECEIVE,";
                strSQL += " APRV_SURF_TR,APRV_REAL_TR,APRV_SERV_CHAR,A_ISU_FACTOR,APRV_OTH_APLY_COND,APRV_FINA_PURP ";
                strSQL += " from OR3_QUOTA_APLY_APRV_COND a";
                strSQL += " where a.QUOTA_APLY_NO='" + strAplyNo + "'";
                dt = dg.GetDataTable(strSQL);

                if (dt.Rows.Count == 0)
                {
                    dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }

                this.rptRun.DataSource = dt;
                this.rptRun.DataBind();




                this.setPREV_APLY_NO("btnTab5");



                strSQL = " select AUD_LVL_CODE,B_TR,DURN FROM OR3_QUOTA_APRV_CALSS_APRV ";
                strSQL += " where QUOTA_APLY_NO='" + strAplyNo + "'";
                dt = dg.GetDataTable(strSQL);

                int scnt = dt.Rows.Count;
                int ecnt = 9;
                for (int i = scnt; i <= ecnt; i++)
                {
                    dr = dt.NewRow();
                    dr["AUD_LVL_CODE"] = "";
                    dr["B_TR"] = 0;
                    dr["DURN"] = 0;
                    dt.Rows.Add(dr);
                }

                this.rptAPRV1.DataSource = dt;
                this.rptAPRV1.DataBind();



            }
            else
            {
                DataTable dtRequest = dts.GetTable("OR3_QUOTA_APLY_APRV_COND", "QUOTA_APLY_NO='" + strAplyNo + "'");


                if (dtRequest.Rows.Count == 0)
                {

                    dr = dtRequest.NewRow();
                    dr["QUOTA_APLY_NO"] = strAplyNo;
                    dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                    dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                    dr["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                }
                else
                    dr = dtRequest.Rows[0];


                if (this.STSCODE == "0" || this.Master.Master.nowStatus == "Upd")
                {
                    dr["APRV_TOT_QUOTA"] = this.rptRequest.Items[0].FindControl("APLY_TOT_QUOTA").value();
                    dr["APRV_APPR_TYPE"] = this.rptRequest.Items[0].FindControl("APLY_APPR_TYPE").value();
                    dr["APRV_FIRST_APPR_DATE"] = this.rptRequest.Items[0].FindControl("APLY_FIRST_APPR_DATE").value().Replace("/", "");
                    dr["APRV_QUOTA_MONTHS"] = this.rptRequest.Items[0].FindControl("APLY_QUOTA_MONTHS").value();
                    dr["APRV_DURN_M"] = this.rptRequest.Items[0].FindControl("APLY_DURN_M").value();
                    dr["APRV_PAY_PERD"] = this.rptRequest.Items[0].FindControl("APLY_PAY_PERD").value();
                    if (this.rptRequest.Items[0].FindControl("APLY_B_RECEIVE").value() == "Y")
                    {
                        dr["APRV_B_RECEIVE"] = "Y";
                        dr["APRV_A_RECEIVE"] = "N";
                    }
                    else
                    {
                        dr["APRV_B_RECEIVE"] = "N";
                        dr["APRV_A_RECEIVE"] = "Y";
                    }
                    dr["APRV_SURF_TR"] = this.rptRequest.Items[0].FindControl("APLY_SURF_TR").value();
                    dr["APRV_REAL_TR"] = this.rptRequest.Items[0].FindControl("APLY_REAL_TR").value();
                    dr["APRV_SERV_CHAR"] = this.rptRequest.Items[0].FindControl("APLY_SERV_CHAR").value();
                    dr["A_ISU_FACTOR"] = this.rptRequest.Items[0].FindControl("ISU_FACTOR").value();
                    dr["APRV_OTH_APLY_COND"] = this.rptRequest.Items[0].FindControl("APLY_OTH_APLY_COND").value();
                    dr["APRV_FINA_PURP"] = this.rptRequest.Items[0].FindControl("APLY_FINA_PURP").value();
                }
                else
                {
                    dr["APRV_TOT_QUOTA"] = this.rptRun.Items[0].FindControl("APRV_TOT_QUOTA").value();
                    dr["APRV_APPR_TYPE"] = this.rptRun.Items[0].FindControl("APRV_APPR_TYPE").value();
                    dr["APRV_FIRST_APPR_DATE"] = this.rptRun.Items[0].FindControl("APRV_FIRST_APPR_DATE").value().Replace("/", "");
                    dr["APRV_QUOTA_MONTHS"] = this.rptRun.Items[0].FindControl("APRV_QUOTA_MONTHS").value();
                    dr["APRV_DURN_M"] = this.rptRun.Items[0].FindControl("APRV_DURN_M").value();
                    dr["APRV_PAY_PERD"] = this.rptRun.Items[0].FindControl("APRV_PAY_PERD").value();
                    if (this.rptRun.Items[0].FindControl("APRV_B_RECEIVE").value() == "Y")
                    {
                        dr["APRV_B_RECEIVE"] = "Y";
                        dr["APRV_A_RECEIVE"] = "N";
                    }
                    else
                    {
                        dr["APRV_B_RECEIVE"] = "N";
                        dr["APRV_A_RECEIVE"] = "Y";
                    }
                    dr["APRV_SURF_TR"] = this.rptRun.Items[0].FindControl("APRV_SURF_TR").value();
                    dr["APRV_REAL_TR"] = this.rptRun.Items[0].FindControl("APRV_REAL_TR").value();
                    dr["APRV_SERV_CHAR"] = this.rptRun.Items[0].FindControl("APRV_SERV_CHAR").value();
                    dr["A_ISU_FACTOR"] = this.rptRun.Items[0].FindControl("A_ISU_FACTOR").value();
                    dr["APRV_OTH_APLY_COND"] = this.rptRun.Items[0].FindControl("APRV_OTH_APLY_COND").value();
                    dr["APRV_FINA_PURP"] = this.rptRun.Items[0].FindControl("APRV_FINA_PURP").value();
                }

                dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                if (dtRequest.Rows.Count == 0)
                    dtRequest.Rows.Add(dr);



                dt = dts.GetTable("OR3_QUOTA_CONTAIN_FORMER_APLY", "QUOTA_APLY_NO='" + strAplyNo + "'");

                DataTable dtCopy = dt.Copy();
                DataRow[] drCopy;

                Repeater rpt = null;
                if (this.STSCODE == "0")
                    rpt = this.rptFORMER;
                else
                    rpt = this.rptFORMER1;

                for (int i = 0; i < rpt.Items.Count; i++)
                {
                    drCopy = dtCopy.Select("PREV_APLY_NO='" + rpt.Items[0].FindControl("PREV_APLY_NO").value() + "'");

                    string chkSelect = rpt.Items[0].FindControl("chkSelect").value();
                    if (chkSelect == "Y")
                    {
                        dr = dt.NewRow();
                        dr["QUOTA_APLY_NO"] = strAplyNo;
                        dr["PREV_APLY_NO"] = rpt.Items[i].FindControl("PREV_APLY_NO").value();
                        dr["USED_QUOTA"] = rpt.Items[i].FindControl("USED_QUOTA").value();

                        dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                        if (drCopy.Length > 0)
                        {
                            dr["LAST_UPD_USER_ID"] = drCopy[0]["LAST_UPD_USER_ID"].ToString();
                            dr["LAST_UPD_DATE"] = drCopy[0]["LAST_UPD_DATE"].ToString();
                            dr["LAST_UPD_TIME"] = drCopy[0]["LAST_UPD_TIME"].ToString();
                        }
                        else
                        {
                            dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                            dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                            dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                        }

                        dt.Rows.Add(dr);
                    }

                }

                dt = dts.GetTable("OR3_QUOTA_APRV_CALSS_APRV", "QUOTA_APLY_NO='" + strAplyNo + "'");

                dtCopy = dt.Copy();
                dt.DeleteRows();

                if (this.STSCODE == "0" || this.Master.Master.nowStatus == "Upd")
                    rpt = this.rptAPRV;
                else
                    rpt = this.rptAPRV1;
                for (int i = 0; i < rpt.Items.Count; i++)
                {
                    string strCODE = rpt.Items[i].FindControl("AUD_LVL_CODE").value();
                    if (strCODE == "")
                        continue;
                    drCopy = dtCopy.Select("AUD_LVL_CODE='" + strCODE + "'");
                    dr = dt.NewRow();


                    dr["QUOTA_APLY_NO"] = strAplyNo;
                    dr["AUD_LVL_CODE"] = rpt.Items[i].FindControl("AUD_LVL_CODE").value();
                    dr["B_TR"] = rpt.Items[i].FindControl("B_TR").value();
                    dr["DURN"] = rpt.Items[i].FindControl("DURN").value();

                    dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                    dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                    dr["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                    if (drCopy.Length > 0)
                    {
                        dr["LAST_UPD_USER_ID"] = drCopy[0]["LAST_UPD_USER_ID"].ToString();
                        dr["LAST_UPD_DATE"] = drCopy[0]["LAST_UPD_DATE"].ToString();
                        dr["LAST_UPD_TIME"] = drCopy[0]["LAST_UPD_TIME"].ToString();
                    }
                    else
                    {
                        dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                    }

                    dt.Rows.Add(dr);
                }

            }

        }
        #endregion

        #region tab06 dataprocess 往來實績
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strType"></param>
        private void tab06_DataProcess(bool bolSave, string strAplyNo)
        {
            string strSQL = "";
            SqlDataReader myReader = null;
            DataTable dt;
            DataRow dr;

            if (!bolSave)
            {

                strSQL = " select	OALT_AMT,L_THIS,L_APRV_IPREV_QUOTA_CYCLE,L_IPREV_QUOTA_CYCLE_USED,L_IPREV_QUOTA_CYCLE_Remains=L_APRV_IPREV_QUOTA_CYCLE-L_IPREV_QUOTA_CYCLE_USED,L_IPREV_CYCLE_SUR,";
                strSQL += " L_APRV_IPREV_QUOTA_NCYCLE,L_IPREV_QUOTA_NCYCLE_USED,L_IPREV_QUOTA_NCYCLE_Remains=L_APRV_IPREV_QUOTA_NCYCLE-L_IPREV_QUOTA_NCYCLE_USED,L_IPREV_NCYCLE_SUR,";
                strSQL += " L_APLY,L_APRV_PREV_QUOTA_CYCLE,L_PREV_QUOTA_CYCLE_USED,L_PREV_QUOTA_CYCLE_Remains=L_APRV_PREV_QUOTA_CYCLE-L_PREV_QUOTA_CYCLE_USED,L_PREV_CYCLE_SUR,";
                strSQL += " L_PREV,L_APRV_PREV_QUOTA_NCYCLE,L_PREV_QUOTA_NCYCLE_USED,L_PREV_QUOTA_NCYCLE_Remains=L_APRV_PREV_QUOTA_NCYCLE-L_PREV_QUOTA_NCYCLE_USED,L_PREV_NCYCLE_SUR,";
                strSQL += " L_APRV_PREV_QUOTA_SURPLUS,L_RISKS,L_SURPLUS=L_IPREV_CYCLE_SUR+L_IPREV_NCYCLE_SUR+L_PREV_CYCLE_SUR+L_PREV_NCYCLE_SUR+L_APRV_PREV_QUOTA_SURPLUS,";
                strSQL += " R_APLY,R_APRV_PREV_QUOTA_CYCLE,R_PREV_QUOTA_CYCLE_USED,R_PREV_QUOTA_CYCLE_Remains=R_APRV_PREV_QUOTA_CYCLE-R_PREV_QUOTA_CYCLE_USED,R_PREV_CYCLE_SUR,";
                strSQL += " R_PREV,R_APRV_PREV_QUOTA_NCYCLE,R_PREV_QUOTA_NCYCLE_USED,R_PREV_QUOTA_NCYCLE_Remains=R_APRV_PREV_QUOTA_NCYCLE-R_PREV_QUOTA_NCYCLE_USED,R_PREV_NCYCLE_SUR,";
                strSQL += " R_APRV_PREV_QUOTA_SURPLUS,R_RISKS,R_SURPLUS=R_PREV_CYCLE_SUR+R_PREV_NCYCLE_SUR+R_PREV_CYCLE_SUR+R_PREV_NCYCLE_SUR+R_APRV_PREV_QUOTA_SURPLUS,";
                strSQL += " ALL_RISKS=L_RISKS+R_RISKS,ALL_SURPLUS=L_IPREV_CYCLE_SUR+L_IPREV_NCYCLE_SUR+L_PREV_CYCLE_SUR+L_PREV_NCYCLE_SUR+L_APRV_PREV_QUOTA_SURPLUS+R_PREV_CYCLE_SUR+R_PREV_NCYCLE_SUR+R_PREV_CYCLE_SUR+R_PREV_NCYCLE_SUR+R_APRV_PREV_QUOTA_SURPLUS";

                strSQL += " from OR3_CON_BAL";
                strSQL += " where APLY_NO='" + strAplyNo + "'";
                dt = dg.GetDataTable(strSQL);


                if (dt.Rows.Count == 0)
                {
                    dr = dt.NewRow();

                    dt.Rows.Add(dr);
                }
                this.rptContact.DataSource = dt;
                this.rptContact.DataBind();
            }
            else
            {
                dt = dts.GetTable("OR3_CON_BAL", "APLY_NO='" + strAplyNo + "'");
                if (dt.Rows.Count == 0)
                {

                    dr = dt.NewRow();
                    dr["APLY_NO"] = strAplyNo;
                }
                else
                    dr = dt.Rows[0];



                dr["OALT_AMT"] = dtSURPLUS.Rows[0]["OALT_AMT"].ToString().Trim();
                dr["L_THIS"] = dtSURPLUS.Rows[0]["L_THIS"].ToString().Trim();
                dr["L_APLY"] = dtSURPLUS.Rows[0]["L_APLY"].ToString().Trim();
                dr["L_PREV"] = dtSURPLUS.Rows[0]["L_PREV"].ToString().Trim();
                dr["R_APLY"] = dtSURPLUS.Rows[0]["R_APLY"].ToString().Trim();
                dr["R_PREV"] = dtSURPLUS.Rows[0]["R_PREV"].ToString().Trim();

                dr["L_APRV_IPREV_QUOTA_CYCLE"] = dtSURPLUS.Rows[0]["L_APRV_IPREV_QUOTA_CYCLE"].ToString().Trim();
                dr["L_IPREV_QUOTA_CYCLE_USED"] = dtSURPLUS.Rows[0]["L_IPREV_QUOTA_CYCLE_USED"].ToString().Trim();
                dr["L_IPREV_CYCLE_SUR"] = dtSURPLUS.Rows[0]["L_IPREV_CYCLE_SUR"].ToString().Trim();

                dr["L_APRV_IPREV_QUOTA_NCYCLE"] = dtSURPLUS.Rows[0]["L_APRV_IPREV_QUOTA_NCYCLE"].ToString().Trim();
                dr["L_IPREV_QUOTA_NCYCLE_USED"] = dtSURPLUS.Rows[0]["L_IPREV_QUOTA_NCYCLE_USED"].ToString().Trim();
                dr["L_IPREV_NCYCLE_SUR"] = dtSURPLUS.Rows[0]["L_IPREV_NCYCLE_SUR"].ToString().Trim();

                dr["L_APRV_PREV_QUOTA_CYCLE"] = dtSURPLUS.Rows[0]["L_APRV_PREV_QUOTA_CYCLE"].ToString().Trim();
                dr["L_PREV_QUOTA_CYCLE_USED"] = dtSURPLUS.Rows[0]["L_PREV_QUOTA_CYCLE_USED"].ToString().Trim();
                dr["L_PREV_CYCLE_SUR"] = dtSURPLUS.Rows[0]["L_PREV_CYCLE_SUR"].ToString().Trim();

                dr["L_APRV_PREV_QUOTA_NCYCLE"] = dtSURPLUS.Rows[0]["L_APRV_PREV_QUOTA_NCYCLE"].ToString().Trim();
                dr["L_PREV_QUOTA_NCYCLE_USED"] = dtSURPLUS.Rows[0]["L_PREV_QUOTA_NCYCLE_USED"].ToString().Trim();
                dr["L_PREV_NCYCLE_SUR"] = dtSURPLUS.Rows[0]["L_PREV_NCYCLE_SUR"].ToString().Trim();

                dr["R_APRV_PREV_QUOTA_CYCLE"] = dtSURPLUS.Rows[0]["R_APRV_PREV_QUOTA_CYCLE"].ToString().Trim();
                dr["R_PREV_QUOTA_CYCLE_USED"] = dtSURPLUS.Rows[0]["R_PREV_QUOTA_CYCLE_USED"].ToString().Trim();
                dr["R_PREV_CYCLE_SUR"] = dtSURPLUS.Rows[0]["R_PREV_CYCLE_SUR"].ToString().Trim();

                dr["R_APRV_PREV_QUOTA_NCYCLE"] = dtSURPLUS.Rows[0]["R_APRV_PREV_QUOTA_NCYCLE"].ToString().Trim();
                dr["R_PREV_QUOTA_NCYCLE_USED"] = dtSURPLUS.Rows[0]["R_PREV_QUOTA_NCYCLE_USED"].ToString().Trim();
                dr["R_PREV_NCYCLE_SUR"] = dtSURPLUS.Rows[0]["R_PREV_NCYCLE_SUR"].ToString().Trim();



                dr["L_APRV_PREV_QUOTA_SURPLUS"] = dtSURPLUS.Rows[0]["L_APRV_PREV_QUOTA_SURPLUS"].ToString().Trim();
                dr["L_RISKS"] = dtSURPLUS.Rows[0]["L_RISKS"].ToString().Trim();

                dr["R_APRV_PREV_QUOTA_SURPLUS"] = dtSURPLUS.Rows[0]["R_APRV_PREV_QUOTA_SURPLUS"].ToString().Trim();
                dr["R_RISKS"] = dtSURPLUS.Rows[0]["R_RISKS"].ToString().Trim();

                if (dt.Rows.Count == 0)
                    dt.Rows.Add(dr);
            }
        }

        #endregion

        #region tab07 dataprocess 權限條件
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strType"></param>
        private void tab07_DataProcess(bool bolSave, string strAplyNo)
        {
            string strSQL = "";
            SqlDataReader myReader = null;
            DataTable dt;
            DataRow dr;

            if (!bolSave)
            {
                //  strSQL = " select a.SEL,a.AUD_CODE,b.AUD_NAME,b.AUD_LVL_CODE,c.AUD_LVL_NAME,Memo=case HIGH_PRIORITY when '1' then '最高等級' else '' end  ";
                //  strSQL += " from OR_CASE_AUD_SELECT a left join  OR_AUD_REF b on a.CASE_TYPE=b.CASE_TYPE and a.AUD_CODE=b.AUD_CODE ";
                //  strSQL += " inner JOIN OR_AUD_LVL_NAME c  on b.AUD_LVL_CODE=c.AUD_LVL_CODE ";
                //  strSQL += " where a.APLY_NO='" + strAplyNo + "'";
                string AUD_CASE_TYPE = this.rptObjMain.Items[0].FindControl("AUD_CASE_TYPE").value();
                strSQL = "exec s_WA060_Grid_AUD @PAply_no='" + strAplyNo + "',@PCase_Type='" + AUD_CASE_TYPE + "'";
                myReader = dg.openSqlReader(strSQL);
                dt = new DataTable();
                dt.Load(myReader);
                dg.closeSqlReader(myReader);
                this.rptAud.DataSource = dt;
                this.rptAud.DataBind();
            }
            else
            {
                string strPCase_Type = this.rptObjMain.Items[0].FindControl("AUD_CASE_TYPE").value();
                dt = dts.GetTable("OR_CASE_AUD_SELECT", "APLY_NO='" + strAplyNo + "'");
                dt.DeleteRows();
                for (int i = 0; i < this.rptAud.Items.Count; i++)
                {
                    if (((CheckBox)this.rptAud.Items[i].FindControl("SEL")).Checked)
                    {
                        string AUD_CODE = this.rptAud.Items[i].FindControl("AUD_CODE").value();

                        dr = dt.NewRow();
                        dr["APLY_NO"] = strAplyNo;
                        dr["AUD_CODE"] = AUD_CODE;
                        dr["CASE_TYPE"] = strPCase_Type;
                        dr["SEL"] = "1";
                        dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                        dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                        dt.Rows.Add(dr);
                    }
                }

            }
        }
        #endregion

        #region tab08 dataprocess 授信權限
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strType"></param>
        private void tab08_DataProcess(bool bolSave, string strAplyNo)
        {
            string strSQL = "";
            SqlDataReader myReader = null;
            DataTable dt;
            DataRow dr;

            if (!bolSave)
            {
                //授信權限
                strSQL = " select	S1=GEN_TR,SN1=isnull((select AUD_LVL_NAME from OR_AUD_LVL_NAME where GEN_TR=AUD_LVL_CODE),''),     ";
                strSQL += " S2=GEN_CHECK,SN2=isnull((select AUD_LVL_NAME from OR_AUD_LVL_NAME where GEN_CHECK=AUD_LVL_CODE),''),    ";
                //strSQL += " S3=GEN_NEW,SN3=isnull((select AUD_LVL_NAME from OR_AUD_LVL_NAME where GEN_NEW=AUD_LVL_CODE),''),     ";
                strSQL += " S4=GEN_CON,SN4=isnull((select AUD_LVL_NAME from OR_AUD_LVL_NAME where GEN_CON=AUD_LVL_CODE),''),  ";
                strSQL += " S5=VP_SUP,SN5=isnull((select AUD_LVL_NAME from OR_AUD_LVL_NAME where VP_SUP=AUD_LVL_CODE),''), ";
                strSQL += " S6=CRD,SN6=isnull((select AUD_LVL_NAME from OR_AUD_LVL_NAME where CRD=AUD_LVL_CODE),''), ";
                strSQL += " S7=OTH,SN7=isnull((select AUD_LVL_NAME from OR_AUD_LVL_NAME where OTH=AUD_LVL_CODE),''), ";
                //strSQL += " S8=VP_RANGE,SN8=isnull((select AUD_LVL_NAME from OR_AUD_LVL_NAME where VP_RANGE=AUD_LVL_CODE),''), ";
                strSQL += " S9=VP_STOCK,SN9=isnull((select AUD_LVL_NAME from OR_AUD_LVL_NAME where VP_STOCK=AUD_LVL_CODE),''), ";
                strSQL += " S10=VP_SUP_NO1,SN10=isnull((select AUD_LVL_NAME from OR_AUD_LVL_NAME where VP_SUP_NO1=AUD_LVL_CODE),''), ";
                strSQL += " S11=VP_TR,SN11=isnull((select AUD_LVL_NAME from OR_AUD_LVL_NAME where VP_TR=AUD_LVL_CODE),''), ";
                strSQL += " S12=VP_SUP_NOT,SN12=isnull((select AUD_LVL_NAME from OR_AUD_LVL_NAME where VP_SUP_NOT=AUD_LVL_CODE),''), ";
                //strSQL += " S13=VP_NEW,SN13=isnull((select AUD_LVL_NAME from OR_AUD_LVL_NAME where VP_NEW=AUD_LVL_CODE),''), ";
                strSQL += " S14=VP_CON,SN14=isnull((select AUD_LVL_NAME from OR_AUD_LVL_NAME where VP_CON=AUD_LVL_CODE),''), ";
                strSQL += " S15=VP_CHECK,SN15=isnull((select AUD_LVL_NAME from OR_AUD_LVL_NAME where VP_CHECK=AUD_LVL_CODE),''), ";
                strSQL += " S16=LAST_AUT,SN16=isnull((select AUD_LVL_NAME from OR_AUD_LVL_NAME where LAST_AUT=AUD_LVL_CODE),'') ";
                strSQL += " from	OR_CASE_REF ";
                strSQL += " where APLY_NO='" + this.QUOTA_APLY_NO.Text.Trim() + "'";
                myReader = dg.openSqlReader(strSQL);
                dt = new DataTable();
                dt.Load(myReader);
                dg.closeSqlReader(myReader);
                if (dt.Rows.Count == 0)
                {
                    dr = dt.NewRow();
                    dr["SN1"] = "";
                    dr["SN2"] = "";
                    //  dr["SN3"] = "";
                    dr["SN4"] = "";
                    dr["SN5"] = "";
                    dr["SN6"] = "";
                    dr["SN7"] = "";
                    // dr["SN8"] = "";
                    dr["SN9"] = "";
                    dr["SN10"] = "";
                    dr["SN11"] = "";
                    dr["SN12"] = "";
                    //dr["SN13"] = "";
                    dr["SN14"] = "";
                    dr["SN15"] = "";
                    dr["SN16"] = "";

                    dt.Rows.Add(dr);
                }
                this.rptRef.DataSource = dt;
                this.rptRef.DataBind();
            }
            else
            {
                if (dtRef != null)
                {
                    dt = dts.GetTable("OR_CASE_REF", "APLY_NO='" + strAplyNo + "'");
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


                    dr["GEN_TR"] = dtRef.Rows[0]["S1"].ToString();
                    dr["GEN_CHECK"] = dtRef.Rows[0]["S2"].ToString();
                    //  dr["GEN_NEW"] = dtRef.Rows[0]["S3"].ToString();
                    dr["GEN_CON"] = dtRef.Rows[0]["S4"].ToString();
                    dr["VP_SUP"] = dtRef.Rows[0]["S5"].ToString();
                    dr["CRD"] = dtRef.Rows[0]["S6"].ToString();
                    dr["OTH"] = dtRef.Rows[0]["S7"].ToString();
                    // dr["VP_RANGE"] = dtRef.Rows[0]["S8"].ToString();
                    dr["VP_STOCK"] = dtRef.Rows[0]["S9"].ToString();
                    dr["VP_SUP_NO1"] = dtRef.Rows[0]["S10"].ToString();
                    dr["VP_TR"] = dtRef.Rows[0]["S11"].ToString();
                    dr["VP_SUP_NOT"] = dtRef.Rows[0]["S12"].ToString();
                    // dr["VP_NEW"] = dtRef.Rows[0]["S13"].ToString();
                    dr["VP_CON"] = dtRef.Rows[0]["S14"].ToString();
                    dr["VP_CHECK"] = dtRef.Rows[0]["S15"].ToString();
                    dr["LAST_AUT"] = dtRef.Rows[0]["S16"].ToString();



                    dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                    dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                    dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                    if (dt.Rows.Count == 0)
                        dt.Rows.Add(dr);
                }
            }
        }
        #endregion

        #region tab09 dataprocess 審查
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strType"></param>
        private void tab09_DataProcess(bool bolSave, string strAplyNo)
        {
            string strSQL = "";
            SqlDataReader myReader = null;
            DataTable dt;
            DataRow dr;

            if (!bolSave)
            {
                //  strSQL = " SELECT a.AUD_LVL_CODE,b.AUD_LVL_NAME,IF_PASS,AUD_OPIN ";
                //   strSQL += " FROM OR_CASE_APLY_AUD a left join OR_AUD_LVL_NAME b on a.AUD_LVL_CODE=b.AUD_LVL_CODE ";
                //   strSQL += " where APLY_NO='" + this.QUOTA_APLY_NO.Text.Trim() + "'";

                string strCode = dg.GetDataRow("select max(AUD_LVL_CODE) FROM OR_CASE_APLY_AUD   where APLY_NO='" + this.QUOTA_APLY_NO.Text.Trim() + "' and IF_PASS<>''")[0].ToString();


                /*strSQL = " SELECT a.AUD_LVL_CODE,b.AUD_LVL_NAME,IF_PASS,AUD_OPIN,";
                strSQL += " ENABLED=case when a.AUD_LVL_CODE=(case " + (this.STS.Trim().CompareTo("1")<=0 ? "1" : "2") + " when 1 then '00000' else (SELECT OR_EMP.AUD_LVL_CODE ";
                strSQL += " FROM OR3_USERS INNER JOIN OR_EMP ON OR_EMP.EMP_CODE=OR3_USERS.EMP_CODE ";
                strSQL += " WHERE OR3_USERS.USER_ID='" + this.Master.Master.UserId + "' ";
                strSQL += " AND OR3_USERS.USER_NAME='" + this.Master.Master.EmployeeName + "') end) then (case when a.AUD_LVL_CODE>=='" + strCode + "' then 1 else 0 end ) else 0 end";
                strSQL += "  FROM OR_CASE_APLY_AUD a left join OR_AUD_LVL_NAME b on a.AUD_LVL_CODE=b.AUD_LVL_CODE ";
                */

                if (this.Master.Master.nowStatus == "UpdAfter")
                {
                    strSQL = " SELECT a.AUD_LVL_CODE,b.AUD_LVL_NAME,IF_PASS,AUD_OPIN,LVL,";
                    strSQL += " ENABLED=case when '" + this.STS.Trim() + "'<='1' and a.AUD_LVL_CODE=(SELECT OR_EMP.AUD_LVL_CODE ";
                    strSQL += " FROM OR3_USERS INNER JOIN OR_EMP ON OR_EMP.EMP_CODE=OR3_USERS.EMP_CODE ";
                    strSQL += " WHERE OR3_USERS.USER_ID='" + this.Master.Master.UserId + "' ";
                    strSQL += " AND OR_EMP.EMP_NAME='" + this.Master.Master.EmployeeName + "')  then (case when a.AUD_LVL_CODE>='" + strCode + "' then 1 else 0 end ) else 0 end";
                    strSQL += "  FROM OR_CASE_APLY_AUD a left join OR_AUD_LVL_NAME b on a.AUD_LVL_CODE=b.AUD_LVL_CODE ";
                }
                else
                {
                    strSQL = " select a.AUD_LVL_CODE,b.AUD_LVL_NAME,IF_PASS,AUD_OPIN,";
                    strSQL += " ENABLED=case a.AUD_LVL_CODE when '00000' then 1 else 0 end,LVL";
                    strSQL += " FROM OR_CASE_APLY_AUD a left join OR_AUD_LVL_NAME b on a.AUD_LVL_CODE=b.AUD_LVL_CODE ";
                }

                if (this.Master.Master.nowStatus == "Copy")
                    strSQL += "where 1=2";
                else
                    strSQL += " where a.APLY_NO='" + this.QUOTA_APLY_NO.Text.Trim() + "' order by a.LVL,a.AUD_LVL_CODE";

                myReader = dg.openSqlReader(strSQL);
                this.rptOpin.DataSource = myReader;
                this.rptOpin.DataBind();
                dg.closeSqlReader(myReader);


            }
            else
            {

                DataTable dtAUD = dts.GetTable("OR_CASE_APLY_AUD", "APLY_NO='" + strAplyNo + "'");
                // DataTable dtSIGN = dts.GetTable("OR3_CASE_APLY_SIGN_REC", "APLY_NO='" + strAplyNo + "'");

                DataTable dtCopy = dtAUD.Copy();
                DataRow[] drCopy;
                dtAUD.DeleteRows();

                for (int i = 0; i < this.rptOpin.Items.Count; i++)
                {
                    string strCode = this.rptOpin.Items[i].FindControl("AUD_LVL_CODE").value();
                    drCopy = dtCopy.Select("AUD_LVL_CODE='" + strCode + "'");
                    bool bolEnable = ((DropDownList)this.rptOpin.Items[i].FindControl("IF_PASS")).Enabled;
                    dr = dtAUD.NewRow();

                    dr["APLY_NO"] = strAplyNo;
                    dr["AUD_LVL_CODE"] = strCode;
                    dr["AUD_OPIN"] = this.rptOpin.Items[i].FindControl("AUD_OPIN").value();
                    if (this.rptOpin.Items[i].FindControl("LVL").value() == "")
                        dr["LVL"] = System.DBNull.Value;
                    else
                        dr["LVL"] = this.rptOpin.Items[i].FindControl("LVL").value();

                    if (drCopy.Length > 0)
                    {
                        dr["APRV_DATE"] = drCopy[0]["APRV_DATE"].ToString();
                        dr["APRV_NAME"] = drCopy[0]["APRV_NAME"].ToString();
                    }
                    if (bolEnable)
                    {
                        if (drCopy.Length > 0)
                        {
                            if (drCopy[0]["IF_PASS"].ToString().Trim() != this.rptOpin.Items[i].FindControl("IF_PASS").value())
                                dr["APRV_DATE"] = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        }
                        else
                            dr["APRV_DATE"] = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                        dr["APRV_NAME"] = this.Master.Master.EmployeeName;
                    }

                    if (this.rptOpin.Items[i].FindControl("IF_PASS").value() == "")
                    {
                        dr["APRV_DATE"] = "";
                        dr["APRV_NAME"] = "";
                    }
                    dr["IF_PASS"] = this.rptOpin.Items[i].FindControl("IF_PASS").value();
                    /* dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                     dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                     dr["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                     if (drCopy.Length > 0)
                     {
                         dr["LAST_UPD_USER_ID"] = drCopy[0]["LAST_UPD_USER_ID"].ToString();
                         dr["LAST_UPD_DATE"] = drCopy[0]["LAST_UPD_DATE"].ToString();
                         dr["LAST_UPD_TIME"] = drCopy[0]["LAST_UPD_TIME"].ToString();
                     }
                     else
                     {
                         dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                         dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                         dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                     }*/

                    dtAUD.Rows.Add(dr);
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
            if (this.bolWA070)
            {
                this.Master.Master.nowStatus = "UpdAfter";
                strStatus = this.Master.Master.nowStatus;
            }

            if (!DataCheck(strStatus))
                return 0;


            this.bolSave = true;
            string strAplyNo = "";

            string strDEPT = this.rptBase.Items[0].FindControl("DEPT_CODE").value();
            string strDate = this.rptBase.Items[0].FindControl("APLY_DATE").value().Replace("/", "");

            if (this.QUOTA_APLY_NO.Text.Trim() == "")
            {
                string STS = "Q";//dg.GetDataRow("select CASE_STS from OR_CASE_TYPE where CASE_TYPE_CODE='" + this.rptObjMain.Items[0].FindControl("CASE_TYPE_CODE").value() + "'")[0].ToString();
                string strAPPTYPE = this.rptRequest.Items[0].FindControl("APLY_APPR_TYPE").value();
                if (strAPPTYPE == "2")
                    STS = "U";


                //[下午 03:08:57] Louis: 額度申請書編號的第一個參數由A, 改成J
                strAplyNo = dg.GetDataRow("exec s_GetNumber 'J','" + strDEPT + "" + STS + "','" + strDate.Substring(0, 4) + "',''")[0].ToString();
            }
            else
                strAplyNo = this.QUOTA_APLY_NO.Text.rpsText();

            DataTable dtBase = dts.GetTable(" OR3_QUOTA_APLY_BASE", "QUOTA_APLY_NO='" + strAplyNo + "'");
            DataRow dr = null;


            switch (strStatus)
            {

                case "Add":
                case "Copy":
                case "Upd":
                case "UpdAfter":

                    if (strStatus == "Add" || strStatus == "Copy")
                        this.STSCODE = "0";

                    if (this.STS != "L")
                    {
                        for (int i = 0; i < this.rptOpin.Items.Count; i++)
                        {
                            string ifPass = this.rptOpin.Items[i].FindControl("IF_PASS").value();
                            if (ifPass == "1" || ifPass == "2" || ifPass == "3")
                            {
                                break;
                            }
                            if (ifPass == "0")
                            {
                                this.STSCODE = "R";
                                this.CUR_STS.Text = "申請駁回";
                            }
                            else
                            {
                                this.STSCODE = "0";
                                this.CUR_STS.Text = "申請中";
                            }
                        }
                    }




                    //實績計算
                    this.GridFunc_Click(btnSURPLUS, new CommandEventArgs("SURPLUS", null));

                    //審查計算
                    this.GridFunc_Click(btnSet_Ref, new CommandEventArgs("Ref", null));
                    //GridFunc_Click(object sender, System.Web.UI.WebControls.CommandEventArgs e)                   


                    //this.SaveObjectDetail(this.bolGridAdd);
                    this.tab01_DataProcess(true, strAplyNo);
                    this.tab02_DataProcess(true, strAplyNo);
                    this.tab10_DataProcess(true, strAplyNo);

                    if (this.STSCODE == "0")
                        this.tab03_DataProcess(true, strAplyNo);

                    this.tab04_DataProcess(true, strAplyNo);
                    this.tab05_DataProcess(true, strAplyNo);
                    this.tab06_DataProcess(true, strAplyNo);
                    this.tab07_DataProcess(true, strAplyNo);
                    this.tab08_DataProcess(true, strAplyNo);
                    this.tab09_DataProcess(true, strAplyNo);




                    break;
                case "Del":
                    //  dt.DeleteRows();

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