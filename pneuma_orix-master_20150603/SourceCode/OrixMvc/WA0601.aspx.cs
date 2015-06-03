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
    public partial class WA0601 : PageParent
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

        public decimal tRate
        {
            set { Session["tRate"] = value; }
            get { return (Session["tRate"] == null ? 0 : (decimal)Session["tRate"]); }
        }

        protected string strFRCCode
        {
            set { ViewState["strFRCCode"] = value; }
            get { return (ViewState["strFRCCode"] == null ? "" : (string)ViewState["strFRCCode"]); }
        }

        protected string strDEPT
        {
            set { ViewState["strDEPT"] = value; }
            get { return (ViewState["strDEPT"] == null ? "" : (string)ViewState["strDEPT"]); }
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

            if (Session["nowStatus"] != null && Session["bolWA070"] == null && Session["bolWE020"] == null)
                this.Master.Master.nowStatus = (String)Session["nowStatus"];
            //***************************end 勿動****************************

            Session["bolWA070"] = null;
            Session["bolWE020"] = null;
        }
        #endregion


        protected void reload_data(object sender, EventArgs e)
        {
            this.txtOpin.Text = "";
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

                if (Session["bolWE020"] != null)
                {
                    this.Master.Master.nowStatus = "Upd";
                    this.Master.bolSave = false;
                    this.bolWE020 = true;
                    this.status = this.Master.Master.nowStatus;
                }

                if (Session["bolWA070"] != null)
                {
                    this.Master.Master.nowStatus = "UpdAfter";
                    this.bolWA070 = true;                   
                    this.status = this.Master.Master.nowStatus;
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

                this.APLY_MTHD = this.rptRequest.Items[0].FindControl("APLY_AMOR_MTHD").value();
                this.APRV_MTHD = this.rptRun.Items[0].FindControl("APRV_AMOR_MTHD").value();
            }

        }
        #endregion



        #region setDefaultValue：欄位預設值
        /// <summary>
        /// 設定欄位預設值
        /// 本頁作業：無作用
        /// </summary>
        public string status = "";
        public string empname = "";
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

            this.tab04_DataProcess(false, strAplyNo);

            this.tab05_DataProcess(false, strAplyNo);

            this.tab06_DataProcess(false, strAplyNo);

            this.tab07_DataProcess(false, strAplyNo);

            this.tab08_DataProcess(false, strAplyNo);
                       
            this.tab09_DataProcess(false, strAplyNo);

            
            this.tab10_DataProcess(false, strAplyNo);

            this.tab11_DataProcess(false, strAplyNo);

            this.btnSet_Ref.Editing(true);
            this.Master.bolSave = true;
            this.status = this.Master.Master.nowStatus;
            this.empname = this.Master.Master.EmployeeName;

            switch (this.Master.Master.nowStatus)
            {
                case "Add":
                    ((DropDownList)rptCon.Items[0].FindControl("CON_TYPE")).SelectedValue = "1";
                    ((CheckBox)this.rptObjDetail.Items[0].FindControl("OTC")).Checked = true;
                    ((DropDownList)this.rptRequest.Items[0].FindControl("APLY_PAY_PERD")).SelectedValue = "1";
                    ((DropDownList)this.rptRun.Items[0].FindControl("APRV_PAY_PERD")).SelectedValue = "1";

                    ((DropDownList)this.rptRequest.Items[0].FindControl("APLY_PAY_MTHD")).SelectedValue = "2";
                    ((DropDownList)this.rptRun.Items[0].FindControl("APRV_PAY_MTHD")).SelectedValue = "2";

                    ((DropDownList)this.rptRequest.Items[0].FindControl("APLY_AMOR_MTHD")).SelectedValue = "1";
                    ((DropDownList)this.rptRun.Items[0].FindControl("APRV_AMOR_MTHD")).SelectedValue = "1";

                    ((DropDownList)this.rptRequest.Items[0].FindControl("APLY_TAX_ZERO")).SelectedValue = "1";
                    ((DropDownList)this.rptRun.Items[0].FindControl("APRV_TAX_ZERO")).SelectedValue = "1";
                    this.CUR_STS.Text = "申請中";

                    this.rptRequest.Editing(true);
                    this.rptRun.Editing(false); 

                    break;

                case "Copy":
                    this.APLY_NO.Text = "";
                    this.CUR_STS.Text = "申請中";

                    ((ocxControl.ocxDate)this.rptBase.Items[0].FindControl("APLY_DATE")).Text = System.DateTime.Now.ToString("yyyy/MM/dd");
                    ((TextBox)this.rptBase.Items[0].FindControl("CON_SEQ_NO")).Text = "";
                    ((TextBox)this.rptBase.Items[0].FindControl("OLD_CON_NO")).Text = "";


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
                    this.btnSet_Ref.Editing(false);
                    break;
            }


            this.tRate = decimal.Parse(dg.GetDataRow("select tax_rate from acc17 where corp_no='ORIX'")[0].ToString()) / 100;

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
                string strSQL = "exec s_WA060_Grid_AUD '" + this.APLY_NO.Text.Trim() + "','" + strCaseType + "'";
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

        private void setAGENT()
        {
            /* 找 OR3_FRC_SALES.FRC_SALES_NAME=介紹人姓名 符合之資料
 將 OR3_FRC_SALES.MOBILE 帶至右方欄位:其他
 此介紹人之供應商&營業單位名稱 為 新增第一筆標的物之預設值*/
            string strSQL = "select MOBILE from OR3_FRC_SALES where OR3_FRC_SALES.FRC_SALES_NAME='" + this.rptObjMain.Items[0].FindControl("AGENT").value() + "'";
            this.setScript("document.getElementById('" + this.rptObjMain.Items[0].FindControl("OTHER").ClientID + "').value='" + dg.GetDataRow(strSQL)[0].ToString().Trim() + "';");



        }

        private void setRETK()
        {
            string strFRC_CODE = this.rptObjDetail.Items[0].FindControl("FRC_CODE").value();
            if (strFRC_CODE == "")// || ((RadioButtonList)this.rptObjDetail.Items[0].FindControl("Is_spec_repo")).SelectedValue!="Y")
                return;

            //string strSQL = " select RETK_DURN_FR,RETK_DURN_TO,DUE_BUY_RATE from OR3_FRC_VER a left join OR_FRC_OBJ_BUY_SET b";
            //strSQL += " on a.FRC_CODE=b.FRC_CODE and a.VER=b.VER and a.FRC_STS='1'";
            //strSQL += " where a.FRC_CODE='" + strFRC_CODE + "'";

            string strSQL = "exec s_GetBuyRateGrid @PFRC_CODE='" + strFRC_CODE + "',@PAPLY_NO='" + this.APLY_NO.Text.Trim() + "'";
            //ScriptManager.RegisterStartupScript(this.Page,this.Page.GetType(),"key", "alert(\""+strSQL+"\");",true);
            DataTable dt=dg.GetDataTable(strSQL);
            this.rptRETK.DataSource = dt;
            this.rptRETK.DataBind();
            this.upRETK.Update();

            if (((RadioButtonList)this.rptObjDetail.Items[0].FindControl("Is_spec_repo")).SelectedValue == "Y" && dt.Rows.Count>0)
                ((DropDownList)this.rptObjDetail.Items[0].FindControl("BUY_WAY")).SelectedValue = dt.Rows[0]["BUY_WAY"].ToString();
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

            string strMessage = "";
            DataView dv;
            DataRow[] adr;

            DataView dvObj;
            string strID = ((Button)sender).ID;
            switch (strID)
            {
                case "btnSet_Request":
                case "btnSet_Run":
                case "btnTR_Request":
                case "btnTR_Run":

                    /*
                     * List<long> 租金,
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
                            string 稅別*/

                    RepeaterItem ritem;
                    if (strID.IndexOf("Request") != -1)
                        ritem = this.rptRequest.Items[0];
                    else
                        ritem = this.rptRun.Items[0];

                    // RepeaterItem ritem = this.rptRequest.Items[0];

                    string strType = (strID.IndexOf("Request") != -1 ? "APLY" : "APRV");
                    string APRV_AMOR_MTHD = ritem.FindControl(strType + "_AMOR_MTHD").value();
                    int APLY_PERD = ritem.FindControl(strType + "_PERD").value().toInt(); //期數

                    this.APLY_MTHD = this.rptRequest.Items[0].FindControl("APLY_AMOR_MTHD").value();
                    this.APRV_MTHD = this.rptRun.Items[0].FindControl("APRV_AMOR_MTHD").value();

                    List<long> APLY_HIRE = new List<long>();
                    switch (APRV_AMOR_MTHD)
                    {
                        case "1":
                            for (int i = 1; i <= APLY_PERD; i++)
                            {
                                APLY_HIRE.Add(long.Parse(ritem.FindControl(strType + "_HIRE").value()));
                            }
                            break;

                        case "2":
                            for (int i = 1; i <= 5; i++)
                            {
                                int APLY_LF_FR = ritem.FindControl(strType + "_LF" + i.ToString() + "_FR").value().toInt();
                                int APLY_LF_TO = ritem.FindControl(strType + "_LF" + i.ToString() + "_TO").value().toInt();
                                long APLY_LF_HIRE = long.Parse(ritem.FindControl(strType + "_LF" + i.ToString() + "_HIRE").value());
                                for (int c = APLY_LF_FR; c <= APLY_LF_TO; c++)
                                {
                                    APLY_HIRE.Add(APLY_LF_HIRE);
                                }
                            }

                            break;

                        case "5":
                            /* for (int i = 1; i <= 5; i++)
                             {
                                 int APLY_LF_FR = ritem.FindControl(strType + "_LF1_FR").value().toInt();
                                 int APLY_LF_TO = ritem.FindControl(strType + "_LF1_TO").value().toInt();
                                 long APLY_LF_HIRE = long.Parse(ritem.FindControl(strType + "_LF1_HIRE").value());
                                 for (int c = APLY_LF_FR; c <= APLY_LF_TO; c++)
                                 {
                                     APLY_HIRE.Add(APLY_LF_HIRE);
                                 }
                             }*/

                            Repeater rptS = this.rptShare;

                            if (strID.IndexOf("Request") == -1)
                                rptS = this.rptShare1;


                            for (int i = 1; i < rptS.Items.Count; i++)
                            {
                                long SURF_HIRE = long.Parse(rptS.Items[i].FindControl("HIRE").value());
                                APLY_HIRE.Add(SURF_HIRE);
                            }

                            break;

                    }

                    int PAY_COND_DAY = ritem.FindControl("PAY_COND_DAY").value().toInt(); //付款條件天數
                    int PAY_CHECK_DAY = 0; //票期天數
                    int PAY_DAY = ritem.FindControl("PAY_DAY").value().toInt(); //付款供應商天數
                    long APLY_BUY_AMT = ritem.FindControl(strType + "_BUY_AMT").value().toInt(); //購買額
                    long APLY_RAKE = long.Parse(ritem.FindControl(strType + "_RAKE").value()); //佣金
                    long APLY_ANT_EXP = long.Parse(ritem.FindControl(strType + "_ANT_EXP").value()); //其他費用
                    long APLY_BOND = long.Parse(ritem.FindControl(strType + "_BOND").value()); //保證金
                    long APLY_REST = long.Parse(ritem.FindControl(strType + "_REST").value()); //殘值
                    int APLY_PAY_PERD = ritem.FindControl(strType + "_PAY_PERD").value().toInt(); //付款週期

                    double ISU_FACTOR = ritem.FindControl("ISU_FACTOR").value().toNumber(); //保險因子
                    long APLY_SAVING_EXP = long.Parse(ritem.FindControl(strType + "_SAVING_EXP").value()); //保留款
                    string APLY_PAY_MTHD = ritem.FindControl(strType + "_PAY_MTHD").value(); //繳款方式
                    long APLY_DEPS = long.Parse(ritem.FindControl(strType + "_DEPS").value()); //頭期款
                    long ISU_AMT = long.Parse(ritem.FindControl("ISU_AMT").value()); //保險費
                    long APLY_SERV_CHAR = long.Parse(ritem.FindControl(strType + "_SERV_CHAR").value()); //手續費
                    string APLY_TAX_ZERO = ritem.FindControl(strType + "_TAX_ZERO").value(); //稅別


                    if (APRV_AMOR_MTHD == "")
                        strMessage += "[攤提試算]";

                    if (APLY_PAY_MTHD == "")
                        strMessage += "[繳款方式]";

                    if (APLY_TAX_ZERO == "")
                        strMessage += "[稅別]";

                    if (APLY_PAY_PERD == 0)
                        strMessage += "[付款週期]";

                    if (strMessage != "")
                    {
                        this.setMessageBox(strMessage + "必須輸入！");
                        return;
                    }

                    Repeater Share = this.rptShare;
                    Repeater ShareEx = this.rptShareEx;
                    Repeater ShareReal = this.rptShareReal;
                    string index = "";
                    string strRpt = "";

                    if (strID.IndexOf("Request") == -1)
                    {
                        Share = this.rptShare1;
                        ShareEx = this.rptShareEx1;
                        ShareReal = this.rptShareReal1;
                    }



                    if (APRV_AMOR_MTHD == "5" && (strID == "btnSet_Request" || strID == "btnSet_Run"))
                    {
                        r = this.攤提試算(APLY_BUY_AMT, APLY_ANT_EXP, APLY_PERD, APLY_SAVING_EXP, APLY_DEPS, APLY_TAX_ZERO);


                    }
                    else
                    {
                        r = this.攤提試算(APLY_HIRE, PAY_COND_DAY, PAY_CHECK_DAY, PAY_DAY, APLY_BUY_AMT, APLY_RAKE,
                            APLY_ANT_EXP, APLY_BOND, APLY_REST, APLY_PAY_PERD, APLY_PERD, ISU_FACTOR, APLY_SAVING_EXP, APLY_PAY_MTHD, APLY_DEPS, ISU_AMT, APLY_SERV_CHAR, APLY_TAX_ZERO);

                        /*   public Rtn 攤提試算(List<long> 租金,
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
                          */
                        ((ocxControl.ocxNumber)ritem.FindControl(strType + "_OTH_INT")).clientText(r.其他利息);
                        ((ocxControl.ocxNumber)ritem.FindControl(strType + "_SURF_TR")).clientText(r.表面TR);
                        ((ocxControl.ocxNumber)ritem.FindControl(strType + "_REAL_TR")).clientText(r.實質TR);
                        ((ocxControl.ocxNumber)ritem.FindControl(strType + "_OTH_INT")).Text = (r.其他利息).ToString();
                        ((ocxControl.ocxNumber)ritem.FindControl(strType + "_SURF_TR")).Text = (r.表面TR).ToString();
                        ((ocxControl.ocxNumber)ritem.FindControl(strType + "_REAL_TR")).Text = (r.實質TR).ToString();
                    }


                    Share.DataSource = r.表面TR_T;
                    Share.DataBind();

                    ShareEx.DataSource = r.其他_T;
                    ShareEx.DataBind();

                    ShareReal.DataSource = r.實質TR_T;
                    ShareReal.DataBind();

                    strScript = "";

                    if (strID.IndexOf("Request") == -1)
                    {
                        strRpt = "1";
                    }
                    strScript += "document.getElementById('rptShare" + strRpt + "_HIRE').innerHTML='" + r.表面TR_T.Compute("sum(HIRE)", "").ToString().toNumber().ToString("###,###,##0") + "';\n";
                    strScript += "document.getElementById('rptShare" + strRpt + "_DIVD').innerHTML='" + r.表面TR_T.Compute("sum(DIVD)", "").ToString().toNumber().ToString("###,###,##0") + "';\n";

                    strScript += "document.getElementById('rptShareReal" + strRpt + "_HIRE').innerHTML='" + r.實質TR_T.Compute("sum(HIRE)", "").ToString().toNumber().ToString("###,###,##0") + "';\n";
                    strScript += "document.getElementById('rptShareReal" + strRpt + "_DIVD').innerHTML='" + r.實質TR_T.Compute("sum(DIVD)", "").ToString().toNumber().ToString("###,###,##0") + "';\n";

                    this.setScript(strScript);


                    //APLY_OTH_INT

                    ((ocxControl.ocxNumber)ritem.FindControl(strType + "_INCM_TOL")).clientText(r.收入總額);
                    ((ocxControl.ocxNumber)ritem.FindControl(strType + "_SELL_TAX")).clientText(r.銷項稅額);
                    ((ocxControl.ocxNumber)ritem.FindControl(strType + "_MARG")).clientText(r.毛收益);
                    ((ocxControl.ocxNumber)ritem.FindControl(strType + "_INCM_TOL")).Text = (r.收入總額).ToString();
                    ((ocxControl.ocxNumber)ritem.FindControl(strType + "_SELL_TAX")).Text = (r.銷項稅額).ToString();
                    ((ocxControl.ocxNumber)ritem.FindControl(strType + "_MARG")).Text = (r.毛收益).ToString();

                    // ((ocxControl.ocxNumber)rpt.FindControl("APLY_SURF_TR")).clientText(r.表面租金合計);
                    // ((ocxControl.ocxNumber)rpt.FindControl("APLY_SURF_TR")).clientText(r.實質租金合計);
                    // ((ocxControl.ocxNumber)rpt.FindControl("APLY_SURF_TR")).clientText(r.表面利息合計);
                    // ((ocxControl.ocxNumber)rpt.FindControl("APLY_SURF_TR")).clientText(r.實質利息合計);


                    if (strID.IndexOf("Request") != -1)
                        this.upShare.Update();
                    else
                        this.upShare1.Update();


                    //this.攤提試算(
                    break;


                case "btnRETK":
                    this.setRETK();
                    break;

                case "btnMobile":
                    this.setAGENT();
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

                    if (this.rptObjDetail.Items[0].FindControl("REAL_BUY_PRC").value() == "0")
                        strMessage += "[市價]";


                    if (((RadioButtonList)this.rptObjDetail.Items[0].FindControl("Is_spec_repo")).SelectedValue != "Y")
                    {

                        if (this.rptObjDetail.Items[0].FindControl("BUY_WAY").value() == "")
                            strMessage += "[附買回方式]";


                        //3.型態標的物.買回比率的必輸檢查改為, 若[附買回方式]='不附買回'時, 不需檢查.

                        if (this.rptObjDetail.Items[0].FindControl("BUY_RATE").value() == "0" && this.rptObjDetail.Items[0].FindControl("BUY_WAY").value() != "1")
                            strMessage += "[買回比率]";
                    }


                    if (this.rptObjDetail.Items[0].FindControl("FRC_CODE").value() == "")
                        strMessage += "[供應商]";




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

                    if (this.Master.Master.nowStatus=="UpdAfter")
                        break;
                    DataTable dt = new DataTable("Ref");
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

                    string ALL_RISKS = this.rptContact.Items[0].FindControl("ALL_RISKS").value().toNumber().ToString();
                    strSQL += " ,@PAll_TOT=" + ALL_RISKS + "";

                    string APLY_REAL_TR = this.rptRequest.Items[0].FindControl("APLY_REAL_TR").value();
                    strSQL += " ,@PAPLY_REAL_TR=" + APLY_REAL_TR + "";

                    string iAPLY_PERD = this.rptRequest.Items[0].FindControl("APLY_PERD").value();
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


                    this.setScript("document.getElementById('" + ((ocxControl.ocxDialog)this.rptObjMain.Items[0].FindControl("AUD_LVL_CODE")).ClientID + "').value='" + dr["S16"].ToString().Trim() + "';\n document.getElementById('AUD_LVL_NAME').value='" + dr["SN16"].ToString().Trim() + "';");
                    // this.setScript("document.getElementById('AUD_LVL_NAME').value='" + dr["SN16"].ToString().Trim() + "';");

                    this.rptRef.DataSource = dt;
                    this.rptRef.DataBind();
                    this.upRef.Update();

                    this.dtRef = dt;
                    if (bolSave && (this.STS=="" || this.STS=="0"))
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



                        //                        @PAPLY_OR_AUD int,			--申請或審核(1 or 2)
                        //@PUserID varchar(10),		--UserID
                        //@PUserName nvarchar(25)		--UserName

                        DataTable tbOpin = dg.GetDataTable(strSQL);


                        this.rptOpin.DataSource = tbOpin;
                        this.rptOpin.DataBind();
                        this.upOpin.Update();

                    }


                    break;

                //還沒確認的可以修改, 已確認的最新版可以取消,
                //  都確認過的, 才可以新增


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
                     * */

                    strSQL = "exec s_WA060_RealCalc";
                    strSQL += " @PCust_No='" + this.rptBase.Items[0].FindControl("CUST_NO").value() + "',";
                    strSQL += " @PMthd='1',";
                    strSQL += " @PAply_No='" + this.APLY_NO.Text.Trim() + "',";
                    strSQL += " @PCur_Quota_Aply_No='" + this.rptBase.Items[0].FindControl("CUR_QUOTA_APLY_NO").value() + "',";
                    strSQL += " @P收入總額=" + this.rptRequest.Items[0].FindControl("APLY_INCM_TOL").value() + ",";
                    strSQL += " @P頭期款=" + this.rptRequest.Items[0].FindControl("APLY_DEPS").value() + "";

                    dt = dg.GetDataTable(strSQL);
                    this.dtSURPLUS = dt;

                    this.rptContact.DataSource = dt;
                    this.rptContact.DataBind();
                    this.upContact.Update();
                    break;
            }
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
            dr["OLD_APLY_NO"] = this.rptObjDetail.Items[0].FindControl("OLD_APLY_NO").value();
            dr["NotInclude"] = "";
            /*  try
              {
                  dr["OLD_APLY_NO"] = "";
              }
              catch
              {
              }*/
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

                    if (dg.GetDataRow("select CUST_STS from OR_CUSTOM where CUST_NO='" + this.rptBase.Items[0].FindControl("CUST_NO").value() + "'")[0].ToString() == "Y")
                    {
                        this.setMessageBox("此客戶為潛在客戶，請至客戶維護修改。");
                        return false;
                    }


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
                    else if (this.rptBase.Items[0].FindControl("REQ_PAY_ADDR").value() == ((DropDownList)this.rptBase.Items[0].FindControl("CITY_CODE")).SelectedItem.Text.Trim() + ((DropDownList)this.rptBase.Items[0].FindControl("REQU_ZIP")).SelectedItem.Text.Trim().Substring(0, ((DropDownList)this.rptBase.Items[0].FindControl("REQU_ZIP")).SelectedItem.Text.Trim().Length - 4))
                    {
                        strMessage += "[請款地址]";
                    }
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



                    if (strStatus == "Add" || strStatus == "Upd" || strStatus == "Copy")
                    {


                        if (this.rptRequest.Items[0].FindControl("APLY_PAY_MTHD").value() == "")
                            strMessage += "[申請條件:繳款方式]";

                        if (this.rptRequest.Items[0].FindControl("APLY_AMOR_MTHD").value() == "")
                            strMessage += "[申請條件:攤提方式]";

                        if (this.rptRequest.Items[0].FindControl("APLY_PAY_PERD").value() == "")
                            strMessage += "[申請條件:付款週期]";

                        if (this.rptRequest.Items[0].FindControl("APLY_TAX_ZERO").value() == "")
                            strMessage += "[申請條件:稅別]";



                        if (strMessage != "")
                        {
                            this.setMessageBox(strMessage + "必須輸入！");
                            return false;
                        }

                        if (rptBase.Items[0].FindControl("CUR_QUOTA_APLY_NO").value() != "")
                        {
                            // if (this.rptRequest.Items[0].FindControl("APLY_INCM_TOL").value().toNumber() - this.rptRequest.Items[0].FindControl("APLY_DEPS").value().toNumber() > this.rptContact.Items[0].FindControl("L_APRV_PREV_QUOTA_CYCLE").value().toNumber() + this.rptContact.Items[0].FindControl("L_APRV_PREV_QUOTA_ACYCLIC").value().toNumber())
                            // {
                            //     this.setMessageBox("申請之額度超出指定額度之剩餘金額！");
                            //     return false;
                            // }
                            /*
                             * 往來實績SP只能檢查總額度
                                無法檢客戶之額度上限
                                因此在開sp 可都檢查
                                 
                                 
                                執行 s_checkQuota
                                傳入三個參數
                                           @PQuota_Aply_No varchar(15)='',   --額度申請書編號
                                           @PCust_No varchar(10)='',    --客戶代號
                                           @PAply_No varchar(14)=''       --本案申請書號 (FOR排除,若此申請書已存檔過則需傳入排除之,不輸入則為此客戶目前剩餘可用額度)
                                 
                                輸出:
                                select         @CYCLE                                              as CYCLE,
                                                     @總額度                                                as ALL_QUOTA,
                                                     @剩餘額度                                 as ALL_LEFT,
                                                     @客戶額度                                 as CUST_QUOTA,
                                                     @客戶剩餘額度                         as CUST_LEFT,
                                                     @額度到期日                            as DUE_DATE  
                                                     ,@總申請中                                           as ALL_APLY
                                                     ,@總已核准                                           as ALL_APRV
                                                     ,@總實行中                                           as ALL_EXE
                                                     ,@客申請中                                           as CUST_APLY
                                                     ,@客已核准                                           as CUST_APRV
                                                     ,@客實行中                                           as CUST_EXE
                                 
                                 
                                /*
                                C#需做以下判斷
                                若 @額度到期日 < SYS_DATE  錯誤訊息:   額度已失效
                                若 @剩餘額度-本次申請 < 0  錯誤訊息:  剩餘額度XXXXX不足
                                若 @客戶剩餘額度-本次申請 < 0  錯誤訊息: 客戶剩餘額度XXXXX不足
                                PS:本次申請 = 畫面欄位APRV_INCM_TOL-APRV_DEPS
                             */
                            DataRow qdr=dg.GetDataRow("exec s_checkQuota'"+rptBase.Items[0].FindControl("CUR_QUOTA_APLY_NO").value() +"','"+this.rptBase.Items[0].FindControl("CUST_NO").value() +"','"+ this.APLY_NO.Text +"'");
                            if (qdr[0] != "")
                            {
                                if (DateTime.Parse(qdr["DUE_DATE"].ToString()).ToString("yyyy/MM/dd").CompareTo(System.DateTime.Now.ToString("yyyy/MM/dd")) < 0)
                                {
                                    this.setMessageBox("額度已失效！");
                                    return false;
                                }
                                if (qdr["ALL_LEFT"].ToString().toNumber() < this.rptRun.Items[0].FindControl("APRV_INCM_TOL").value().toNumber() - this.rptRun.Items[0].FindControl("APRV_DEPS").value().toNumber())
                                {
                                    this.setMessageBox("剩餘額度[" + qdr["ALL_LEFT"].ToString().toNumber().ToString("###,###,###,##0") + "]不足！");
                                    return false;
                                }

                                if (qdr["CUST_LEFT"].ToString().toNumber() < this.rptRun.Items[0].FindControl("APRV_INCM_TOL").value().toNumber() - this.rptRun.Items[0].FindControl("APRV_DEPS").value().toNumber())
                                {
                                    this.setMessageBox("客戶剩餘額度[" + qdr["CUST_LEFT"].ToString().toNumber().ToString("###,###,###,##0") + "]不足！");
                                    return false;
                                }

                            }



                        }
                        if (this.rptRequest.Items[0].FindControl("APLY_AMOR_MTHD").value() == "1" && this.rptRequest.Items[0].FindControl("APLY_TAX_ZERO").value() == "1")
                        {
                            if ((decimal.Parse(this.rptRequest.Items[0].FindControl("APLY_HIRE").value()) * this.tRate).ToString("########0").toNumber() > this.rptRequest.Items[0].FindControl("APLY_TAX").value().toNumber())
                            {
                                this.setMessageBox("申請條件:稅金須大於等於5%！");
                                return false;
                            }

                        }

                    }

                    if (strStatus == "UpdAfter")
                    {
                        if (this.rptRun.Items[0].FindControl("APRV_PAY_MTHD").value() == "")
                            strMessage += "[實行條件:繳款方式]";

                        if (this.rptRun.Items[0].FindControl("APRV_AMOR_MTHD").value() == "")
                            strMessage += "[實行條件:攤提方式]";

                        if (this.rptRun.Items[0].FindControl("APRV_PAY_PERD").value() == "")
                            strMessage += "[實行條件:付款週期]";

                        if (this.rptRun.Items[0].FindControl("APRV_TAX_ZERO").value() == "")
                            strMessage += "[實行條件:稅別]";

                        if (strMessage != "")
                        {
                            this.setMessageBox(strMessage + "必須輸入！");
                            return false;

                        }

                        if (this.rptRun.Items[0].FindControl("APRV_PAY_MTHD").value() == "1" && this.rptRun.Items[0].FindControl("APRV_TAX_ZERO").value() == "1")
                        {
                            if ((decimal.Parse(this.rptRun.Items[0].FindControl("APRV_HIRE").value()) * this.tRate).ToString("########0").toNumber() > this.rptRun.Items[0].FindControl("APRV_TAX").value().toNumber())
                            {
                                this.setMessageBox("實行條件:稅金須大於等於5%！");
                                return false;
                            }

                        }
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
                                if (this.GridObject.Rows[i].RowState != DataRowState.Deleted)
                                {
                                    if (this.GridObject.Rows[i]["OTC"].ToString().Trim() != "Y" && this.GridObject.Rows[i]["OBJ_CODE"].ToString().Trim() != "")
                                    {
                                        this.setScript("checkObject_Fail();");
                                        return false;
                                    }
                                }
                            }

                        }
                    }


                    /*
3.階定額控制要點:a.最大階段迄值需等於期數.
b.階段迄值需大於等於起值.
c.上階之迄值=下階起值-1.
d.當階段有值時, 金額不得為0.
e.第一階起值等於1."*/
                    string strMTHD = "";
                    if (strStatus == "Add" || strStatus == "Upd" || strStatus == "Copy")
                    {
                        strMTHD = this.rptRequest.Items[0].FindControl("APLY_AMOR_MTHD").value();
                        if (strMTHD == "1")
                        {

                            for (int i = 1; i <= 3; i++)
                            {
                                ((ocxControl.ocxNumber)this.rptRequest.Items[0].FindControl("APLY_LF" + i.ToString() + "_FR")).Text = "0";
                                ((ocxControl.ocxNumber)this.rptRequest.Items[0].FindControl("APLY_LF" + i.ToString() + "_TO")).Text = "0";
                                ((ocxControl.ocxNumber)this.rptRequest.Items[0].FindControl("APLY_LF" + i.ToString() + "_HIRE")).Text = "0";

                            }

                            if (((ocxControl.ocxNumber)this.rptRequest.Items[0].FindControl("APLY_HIRE")).Text == "0")
                            {
                                this.setMessageBox("申請條件:定額租金必須輸入！");
                                return false;
                            }

                        }
                        else if (strMTHD == "2")
                        {
                            ((ocxControl.ocxNumber)this.rptRequest.Items[0].FindControl("APLY_HIRE")).Text = "0";
                            ((ocxControl.ocxNumber)this.rptRequest.Items[0].FindControl("APLY_TAX")).Text = "0";

                            double to = 0;
                            for (int i = 1; i <= 3; i++)
                            {
                                if (this.rptRequest.Items[0].FindControl("APLY_LF" + i.ToString() + "_TO").value().toNumber() == 0)
                                    break;

                                double fr = this.rptRequest.Items[0].FindControl("APLY_LF" + i.ToString() + "_FR").value().toNumber();
                                to = this.rptRequest.Items[0].FindControl("APLY_LF" + i.ToString() + "_TO").value().toNumber();
                                double hire = this.rptRequest.Items[0].FindControl("APLY_LF" + i.ToString() + "_HIRE").value().toNumber();
                                //    if (to != 0 && hire == 0)
                                //      {
                                //       this.setMessageBox("申請條件:LF" + i.ToString() + "租金必須輸入！");
                                //     return false;
                                //     }

                            }

                            if (to != this.rptRequest.Items[0].FindControl("APLY_PERD").value().toNumber())
                            {
                                this.setMessageBox("申請條件:最大階段迄值需等於期數！");
                                return false;
                            }
                        }
                    }

                    if (strStatus == "UpdAfter")
                    {
                        strMTHD = this.rptRun.Items[0].FindControl("APRV_AMOR_MTHD").value();
                        if (strMTHD == "1")
                        {

                            for (int i = 1; i <= 3; i++)
                            {
                                ((ocxControl.ocxNumber)this.rptRun.Items[0].FindControl("APRV_LF" + i.ToString() + "_FR")).Text = "0";
                                ((ocxControl.ocxNumber)this.rptRun.Items[0].FindControl("APRV_LF" + i.ToString() + "_TO")).Text = "0";
                                ((ocxControl.ocxNumber)this.rptRun.Items[0].FindControl("APRV_LF" + i.ToString() + "_HIRE")).Text = "0";

                            }

                            if (((ocxControl.ocxNumber)this.rptRun.Items[0].FindControl("APRV_HIRE")).Text == "0")
                            {
                                this.setMessageBox("實行條件:定額租金必須輸入！");
                                return false;
                            }

                        }
                        else if (strMTHD == "2")
                        {
                            ((ocxControl.ocxNumber)this.rptRun.Items[0].FindControl("APRV_HIRE")).Text = "0";
                            ((ocxControl.ocxNumber)this.rptRun.Items[0].FindControl("APRV_TAX")).Text = "0";

                            double to = 0;
                            for (int i = 1; i <= 3; i++)
                            {
                                if (this.rptRun.Items[0].FindControl("APRV_LF" + i.ToString() + "_TO").value().toNumber() == 0)
                                    break;

                                double fr = this.rptRun.Items[0].FindControl("APRV_LF" + i.ToString() + "_FR").value().toNumber();
                                to = this.rptRun.Items[0].FindControl("APRV_LF" + i.ToString() + "_TO").value().toNumber();
                                double hire = this.rptRun.Items[0].FindControl("APRV_LF" + i.ToString() + "_HIRE").value().toNumber();
                                //  if (to != 0 && hire == 0)
                                // {
                                //      this.setMessageBox("實行條件:LF" + i.ToString() + "租金必須輸入！");
                                //      return false;
                                //  }

                            }

                            if (to != this.rptRun.Items[0].FindControl("APRV_PERD").value().toNumber())
                            {
                                this.setMessageBox("實行條件:最大階段迄值需等於期數！");
                                return false;
                            }
                        }
                    }
                    if (this.rptCon.Items[0].FindControl("TMP_CODE").value() == "")
                    {
                        this.setMessageBox("合約範本必須選擇！");
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
            string strSQL = " select  TAKER,BUILD_DATE=dbo.f_DateAddSlash(BUILD_DATE),BACKGROUND,SALES_RGT_ADDR,";
            strSQL += " CAPT_STR=dbo.f_ConditionGetDesc('CAPT_STR',CAPT_STR,'N'),";
            strSQL += " ORG_TYPE=dbo.f_ConditionGetDesc('ORG_TYPE',ORG_TYPE,'N'),";
            strSQL += " RGT_CAPT_AMT,EMP_PSNS,REAL_CAPT_AMT,MAIN_BUS_ITEM,";
            strSQL += " CREDIT_CUST,JUDGE_LVL,";
            strSQL += " HONEST_AGREEMENT,SECRET_PROMISE,CUST_SNAME,CONTACT,TEL=phone1,FACSIMILE,MOBILE=phone2,Email=space(10)";
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
                string strScript = "setCustom('" + strCustom + "', '" + dr["CUST_SNAME"].ToString().Trim() + "', '" + dr["CONTACT"].ToString().Trim() + "', '" + dr["TEL"].ToString().Trim() + "', '" + dr["FACSIMILE"].ToString().Trim() + "', '" + dr["MOBILE"].ToString().Trim() + "', '" + dr["Email"].ToString().Trim() + "');\n";
                //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setCustom", strScript, true);

                if (this.rptObjGrid.Items.Count == 1)
                {
                    strScript += "document.getElementById('" + ((TextBox)this.rptObjDetail.Items[0].FindControl("OBJ_LOC_ADDR")).ClientID + "').value ='" + dr["SALES_RGT_ADDR"].ToString().Trim() + "';";

                }

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
                strSQL += " left join OR_EMP f on a.N_EMP_CODE=f.EMP_CODE ";
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
                if (this.Master.Master.nowStatus == "Copy")
                {
                    dt.Rows[0]["APLY_DATE"] = System.DateTime.Now.ToString("yyyy/MM/dd");
                    dt.Rows[0]["CUR_STS"] = "";
                }
                this.rptBase.DataSource = dt;
                this.rptBase.DataBind();
                this.STS = dt.Rows[0]["CUR_STS"].ToString();

                this.hiddenREQU_ZIP.Value = dt.Rows[0]["REQU_ZIP"].ToString().Trim();

                DropDownList CITY_CODE = ((DropDownList)this.rptBase.Items[0].FindControl("CITY_CODE"));

                CITY_CODE.SelectedValue = CITY_CODE.ToolTip;

                ZIP_CODE_LOAD((DropDownList)this.rptBase.Items[0].FindControl("REQ_ZIP"), null);
                //setting s_GetAccNo
                ((TextBox)this.rptBase.Items[0].FindControl("ACCNO")).Text = dg.GetDataRow("exec s_GetAccNo '" + strAplyNo + "'")[0].ToString();

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
                    dr["CON_SEQ_NO"] = "";
                    dr["PAPER_DURN_D"] = 0;
                    dr["THIS_MTH_RATE"] = 0;
                    dr["PRJ_PAY_DATE"] = "";
                    dr["REL_SUM_CON_AMT"] = 0;
                    dr["REL_SUM_CON_SUR"] = 0;
                    dr["SUM_CON_AMT"] = 0;
                    dr["SUM_CON_SUR"] = 0;
                    dr["CUR_CON_AMT"] = 0;
                    dr["CON_TOL_SUR"] = 0;
                    dr["TRAN_MAX_SUR"] = 0;
                    dr["PAY_COND_DAY1"] = 0;
                    dr["PAPER_DURN_D1"] = 0;
                    dr["PAY_DAY1"] = 0;
                    dr["DELAY_DIVD_RATE1"] = 0;
                   
                    dr["TRAN_MAX_SUR_DATE"] = "";
                    dr["KEYIN_USER"] = this.Master.Master.EmployeeName;
                    dr["KEYIN_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                    dr["KEYIN_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                }
                else
                    dr = dt.Rows[0];

                dr["PAPER"] = this.rptCon.Items[0].FindControl("PAPER").value();
                dr["APLY_DATE"] = this.rptBase.Items[0].FindControl("APLY_DATE").value().Replace("/", "");
                dr["DEPT_CODE"] = this.rptBase.Items[0].FindControl("DEPT_CODE").value();
                dr["EMP_CODE"] = dg.GetDataRow("select dbo.f_CorpAcctToEmpID('" + this.rptBase.Items[0].FindControl("EMP_CODE").value() + "')")[0].ToString();
                dr["CUST_NO"] = this.rptBase.Items[0].FindControl("CUST_NO").value();
                dr["InitContactDate"] = this.rptBase.Items[0].FindControl("InitContactDate").value();
               // if (this.Master.Master.nowStatus == "Upd")
                    dr["CUR_STS"] = "";

                if (this.Master.Master.nowStatus == "UpdAfter")
                {

                    if (this.rptOpin.Items[this.rptOpin.Items.Count - 1].FindControl("IF_PASS").value() != "")
                    {
                        if (this.rptOpin.Items[this.rptOpin.Items.Count - 1].FindControl("IF_PASS").value() == "0")
                            dr["CUR_STS"] = "R";
                        else
                            dr["CUR_STS"] = "2";
                    }
                    else {
                        for (int i = 0; i <= this.rptOpin.Items.Count - 2; i++)
                        {
                            if (this.rptOpin.Items[i].FindControl("IF_PASS").value() != ""){
                                dr["CUR_STS"] = "1";
                                break;
                            }
                        }
                    }
                    
                }
                // dr["CON_SEQ_NO"] = this.rptBase.Items[0].FindControl("CON_SEQ_NO").value();
                dr["OLD_CON_NO"] = this.rptBase.Items[0].FindControl("OLD_CON_NO").value();
                dr["MAST_CON_NO"] = this.rptBase.Items[0].FindControl("MAST_CON_NO").value();
                //  dr["ORG_QUOTA_APLY_NO"] = this.rptBase.Items[0].FindControl("ORG_QUOTA_APLY_NO").value();
                dr["CUR_QUOTA_APLY_NO"] = this.rptBase.Items[0].FindControl("CUR_QUOTA_APLY_NO").value();
                dr["REQU_ZIP"] = this.rptBase.Items[0].FindControl("REQU_ZIP").value();
                dr["REQ_PAY_ADDR"] = this.rptBase.Items[0].FindControl("REQ_PAY_ADDR").value();
                // dr["N_EMP_CODE"] = this.rptBase.Items[0].FindControl("N_EMP_CODE").value();
                dr["RECVER"] = this.rptBase.Items[0].FindControl("RECVER").value();
                dr["RECVER_DEPT"] = this.rptBase.Items[0].FindControl("RECVER_DEPT").value();
                dr["MMail_NO"] = this.rptBase.Items[0].FindControl("MMail_NO").value();
                dr["CONTACT"] = this.rptBase.Items[0].FindControl("CONTACT").value();
                dr["CTAC_TEL"] = this.rptBase.Items[0].FindControl("CTAC_TEL").value();
                dr["FAX"] = this.rptBase.Items[0].FindControl("FAX").value();
                dr["Mobile"] = this.rptBase.Items[0].FindControl("Mobile").value();
                dr["SupplierBackground"] = this.rptBase.Items[0].FindControl("SupplierBackground").value();
                dr["MainCondition"] = this.rptBase.Items[0].FindControl("MainCondition").value();
                dr["OTHER_CONDITION"] = this.OTHER_CONDITION.Text.Trim();
                dr["EXPECT_AR_DATE"] = this.EXPECT_AR_DATE.Text.Trim().Replace("/", "");
                dr["FINANCIAL_PURPOSE"] = this.FINANCIAL_PURPOSE.Text.Trim();
                dr["Auth_Cond_Remark"] = this.Auth_Cond_Remark.Text.Trim();
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


                strSQL = "  select OBJ_KEY=a.OBJ_CODE,a.OBJ_CODE,OBJ_LOC_ADDR,b.OLD_APLY_NO,b.NOtInclude,a.Actual_lessee,a.PROD_NAME,a.OTC,a.SPEC,a.BRAND,";
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
                if (dt.Rows.Count == 1)
                {
                    ((TextBox)this.rptObjDetail.Items[0].FindControl("OBJ_LOC_ADDR")).Text = this.rptBaseCustom.Items[0].FindControl("SALES_RGT_ADDR").value();
                    ((CheckBox)this.rptObjDetail.Items[0].FindControl("otc")).Checked = true;
                    ((RadioButtonList)this.rptObjDetail.Items[0].FindControl("Is_spec_repo")).SelectedValue = "Y";
                }


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
                    dr["APLY_LEASE_PERD"] = 0;
                    dr["STL_LEASE"] = 0;
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
                            string strCode = this.GridObject.Rows[i]["OBJ_CODE"].ToString();
                            if (strKey != "")
                            {
                                if (this.dg.GetDataRow("select 'Y' from OR_OBJECT where OBJ_CODE='" + strCode + "' and obj_code!=''")[0].ToString() != "Y")
                                {
                                    string strKeyChk = strCode.PadLeft(3, ' ').Substring(0, 3).ToUpper();
                                    intCNT++;
                                    if (strKeyChk != "OTC" && strKeyChk != "ORX" && strKeyChk != "XRC")
                                    {
                                        strCode = strAplyNo.Substring(0, 4) + intCNT.ToString().PadLeft(3, '0') + strAplyNo.Substring(6, 4) + strAplyNo.Substring(11, 3);
                                        if (this.dg.GetDataRow("select 'Y'  from OR_CASE_APLY_OBJ where OBJ_CODE='" + strCode + "'")[0].ToString() == "Y")
                                        {
                                            intCNT++;
                                            strCode = strAplyNo.Substring(0, 4) + intCNT.ToString().PadLeft(3, '0') + strAplyNo.Substring(6, 4) + strAplyNo.Substring(11, 3);
                                        }
                                        if (this.dg.GetDataRow("select 'Y'  from OR_CASE_APLY_OBJ where OBJ_CODE='" + strCode + "'")[0].ToString() == "Y")
                                        {
                                            intCNT++;
                                            strCode = strAplyNo.Substring(0, 4) + intCNT.ToString().PadLeft(3, '0') + strAplyNo.Substring(6, 4) + strAplyNo.Substring(11, 3);
                                        }
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
                    DataTable dtKey = dg.GetDataTable(" select OBJ_CODE  from OR_CASE_APLY_OBJ where APLY_NO='" + strAplyNo + "'");
                    for (int i = 0; i < dtKey.Rows.Count; i++)
                    {
                        string text = strCodeKey;
                        strCodeKey = string.Concat(new string[] { text, (strCodeKey == "") ? "" : ",", "'", dtKey.Rows[i]["OBJ_CODE"].ToString().Trim(), "'" });
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
                                dr["NotInclude"] = this.GridObject.Rows[i]["NotInclude"].ToString();

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

                        DataTable dtACCS = this.dts.GetTable("OR_ACCS", "OBJ_CODE in (" + strCodeKey + ")");
                        dtCopy = dtACCS.Copy();
                        for (int i = 0; i < this.GridAccs.Rows.Count; i++)
                        {
                            string strCode = this.GridAccs.Rows[i]["OBJ_CODE"].ToString();
                            if (strCode != "")
                            {
                                string strSeq = (i + 1).ToString();
                                DataRow[] drCopy = dtCopy.Select("OBJ_CODE='" + strCode + "' and ACCS_SEQ=" + strSeq);
                                string strKeyChk = strCode.PadLeft(3, ' ').Substring(0, 3).ToUpper();
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


                        DataTable dtOBJECT = this.dts.GetTable("OR_OBJECT", "OBJ_CODE in (" + strCodeKey + ")");
                        dtCopy = dtOBJECT.Copy();


                        for (int i = 0; i < this.GridObject.Rows.Count; i++)
                        {
                            if (this.GridObject.Rows[i].RowState != DataRowState.Deleted)
                            {

                                string strCode = this.GridObject.Rows[i]["OBJ_CODE"].ToString().Trim();
                                if (strCode == "")
                                    continue;

                                DataRow[] drCopy = dtCopy.Select("OBJ_CODE='" + strCode + "'");
                                string strKeyChk = strCode.PadLeft(3, ' ').Substring(0, 3).ToUpper();
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
                string strPCase_Type = this.rptObjMain.Items[0].FindControl("AUD_CASE_TYPE").value();
                this.txtCaseType.Text = strPCase_Type;
                this.setAUD(strPCase_Type);

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


                this.Auth_Cond_Remark.Text = dt.Rows[0]["Auth_Cond_Remark"].ToString();
                this.OTHER_CONDITION.Text = dt.Rows[0]["OTHER_CONDITION"].ToString();
                this.FINANCIAL_PURPOSE.Text = dt.Rows[0]["FINANCIAL_PURPOSE"].ToString();
                this.EXPECT_AR_DATE.Text = dt.Rows[0]["EXPECT_AR_DATE"].ToString();


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
                    dr["APLY_NO"] = strAplyNo;
                    dr["Period"] = PERIOD;
                    dr["HIRE"] = this.rptShare.Items[i].FindControl("HIRE").value();
                    dr["DIVD"] = r.表面TR_T.Rows[i]["DIVD"].ToString().toNumber();
                    dr["CAPT_AMOR"] = r.表面TR_T.Rows[i]["CAPT_AMOR"].ToString().toNumber();

                    dr["REAL_HIRE"] = r.實質TR_T.Rows[i]["HIRE"].ToString().toNumber();
                    dr["REAL_DIVD"] = r.實質TR_T.Rows[i]["DIVD"].ToString().toNumber();
                    dr["REAL_CAPT_AMOR"] = r.實質TR_T.Rows[i]["CAPT_AMOR"].ToString().toNumber();

                    dr["EX_HIRE"] = r.其他_T.Rows[i]["HIRE"].ToString().toNumber();
                    dr["EX_DIVD"] = r.其他_T.Rows[i]["DIVD"].ToString().toNumber();
                    dr["EX_CAPT_AMOR"] = r.其他_T.Rows[i]["CAPT_AMOR"].ToString().toNumber();


                    dtShare.Rows.Add(dr);

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
                dtCopy = dtIMP.Copy();
                dtIMP.DeleteRows();

                for (int i = 0; i < this.rptIMP.Items.Count; i++)
                {

                    if (this.rptIMP.Items[i].FindControl("RLTY_DESC").value() != "")
                    {
                        string strCode = (i + 1).ToString();
                        drCopy = dtCopy.Select("SEQ_NO='" + strCode + "'");
                        dr = dtIMP.NewRow();
                        dr["APLY_NO"] = strAplyNo;
                        dr["SEQ_NO"] = strCode;
                        dr["RLTY_DESC"] = this.rptIMP.Items[i].FindControl("RLTY_DESC").value();
                        dr["BUILDING_NO"] = this.rptIMP.Items[i].FindControl("BUILDING_NO").value();
                        dr["LAND_AREA"] = this.rptIMP.Items[i].FindControl("LAND_AREA").value();
                        dr["BUILDING_AREA"] = this.rptIMP.Items[i].FindControl("BUILDING_AREA").value();
                        dr["DECD_PRC"] = this.rptIMP.Items[i].FindControl("DECD_PRC").value();
                        dr["COLL_SUR"] = this.rptIMP.Items[i].FindControl("COLL_SUR").value();
                        dr["CASE_FLAG"] = this.rptIMP.Items[i].FindControl("CASE_FLAG").value();
                        dr["PRCICE_BASE"] = this.rptIMP.Items[i].FindControl("PRICE_BASE").value();
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

                        dtIMP.Rows.Add(dr);
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
            SqlDataReader myReader = null;
            DataTable dt;
            DataRow dr;

            if (!bolSave)
            {
                string strPCase_Type = this.rptObjMain.Items[0].FindControl("AUD_CASE_TYPE").value();
                this.txtCaseType.Text = strPCase_Type;
                this.setAUD(strPCase_Type);

                strSQL = " select  PAY_COND_DAY,PAY_DAY,";
                strSQL += " b.APRV_BUY_AMT,b.APRV_RAKE,b.APRV_ANT_EXP,TOTAL=b.APRV_BUY_AMT+b.APRV_RAKE+b.APRV_ANT_EXP,";
                strSQL += " b.APRV_BOND,APRV_REST=b.APRV_SUR,b.APRV_PURS_TAX,b.APRV_PAY_DURN ,";
                strSQL += " b.APRV_DURN_M,b.APRV_PERD,b.A_ISU_FACTOR, b.APRV_SAVING_EXP,b.APRV_REAL_TR,";
                strSQL += " b.APRV_PAY_MTHD,b.APRV_DEPS,b.A_ISU_AMT,b.APRV_SURF_TR,";
                strSQL += " b.APRV_AMOR_MTHD,b.APRV_SERV_CHAR,b.APRV_TAX_ZERO,b.NON_FEAT_CHARGE,b.APRV_OTH_INT,";
                strSQL += " b.APRV_HIRE,b.APRV_TAX,";
                strSQL += " b.APRV_LF1_FR,b.APRV_LF1_TO,b.APRV_LF1_HIRE, ";
                strSQL += " b.APRV_LF2_FR,b.APRV_LF2_TO,b.APRV_LF2_HIRE,";
                strSQL += " b.APRV_LF3_FR,b.APRV_LF3_TO,b.APRV_LF3_HIRE,";
                strSQL += " b.APRV_LF4_FR,b.APRV_LF4_TO,b.APRV_LF4_HIRE,";
                strSQL += " b.APRV_LF5_FR,b.APRV_LF5_TO,b.APRV_LF5_HIRE,";
                strSQL += " b.APRV_INCM_TOL,b.APRV_SELL_TAX,APRV_MARG,Auth_Cond_Remark,OTHER_CONDITION,FINANCIAL_PURPOSE,EXPECT_AR_DATE";
                strSQL += " from OR_CASE_APLY_BASE  a";
                strSQL += " left join OR_CASE_APLY_EXE_COND b on a.APLY_NO=b.APLY_NO";
                strSQL += " where a.APLY_NO='" + strAplyNo + "'";
                dt = dg.GetDataTable(strSQL);

                if (dt.Rows.Count == 0)
                {
                    dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }

                this.rptRun.DataSource = dt;
                this.rptRun.DataBind();

                double APRV_BUY_AMT = this.rptRun.Items[0].FindControl("APRV_BUY_AMT").value().toNumber();

                strSQL = " select PERIOD,HIRE=SURF_HIRE,DIVD=SURF_DIVD,CAPT_AMOR=SURF_CAPT_AMOR,REST=0 from OR_CASE_APLY_EXE_AMOR_DTL ";
                strSQL += " where APLY_NO='" + strAplyNo + "'";

                dt = dg.GetDataTable(strSQL);
                double amt = APRV_BUY_AMT;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["REST"] = amt - dt.Rows[i]["CAPT_AMOR"].ToString().toNumber();
                    amt = amt - dt.Rows[i]["CAPT_AMOR"].ToString().toNumber();
                }

                this.rptShare1.DataSource = dt;
                this.rptShare1.DataBind();

                string strScript = "";
                strScript += "document.getElementById('rptShare1_HIRE').innerHTML='" + dt.Compute("sum(HIRE)", "").ToString().toNumber().ToString("###,###,##0") + "';\n";
                strScript += "document.getElementById('rptShare1_DIVD').innerHTML='" + dt.Compute("sum(DIVD)", "").ToString().toNumber().ToString("###,###,##0") + "';\n";


                strSQL = "  select PERIOD,HIRE,DIVD,CAPT_AMOR,REST=0 from OR_CASE_APLY_EXE_AMOR_DTL ";
                strSQL += " where APLY_NO='" + strAplyNo + "'";
                dt = dg.GetDataTable(strSQL);
                amt = APRV_BUY_AMT;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["REST"] = amt - dt.Rows[i]["CAPT_AMOR"].ToString().toNumber();
                    amt = amt - dt.Rows[i]["CAPT_AMOR"].ToString().toNumber();
                }
                this.rptShareReal1.DataSource = dt;
                this.rptShareReal1.DataBind();

                strScript += "document.getElementById('rptShareReal1_HIRE').innerHTML='" + dt.Compute("sum(HIRE)", "").ToString().toNumber().ToString("###,###,##0") + "';\n";
                strScript += "document.getElementById('rptShareReal1_DIVD').innerHTML='" + dt.Compute("sum(DIVD)", "").ToString().toNumber().ToString("###,###,##0") + "';\n";
                this.setScript(strScript);

                strSQL = "  select PERIOD,HIRE=EX_HIRE,DIVD=EX_DIVD,CAPT_AMOR=EX_CAPT_AMOR,REST=0,TOTAL=0 from OR_CASE_APLY_EXE_AMOR_DTL ";
                strSQL += " where APLY_NO='" + strAplyNo + "'";
                dt = dg.GetDataTable(strSQL);
                amt = APRV_BUY_AMT;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["REST"] = amt - dt.Rows[i]["CAPT_AMOR"].ToString().toNumber();
                    amt = amt - dt.Rows[i]["CAPT_AMOR"].ToString().toNumber();
                }
                this.rptShareEx1.DataSource = dt;
                this.rptShareEx1.DataBind();
            }
            else
            {
                DataTable dtRequest = dts.GetTable("OR_CASE_APLY_EXE_COND", "APLY_NO='" + strAplyNo + "'");


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


                if (this.STSCODE == "0" || this.Master.Master.nowStatus == "Upd")
                {
                    dr["APRV_BUY_AMT"] = this.rptRequest.Items[0].FindControl("APLY_BUY_AMT").value();
                    dr["APRV_RAKE"] = this.rptRequest.Items[0].FindControl("APLY_RAKE").value();
                    dr["APRV_ANT_EXP"] = this.rptRequest.Items[0].FindControl("APLY_ANT_EXP").value();
                    dr["APRV_BOND"] = this.rptRequest.Items[0].FindControl("APLY_BOND").value();
                    dr["APRV_SUR"] = this.rptRequest.Items[0].FindControl("APLY_REST").value();
                    dr["APRV_PURS_TAX"] = this.rptRequest.Items[0].FindControl("APLY_PURS_TAX").value();
                    dr["APRV_PAY_DURN"] = this.rptRequest.Items[0].FindControl("APLY_PAY_PERD").value();
                    dr["APRV_DURN_M"] = this.rptRequest.Items[0].FindControl("APLY_DURN_M").value();
                    dr["APRV_PERD"] = this.rptRequest.Items[0].FindControl("APLY_PERD").value();
                    dr["A_ISU_FACTOR"] = this.rptRequest.Items[0].FindControl("ISU_FACTOR").value();
                    dr["APRV_SAVING_EXP"] = this.rptRequest.Items[0].FindControl("APLY_SAVING_EXP").value();
                    dr["APRV_REAL_TR"] = this.rptRequest.Items[0].FindControl("APLY_REAL_TR").value();
                    dr["APRV_PAY_MTHD"] = this.rptRequest.Items[0].FindControl("APLY_PAY_MTHD").value();
                    dr["APRV_DEPS"] = this.rptRequest.Items[0].FindControl("APLY_DEPS").value();
                    dr["A_ISU_AMT"] = this.rptRequest.Items[0].FindControl("ISU_AMT").value();
                    dr["APRV_SURF_TR"] = this.rptRequest.Items[0].FindControl("APLY_SURF_TR").value();
                    dr["APRV_AMOR_MTHD"] = this.rptRequest.Items[0].FindControl("APLY_AMOR_MTHD").value();
                    dr["APRV_SERV_CHAR"] = this.rptRequest.Items[0].FindControl("APLY_SERV_CHAR").value();
                    dr["APRV_TAX_ZERO"] = this.rptRequest.Items[0].FindControl("APLY_TAX_ZERO").value();
                    dr["NON_FEAT_CHARGE"] = this.rptRequest.Items[0].FindControl("NON_FEAT_CHARGE").value();
                    dr["APRV_OTH_INT"] = this.rptRequest.Items[0].FindControl("APLY_OTH_INT").value();
                    dr["APRV_HIRE"] = this.rptRequest.Items[0].FindControl("APLY_HIRE").value();
                    dr["APRV_TAX"] = this.rptRequest.Items[0].FindControl("APLY_TAX").value();
                    dr["APRV_LF1_FR"] = this.rptRequest.Items[0].FindControl("APLY_LF1_FR").value();
                    dr["APRV_LF1_TO"] = this.rptRequest.Items[0].FindControl("APLY_LF1_TO").value();
                    dr["APRV_LF1_HIRE"] = this.rptRequest.Items[0].FindControl("APLY_LF1_HIRE").value();
                    dr["APRV_LF2_FR"] = this.rptRequest.Items[0].FindControl("APLY_LF2_FR").value();
                    dr["APRV_LF2_TO"] = this.rptRequest.Items[0].FindControl("APLY_LF2_TO").value();
                    dr["APRV_LF2_HIRE"] = this.rptRequest.Items[0].FindControl("APLY_LF2_HIRE").value();
                    dr["APRV_LF3_FR"] = this.rptRequest.Items[0].FindControl("APLY_LF3_FR").value();
                    dr["APRV_LF3_TO"] = this.rptRequest.Items[0].FindControl("APLY_LF3_TO").value();
                    dr["APRV_LF3_HIRE"] = this.rptRequest.Items[0].FindControl("APLY_LF3_HIRE").value();
                    dr["APRV_LF4_FR"] = this.rptRequest.Items[0].FindControl("APLY_LF4_FR").value();
                    dr["APRV_LF4_TO"] = this.rptRequest.Items[0].FindControl("APLY_LF4_TO").value();
                    dr["APRV_LF4_HIRE"] = this.rptRequest.Items[0].FindControl("APLY_LF4_HIRE").value();
                    dr["APRV_LF5_FR"] = this.rptRequest.Items[0].FindControl("APLY_LF5_FR").value();
                    dr["APRV_LF5_TO"] = this.rptRequest.Items[0].FindControl("APLY_LF5_TO").value();
                    dr["APRV_LF5_HIRE"] = this.rptRequest.Items[0].FindControl("APLY_LF5_HIRE").value();
                    dr["APRV_INCM_TOL"] = this.rptRequest.Items[0].FindControl("APLY_INCM_TOL").value();
                    dr["APRV_SELL_TAX"] = this.rptRequest.Items[0].FindControl("APLY_SELL_TAX").value();
                    dr["APRV_MARG"] = this.rptRequest.Items[0].FindControl("APLY_MARG").value();
                }
                else
                {
                    dr["APRV_BUY_AMT"] = this.rptRun.Items[0].FindControl("APRV_BUY_AMT").value();
                    dr["APRV_RAKE"] = this.rptRun.Items[0].FindControl("APRV_RAKE").value();
                    dr["APRV_ANT_EXP"] = this.rptRun.Items[0].FindControl("APRV_ANT_EXP").value();
                    dr["APRV_BOND"] = this.rptRun.Items[0].FindControl("APRV_BOND").value();
                    dr["APRV_SUR"] = this.rptRun.Items[0].FindControl("APRV_REST").value();
                    dr["APRV_PURS_TAX"] = this.rptRun.Items[0].FindControl("APRV_PURS_TAX").value();
                    dr["APRV_PAY_DURN"] = this.rptRun.Items[0].FindControl("APRV_PAY_PERD").value();
                    dr["APRV_DURN_M"] = this.rptRun.Items[0].FindControl("APRV_DURN_M").value();
                    dr["APRV_PERD"] = this.rptRun.Items[0].FindControl("APRV_PERD").value();
                    dr["A_ISU_FACTOR"] = this.rptRun.Items[0].FindControl("ISU_FACTOR").value();
                    dr["APRV_SAVING_EXP"] = this.rptRun.Items[0].FindControl("APRV_SAVING_EXP").value();
                    dr["APRV_REAL_TR"] = this.rptRun.Items[0].FindControl("APRV_REAL_TR").value();
                    dr["APRV_PAY_MTHD"] = this.rptRun.Items[0].FindControl("APRV_PAY_MTHD").value();
                    dr["APRV_DEPS"] = this.rptRun.Items[0].FindControl("APRV_DEPS").value();
                    dr["A_ISU_AMT"] = this.rptRun.Items[0].FindControl("ISU_AMT").value();
                    dr["APRV_SURF_TR"] = this.rptRun.Items[0].FindControl("APRV_SURF_TR").value();
                    dr["APRV_AMOR_MTHD"] = this.rptRun.Items[0].FindControl("APRV_AMOR_MTHD").value();
                    dr["APRV_SERV_CHAR"] = this.rptRun.Items[0].FindControl("APRV_SERV_CHAR").value();
                    dr["APRV_TAX_ZERO"] = this.rptRun.Items[0].FindControl("APRV_TAX_ZERO").value();
                    dr["NON_FEAT_CHARGE"] = this.rptRun.Items[0].FindControl("NON_FEAT_CHARGE").value();
                    dr["APRV_OTH_INT"] = this.rptRun.Items[0].FindControl("APRV_OTH_INT").value();
                    dr["APRV_HIRE"] = this.rptRun.Items[0].FindControl("APRV_HIRE").value();
                    dr["APRV_TAX"] = this.rptRun.Items[0].FindControl("APRV_TAX").value();
                    dr["APRV_LF1_FR"] = this.rptRun.Items[0].FindControl("APRV_LF1_FR").value();
                    dr["APRV_LF1_TO"] = this.rptRun.Items[0].FindControl("APRV_LF1_TO").value();
                    dr["APRV_LF1_HIRE"] = this.rptRun.Items[0].FindControl("APRV_LF1_HIRE").value();
                    dr["APRV_LF2_FR"] = this.rptRun.Items[0].FindControl("APRV_LF2_FR").value();
                    dr["APRV_LF2_TO"] = this.rptRun.Items[0].FindControl("APRV_LF2_TO").value();
                    dr["APRV_LF2_HIRE"] = this.rptRun.Items[0].FindControl("APRV_LF2_HIRE").value();
                    dr["APRV_LF3_FR"] = this.rptRun.Items[0].FindControl("APRV_LF3_FR").value();
                    dr["APRV_LF3_TO"] = this.rptRun.Items[0].FindControl("APRV_LF3_TO").value();
                    dr["APRV_LF3_HIRE"] = this.rptRun.Items[0].FindControl("APRV_LF3_HIRE").value();
                    dr["APRV_LF4_FR"] = this.rptRun.Items[0].FindControl("APRV_LF4_FR").value();
                    dr["APRV_LF4_TO"] = this.rptRun.Items[0].FindControl("APRV_LF4_TO").value();
                    dr["APRV_LF4_HIRE"] = this.rptRun.Items[0].FindControl("APRV_LF4_HIRE").value();
                    dr["APRV_LF5_FR"] = this.rptRun.Items[0].FindControl("APRV_LF5_FR").value();
                    dr["APRV_LF5_TO"] = this.rptRun.Items[0].FindControl("APRV_LF5_TO").value();
                    dr["APRV_LF5_HIRE"] = this.rptRun.Items[0].FindControl("APRV_LF5_HIRE").value();
                    dr["APRV_INCM_TOL"] = this.rptRun.Items[0].FindControl("APRV_INCM_TOL").value();
                    dr["APRV_SELL_TAX"] = this.rptRun.Items[0].FindControl("APRV_SELL_TAX").value();
                    dr["APRV_MARG"] = this.rptRun.Items[0].FindControl("APRV_MARG").value();
                }


                dr["UPD_USER"] = this.Master.Master.EmployeeName;
                dr["UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                dr["UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                if (dtRequest.Rows.Count == 0)
                    dtRequest.Rows.Add(dr);


                DataTable dtShare = dts.GetTable("OR_CASE_APLY_EXE_AMOR_DTL", "APLY_NO='" + strAplyNo + "'");
                DataRow[] drCopy;
                DataTable dtCopy = dtShare.Copy();
                dtShare.DeleteRows();


                for (int i = 0; i < r.表面TR_T.Rows.Count; i++)
                {

                    string PERIOD = r.表面TR_T.Rows[i]["PERIOD"].ToString();
                    drCopy = dtCopy.Select("PERIOD='" + PERIOD + "'");


                    dr = dtShare.NewRow();
                    dr["APLY_NO"] = strAplyNo;
                    dr["Period"] = PERIOD;
                    dr["SURF_HIRE"] = this.rptShare.Items[i].FindControl("HIRE").value();
                    dr["SURF_DIVD"] = r.表面TR_T.Rows[i]["DIVD"].ToString().toNumber();
                    dr["SURF_CAPT_AMOR"] = r.表面TR_T.Rows[i]["CAPT_AMOR"].ToString().toNumber();

                    dr["HIRE"] = r.實質TR_T.Rows[i]["HIRE"].ToString().toNumber();
                    dr["DIVD"] = r.實質TR_T.Rows[i]["DIVD"].ToString().toNumber();
                    dr["CAPT_AMOR"] = r.實質TR_T.Rows[i]["CAPT_AMOR"].ToString().toNumber();

                    dr["EX_HIRE"] = r.其他_T.Rows[i]["HIRE"].ToString().toNumber();
                    dr["EX_DIVD"] = r.其他_T.Rows[i]["DIVD"].ToString().toNumber();
                    dr["EX_CAPT_AMOR"] = r.其他_T.Rows[i]["CAPT_AMOR"].ToString().toNumber();


                    dtShare.Rows.Add(dr);

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
                strSQL += " S3=GEN_NEW,SN3=isnull((select AUD_LVL_NAME from OR_AUD_LVL_NAME where GEN_NEW=AUD_LVL_CODE),''),     ";
                strSQL += " S4=GEN_CON,SN4=isnull((select AUD_LVL_NAME from OR_AUD_LVL_NAME where GEN_CON=AUD_LVL_CODE),''),  ";
                strSQL += " S5=VP_SUP,SN5=isnull((select AUD_LVL_NAME from OR_AUD_LVL_NAME where VP_SUP=AUD_LVL_CODE),''), ";
                strSQL += " S6=CRD,SN6=isnull((select AUD_LVL_NAME from OR_AUD_LVL_NAME where CRD=AUD_LVL_CODE),''), ";
                strSQL += " S7=OTH,SN7=isnull((select AUD_LVL_NAME from OR_AUD_LVL_NAME where OTH=AUD_LVL_CODE),''), ";
                strSQL += " S8=VP_RANGE,SN8=isnull((select AUD_LVL_NAME from OR_AUD_LVL_NAME where VP_RANGE=AUD_LVL_CODE),''), ";
                strSQL += " S9=VP_STOCK,SN9=isnull((select AUD_LVL_NAME from OR_AUD_LVL_NAME where VP_STOCK=AUD_LVL_CODE),''), ";
                strSQL += " S10=VP_SUP_NO1,SN10=isnull((select AUD_LVL_NAME from OR_AUD_LVL_NAME where VP_SUP_NO1=AUD_LVL_CODE),''), ";
                strSQL += " S11=VP_TR,SN11=isnull((select AUD_LVL_NAME from OR_AUD_LVL_NAME where VP_TR=AUD_LVL_CODE),''), ";
                strSQL += " S12=VP_SUP_NOT,SN12=isnull((select AUD_LVL_NAME from OR_AUD_LVL_NAME where VP_SUP_NOT=AUD_LVL_CODE),''), ";
                strSQL += " S13=VP_NEW,SN13=isnull((select AUD_LVL_NAME from OR_AUD_LVL_NAME where VP_NEW=AUD_LVL_CODE),''), ";
                strSQL += " S14=VP_CON,SN14=isnull((select AUD_LVL_NAME from OR_AUD_LVL_NAME where VP_CON=AUD_LVL_CODE),''), ";
                strSQL += " S15=VP_CHECK,SN15=isnull((select AUD_LVL_NAME from OR_AUD_LVL_NAME where VP_CHECK=AUD_LVL_CODE),''), ";
                strSQL += " S16=LAST_AUT,SN16=isnull((select AUD_LVL_NAME from OR_AUD_LVL_NAME where LAST_AUT=AUD_LVL_CODE),'') ";
                strSQL += " from	OR_CASE_REF ";
                strSQL += " where APLY_NO='" + this.APLY_NO.Text.Trim() + "'";
                myReader = dg.openSqlReader(strSQL);
                dt = new DataTable();
                dt.Load(myReader);
                dg.closeSqlReader(myReader);
                if (dt.Rows.Count == 0)
                {
                    dr = dt.NewRow();
                    dr["SN1"] = "";
                    dr["SN2"] = "";
                    dr["SN3"] = "";
                    dr["SN4"] = "";
                    dr["SN5"] = "";
                    dr["SN6"] = "";
                    dr["SN7"] = "";
                    dr["SN8"] = "";
                    dr["SN9"] = "";
                    dr["SN10"] = "";
                    dr["SN11"] = "";
                    dr["SN12"] = "";
                    dr["SN13"] = "";
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
                if (this.dtRef != null)
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
                    dr["GEN_NEW"] = dtRef.Rows[0]["S3"].ToString();
                    dr["GEN_CON"] = dtRef.Rows[0]["S4"].ToString();
                    dr["VP_SUP"] = dtRef.Rows[0]["S5"].ToString();
                    dr["CRD"] = dtRef.Rows[0]["S6"].ToString();
                    dr["OTH"] = dtRef.Rows[0]["S7"].ToString();
                    dr["VP_RANGE"] = dtRef.Rows[0]["S8"].ToString();
                    dr["VP_STOCK"] = dtRef.Rows[0]["S9"].ToString();
                    dr["VP_SUP_NO1"] = dtRef.Rows[0]["S10"].ToString();
                    dr["VP_TR"] = dtRef.Rows[0]["S11"].ToString();
                    dr["VP_SUP_NOT"] = dtRef.Rows[0]["S12"].ToString();
                    dr["VP_NEW"] = dtRef.Rows[0]["S13"].ToString();
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
                string strCode = dg.GetDataRow("select max(AUD_LVL_CODE) FROM OR_CASE_APLY_AUD   where APLY_NO='" + this.APLY_NO.Text.Trim() + "' and IF_PASS<>''")[0].ToString();
                    

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
                    strSQL += " where a.APLY_NO='" + this.APLY_NO.Text.Trim() + "' order by a.LVL,a.AUD_LVL_CODE";
                myReader = dg.openSqlReader(strSQL);
                this.rptOpin.DataSource = myReader;
                this.rptOpin.DataBind();
                dg.closeSqlReader(myReader);

                strSQL = " SELECT a.SIGNED_NO,b.MAIN,a.DEBIT_AMT ";
                strSQL += " FROM OR3_CASE_APLY_SIGN_REC a left join orix_otc..OTC_PROCED_Content b on a.SIGNED_NO=b.PROCED_APLY_NO";
                strSQL += " where APLY_NO='" + this.APLY_NO.Text.Trim() + "'";
                myReader = dg.openSqlReader(strSQL);
                dt = new DataTable();
                try
                {
                    dt.Load(myReader);
                    dg.closeSqlReader(myReader);
                    int scnt = dt.Rows.Count;
                    int ecnt = 5;
                    for (int i = scnt; i <= ecnt; i++)
                    {
                        dr = dt.NewRow();
                        dr["SIGNED_NO"] = "";
                        dt.Rows.Add(dr);
                    }
                    this.rptSign.DataSource = dt;
                    this.rptSign.DataBind();
                }
                catch (Exception ex)
                { 
                }
            }
            else
            {
                DataTable dtAUD = dts.GetTable("OR_CASE_APLY_AUD", "APLY_NO='" + strAplyNo + "'");
                DataTable dtSIGN = dts.GetTable("OR3_CASE_APLY_SIGN_REC", "APLY_NO='" + strAplyNo + "'");

                DataTable dtCopy = dtAUD.Copy();
                DataRow[] drCopy;
                dtAUD.DeleteRows();

                for (int i = 0; i < this.rptOpin.Items.Count; i++)
                {
                    string strCode = this.rptOpin.Items[i].FindControl("AUD_LVL_CODE").value();
                    drCopy = dtCopy.Select("AUD_LVL_CODE='" + strCode + "'");
                    bool bolEnable=((DropDownList)this.rptOpin.Items[i].FindControl("IF_PASS")).Enabled;
                    dr = dtAUD.NewRow();

                    dr["APLY_NO"] = strAplyNo;
                    dr["AUD_LVL_CODE"] = strCode;                    
                    dr["AUD_OPIN"] = this.rptOpin.Items[i].FindControl("AUD_OPIN").value();
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

                dtCopy = dtSIGN.Copy();
                dtSIGN.DeleteRows();

                for (int i = 0; i < this.rptSign.Items.Count; i++)
                {
                    string strSign = this.rptSign.Items[i].FindControl("SIGNED_NO").value();
                    if (strSign == "")
                        continue;
                    drCopy = dtCopy.Select("SIGNED_NO='" + strSign + "'");
                    dr = dtSIGN.NewRow();

                    dr["APLY_NO"] = strAplyNo;
                    dr["SIGNED_NO"] = strSign;
                    dr["DEBIT_AMT"] = this.rptSign.Items[i].FindControl("DEBIT_AMT").value();
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

                    dtSIGN.Rows.Add(dr);
                }


            }
        }
        #endregion

        #region tab10 dataprocess 承租人
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strType"></param>
        private void tab10_DataProcess(bool bolSave, string strAplyNo)
        {
            string strSQL = "";
            SqlDataReader myReader = null;
            DataTable dt;
            DataRow dr;

            if (!bolSave)
            {
                strSQL = " select SEQ_NO,a.CUST_NO,CUST_SNAME,a.CONTACT,TEL,a.FACSIMILE,MOBILE,Email ";
                strSQL += " from OR3_CASE_APLY_RENTER a left join OR_CUSTOM b on a.CUST_NO=b.CUST_NO";
                strSQL += " where APLY_NO='" + this.APLY_NO.Text.Trim() + "'";

                myReader = dg.openSqlReader(strSQL);
                dt = new DataTable();
                dt.Load(myReader);
                dg.closeSqlReader(myReader);

                if (dt.Rows.Count > 0)
                {
                    dr = dt.Rows[0];
                    string strScript = "setCustom('" + dr["CUST_NO"].ToString().Trim() + "', '" + dr["CUST_SNAME"].ToString().Trim() + "', '" + dr["CONTACT"].ToString().Trim() + "', '" + dr["TEL"].ToString().Trim() + "', '" + dr["FACSIMILE"].ToString().Trim() + "', '" + dr["MOBILE"].ToString().Trim() + "', '" + dr["Email"].ToString().Trim() + "');";
                    //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setCustom", strScript, true);
                    this.setScript(strScript);
                    //Response.Write(strScript);
                }
                int scnt = dt.Rows.Count;
                int ecnt = 4;
                for (int i = scnt; i <= ecnt; i++)
                {
                    dr = dt.NewRow();
                    dr["Seq_No"] = i + 1;
                    dr["CUST_NO"] = "";
                    dr["CONTACT"] = "";
                    dr["TEL"] = "";
                    dt.Rows.Add(dr);
                }



                DataView dv = dt.DefaultView;
                dv.RowFilter = "SEQ_NO<>1";

                this.rptRent.DataSource = dv;
                this.rptRent.DataBind();
            }
            else
            {
                dt = dts.GetTable("OR3_CASE_APLY_RENTER", "APLY_NO='" + strAplyNo + "'");

                DataTable dtCopy = dt.Copy();
                DataRow[] drCopy;
                dt.DeleteRows();


                drCopy = dtCopy.Select("SEQ_NO=1");
                dr = dt.NewRow();
                dr["APLY_NO"] = strAplyNo;
                dr["SEQ_NO"] = 1;
                dr["CUST_NO"] = this.rptBase.Items[0].FindControl("CUST_NO").value();
                dr["CONTACT"] = this.rptBase.Items[0].FindControl("CONTACT").value();
                dr["TEL"] = this.rptBase.Items[0].FindControl("CTAC_TEL").value();
                dr["FACSIMILE"] = this.rptBase.Items[0].FindControl("FAX").value();
                dr["MOBILE"] = this.rptBase.Items[0].FindControl("Mobile").value();
                dr["Email"] = this.rptBase.Items[0].FindControl("CUST_EMAIL_ADDR").value();

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

                for (int i = 0; i < this.rptRent.Items.Count; i++)
                {
                    string strCust = this.rptRent.Items[i].FindControl("CUST_NO").value();
                    if (strCust == "")
                        continue;

                    drCopy = dtCopy.Select("SEQ_NO=" + (i + 2).ToString());
                    dr = dt.NewRow();
                    dr["APLY_NO"] = strAplyNo;
                    dr["SEQ_NO"] = i + 2;
                    dr["CUST_NO"] = this.rptRent.Items[i].FindControl("CUST_NO").value();
                    dr["CONTACT"] = this.rptRent.Items[i].FindControl("CONTACT").value();
                    dr["TEL"] = this.rptRent.Items[i].FindControl("TEL").value();
                    dr["FACSIMILE"] = this.rptRent.Items[i].FindControl("FACSIMILE").value();
                    dr["MOBILE"] = this.rptRent.Items[i].FindControl("MOBILE").value();
                    dr["Email"] = this.rptRent.Items[i].FindControl("Email").value();

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

        #region tab11 dataprocess 其他申請條件
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strType"></param>
        private void tab11_DataProcess(bool bolSave, string strAplyNo)
        {
            string strSQL = "";
            SqlDataReader myReader = null;
            DataTable dt;
            DataRow dr;

            if (!bolSave)
            {
                strSQL = "  select	PAPER,CON_TYPE,";
                strSQL += " b.TMP_CODE,EXPIRED_RENEW,";
                strSQL += " HIRE,TAX,YAR,MTH,PERIOD,";
                strSQL += " PACKAGE,CASHIER,CHG_CON,CHG_CODICIL,PRV_APLY_NO,LIMIT,RESTRICTION_PERIODS,FREE_SHOW_MAC_NO,CODICIL";
                strSQL += " from OR_CASE_APLY_BASE a left join OR3_CASE_APLY_APLY_COND2 b on a.APLY_NO=b.APLY_NO";
                strSQL += " where a.APLY_NO='" + this.APLY_NO.Text.Trim() + "'";
                myReader = dg.openSqlReader(strSQL);
                dt = new DataTable();
                dt.Load(myReader);
                dg.closeSqlReader(myReader);
                if (dt.Rows.Count == 0)
                {
                    dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }
                this.rptCon.DataSource = dt;
                this.rptCon.DataBind();
            }
            else
            {
                dt = dts.GetTable("OR3_CASE_APLY_APLY_COND2", "APLY_NO='" + strAplyNo + "'");
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


                //dr["PAPER"] = this.rptCon.Items[0].FindControl("PAPER").value();
                dr["CON_TYPE"] = this.rptCon.Items[0].FindControl("CON_TYPE").value();
                dr["TMP_CODE"] = this.rptCon.Items[0].FindControl("TMP_CODE").value();
                dr["EXPIRED_RENEW"] = this.rptCon.Items[0].FindControl("EXPIRED_RENEW").value();
                dr["HIRE"] = this.rptCon.Items[0].FindControl("HIRE").value();
                dr["TAX"] = this.rptCon.Items[0].FindControl("TAX").value();
                dr["YAR"] = this.rptCon.Items[0].FindControl("YAR").value();
                dr["MTH"] = this.rptCon.Items[0].FindControl("MTH").value();
                dr["PERIOD"] = this.rptCon.Items[0].FindControl("PERIOD").value();
                dr["PACKAGE"] = this.rptCon.Items[0].FindControl("PACKAGE").value();
                dr["CASHIER"] = this.rptCon.Items[0].FindControl("CASHIER").value();
                dr["CHG_CON"] = this.rptCon.Items[0].FindControl("CHG_CON").value();
                dr["CHG_CODICIL"] = this.rptCon.Items[0].FindControl("CHG_CODICIL").value();
                dr["PRV_APLY_NO"] = this.rptCon.Items[0].FindControl("PRV_APLY_NO").value();
                dr["LIMIT"] = this.rptCon.Items[0].FindControl("LIMITY").value() == "Y" ? "Y" : this.rptCon.Items[0].FindControl("LIMITN").value() == "Y" ? "N" : "";
                dr["RESTRICTION_PERIODS"] = this.rptCon.Items[0].FindControl("RESTRICTION_PERIODS").value();
                dr["CODICIL"] = this.rptCon.Items[0].FindControl("CODICIL").value();


                dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
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
            if (this.bolWE020)
            {
                this.Master.Master.nowStatus = "Upd";
                strStatus = this.Master.Master.nowStatus;
            }

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

            if (this.APLY_NO.Text.Trim() == "")
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

                    if (this.APLY_NO.Text.Trim() == "")
                    {
                        this.STSCODE = "0";
                        this.CUR_STS.Text = "申請中";
                    }

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


                    //攤提試算                                      

                    //this.SaveObjectDetail(this.bolGridAdd);
                    this.tab01_DataProcess(true, strAplyNo);
                    this.tab02_DataProcess(true, strAplyNo);
                    if (this.STSCODE == "0" || strStatus == "Upd")
                    {
                        string APRV_AMOR_MTHD = this.rptRequest.Items[0].FindControl("APLY_AMOR_MTHD").value();
                        Button btn;
                        if (APRV_AMOR_MTHD == "5")
                        {
                            btn = this.btnTR_Request;
                        }
                        else
                        {
                            btn = this.btnSet_Request;
                        }
                        this.GridFunc_Click(btn, null);
                        this.tab03_DataProcess(true, strAplyNo);
                    }
                    else
                    {
                        if (strStatus == "UpdAfter")
                        {
                            string APRV_AMOR_MTHD = this.rptRun.Items[0].FindControl("APRV_AMOR_MTHD").value();
                            Button btn;
                            if (APRV_AMOR_MTHD == "5")
                            {
                                btn = this.btnTR_Run;
                            }
                            else
                            {
                                btn = this.btnSet_Run;
                            }
                            this.GridFunc_Click(btn, null);
                        }
                    }


                    this.tab04_DataProcess(true, strAplyNo);
                    this.tab05_DataProcess(true, strAplyNo);
                    this.tab06_DataProcess(true, strAplyNo);
                    this.tab07_DataProcess(true, strAplyNo);
                    this.tab08_DataProcess(true, strAplyNo);
                    this.tab09_DataProcess(true, strAplyNo);
                    this.tab10_DataProcess(true, strAplyNo);
                    this.tab11_DataProcess(true, strAplyNo);



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

                for (int r = 0; r <= 期數; r++)  //for (int r = 1; r <= 期數; r++) 2014/01/07 因筆數不符, 由0開始
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