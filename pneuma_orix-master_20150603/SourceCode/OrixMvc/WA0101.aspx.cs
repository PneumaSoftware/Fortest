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
    public partial class WA0101 : PageParent
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
            this.Master.KeyFields = "TMP_CODE";


           



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
            rptEdit.DataSource = this.GetGridSource();
            rptEdit.DataBind();
           


        }
        #endregion


        protected void reloadWord(object sender, EventArgs e)
        {
            if (this.idWord.Text.IndexOf("CON_ATT_FILE_SEQ") == -1)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setClose", "window.parent.closeLoading();", true);
                return;
            }
            byte[] _ByteArray = CON_ATT_FILE_SEQ.bImage;
            string _FileName = this.Request.PhysicalApplicationPath + "\\" + this.Master.Master.EmployeeId + ".doc";

            System.IO.FileStream _FileStream = new System.IO.FileStream(_FileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            // Writes a block of bytes to this stream using data from
            // a byte array.
            _FileStream.Write(_ByteArray, 0, _ByteArray.Length);

            // close file stream
            _FileStream.Close();

            Microsoft.Office.Interop.Word.Application wrdApp;
            Microsoft.Office.Interop.Word._Document oDataDoc;
            // Microsoft.Office.Interop.Word.MailMerge wrdMailMerge;
            DataTable dt=null;

            try
            {
                Object oMissing = System.Type.Missing;
                Object oFalse = false;
                Object oTrue = true;

                Object oName = _FileName;

                wrdApp = new Microsoft.Office.Interop.Word.ApplicationClass();
                //wrdApp.Visible = true;
                wrdApp.WindowState = Microsoft.Office.Interop.Word.WdWindowState.wdWindowStateMaximize;

                // Open the template 
                oDataDoc = wrdApp.Documents.Add(ref oName, ref oFalse, ref oMissing, ref oMissing);

                Microsoft.Office.Interop.Word.Bookmarks odf = oDataDoc.Bookmarks;


                dt = new DataTable();
                dt.Columns.Add("BOOK_MARK");
                dt.Columns.Add("PARA_CODE");
                dt.Columns.Add("sort", Type.GetType("System.Int16"));
                int ic = 0;

                foreach (Microsoft.Office.Interop.Word.Bookmark bookmark in oDataDoc.Bookmarks)
                {
                    // Do whatever you want with the bookmark.
                    DataRow dr = dt.NewRow();
                    string strBook = bookmark.Name.Trim();
                    ic++;

                    dr["sort"] = ic;
                    dr["BOOK_MARK"] = strBook;
                    dr["PARA_CODE"] = "";
                    for (int i = 0; i < this.rptEdit.Items.Count; i++)
                    {
                        if (this.rptEdit.Items[i].FindControl("BOOK_MARK").value() == strBook)
                            dr["PARA_CODE"] = this.rptEdit.Items[i].FindControl("PARA_CODE").value();
                    }
                    dt.Rows.Add(dr);
                }

                object miss = System.Reflection.Missing.Value;
                object missingValue = Type.Missing;

                wrdApp.Application.Quit(ref miss, ref miss, ref miss);
                object doNotSaveChanges = Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges;

              //  oDataDoc.Close(ref doNotSaveChanges, ref missingValue, ref missingValue);
            }
            catch(Exception err)
            {
                commonWriteFile.writeErrorLog("error word", err.ToString());
             //   this.setMessageBox("載入word元件失敗,請通知系統人員!!");
            }
            finally
            { 
                oDataDoc = null;
                wrdApp = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            if (dt != null)
            {
                DataView dv = dt.DefaultView;
                dv.Sort = "sort";
                this.rptEdit.DataSource = dv;
                this.rptEdit.DataBind();
            }

            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setClose", "window.setTimeout('window.parent.closeLoading()',500);", true);
        }

        



        private DataTable GetGridSource()
        {
            DataTable dtGridSource = null;

            if (ViewState["GridSource"] == null)
            {
                string strSQL = "select BOOK_MARK,PARA_CODE from OR3_CONTRACT_TEMP_PARA  where TMP_CODE='" + this.TMP_CODE.Text + "'";
                dtGridSource = dg.GetDataTable(strSQL);
                DataColumn DC = new DataColumn();
                DC.ColumnName = "STATUS";
                DC.DefaultValue = "";
                DC.DataType = Type.GetType("System.String");
                dtGridSource.Columns.Add(DC);

                ViewState["GridSource"] = dtGridSource;
            }
            else
            {
                ViewState["GridSource"] = dtGridSource;
            }

            return dtGridSource;
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
                    if (this.TMP_CODE.Text.Trim() == "")
                        strMessage += "[範本代碼]";

                    if (this.TMP_DESC.Text.Trim() == "")
                        strMessage += "[範本說明]";

                    if (this.DEPT.Text.Trim() == "")
                        strMessage += "[部門]";

                    
                    
                    if (strMessage != "")
                        strMessage += "必須輸入！";


                    if (this.CON_COND_FILE_SEQ.Seq == "")
                        strMessage += "\\r\\n合約條文檔案必須選擇！";

                    if (this.CON_ATT_FILE_SEQ.Seq == "")
                        strMessage += "\\r\\n附表樣版檔案必須選擇！";

                    if (strMessage != "")
                    {
                        strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                        this.setMessageBox(strMessage);
                        return false;
                    }

                    if (this.CON_COND_FILE_SEQ.ExtName.ToLower().IndexOf("doc") == -1 && this.CON_COND_FILE_SEQ.ExtName.Trim()!="")
                    {
                        this.setMessageBox("合約條文檔案只限word格式");
                        return false;
                    }

                    if (this.CON_ATT_FILE_SEQ.ExtName.ToLower().IndexOf("doc") == -1 && this.CON_ATT_FILE_SEQ.ExtName.Trim() != "")
                    {
                        this.setMessageBox("附表樣版檔案只限word格式");
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


            string FILE_SEQ1 = this.CON_COND_FILE_SEQ.Seq;
            string FILE_SEQ2 = this.CON_ATT_FILE_SEQ.Seq;

            DataTable dtAMT = dts.GetTable("OR3_CONTRACT_TEMP_SET", "TMP_CODE='" + this.TMP_CODE.Text.Trim() + "'");
            DataTable dtPARA = dts.GetTable("OR3_CONTRACT_TEMP_PARA", "TMP_CODE='" + this.TMP_CODE.Text.Trim() + "'");
            DataTable dtF = dts.GetTable("OR3_FILE_STORE", "FILE_SEQ='" + FILE_SEQ1 + "' or FILE_SEQ='" + FILE_SEQ2 + "'");

            byte[] BINARY_FILE1 = this.CON_COND_FILE_SEQ.bImage;
            string strExtName1 = this.CON_COND_FILE_SEQ.ExtName;

            byte[] BINARY_FILE2 = this.CON_ATT_FILE_SEQ.bImage;
            string strExtName2 = this.CON_ATT_FILE_SEQ.ExtName;
            DataRow dr = null;

            switch (strStatus)
            {

                case "Add":
                case "Copy":
                case "Upd":
                    if (dtAMT.Rows.Count == 0)
                    {
                        dr = dtAMT.NewRow();
                        dr["TMP_CODE"] = this.TMP_CODE.Text.Trim();
                        dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                    }
                    else
                    {
                        dr = dtAMT.Rows[0];                        
                    }
                   

                    dr["TMP_DESC"] = this.TMP_DESC.Text;
                    dr["DEPT"] = this.DEPT.SelectedValue;
                    dr["CON_COND_FILE_SEQ"] = FILE_SEQ1;
                    dr["CON_ATT_FILE_SEQ"] = FILE_SEQ2;                    
                    

                    dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                    dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                    dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                    if (dtAMT.Rows.Count == 0)
                        dtAMT.Rows.Add(dr);

                    if (strExtName1 != "")
                    {
                        if (dtF.Select("FILE_SEQ='" + FILE_SEQ1 + "'").Length == 0)
                        {
                            dr = dtF.NewRow();
                            dr["FILE_SEQ"] = FILE_SEQ1;
                            dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                            dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                            dr["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                        }
                        else
                            dr = dtF.Select("FILE_SEQ='" + FILE_SEQ1 + "'")[0];

                        dr["FILE_TYPE"] = strExtName1;
                        dr["BINARY_FILE"] = BINARY_FILE1;                       
                        dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");

                        if (dtF.Select("FILE_SEQ='" + FILE_SEQ1 + "'").Length == 0)
                            dtF.Rows.Add(dr);
                    }

                    if (strExtName2 != "")
                    {
                        if (dtF.Select("FILE_SEQ='" + FILE_SEQ2 + "'").Length == 0)
                        {
                            dr = dtF.NewRow();
                            dr["FILE_SEQ"] = FILE_SEQ2;
                            dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                            dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                            dr["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                        }
                        else
                            dr = dtF.Select("FILE_SEQ='" + FILE_SEQ2 + "'")[0];
                       
                        dr["FILE_TYPE"] = strExtName2;
                        dr["BINARY_FILE"] = BINARY_FILE2;                       
                        dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                        
                        if (dtF.Select("FILE_SEQ='" + FILE_SEQ2 + "'").Length == 0)
                            dtF.Rows.Add(dr);
                    }

                    break;
                case "Del":
                    dtAMT.DeleteRows();
                    dtPARA.DeleteRows();
                    dtF.DeleteRows();

                    break;

            }

            DataTable dt = dtPARA.Copy();
            DataRow[] drCopy;
            dtPARA.DeleteRows();

            for (int i = 0; i < this.rptEdit.Items.Count; i++)
            {

                string BOOK_MARK = this.rptEdit.Items[i].FindControl("BOOK_MARK").value();
                string PARA_CODE = this.rptEdit.Items[i].FindControl("PARA_CODE").value();

               
               


                drCopy = dtPARA.Select("BOOK_MARK='" + BOOK_MARK + "'");

                dr = dtPARA.NewRow();
                dr["TMP_CODE"] = TMP_CODE.Text.Trim();
                dr["BOOK_MARK"] = BOOK_MARK;
                dr["PARA_CODE"] = PARA_CODE;
                if (drCopy.Length > 0)
                {
                    dr["ADD_USER_ID"] = drCopy[0]["ADD_USER_ID"];
                    dr["ADD_DATE"] = drCopy[0]["ADD_DATE"];
                    dr["ADD_TIME"] = drCopy[0]["ADD_TIME"];
                }
                dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HHmmss");
                dtPARA.Rows.Add(dr);




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
            switch (e.CommandName)
            {
                case "ChangePASS":

                   
                    break;
            }
        }
    }
}