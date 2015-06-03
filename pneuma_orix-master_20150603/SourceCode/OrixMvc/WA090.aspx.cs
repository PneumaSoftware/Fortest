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
    public partial class WA090 : OrixMvc.Pattern.PageParent
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


        public string AplyNo
        {
            set { ViewState["AplyNo"] = value; }
            get { return (ViewState["AplyNo"] == null ? "" : (string)ViewState["AplyNo"]); }
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

            this.Master.ProcessEvent += new ProcessDelegate(Save_Click);


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

           // this.Master.editSave = false;

        }
        #endregion



        #region setDefaultValue：欄位預設值
        /// <summary>
        /// 設定欄位預設值
        /// 本頁作業：無作用
        /// </summary>
        private void setDefaultValue()
        {
            rptEdit.DataSource = this.GetGridSource(false);
            rptEdit.DataBind();

            
        }
        #endregion


        protected void GridFunc_Click(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            DataTable dtGridSource;
            switch (((Button)sender).ID)
            {
                case "btnDel":
                    

                    dtGridSource = (DataTable)Session["GridSource"];
                    string SEQ_NO = e.CommandName;
                    DataRow dr = dtGridSource.Select("SEQ_NO='" + SEQ_NO + "'")[0];
                    dr["STATUS"] = "D";
                    rptEdit.DataSource = dtGridSource;
                    rptEdit.DataBind();
                 //   this.upGrid.Update();


                    break;

                case "btnAdd":
                   
                    string strMessage = "";
                    if (this.addATCH_CODE.Text.Trim()=="")
                        strMessage += "\\r\\n附件代碼必須輸入！";

                    string strExtName = this.addFILE_SEQ.ExtName;
                    if (strExtName == "")
                        strMessage += "\\r\\n影像必須選擇！";
                    if (strMessage != "")
                    {
                        strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                        this.setMessageBox(strMessage);
                        return ;
                    }

                    dtGridSource = GetGridSource(true);
                    if (Session["GridSource"] != null)
                    {
                        dtGridSource = (DataTable)Session["GridSource"];                       
                        rptEdit.DataSource = dtGridSource;
                        rptEdit.DataBind();
                        rptEdit.Visible = true;

                        //記錄影像
                        /*ocxControl.ocxUpload dlg = (ocxControl.ocxUpload)rptEdit.Items[rptEdit.Items.Count - 1].FindControl("FILE_SEQ");
                        dlg.Seq = addFILE_SEQ.Seq;
                        dlg.bImage = addFILE_SEQ.bImage;
                        dlg.MIME = addFILE_SEQ.MIME;
                        dlg.ExtName = addFILE_SEQ.ExtName;*/

                    }
                    else
                    {
                        rptEdit.DataSource = null;
                        rptEdit.DataBind();
                        rptEdit.Visible = false;
                    }
                    addATCH_CODE.Clearing();
                    addATCH_NAME.Clearing();
                    addFILE_SEQ.Seq = "";
                    addFILE_SEQ.bImage = null;
                    addFILE_SEQ.MIME = "";
                    addFILE_SEQ.ExtName = "";
                    addREMARK.Clearing();
                    break;
            }            
        }

        
        private string GetGridSql()
        {
            string strSQL = "";

            strSQL = "select APLY_NO,SEQ_NO,a.ATCH_CODE,ATCH_NAME,IMG_POST,REMARK,FILE_SEQ from OR_CASE_APLY_IMAGE a left join OR_ATCH_CODE b on a.ATCH_CODE=b.ATCH_CODE  where APLY_NO='" + this.APLY_NO.Text + "'";

            return strSQL;
        }

        private DataTable GetGridSource(bool bolAdd)
        {
            DataTable dtGridSource = null;

            if (Session["GridSource"] == null)
            {

                dtGridSource = dg.GetDataTable(this.GetGridSql());
                 DataColumn DC = new DataColumn();
                DC.ColumnName = "STATUS";
                DC.DefaultValue = "";
                DC.DataType = Type.GetType("System.String");
                dtGridSource.Columns.Add(DC);
                dtGridSource.Columns.Add("MIME");
                dtGridSource.Columns.Add("ExtName");
                dtGridSource.Columns.Add("bImage",System.Type.GetType("System.Byte[]"));
                for (int i = 0; i < dtGridSource.Rows.Count; i++)
                {
                    dtGridSource.Rows[i]["bImage"] = new System.Byte[0];
                }

                Session["GridSource"] = dtGridSource;
            }
            else
                dtGridSource = (DataTable)Session["GridSource"];

            if (bolAdd){
                 dtGridSource = (DataTable)Session["GridSource"];
                this.updateGrid(dtGridSource, rptEdit);
                DataRow dtRow = dtGridSource.NewRow();
                int seq = dtGridSource.Compute("max(SEQ_NO)", "").ToString().toInt();
                if (seq == 0)
                    seq = 1;
                else
                    seq++;
                dtRow["SEQ_NO"] = seq;
                dtRow["ATCH_CODE"] = addATCH_CODE.Text.Trim();
                dtRow["ATCH_NAME"] = addATCH_NAME.Text.Trim();
                dtRow["REMARK"] = addREMARK.Text.Trim();
                dtRow["MIME"] = addFILE_SEQ.MIME;
                dtRow["ExtName"] = addFILE_SEQ.ExtName;
                dtRow["bImage"] = addFILE_SEQ.bImage;
                dtRow["Status"] = "A";
                dtRow["FILE_SEQ"] = this.addFILE_SEQ.Seq;
                

                dtGridSource.Rows.Add(dtRow);
                Session["GridSource"] = dtGridSource;
            }

            return dtGridSource;
        }





        #region Save_Click：儲存鍵觸發
        /// <summary>
        /// 儲存鍵觸發Event,
        /// 本頁作業：無  
        /// </summary>
        private void Save_Click(string strType)
        {

            if (strType == "Clear")
            {
                this.AplyNo = "";
                this.APLY_NO.Text = "";
                this.APLY_NO.Editing(true);
                this.btnQry.Editing(true);

                Session["GridSource"] = null;
                this.rptEdit.DataSource = null;
                this.rptEdit.DataBind();
                return;
            }

            /*START 勿動 更新Grid*/
            this.updateGrid(this.GetGridSource(false), rptEdit);

            /*END 勿動 更新Grid*/
            if (this.rptEdit.Items.Count == 0)
            {
                this.setMessageBox("無資料可儲存！");
                return;
            }

            DataTable dt = dts.GetTable("OR_CASE_APLY_IMAGE", "APLY_NO='"+ this.APLY_NO.Text.Trim()+"'");
            DataTable dtF = dts.GetTable("OR3_FILE_STORE", "FILE_SEQ IN(select FILE_SEQ from OR_CASE_APLY_IMAGE where APLY_NO='" + this.AplyNo.Trim() + "')"); 
            DataRow[] adr;
            DataRow dr;
            DataRow drF;
            

            for (int i = 0; i < this.rptEdit.Items.Count; i++)
            {

                string strMessage = "";
               

                string STATUS = this.rptEdit.Items[i].FindControl("STATUS").value();
                string SEQ_NO = this.rptEdit.Items[i].FindControl("SEQ_NO").value();
                string ATCH_CODE = this.rptEdit.Items[i].FindControl("ATCH_CODE").value();
                string REMARK = this.rptEdit.Items[i].FindControl("REMARK").value();
                string FILE_SEQ = this.rptEdit.Items[i].FindControl("FILE_SEQ").value();
                byte[] BINARY_FILE = ((OrixMvc.ocxControl.ocxUpload)this.rptEdit.Items[i].FindControl("FILE_SEQ")).bImage;
                string strExtName = ((OrixMvc.ocxControl.ocxUpload)this.rptEdit.Items[i].FindControl("FILE_SEQ")).ExtName;
                if (ATCH_CODE == "")
                    strMessage += "附件代碼必須輸入！";

                if (strExtName == "" && STATUS=="A")
                    strMessage += " 影像必須選擇！";
                if (strMessage != "")
                {
                    strMessage = "第" + (i+1).ToString() + "筆：" + strMessage;
                    this.setMessageBox(strMessage);
                    return ;
                }

                switch (STATUS)
                {
                    case "A":
                       // FILE_SEQ = System.DateTime.Now.ToString("yyyyMMddHHmmssfff");

                        dr = dt.NewRow();

                        dr["APLY_NO"] = this.AplyNo.Trim();
                        dr["ATCH_CODE"] = ATCH_CODE;
                        dr["FILE_SEQ"] = FILE_SEQ;
                        dr["IMG_POST"] = "";
                        dr["SEQ_NO"] = SEQ_NO;
                        dr["REMARK"] = REMARK;
                        dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                        dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                        dt.Rows.Add(dr);

                        if (BINARY_FILE != null)
                        {
                            //if (dtF.Select("FILE_SEQ=" + FILE_SEQ).Length != 0)
                           // {
                                dtF.DeleteRows("FILE_SEQ=" + FILE_SEQ);
                          //  }
                          //  dtF.AcceptChanges();
                            //if (dtF.Select("FILE_SEQ=" + FILE_SEQ).Length == 0)
                            //{
                            drF = dtF.NewRow();
                            drF["FILE_SEQ"] = FILE_SEQ;
                            //}
                            //else
                            //  drF = dtF.Select("FILE_SEQ=" + FILE_SEQ)[0];
                            drF["FILE_SEQ"] = FILE_SEQ;
                            drF["FILE_TYPE"] = strExtName;
                            drF["BINARY_FILE"] = BINARY_FILE;
                            drF["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                            drF["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                            drF["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                            drF["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                            drF["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                            drF["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");

                            //if (dtF.Select("FILE_SEQ=" + FILE_SEQ).Length == 0)
                            dtF.Rows.Add(drF);
                        }

                        break;

                    case "U":
                        //FILE_SEQ = System.DateTime.Now.ToString("yyMMddHHmmssfff");

                        dr = dt.Select("SEQ_NO='" + SEQ_NO + "'")[0];
                        dr["ATCH_CODE"] = ATCH_CODE;
                        dr["REMARK"] = REMARK;
                        dr["FILE_SEQ"] = FILE_SEQ;
                        if (BINARY_FILE != null)
                        {
                            if (dtF.Select("FILE_SEQ=" + FILE_SEQ).Length != 0)
                            {
                                dtF.DeleteRows("FILE_SEQ=" + FILE_SEQ);
                            }

                            //if (dtF.Select("FILE_SEQ=" + FILE_SEQ).Length == 0)
                            //{
                                drF = dtF.NewRow();
                                drF["FILE_SEQ"] = FILE_SEQ;
                            //}
                            //else
                              //  drF = dtF.Select("FILE_SEQ=" + FILE_SEQ)[0];
                            drF["FILE_SEQ"] = FILE_SEQ;
                            drF["FILE_TYPE"] = strExtName;
                            drF["BINARY_FILE"] = BINARY_FILE;
                            drF["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                            drF["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                            drF["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                            drF["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                            drF["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                            drF["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");

                            //if (dtF.Select("FILE_SEQ=" + FILE_SEQ).Length == 0)
                                dtF.Rows.Add(drF);
                        }
                        dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");

                        break;

                    case "D":
                        dt.DeleteRows("SEQ_NO='"+ SEQ_NO +"'");
                        dtF.DeleteRows("FILE_SEQ='" + FILE_SEQ + "'");
                        

                        break;
                }               
               
            }

            if (dts.Save())
            {
                this.setProcessMessage("資料處理成功!!", false);
                Session["GridSource"] = null;
                setDefaultValue();
                //this.rptEdit.DataBind();
                //此程式特殊, 需重新載入parent
               // this.setScript("window.parent.self.location.reload();");
            }
            else
                this.setProcessMessage("資料處理失敗!!", true); 

        }
        #endregion


        /// <summary>
        /// 資料查詢
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Status_Click(object sender, CommandEventArgs e)
        {

            string strMessage = "";
            
            switch (e.CommandName)
            {
                case "Query":
                    if (APLY_NO.Text.Trim()  == "")
                        strMessage += "申請書編號必須輸入！";

                                        
                    if (strMessage != "")
                    {   
                        this.AplyNo = "";
                        this.rptEdit.DataSource = null;
                        this.rptEdit.DataBind();

                        this.setMessageBox(strMessage);
                        return;
                    }

                    if (dg.GetDataRow("SELECT * FROM OR_CASE_APLY_BASE WHERE APLY_NO='" + this.APLY_NO.Text.Trim() + "'")[0].ToString().Trim() == "")
                    {
                        this.AplyNo = "";
                        this.rptEdit.DataSource = null;
                        this.rptEdit.DataBind();
                        this.APLY_NO.Focus();

                        this.setMessageBox("申請書編號輸入錯誤, 請重新輸入!!");
                        return;
                    }

                    Session["GridSource"] = null;
                    this.AplyNo = this.APLY_NO.Text.Trim();
                    this.rptEdit.DataSource = this.GetGridSource(false);
                    this.rptEdit.DataBind();

                    if (this.rptEdit.Items.Count == 0)
                    {
                        DataRow dr = dg.GetDataRow("select ATCH_CODE,ATCH_NAME from OR_ATCH_CODE where ATCH_CODE='20000'");
                        this.addATCH_CODE.Text = dr[0].ToString();
                        this.addATCH_NAME.Text = dr[1].ToString();
                    }
                    else
                    {
                        this.addATCH_CODE.Clearing();
                        this.addATCH_NAME.Clearing();
                    }
                    addFILE_SEQ.Seq = "";
                    addFILE_SEQ.bImage = null;
                    addFILE_SEQ.MIME = "";
                    addFILE_SEQ.ExtName = "";
                    addREMARK.Clearing();
                  //  this.Master.editSave = true;

                  //  this.APLY_NO.Editing(false);
                //    this.btnQry.Editing(false);

                    break;

                case "Query_Apply":
                    
                    break;

            }

            //if (e.CommandName!="Query")



            //  if (SelectEvent != null)
            //      SelectEvent();


        }

    }
}
