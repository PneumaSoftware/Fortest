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
    public partial class WA030 : OrixMvc.Pattern.PageParent
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

            this.Master.PrintEvent += new PrintDelegate(Print_Click);


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

        }
        #endregion



        string TMP_CODE = "";

        #region PrintCheck：列印前驗證
        /// <summary>
        /// 查詢前的驗證
        /// </summary>
        /// <returns>驗證成功或失敗：true/false</returns>
        private bool PrintCheck()
        {
            string strMessage = "";
            if (this.APLY_NO.Text.Trim() == "")
                strMessage += "[申請書編號]";

            
            if (strMessage != "")
                strMessage += "必須輸入！";

            if (strMessage != "")
            {
                strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                this.setMessageBox(strMessage);
                return false;
            }

            TMP_CODE = dg.GetDataRow("SELECT TMP_CODE  FROM OR3_CASE_APLY_APLY_COND2 where APLY_NO='" + this.APLY_NO.Text.Trim() + "'")[0].ToString().Trim();

            if (TMP_CODE=="")
            {
                strMessage = ("該申請書未選取範本合約!!");
                this.setMessageBox(strMessage);
                return false;
            }

            return true;
        }
        #endregion


        #region Print_Click：列印鍵觸發
        /// <summary>
        /// 儲存鍵觸發Event,
        /// 本頁作業：無  
        /// </summary>
        private void Print_Click()
        {

            if (!PrintCheck())
                return;

            //1.開啟word
            //2.取代bookmark
            //3.預覽


            string strSQL = "select b.BINARY_FILE as file2,c.BINARY_FILE as file1 from OR3_CONTRACT_TEMP_set a ";
            strSQL += " left join OR3_FILE_STORE b on a.CON_COND_FILE_SEQ=b.FILE_SEQ";
            strSQL += " Left join OR3_FILE_STORE c on a.CON_ATT_FILE_SEQ=c.FILE_SEQ where a.TMP_CODE='" + TMP_CODE + "'";
            DataTable dt = dg.GetDataTable(strSQL);

            byte[] _ByteArray1 = (byte[])dt.Rows[0]["file1"];
            string _FileName1 = this.Request.PhysicalApplicationPath + "\\Contract\\" + this.Master.Master.EmployeeId + "1.doc";

            byte[] _ByteArray2 = (byte[])dt.Rows[0]["file2"];
            string _FileName2 = this.Request.PhysicalApplicationPath + "\\Contract\\" + this.Master.Master.EmployeeId + "2.doc";

            System.IO.FileStream _FileStream1 = new System.IO.FileStream(_FileName1, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            // Writes a block of bytes to this stream using data from
            // a byte array.
            _FileStream1.Write(_ByteArray1, 0, _ByteArray1.Length);

            // close file stream
            _FileStream1.Close();

           // System.IO.File.fl
            System.IO.FileStream _FileStream2 = new System.IO.FileStream(_FileName2, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            // Writes a block of bytes to this stream using data from
            // a byte array.
            _FileStream2.Write(_ByteArray2, 0, _ByteArray2.Length);

            // close file stream
            _FileStream2.Close();

            strSQL = "select PARA_CODE,PARA_FORMULA from OR3_CONTRACT_TEMP_PARA_LST";
            DataTable dtP = dg.GetDataTable(strSQL);

            strSQL = "select c.BOOK_MARK,c.PARA_CODE,e.PARA_FORMULA from OR3_CASE_APLY_APLY_COND2 a left join OR3_CONTRACT_TEMP_SET b on a.TMP_CODE=b.TMP_CODE ";
            strSQL += " left join OR3_CONTRACT_TEMP_PARA c on b.TMP_CODE=c.TMP_CODE ";
            strSQL += " left join OR3_CONTRACT_TEMP_PARA_LST e on c.PARA_CODE=e.PARA_CODE";
            strSQL += " where a.APLY_NO='" + this.APLY_NO.Text.Trim() + "' ";
            DataTable dtL = dg.GetDataTable(strSQL);


            Microsoft.Office.Interop.Word.Application wrdApp;
            Microsoft.Office.Interop.Word._Document oDataDoc;
            Microsoft.Office.Interop.Word._Document oDataDocF;
            object miss = System.Reflection.Missing.Value;
            object missingValue = Type.Missing;

            Object oMissing = System.Type.Missing;
            Object oFalse = false;
            Object oTrue = true;

            Object oName =  _FileName2;// this.Request.PhysicalApplicationPath + "\\Contract\\FRC.doc";
            ;

            wrdApp = new Microsoft.Office.Interop.Word.ApplicationClass();
            wrdApp.Visible = true;
            wrdApp.WindowState = Microsoft.Office.Interop.Word.WdWindowState.wdWindowStateMaximize;

            // Open the template 
            oDataDoc = wrdApp.Documents.Add(ref oName, ref oFalse, ref oMissing, ref oMissing);

            foreach (Microsoft.Office.Interop.Word.Bookmark bookmark in oDataDoc.Bookmarks)
            {

                // Do whatever you want with the bookmark.
                DataRow dr = dt.NewRow();
                string strBook = bookmark.Name.Trim();
                if (dtP.Select("PARA_CODE='" + strBook + "'").Length == 0)
                    continue;

                string strSQLF = dtP.Select("PARA_CODE='" + strBook + "'")[0]["PARA_FORMULA"].ToString().Replace("?", this.APLY_NO.Text.Trim()); ;
                string strData = "";

                if (strSQLF != "")
                {
                    DataTable dtR = dg.GetDataTable(strSQLF);
                    for (int i = 0; i < dtR.Rows.Count; i++)
                    {
                        strData += (strData == "" ? "" : "\r") + dtR.Rows[i][0].ToString().Trim();

                    }
                }

                bookmark.Range.Text = strData;
            }

            oDataDoc.Saved = true;
             object oOutputDoc = this.Request.PhysicalApplicationPath + @"\Contract\" + this.Master.Master.EmployeeId + "c.doc";

             wrdApp.ActiveDocument.SaveAs(ref oOutputDoc,
                                              ref oMissing, ref oMissing, ref oMissing,
                                              ref oMissing, ref oMissing, ref oMissing,
                                              ref oMissing, ref oMissing, ref oMissing,
                                              ref oMissing, ref oMissing, ref oMissing,
                                              ref oMissing, ref oMissing, ref oMissing);

            oDataDoc.Close(ref oFalse, ref oMissing, ref oMissing);
            wrdApp.Quit(ref oMissing, ref oMissing, ref oMissing);


            oDataDoc = null;
            wrdApp = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();

            oName = _FileName1;

            wrdApp = new Microsoft.Office.Interop.Word.ApplicationClass();
            wrdApp.Visible = true;
            wrdApp.WindowState = Microsoft.Office.Interop.Word.WdWindowState.wdWindowStateMaximize;

            // Open the template 
            oDataDoc = wrdApp.Documents.Add(ref oName, ref oFalse, ref oMissing, ref oMissing);

            foreach (Microsoft.Office.Interop.Word.Bookmark bookmark in oDataDoc.Bookmarks)
            {

                // Do whatever you want with the bookmark.
                DataRow dr = dt.NewRow();
                string strBook = bookmark.Name.Trim();
                if (dtL.Select("BOOK_MARK='" + strBook + "'").Length == 0)
                    continue;

                string strSQLF = dtL.Select("BOOK_MARK='" + strBook + "'")[0]["PARA_FORMULA"].ToString().Replace("?", this.APLY_NO.Text.Trim()); ;
                string strData = "";

                if (strSQLF != "")
                {
                    DataTable dtR = dg.GetDataTable(strSQLF);
                    for (int i = 0; i < dtR.Rows.Count; i++)
                    {
                        strData += (strData == "" ? "" : "\r") + dtR.Rows[i][0].ToString().Trim();

                    }
                }

                bookmark.Range.Text = strData;
            }
            oOutputDoc = this.Request.PhysicalApplicationPath + @"\Contract\" + this.Master.Master.EmployeeId + ".doc";

            object oPageBreak = Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak;
            wrdApp.Selection.InsertFile(this.Request.PhysicalApplicationPath + @"\Contract\" + this.Master.Master.EmployeeId + "c.doc", ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            wrdApp.Selection.InsertBreak(ref oPageBreak);
            oDataDoc.Saved = true;


            wrdApp.ActiveDocument.SaveAs(ref oOutputDoc,
                                            ref oMissing, ref oMissing, ref oMissing,
                                            ref oMissing, ref oMissing, ref oMissing,
                                            ref oMissing, ref oMissing, ref oMissing,
                                            ref oMissing, ref oMissing, ref oMissing,
                                            ref oMissing, ref oMissing, ref oMissing);

            oDataDoc.Close(ref oFalse, ref oMissing, ref oMissing);
            wrdApp.Quit(ref oMissing, ref oMissing, ref oMissing);


            oDataDoc = null;
            wrdApp = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();



            //this.Response.Redirect("/Contract/" + this.Master.Master.EmployeeId + ".doc");
            string js = "window.open('/Contract/" + this.Master.Master.EmployeeId + ".doc','_self');\n";
            

            if (chkFRC.Checked)
            {


                oName = this.Request.PhysicalApplicationPath + "\\Contract\\FRC.doc";

                wrdApp = new Microsoft.Office.Interop.Word.ApplicationClass();
                wrdApp.Visible = true;
                wrdApp.WindowState = Microsoft.Office.Interop.Word.WdWindowState.wdWindowStateMaximize;

                // Open the template 
                oDataDoc = wrdApp.Documents.Add(ref oName, ref oFalse, ref oMissing, ref oMissing);

                foreach (Microsoft.Office.Interop.Word.Bookmark bookmark in oDataDoc.Bookmarks)
                {

                    // Do whatever you want with the bookmark.
                    DataRow dr = dt.NewRow();
                    string strBook = bookmark.Name.Trim();
                    if (dtP.Select("PARA_CODE='" + strBook + "'").Length == 0)
                        continue;
                    string strSQLF = dtP.Select("PARA_CODE='" + strBook + "'")[0]["PARA_FORMULA"].ToString().Replace("?", this.APLY_NO.Text.Trim()); ;
                    string strData = "";

                    if (strSQLF != "")
                    {
                        DataTable dtR = dg.GetDataTable(strSQLF);
                        for (int i = 0; i < dtR.Rows.Count; i++)
                        {
                            strData += (strData == "" ? "" : "\r") + dtR.Rows[i][0].ToString().Trim();

                        }
                    }

                    bookmark.Range.Text = strData;
                }

                oOutputDoc = this.Request.PhysicalApplicationPath + @"\Contract\" + this.Master.Master.EmployeeId + "_FRC.doc";

               // oPageBreak = Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak;
              //  wrdApp.Selection.InsertFile(this.Request.PhysicalApplicationPath + @"\Contract\" + this.Master.Master.EmployeeId + ".doc", ref oMissing, ref oMissing, ref oMissing, ref oMissing);
              //  wrdApp.Selection.InsertBreak(ref oPageBreak);
                oDataDoc.Saved = true;

                try
                {
                    wrdApp.ActiveDocument.SaveAs(ref oOutputDoc,
                                                    ref oMissing, ref oMissing, ref oMissing,
                                                    ref oMissing, ref oMissing, ref oMissing,
                                                    ref oMissing, ref oMissing, ref oMissing,
                                                    ref oMissing, ref oMissing, ref oMissing,
                                                    ref oMissing, ref oMissing, ref oMissing);
                }
                catch { }

                oDataDoc.Close(ref oFalse, ref oMissing, ref oMissing);
                wrdApp.Quit(ref oMissing, ref oMissing, ref oMissing);

                oDataDoc = null;
                wrdApp = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                
                js = "window.open('/Contract/" + this.Master.Master.EmployeeId + "_FRC.doc','FRC');\n";
                js += "window.open('/Contract/" + this.Master.Master.EmployeeId + ".doc','_self');\n";
            }


            //指向報表頁面
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "openReport", js, true);


        }
        #endregion




    }
}
