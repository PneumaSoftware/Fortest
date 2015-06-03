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
    public partial class WA0201 : PageParent
    {


        public DataTable GridObject
        {
            set { ViewState["GridObject"] = value; }
            get { return (ViewState["GridObject"] == null ? null : (DataTable)ViewState["GridObject"]); }
        }

        public DataTable GridAccs
        {
            set { ViewState["GridAccs"] = value; }
            get { return (ViewState["GridAccs"] == null ? null : (DataTable)ViewState["GridAccs"]); }
        }

        public DataTable exGridAccs
        {
            set { ViewState["exGridAccs"] = value; }
            get { return (ViewState["exGridAccs"] == null ? null : (DataTable)ViewState["exGridAccs"]); }
        }

        public String nowRow_Object
        {
            set { ViewState["nowRow_Object"] = value; }
            get { return (ViewState["nowRow_Object"] == null ? "" : (String)ViewState["nowRow_Object"]); }
        }


        protected bool bolWA070
        {
            set { ViewState["bolWA070"] = value; }
            get { return (ViewState["bolWA070"] == null ? false : (bool)ViewState["bolWA070"]); }
        }

        private bool bolGridAdd
        {
            set { ViewState["bolGridAdd"] = value; }
            get { return (ViewState["bolGridAdd"] == null ? true : (bool)ViewState["bolGridAdd"]); }
        }

        public String STS
        {
            set { ViewState["STS"] = value; }
            get { return (ViewState["STS"] == null ? "" : (String)ViewState["STS"]); }
        }

        public decimal tRate
        {
            set { Session["tRate"] = value; }
            get { return (Session["tRate"] == null ? 0 : (decimal)Session["tRate"]); }
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
            if (Session["APLY_NO"] == null || Session["FAST_STS"] == null)
            {
                this.Response.Redirect("WA020.aspx");
            }

            if (Session["bolWA070"] != null)
            {
              //  this.Master.Master.nowStatus = "Appove";
              //  this.bolWA070 = true;
                Session["bolWA070"] = null;
            }

            this.APLY_NO.Text = (string)Session["APLY_NO"];
            this.FAST_STS.Text = (string)Session["FAST_STS"];
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
            //1.form
            //this.USER_ID.Text = (String)Session["USER_ID"];
            string strSQL = "";
            DataTable dt;
            DataRow dr;
            SqlDataReader myReader;

            string strAplyNo = this.APLY_NO.Text.Trim();

            this.tab01_DataProcess(false, strAplyNo);

            this.tab02_DataProcess(false, strAplyNo);


            if (this.Master.Master.nowStatus == "Detail" || this.Master.Master.nowStatus == "Cancel")
            {
                this.myArea.Editing(false);
            }

            this.tRate = decimal.Parse(dg.GetDataRow("select tax_rate from acc17 where corp_no='ORIX'")[0].ToString()) / 100;
            switch (this.Master.Master.nowStatus)
            {

                case "Copy":
                    this.APLY_NO.Text = "";
                    ((ocxControl.ocxDate)this.rptBase.Items[0].FindControl("APLY_DATE")).Text = System.DateTime.Now.ToString("yyyy/MM/dd");
                    break;

            }


        }
        #endregion





        protected void GridFunc_Click(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {

            DataTable dtGridSource;
            string strScript = "";
            string strSQL = "";
            DataView dvObj;

            DataView dv;
            DataRow[] adr;
            string strMessage = "";
            string strID = ((Button)sender).ID;
            switch (strID)
            {
                case "btnDel_Object":
                    this.GridAccs.DeleteRows("OBJ_KEY='" + e.CommandName + "'");
                    this.GridObject.DeleteRows("OBJ_KEY='" + e.CommandName + "'");

                    dvObj = this.GridObject.DefaultView;
                    dvObj.RowFilter = "OBJ_KEY<>''";

                    this.rptObjGrid.DataSource = dvObj;
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
                     strScript += "document.getElementById('" + this.rptObjDetail.Items[0].FindControl("OLD_APLY_NO").ClientID + "').value='';";
                     strScript += "document.getElementById('" + this.rptObjDetail.Items[0].FindControl("NotInclude").ClientID + "').checked=false;";
                    this.setScript(strScript);

                    dv = this.GridObject.DefaultView;
                    dv.RowFilter = "";

                    dv.RowFilter = "OBJ_KEY=''";
                    this.rptObjDetail.DataSource = dv;
                    this.rptObjDetail.DataBind();
                    ((UpdatePanel)this.rptObjDetail.Items[0].FindControl("upObjDetail")).Update();
                    //this.upObjDetail.Update();
                    
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
                        ((TextBox)this.rptObjDetail.Items[0].FindControl("OBJ_LOC_ADDR")).Text = this.rptBase.Items[0].FindControl("SALES_RGT_ADDR").value();
                        ((CheckBox)this.rptObjDetail.Items[0].FindControl("otc")).Checked = true;
                    }
                    if (strID == "btnAdd_Object")
                    {
                        ((ocxControl.ocxDialog)this.rptObjDetail.Items[0].FindControl("OLD_APLY_NO")).Text = "";
                        ((TextBox)this.rptObjDetail.Items[0].FindControl("OBJ_CODE")).Text = "";
                        ((TextBox)this.rptObjDetail.Items[0].FindControl("MAC_NO")).Text = "";
                    }
                    ((UpdatePanel)this.rptObjDetail.Items[0].FindControl("upObjDetail")).Update();
                    //this.upObjDetail.Update();

                    
                    ((UpdatePanel)this.rptObjDetail.Items[0].FindControl("upObjDetailM")).Update();

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
                    dv.RowFilter ="";
                    dv.RowFilter = "OBJ_KEY='" + ((TextBox)this.rptObjDetail.Items[0].FindControl("OBJ_KEY")).Text + "'";
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
                    ((UpdatePanel)this.rptObjDetail.Items[0].FindControl("upObjDetail")).Update();
                    //this.upObjDetail.Update();

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
            dr["OLD_APLY_NO"] = this.rptObjDetail.Items[0].FindControl("OLD_APLY_NO").value();
            dr["PROD_NAME"] = this.rptObjDetail.Items[0].FindControl("PROD_NAME").value();
            dr["OTC"] = this.rptObjDetail.Items[0].FindControl("OTC").value();
            dr["BRAND"] = this.rptObjDetail.Items[0].FindControl("BRAND").value();
            dr["MAC_NO"] = this.rptObjDetail.Items[0].FindControl("MAC_NO").value();
            dr["FRC_CODE"] = this.rptObjDetail.Items[0].FindControl("FRC_CODE").value();
            dr["FRC_SNAME"] = this.Request.Form["FRC_SNAME"].ToString();
            dr["NotInclude"] = (this.rptObjDetail.Items[0].FindControl("NotInclude").value() == "Y" ? "Y" : "");

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
                    dr["OBJ_CODE"] = strCode;
                    dr["OBJ_KEY"] = strKey;
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="E"></param>
        protected void Reload_Custom(object sender, System.EventArgs E)
        {
            this.setCUSTOM();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="E"></param>
        protected void Reload_Object(object sender, System.EventArgs E)
        {
            this.setObject();

        }

        private void setCUSTOM()
        {
            string strSQL = "";
            string strCustom = this.rptBase.Items[0].FindControl("CUST_NO").value();
            strSQL = " select  TAKER,SALES_RGT_ADDR";
            strSQL += " from OR_CUSTOM where CUST_NO='" + strCustom + "' ";
            DataRow dr = dg.GetDataRow(strSQL);

            string strScript = "document.getElementById('TAKER').innerHTML='" + dr[0].ToString().Trim() + "';\n";
            strScript += "document.getElementById('" + this.rptBase.Items[0].FindControl("SALES_RGT_ADDR").ClientID + "').value='" + dr[1].ToString().Trim() + "';\n";
            if (this.rptObjGrid.Items.Count == 1)
            {
                strScript += "document.getElementById('" + ((TextBox)this.rptObjDetail.Items[0].FindControl("OBJ_LOC_ADDR")).ClientID + "').value ='" + dr["SALES_RGT_ADDR"].ToString().Trim() + "';";

            }
            this.setScript(strScript);

        }

        private void setObject()
        {
            string strSQL = "";

            RepeaterItem rItem = this.rptObjDetail.Items[0];
            string APLY_NO = rItem.FindControl("OLD_APLY_NO").value();
            string OBJ_CODE = rItem.FindControl("OBJ_CODE").value();
            strSQL = "  select OBJ_KEY=a.OBJ_CODE,OBJ_LOC_ADDR,a.OTC,a.MAC_NO,a.BRAND,a.FRC_CODE,c.FRC_SNAME,b.NOtInclude";
            strSQL += " from OR_OBJECT  a left join OR_CASE_APLY_OBJ b on a.OBJ_CODE=b.OBJ_CODE ";
            strSQL += "  left join OR_FRC c on a.FRC_CODE=c.FRC_CODE where  a.OBJ_CODE='" + OBJ_CODE + "'";

            string PROD_NAME = rItem.FindControl("PROD_NAME").ClientID;
            string OBJ_CODE_c = rItem.FindControl("OBJ_CODE").ClientID;
            string OBJ_LOC_ADDR = rItem.FindControl("OBJ_LOC_ADDR").ClientID;
            string OTC = rItem.FindControl("OTC").ClientID;
            string MAC_NO = rItem.FindControl("MAC_NO").ClientID;
            string BRAND = rItem.FindControl("BRAND").ClientID;
            string FRC_CODE = rItem.FindControl("FRC_CODE").ClientID;
            string FRC_SNAME = "FRC_SNAME";

            DataRow dr = dg.GetDataRow(strSQL);
            string strScript = "";
            if (OBJ_CODE != "")
            {
                strScript += "document.getElementById('" + OBJ_LOC_ADDR + "').value='" + dr["OBJ_LOC_ADDR"].ToString().Trim() + "';\n";
                strScript += "document.getElementById('" + OTC + "').checked=" + dr["OTC"].ToString().Trim() == "Y" ? "true" : "false" + ";\n";
                strScript += "document.getElementById('" + MAC_NO + "').value='" + dr["MAC_NO"].ToString().Trim() + "';\n";
                strScript += "document.getElementById('" + BRAND + "').value='" + dr["BRAND"].ToString().Trim() + "';\n";
                strScript += "document.getElementById('" + FRC_CODE + "').value='" + dr["FRC_CODE"].ToString().Trim() + "';\n";
                strScript += "document.getElementById('" + FRC_SNAME + "').value='" + dr["FRC_SNAME"].ToString().Trim() + "';\n";
            }

            strScript += "document.getElementById('" + OBJ_CODE_c + "').readonly=" + (OBJ_CODE == "" ? "false" : "true") + "; \n";
            strScript += "document.getElementById('" + PROD_NAME + "').readonly=" + (OBJ_CODE == "" ? "false" : "true") + "; \n";
            strScript += "document.getElementById('" + OBJ_LOC_ADDR + "').readonly=" + (OBJ_CODE == "" ? "false" : "true") + "; \n";
            strScript += "document.getElementById('" + OTC + "').disabled=" + (OBJ_CODE == "" ? "false" : "true") + ";\n";
            strScript += "document.getElementById('" + MAC_NO + "').readonly=" + (OBJ_CODE == "" ? "false" : "true") + ";\n";
            strScript += "document.getElementById('" + BRAND + "').readonly=" + (OBJ_CODE == "" ? "false" : "true") + ";\n";
            strScript += "document.getElementById('" + FRC_CODE + "').readonly=" + (OBJ_CODE == "" ? "false" : "true") + ";\n";
            strScript += "document.getElementById('" + FRC_SNAME + "').readonly=" + (OBJ_CODE == "" ? "false" : "true") + ";\n";

            strScript += "document.getElementById('" + OBJ_CODE_c + "').className='" + (OBJ_CODE == "" ? "" : "display") + "' \n";
            strScript += "document.getElementById('" + PROD_NAME + "').className='" + (OBJ_CODE == "" ? "" : "display") + "';\n";
            strScript += "document.getElementById('" + OBJ_LOC_ADDR + "').className='" + (OBJ_CODE == "" ? "" : "display") + "' \n";
            strScript += "document.getElementById('" + MAC_NO + "').className='" + (OBJ_CODE == "" ? "" : "display") + "';\n";
            strScript += "document.getElementById('" + BRAND + "').className='" + (OBJ_CODE == "" ? "" : "display") + "';\n";
            strScript += "document.getElementById('" + FRC_CODE + "').className='" + (OBJ_CODE == "" ? "" : "display") + "';\n";
            strScript += "document.getElementById('" + FRC_SNAME + "').className='" + (OBJ_CODE == "" ? "" : "display") + "';\n";
            this.setScript(strScript);

            strSQL = " select OBJ_KEY=a.OBJ_CODE, a.OBJ_CODE,a.ACCS_NAME,a.ACCS_SEQ from OR_ACCS a left join OR_CASE_APLY_OBJ b on a.OBJ_CODE=b.OBJ_CODE  ";
            strSQL += " where b.APLY_NO='" + APLY_NO + "' and b.OBJ_CODE='" + OBJ_CODE + "'";
            // myReader = dg.openSqlReader(strSQL);
            DataTable dt = dg.GetDataTable(strSQL);


          //  this.GridAccs = dt;

            DataView dv = dt.DefaultView;
          //  dv.RowFilter = "";
          //  dv.RowFilter = "OBJ_CODE=''";
            this.rptAccs.DataSource = dv;
            this.rptAccs.DataBind();

            this.upAccs.Update();

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
                    if (this.rptBase.Items[0].FindControl("APLY_DATE").value() == "")
                        strMessage += "[申請日期]";

                    if (this.rptBase.Items[0].FindControl("DEPT_CODE").value() == "")
                        strMessage += "[申請單位]";

                    if (this.rptBase.Items[0].FindControl("EMP_CODE").value() == "")
                        strMessage += "[經辦代號]";

                    if (this.rptBase.Items[0].FindControl("CUST_NO").value() == "")
                        strMessage += "[客戶代號]";

                    if (this.rptBase.Items[0].FindControl("CASE_TYPE_CODE").value() == "")
                        strMessage += "[案件類別]";

                    if (this.rptRequest.Items[0].FindControl("APLY_DURN_M").value() == "0" || this.rptRequest.Items[0].FindControl("APLY_PERD").value() == "0")
                        strMessage += "[期間]";

                    if (this.rptRequest.Items[0].FindControl("APLY_PAY_MTHD").value() == "")
                        strMessage += "[繳款方式]";

                    if (this.rptRequest.Items[0].FindControl("APLY_AMOR_MTHD").value() == "")
                        strMessage += "[攤提方式]";

                    if (this.rptCon.Items[0].FindControl("TMP_CODE").value() == "")
                        strMessage += "[合約範本]";




                    if (strMessage != "")
                    {
                        this.setMessageBox(strMessage + "必須輸入！");
                        return false;
                    }


                    /*  string strScur = "";
                      for (int i = 0; i < this.rptScur.Items.Count; i++)
                      {
                          strScur=this.rptScur.Items[i].FindControl("SCUR_NATUR").value();
                          if (strScur != "")
                              break;
                      }

                      if (strScur == "")
                      {
                          this.setMessageBox("保證人請至少輸入一筆資料!");
                          return false;
                      }
                      */
                    /*
3.階定額控制要點:a.最大階段迄值需等於期數.
b.階段迄值需大於等於起值.
c.上階之迄值=下階起值-1.
d.當階段有值時, 金額不得為0.
e.第一階起值等於1."*/

                    string strMTHD = this.rptRequest.Items[0].FindControl("APLY_AMOR_MTHD").value();
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
                            this.setMessageBox("定額租金必須輸入！");
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
                          //  if (to != 0 && hire == 0)
                      //      {
                       //         this.setMessageBox("LF" + i.ToString() + "租金必須輸入！");
                        //        return false;
                        //    }

                        }

                        if (to != this.rptRequest.Items[0].FindControl("APLY_PERD").value().toNumber())
                        {
                            this.setMessageBox("最大階段迄值需等於期數！");
                            return false;
                        }
                    }

                    break;


            }

            return true;
        }
        #endregion


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
                strSQL = " select	a.FAST_STS,APLY_DATE=dbo.f_DateAddSlash(a.APLY_DATE), ";
                strSQL += " a.DEPT_CODE,b.DEPT_NAME,EMP_CODE=dbo.f_EmpIDToCorpAcct(a.EMP_CODE) ,c.EMP_NAME,i.CASE_TYPE_CODE,j.CASE_TYPE_NAME, ";
                strSQL += " a.CUST_NO,d.CUST_SNAME,a.MAST_CON_NO, ";
                strSQL += " PROJECT=(case when d.GEN_CURR_QUOTA+d.VP_CURR_QUOTA+d.AR_CURR_QUOTA>0 then '是額度客戶' else '' end), ";
                strSQL += " d.TAKER,d.SALES_RGT_ADDR ";
                strSQL += " from	OR_CASE_APLY_BASE a left join OR_DEPT b on a.DEPT_CODE=b.DEPT_CODE ";
                strSQL += " left join V_OR_EMP c on a.EMP_CODE=c.EMP_CODE ";
                strSQL += " left join OR_CUSTOM d on a.CUST_NO=d.CUST_NO ";
                strSQL += " left join OR_DEPT e on a.N_DEPT_CODE=e.DEPT_CODE ";
                strSQL += " left join V_OR_EMP f on a.EMP_CODE=f.EMP_CODE ";
                strSQL += " left join OR_MERG_MAIL g on a.MMail_NO=g.MMail_NO ";
                strSQL += " left join OR3_Zip h on a.REQU_ZIP=h.Zip_Code ";
                strSQL += " left join OR_CASE_APLY_APLY_COND i on a.APLY_NO=i.APLY_NO";
                strSQL += " left join OR_CASE_TYPE  j on i.CASE_TYPE_CODE=j.CASE_TYPE_CODE";
                strSQL += " where a.APLY_NO='" + strAplyNo + "'";
                dt = dg.GetDataTable(strSQL);

                // dt  = dg.GetDataTable(strSQL);

                if (dt.Rows.Count == 0)
                {
                    dr = dt.NewRow();
                    dr["APLY_DATE"] = System.DateTime.Now.ToString("yyyy/MM/dd");
                    dt.Rows.Add(dr);
                }
                this.rptBase.DataSource = dt;
                this.rptBase.DataBind();
                this.STS = dt.Rows[0]["FAST_STS"].ToString();


                strSQL = "  select  OBJ_KEY=a.OBJ_CODE,a.OBJ_CODE,OBJ_LOC_ADDR,OLD_APLY_NO,a.Actual_lessee,a.PROD_NAME,a.OTC,a.MAC_NO,a.BRAND,a.FRC_CODE,c.FRC_SNAME,b.NotInclude";
                strSQL += " from OR_OBJECT  a left join OR_CASE_APLY_OBJ b on a.OBJ_CODE=b.OBJ_CODE ";
                strSQL += " left join OR_FRC c on a.FRC_CODE=c.FRC_CODE";
                strSQL += " where b.APLY_NO='" + strAplyNo + "'";
                //  myReader = dg.openSqlReader(strSQL);
                dt = dg.GetDataTable(strSQL);


                dr = dt.NewRow();
                dr["OBJ_KEY"] = "";
                dr["OBJ_CODE"] = "";
                dr["OLD_APLY_NO"] = "";
                dr["PROD_NAME"] = "";
                dr["FRC_CODE"] = "";

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
                    ((TextBox)this.rptObjDetail.Items[0].FindControl("OBJ_LOC_ADDR")).Text = this.rptBase.Items[0].FindControl("SALES_RGT_ADDR").value();
                    ((CheckBox)this.rptObjDetail.Items[0].FindControl("otc")).Checked = true;
                }


                strSQL = " select OBJ_KEY=a.OBJ_CODE, a.OBJ_CODE,a.ACCS_NAME,a.ACCS_SEQ from OR_ACCS a left join OR_CASE_APLY_OBJ b on a.OBJ_CODE=b.OBJ_CODE  ";
                strSQL += " where b.APLY_NO='" + strAplyNo + "'";
                // myReader = dg.openSqlReader(strSQL);
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

                strSQL = " select  b.APLY_BOND, b.APLY_DURN_M,b.APLY_PERD,b.APLY_PAY_PERD,";
                strSQL += " b.APLY_DEPS,b.APLY_PAY_MTHD,b.APLY_SERV_CHAR,b.APLY_AMOR_MTHD,";
                strSQL += " b.APLY_HIRE,b.APLY_TAX,";
                strSQL += " b.APLY_LF1_FR,b.APLY_LF1_TO,b.APLY_LF1_HIRE, ";
                strSQL += " b.APLY_LF2_FR,b.APLY_LF2_TO,b.APLY_LF2_HIRE,";
                strSQL += " b.APLY_LF3_FR,b.APLY_LF3_TO,b.APLY_LF3_HIRE,";
                strSQL += " b.APLY_LF4_FR,b.APLY_LF4_TO,b.APLY_LF4_HIRE,";
                strSQL += " b.APLY_LF5_FR,b.APLY_LF5_TO,b.APLY_LF5_HIRE";
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
            }
            else
            {
                dt = dts.GetTable("OR_CASE_APLY_BASE", "APLY_NO='" + strAplyNo + "'");
                if (dt.Rows.Count == 0)
                {

                    dr = dt.NewRow();
                    dr["APLY_NO"] = strAplyNo;
                    dr["FAST_STS"] = "1";
                    dr["CUR_STS"] = "0";
                    dr["CON_SEQ_NO"] = "";
                    dr["OLD_CON_NO"] = "";
                    dr["REQ_PAY_ADDR"] = "";
                    dr["RECVER"] = "";
                    dr["FAX"] = "";
                    dr["CTAC_TEL"] = "";
                    dr["PAY_COND_DAY"] = 0;
                    dr["PAPER_DURN_D"] = 0;
                    dr["PRJ_PAY_DATE"] = "";
                    dr["THIS_MTH_RATE"] = 0;
                    dr["REL_SUM_CON_AMT"] = 0;
                    dr["REL_SUM_CON_SUR"] = 0;
                    dr["SUM_CON_AMT"] = 0;
                    dr["SUM_CON_SUR"] = 0;
                    dr["CUR_CON_AMT"] = 0;
                    dr["CON_TOL_SUR"] = 0;
                    dr["TRAN_MAX_SUR"] = 0;
                    dr["TRAN_MAX_SUR_DATE"] = "";
                    dr["KEYIN_USER"] = this.Master.Master.EmployeeName;
                    dr["KEYIN_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                    dr["KEYIN_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                }
                else
                    dr = dt.Rows[0];



                //binding rptBase
                strSQL = " select	a.FAST_STS,APLY_DATE=dbo.f_DateAddSlash(a.APLY_DATE), ";
                strSQL += " a.DEPT_CODE,b.DEPT_NAME,EMP_CODE=dbo.f_EmpIDToCorpAcct(a.EMP_CODE) ,c.EMP_NAME, ";
                strSQL += " a.CUST_NO,d.CUST_SNAME, ";


                dr["APLY_DATE"] = this.rptBase.Items[0].FindControl("APLY_DATE").value().Replace("/", "");
                dr["DEPT_CODE"] = this.rptBase.Items[0].FindControl("DEPT_CODE").value();
                dr["EMP_CODE"] = dg.GetDataRow("select EMP_CODE from OR_EMP WHERE CORP_ACCT='" + this.rptBase.Items[0].FindControl("EMP_CODE").value() + "'")[0].ToString();
                dr["CUST_NO"] = this.rptBase.Items[0].FindControl("CUST_NO").value();
                dr["PAPER"] = this.rptCon.Items[0].FindControl("PAPER").value();
                dr["MAST_CON_NO"] = this.rptBase.Items[0].FindControl("MAST_CON_NO").value();

                dr["UPD_USER"] = this.Master.Master.EmployeeName;
                dr["UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                dr["UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                if (dt.Rows.Count == 0)
                    dt.Rows.Add(dr);

                DataTable dtCopy;
                DataRow[] drCopy;

                //this.GridObject.AcceptChanges();

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
                            drCopy = dtCopy.Select("OBJ_CODE='" + strCode + "'");
                            if (strCode == "")
                                continue;

                            // string strKeyChk = strCode.Substring(0, 3).ToUpper();

                            // dtOBJECTC.DeleteRows("APLY_NO='" + strAplyNo + "' and OBJ_CODE='" + strCode + "'");
                            dr = dtOBJECTC.NewRow();
                            dr["APLY_NO"] = strAplyNo;
                            dr["OLD_APLY_NO"] = this.GridObject.Rows[i]["OLD_APLY_NO"].ToString();
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
                        if (this.GridAccs.Rows[i].RowState.ToString() != "Deleted")
                        {
                            string strCode = this.GridAccs.Rows[i]["OBJ_CODE"].ToString();
                            if (strCode != "")
                            {
                                string strSeq = (i + 1).ToString();
                                drCopy = dtCopy.Select("OBJ_CODE='" + strCode + "' and ACCS_SEQ=" + strSeq);
                                string strKeyChk = strCode.Substring(0, 3).ToUpper();
                                if (strKeyChk != "OTC" && strKeyChk != "ORX" && strKeyChk != "XRC")//&& this.GridObject.Rows[i].RowState != DataRowState.Added)
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

                if (strCodeKey != "")
                {


                    DataTable dtOBJECT = dts.GetTable("OR_OBJECT", "OBJ_CODE in (" + strCodeKey + ")");
                    dtCopy = dtOBJECT.Copy();



                    for (int i = 0; i < this.GridObject.Rows.Count; i++)
                    {


                        if (GridObject.Rows[i].RowState != DataRowState.Deleted)
                        {
                            string strCode = GridObject.Rows[i]["OBJ_CODE"].ToString();


                            if (strCode == "")
                                continue;

                            drCopy = dtCopy.Select("OBJ_CODE='" + strCode + "'");
                            string strKeyChk = strCode.Substring(0, 3).ToUpper();
                            if (strKeyChk != "OTC" && strKeyChk != "ORX" && strKeyChk != "XRC")
                            {
                                dtOBJECT.DeleteRows("OBJ_CODE='" + strCode + "'");
                                dr = dtOBJECT.NewRow();

                                dr["OBJ_CODE"] = strCode;
                                dr["OBJ_LOC_ADDR"] = GridObject.Rows[i]["OBJ_LOC_ADDR"].ToString();
                                dr["PROD_NAME"] = GridObject.Rows[i]["PROD_NAME"].ToString();
                                dr["OTC"] = GridObject.Rows[i]["OTC"].ToString();
                                dr["MAC_NO"] = GridObject.Rows[i]["MAC_NO"].ToString();
                                dr["BRAND"] = GridObject.Rows[i]["BRAND"].ToString();
                                dr["FRC_CODE"] = GridObject.Rows[i]["FRC_CODE"].ToString();
                                dr["SALES_UNIT"] = "";

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

                                dtOBJECT.Rows.Add(dr);
                            }

                        }
                        //  dtOBJECT.AcceptChanges();

                    }
                }



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


                strSQL = " select  b.APLY_BOND, b.APLY_DURN_M,b.APLY_PERD,b.APLY_PAY_PERD,";
                strSQL += " b.APLY_DEPS,b.APLY_PAY_MTHD,b.APLY_SERV_CHAR,b.APLY_AMOR_MTHD,";
                strSQL += " b.APLY_HIRE,b.APLY_TAX,";
                strSQL += " b.APLY_LF1_FR,b.APLY_LF1_TO,b.APLY_LF1_HIRE, ";
                strSQL += " b.APLY_LF2_FR,b.APLY_LF2_TO,b.APLY_LF2_HIRE,";
                strSQL += " b.APLY_LF3_FR,b.APLY_LF3_TO,b.APLY_LF3_HIRE,";
                strSQL += " b.APLY_LF4_FR,b.APLY_LF4_TO,b.APLY_LF4_HIRE,";
                strSQL += " b.APLY_LF5_FR,b.APLY_LF5_TO,b.APLY_LF5_HIRE,";


                dr["CASE_TYPE_CODE"] = this.rptBase.Items[0].FindControl("CASE_TYPE_CODE").value();
                dr["APLY_BOND"] = this.rptRequest.Items[0].FindControl("APLY_BOND").value();
                dr["APLY_DURN_M"] = this.rptRequest.Items[0].FindControl("APLY_DURN_M").value();
                dr["APLY_PERD"] = this.rptRequest.Items[0].FindControl("APLY_PERD").value();
                dr["APLY_PAY_PERD"] = this.rptRequest.Items[0].FindControl("APLY_PAY_PERD").value();
                dr["APLY_DEPS"] = this.rptRequest.Items[0].FindControl("APLY_DEPS").value();
                dr["APLY_PAY_MTHD"] = this.rptRequest.Items[0].FindControl("APLY_PAY_MTHD").value();
                dr["APLY_SERV_CHAR"] = this.rptRequest.Items[0].FindControl("APLY_SERV_CHAR").value();
                dr["APLY_AMOR_MTHD"] = this.rptRequest.Items[0].FindControl("APLY_AMOR_MTHD").value();
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

                dr["UPD_USER"] = this.Master.Master.EmployeeName;
                dr["UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                dr["UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                if (dtRequest.Rows.Count == 0)
                    dtRequest.Rows.Add(dr);


                DataTable dtSCUR = dts.GetTable("OR_CASE_APLY_SCUR", "APLY_NO='" + strAplyNo + "'");


                dtCopy = dtSCUR.Copy();
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

            }
        }
        #endregion




        #region tab2 dataprocess 其他申請條件
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
                strSQL = "  select	PAPER,CON_TYPE,";
                strSQL += " a.TMP_CODE,EXPIRED_RENEW,";
                strSQL += " HIRE,TAX,YAR,MTH,PERIOD,";
                strSQL += " PACKAGE,CASHIER,CHG_CON,CHG_CODICIL,PRV_APLY_NO,LIMIT,RESTRICTION_PERIODS,FREE_SHOW_MAC_NO,CODICIL";
                strSQL += " from OR3_CASE_APLY_APLY_COND2 a left join OR_CASE_APLY_BASE b on a.APLY_NO=b.APLY_NO";
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



                dr["CON_TYPE"] = this.rptCon.Items[0].FindControl("CON_TYPE").value();
                dr["TMP_CODE"] = this.rptCon.Items[0].FindControl("TMP_CODE").value();
                dr["EXPIRED_RENEW"] = this.rptCon.Items[0].FindControl("EXPIRED_RENEW").value();
                dr["HIRE"] = this.rptCon.Items[0].FindControl("HIRE").value();
                dr["TAX"] = this.rptCon.Items[0].FindControl("TAX").value();
                dr["YAR"] = decimal.Parse(this.rptCon.Items[0].FindControl("YAR").value().toNumber().ToString());
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
            if (this.bolWA070)
            {
            //    this.Master.Master.nowStatus = "UpdAfter";
            //    strStatus = this.Master.Master.nowStatus;
            }

            if (!DataCheck(strStatus))
                return 0;



            string strAplyNo = "";

            string strDEPT = this.rptBase.Items[0].FindControl("DEPT_CODE").value();
            string strDate = this.rptBase.Items[0].FindControl("APLY_DATE").value().Replace("/", "");

            if (strStatus == "Add" || strStatus == "Copy")
            {
                string STS = dg.GetDataRow("select CASE_STS from OR_CASE_TYPE where CASE_TYPE_CODE='" + this.rptBase.Items[0].FindControl("CASE_TYPE_CODE").value() + "'")[0].ToString();
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

                    //                    this.SaveObjectDetail(this.bolGridAdd);
                    this.tab01_DataProcess(true, strAplyNo);
                    this.tab02_DataProcess(true, strAplyNo);

                    break;


                case "Cancel":
                    dr = dtBase.Rows[0];
                    dr["FAST_STS"] = "3";
                    dr["CUR_STS"] = "F";

                    dr["UPD_USER"] = this.Master.Master.EmployeeName;
                    dr["UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                    dr["UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                    break;

                case "Appove":

                    dr = dtBase.Rows[0];
                    dr["FAST_STS"] = "2";

                    dr["UPD_USER"] = this.Master.Master.EmployeeName;
                    dr["UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                    dr["UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
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