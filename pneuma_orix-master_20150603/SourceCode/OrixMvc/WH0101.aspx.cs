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
    public partial class WH0101 : PageParent
    {

        protected bool bolQuery
        {
            set { ViewState["bolQuery"] = value; }
            get { return (ViewState["bolQuery"] == null ? false : (bool )ViewState["bolQuery"]); }
        }

        protected string  EFFDate
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
            this.Master.KeyFields = "SALES";




        }
        #endregion

        #region 代入員工姓名
        protected void CORP_ACCT_changed(object obj, EventArgs e)
        {
            string strName=dg.GetDataRow("select EMP_NAME from V_OR_EMP where CORP_ACCT='" + this.CORP_ACCT.Text.Trim() + "'")[0].ToString();
            string strScript = "";
            if (strName == "")
            {
                strScript = "alert('社內帳號不存在, 請重新輸入!');\n";
                strScript += "document.getElementById('" + this.CORP_ACCT.ClientID + "').value='';\n";
                strScript += "document.getElementById('" + this.CORP_ACCT.ClientID + "').focus();\n";
                this.setScript(strScript);
                
            }
            else
            {
                this.SALES1.Text = strName;
                strScript += "document.getElementById('" + this.DAY_REPORT_GROUP.ClientID + "').select();\n";
                this.setScript(strScript);
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
            
            if (tb.Rows.Count > 0)
            {
                this.EFFDate = tb.Select("STATUS<>'D'", "EFF_DATE desc")[0]["EFF_DATE"].ToString();
            }
            this.rptEdit.DataSource = tb;
            this.rptEdit.DataBind();
            this.bolQuery = this.Master.Master.nowStatus == "Query" || this.Master.Master.nowStatus == "Del";  
        }

        #endregion


       
       

        #region grid function
        protected void GridFunc_Click(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            this.bolQuery=false;
            DataTable dtGridSource;
            string strScript = "";
            switch (((Button)sender).ID)
            {

                case "btnDel":
                    dtGridSource = (DataTable)ViewState["GridSource"];
                    string EFF_DATE = e.CommandName;
                    DataRow drFR = dtGridSource.Select("EFF_DATE='" + EFF_DATE + "'")[0];
                    drFR["STATUS"] = "D";
                    rptEdit.DataSource = dtGridSource;
                    rptEdit.DataBind();
                    this.upDetailEditing.Update();


                    break;

                case "btnAdd":
                    string strMessage = "";
                    if (this.addEFF_DATE.Text.Trim() == "")
                        strMessage += "[生效日 ]";

                    if (this.addDEP_NO.Text.Trim() == "")
                        strMessage += "[業務員部門代號]";

                  //  if (this.addDEP_NO2.Text.Trim() == "")
                  //      strMessage += "[業務員組別]";


                    if (strMessage != "")
                    {
                        strMessage += " 必須輸入！";

                        strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                        this.setMessageBox(strMessage);
                        return;
                    }


                    if (ViewState["GridSource"] != null && this.addEFF_DATE.Text.Trim() != "")
                    {
                        dtGridSource = (DataTable)ViewState["GridSource"];
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



                    addDEP.Clearing();
                    addEFF_DATE.Clearing();
                    addDEP_NO.Clearing();
                    addDEP_NO2.Clearing();
                    this.upDetailEditing.Update();
                    break;

               
            }
        }


        private bool SaveGrid()
        {
           


            DataTable dt = dts.GetTable("OR3_SALES_DEP_REC", "SALES='" + this.SALES.Text.rpsText() + "'");
            DataRow dr;

            this.updateGrid(this.GetGridSource(false), rptEdit);

            /*END 勿動 更新Grid*/

            

            for (int i = 0; i < this.rptEdit.Items.Count; i++)
            {
                string STATUS = this.rptEdit.Items[i].FindControl("STATUS").value();
                string EFF_DATE = this.rptEdit.Items[i].FindControl("EFF_DATE").value().Replace("/","");
                string DEP_NO = this.rptEdit.Items[i].FindControl("DEP_NO").value();
                string DEP_NO2 = this.rptEdit.Items[i].FindControl("DEP_NO2").value();
                string DEP = this.rptEdit.Items[i].FindControl("DEP").value();

               
                string strMessage = "";
                if (EFF_DATE == "")
                    strMessage += "[生效日]";

                if (DEP_NO == "")
                    strMessage += "[業務員部門代號]";

             //   if (DEP_NO2 == "")
              //      strMessage += "[業務員組別]";


                if (strMessage != "")
                {
                    strMessage+= " 必須輸入！";

                    strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                    this.setMessageBox(strMessage);
                    return false;
                }
                

                if (i != 0 && EFF_DATE != "")
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
                        if (this.rptEdit.Items[intRow].FindControl("EFF_DATE").value().Replace("/", "").CompareTo(EFF_DATE) >= 0)
                        {
                            strMessage += "\\r\\n生效日[" + EFF_DATE + "]必須大於前一筆的生效日！";                           
                        }

                    }
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


                        dr = dt.NewRow();
                        dr["SALES"] = this.SALES.Text;
                        dr["EFF_DATE"] = EFF_DATE.Replace("/","");
                        dr["DEP_NO"] = DEP_NO;
                        dr["DEP"] = DEP;
                        dr["DEP_NO2"] = DEP_NO2;                       
                        dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["ADD_TIME"] = System.DateTime.Now.ToString("HHmmss");
                        dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HHmmss");
                        dt.Rows.Add(dr);

                        

                        break;

                    case "U":
                        dr = dt.Select("EFF_DATE='" + EFF_DATE + "'")[0];
                        dr["DEP_NO"] = DEP_NO;
                        dr["DEP"] = DEP;
                        dr["DEP_NO2"] = DEP_NO2;          
                        dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HHmmss");

                        break;

                    case "D":
                        dt.DeleteRows("EFF_DATE='" + EFF_DATE + "'");
                        break;
                }
            }

          //  if (dts.Save())
          //      this.setProcessMessage("版次資料儲存完成！", false);
          //  else
          //      this.setProcessMessage("版次資料儲存失敗！", true);

            return true;

        }


        private string GetGridSql()
        {
            string strSQL = "";
            strSQL += "select EFF_DATE=dbo.f_DateAddSlash(EFF_DATE),DEP_NO,DEP,DEP_NO2 from OR3_SALES_DEP_REC where SALES='" + this.SALES.Text.Trim() + "'";

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
                dtRow["EFF_DATE"] = addEFF_DATE.Text.Trim();
                dtRow["DEP_NO"] = addDEP_NO.Text.Trim();
                dtRow["DEP"] = addDEP.Text.Trim();
                dtRow["DEP_NO2"] = addDEP_NO2.Text.Trim();
                dtRow["Status"] = "A";

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
           
            string strMessage = "";

            
            switch (strStatus)
            {
                case "Add":
                case "Copy":
                case "Upd":
                    if (this.SALES.Text.Trim() == "")
                        strMessage += "[業務員代號]";

                    if (this.SALES1.Text.Trim() == "")
                        strMessage += "[業務員姓名]";

                  
                 

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
                return 0;

            DataTable tb = this.GetGridSource(false);
            if (tb.Select("STATUS<>'D'").Length == 0)
            {
                this.setMessageBox("請新增業務部門資訊!");
                return 0;
            }
            DataTable dt = dts.GetTable("業務員資料表", "SALES='" + this.SALES.Text.rpsText() + "'");
            

            DataRow dr = null;

            switch (strStatus)
            {

                case "Add":
                case "Copy":
                case "Upd":
                    if (dt.Rows.Count == 0)
                    {
                        dr = dt.NewRow();
                        dr["SALES"] = this.SALES.Text.Trim();
                        dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                    }
                    else
                        dr = dt.Rows[0];


                    dr["SALES1"] = this.SALES1.Text.Trim();
                    dr["ENABLE"] = this.ENABLE.Checked;
                    dr["SP_ENABLE"] = this.SP_ENABLE.Checked;
                    dr["DAY_REPORT_GROUP"] = this.DAY_REPORT_GROUP.Text.Trim();
                    dr["CORP_ACCT"] = this.CORP_ACCT.Text.Trim();
                    dr["EMAIL"] = this.EMAIL.Text.Trim();
                   
                   
                    if (tb.Rows.Count>0)
                    {
                        DataRow drRec=tb.Select("STATUS<>'D'","EFF_DATE desc")[0];
                        dr["DEP_NO"] = drRec["DEP_NO"].ToString();
                        dr["DEP"] = drRec["DEP"].ToString();
                        dr["DEP_NO2"] = drRec["DEP_NO2"].ToString();

                    }
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