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
    public partial class WC0101 : PageParent
    {

        /// <summary>
        /// 是否由WF010呼叫
        /// </summary>
        /// <returns></returns>
        public bool bolWF0101
        {
            set { ViewState["bolWF0101"] = value; }
            get { return (ViewState["bolWF0101"] == null ? false : (bool)ViewState["bolWF0101"]); }
        }

        public bool bolWA070
        {
            set { ViewState["bolWA070"] = value; }
            get { return (ViewState["bolWA070"] == null ? false : (bool)ViewState["bolWA070"]); }
        }

        protected bool bolQuery
        {
            set { ViewState["bolQuery"] = value; }
            get { return (ViewState["bolQuery"] == null ? false : (bool )ViewState["bolQuery"]); }
        }

        public String nowRow
        {
            set { ViewState["nowRow"] = value; }
            get { return (ViewState["nowRow"] == null ? "" : (String)ViewState["nowRow"]); }
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

            if (this.Request.UrlReferrer.AbsolutePath.IndexOf("WF0101") != -1)
            {
                bolWF0101 = true;

                WC010 myForm = new WC010();
                string strSQL = myForm.callFromWF0101();


                strSQL += " @FRC_CODE=''";
                this.Master.queryString = strSQL;
                this.Master.Master.nowStatus = "Add";
                this.Master.bolExit = false;
            }

            if (Session["bolWA070"] != null)
            {
               
                WC010 myForm = new WC010();
                string strSQL = myForm.callFromWF0101();


                strSQL += " @FRC_CODE='" + (String)Session["FRC_CODE"] + "'";
                this.Master.queryString = strSQL;

                this.Master.Master.nowStatus = "Upd";
                this.bolWA070 = true;
                Session["bolWA070"] = null;
                Session["FRC_CODE"]=null;
            }

            //3.key fields
            this.Master.KeyFields = "FRC_CODE";




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

            if (this.EMP_CODE.Text.Trim() == "")
            {
                this.EMP_CODE.Text = this.Master.Master.CorpAcct;
                this.EMP_NAME.Text = this.Master.Master.EmployeeName;
                
            }
           // this.rptFRC_VER.DataSourceID = "sqlFRC_VER";
          //  this.rptFRC_VER.DataBind();

            if (this.rptFRC_VER.Items.Count > 0)
            {
                Button btn = (Button)this.rptFRC_VER.Items[this.rptFRC_VER.Items.Count - 1].FindControl("btnQuery");
                this.GridFunc_Click(btn, new CommandEventArgs(btn.CommandName, null));
                this.upDetailEditing.Editing(false); 
            }

        }
        #endregion

        public String VER
        {
            set { ViewState["VER"] = value; }
            get { return (ViewState["VER"] == null ? "" : (String)ViewState["VER"]); }
        }

        protected void SaveAndPrint(object sender, System.EventArgs e)
        {
            int intReturn = this.Save_Click(this.Master.Master.nowStatus);
            

            if (intReturn == 0)
            {               
                return;
            }

            this.setProcessMessage("資料儲存" + (intReturn == 1?"失敗":"成功"), intReturn == 1);

            if (intReturn == 2)
                this.Master.returnPage();

    

            //傳參數給報表
            string PRTSERVER = ConfigurationManager.AppSettings["RPTSERVER"].ToString();
            string PRJCODE = ConfigurationManager.AppSettings["PRJCODE"].ToString();

            string FILENAME = "WC040";
            string SYS = this.Master.Master.ProgramId.Substring(0, 2);

            string URL = "http://" + PRTSERVER + "/Smart-Query/squery.aspx?Path=" + PRJCODE + "&filename=" + FILENAME + "&sys=" + SYS;
            URL += "&Parameter1=" + this.FRC_CODE.Text.Trim();
            

            string js = "window.open('" + URL + "','','height=600,width=1024,status=yes,toolbar=yes,menubar=yes,location=no,Resizable = yes','')";

            //指向報表頁面
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "openReport", js, true);

        }


        #region grid function
        protected void GridFunc_Click(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {

            this.bolQuery=false;
            DataTable dtGridSource;
            string strScript = "";
            switch (((Button)sender).ID)
            {
                case "btnAdd_D":
                    this.VER = (this.rptFRC_VER.Items.Count + 1).ToString();
                    this.upDetailEditing.Editing(true);
                    this.QUOTA_AMT.Text = "";
                    this.BUY_WAY.SelectedIndex = -1;
                    this.EXPIRE_PROMISE.Text = "";
                    this.OTH_PROMISE.Text = "";
                    this.SEND_EXAM_IMAGE.Seq = "";
                    this.DEBT_IMAGE.Seq = "";
                    this.SEND_EXAM_IMAGE.bolUpload = true;
                    this.DEBT_IMAGE.bolUpload = true;
                    ViewState["GridSource"] = null;
                    rptEdit.DataSource = this.GetGridSource(false);
                    rptEdit.DataBind();
                     addRETK_DURN_TO.Clearing();
                    addDUE_BUY_RATE.Clearing();
                    this.setRETK_DURN_FR();
                    this.upDetailEditing.Update();
                    break;
                case "btnUpd_D":
                case "btnQuery":
                    this.nowRow = (((RepeaterItem)((Button)sender).Parent).ItemIndex + 1).ToString();
                    strScript = "document.getElementById('tr" + this.nowRow  + "').className='crow';";
                    this.setScript(strScript);
                    this.VER = e.CommandName;

                    string strSQL = "select QUOTA_AMT,SEND_EXAM_IMAGE,DEBT_IMAGE,BUY_WAY,EXPIRE_PROMISE,OTH_PROMISE from OR3_FRC_VER where FRC_CODE='"+ this.FRC_CODE.Text.Trim()+"' and VER='"+ this.VER+"'";
                    DataTable dt = dg.GetDataTable(strSQL);
                    if (dt.Rows.Count>0)
                    {
                        DataRow dr=dt.Rows[0];
                        this.QUOTA_AMT.Text = dr["QUOTA_AMT"].ToString();
                        if (dr["BUY_WAY"].ToString().Trim() != "")
                        {
                            try
                            {
                                this.BUY_WAY.SelectedValue = dr["BUY_WAY"].ToString();
                            }
                            catch { }
                        }
                        else
                            this.BUY_WAY.SelectedIndex = 0;
                        this.SEND_EXAM_IMAGE.Seq = dr["SEND_EXAM_IMAGE"].ToString();
                        this.DEBT_IMAGE.Seq = dr["DEBT_IMAGE"].ToString();
                        this.EXPIRE_PROMISE.Text = dr["EXPIRE_PROMISE"].ToString();
                        this.OTH_PROMISE.Text = dr["OTH_PROMISE"].ToString();
                    }
                    ViewState["GridSource"] = null;
                    rptEdit.DataSource = this.GetGridSource(false);
                    rptEdit.DataBind();
                    if (((Button)sender).ID == "btnUpd_D")
                    {
                        this.upDetailEditing.Editing(true);
                        this.rptEdit.Editing(true);
                        this.SEND_EXAM_IMAGE.bolUpload = true;
                        this.DEBT_IMAGE.bolUpload = true;
                        this.setRETK_DURN_FR();
                        
                    }
                    else
                    {
                        this.bolQuery = true;
                        this.upDetailEditing.Editing(false);
                        this.QUOTA_AMT.Editing(false);
                        this.BUY_WAY.Editing(false);
                        this.EXPIRE_PROMISE.Editing(false);
                        this.OTH_PROMISE.Editing(false);
                        this.SEND_EXAM_IMAGE.Editing(true);
                        this.DEBT_IMAGE.Editing(true);
                        this.SEND_EXAM_IMAGE.bolUpload = false;
                        this.DEBT_IMAGE.bolUpload = false;

                    }
                    this.upDetailEditing.Update();
                    break;
                case "btnSure_D":
                    this.nowRow = (((RepeaterItem)((Button)sender).Parent).ItemIndex + 1).ToString();
                    strScript = "document.getElementById('tr" + this.nowRow + "').className='crow';";
                    this.setScript(strScript);
                    

                    ArrayList alSure = new ArrayList();
                    alSure.Add("update OR3_FRC_VER set FRC_STS='1',Eff_Date='" + DateTime.Now.ToString("yyyyMMdd") + "',APRV_BY='" + this.Master.Master.EmployeeId + "' where FRC_CODE='" + this.FRC_CODE.Text.Trim() + "' and VER='" + e.CommandName + "'");
                    alSure.Add("update OR3_FRC_VER set Exp_Date='" + DateTime.Now.AddDays(-1).ToString("yyyyMMdd") + "' where FRC_CODE='" + this.FRC_CODE.Text.Trim() + "' and VER='" + (e.CommandName.toInt()-1) + "'");
                    if (cts.Execute(alSure))
                    {
                        this.setProcessMessage("版次資料確認完成！", false);
                        this.resetVerGrid();
                    }
                    else
                        this.setProcessMessage("資料處理失敗！", true);
                    
                   

                    break;

                case "btnCSure_D":
                    this.nowRow = (((RepeaterItem)((Button)sender).Parent).ItemIndex + 1).ToString();
                    strScript = "document.getElementById('tr" + this.nowRow + "').className='crow';";
                    this.setScript(strScript);

                    ArrayList alCSure = new ArrayList();
                    alCSure.Add("update OR3_FRC_VER set FRC_STS='0',Eff_Date='',APRV_BY='' where FRC_CODE='" + this.FRC_CODE.Text.Trim() + "' and VER='" + e.CommandName + "'");
                    alCSure.Add("update OR3_FRC_VER set Exp_Date='' where FRC_CODE='" + this.FRC_CODE.Text.Trim() + "' and VER='" + (e.CommandName.toInt()-1) + "'");

                    if (cts.Execute(alCSure))
                    {
                        this.setProcessMessage("版次資料確認取消完成！", false);
                        this.resetVerGrid();
                    }
                    else
                        this.setProcessMessage("資料處理失敗！", true);
                    break;

                case "btnDel_D":
                    this.nowRow = (this.rptFRC_VER.Items.Count-1).ToString();
                    ArrayList alDel = new ArrayList();
                    alDel.Add("delete OR3_FRC_VER  where FRC_CODE='" + this.FRC_CODE.Text.Trim() + "' and VER='" + e.CommandName + "'");
                    alDel.Add("delete OR_FRC_OBJ_BUY_SET  where FRC_CODE='" + this.FRC_CODE.Text.Trim() + "' and VER='" + e.CommandName + "'");

                    
                    if (cts.Execute(alDel))
                    {
                        this.setProcessMessage("版次資料刪除完成！", false);
                        this.resetVerGrid();
                    }
                    else
                        this.setProcessMessage("資料處理失敗！", true);
                    break;

                case "btnDel":
                    dtGridSource = (DataTable)ViewState["GridSource"];
                    string RETK_DURN_FR = e.CommandName;
                    DataRow drFR = dtGridSource.Select("RETK_DURN_FR='" + RETK_DURN_FR + "'")[0];
                    drFR["STATUS"] = "D";
                    rptEdit.DataSource = dtGridSource;
                    rptEdit.DataBind();
                    this.upDetailEditing.Update();
                    this.setRETK_DURN_FR();
                     
                    break;

                case "btnAdd":
                    string strMessage = "";
                    if (this.addRETK_DURN_FR.Text.Trim() == "0")
                        strMessage += "[取回期間-起(月數)]";

                    if (this.addRETK_DURN_TO.Text.Trim() == "0")
                        strMessage += "[取回期間-迄(月數)]";

                    if (this.addDUE_BUY_RATE.Text.Trim() == "0")
                        strMessage += "[買回比率(%)]";

             
                    if (strMessage != "")
                    {
                        strMessage += " 必須輸入！";

                        strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                        this.setMessageBox(strMessage);
                        return;
                    }

                    if (this.addRETK_DURN_TO.Text.Trim().toInt() < this.addRETK_DURN_FR.Text.Trim().toInt())
                    {
                        this.setMessageBox("[取回期間-迄(月數)]比須大於[取回期間-起(月數)]");
                        return ;
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

                    this.setRETK_DURN_FR();

                    
                    addRETK_DURN_TO.Clearing();
                    addDUE_BUY_RATE.Clearing();
                    this.upDetailEditing.Update();
                    break;

                case "btnEdit":
                    this.SaveGrid();
                    break;

                    //還沒確認的可以修改, 已確認的最新版可以取消,
                      //  都確認過的, 才可以新增

            }
        }


        private bool SaveGrid()
        {
            if (this.VER == "" || this.EXPIRE_PROMISE.CssClass.ToLower().IndexOf("slock")!=-1 )
                return true;
            

            DataTable dtVER = dts.GetTable("OR3_FRC_VER", "FRC_CODE='" + this.FRC_CODE.Text.rpsText() + "' and VER='"+ this.VER +"'");
            DataTable dtOBJ = dts.GetTable("OR_FRC_OBJ_BUY_SET", "FRC_CODE='" + this.FRC_CODE.Text.rpsText() + "' and VER='" + this.VER + "'");
            DataTable dtF = dts.GetTable("OR3_FILE_STORE", "FILE_SEQ='" + this.SEND_EXAM_IMAGE.Seq + "' or FILE_SEQ='" + this.DEBT_IMAGE.Seq + "'");

            byte[] BINARY_FILE1 = this.SEND_EXAM_IMAGE.bImage;
            string strExtName1 = this.SEND_EXAM_IMAGE.ExtName;

            byte[] BINARY_FILE2 = this.DEBT_IMAGE.bImage;
            string strExtName2 = this.DEBT_IMAGE.ExtName;
            DataRow dr = null;

            if (strExtName1 != "")
            {
                if (dtF.Select("FILE_SEQ='" + this.SEND_EXAM_IMAGE.Seq + "'").Length == 0)
                {
                    dr = dtF.NewRow();
                    dr["FILE_SEQ"] = this.SEND_EXAM_IMAGE.Seq;
                    dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                    dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                    dr["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                }
                else
                    dr = dtF.Select("FILE_SEQ='" + this.SEND_EXAM_IMAGE.Seq + "'")[0];

                dr["FILE_TYPE"] = strExtName1;
                dr["BINARY_FILE"] = BINARY_FILE1;                
                dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");

                if (dtF.Select("FILE_SEQ='" + this.SEND_EXAM_IMAGE.Seq + "'").Length == 0)
                    dtF.Rows.Add(dr);
            }

            if (strExtName2 != "")
            {
                if (dtF.Select("FILE_SEQ='" + this.DEBT_IMAGE.Seq + "'").Length == 0)
                {
                    dr = dtF.NewRow();
                    dr["FILE_SEQ"] = this.DEBT_IMAGE.Seq;
                    dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                    dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                    dr["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                }
                else
                    dr = dtF.Select("FILE_SEQ='" + this.DEBT_IMAGE.Seq + "'")[0];

                dr["FILE_TYPE"] = strExtName2;
                dr["BINARY_FILE"] = BINARY_FILE2;
                
                dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                
                if (dtF.Select("FILE_SEQ='" + this.DEBT_IMAGE.Seq + "'").Length == 0)
                    dtF.Rows.Add(dr);
            }

            //新增版次後新增標的物檔
            if (dtVER.Rows.Count == 0)
            {
                dr = dtVER.NewRow();
                dr["FRC_CODE"] = this.FRC_CODE.Text;
                dr["VER"] = this.VER;
                dr["FRC_STS"] = "0";
                dr["Eff_Date"] = "";
                dr["EXP_Date"] = "";
                dr["APRV_BY"] = "";
                dr["APRV_BOOK_IMAGE"] = "";
                dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                dr["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
            }
            else
                dr = dtVER.Rows[0];


            dr["QUOTA_AMT"] = this.QUOTA_AMT.Text;
            dr["BUY_WAY"] = this.BUY_WAY.SelectedValue;
            dr["EXPIRE_PROMISE"] = this.EXPIRE_PROMISE.Text ;
            dr["OTH_PROMISE"] = this.OTH_PROMISE.Text;
            dr["SEND_EXAM_IMAGE"] = this.SEND_EXAM_IMAGE.Seq;
            dr["DEBT_IMAGE"] = this.DEBT_IMAGE.Seq;
           
            dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
            dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
            dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
            if (dtVER.Rows.Count == 0)
                dtVER.Rows.Add(dr);


            this.updateGrid(this.GetGridSource(false), rptEdit);

            /*END 勿動 更新Grid*/

            

            for (int i = 0; i < this.rptEdit.Items.Count; i++)
            {
                string STATUS = this.rptEdit.Items[i].FindControl("STATUS").value();
                string RETK_DURN_FR = this.rptEdit.Items[i].FindControl("RETK_DURN_FR").value();
                string RETK_DURN_TO = this.rptEdit.Items[i].FindControl("RETK_DURN_TO").value().Replace("/", "");
                string DUE_BUY_RATE = this.rptEdit.Items[i].FindControl("DUE_BUY_RATE").value().Replace("/", "");

                string strMessage = "";
                if (RETK_DURN_FR == "0")
                    strMessage += "[取回期間-起(月數)]";

                if (RETK_DURN_TO == "0")
                    strMessage += "[取回期間-迄(月數)]";

                if (DUE_BUY_RATE == "0")
                    strMessage += "[買回比率(%)]";

                if (strMessage != "")
                {
                    strMessage += " 必須輸入！";                    
                    this.setMessageBox(strMessage);
                    return false;
                }
                if (RETK_DURN_TO.toInt() < RETK_DURN_FR.toInt())
                {
                    this.setMessageBox("[取回期間-迄(月數)]比須大於[取回期間-起(月數)]");
                    return false;
                }
                switch (STATUS)
                {
                    case "A":


                        dr = dtOBJ.NewRow();
                        dr["FRC_CODE"] = this.FRC_CODE.Text;
                        dr["VER"] = this.VER;
                        dr["RETK_DURN_FR"] = RETK_DURN_FR;
                        dr["RETK_DURN_TO"] = RETK_DURN_TO;
                        dr["DUE_BUY_RATE"] = DUE_BUY_RATE;
                        dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["ADD_TIME"] = System.DateTime.Now.ToString("HHmmss");
                        dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HHmmss");
                        dtOBJ.Rows.Add(dr);

                        

                        break;

                    case "U":
                        dr = dtOBJ.Select("RETK_DURN_FR='" + RETK_DURN_FR + "'")[0];
                        dr["RETK_DURN_TO"] = RETK_DURN_TO;
                        dr["DUE_BUY_RATE"] = DUE_BUY_RATE;
                        dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HHmmss");

                        break;

                    case "D":
                        dtOBJ.DeleteRows("RETK_DURN_FR='" + RETK_DURN_FR + "'");
                        break;
                }
            }

            return true;
          //  if (dts.Save())
          //      this.setProcessMessage("版次資料儲存完成！", false);
          //  else
          //      this.setProcessMessage("版次資料儲存失敗！", true);

        }


        private string GetGridSql()
        {
            string strSQL = "";
            strSQL += "select RETK_DURN_FR,RETK_DURN_TO,DUE_BUY_RATE from OR_FRC_OBJ_BUY_SET where FRC_CODE='"+ this.FRC_CODE.Text.rpsText()+"' and VER='"+ this.VER +"'";

            return strSQL;
        }

        /// <summary>
        /// 設定起始月數
        /// </summary>
        private void  setRETK_DURN_FR(){
            DataTable dtd = this.GetGridSource(false);
            double intData = dtd.Compute("max(RETK_DURN_TO)", "STATUS<>'D'").ToString().toNumber() + 1;
            this.addRETK_DURN_FR.Text = intData.ToString();
        }

        protected void BUY_WAY_Changed(object sender, EventArgs e)
        {
            this.setRETK_DURN_FR();

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

            if (bolAdd)
            {
                dtGridSource = (DataTable)ViewState["GridSource"];
                this.updateGrid(dtGridSource, rptEdit);
                DataRow dtRow = dtGridSource.NewRow();
                dtRow["RETK_DURN_FR"] = addRETK_DURN_FR.Text.Trim();
                dtRow["RETK_DURN_TO"] = addRETK_DURN_TO.Text.Trim();
                dtRow["DUE_BUY_RATE"] = addDUE_BUY_RATE.Text.Trim();
                dtRow["Status"] = "A";
                this.setRETK_DURN_FR();
                dtGridSource.Rows.Add(dtRow);
                ViewState["GridSource"] = dtGridSource;
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
            string strSQL = "";
            string strMessage = "";


            switch (strStatus)
            {
                case "Add":
                case "Copy":
                case "Upd":
                    if (this.FRC_CODE.Text.Trim() == "")
                        strMessage += "[供應商代號]";

                    if (this.FRC_NAME.Text.Trim() == "")
                        strMessage += "[供應商名稱]";

                    if (this.FRC_SNAME.Text.Trim() == "")
                        strMessage += "[供應商簡稱]";


                    if (this.Master.Master.nowStatus == "Add")
                    {
                        if (dg.GetDataRow("select 'Y' from OR_FRC where FRC_CODE='" + this.FRC_CODE.Text.Trim() + "'")[0].ToString() == "Y")
                        {
                            this.setMessageBox("供應商代號已存在,請確認!");
                            return false;
                        }
                    }

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
            if (this.bolWA070)
            {
                this.Master.Master.nowStatus = "Upd";
                strStatus = this.Master.Master.nowStatus;
            }

            if (!DataCheck(strStatus))
                return 0;


            DataTable dt = dts.GetTable("OR_FRC", "FRC_CODE='" + this.FRC_CODE.Text.rpsText() + "'");
            

            DataRow dr = null;

            if (bolWF0101)
                strStatus = "Add";

            
            switch (strStatus)
            {

                case "Add":
                case "Copy":
                case "Upd":
                    if (dt.Rows.Count == 0)
                    {
                        dr = dt.NewRow();
                        dr["FRC_CODE"] = this.FRC_CODE.Text.Trim();
                        dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                    }
                    else
                        dr = dt.Rows[0];

                
                    dr["FRC_SNAME"] = this.FRC_SNAME.Text.Trim();
                    dr["BLOC_NO"] = this.BLOC_NO.Text.Trim();
                    dr["BANK_CODE"] = this.BANK_CODE.Text.Trim();
                    dr["BANK_ACCT"] = this.BANK_ACCT.Text.Trim();
                    dr["FRC_NAME"] = this.FRC_NAME.Text.Trim();
                    dr["CONTACT"] = this.CONTACT.Text.Trim();
                    dr["UNIF_NO"] = this.UNIF_NO.Text.Trim();
                    dr["TAKER"] = this.TAKER.Text.Trim();
                    dr["PHONE1"] = this.PHONE1.Text.Trim();
                    dr["PHONE2"] = this.PHONE2.Text.Trim();
                    dr["FACSIMILE"] = this.FACSIMILE.Text.Trim();
                    dr["SALES_RGT_ADDR"] = this.SALES_RGT_ADDR.Text.Trim();
                    dr["CTAC_ADDR"] = this.CTAC_ADDR.Text.Trim();
                    dr["EMP_PSNS"] = this.EMP_PSNS.Text.Trim();
                    dr["BUILD_DATE"] = this.BUILD_DATE.Text.Replace("/","");
                    dr["CUST_EMAIL_ADDR"] = this.CUST_EMAIL_ADDR.Text.Trim();
                    dr["REAL_CAPT_AMT"] = this.REAL_CAPT_AMT.Text.Trim();
                    dr["RGT_CAPT_AMT"] = this.RGT_CAPT_AMT.Text.Trim();
                    dr["TURNOVER_YM_S"] = this.TURNOVER_YM_S.Text.Trim();
                    dr["TURNOVER_YM_E"] = this.TURNOVER_YM_E.Text.Trim();
                    dr["TURNOVER"] = this.TURNOVER.Text.Trim();
                    dr["COOR_SUBJECT"] = this.COOR_SUBJECT.Text.Trim();
                    dr["MAIN_PROD_DESC"] = this.MAIN_PROD_DESC.Text.Trim();
                    dr["FRC_BACKGROUND"] = this.FRC_BACKGROUND.Text.Trim();

                    dr["EMP_CODE"] = this.convertToEmployeeCode(this.EMP_CODE.Text.Trim());
                    dr["HAND_OPINION"] = this.HAND_OPINION.Text.Trim(); 

                    dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                    dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                    dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                    if (dt.Rows.Count == 0)
                        dt.Rows.Add(dr);

                   

                    break;
                case "Del":
                    dt.DeleteRows();

                    break;

            }

            if (!this.SaveGrid())
                return 0;

            if (dts.Save())
            {
                this.resetVerGrid();

                if (bolWF0101)
                {
                    this.setScript("window.parent.setData('Frc','" + this.FRC_CODE.Text.Trim() + "','" + this.FRC_NAME.Text.Trim() + "');");
                    this.setProcessMessage("新增處理成功!", false);
                    this.Master.setParms("Add");
                    this.setParms();
                    return 0;

                }

                return 2;
            }
            else
            {
                if (bolWF0101)
                {
                    this.setProcessMessage("新增處理失敗!", false);
                    return 0;

                }
                return 1;
            }
        }
        #endregion



        private void resetVerGrid()
        {
            this.rptFRC_VER.DataBind();
            this.upDetailQuery.Update();
            this.upDetailEditing.Clearing();
            this.VER = "";
            this.DEBT_IMAGE.ExtName = "";
            this.DEBT_IMAGE.Seq = "";
            this.DEBT_IMAGE.MIME = "";
            this.DEBT_IMAGE.bImage = null;
            this.SEND_EXAM_IMAGE.ExtName = "";
            this.SEND_EXAM_IMAGE.Seq = "";
            this.SEND_EXAM_IMAGE.MIME = "";
            this.SEND_EXAM_IMAGE.bImage = null;

            if (this.rptFRC_VER.Items.Count > 0)
            {
                Button btn = (Button)this.rptFRC_VER.Items[this.rptFRC_VER.Items.Count - 1].FindControl("btnQuery");
                this.GridFunc_Click(btn, new CommandEventArgs(btn.CommandName, null));
                this.upDetailEditing.Editing(false);

            }
            else
            {

                this.rptEdit.DataSource = null;
                this.rptEdit.DataBind();
                this.upDetailEditing.Update();
            }

            if (!bolWF0101)
            {

                string strScript = "document.getElementById('tr" + this.nowRow + "').className='crow';";
                this.setScript(strScript);
            }
        }
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