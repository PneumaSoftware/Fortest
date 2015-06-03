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
    public partial class WA1301 : PageParent
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

        protected string APRV_MTHD
        {
            set { ViewState["APRV_MTHD"] = value; }
            get { return (ViewState["APRV_MTHD"] == null ? "" : (string)ViewState["APRV_MTHD"]); }
        }

        protected string APLY_MTHD
        {
            set { ViewState["APLY_MTHD"] = value; }
            get { return (ViewState["APLY_MTHD"] == null ? "" : (string)ViewState["APLY_MTHD"]); }
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




        Rtn r;
        bool bolSave = false;
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

            if (Session["nowStatus"] != null)
                this.Master.Master.nowStatus = (String)Session["nowStatus"];
            //***************************end 勿動****************************
        }
        #endregion


        protected void reload_data(object sender, EventArgs e)
        {

            if (this.APLY_NO.Text == "")
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
                if (Session["APLY_NO"] == null || Session["CUR_STS"] == null)
                {
                    this.Response.Redirect("WA060.aspx");
                }



                this.APLY_NO.Text = ((string)Session["APLY_NO"]).Trim();
                this.CUR_STS.Text = ((string)Session["CUR_STS"]).Trim();
                reload_data(null, null);
            }
            else
            {
                this.APLY_NO.Text = "";
                this.CUR_STS.Text = "";
                this.setDefaultValue();


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
            string strSQL = "";
            DataTable dt;
            DataRow dr;
            SqlDataReader myReader;

            string strAplyNo = this.APLY_NO.Text.Trim();

            this.tab01_DataProcess(false, strAplyNo);

            this.tab02_DataProcess(false, strAplyNo);

            this.tab03_DataProcess(false, strAplyNo);
            this.rptRequest.Editing(false);

            this.tab04_DataProcess(false, strAplyNo);

            this.tab05_DataProcess(false, strAplyNo);




        }
        #endregion




        #region zipcode
        protected void SetAddress(object sender, EventArgs e)
        {
            DropDownList REQU_ZIP = (DropDownList)this.rptBase.Items[0].FindControl("REQU_ZIP");
            DropDownList CITY_CODE = (DropDownList)this.rptBase.Items[0].FindControl("CITY_CODE");
            TextBox REQ_PAY_ADDR = (TextBox)this.rptBase.Items[0].FindControl("REQ_PAY_ADDR");

            if (REQ_PAY_ADDR.Text == "" && CITY_CODE.SelectedValue != "" && REQU_ZIP.SelectedValue != "")
                REQ_PAY_ADDR.Text = CITY_CODE.SelectedItem.Text.Trim() + REQU_ZIP.SelectedItem.Text.Trim().Substring(0, REQU_ZIP.SelectedItem.Text.Trim().Length - 4);

        }




        protected void ZIP_CODE_LOAD(object sender, EventArgs e)
        {

            DropDownList REQU_ZIP = (DropDownList)this.rptBase.Items[0].FindControl("REQU_ZIP");
            DropDownList CITY_CODE = (DropDownList)this.rptBase.Items[0].FindControl("CITY_CODE");

            if (CITY_CODE.ToolTip.Trim() != "")
            {
                CITY_CODE.SelectedValue = CITY_CODE.ToolTip;
                CITY_CODE.ToolTip = "";
            }
            string strCITY = CITY_CODE.SelectedValue;

            REQU_ZIP.Items.Clear();
            dg.ListBinding(REQU_ZIP, "select '' ZIP_CODE,'請選擇..' ZIP_NAME union all  select ZIP_CODE,ZIP_NAME=zone_name+' '+zip_code from or3_zip where city_code='" + strCITY + "' and ZIP_CODE!='' order by zip_code ");

            this.SetAddress(REQU_ZIP, null);
        }
        #endregion





        protected void GridFunc_Click(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {

            DataTable dtGridSource;
            string strScript = "";
            string strSQL = "";

            string strMessage = "";
            DataView dv;
            DataRow[] adr;

            DataView dvObj;
            string strID = ((Button)sender).ID;
            switch (strID)
            {



                case "btnDel_Object":

                    this.GridAccs.DeleteRows("OBJ_KEY='" + e.CommandName + "'");
                    this.GridObject.DeleteRows("OBJ_KEY='" + e.CommandName + "'");
                    DataView dvObjD = this.GridObject.DefaultView;
                    dvObjD.RowFilter = "OBJ_CODE<>''";
                    this.rptObjGrid.DataSource = dvObjD;
                    this.rptObjGrid.DataBind();
                    this.upObjGrid.Update();

                    break;

                case "btnEdit":
                    if (this.rptObjDetail.Items[0].FindControl("OBJ_CODE").value() == "")
                        strMessage += "[標的物代號]";

                    if (this.rptObjDetail.Items[0].FindControl("PROD_NAME").value() == "")
                        strMessage += "[品名]";

                    if (this.rptObjDetail.Items[0].FindControl("REAL_BUY_PRC").value() == "0")
                        strMessage += "[市價]";


                    if (this.rptObjDetail.Items[0].FindControl("BUY_WAY").value() == "")
                        strMessage += "[附買回方式]";


                    //3.型態標的物.買回比率的必輸檢查改為, 若[附買回方式]='不附買回'時, 不需檢查.

                    if (this.rptObjDetail.Items[0].FindControl("BUY_RATE").value() == "0" && this.rptObjDetail.Items[0].FindControl("BUY_WAY").value() != "1")
                        strMessage += "[買回比率]";


                    if (this.rptObjDetail.Items[0].FindControl("FRC_CODE").value() == "")
                        strMessage += "[供應商]";

                    if (this.rptObjDetail.Items[0].FindControl("OBJ_ASUR_TYPE").value() == "")
                        strMessage += "[險種別]";


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
                    dv.RowFilter = "OBJ_CODE=''";
                    this.rptObjDetail.DataSource = dv;
                    this.rptObjDetail.DataBind();
                    this.upObjDetail.Update();

                    this.exGridAccs = null;
                    this.exGridAccs = this.GridAccs.Clone();
                    adr = this.GridAccs.Select("OBJ_CODE=''");
                    for (int i = 0; i < adr.Length; i++)
                    {
                        this.exGridAccs.ImportRow(adr[i]);
                    }


                    if (this.exGridAccs != null)
                    {
                        dv = this.exGridAccs.DefaultView;
                        dv.RowFilter = "OBJ_CODE=''";
                        this.rptAccs.DataSource = dv;
                        this.rptAccs.DataBind();
                        this.upAccs.Update();
                    }

                    dvObj = this.GridObject.DefaultView;
                    dvObj.RowFilter = "OBJ_CODE<>''";

                    this.rptObjGrid.DataSource = dvObj;
                    this.rptObjGrid.DataBind();
                    this.upObjGrid.Update();

                    this.setProcessMessage("明細已暫存完成!!", false);
                    break;

                case "btnAdd_Object":
                case "btnUpd_Object":





                    if (((Button)sender).ID == "btnUpd_Object")
                        this.nowRow_Object = (((RepeaterItem)((Button)sender).Parent).ItemIndex + 1).ToString();

                    else
                        this.nowRow_Object = "0";

                    dvObj = this.GridObject.DefaultView;
                    dvObj.RowFilter = "OBJ_CODE<>''";

                    this.rptObjGrid.DataSource = dvObj;
                    this.rptObjGrid.DataBind();
                    this.upObjGrid.Update();

                    strScript = "document.getElementById('trObj" + this.nowRow_Object + "').className='crow';";
                    this.setScript(strScript);

                    dv = this.GridObject.DefaultView;
                    dv.RowFilter = "OBJ_CODE='" + e.CommandName + "'";
                    this.rptObjDetail.DataSource = dv;
                    this.rptObjDetail.DataBind();
                    this.upObjDetail.Update();

                    this.exGridAccs = null;
                    this.exGridAccs = this.GridAccs.Clone();
                    adr = this.GridAccs.Select("OBJ_CODE='" + e.CommandName + "'");
                    for (int i = 0; i < adr.Length; i++)
                    {
                        this.exGridAccs.ImportRow(adr[i]);
                    }


                    if (this.exGridAccs != null)
                    {
                        dv = this.exGridAccs.DefaultView;
                        dv.RowFilter = "OBJ_CODE='" + e.CommandName + "'";
                        this.rptAccs.DataSource = dv;
                        this.rptAccs.DataBind();
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

                    break;





            }
        }


        private void SaveObjectDetail(bool bolAdd)
        {
            DataTable dt = this.GridObject;
            string strKey = DateTime.Now.ToString("mmssfff");
            string strCode = this.rptObjDetail.Items[0].FindControl("OBJ_CODE").value();
            DataRow dr;
            if (bolAdd)
            {
                dr = dt.NewRow();
            }
            else
            {
                if (this.GridObject.Select("OBJ_CODE='" + strCode + "'").Length == 0)
                {
                    return;
                }
                dr = this.GridObject.Select("OBJ_CODE='" + strCode + "'")[0];
            }
            dr["OBJ_KEY"] = strKey;
            dr["OBJ_CODE"] = strCode;
            dr["OBJ_LOC_ADDR"] = this.rptObjDetail.Items[0].FindControl("OBJ_LOC_ADDR").value();
            dr["Actual_lessee"] = this.rptObjDetail.Items[0].FindControl("Actual_lessee").value();
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
            dr["Is_spec_repo"] = this.rptObjDetail.Items[0].FindControl("Is_spec_repo").value();
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



            try
            {
                dr["OLD_APLY_NO"] = "";
            }
            catch
            {
            }
            if (bolAdd)
            {
                dt.Rows.Add(dr);
            }
            this.GridAccs.DeleteRows("OBJ_CODE='" + strCode + "'");
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

                    /*  if (strStatus == "Add" || strStatus == "Upd")
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
                      */

                    if (this.rptBase.Items[0].FindControl("APLY_DATE").value() == "")
                        strMessage += "[申請日期]";

                    if (this.rptBase.Items[0].FindControl("DEPT_CODE").value() == "")
                        strMessage += "[申請單位]";

                    if (this.rptBase.Items[0].FindControl("EMP_CODE").value() == "")
                        strMessage += "[經辦代號]";

                    if (this.rptBase.Items[0].FindControl("CUST_NO").value() == "")
                        strMessage += "[客戶代號]";



                    if (this.rptBase.Items[0].FindControl("REQ_PAY_ADDR").value() == "")
                        strMessage += "[請款地址]";


                    if (this.rptBase.Items[0].FindControl("CONTACT").value() == "")
                        strMessage += "[連絡人]";

                    if (this.rptBase.Items[0].FindControl("CTAC_TEL").value() == "")
                        strMessage += "[電話]";


                    if (this.rptBase.Items[0].FindControl("FAX").value() == "")
                        strMessage += "[傳真]";


                    if (strMessage != "")
                    {
                        this.setMessageBox(strMessage + "必須輸入！");
                        return false;
                    }

                    if (this.rptObjMain.Items[0].FindControl("CASE_TYPE_CODE").value() == "")
                        strMessage += "[案件類別]";

                    if (this.rptObjMain.Items[0].FindControl("AUD_CASE_TYPE").value() == "")
                        strMessage += "[審查案件類別]";


                    if (strMessage != "")
                    {
                        this.setMessageBox(strMessage + "必須輸入！");
                        return false;
                    }

                    if (this.rptObjMain.Items[0].FindControl("AUD_CASE_TYPE").value() == "7")
                    {
                        if (rptBase.Items[0].FindControl("CUR_QUOTA_APLY_NO").value() == "")
                        {
                            this.setMessageBox("現額度編號必須輸入！");
                            return false;
                        }
                    }

                    if (this.rptObjMain.Items[0].FindControl("AUD_CASE_TYPE").value() == "5")
                    {
                        if (rptBaseCustom.Items[0].FindControl("BUILD_DATE").value() == "")
                        {
                            this.setMessageBox("公司設立日期未建！");
                            return false;
                        }
                    }


                    if (this.rptRequest.Items[0].FindControl("APLY_DURN_M").value() == "0" || this.rptRequest.Items[0].FindControl("APLY_PERD").value() == "0")
                        strMessage += "[期間]";



                    if (strStatus == "Add" || strStatus == "Upd")
                    {


                        if (this.rptRequest.Items[0].FindControl("APLY_PAY_MTHD").value() == "")
                            strMessage += "[申請條件:繳款方式]";

                        if (this.rptRequest.Items[0].FindControl("APLY_AMOR_MTHD").value() == "")
                            strMessage += "[申請條件:攤提方式]";

                        if (this.rptRequest.Items[0].FindControl("APLY_PAY_PERD").value() == "")
                            strMessage += "[申請條件:付款週期]";

                    }



                    if (strMessage != "")
                    {
                        this.setMessageBox(strMessage + "必須輸入！");
                        return false;
                    }



                    string STS = dg.GetDataRow("select CASE_STS from OR_CASE_TYPE where CASE_TYPE_CODE='" + this.rptObjMain.Items[0].FindControl("CASE_TYPE_CODE").value() + "'")[0].ToString();
                    if (STS != "L")
                    {
                        if (this.rptObjGrid.Items.Count == 0)
                        {
                            this.setMessageBox("標的物至少要有一筆資料!");
                            return false;
                        }

                        if (this.checkObj.Text == "N")
                        {
                            for (int i = 0; i < this.GridObject.Rows.Count; i++)
                            {
                                if (this.GridObject.Rows[i]["OTC"].ToString().Trim() != "Y" && this.GridObject.Rows[i]["OBJ_CODE"].ToString().Trim() != "")
                                {
                                    this.setScript("checkObject_Fail();");
                                    return false;
                                }
                            }

                        }
                    }

                    if (this.rptConfirm.Items[0].FindControl("SING_CON_DATE").value() == "")
                        strMessage += "[簽約日期]";


                    if (this.rptConfirm.Items[0].FindControl("PREDICT_LEASE_DATE").value() == "")
                        strMessage += "[預計起租日期]";

                    if (this.rptConfirm.Items[0].FindControl("CUS_PAY_WAY").value() == "")
                        strMessage += "[客戶付款方式]";


                    if (strMessage != "")
                    {
                        this.setMessageBox(strMessage + "必須輸入！");
                        return false;
                    }

                    if (this.rptRemark.Items[0].FindControl("BANK_NO").value() != "" && this.rptRemark.Items[0].FindControl("ACCNO").value() == "")
                    {
                        this.setMessageBox("全方位帳號有誤！");
                        return false;
                    }

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
            string strSQL = " select  TAKER,BUILD_DATE=dbo.f_DateAddSlash(BUILD_DATE),SALES_RGT_ADDR,";
            strSQL += " CAPT_STR=dbo.f_ConditionGetDesc('CAPT_STR',CAPT_STR,'N'),";
            strSQL += " ORG_TYPE=dbo.f_ConditionGetDesc('ORG_TYPE',ORG_TYPE,'N'),";
            strSQL += " RGT_CAPT_AMT,EMP_PSNS,REAL_CAPT_AMT,MAIN_BUS_ITEM,";
            strSQL += " CREDIT_CUST,JUDGE_LVL,";
            strSQL += " HONEST_AGREEMENT,SECRET_PROMISE";
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
                strSQL += " a.CUST_NO,d.CUST_SNAME,d.CUST_EMAIL_ADDR,InitContactDate,CON_SEQ_NO,OLD_CON_NO,MAST_CON_NO, ORG_QUOTA_APLY_NO,CUR_QUOTA_APLY_NO, ";
                strSQL += " PROJECT=(case when d.GEN_CURR_QUOTA+d.VP_CURR_QUOTA+d.AR_CURR_QUOTA>0 then '是額度客戶' else '' end), ";
                strSQL += " N_DEPT_CODE,N_DEPT_NAME=e.DEPT_NAME,a.REQU_ZIP,h.City_Code,a.REQ_PAY_ADDR, ";
                strSQL += " N_EMP_CODE=dbo.f_EmpIDToCorpAcct(a.N_EMP_CODE),N_EMP_NAME=f.EMP_NAME, ";
                strSQL += " RECVER,RECVER_DEPT,a.MMail_NO,g.MMail_NAME, ";
                strSQL += " a.CONTACT,a.CTAC_TEL,a.FAX,a.Mobile,a.SupplierBackground,a.MainCondition ";


                strSQL += " from	OR_CASE_APLY_BASE a left join OR_DEPT b on a.DEPT_CODE=b.DEPT_CODE ";
                strSQL += " left join OR_EMP c on a.EMP_CODE=c.EMP_CODE ";
                strSQL += " left join OR_CUSTOM d on a.CUST_NO=d.CUST_NO ";
                strSQL += " left join OR_DEPT e on a.N_DEPT_CODE=e.DEPT_CODE ";
                strSQL += " left join V_OR_EMP f on a.N_EMP_CODE=f.EMP_CODE ";
                strSQL += " left join OR_MERG_MAIL g on a.MMail_NO=g.MMail_NO ";
                strSQL += " left join OR3_Zip h on a.REQU_ZIP=h.Zip_Code ";
                strSQL += " where a.APLY_NO='" + strAplyNo + "'";
                dt = dg.GetDataTable(strSQL);

                if (dt.Rows.Count == 0)
                {
                    dr = dt.NewRow();
                    dr["APLY_DATE"] = System.DateTime.Now.ToString("yyyy/MM/dd");
                    dr["CUR_STS"] = "";
                    dt.Rows.Add(dr);
                }
                this.rptBase.DataSource = dt;
                this.rptBase.DataBind();
                this.STS = dt.Rows[0]["CUR_STS"].ToString();
                this.hiddenREQU_ZIP.Value = dt.Rows[0]["REQU_ZIP"].ToString().Trim();

                strSQL = "	select AP_SUP=(case when AP_SUP='Y' then 0 else 1 end), ";
                strSQL += " STOCK_CORP=(case when STOCK_CORP='Y' then 0 else 1 end), ";
                strSQL += " CHK_RCD_S=(case when CHK_RCD_S='Y' then 0 else 1 end), ";
                strSQL += " CHK_RCD_C=(case when CHK_RCD_C='Y' then 0 else 1 end), ";
                strSQL += " CHK_RCD_G=(case when CHK_RCD_G='Y' then 0 else 1 end), ";
                strSQL += " NEW_CORP=(case when NEW_CORP='Y' then 0 else 1 end), ";
                strSQL += " CRD_SAME=(case when CRD_SAME='Y' then 0 else 1 end), ";
                strSQL += " CRD_RAL=(case when CRD_RAL='Y' then 0 else 1 end), ";
                strSQL += " CRD_UNINV=(case when CRD_UNINV='Y' then 0 else 1 end) from OR_CASE_REF where APLY_NO='" + strAplyNo + "'";
                dt = dg.GetDataTable(strSQL);

                if (dt.Rows.Count == 0)
                {
                    dr = dt.NewRow();
                    dr["AP_SUP"] = 1;
                    dr["STOCK_CORP"] = 1;
                    dr["CHK_RCD_S"] = 1;
                    dr["CHK_RCD_C"] = 1;
                    dr["CHK_RCD_G"] = 1;
                    dr["NEW_CORP"] = 1;
                    dr["CRD_SAME"] = 1;
                    dr["CRD_RAL"] = 1;
                    dr["CRD_UNINV"] = 1;
                    dt.Rows.Add(dr);
                }
                this.rptREF.DataSource = dt;
                this.rptREF.DataBind();


                DropDownList CITY_CODE = ((DropDownList)this.rptBase.Items[0].FindControl("CITY_CODE"));

                CITY_CODE.SelectedValue = CITY_CODE.ToolTip;

                ZIP_CODE_LOAD((DropDownList)this.rptBase.Items[0].FindControl("REQ_ZIP"), null);


                //binding rptBaseCustom
                this.setCUSTOM();
            }
            else
            {
                dt = dts.GetTable("OR_CASE_APLY_BASE", "APLY_NO='" + strAplyNo + "'");
                if (dt.Rows.Count == 0)
                {

                    dr = dt.NewRow();
                    dr["APLY_NO"] = strAplyNo;
                    dr["CUR_STS"] = "";
                    dr["KEYIN_USER"] = this.Master.Master.EmployeeName;
                    dr["KEYIN_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                    dr["KEYIN_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                }
                else
                    dr = dt.Rows[0];

                //  dr["PAPER"] = this.rptCon.Items[0].FindControl("PAPER").value();
                dr["APLY_DATE"] = this.rptBase.Items[0].FindControl("APLY_DATE").value().Replace("/", "");
                dr["DEPT_CODE"] = this.rptBase.Items[0].FindControl("DEPT_CODE").value();
                dr["EMP_CODE"] = dg.GetDataRow("select dbo.f_CorpAcctToEmpID('" + this.rptBase.Items[0].FindControl("EMP_CODE").value() + "')")[0].ToString();
                dr["CUST_NO"] = this.rptBase.Items[0].FindControl("CUST_NO").value();
                dr["InitContactDate"] = this.rptBase.Items[0].FindControl("InitContactDate").value();
                // dr["CON_SEQ_NO"] = this.rptBase.Items[0].FindControl("CON_SEQ_NO").value();
                dr["OLD_CON_NO"] = this.rptBase.Items[0].FindControl("OLD_CON_NO").value();
                dr["MAST_CON_NO"] = this.rptBase.Items[0].FindControl("MAST_CON_NO").value();
                dr["ORG_QUOTA_APLY_NO"] = this.rptBase.Items[0].FindControl("ORG_QUOTA_APLY_NO").value();
                dr["CUR_QUOTA_APLY_NO"] = this.rptBase.Items[0].FindControl("CUR_QUOTA_APLY_NO").value();
                dr["REQU_ZIP"] = this.rptBase.Items[0].FindControl("REQU_ZIP").value();
                dr["REQ_PAY_ADDR"] = this.rptBase.Items[0].FindControl("REQ_PAY_ADDR").value();
                // dr["N_EMP_CODE"] = this.rptBase.Items[0].FindControl("N_EMP_CODE").value();
                dr["RECVER"] = this.rptBase.Items[0].FindControl("RECVER").value();
                dr["RECVER_DEPT"] = this.rptBase.Items[0].FindControl("RECVER_DEPT").value();
                //   if (this.rptConfirm.Items[0].FindControl("MMail_no").value()!="")
                dr["MMail_NO"] = this.rptConfirm.Items[0].FindControl("MMail_NO").value();
                //  else
                //      dr["MMail_NO"] = this.rptBase.Items[0].FindControl("MMail_NO").value();
                dr["CONTACT"] = this.rptBase.Items[0].FindControl("CONTACT").value();
                dr["CTAC_TEL"] = this.rptBase.Items[0].FindControl("CTAC_TEL").value();
                dr["FAX"] = this.rptBase.Items[0].FindControl("FAX").value();
                dr["Mobile"] = this.rptBase.Items[0].FindControl("Mobile").value();
                dr["SupplierBackground"] = this.rptBase.Items[0].FindControl("SupplierBackground").value();
                dr["MainCondition"] = this.rptBase.Items[0].FindControl("MainCondition").value();
                //  dr["OTHER_CONDITION"] = this.OTHER_CONDITION.Text.Trim();
                //   dr["EXPECT_AR_DATE"] = this.EXPECT_AR_DATE.Text.Trim().Replace("/","");
                //   dr["FINANCIAL_PURPOSE"] = this.FINANCIAL_PURPOSE.Text.Trim();
                //   dr["Auth_Cond_Remark"] = this.Auth_Cond_Remark.Text.Trim();
                dr["PAY_COND_DAY"] = this.rptRequest.Items[0].FindControl("PAY_COND_DAY").value();
                dr["PAY_DAY"] = this.rptRequest.Items[0].FindControl("PAY_DAY").value();
                dr["DIVIDE"] = this.rptObjMain.Items[0].FindControl("DIVIDE").value();

                dr["UPD_USER"] = this.Master.Master.EmployeeName;
                dr["UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                dr["UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
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
                strSQL = " select a.AUD_LVL_CODE,b.AUD_LVL_NAME,a.CASE_TYPE_CODE ,c.CASE_TYPE_NAME,a.AUD_CASE_TYPE ,d.CAL_NAME,";
                strSQL += " a.CASE_SOUR,e.DIVIDE,a.AGENT,a.OTHER ";
                strSQL += " from OR_CASE_APLY_APLY_COND  a left join OR_AUD_LVL_NAME  b on a.AUD_LVL_CODE=b.AUD_LVL_CODE ";
                strSQL += " left join OR_CASE_TYPE  c on a.CASE_TYPE_CODE=c.CASE_TYPE_CODE";
                strSQL += " left join OR_CASE_CAL  d on a.AUD_CASE_TYPE=d.CAL_NO";
                strSQL += " left join OR_CASE_APLY_BASE  e on a.APLY_NO=e.APLY_NO";


                strSQL += " where a.APLY_NO='" + strAplyNo + "'";
                myReader = dg.openSqlReader(strSQL);
                dt = new DataTable();
                dt.Load(myReader);
                dg.closeSqlReader(myReader);

                if (dt.Rows.Count == 0)
                {
                    dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }

                this.rptObjMain.DataSource = dt;
                this.rptObjMain.DataBind();


                strSQL = "  select OBJ_KEY=a.OBJ_CODE,a.OBJ_CODE,OBJ_LOC_ADDR,OLD_APLY_NO='',a.Actual_lessee,a.PROD_NAME,a.OTC,a.SPEC,a.BRAND,";
                strSQL += " a.OBJ_DUE_OWNER,a.MAC_NO,a.YEAR,a.CAR_NO,a.REAL_BUY_PRC,a.BUDG_LEASE_AMT,a.SELF_RATE,a.Is_spec_repo,a.BUDG_LEASE,a.BUY_RATE,a.OBJ_ASUR_TYPE,";
                strSQL += " a.INV_AMT_I_IB,a.RV_AMT,a.OBJ_LOC_TEL,a.OBJ_LOC_FAX,a.OBJ_LOC_CTAC,a.BUY_WAY,a.BUY_PROMISE,a.FRC_CODE,c.FRC_SNAME,";
                strSQL += " a.SALES_UNIT,a.OBJ_ONUS";
                strSQL += " from OR_OBJECT  a left join OR_CASE_APLY_OBJ b on a.OBJ_CODE=b.OBJ_CODE ";
                strSQL += " left join OR_FRC c on a.FRC_CODE=c.FRC_CODE";
                strSQL += " where b.APLY_NO='" + strAplyNo + "'";
                myReader = dg.openSqlReader(strSQL);
                dt = new DataTable();
                dt.Load(myReader);
                dg.closeSqlReader(myReader);
                dr = dt.NewRow();
                dr["OBJ_KEY"] = "";
                dr["OBJ_CODE"] = "";
                dr["OLD_APLY_NO"] = "";
                dr["PROD_NAME"] = "";
                dr["FRC_CODE"] = "";
                dr["SALES_UNIT"] = "";


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

                strSQL = " select RV_AMT_SUM=sum(RV_AMT), ";
                strSQL += " AP_SUM=sum(case OTC when 'Y' then REAL_BUY_PRC else 0 end), ";
                strSQL += " SL_SUM=sum(case OTC when 'Y' then BUDG_LEASE_AMT else 0 end), ";
                strSQL += " IIB_SUM=sum(case OTC when 'Y' then INV_AMT_I_IB else 0 end) from or_object  a left join OR_CASE_APLY_OBJ b on a.OBJ_CODE=b.OBJ_CODE ";
                strSQL += " where b.APLY_NO='" + strAplyNo + "'";
                myReader = dg.openSqlReader(strSQL);
                dt = new DataTable();
                dt.Load(myReader);
                dg.closeSqlReader(myReader);
                if (dt.Rows.Count == 0)
                {
                    dr = dt.NewRow();
                    dr["RV_AMT_SUM"] = 0;
                    dr["AP_SUM"] = 0;
                    dr["SL_SUM"] = 0;
                    dr["IIB_SUM"] = 0;
                    dt.Rows.Add(dr);
                }

                this.rptObjDetail_Sum.DataSource = dt;
                this.rptObjDetail_Sum.DataBind();



                strSQL = " select  OBJ_KEY=a.OBJ_CODE,a.OBJ_CODE,a.ACCS_NAME,a.ACCS_SEQ from OR_ACCS a left join OR_CASE_APLY_OBJ b on a.OBJ_CODE=b.OBJ_CODE  ";
                strSQL += " where b.APLY_NO='" + strAplyNo + "'";
                myReader = dg.openSqlReader(strSQL);
                dt = new DataTable();
                dt.Load(myReader);
                dg.closeSqlReader(myReader);

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
                dt = dts.GetTable("OR_CASE_APLY_APLY_COND", "APLY_NO='" + strAplyNo + "'");
                if (dt.Rows.Count == 0)
                {

                    dr = dt.NewRow();
                    dr["APLY_NO"] = strAplyNo;
                    dr["KEYIN_USER"] = this.Master.Master.EmployeeName;
                    dr["KEYIN_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                    dr["KEYIN_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                }
                else
                    dr = dt.Rows[0];


                dr["AUD_LVL_CODE"] = this.rptObjMain.Items[0].FindControl("AUD_LVL_CODE").value();
                dr["CASE_TYPE_CODE"] = this.rptObjMain.Items[0].FindControl("CASE_TYPE_CODE").value();
                dr["AUD_CASE_TYPE"] = this.rptObjMain.Items[0].FindControl("AUD_CASE_TYPE").value();
                dr["CASE_SOUR"] = this.rptObjMain.Items[0].FindControl("CASE_SOUR").value();

                dr["AGENT"] = this.rptObjMain.Items[0].FindControl("AGENT").value();
                dr["OTHER"] = this.rptObjMain.Items[0].FindControl("OTHER").value();


                dr["UPD_USER"] = this.Master.Master.EmployeeName;
                dr["UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                dr["UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                if (dt.Rows.Count == 0)
                    dt.Rows.Add(dr);


                if (!(this.STS == "L"))
                {
                    string strCodeKey = "";
                    int intCNT = this.dg.GetDataRow("select count(*)  from OR_CASE_APLY_OBJ where APLY_NO='" + strAplyNo + "'")[0].ToString().toInt();
                    for (int i = 0; i < this.GridObject.Rows.Count; i++)
                    {
                        if (this.GridObject.Rows[i].RowState != DataRowState.Deleted)
                        {
                            string strKey = this.GridObject.Rows[i]["OBJ_KEY"].ToString();
                            string strCode = strKey;
                            if (strCode != "")
                            {
                                if (this.dg.GetDataRow("select 'Y' from OR_OBJECT where OBJ_CODE='" + strCode + "'")[0].ToString() != "Y")
                                {
                                    intCNT++;
                                    strCode = strAplyNo.Substring(0, 4) + (intCNT + 1).ToString().PadLeft(3, '0') + strAplyNo.Substring(6, 4) + strAplyNo.Substring(11, 3);
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
                    DataTable dtCopy;
                    if (strCodeKey != "")
                    {
                        DataTable dtOBJECTC = this.dts.GetTable("OR_CASE_APLY_OBJ", "APLY_NO='" + strAplyNo + "' and OBJ_CODE in (" + strCodeKey + ")");
                        dtCopy = dtOBJECTC.Copy();
                        dtOBJECTC.DeleteRows();
                        for (int i = 0; i < this.GridObject.Rows.Count; i++)
                        {
                            if (this.GridObject.Rows[i].RowState != DataRowState.Deleted)
                            {
                                string strCode = this.GridObject.Rows[i]["OBJ_CODE"].ToString();
                                DataRow[] drCopy = dtCopy.Select("OBJ_CODE='" + strCode + "'");
                                if (strCode == "")
                                    continue;

                                //string strKeyChk = strCode.Substring(0, 3).ToUpper();

                                //  dtOBJECTC.DeleteRows("APLY_NO='" + strAplyNo + "' and OBJ_CODE='" + strCode + "'");
                                dr = dtOBJECTC.NewRow();
                                dr["APLY_NO"] = strAplyNo;
                                dr["OBJ_CODE"] = strCode;

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
                                dtOBJECTC.Rows.Add(dr);
                            }
                        }
                        DataTable dtOBJECT = this.dts.GetTable("OR_OBJECT", "OBJ_CODE in (" + strCodeKey + ")");
                        dtCopy = dtOBJECT.Copy();
                        for (int i = 0; i < this.GridObject.Rows.Count; i++)
                        {
                            if (this.GridObject.Rows[i].RowState != DataRowState.Deleted)
                            {
                                string strCode = this.GridObject.Rows[i]["OBJ_CODE"].ToString();

                                if (strCode == "")
                                    continue;

                                DataRow[] drCopy = dtCopy.Select("OBJ_CODE='" + strCode + "'");
                                string strKeyChk = strCode.Substring(0, 3).ToUpper();
                                if (strKeyChk != "OTC" && strKeyChk != "ORX" && strKeyChk != "XRC")
                                {
                                    dtOBJECT.DeleteRows("OBJ_CODE='" + strCode + "'");

                                    dr = dtOBJECT.NewRow();
                                    dr["OBJ_CODE"] = strCode;
                                    dr["OBJ_LOC_ADDR"] = this.GridObject.Rows[i]["OBJ_LOC_ADDR"].ToString();
                                    dr["Actual_lessee"] = this.GridObject.Rows[i]["Actual_lessee"].ToString();
                                    dr["PROD_NAME"] = this.GridObject.Rows[i]["PROD_NAME"].ToString();
                                    dr["OTC"] = this.GridObject.Rows[i]["OTC"].ToString();
                                    dr["SPEC"] = this.GridObject.Rows[i]["SPEC"].ToString();
                                    dr["BRAND"] = this.GridObject.Rows[i]["BRAND"].ToString();
                                    dr["OBJ_DUE_OWNER"] = this.GridObject.Rows[i]["OBJ_DUE_OWNER"].ToString();
                                    dr["MAC_NO"] = this.GridObject.Rows[i]["MAC_NO"].ToString();
                                    dr["YEAR"] = this.GridObject.Rows[i]["YEAR"].ToString();
                                    dr["CAR_NO"] = this.GridObject.Rows[i]["CAR_NO"].ToString();
                                    dr["REAL_BUY_PRC"] = this.GridObject.Rows[i]["REAL_BUY_PRC"].ToString().toNumber();
                                    dr["BUDG_LEASE_AMT"] = this.GridObject.Rows[i]["BUDG_LEASE_AMT"].ToString().toNumber();
                                    dr["SELF_RATE"] = this.GridObject.Rows[i]["SELF_RATE"].ToString().toNumber();
                                    dr["Is_spec_repo"] = this.GridObject.Rows[i]["Is_spec_repo"].ToString();
                                    dr["BUDG_LEASE"] = this.GridObject.Rows[i]["BUDG_LEASE"].ToString();
                                    dr["BUY_RATE"] = this.GridObject.Rows[i]["BUY_RATE"].ToString().toNumber();
                                    dr["OBJ_ASUR_TYPE"] = this.GridObject.Rows[i]["OBJ_ASUR_TYPE"].ToString();
                                    dr["INV_AMT_I_IB"] = this.GridObject.Rows[i]["INV_AMT_I_IB"].ToString().toNumber();
                                    dr["RV_AMT"] = this.GridObject.Rows[i]["RV_AMT"].ToString().toNumber();
                                    dr["OBJ_LOC_TEL"] = this.GridObject.Rows[i]["OBJ_LOC_TEL"].ToString();
                                    dr["OBJ_LOC_FAX"] = this.GridObject.Rows[i]["OBJ_LOC_FAX"].ToString();
                                    dr["OBJ_LOC_CTAC"] = this.GridObject.Rows[i]["OBJ_LOC_CTAC"].ToString();
                                    dr["BUY_WAY"] = this.GridObject.Rows[i]["BUY_WAY"].ToString();
                                    dr["BUY_PROMISE"] = this.GridObject.Rows[i]["BUY_PROMISE"].ToString();
                                    dr["FRC_CODE"] = this.GridObject.Rows[i]["FRC_CODE"].ToString();
                                    dr["SALES_UNIT"] = this.GridObject.Rows[i]["SALES_UNIT"].ToString();
                                    dr["OBJ_ONUS"] = this.GridObject.Rows[i]["OBJ_ONUS"].ToString();
                                    //  dr["RV_AMT_SUM"] = this.GridObject.Rows[i]["RV_AMT_SUM"].ToString();
                                    //  dr["AP_SUM"] = this.GridObject.Rows[i]["AP_SUM"].ToString();
                                    //  dr["SL_SUM"] = this.GridObject.Rows[i]["SL_SUM"].ToString();
                                    // dr["IIB_SUM"] = this.GridObject.Rows[i]["IIB_SUM"].ToString();

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
                        }
                    }


                    DataTable dtACCS = this.dts.GetTable("OR_ACCS", "OBJ_CODE in (" + strCodeKey + ")");
                    dtCopy = dtACCS.Copy();
                    for (int i = 0; i < this.GridAccs.Rows.Count; i++)
                    {
                        string strCode = this.GridAccs.Rows[i]["OBJ_CODE"].ToString();
                        if (strCode != "")
                        {
                            string strSeq = (i + 1).ToString();
                            DataRow[] drCopy = dtCopy.Select("OBJ_CODE='" + strCode + "' and ACCS_SEQ=" + strSeq);
                            string strKeyChk = strCode.Substring(0, 3).ToUpper();
                            if (strKeyChk != "OTC" && strKeyChk != "ORX" && strKeyChk != "XRC")
                            {
                                dtACCS.DeleteRows("OBJ_CODE='" + strCode + "' and ACCS_SEQ=" + strSeq);
                                dr = dtACCS.NewRow();
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
            SqlDataReader myReader = null;
            DataTable dt;
            DataRow dr;

            if (!bolSave)
            {


                strSQL = " select  PAY_COND_DAY,PAY_DAY,";
                strSQL += " b.APLY_BUY_AMT,b.APLY_RAKE,b.APLY_ANT_EXP,TOTAL=b.APLY_BUY_AMT+b.APLY_RAKE+b.APLY_ANT_EXP,";
                strSQL += " b.APLY_BOND,b.APLY_REST,b.APLY_PURS_TAX,b.APLY_PAY_PERD ,";
                strSQL += " b.APLY_DURN_M,b.APLY_PERD,b.ISU_FACTOR, b.APLY_SAVING_EXP,b.APLY_REAL_TR,";
                strSQL += " b.APLY_PAY_MTHD,b.APLY_DEPS,b.ISU_AMT,b.APLY_SURF_TR,";
                strSQL += " b.APLY_AMOR_MTHD,b.APLY_SERV_CHAR,b.APLY_TAX_ZERO,b.NON_FEAT_CHARGE,b.APLY_OTH_INT,";
                strSQL += " b.APLY_HIRE,b.APLY_TAX,";
                strSQL += " b.APLY_LF1_FR,b.APLY_LF1_TO,b.APLY_LF1_HIRE, ";
                strSQL += " b.APLY_LF2_FR,b.APLY_LF2_TO,b.APLY_LF2_HIRE,";
                strSQL += " b.APLY_LF3_FR,b.APLY_LF3_TO,b.APLY_LF3_HIRE,";
                strSQL += " b.APLY_LF4_FR,b.APLY_LF4_TO,b.APLY_LF4_HIRE,";
                strSQL += " b.APLY_LF5_FR,b.APLY_LF5_TO,b.APLY_LF5_HIRE,";
                strSQL += " b.APLY_INCM_TOL,b.APLY_SELL_TAX,APLY_MARG,Auth_Cond_Remark,OTHER_CONDITION,FINANCIAL_PURPOSE,EXPECT_AR_DATE";
                strSQL += " from OR_CASE_APLY_BASE  a";
                strSQL += " left join OR_CASE_APLY_APLY_COND b on a.APLY_NO=b.APLY_NO";
                strSQL += " where a.APLY_NO='" + strAplyNo + "'";
                dt = dg.GetDataTable(strSQL);

                if (dt.Rows.Count == 0)
                {
                    dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }

                this.rptRequest.DataSource = dt;
                this.rptRequest.DataBind();





                double APLY_BUY_AMT = this.rptRequest.Items[0].FindControl("APLY_BUY_AMT").value().toNumber();


                strSQL = " select PERIOD,HIRE,DIVD,CAPT_AMOR,REST=0 from OR_CASE_APLY_SHARE_DET ";
                strSQL += " where APLY_NO='" + strAplyNo + "'";

                dt = dg.GetDataTable(strSQL);

                double amt = APLY_BUY_AMT;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["REST"] = amt - dt.Rows[i]["CAPT_AMOR"].ToString().toNumber();
                    amt = amt - dt.Rows[i]["CAPT_AMOR"].ToString().toNumber();
                }
                this.rptShare.DataSource = dt;
                this.rptShare.DataBind();


                string strScript = "";
                strScript += "document.getElementById('rptShare_HIRE').innerHTML='" + dt.Compute("sum(HIRE)", "").ToString().toNumber().ToString("###,###,##0") + "';\n";
                strScript += "document.getElementById('rptShare_DIVD').innerHTML='" + dt.Compute("sum(DIVD)", "").ToString().toNumber().ToString("###,###,##0") + "';\n";


                strSQL = " select PERIOD,HIRE=REAL_HIRE,DIVD=REAL_DIVD,CAPT_AMOR=REAL_CAPT_AMOR,REST=0 from OR_CASE_APLY_SHARE_DET ";
                strSQL += " where APLY_NO='" + strAplyNo + "'";
                dt = dg.GetDataTable(strSQL);

                amt = APLY_BUY_AMT;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["REST"] = amt - dt.Rows[i]["CAPT_AMOR"].ToString().toNumber();
                    amt = amt - dt.Rows[i]["CAPT_AMOR"].ToString().toNumber();
                }

                this.rptShareReal.DataSource = dt;
                this.rptShareReal.DataBind();

                strScript += "document.getElementById('rptShareReal_HIRE').innerHTML='" + dt.Compute("sum(HIRE)", "").ToString().toNumber().ToString("###,###,##0") + "';\n";
                strScript += "document.getElementById('rptShareReal_DIVD').innerHTML='" + dt.Compute("sum(DIVD)", "").ToString().toNumber().ToString("###,###,##0") + "';\n";
                this.setScript(strScript);

                strSQL = " select PERIOD,HIRE=EX_HIRE,DIVD=EX_DIVD,CAPT_AMOR=EX_CAPT_AMOR,REST=0,TOTAL=0 from OR_CASE_APLY_SHARE_DET ";
                strSQL += " where APLY_NO='" + strAplyNo + "'";
                dt = dg.GetDataTable(strSQL);
                amt = APLY_BUY_AMT;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["REST"] = amt - dt.Rows[i]["CAPT_AMOR"].ToString().toNumber();
                    amt = amt - dt.Rows[i]["CAPT_AMOR"].ToString().toNumber();
                }
                this.rptShareEx.DataSource = dt;
                this.rptShareEx.DataBind();

            }
            else
            {
                DataTable dtRequest = dts.GetTable("OR_CASE_APLY_APLY_COND", "APLY_NO='" + strAplyNo + "'");


                if (dtRequest.Rows.Count == 0)
                {

                    dr = dtRequest.NewRow();
                    dr["APLY_NO"] = strAplyNo;
                    dr["KEYIN_USER"] = this.Master.Master.EmployeeName;
                    dr["KEYIN_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                    dr["KEYIN_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                }
                else
                    dr = dtRequest.Rows[0];




                dr["APLY_BUY_AMT"] = this.rptRequest.Items[0].FindControl("APLY_BUY_AMT").value();
                dr["APLY_RAKE"] = this.rptRequest.Items[0].FindControl("APLY_RAKE").value();
                dr["APLY_ANT_EXP"] = this.rptRequest.Items[0].FindControl("APLY_ANT_EXP").value();
                dr["APLY_BOND"] = this.rptRequest.Items[0].FindControl("APLY_BOND").value();
                dr["APLY_REST"] = this.rptRequest.Items[0].FindControl("APLY_REST").value();
                dr["APLY_PURS_TAX"] = this.rptRequest.Items[0].FindControl("APLY_PURS_TAX").value();
                dr["APLY_PAY_PERD"] = this.rptRequest.Items[0].FindControl("APLY_PAY_PERD").value();
                dr["APLY_DURN_M"] = this.rptRequest.Items[0].FindControl("APLY_DURN_M").value();
                dr["APLY_PERD"] = this.rptRequest.Items[0].FindControl("APLY_PERD").value();
                dr["ISU_FACTOR"] = this.rptRequest.Items[0].FindControl("ISU_FACTOR").value();
                dr["APLY_SAVING_EXP"] = this.rptRequest.Items[0].FindControl("APLY_SAVING_EXP").value();
                dr["APLY_REAL_TR"] = this.rptRequest.Items[0].FindControl("APLY_REAL_TR").value();
                dr["APLY_PAY_MTHD"] = this.rptRequest.Items[0].FindControl("APLY_PAY_MTHD").value();
                dr["APLY_DEPS"] = this.rptRequest.Items[0].FindControl("APLY_DEPS").value();
                dr["ISU_AMT"] = this.rptRequest.Items[0].FindControl("ISU_AMT").value();
                dr["APLY_SURF_TR"] = this.rptRequest.Items[0].FindControl("APLY_SURF_TR").value();
                dr["APLY_AMOR_MTHD"] = this.rptRequest.Items[0].FindControl("APLY_AMOR_MTHD").value();
                dr["APLY_SERV_CHAR"] = this.rptRequest.Items[0].FindControl("APLY_SERV_CHAR").value();
                dr["APLY_TAX_ZERO"] = this.rptRequest.Items[0].FindControl("APLY_TAX_ZERO").value();
                dr["NON_FEAT_CHARGE"] = this.rptRequest.Items[0].FindControl("NON_FEAT_CHARGE").value();
                dr["APLY_OTH_INT"] = this.rptRequest.Items[0].FindControl("APLY_OTH_INT").value();
                dr["APLY_HIRE"] = this.rptRequest.Items[0].FindControl("APLY_HIRE").value();
                dr["APLY_TAX"] = this.rptRequest.Items[0].FindControl("APLY_TAX").value();
                dr["APLY_LF1_FR"] = this.rptRequest.Items[0].FindControl("APLY_LF1_FR").value();
                dr["APLY_LF1_TO"] = this.rptRequest.Items[0].FindControl("APLY_LF1_TO").value();
                dr["APLY_LF1_HIRE"] = this.rptRequest.Items[0].FindControl("APLY_LF1_HIRE").value();
                dr["APLY_LF2_FR"] = this.rptRequest.Items[0].FindControl("APLY_LF2_FR").value();
                dr["APLY_LF2_TO"] = this.rptRequest.Items[0].FindControl("APLY_LF2_TO").value();
                dr["APLY_LF2_HIRE"] = this.rptRequest.Items[0].FindControl("APLY_LF2_HIRE").value();
                dr["APLY_LF3_FR"] = this.rptRequest.Items[0].FindControl("APLY_LF3_FR").value();
                dr["APLY_LF3_TO"] = this.rptRequest.Items[0].FindControl("APLY_LF3_TO").value();
                dr["APLY_LF3_HIRE"] = this.rptRequest.Items[0].FindControl("APLY_LF3_HIRE").value();
                dr["APLY_LF4_FR"] = this.rptRequest.Items[0].FindControl("APLY_LF4_FR").value();
                dr["APLY_LF4_TO"] = this.rptRequest.Items[0].FindControl("APLY_LF4_TO").value();
                dr["APLY_LF4_HIRE"] = this.rptRequest.Items[0].FindControl("APLY_LF4_HIRE").value();
                dr["APLY_LF5_FR"] = this.rptRequest.Items[0].FindControl("APLY_LF5_FR").value();
                dr["APLY_LF5_TO"] = this.rptRequest.Items[0].FindControl("APLY_LF5_TO").value();
                dr["APLY_LF5_HIRE"] = this.rptRequest.Items[0].FindControl("APLY_LF5_HIRE").value();
                dr["APLY_INCM_TOL"] = this.rptRequest.Items[0].FindControl("APLY_INCM_TOL").value();
                dr["APLY_SELL_TAX"] = this.rptRequest.Items[0].FindControl("APLY_SELL_TAX").value();
                dr["APLY_MARG"] = this.rptRequest.Items[0].FindControl("APLY_MARG").value();
                // dr["DIVIDE"] = this.rptObjMain.Items[0].FindControl("DIVIDE").value();

                dr["UPD_USER"] = this.Master.Master.EmployeeName;
                dr["UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                dr["UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                if (dtRequest.Rows.Count == 0)
                    dtRequest.Rows.Add(dr);





                DataTable dtShare = dts.GetTable("OR_CASE_APLY_SHARE_DET", "APLY_NO='" + strAplyNo + "'");
                DataRow[] drCopy;
                DataTable dtCopy = dtShare.Copy();
                dtShare.DeleteRows();



                for (int i = 0; i < r.表面TR_T.Rows.Count; i++)
                {

                    string PERIOD = r.表面TR_T.Rows[i]["PERIOD"].ToString();
                    drCopy = dtCopy.Select("PERIOD='" + PERIOD + "'");


                    dr = dtShare.NewRow();
                    dr["HIRE"] = this.rptShare.Items[i].FindControl("HIRE").value();
                    dr["DIVD"] = r.表面TR_T.Rows[i]["DIVD"].ToString().toNumber();
                    dr["CAPT_AMOR"] = r.表面TR_T.Rows[i]["CAPT_AMOR"].ToString().toNumber();
                    dr["REAL_HIRE"] = r.實質TR_T.Rows[i]["HIRE"].ToString().toNumber();

                    dr["CAPT_AMOR"] = this.rptShare.Items[i].FindControl("CAPT_AMOR").value();
                    dr["REAL_HIRE"] = this.rptShareReal.Items[i].FindControl("HIRE").value();
                    dr["REAL_DIVD"] = this.rptShareReal.Items[i].FindControl("DIVD").value();
                    dr["REAL_CAPT_AMOR"] = this.rptShareReal.Items[i].FindControl("CAPT_AMOR").value();
                    dr["EX_HIRE"] = this.rptShareReal.Items[i].FindControl("HIRE").value();
                    dr["EX_DIVD"] = this.rptShareReal.Items[i].FindControl("DIVD").value();
                    dr["EX_CAPT_AMOR"] = this.rptShareReal.Items[i].FindControl("CAPT_AMOR").value();

                    /*   if (drCopy.Length > 0)
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
                    dtShare.Rows.Add(dr);

                }
            }


        }
        #endregion

        #region tab05 dataprocess 備註

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strType"></param>
        private void tab05_DataProcess(bool bolSave, string strAplyNo)
        {
            string strSQL = "";
            SqlDataReader myReader = null;
            DataTable dt;
            DataRow dr;

            if (!bolSave)
            {

                strSQL += " select REMARK,a.BANK_NO,b.BANK_NAME,INVO_REMARK,c.Add_User,Add_Date=dbo.f_DateAddSlash(c.Add_Date),c.Upd_User,Upd_Date=dbo.f_DateAddSlash(c.Upd_Date),ACC_NO  ";
                strSQL += " from or_request_remark a left join ACC18 b on a.BANK_NO=b.BANK_NO left join OTC_CASE_BASE_REM c ";
                strSQL += " on a.APLY_NO=c.APLY_NO where a.APLY_NO='" + strAplyNo + "'";
                myReader = dg.openSqlReader(strSQL);
                dt = new DataTable();
                dt.Load(myReader);
                dg.closeSqlReader(myReader);
                if (dt.Rows.Count == 0)
                {
                    dr = dt.NewRow();
                    dr["BANK_NO"] = "";
                    dr["Add_User"] = this.Master.Master.UserId;
                    dr["Add_Date"] = System.DateTime.Now.ToString("yyyy/MM/dd");


                    dt.Rows.Add(dr);
                }
                this.rptRemark.DataSource = dt;
                this.rptRemark.DataBind();

                if (((TextBox)this.rptRemark.Items[0].FindControl("ACCNO")).Text == "")
                {
                    //setting s_GetAccNo
                    ((TextBox)this.rptRemark.Items[0].FindControl("ACCNO")).Text = dg.GetDataRow("exec s_GetAccNo '" + strAplyNo + "'")[0].ToString();
                }
            }
            else
            {

                dt = dts.GetTable("or_request_remark", "APLY_NO='" + strAplyNo + "'");
                if (dt.Rows.Count == 0)
                {

                    dr = dt.NewRow();
                    dr["APLY_NO"] = strAplyNo;
                }
                else
                    dr = dt.Rows[0];

                dr["ACC_NO"] = this.rptRemark.Items[0].FindControl("ACCNO").value();
                dr["REMARK"] = this.rptRemark.Items[0].FindControl("REMARK").value();
                dr["BANK_NO"] = this.rptRemark.Items[0].FindControl("BANK_NO").value();
                dr["INVO_REMARK"] = this.rptRemark.Items[0].FindControl("INVO_REMARK").value();


                if (dt.Rows.Count == 0)
                    dt.Rows.Add(dr);

            }
        }
        #endregion


        #region tab04 dataprocess 確認內容

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

                strSQL += " select	SING_CON_DATE=dbo.f_DateAddSlash(SING_CON_DATE),PREDICT_LEASE_DATE=dbo.f_DateAddSlash(PREDICT_LEASE_DATE),first_pay_date=dbo.f_DateAddSlash(first_pay_date),";
                strSQL += " PREDICT_PAY_DATE=dbo.f_DateAddSlash(PREDICT_PAY_DATE),SUPPLY_PAY_WAY,PAY_SUPPLY_DUE_DATE=dbo.f_DateAddSlash(PAY_SUPPLY_DUE_DATE),pay_supply_tt_acc,PAY_AUPPY_AMT,";
                strSQL += " b.MMail_no,c.MMail_Name,IF_CON,CON_MAN,CON_DATE=dbo.f_DateAddSlash(CON_DATE),OTC_CON_MAN,CUS_PAY_WAY,selfuse,";
                strSQL += " supplybuy,EASYCASE,CHM_YN,DIS_CONTRACT,confirm_25to31,TEST='',REM_ORDER,REM_CUST_CONTRACT,REM_CUST_AGREEMENT,REM_CUST_INV,REM_FRC_ZEOX,";
                strSQL += " REM_CUST_OTH,REM_FRC_CONTRACT,REM_FRC_COOPAGREEMENT,REM_FRC_AGREEMENT,REM_FRC_INV,REM_FRC_OTH,MEM,CRD_CON_DATE=dbo.f_DateAddSlash(CRD_CON_DATE),REM_CUST_OTH_DESC,REM_FRC_OTH_DESC,ACC_MEM	 ";

                strSQL += " from OTC_CASE_BASE_REM a left join OR_CASE_APLY_BASE b on a.APLY_NO=b.APLY_NO left join OR_MERG_MAIL c on b.MMail_NO=c.MMail_NO WHERE a.APLY_NO='" + strAplyNo + "'";


                dt = dg.GetDataTable(strSQL);

                if (dt.Rows.Count == 0)
                {
                    dr = dt.NewRow();
                    dr["DIS_CONTRACT"] = "Y";
                    dt.Rows.Add(dr);
                }

                this.rptConfirm.DataSource = dt;
                this.rptConfirm.DataBind();


            }
            else
            {
                dt = dts.GetTable("OTC_CASE_BASE_REM", "APLY_NO='" + strAplyNo + "'");


                if (dt.Rows.Count == 0)
                {

                    dr = dt.NewRow();
                    dr["APLY_NO"] = strAplyNo;
                    dr["ADD_USER"] = this.rptRemark.Items[0].FindControl("ADD_USER").value();
                    dr["ADD_DATE"] = this.rptRemark.Items[0].FindControl("ADD_DATE").value().Replace("/", "");
                }
                else
                    dr = dt.Rows[0];



                dr["SING_CON_DATE"] = this.rptConfirm.Items[0].FindControl("SING_CON_DATE").value().Replace("/", "");
                dr["PREDICT_LEASE_DATE"] = this.rptConfirm.Items[0].FindControl("PREDICT_LEASE_DATE").value().Replace("/", "");
                dr["first_pay_date"] = this.rptConfirm.Items[0].FindControl("first_pay_date").value().Replace("/", "");
                dr["PREDICT_PAY_DATE"] = this.rptConfirm.Items[0].FindControl("PREDICT_PAY_DATE").value().Replace("/", "");
                dr["SUPPLY_PAY_WAY"] = this.rptConfirm.Items[0].FindControl("SUPPLY_PAY_WAY").value();
                dr["PAY_SUPPLY_DUE_DATE"] = this.rptConfirm.Items[0].FindControl("PAY_SUPPLY_DUE_DATE").value().Replace("/", "");
                dr["pay_supply_tt_acc"] = this.rptConfirm.Items[0].FindControl("pay_supply_tt_acc").value();
                dr["PAY_AUPPY_AMT"] = this.rptConfirm.Items[0].FindControl("PAY_AUPPY_AMT").value();
                // dr["MMail_no"] = this.rptConfirm.Items[0].FindControl("MMail_no").value();
                dr["CON_MAN"] = this.rptConfirm.Items[0].FindControl("CON_MAN").value();
                dr["MEM"] = this.rptConfirm.Items[0].FindControl("MEM").value();
                dr["IF_CON"] = this.rptConfirm.Items[0].FindControl("IF_CON").value();
                dr["CON_DATE"] = this.rptConfirm.Items[0].FindControl("CON_DATE").value().Replace("/", "");
                dr["OTC_CON_MAN"] = this.rptConfirm.Items[0].FindControl("OTC_CON_MAN").value();
                dr["CUS_PAY_WAY"] = this.rptConfirm.Items[0].FindControl("CUS_PAY_WAY").value();
                dr["selfuse"] = this.rptConfirm.Items[0].FindControl("selfuse").value();
                dr["supplybuy"] = this.rptConfirm.Items[0].FindControl("supplybuy").value();
                dr["EASYCASE"] = this.rptConfirm.Items[0].FindControl("EASYCASE").value();
                dr["CHM_YN"] = this.rptConfirm.Items[0].FindControl("CHM_YN").value();
                dr["DIS_CONTRACT"] = this.rptConfirm.Items[0].FindControl("DIS_CONTRACT").value();
                dr["confirm_25to31"] = this.rptConfirm.Items[0].FindControl("confirm_25to31").value();
                //  dr["TEST"] = this.rptConfirm.Items[0].FindControl("TEST").value();
                dr["REM_ORDER"] = this.rptConfirm.Items[0].FindControl("REM_ORDER").value();
                dr["REM_CUST_CONTRACT"] = this.rptConfirm.Items[0].FindControl("REM_CUST_CONTRACT").value();
                dr["REM_CUST_AGREEMENT"] = this.rptConfirm.Items[0].FindControl("REM_CUST_AGREEMENT").value();
                dr["REM_CUST_INV"] = this.rptConfirm.Items[0].FindControl("REM_CUST_INV").value();
                dr["REM_CUST_OTH"] = this.rptConfirm.Items[0].FindControl("REM_CUST_OTH").value();
                dr["REM_FRC_CONTRACT"] = this.rptConfirm.Items[0].FindControl("REM_FRC_CONTRACT").value();
                //  dr["REM_FRC_COOPAGREEMENT"] = this.rptConfirm.Items[0].FindControl("REM_FRC_COOPAGREEMENT").value();
                dr["REM_FRC_AGREEMENT"] = this.rptConfirm.Items[0].FindControl("REM_FRC_AGREEMENT").value();
                dr["REM_FRC_INV"] = this.rptConfirm.Items[0].FindControl("REM_FRC_INV").value();
                dr["REM_FRC_OTH"] = this.rptConfirm.Items[0].FindControl("REM_FRC_OTH").value();
                dr["CRD_CON_DATE"] = this.rptConfirm.Items[0].FindControl("CRD_CON_DATE").value().Replace("/", "");
                dr["REM_CUST_OTH_DESC"] = this.rptConfirm.Items[0].FindControl("REM_CUST_OTH_DESC").value();
                dr["REM_FRC_OTH_DESC"] = this.rptConfirm.Items[0].FindControl("REM_FRC_OTH_DESC").value();
                dr["REM_FRC_ZEOX"] = this.rptConfirm.Items[0].FindControl("REM_FRC_ZEOX").value();
                dr["ACC_MEM"] = this.rptConfirm.Items[0].FindControl("ACC_MEM").value();
                dr["UPD_USER"] = this.Master.Master.UserId;
                dr["UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");

                if (dt.Rows.Count == 0)
                    dt.Rows.Add(dr);
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


            this.bolSave = true;

            string strAplyNo = "";

            string strDEPT = this.rptBase.Items[0].FindControl("DEPT_CODE").value();
            string strDate = this.rptBase.Items[0].FindControl("APLY_DATE").value().Replace("/", "");

            if (strStatus == "Add")
            {
                string STS = dg.GetDataRow("select CASE_STS from OR_CASE_TYPE where CASE_TYPE_CODE='" + this.rptObjMain.Items[0].FindControl("CASE_TYPE_CODE").value() + "'")[0].ToString();
                strAplyNo = dg.GetDataRow("exec s_GetNumber 'A','" + strDEPT + "" + STS + "','" + strDate.Substring(0, 4) + "','" + strDate.Substring(4, 2) + "'")[0].ToString();
            }
            else
                strAplyNo = this.APLY_NO.Text.rpsText();

            DataTable dtBase = dts.GetTable("OR_CASE_APLY_BASE", "APLY_NO='" + strAplyNo + "'");
            DataRow dr = null;

            switch (strStatus)
            {

                case "Add":
                case "Copy":
                case "Upd":
                case "UpdAfter":


                    //攤提試算                                      

                    this.tab05_DataProcess(true, strAplyNo);
                    cts.Execute("update OTC_case_base_rem set UPD_USER='" + this.Master.Master.UserId + "',UPD_DATE='" + System.DateTime.Now.ToString("yyyyMMdd") + "' where APLY_NO='" + strAplyNo + "'");


                  /*  if (this.STSCODE.CompareTo("9") > 0 && this.STSCODE != "R")
                    {
                        string dt = dg.GetDataRow("select convert(char(10),dateadd(day,60,ADD_DATE),111) from OTC_CASE_BASE_REM where APLY_NO='" + strAplyNo + "'")[0].ToString().Trim();
                        if (dt.CompareTo(System.DateTime.Now.ToString("yyyy/MM/dd")) < 0)
                        {
                            ArrayList al = new ArrayList();
                            al.Add("update OR_CASE_APLY_BASE set MMAIL_NO='" + this.rptConfirm.Items[0].FindControl("MMail_NO").value() + "' where APLY_NO='" + strAplyNo + "'");
                            al.Add("update OTC_CASE_BASE_REM set CHM_YN='" + this.rptConfirm.Items[0].FindControl("CHM_YN").value() + "' ,CUS_PAY_WAY='" + this.rptConfirm.Items[0].FindControl("CUS_PAY_WAY").value() + "' where APLY_NO='" + strAplyNo + "'");

                            cts.Execute(al);
                            this.setMessageBox("除客戶付款方式, 收整組票, 統一郵寄外, 其餘不可更改!");
                            return 0;
                        }
                    }
                    else*/

                    string dt = dg.GetDataRow("select isnull((convert(char(10),dateadd(day,60,ADD_DATE),111)),'') from OTC_CASE_BASE_REM where APLY_NO='" + strAplyNo + "'")[0].ToString().Trim();
                     if (this.STSCODE.CompareTo("9") > 0 && this.STSCODE != "R" && dt.CompareTo(System.DateTime.Now.ToString("yyyy/MM/dd")) < 0 && dt!="")
                    {
                        ArrayList al = new ArrayList();
                        al.Add("update OR_CASE_APLY_BASE set MMAIL_NO='" + this.rptConfirm.Items[0].FindControl("MMail_NO").value() + "' where APLY_NO='" + strAplyNo + "'");
                        al.Add("update OTC_CASE_BASE_REM set CHM_YN='" + this.rptConfirm.Items[0].FindControl("CHM_YN").value() + "' ,CUS_PAY_WAY='" + this.rptConfirm.Items[0].FindControl("CUS_PAY_WAY").value() + "' where APLY_NO='" + strAplyNo + "'");
 
                        cts.Execute(al);
                        this.setMessageBox("除客戶付款方式, 收整組票, 統一郵寄外, 其餘不可更改!");
                        return 0;
                    }
                    else   // 存檔
                    {
                        //this.SaveObjectDetail(this.bolGridAdd);
                        this.tab01_DataProcess(true, strAplyNo);
                        this.tab02_DataProcess(true, strAplyNo);


                        this.tab04_DataProcess(true, strAplyNo);


                        if (this.rptConfirm.Items[0].FindControl("IMPORT_YN").value() == "Y")
                        {
                            // 存檔時 , 符合正常存檔的條件時 (即 <實行中+60天)
                            //若勾選 轉入資金預撥 另對OTC_DB做動作 (測試環境為 ORIX_TEST 資料庫)


                            bool SOTCInsert = false;
                            bool SOTCDel = false;
                            DataSetToSql dtsO = new DataSetToSql("orixConn");
                            DataGetting dgO = new DataGetting("orixConn");
                            DataRow drOTC = dgO.GetDataRow("select isnull(資金課確認日,'') as dt1,資金課確認日 as dt2 from expallot where 肯美='Y' and isnull(標示刪除,'N') ='N' and 合約編號='" + this.APLY_NO.Text.Trim() + "'");
                            if (drOTC["dt1"].ToString().Trim() == "" || drOTC["dt2"].ToString().Trim() == "")
                            {
                                this.setScript("checkOTC_Insert()");
                                dts.Save();
                                return 0;
                            }


                        }

                    }




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

        protected void saveOTC(object sender ,EventArgs e)
        {
            if (this.checkOTC.Text == "Y")
            {
                this.AP_OTCInsert(true);
            }
            else{
                this.setProcessMessage(this.Master.nowStatusName + "處理成功!!", false);
                this.Master.returnPage();
            }
        }

        private void AP_OTCInsert(bool bolDel)
        {
            string LSql = "";
            string L客戶簡稱 = "";
            string L付款方式 = "";
            string L供應商 = "";
            string L預撥日期 = this.rptConfirm.Items[0].FindControl("PAY_SUPPLY_DUE_DATE").value();
            DataSetToSql dtsO = new DataSetToSql("orixConn");
            CommandToSql cts = new CommandToSql("orixConn");
            DataTable dtO = dtsO.GetTable("expallot", "1=2");

            if (bolDel)
            {
                cts.Execute("update expallot  WITH (ROWLOCK,UPDLOCK) set 標示刪除='Y' where 肯美='Y' and isnull(標示刪除,'N') ='N' and 合約編號='" + this.APLY_NO.Text + "'");
            }

            L客戶簡稱 = dg.GetDataRow("select CUST_SNAME from OR_CUSTOM where CUST_NO='" + this.rptBase.Items[0].FindControl("CUST_NO").value() + "'")[0].ToString();

            switch (this.rptConfirm.Items[0].FindControl("SUPPLY_PAY_WAY").value())
            {
                case "1":
                    L付款方式 = "期票";
                    L預撥日期 = this.rptConfirm.Items[0].FindControl("PREDICT_PAY_DATE").value();
                    break;
                case "2":
                    L付款方式 = "電匯";
                    L預撥日期 = this.rptConfirm.Items[0].FindControl("PAY_SUPPLY_DUE_DATE").value();
                    break;
                case "3":
                    L付款方式 = "電匯";
                    L預撥日期 = this.rptConfirm.Items[0].FindControl("PAY_SUPPLY_DUE_DATE").value();
                    break;
                case "4":
                    L付款方式 = "電匯";
                    L預撥日期 = this.rptConfirm.Items[0].FindControl("PAY_SUPPLY_DUE_DATE").value();
                    break;
            }
            L供應商 = dg.GetDataRow("select FRC_SNAME from OR_FRC where FRC_CODE='" + this.GridObject.Rows[0]["FRC_CODE"].ToString() + "'")[0].ToString();

            DataRow dr = dtO.NewRow();
            dr["公司別"] = "VPIT";
            dr["帳號"] = this.Master.Master.CorpAcct;
            dr["建檔人"] = this.Master.Master.EmployeeName;
            dr["合約編號"] = this.APLY_NO.Text;
            dr["戶名"] = L客戶簡稱;
            dr["本次預撥金額"] = this.rptConfirm.Items[0].FindControl("PAY_AUPPY_AMT").value();
            if (L預撥日期!="")
                dr["預撥日期"] = L預撥日期;

            if (L付款方式 != "")
            dr["付款方式"] = L付款方式;
            if (this.rptConfirm.Items[0].FindControl("PAY_SUPPLY_DUE_DATE").value()!="")
                dr["付款日"] = this.rptConfirm.Items[0].FindControl("PAY_SUPPLY_DUE_DATE").value();

            if (this.rptConfirm.Items[0].FindControl("PREDICT_PAY_DATE").value() != "")
                dr["作業日"] = this.rptConfirm.Items[0].FindControl("PREDICT_PAY_DATE").value(); ;
            dr["備註"] = "";
            dr["營業員"] = this.rptBase.Items[0].FindControl("EMP_NAME").value();
            dr["供應商"] = L供應商;
            dr["標示刪除"] = "N";
            dr["計張"] = "N";
            dr["肯美"] = "Y";

            if (this.rptConfirm.Items[0].FindControl("SING_CON_DATE").value() != "")
                dr["起租日"] = this.rptConfirm.Items[0].FindControl("SING_CON_DATE").value();

            dtO.Rows.Add(dr);
            if (!dtsO.Save())
            {
                this.setProcessMessage(this.Master.nowStatusName + "處理失敗!", false);
                this.Master.returnPage();
            }
            else
            {
                this.setProcessMessage(this.Master.nowStatusName + "處理成功!!", false);
                this.Master.returnPage();
            }
        }

        /*
       
 
 
 private void AP_OTCInsert(){
     string LSql="";
     string L客戶簡稱="";
     string L付款方式="";
     string L供應商="";
     string L預撥日期="";
      DataSetToSql dtsO = new DataSetToSql("orixConn");
                    DataTable dtO=dtsO.GetTable("Service", "1=2");

 }

Public Sub Ap_OTCInsert()
    Dim LSql As String
    Dim LRec As New ADODB.Recordset
    Dim L客戶簡稱 As String
    Dim L付款方式 As String
    Dim L供應商 As String   
    Dim L預撥日期 As String   
    Dim LConnQ As New ADODB.Connection
    Set LConnQ = CSys_ADOConn(SDsn(3), SProgram_Id) ‘ (ORIX_TEST DB)
   
    '20120711
    If SOTCDel Then
                LSql = "update expallot  WITH (ROWLOCK,UPDLOCK) set 標示刪除='Y' where 肯美='Y' and isnull(標示刪除,'N') ='N' and 合約編號='" &畫面APLY_NO & "'"
                LConnQ.Execute LSql
    End If   
   
   
    LSql = "select CUST_SNAME from or_custom where cust_no='" & 畫面客戶代號 & "'"
    LRec.Open LSql, SConn, adOpenStatic, adLockReadOnly
    L客戶簡稱 = CSys_NullCheck(LRec("CUST_SNAME"))
    LRec.Close
   
    Select Case Screen.ActiveForm.DataField(CSUPPLY_PAY_WAY).Text   ‘付供應商方式
        Case "1"
            L付款方式 = "期票"
            L預撥日期 = Format(預計附供應商日), "####/##/##")
        Case "2"
            L付款方式 = "電匯"
            L預撥日期 = Format(付供應商票期), "####/##/##")
        Case "3"
            L付款方式 = "電匯"
            L預撥日期 = Format(付供應商票期), "####/##/##")
        Case "4"
            L付款方式 = "電匯"
            L預撥日期 = Format(付供應商票期), "####/##/##")
    End Select
   
    L供應商 = ""
   'GIRD第一筆標的物之供應商
   If GIRD第一筆標的物代號 <> 空白
        LSql = "select FRC_CODE ,FRC_SNAME  from OR_FRC where FRC_CODE='第一比標的物之供應商代號'
        LRec.Open LSql, SConn, adOpenStatic, adLockReadOnly
        L供應商 = CSys_NullCheck(LRec("FRC_SNAME"))
        LRec.Close
    End If
       
    LSql = "insert into expallot (公司別,帳號,建檔人,合約編號,戶名,本次預撥金額,預撥日期,付款方式,付款日,作業日,備註,營業員,供應商,
標示刪除,計張,肯美,起租日) "
    LSql = LSql & "values('VPIT','" & SUser_Id & "'"
    LSql = LSql & ",'" & SUser_Name & "'"
    LSql = LSql & ",'" & 畫面.APLY_NO & "'"
    LSql = LSql & ",'" & L客戶簡稱 & "'"
    LSql = LSql & "," & 畫面.付供應商金額 & ""
    LSql = LSql & ",'" & L預撥日期 & "'"
    LSql = LSql & ",'" & L付款方式 & "'"
    LSql = LSql & ",'" & Format(畫面.付供應商票期), "####/##/##") & "'"
    LSql = LSql & ",'" & Format(畫面.預計附供應商日), "####/##/##") & "'"
    LSql = LSql & ",' '"    ' 備註不轉
    LSql = LSql & ",'" & 畫面.經辦姓名 & "'"
    LSql = LSql & ",'" & L供應商 & "'"
    LSql = LSql & ",'N','N','Y'"
    LSql = LSql & ",'" & Format(畫面.預計起租日), "####/##/##") & "'"   
    LSql = LSql & ")"
       
    LConnQ.Execute LSql
    LConnQ.Close
   
End Sub
        */

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


        #region tr / 攤提
        public struct Rtn
        {
            public DataTable 表面TR_T;
            public DataTable 實質TR_T;
            public DataTable 其他_T;
            public double 其他利息;
            public double 表面TR;
            public double 實質TR;
            public double 表面租金合計;
            public double 實質租金合計;
            public double 表面利息合計;
            public double 實質利息合計;
            public double 收入總額;
            public double 銷項稅額;
            public double 毛收益;
        }
        /*
        public struct Rtn2
        {
            public DataTable 表面TR_T;
            public DataTable 實質TR_T;
            public DataTable 其他_T;
            public double 表面租金合計;
            public double 實質租金合計;
            public double 表面利息合計;
            public double 實質利息合計;
            public double 收入總額;
            public double 銷項稅額;
            public double 毛收益;
        }
        */
        public Rtn 攤提試算(List<long> 租金,
                            int 付款條件天數,
                            int 票期天數,
                            int 付供應商天數,
                            long 購買額,
                            long 佣金,
                            long 其他費用,
                            long 保證金,
                            long 殘值,
                            int 付款週期,
                            int 期數,
                            double 保險因子,
                            long 保留款,
                            string 繳款方式,
                            long 頭期款,
                            long 保險費,
                            long 手續費,
                            string 稅別)
        {

            double[] dVal = new double[期數 - (繳款方式 == "2" ? 0 : 1) + 2];
            double[] dVal2 = new double[期數 - (繳款方式 == "2" ? 0 : 1) + 2];
            double[] dVal3 = new double[期數 - (繳款方式 == "2" ? 0 : 1) + 2];
            double dTR;
            double d本金;

            d本金 = 購買額 - 頭期款 - 手續費 - 保證金 + 保險費 + 佣金 + 其他費用 + 保留款;

            Rtn b = new Rtn();

            b.表面TR_T = new DataTable("表面TR_T");

            b.表面TR_T.Columns.Add("PERIOD", typeof(System.Int16));
            b.表面TR_T.Columns.Add("HIRE", typeof(System.Int32));
            b.表面TR_T.Columns.Add("DIVD", typeof(System.Int32));
            b.表面TR_T.Columns.Add("CAPT_AMOR", typeof(System.Int32));
            b.表面TR_T.Columns.Add("REST", typeof(System.Int32));

            b.實質TR_T = new DataTable("實質TR_T");

            b.實質TR_T.Columns.Add("PERIOD", typeof(System.Int16));
            b.實質TR_T.Columns.Add("HIRE", typeof(System.Int32));
            b.實質TR_T.Columns.Add("DIVD", typeof(System.Int32));
            b.實質TR_T.Columns.Add("CAPT_AMOR", typeof(System.Int32));
            b.實質TR_T.Columns.Add("REST", typeof(System.Int32));

            b.其他_T = new DataTable("其他_T");

            b.其他_T.Columns.Add("PERIOD", typeof(System.Int16));
            b.其他_T.Columns.Add("HIRE", typeof(System.Int32));
            b.其他_T.Columns.Add("DIVD", typeof(System.Int32));
            b.其他_T.Columns.Add("CAPT_AMOR", typeof(System.Int32));
            b.其他_T.Columns.Add("REST", typeof(System.Int32));
            b.其他_T.Columns.Add("TOTAL", typeof(System.Int32));

            DataRow row;
            row = b.表面TR_T.NewRow();
            row["PERIOD"] = 0;
            row["HIRE"] = 頭期款;
            row["DIVD"] = 0;
            row["CAPT_AMOR"] = 頭期款;
            row["REST"] = 購買額 - 頭期款;

            b.表面TR_T.Rows.Add(row);

            row = b.其他_T.NewRow();
            row["PERIOD"] = 0;
            row["HIRE"] = 頭期款;
            row["DIVD"] = 0;
            row["CAPT_AMOR"] = 頭期款;
            row["REST"] = 購買額 - 頭期款 + 其他費用 + 保留款;
            row["TOTAL"] = 其他費用 + 保留款;

            b.其他_T.Rows.Add(row);

            dVal[0] = -(購買額 - 頭期款 - (繳款方式 == "2" ? 0 : 租金[0]));
            dVal2[0] = -(d本金 - (繳款方式 == "2" ? 0 : 租金[0]));
            dVal3[0] = -(購買額 - 頭期款 + 其他費用 + 保留款 - (繳款方式 == "2" ? 0 : 租金[0]));

            for (int r = 1; r <= 期數; r++)
            {
                row = b.表面TR_T.NewRow();
                row["PERIOD"] = r;
                row["HIRE"] = 租金[r - 1];
                b.表面TR_T.Rows.Add(row);

                row = b.其他_T.NewRow();
                row["PERIOD"] = r;
                row["HIRE"] = 租金[r - 1];
                b.其他_T.Rows.Add(row);

                if (繳款方式 == "2")
                {
                    dVal[r] = 租金[r - 1];
                    dVal2[r] = 租金[r - 1];
                    dVal3[r] = 租金[r - 1];
                }
                else
                {
                    if (r > 1)
                    {
                        dVal[r - 1] = 租金[r - 1];
                        dVal2[r - 1] = 租金[r - 1];
                        dVal3[r - 1] = 租金[r - 1];
                    }
                }
            }
            if (繳款方式 == "2")
            {
                dVal[期數] += 殘值;
                dVal2[期數] += 殘值 - 保證金;
                dVal3[期數] += 殘值;
            }
            else
            {
                dVal[期數 - 1] += 殘值;
                dVal2[期數 - 1] += 殘值 - 保證金;
                dVal3[期數 - 1] += 殘值;
            }


            if (購買額 == 0)
            {
                b.其他利息 = 0;

                for (int r = 1; r <= 期數; r++)
                {
                    b.表面TR_T.Rows[r]["DIVD"] = double.Parse(b.表面TR_T.Rows[r]["HIRE"].ToString());
                    b.表面TR_T.Rows[r]["CAPT_AMOR"] = 0;
                    b.表面TR_T.Rows[r]["REST"] = 0;

                    b.實質TR_T.ImportRow(b.表面TR_T.Rows[r]);

                    b.其他_T.Rows[r]["DIVD"] = double.Parse(b.其他_T.Rows[r]["HIRE"].ToString());
                    b.其他_T.Rows[r]["CAPT_AMOR"] = 0;
                    b.其他_T.Rows[r]["REST"] = 0;
                    b.其他_T.Rows[r]["TOTAL"] = 0;
                }
                b.表面TR = 9999;
                b.實質TR = 9999;
            }
            else
            {
                try
                {
                    b.表面TR = ((1 + Financial.IRR(ref dVal, 0.01)) - 1) * 1200 / 付款週期;
                    b.實質TR = Math.Round(((1 + Financial.IRR(ref dVal2, 0.01)) - 1) * 1200 / 付款週期, 4);
                    dTR = ((1 + Financial.IRR(ref dVal3, 0.01)) - 1) * 1200 / 付款週期;
                }
                catch (ArgumentException e)
                {
                    b.表面TR = 0;
                    b.實質TR = 0;
                    dTR = 0;
                }
                b.其他利息 = Math.Round(d本金 * ((b.實質TR / 36500) * (付供應商天數 - 付款條件天數 - 票期天數)));
                d本金 -= d本金 * ((b.實質TR / 36500) * (付供應商天數 - 付款條件天數 - 票期天數));
                dVal2[0] = -(d本金 - (繳款方式 == "2" ? 0 : 租金[0]));
                ///////////////////////////////////// 實質TR處理///////////////////////////////
                row = b.實質TR_T.NewRow();
                row["PERIOD"] = 0;
                row["HIRE"] = 頭期款;
                row["DIVD"] = 0;
                row["CAPT_AMOR"] = 頭期款;
                row["REST"] = d本金;

                b.實質TR_T.Rows.Add(row);

                for (int r = 1; r <= 期數; r++)
                {
                    double dAmt;
                    if (保險因子 != 0 && 租金[r - 1] != 0)
                        dAmt = Math.Round(d本金 * (Math.Round(租金[r - 1] / d本金, 6) - (保險因子 * 付款週期 / 100)), 4);
                    else
                        dAmt = 租金[r - 1];
                    row = b.實質TR_T.NewRow();
                    row["PERIOD"] = r;
                    row["HIRE"] = dAmt;
                    b.實質TR_T.Rows.Add(row);

                    if (繳款方式 == "2")
                    {
                        dVal2[r] = dAmt;
                    }
                    else
                    {
                        if (r > 1)
                        {
                            dVal2[r - 1] = dAmt;
                        }
                    }
                }
                if (繳款方式 == "2")
                    dVal2[期數] += 殘值 - 保證金;
                else
                    dVal2[期數 - 1] += 殘值 - 保證金;

                try
                {
                    b.實質TR = ((1 + Financial.IRR(ref dVal2, 0.01)) - 1) * 1200 / 付款週期;
                }
                catch (ArgumentException e)
                {
                    b.實質TR = 0;
                }

                ///////////////////////////////////////////////////////////////////

                for (int r = 1; r <= 期數; r++)
                {
                    if (r < 期數)
                    {
                        if (繳款方式 == "1" && r == 1)
                        {
                            b.表面TR_T.Rows[r]["DIVD"] = 0;
                            b.實質TR_T.Rows[r]["DIVD"] = 0;
                            b.其他_T.Rows[r]["DIVD"] = 0;
                        }
                        else
                        {
                            b.表面TR_T.Rows[r]["DIVD"] = Math.Round(double.Parse(b.表面TR_T.Rows[r - 1]["REST"].ToString()) * b.表面TR / 1200 * 付款週期);
                            b.實質TR_T.Rows[r]["DIVD"] = Math.Round(double.Parse(b.實質TR_T.Rows[r - 1]["REST"].ToString()) * b.實質TR / 1200 * 付款週期);
                            b.其他_T.Rows[r]["DIVD"] = Math.Round(double.Parse(b.其他_T.Rows[r - 1]["REST"].ToString()) * dTR / 1200 * 付款週期);
                        }
                        b.表面TR_T.Rows[r]["CAPT_AMOR"] = double.Parse(b.表面TR_T.Rows[r]["HIRE"].ToString()) - double.Parse(b.表面TR_T.Rows[r]["DIVD"].ToString());
                        b.實質TR_T.Rows[r]["CAPT_AMOR"] = double.Parse(b.實質TR_T.Rows[r]["HIRE"].ToString()) - double.Parse(b.實質TR_T.Rows[r]["DIVD"].ToString());
                        b.其他_T.Rows[r]["CAPT_AMOR"] = double.Parse(b.其他_T.Rows[r]["HIRE"].ToString()) - double.Parse(b.其他_T.Rows[r]["DIVD"].ToString());
                        b.表面TR_T.Rows[r]["REST"] = double.Parse(b.表面TR_T.Rows[r - 1]["REST"].ToString()) - double.Parse(b.表面TR_T.Rows[r]["CAPT_AMOR"].ToString());
                        b.實質TR_T.Rows[r]["REST"] = double.Parse(b.實質TR_T.Rows[r - 1]["REST"].ToString()) - double.Parse(b.實質TR_T.Rows[r]["CAPT_AMOR"].ToString());
                        b.其他_T.Rows[r]["REST"] = double.Parse(b.其他_T.Rows[r - 1]["REST"].ToString()) - double.Parse(b.其他_T.Rows[r]["CAPT_AMOR"].ToString());
                    }
                    else
                    {
                        if (殘值 == 0)
                        {
                            b.表面TR_T.Rows[r]["REST"] = 0;
                            b.其他_T.Rows[r]["REST"] = 0;
                            b.表面TR_T.Rows[r]["CAPT_AMOR"] = double.Parse(b.表面TR_T.Rows[r - 1]["REST"].ToString());
                            b.其他_T.Rows[r]["CAPT_AMOR"] = double.Parse(b.其他_T.Rows[r - 1]["REST"].ToString());
                            b.表面TR_T.Rows[r]["DIVD"] = double.Parse(b.表面TR_T.Rows[r]["HIRE"].ToString()) - double.Parse(b.表面TR_T.Rows[r - 1]["REST"].ToString());
                            b.其他_T.Rows[r]["DIVD"] = double.Parse(b.其他_T.Rows[r]["HIRE"].ToString()) - double.Parse(b.其他_T.Rows[r - 1]["REST"].ToString());
                        }
                        else
                        {
                            b.表面TR_T.Rows[r]["REST"] = 殘值;
                            b.其他_T.Rows[r]["REST"] = 殘值;
                            b.表面TR_T.Rows[r]["CAPT_AMOR"] = double.Parse(b.表面TR_T.Rows[r - 1]["REST"].ToString()) - 殘值;
                            b.其他_T.Rows[r]["CAPT_AMOR"] = double.Parse(b.其他_T.Rows[r - 1]["REST"].ToString()) - 殘值;
                            b.表面TR_T.Rows[r]["DIVD"] = double.Parse(b.表面TR_T.Rows[r]["HIRE"].ToString()) - double.Parse(b.表面TR_T.Rows[r]["CAPT_AMOR"].ToString());
                            b.其他_T.Rows[r]["DIVD"] = double.Parse(b.其他_T.Rows[r]["HIRE"].ToString()) - double.Parse(b.其他_T.Rows[r]["CAPT_AMOR"].ToString());
                        }
                        if (殘值 == 保證金)
                        {
                            b.實質TR_T.Rows[r]["REST"] = 0;
                            b.實質TR_T.Rows[r]["CAPT_AMOR"] = double.Parse(b.實質TR_T.Rows[r - 1]["REST"].ToString());
                            b.實質TR_T.Rows[r]["DIVD"] = double.Parse(b.實質TR_T.Rows[r]["HIRE"].ToString()) - double.Parse(b.實質TR_T.Rows[r - 1]["REST"].ToString());
                        }
                        else
                        {
                            b.實質TR_T.Rows[r]["REST"] = 殘值 - 保證金;
                            b.實質TR_T.Rows[r]["CAPT_AMOR"] = double.Parse(b.實質TR_T.Rows[r - 1]["REST"].ToString()) - 殘值 + 保證金;
                            b.實質TR_T.Rows[r]["DIVD"] = double.Parse(b.實質TR_T.Rows[r]["HIRE"].ToString()) - double.Parse(b.實質TR_T.Rows[r]["CAPT_AMOR"].ToString());
                        }
                    }
                    b.其他_T.Rows[r]["TOTAL"] = double.Parse(b.其他_T.Rows[r]["REST"].ToString()) - double.Parse(b.表面TR_T.Rows[r]["REST"].ToString());
                }
            }
            b.表面租金合計 = b.表面TR_T.Compute("sum(HIRE)", "").ToString().toNumber();
            b.表面利息合計 = b.表面TR_T.Compute("sum(DIVD)", "").ToString().toNumber();
            b.實質租金合計 = b.實質TR_T.Compute("sum(HIRE)", "").ToString().toNumber();
            b.實質利息合計 = b.實質TR_T.Compute("sum(DIVD)", "").ToString().toNumber();
            b.收入總額 = b.表面租金合計;
            b.毛收益 = b.表面利息合計;
            if (稅別 == "" || 稅別 == "1" || 稅別 == "N")
            {
                b.銷項稅額 = 0;
                for (int r = 0; r <= 期數; r++)
                {
                    b.銷項稅額 += Math.Round(double.Parse(b.表面TR_T.Rows[r]["HIRE"].ToString()) * 0.05);
                }
            }
            else
                b.銷項稅額 = 0;

            return b;
        }

        public Rtn 攤提試算(long 購買額,
                            long 其他費用,
                            int 期數,
                            long 保留款,
                            long 頭期款,
                            string 稅別)
        {
            Rtn b = new Rtn();

            b.表面TR_T = new DataTable("表面TR_T");

            b.表面TR_T.Columns.Add("PERIOD", typeof(System.Int16));
            b.表面TR_T.Columns.Add("HIRE", typeof(System.Int32));
            b.表面TR_T.Columns.Add("DIVD", typeof(System.Int32));
            b.表面TR_T.Columns.Add("CAPT_AMOR", typeof(System.Int32));
            b.表面TR_T.Columns.Add("REST", typeof(System.Int32));


            b.實質TR_T = new DataTable("實質TR_T");

            b.實質TR_T.Columns.Add("PERIOD", typeof(System.Int16));
            b.實質TR_T.Columns.Add("HIRE", typeof(System.Int32));
            b.實質TR_T.Columns.Add("DIVD", typeof(System.Int32));
            b.實質TR_T.Columns.Add("CAPT_AMOR", typeof(System.Int32));
            b.實質TR_T.Columns.Add("REST", typeof(System.Int32));

            b.其他_T = new DataTable("其他_T");

            b.其他_T.Columns.Add("PERIOD", typeof(System.Int16));
            b.其他_T.Columns.Add("HIRE", typeof(System.Int32));
            b.其他_T.Columns.Add("DIVD", typeof(System.Int32));
            b.其他_T.Columns.Add("CAPT_AMOR", typeof(System.Int32));
            b.其他_T.Columns.Add("REST", typeof(System.Int32));
            b.其他_T.Columns.Add("TOTAL", typeof(System.Int32));

            DataRow row;
            row = b.表面TR_T.NewRow();
            row["PERIOD"] = 0;
            row["HIRE"] = 頭期款;
            row["DIVD"] = 0;
            row["CAPT_AMOR"] = 頭期款;
            row["REST"] = 購買額 - 頭期款;

            b.表面TR_T.Rows.Add(row);

            row = b.實質TR_T.NewRow();
            row["PERIOD"] = 0;
            row["HIRE"] = 頭期款;
            row["DIVD"] = 0;
            row["CAPT_AMOR"] = 頭期款;
            row["REST"] = 購買額 - 頭期款;

            b.實質TR_T.Rows.Add(row);

            row = b.其他_T.NewRow();
            row["PERIOD"] = 0;
            row["HIRE"] = 頭期款;
            row["DIVD"] = 0;
            row["CAPT_AMOR"] = 頭期款;
            row["REST"] = 購買額 - 頭期款 + 其他費用 + 保留款;
            row["TOTAL"] = 其他費用 + 保留款;

            b.其他_T.Rows.Add(row);

            for (int r = 1; r <= 期數; r++)
            {
                row = b.表面TR_T.NewRow();
                row["PERIOD"] = r;
                b.表面TR_T.Rows.Add(row);

                row = b.實質TR_T.NewRow();
                row["PERIOD"] = r;
                b.實質TR_T.Rows.Add(row);

                row = b.其他_T.NewRow();
                row["PERIOD"] = r;
                b.其他_T.Rows.Add(row);
            }

            b.表面租金合計 = 頭期款;
            b.表面利息合計 = 0;
            b.實質租金合計 = 頭期款;
            b.實質利息合計 = 0;
            b.收入總額 = b.表面租金合計;
            b.毛收益 = b.表面利息合計;
            if (稅別 == "" || 稅別 == "1" || 稅別 == "N")
                b.銷項稅額 = Math.Round(頭期款 * 0.05);
            else
                b.銷項稅額 = 0;

            return b;
        }


        #endregion
    }
}