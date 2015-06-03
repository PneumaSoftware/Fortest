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
    public partial class WG0201 : PageParent
    {

        protected bool bolQuery
        {
            set { ViewState["bolQuery"] = value; }
            get { return (ViewState["bolQuery"] == null ? false : (bool)ViewState["bolQuery"]); }
        }

        protected string EFFDate
        {
            set { ViewState["EFFDate"] = value; }
            get { return (ViewState["EFFDate"] == null ? "" : (string)ViewState["EFFDate"]); }
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
            this.Master.KeyFields = "SeqNo,LOAN_SEQ";




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


            this.Reload_ACCTNO(null, null);
            this.BANK_ACCT_NO.SelectedValue = this.BANK_ACCT_NO.ToolTip;
            DataTable tb = this.GetGridSource("RATE", false);
            this.rptRATE.DataSource = tb;
            this.rptRATE.DataBind();

            tb = this.GetGridSource("DTL", false);
            this.rptDTL.DataSource = tb;
            this.rptDTL.DataBind();
            this.RED_AMT_TTL.Text = tb.Compute("sum(RED_AMT)", "STATUS<>'D'").ToString();

            tb = this.GetGridSource("DATE", false);
            this.rptDATE.DataSource = tb;
            this.rptDATE.DataBind();

            if (this.Master.Master.nowStatus != "Add" && this.Master.Master.nowStatus != "Copy")
                this.BANK_NO.Editing(false);

            this.bolQuery = this.Master.Master.nowStatus == "Query" || this.Master.Master.nowStatus == "Del";
        }

        #endregion


        protected void Reload_ACCTNO(object sender, System.EventArgs e)
        {
            this.BANK_ACCT_NO.Items.Clear();

            string strSQL = " select '' as BANK_ACCT_NO union all select BANK_ACCT_NO  from  ACC12 a where BANK_NO='" + this.BANK_NO.Text.Trim() + "' ";
            dg.ListBinding(this.BANK_ACCT_NO, strSQL);
            //this.BANK_ACCT_NO.Focus();

            //if (IsPostBack)
            //    this.upBANK_ACCT_NO.DataBind();
        }


        #region grid function
        protected void GridFunc_Click(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            this.bolQuery = false;
            DataTable dtGridSource;
            string strScript = "";
            string strMessage = "";
            switch (((Button)sender).ID)
            {

                case "btnDel_RATE":
                    dtGridSource = (DataTable)ViewState["GridSourceRATE"];
                    string EFF_DATE = e.CommandName;
                    DataRow drRATE = dtGridSource.Select("EFF_DATE='" + EFF_DATE + "'")[0];
                    drRATE["STATUS"] = "D";
                    rptRATE.DataSource = dtGridSource;
                    rptRATE.DataBind();
                    this.upRATE.Update();
                    break;

                case "btnDel_DTL":
                    dtGridSource = (DataTable)ViewState["GridSourceDTL"];
                    string RED_DATE = e.CommandName;
                    DataRow drDTL = dtGridSource.Select("RED_DATE='" + RED_DATE + "'")[0];
                    drDTL["STATUS"] = "D";
                    rptDTL.DataSource = dtGridSource;
                    rptDTL.DataBind();
                    this.RED_AMT_TTL.Text = dtGridSource.Compute("sum(RED_AMT)", "STATUS<>'D'").ToString();
                    this.upDTL.Update();
                    break;

                case "btnDel_DATE":
                    dtGridSource = (DataTable)ViewState["GridSourceDATE"];
                    string INTEREST_DATE = e.CommandName;
                    DataRow drDATE = dtGridSource.Select("INTEREST_DATE='" + INTEREST_DATE + "'")[0];
                    drDATE["STATUS"] = "D";
                    rptDATE.DataSource = dtGridSource;
                    rptDATE.DataBind();
                    this.upDATE.Update();
                    break;

                case "btnAdd_RATE":

                    if (this.addEFF_DATE.Text.Trim() == "")
                        strMessage += "[生效日 ]";


                    if (strMessage != "")
                    {
                        strMessage += " 必須輸入！";

                        strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                        this.setMessageBox(strMessage);
                        return;
                    }


                    if (ViewState["GridSourceRATE"] != null && this.addEFF_DATE.Text.Trim() != "")
                    {
                        dtGridSource = (DataTable)ViewState["GridSourceRATE"];
                        if (dtGridSource.Rows.Count > 0)
                        {
                            int intRow = dtGridSource.Rows.Count - 1;
                            DataRow drAdd = dtGridSource.Rows[intRow];
                            while (drAdd["Status"].ToString() == "D")
                            {
                                intRow--;
                                if (intRow < 0)
                                    break;
                                drAdd = dtGridSource.Rows[intRow];


                            }
                            if (intRow >= 0)
                            {
                                if (drAdd["EFF_DATE"].ToString().Replace("/", "").CompareTo(this.addEFF_DATE.Text.Trim().Replace("/", "")) >= 0)
                                    strMessage += "\\r\\n必須大於前一筆的生效日！";
                            }
                        }

                    }

                    if (strMessage != "")
                    {


                        strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                        this.setMessageBox(strMessage);
                        return;
                    }

                    dtGridSource = GetGridSource("RATE", true);

                    rptRATE.DataSource = dtGridSource;
                    rptRATE.DataBind();
                    rptRATE.Visible = true;



                    addEFF_DATE.Clearing();
                    addRATE.Clearing();
                    this.upRATE.Update();
                    break;


                case "btnAdd_DTL":

                    if (this.addRED_DATE.Text.Trim() == "")
                        strMessage += "[還款日期]";

                    if (this.addRED_AMT.Text.Trim() == "0")
                        strMessage += "[還款金額]";


                    if (strMessage != "")
                    {
                        strMessage += " 必須輸入！";

                        strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                        this.setMessageBox(strMessage);
                        return;
                    }


                    if (ViewState["GridSourceDTL"] != null && this.addRED_DATE.Text.Trim() != "")
                    {
                        dtGridSource = (DataTable)ViewState["GridSourceDTL"];

                        if (dtGridSource.Select("RED_DATE='" + this.addRED_DATE.Text.Trim() + "' and STATUS<>'D'").Length > 0)
                            strMessage += "\\r\\n還款日期不得重複！";
                    }

                    if (strMessage != "")
                    {
                        strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                        this.setMessageBox(strMessage);
                        return;
                    }

                    dtGridSource = GetGridSource("DTL", true);

                    rptDTL.DataSource = dtGridSource;
                    rptDTL.DataBind();
                    rptDTL.Visible = true;
                    this.RED_AMT_TTL.Text = dtGridSource.Compute("sum(RED_AMT)", "STATUS<>'D'").ToString();



                    addRED_DATE.Clearing();
                    addRED_AMT.Clearing();
                    addINTEREST_YN.Clearing();
                    this.upDTL.Update();
                    break;

                case "btnAdd_DATE":

                    if (this.addINTEREST_DATE.Text.Trim() == "")
                        strMessage += "[繳息日期]";

                    if (strMessage != "")
                    {
                        strMessage += " 必須輸入！";

                        strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                        this.setMessageBox(strMessage);
                        return;
                    }


                    if (ViewState["GridSourceDATE"] != null && this.addINTEREST_DATE.Text.Trim() != "")
                    {
                        dtGridSource = (DataTable)ViewState["GridSourceDATE"];

                        if (dtGridSource.Select("INTEREST_DATE='" + this.addINTEREST_DATE.Text.Trim() + "'").Length > 0)
                            strMessage += "\\r\\n繳息日期不得重複！";
                    }

                    if (strMessage != "")
                    {
                        strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                        this.setMessageBox(strMessage);
                        return;
                    }

                    dtGridSource = GetGridSource("DATE", true);

                    rptDATE.DataSource = dtGridSource;
                    rptDATE.DataBind();
                    rptDATE.Visible = true;


                    addINTEREST_DATE.Clearing();

                    this.upDATE.Update();
                    break;

                case "cal_DATE":
                    //                    ocxControl:ocxNumber MASK="999" ID="PERIOD" runat="server"   />
                    //       繳息日：<br /><ocxControl:ocxNumber MASK="999" ID="RDATE" runat="server"   />
                    int intPeriod = this.Interest_Payment.Text.toInt();
                    if (this.BANK_NO.Text.Trim() == "")
                    {
                        strMessage += "銀行額度必須選擇！";
                    }

                    if (this.BANK_NO.Text.Trim() == "")
                        strMessage += "[銀行額度]";

                    if (this.LOAN_DATE.Text.Trim() == "")
                        strMessage += "[借款日期]";

                    if (this.Interest_Payment.Text.Trim() == "")
                        strMessage += "[繳息週期]";

                    if (this.PAY_DIVD_DATE.Text.Trim() == "")
                        strMessage += "[繳息日]";

                    if (this.DUE_DATE.Text.Trim() == "")
                        strMessage += "[到期日]";

                    if (strMessage != "")
                    {
                        strMessage += " 必須輸入！";

                        strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                        this.setMessageBox(strMessage);
                        return;
                    }


                    int intYear = this.LOAN_DATE.Text.Trim().Substring(0, 4).toInt();
                    int intMonth = this.LOAN_DATE.Text.Trim().Substring(5, 2).toInt();
                    int intDay = this.PAY_DIVD_DATE.Text.Trim().toInt();
                    DateTime Start = new DateTime(intYear, intMonth, PAY_DIVD_DATE.Text.Trim().toInt()).AddMonths(Interest_Payment.Text.toInt());
                    string CDATE = this.CRD_DATE_TO.Text.Trim().CompareTo(this.DUE_DATE.Text.Trim()) < 0 ? this.CRD_DATE_TO.Text.Trim() : this.DUE_DATE.Text.Trim();
                    intYear = CDATE.Substring(0, 4).toInt();
                    intMonth = CDATE.Substring(5, 2).toInt();
                    intDay = CDATE.Substring(8, 2).toInt();



                    if (Start.ToString("yyyy/MM/dd").CompareTo(this.CRD_DATE_TO.Text.Trim()) >= 0)
                    {
                        this.setMessageBox("繳息日不得大於授信到期日！");
                        return;
                    }

                    if (Start.ToString("yyyy/MM/dd").CompareTo(DUE_DATE.Text.Trim()) >= 0)
                    {
                        this.setMessageBox("繳息日不得大於借款到期日！");
                        return;
                    }

                    int per = VS2008.Module.CommonCheck.DateDiff(CommonCheck.DateType.Month, Start, new DateTime(intYear, intMonth, 1)) + 1;

                    string strDATE = this.PAY_DIVD_DATE.Text.Trim();
                    DataTable dt = GetGridSource("DATE", false);
                    DataRow dr;


                    for (int i = 0; i <= per; i++)
                    {
                        string nDATE = Start.AddMonths(i).ToString("yyyy/MM/dd");
                        this.addINTEREST_DATE.Text = nDATE;

                        if (dt.Select("INTEREST_DATE='" + nDATE + "'").Length == 0)
                            this.GetGridSource("DATE", true);
                    }

                    addINTEREST_DATE.Clearing();

                    this.rptDATE.DataSource = dt;
                    this.rptDATE.DataBind();
                    this.upDATE.Update();

                    break;
            }
        }


        private bool SaveGrid(string strType)
        {

            DataTable dt;
            string strSQL = "";
            DataRow dr;
            string strMessage = "";
            switch (strType)
            {
                case "RATE":

                    this.updateGrid(this.GetGridSource(strType, false), rptRATE);

                    dt = dts.GetTable("OR3_BORROW_RATE", "SeqNo='" + this.SeqNo.Text + "' and LOAN_SEQ=" + this.LOAN_SEQ.Text);
                    for (int i = 0; i < this.rptRATE.Items.Count; i++)
                    {
                        string STATUS = this.rptRATE.Items[i].FindControl("STATUS").value();
                        string EFF_DATE = this.rptRATE.Items[i].FindControl("EFF_DATE").value().Replace("/", "");
                        double RATE = this.rptRATE.Items[i].FindControl("RATE").value().toNumber();


                        if (EFF_DATE == "")
                            strMessage += "[生效日]";


                        if (strMessage != "")
                        {
                            strMessage += " 必須輸入！";

                            strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                            this.setMessageBox(strMessage);
                            return false;
                        }



                        if (strMessage != "")
                        {


                            strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                            this.setMessageBox(strMessage);
                            return false;
                        }

                        switch (STATUS)
                        {
                            case "A":
                            case "U":
                                if (STATUS == "A")
                                {
                                    dr = dt.NewRow();
                                    dr["SeqNo"] = this.SeqNo.Text;
                                    dr["LOAN_SEQ"] = this.LOAN_SEQ.Text;
                                    dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                                    dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                                    dr["ADD_TIME"] = System.DateTime.Now.ToString("HHmmss");
                                }
                                else
                                {
                                    dr = dt.Select("EFF_DATE='" + EFF_DATE + "'")[0];
                                }
                                dr["EFF_DATE"] = EFF_DATE.Replace("/", "");
                                dr["RATE"] = RATE;

                                dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                                dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                                dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HHmmss");

                                if (STATUS == "A")
                                    dt.Rows.Add(dr);

                                break;

                            case "D":
                                dt.DeleteRows("EFF_DATE='" + EFF_DATE.Replace("/", "") + "'");
                                break;
                        }
                    }

                    break;

                case "DTL":

                    this.updateGrid(this.GetGridSource(strType, false), rptDTL);

                    dt = dts.GetTable("OR3_BORROW_EXP_REPAY_DTL", "SeqNo='" + this.SeqNo.Text + "' and LOAN_SEQ=" + this.LOAN_SEQ.Text);
                    for (int i = 0; i < this.rptDTL.Items.Count; i++)
                    {
                        string STATUS = this.rptDTL.Items[i].FindControl("STATUS").value();
                        string RED_DATE = this.rptDTL.Items[i].FindControl("RED_DATE").value().Replace("/", "");
                        double RED_AMT = this.rptDTL.Items[i].FindControl("RED_AMT").value().toNumber();
                        string INTEREST_YN = this.rptDTL.Items[i].FindControl("INTEREST_YN").value();

                        if (RED_DATE == "")
                            strMessage += "[還款日期]";

                        if (RED_AMT == 0)
                            strMessage += "[還款金額]";

                        if (strMessage != "")
                        {
                            strMessage += " 必須輸入！";

                            strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                            this.setMessageBox(strMessage);
                            return false;
                        }




                        switch (STATUS)
                        {
                            case "A":
                            case "U":
                                if (STATUS == "A")
                                {
                                    dr = dt.NewRow();
                                    dr["SeqNo"] = this.SeqNo.Text;
                                    dr["LOAN_SEQ"] = this.LOAN_SEQ.Text;
                                    dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                                    dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                                    dr["ADD_TIME"] = System.DateTime.Now.ToString("HHmmss");
                                }
                                else
                                {
                                    dr = dt.Select("RED_DATE='" + RED_DATE + "'")[0];
                                }
                                dr["RED_DATE"] = RED_DATE.Replace("/", "");
                                dr["RED_AMT"] = RED_AMT;
                                dr["INTEREST_YN"] = INTEREST_YN;

                                dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                                dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                                dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HHmmss");

                                if (STATUS == "A")
                                    dt.Rows.Add(dr);

                                break;

                            case "D":
                                dt.DeleteRows("RED_DATE='" + RED_DATE.Replace("/", "") + "'");
                                break;
                        }
                    }
                    break;

                case "DATE":

                    this.updateGrid(this.GetGridSource(strType, false), rptDATE);
                    dt = dts.GetTable("OR3_BORROW_EXP_INTEREST_DTL", "SeqNo='" + this.SeqNo.Text + "' and LOAN_SEQ=" + this.LOAN_SEQ.Text);
                    for (int i = 0; i < this.rptDATE.Items.Count; i++)
                    {
                        string STATUS = this.rptDATE.Items[i].FindControl("STATUS").value();
                        string INTEREST_DATE = this.rptDATE.Items[i].FindControl("INTEREST_DATE").value();

                        if (INTEREST_DATE == "")
                            strMessage += "[繳息日期]";


                        if (strMessage != "")
                        {
                            strMessage += " 必須輸入！";

                            strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                            this.setMessageBox(strMessage);
                            return false;
                        }




                        switch (STATUS)
                        {
                            case "A":
                            case "U":
                                if (STATUS == "A")
                                {
                                    dr = dt.NewRow();
                                    dr["SeqNo"] = this.SeqNo.Text;
                                    dr["LOAN_SEQ"] = this.LOAN_SEQ.Text;
                                    dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                                    dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                                    dr["ADD_TIME"] = System.DateTime.Now.ToString("HHmmss");
                                }
                                else
                                {
                                    dr = dt.Select("INTEREST_DATE='" + INTEREST_DATE + "'")[0];
                                }
                                dr["INTEREST_DATE"] = INTEREST_DATE.Replace("/", "");

                                dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                                dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                                dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HHmmss");

                                if (STATUS == "A")
                                    dt.Rows.Add(dr);

                                break;

                            case "D":
                                dt.DeleteRows("INTEREST_DATE='" + INTEREST_DATE.Replace("/", "") + "'");
                                break;
                        }
                    }
                    break;
            }






            return true;

        }


        private string GetGridSql(string strType)
        {
            string strSQL = "";

            string strSEQ = this.SeqNo.Text;
            string strLSEQ = this.LOAN_SEQ.Text;

            if (strSEQ == "")
                strSEQ = "0";

            if (strLSEQ == "")
                strLSEQ = "0";

            switch (strType)
            {
                case "RATE":
                    strSQL = "select EFF_DATE=dbo.f_DateAddSlash(EFF_DATE),RATE from OR3_BORROW_RATE where SeqNo='" + strSEQ + "' and LOAN_SEQ=" + strLSEQ;
                    break;

                case "DTL":
                    strSQL = "select RED_DATE=dbo.f_DateAddSlash(RED_DATE),RED_AMT,INTEREST_YN from OR3_BORROW_EXP_REPAY_DTL where SeqNo='" + strSEQ + "' and LOAN_SEQ=" + strLSEQ;
                    break;

                case "DATE":
                    strSQL = "select INTEREST_DATE=dbo.f_DateAddSlash(INTEREST_DATE) from OR3_BORROW_EXP_INTEREST_DTL where SeqNo='" + strSEQ + "' and LOAN_SEQ=" + strLSEQ;
                    break;
            }

            return strSQL;
        }



        private DataTable GetGridSource(string strType, bool bolAdd)
        {
            DataTable dtGridSource = null;

            if (ViewState["GridSource" + strType] == null)
            {

                dtGridSource = dg.GetDataTable(this.GetGridSql(strType));

                DataColumn DC = new DataColumn();
                DC.ColumnName = "STATUS";
                DC.DefaultValue = "";
                DC.DataType = Type.GetType("System.String");
                dtGridSource.Columns.Add(DC);


                ViewState["GridSource" + strType] = dtGridSource;
            }
            else
                dtGridSource = (DataTable)ViewState["GridSource" + strType];

            if (bolAdd)
            {
                dtGridSource = (DataTable)ViewState["GridSource" + strType];
                DataRow dtRow = null;
                switch (strType)
                {
                    case "RATE":
                        this.updateGrid(dtGridSource, rptRATE);
                        dtRow = dtGridSource.NewRow();
                        dtRow["EFF_DATE"] = addEFF_DATE.Text.Trim();
                        dtRow["RATE"] = addRATE.Text.Trim();

                        break;

                    case "DTL":
                        this.updateGrid(dtGridSource, rptDTL);
                        dtRow = dtGridSource.NewRow();
                        dtRow["RED_DATE"] = addRED_DATE.Text.Trim();
                        dtRow["RED_AMT"] = addRED_AMT.Text.Trim();
                        dtRow["INTEREST_YN"] = addINTEREST_YN.Checked ? "Y" : "N";

                        break;

                    case "DATE":
                        this.updateGrid(dtGridSource, rptDATE);
                        dtRow = dtGridSource.NewRow();
                        dtRow["INTEREST_DATE"] = addINTEREST_DATE.Text.Trim();

                        break;
                }

                dtRow["Status"] = "A";
                dtGridSource.Rows.Add(dtRow);
                ViewState["GridSource" + strType] = dtGridSource;
            }

            return dtGridSource;
        }
        #endregion


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
                    if (this.BANK_NO.Text.Trim() == "")
                        strMessage += "[銀行額度]";

                    if (this.LOAN_DATE.Text.Trim() == "")
                        strMessage += "[借款日期]";

                    if (this.LOAN_AMT.Text.Trim() == "0")
                        strMessage += "[借款金額]";

                    if (this.DUE_DATE.Text.Trim() == "")
                        strMessage += "[到期日]";

                    if (this.INTEREST_CAL.SelectedValue.Trim() == "")
                        strMessage += "[利息計算方式]";

                    if (strMessage != "")
                        strMessage += "必須輸入！";

                    if (strMessage != "")
                    {
                        strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                        this.setMessageBox(strMessage);
                        return false;
                    }
                    string CDATE = this.CRD_DATE_TO.Text.Trim();
                    if (this.LOAN_DATE.Text.Trim().CompareTo(CDATE) >= 0)
                    {
                        this.setMessageBox("借款日期不得大於授信到期日！");
                        return false;
                    }

                    if (this.LOAN_DATE.Text.Trim().CompareTo(this.DUE_DATE.Text.Trim()) >= 0)
                    {
                        this.setMessageBox("借款日期不得大於到期日！");
                        return false;
                    }

                    //if 新值>舊值
                    //if新值-舊值>剩餘額度
                    //MSG 借款金額超過剩餘可用額度
                    if (this.LOAN_AMT.Text.toInt() - this.LOAN_AMT_OLD.Text.toInt() > this.REST_CREDIT.Text.toInt())
                    {
                        this.setMessageBox("借款金額不得大於剩餘可用額度！");
                        return false;
                    }

                    if (this.rptRATE.Items.Count == 0)
                    {
                        this.setMessageBox("生效日至少需輸入一筆！");
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


            if (this.LOAN_SEQ.Text.Trim() == "")
                this.LOAN_SEQ.Text = (dg.GetDataRow("select count(*) from OR_BANK_LOAN_DTL where SeqNo=" + this.SeqNo.Text)[0].ToString().toInt() + 1).ToString();


            if (!this.SaveGrid("RATE"))
                return 0;

            if (!this.SaveGrid("DTL"))
                return 0;

            if (!this.SaveGrid("DATE"))
                return 0;

            DataTable tb = this.GetGridSource("RATE", false);
            if (tb.Select("STATUS<>'D'").Length == 0)
            {
                this.setMessageBox("請新增生效日資訊!");
                return 0;
            }



            DataTable dt = dts.GetTable("OR_BANK_LOAN_DTL", "SeqNo=" + this.SeqNo.Text + " and LOAN_SEQ=" + this.LOAN_SEQ.Text);
            DataTable dtAMT = dts.GetTable("OR_BANK_AMT", "SeqNo=" + this.SeqNo.Text);


            DataRow dr = null;
            double amt = 0;


            switch (strStatus)
            {

                case "Add":
                case "Copy":
                case "Upd":
                    if (dt.Rows.Count == 0)
                    {
                        dr = dt.NewRow();
                        dr["SeqNo"] = this.SeqNo.Text.Trim();
                        dr["LOAN_SEQ"] = this.LOAN_SEQ.Text.Trim();
                        dr["BANK_NO"] = this.BANK_NO.Text.Trim();
                        dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                    }
                    else
                        dr = dt.Rows[0];


                    if (strStatus == "Upd")
                        amt = dr["LOAN_AMT"].ToString().toNumber();

                    dr["LOAN_MTHD_CODE"] = this.LOAN_MTHD_CODE.SelectedValue.Trim();
                    dr["LONG_SHORT_LOAN"] = this.LONG_SHORT_LOAN.SelectedValue.Trim();
                    dr["BANK_ACCT_NO"] = this.BANK_ACCT_NO.SelectedValue.Trim();
                    dr["LOAN_AMT"] = this.LOAN_AMT.Text.Trim();
                    dr["DUE_DATE"] = this.DUE_DATE.Text.Trim().Replace("/", "");
                    dr["RATE"] = 0;
                    dr["LOAN_DATE"] = this.LOAN_DATE.Text.Trim().Replace("/", "");
                    dr["STAR_DATE"] = "";
                    dr["END_DATE"] = "";
                    dr["Interest_Payment"] = this.Interest_Payment.Text.Trim();
                    dr["PAY_DIVD_DATE"] = this.PAY_DIVD_DATE.Text.Replace("/", "");
                    dr["IsRecourse"] = this.IsRecourse.SelectedValue.Trim();
                    dr["Credit_way"] = this.COLL_MTHD.SelectedValue.Trim();
                    dr["INTEREST_CAL"] = this.INTEREST_CAL.SelectedValue.Trim();
                    dr["Leap_year"] = this.Leap_year.Checked ? "Y" : "N";
                    dr["Feb_rate_Days"] = this.Feb_rate_Days.SelectedValue.Trim(); ;
                    dr["Fixed_interest"] = this.Fixed_interest.SelectedValue.Trim(); ;
                    dr["REMARK"] = this.REMARK.Text.Trim();
                    //  dr["RATE"] = this.RATE.Text.Trim();

                    dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                    dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                    dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                    if (dt.Rows.Count == 0)
                        dt.Rows.Add(dr);

                    if (dtAMT.Rows.Count > 0)
                        dr = dtAMT.Rows[0];

                    dr["USED_CREDIT"] = dr["USED_CREDIT"].ToString().toNumber() - amt + this.LOAN_AMT.Text.toNumber();

                    break;
                case "Del":
                    dt.DeleteRows();


                    break;

            }

            if (dts.Save())
            {
                return 2;
            }
            else
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