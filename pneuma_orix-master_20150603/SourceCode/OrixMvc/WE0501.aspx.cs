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
    public partial class WE0501 : PageParent
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


            DataTable tb = this.GetGridSource(false);
            this.rptEdit.DataSource = tb;
            this.rptEdit.DataBind();

            this.setGridAddField();

            if (this.bolWE020 && this.Master.Master.nowStatus == "Add")
            {
                this.CUST_NO.Value= ((String)Session["CUST_NO"]);
                this.APLY_NO.Text = ((String)Session["APLY_NO"]);                               
                
                Session["APLY_NO"] = null;
                Session["CUST_NO"] = null;
                Session["CUST_NAME"] = null;
            }

        }


        private void setGridAddField()
        {
            this.addREC_DATE.Text = System.DateTime.Now.ToString("yyyy/MM/dd");

            DataTable dt= (DataTable)ViewState["GridSource"];
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[dt.Rows.Count - 1];

                string nbr = dr["PAPER_NBR"].ToString();

                for (int i = 0; i < nbr.Length; i++)
                {
                    if (!nbr.Substring(0, 1).IsNumeric())
                        nbr = nbr.Substring(1);
                    else
                        break;
                }

                this.addPAPER_NBR.Text = dr["PAPER_NBR"].ToString().Substring(0,dr["PAPER_NBR"].ToString().Length-nbr.Length)+ (nbr.toInt() + 1).ToString();
                DateTime time = new DateTime(dr["DUE_DAT"].ToString().Substring(0, 4).toInt(), dr["DUE_DAT"].ToString().Substring(5, 2).toInt(), dr["DUE_DAT"].ToString().Substring(8, 2).toInt());
                this.addDUE_DAT.Text = time.AddMonths(1).ToString("yyyy/MM/dd");
                time = new DateTime(dr["PRJ_CASH_DATE"].ToString().Substring(0, 4).toInt(), dr["PRJ_CASH_DATE"].ToString().Substring(5, 2).toInt(), dr["PRJ_CASH_DATE"].ToString().Substring(8, 2).toInt());
                this.addPRJ_CASH_DATE.Text = time.AddMonths(1).ToString("yyyy/MM/dd");
            }
        }

        #endregion

        
       
       

        #region grid function
        protected void GridFunc_Click(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
           
            DataTable dtGridSource;
            string strScript = "";
            switch (((Button)sender).ID)
            {

                case "btnDel":
                    dtGridSource = (DataTable)ViewState["GridSource"];
                    string strPAPER_NBR = e.CommandName;
                    DataRow drFR = dtGridSource.Select("PAPER_NBR='" + strPAPER_NBR + "'")[0];
                    drFR["STATUS"] = "D";
                    rptEdit.DataSource = dtGridSource;
                    rptEdit.DataBind();
                   // this.upDetailEditing.Update();


                    break;

                case "btnAdd":
                    string strMessage = "";
                    if (this.addREC_DATE.Text.Trim() == "")
                        strMessage += "[收票日期 ]";

                    if (this.addPAPER_NBR.Text.Trim() == "")
                        strMessage += "[票據號碼]";

                    if (this.addPAPER_AMT.Text.Trim() == "0" || this.addPAPER_AMT.Text.Trim() == "")
                        strMessage += "[票據金額]";

                    if (this.addDUE_DAT.Text.Trim() == "")
                        strMessage += "[到期日期]";

                    if (this.addPRJ_CASH_DATE.Text.Trim() == "")
                        strMessage += "[預估兌現日]";

                    if (this.addCHEQ_KIND.Text.Trim() == "")
                        strMessage += "[票據種類]";


                    if (strMessage != "")
                    {
                        strMessage += " 必須輸入！";

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
                    }
                    else
                    {
                        rptEdit.DataSource = null;
                        rptEdit.DataBind();

                    }

                    /*
                    2.新增筆數時, 帶出上一筆資料之所有欄位值, 有3欄位的資料需運算,
票據號碼=上筆+1
到期日期=上筆+1個月
預估兌現日=上筆+1個月
其他欄位預設與上筆資料同

                  //  this.addREC_DATE.Clearing();
                  //  this.addPAPER_NBR.Clearing();
                  //  this.addPAPER_AMT.Clearing();
                  //  this.addDUE_DAT.Clearing();
                  //  this.addPRJ_CASH_DATE.Clearing();
                  //  this.addCHEQ_KIND.Clearing();
                  //  this.addBANK_NO.Clearing();
                  //  this.addBANK_NAME.Clearing();
                    this.addINV_NAME.Clearing();
                    this.addACCOUNT.Clearing();
                    this.addENDORSOR.Clearing();
                    this.addREMARK.Clearing();*/
                    this.setGridAddField();
                
                   // this.upDetailEditing.Update();
                    break;

                case "btnEdit":
                    if (SaveGrid())
                    {
                        
                        this.setProcessMessage("資料處理成功!", false);
                        this.setScript("window.parent.closePopUpWindow();");
                    }
                    else
                    {
                        this.setProcessMessage("資料處理失敗!", false);
                    }
                    break;

                case "btnMaintain":
                    ViewState["GridSource"] = null;
                    dtGridSource = this.GetGridSource(false);
                    rptEdit.DataSource = dtGridSource;
                    rptEdit.DataBind();
                   // this.upDetailEditing.Update();
                    this.setScript("window.parent.openPopUpWindow();");
                    break;
            }
        }


        private bool SaveGrid()
        {

            DataTable dtPAPER = dts.GetTable("OR3_RECV_PAPER_TEMP", "APLY_NO='" + this.APLY_NO.Text.rpsText() + "'");
            DataRow dr;

            this.updateGrid(this.GetGridSource(false), rptEdit);
            DataTable dtEdit=this.GetGridSource(false);

            /*END 勿動 更新Grid*/

            

            for (int i = 0; i < this.rptEdit.Items.Count; i++)
            {
                string STATUS = this.rptEdit.Items[i].FindControl("STATUS").value();
                string REC_DATE = this.rptEdit.Items[i].FindControl("REC_DATE").value().Replace("/", "");
                string oldPAPER_NBR = this.rptEdit.Items[i].FindControl("oldPAPER_NBR").value();
                string PAPER_NBR = this.rptEdit.Items[i].FindControl("PAPER_NBR").value();
                string PAPER_AMT = this.rptEdit.Items[i].FindControl("PAPER_AMT").value();
                string DUE_DAT = this.rptEdit.Items[i].FindControl("DUE_DAT").value().Replace("/","");
                string PRJ_CASH_DATE = this.rptEdit.Items[i].FindControl("PRJ_CASH_DATE").value().Replace("/", "");
                string CHEQ_KIND = this.rptEdit.Items[i].FindControl("CHEQ_KIND").value();
                string BANK_NO = this.rptEdit.Items[i].FindControl("BANK_NO").value();
                string INV_NAME = this.rptEdit.Items[i].FindControl("INV_NAME").value();
                string ACCOUNT = this.rptEdit.Items[i].FindControl("ACCOUNT").value();
                string ENDORSER = this.rptEdit.Items[i].FindControl("ENDORSOR").value();
                
                string REMARK = this.rptEdit.Items[i].FindControl("REMARK").value();

               
                string strMessage = "";
                if (REC_DATE == "")
                    strMessage += "[收票日期 ]";

                if (PAPER_NBR == "")
                    strMessage += "[票據號碼]";

                if (PAPER_AMT == "0" || this.addPAPER_AMT.Text.Trim() == "")
                    strMessage += "[票據金額]";

                if (DUE_DAT == "")
                    strMessage += "[到期日期]";

                if (PRJ_CASH_DATE == "")
                    strMessage += "[預估兌現日]";

                if (CHEQ_KIND == "")
                    strMessage += "[票據種類]";


                if (strMessage != "")
                {
                    strMessage+= " 必須輸入！";

                    strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                    this.setMessageBox(strMessage);
                    return false;
                }

                if (dtEdit.Select("PAPER_NBR='" + PAPER_NBR + "'").Length > 1)
                {                    
                    this.setMessageBox(" 票據號碼["+PAPER_NBR+"]重複輸入，請確認！");
                    return false;
                }
                int intCnt = dg.GetDataRow("select count(*) from (select 'Y' F from OR3_RECV_PAPER_TEMP where PAPER_NBR='" + PAPER_NBR + "' union all select 'Y' F from OR_RECV_PAPER where APLY_NO='"+ this.APLY_NO.Text.Trim()+"' and PAPER_NBR='" + PAPER_NBR + "') s")[0].ToString().toInt();
                if (intCnt>0)
                {
                    if ((STATUS == "A" && intCnt > 0) || (STATUS == "U" && intCnt > 1))
                    {
                        this.setMessageBox(" 票據號碼[" + PAPER_NBR + "]重複輸入，請確認！");
                        return false;
                    }
                }

              

                switch (STATUS)
                {
                    case "A":
                    case "U":

                        if (STATUS == "A")
                        {
                            dr = dtPAPER.NewRow();
                            dr["APLY_NO"] = this.APLY_NO.Text.Trim() ;
                            dr["CUST_NO"] = this.CUST_NO.Value;
                            dr["REC_DATE"] = REC_DATE;
                            dr["PAPER_NBR"] = PAPER_NBR;
                            dr["EMP_CODE"] = this.Master.Master.EmployeeId;
                            dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                            dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                            dr["ADD_TIME"] = System.DateTime.Now.ToString("HHmmss");
                        }
                        else
                            dr = dtPAPER.Select("PAPER_NBR='" + oldPAPER_NBR + "'")[0];

                        dr["PERIOD"] = this.rptEdit_N.Items.Count+(i+1).ToString();
                        dr["PAPER_AMT"] = PAPER_AMT;
                        dr["DUE_DAT"] = DUE_DAT;
                        dr["PRJ_CASH_DATE"] = PRJ_CASH_DATE;
                        dr["CHEQ_KIND"] = CHEQ_KIND;
                        dr["BANK_NO"] = BANK_NO;
                        dr["INV_NAME"] = INV_NAME;
                        dr["ACCOUNT"] = ACCOUNT;
                        dr["ENDORSOR"] = ENDORSER;
                        dr["REMARK"] = REMARK;
                        dr["PAPER_NBR"] = PAPER_NBR;

                        dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HHmmss");
                        if (STATUS == "A")
                        {
                            dtPAPER.Rows.Add(dr);
                        }


                        break;

                

                    case "D":
                        dtPAPER.DeleteRows("PAPER_NBR='" + PAPER_NBR + "'");
                        break;
                }
            }

            if (!dts.Save())
                return false;
            

            return true;

        }


        private string GetGridSql()
        {
            string strSQL = "";
            strSQL += " select oldPAPER_NBR=PAPER_NBR,REC_DATE=dbo.f_DateAddSlash(REC_DATE),PAPER_NBR,PAPER_AMT,DUE_DAT=dbo.f_DateAddSlash(DUE_DAT),PRJ_CASH_DATE=dbo.f_DateAddSlash(PRJ_CASH_DATE),";
            strSQL += " CHEQ_KIND,case cheq_kind when '1' then '1.支票' when '2' then '2.匯票' when '3' then '3.客票' end kind_name,";
            strSQL += " r.BANK_NO,b.bank_name,INV_NAME,ACCOUNT,ENDORSOR,REMARK,SIGN1,SIGN1_DATE=dbo.f_DateAddSlash(SIGN1_DATE),SIGN2,SIGN2_DATE=dbo.f_DateAddSlash(SIGN2_DATE),period";
            strSQL += " from OR_RECV_PAPER r left join ACC18 b on r.bank_no=b.bank_no";
            strSQL += " where aply_no='" + this.APLY_NO.Text.Trim() + "'";
            strSQL += " union all";
            strSQL += " select oldPAPER_NBR=PAPER_NBR,REC_DATE=dbo.f_DateAddSlash(REC_DATE),PAPER_NBR,PAPER_AMT,DUE_DAT=dbo.f_DateAddSlash(DUE_DAT),PRJ_CASH_DATE=dbo.f_DateAddSlash(PRJ_CASH_DATE),";
            strSQL += " CHEQ_KIND,case cheq_kind when '1' then '1.支票' when '2' then '2.匯票' when '3' then '3.客票' end kind_name,";
            strSQL += " r.BANK_NO,b.bank_name,INV_NAME,ACCOUNT,ENDORSOR,REMARK,SIGN1,SIGN1_DATE=dbo.f_DateAddSlash(SIGN1_DATE),SIGN2,SIGN2_DATE=dbo.f_DateAddSlash(SIGN2_DATE),period";
            strSQL += " from OR3_RECV_PAPER_TEMP r left join ACC18 b on r.bank_no=b.bank_no";
            strSQL += " where aply_no='" + this.APLY_NO.Text.Trim() + "' and isnull(SIGN1,'')!='' order by PERIOD";
            this.rptEdit_N.DataSource = dg.GetDataTable(strSQL);
            this.rptEdit_N.DataBind();

            strSQL="";
            strSQL += " select oldPAPER_NBR=PAPER_NBR,REC_DATE=dbo.f_DateAddSlash(REC_DATE),PAPER_NBR,PAPER_AMT,DUE_DAT=dbo.f_DateAddSlash(DUE_DAT),PRJ_CASH_DATE=dbo.f_DateAddSlash(PRJ_CASH_DATE),";
            strSQL += " CHEQ_KIND,case cheq_kind when '1' then '1.支票' when '2' then '2.匯票' when '3' then '3.客票' end kind_name,";
            strSQL += " r.BANK_NO,b.bank_name,INV_NAME,ACCOUNT,ENDORSOR,REMARK,SIGN1,SIGN1_DATE=dbo.f_DateAddSlash(SIGN1_DATE),SIGN2,SIGN2_DATE=dbo.f_DateAddSlash(SIGN2_DATE),period";
            strSQL += " from OR3_RECV_PAPER_TEMP r left join ACC18 b on r.bank_no=b.bank_no";
            strSQL += " where aply_no='" + this.APLY_NO.Text.Trim() + "' and isnull(SIGN1,'')='' order by PERIOD";

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

            if (bolAdd)
            {
                dtGridSource = (DataTable)ViewState["GridSource"];
                this.updateGrid(dtGridSource, rptEdit);
                DataRow dtRow = dtGridSource.NewRow();
                dtRow["REC_DATE"] = addREC_DATE.Text.Trim();
                dtRow["PAPER_NBR"] = addPAPER_NBR.Text.Trim();
                dtRow["PAPER_AMT"] = addPAPER_AMT.Text.Trim();
                dtRow["DUE_DAT"] = addDUE_DAT.Text.Trim();
                dtRow["PRJ_CASH_DATE"] = addPRJ_CASH_DATE.Text.Trim();
                dtRow["CHEQ_KIND"] = addCHEQ_KIND.SelectedValue.Trim();
                dtRow["BANK_NO"] = addBANK_NO.Text.Trim();
                dtRow["BANK_NAME"] = addBANK_NAME.Text.Trim();
                dtRow["INV_NAME"] = addINV_NAME.Text.Trim();
                dtRow["ACCOUNT"] = addACCOUNT.Text.Trim();
                dtRow["ENDORSOR"] = this.addENDORSOR.Text.Trim();
                dtRow["REMARK"] = addREMARK.Text.Trim();
                dtRow["Status"] = "A";

                dtGridSource.Rows.Add(dtRow);
                ViewState["GridSource"] = dtGridSource;
            }

            return dtGridSource;
        }
        #endregion

        #region function click for evey button in client
        protected void Function_Click(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {

            string strMessage = "";
            string strSQL="";
            ArrayList al=new ArrayList();

            switch (e.CommandName)
            {
                case "GetDetail":
                    this.setScript("window.parent.openPopUpWindow();");

                    if (this.GAPLY_NO.Text.Trim() == "")
                    {
                        strMessage += "[申請書編號] 必須輸入！";

                        strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                        this.setMessageBox(strMessage);
                        return;
                    }
                    if (this.GAPLY_NO.Text.Trim() == this.APLY_NO.Text.Trim())
                    {
                        strMessage += "[申請書編號] 不得與新約申請書編號相同！";

                        strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                        this.setMessageBox(strMessage);
                        return;
                    }
                    strSQL += " select oldPAPER_NBR=PAPER_NBR,REC_DATE=dbo.f_DateAddSlash(REC_DATE),PAPER_NBR,PAPER_AMT,DUE_DAT=dbo.f_DateAddSlash(DUE_DAT),PRJ_CASH_DATE=dbo.f_DateAddSlash(PRJ_CASH_DATE),";
                    strSQL += " CHEQ_KIND,case cheq_kind when '1' then '1.支票' when '2' then '2.匯票' when '3' then '3.客票' end kind_name,";
                    strSQL += " r.BANK_NO,b.bank_name,INV_NAME,ACCOUNT,ENDORSOR,REMARK";
                    strSQL += " from OR_RECV_PAPER r left join ACC18 b on r.bank_no=b.bank_no ";
                    strSQL += " where aply_no='" + this.GAPLY_NO.Text.rpsText() + "' ";
                    this.dtPAPER = dg.GetDataTable(strSQL);

                    this.rptDialog.DataSource = dtPAPER;
                    this.rptDialog.DataBind();
                    this.upGetDetail.Update();
                    

                    break;
                case "GetPaper":
                    this.setScript("window.parent.openPopUpWindow();");
                    this.GAPLY_NO.Text = "";
                    this.rptDialog.DataSource = null;
                    this.rptDialog.DataBind();
                    this.upGetDetail.Update();

                    break;
                case "Sign1":
                    strSQL = "update OR3_RECV_PAPER_TEMP set Sign1='" + this.Master.Master.EmployeeName + "',Sign1_DATE='" + System.DateTime.Now.ToString("yyyyMMdd") + "'";
                    strSQL += ",LAST_UPD_USER_ID='" + this.Master.Master.EmployeeName + "'";
                    strSQL += ",LAST_UPD_DATE='" + System.DateTime.Now.ToString("yyyyMMdd") + "'";
                    strSQL += ",LAST_UPD_TIME='" + System.DateTime.Now.ToString("HH:mm:ss") + "'";
                    strSQL += " where APLY_NO='" + this.APLY_NO.Text.rpsText() + "'";
                    strSQL += " and isnull(Sign1,'')=''";
                    if (cts.Execute(strSQL))
                    {
                        this.setProcessMessage("業管課簽收處理成功!", false);
                        this.Master.returnPage();
                    }
                    else
                        this.setProcessMessage("業管課簽收處理失敗!", true);


                    break;
                case "Sign1Back":
                case "Sign2Back":
                    strSQL = "update OR3_RECV_PAPER_TEMP set Sign1='',Sign1_DATE=''";
                    strSQL += ",LAST_UPD_USER_ID='" + this.Master.Master.EmployeeName + "'";
                    strSQL += ",LAST_UPD_DATE='" + System.DateTime.Now.ToString("yyyyMMdd") + "'";
                    strSQL += ",LAST_UPD_TIME='" + System.DateTime.Now.ToString("HH:mm:ss") + "'";
                    strSQL += " where APLY_NO='" + this.APLY_NO.Text.rpsText() + "'";
                    if (cts.Execute(strSQL))
                    {
                        this.setProcessMessage((e.CommandName == "Sign1Back" ? "業管" : "資金") + "課退回處理成功!", false);
                        this.Master.returnPage();
                        
                    }
                    else
                        this.setProcessMessage((e.CommandName == "Sign1Back" ? "業管" : "資金") + "課退回處理失敗!", true);

                    break;
                case "Sign2":

                    if (dg.GetDataRow("select count(*) from OR3_RECV_PAPER_TEMP  where APLY_NO='" + this.APLY_NO.Text.rpsText() + "' and isnull(SIGN1,'')=''")[0].ToString().toInt()>0)
                    {
                        this.setMessageBox("尚有業管課未簽收票據, 無法簽收!");
                        return;
                    }

                    //1.更新暫存檔
                    strSQL = "update OR3_RECV_PAPER_TEMP set Sign2='" + this.Master.Master.EmployeeName + "',Sign2_DATE='" + System.DateTime.Now.ToString("yyyyMMdd") + "'";
                    strSQL += ",LAST_UPD_USER_ID='" + this.Master.Master.EmployeeName + "'";
                    strSQL += ",LAST_UPD_DATE='" + System.DateTime.Now.ToString("yyyyMMdd") + "'";
                    strSQL += ",LAST_UPD_TIME='" + System.DateTime.Now.ToString("HH:mm:ss") + "'";
                    strSQL += " where APLY_NO='" + this.APLY_NO.Text.rpsText() + "'";
                    strSQL += " and isnull(Sign1,'')!=''";
                    al.Add(strSQL);

                    //2.刪除主檔其他申請書票據
                    strSQL = " delete from OR_RECV_PAPER where PAPER_NBR IN (select PAPER_NBR from OR3_RECV_PAPER_TEMP where APLY_NO='" + this.APLY_NO.Text.rpsText() + "'  and SIGN1!='' and SIGN2!='')";
                    al.Add(strSQL);

                    //3.新增至主檔
                    strSQL = " insert into  OR_RECV_PAPER select * from OR3_RECV_PAPER_TEMP where APLY_NO='" + this.APLY_NO.Text.rpsText() + "'";
                    al.Add(strSQL);

                    //4.刪除暫存檔
                    strSQL = " delete from OR3_RECV_PAPER_TEMP where  APLY_NO='" + this.APLY_NO.Text.rpsText() + "' ";
                    al.Add(strSQL);
                    if (cts.Execute(al))
                    {
                        this.setProcessMessage("資金課簽收處理成功!", false);
                        this.Master.returnPage();
                    }
                    else
                        this.setProcessMessage("資金課簽收處理失敗!", true);



                    break;

                case "Ready":
                    if (this.Request.Form["chkPAPER"] == null)
                    {
                        this.setMessageBox("必須選擇項目!");
                        this.setScript("window.parent.openPopUpWindow();");
                        return;
                    }
                    string strSelected = this.Request.Form["chkPAPER"];

                    if (strSelected == "")
                    {
                        this.setMessageBox("必須選擇項目!");
                        this.setScript("window.parent.openPopUpWindow();");
                        return;
                    }
                    string[] arySelected = strSelected.Split(',');

                    DataTable dtGridSource = this.GetGridSource(false);
                    this.updateGrid(dtGridSource, rptEdit);

                    for (int i = 0; i < arySelected.Length; i++)
                    {
                        DataRow[] aryDr = dtPAPER.Select("PAPER_NBR='" + arySelected[i].ToString().Trim() + "' ");

                        if (aryDr.Length > 0)
                        {

                            DataRow dr = aryDr[0];
                            DataRow dtRow = dtGridSource.NewRow();
                            dtRow["REC_DATE"] = dr["REC_DATE"].ToString().Trim();
                            dtRow["PAPER_NBR"] = dr["PAPER_NBR"].ToString().Trim();
                            dtRow["PAPER_AMT"] = dr["PAPER_AMT"];
                            dtRow["DUE_DAT"] = dr["DUE_DAT"].ToString().Trim();
                            dtRow["PRJ_CASH_DATE"] = dr["PRJ_CASH_DATE"].ToString().Trim();
                            dtRow["CHEQ_KIND"] = dr["CHEQ_KIND"].ToString().Trim();
                            dtRow["BANK_NO"] = dr["BANK_NO"].ToString().Trim();
                            dtRow["BANK_NAME"] = dr["BANK_NAME"].ToString().Trim();
                            dtRow["INV_NAME"] = dr["INV_NAME"].ToString().Trim();
                            dtRow["ACCOUNT"] = dr["ACCOUNT"].ToString().Trim();
                            dtRow["ENDORSOR"] = dr["ENDORSOR"].ToString().Trim();
                            dtRow["REMARK"] = dr["REMARK"].ToString().Trim();
                            dtRow["Status"] = "A";

                            dtGridSource.Rows.Add(dtRow);

                        }
                    }

                    this.setScript("window.parent.closePopUpWindow();");

                    this.rptEdit.DataSource = dtGridSource;
                    this.rptEdit.DataBind();

                    return;

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
            {
                if (bolWE020)
                {
                    Session["CUST_NO"] = this.CUST_NO.Value;
                    Session["APLY_NO"] = this.APLY_NO.Text.Trim();
                    //Session["CUST_NAME"] = this.CUST_SNAME.Text;

                    this.setProcessMessage(this.Master.nowStatusName + "資料處理成功!!", false);
                    this.setScript("window.location.replace('WE050.aspx');");

                    return 0;
                }

                return 2;
            }


        }
        #endregion



        
        #region Exit_Click：離開鍵觸發
        /// <summary>
        /// 離開鍵觸發Event,
        /// 本頁作業：無  
        /// </summary>
        private void Exit_Click()
        {
            if (bolWE020)
            {
                Session["CUST_NO"] = this.CUST_NO.Value;
                Session["APLY_NO"] = this.APLY_NO.Text.Trim();

                this.Response.Redirect("WE050.aspx");
            }
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