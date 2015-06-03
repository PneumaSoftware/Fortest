using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
//using OrixMvc;
using Zip = ICSharpCode.SharpZipLib.Zip.Compression;
using VS2008.Module;
using System.Linq;
using AjaxControlToolkit;
using System.Diagnostics;

namespace OrixMvc.Pattern 
{
    /// <summary>
    /// class library
    /// Author      ：  Alinta 
    /// Build Date  ：  2013/04/28     
    /// Purpose     ：  網頁繼承來源
    /// </summary>        
    public  class  PageParent :System.Web.UI.Page 
    {


        #region 追蹤所有錯誤
        protected override void OnError(EventArgs e)
        {
            Exception err = Server.GetLastError().GetBaseException();
            // err.Source;
            //  err.Data;
            string strFolder = this.Request.PhysicalApplicationPath.ToString() + "ErrorData";

            if (!System.IO.Directory.Exists(strFolder))
                System.IO.Directory.CreateDirectory(strFolder);

            ArrayList al = new ArrayList();
            string strMemNo = "";

            if (Session["EmployeeId"] != null)
            {
                strMemNo = ((String)Session["EmployeeId"]).ToString().Trim();
                al.Add("會員編號：" + strMemNo + "\r\n");
            }
            al.Add("URL路徑：" + this.Request.Url.ToString() + "\r\n");
            al.Add("錯誤server：" + this.Server.MachineName.ToString() + "\r\n");

            al.Add("錯誤來源：" + err.Source.ToString() + "\r\n");
            al.Add("錯誤方法：" + err.TargetSite.ToString() + "\r\n");
            al.Add("錯誤執行：" + err.StackTrace.ToString() + "\r\n");
            al.Add("錯誤訊息：" + err.Message.ToString() + "\r\n");
            al.Add("錯誤資訊：" + err.ToString() + "\r\n");
            //Server.GetLastError().StackTrace 
            StackTrace st = new StackTrace(err);
            
            StackFrame sf = st.GetFrame(0);
            al.Add("錯誤行數：" + sf.GetFileLineNumber() + "\r\n");
            al.Add("錯誤欄：" + sf.GetFileColumnNumber() + "\r\n");
            al.Add("錯誤方法：" + sf.GetMethod().Name + "\r\n");
            al.Add("錯誤檔案名：" + sf.GetFileName() + "\r\n");

            VS2008.Module.commonWriteFile.writeFile(strFolder + "\\", DateTime.Now.ToString("yyyyMMdd HHmmss ") + strMemNo + ".log", al);


           // this.Response.Redirect("Errors");

            Server.ClearError();

            base.OnError(e);
        }
        #endregion

        
        /// <summary>
        /// 取得Master Page裏的區塊
        /// </summary>
        public enum Area
        {
            /// <summary>
            /// 搜尋區
            /// </summary>
            SearchArea,
            /// <summary>
            /// 編輯區
            /// </summary>
            DataArea,            
            /// <summary>
            /// 查詢區
            /// </summary>
            QueryArea,
            /// <summary>
            /// 查詢區
            /// </summary>
            FunctionBar,
        }

        /// <summary>
        /// 取得Master Page裏的系統按鍵
        /// </summary>
        public enum SystemButton
        {
            /// <summary>
            /// 查詢
            /// </summary>
            btnQuery,

            /// <summary>
            /// 存檔
            /// </summary>
            btnSave,
            /// <summary>
            /// 取消
            /// </summary>
            btnCancel,
            
            /// <summary>
            /// 新增
            /// </summary>
            btnAdd,

            /// <summary>
            /// 離開
            /// </summary>
            btnExit
        }


        /// <summary>
        /// 取得登入者的組織相關資料
        /// </summary>
        public enum EmpData
        {           
            /// <summary>
            /// 員工代號
            /// </summary>
            EmpCode,
            /// <summary>
            /// 員工中文名
            /// </summary>
            EmpChineseName,
            /// <summary>
            /// 是否為總公司模式
            /// </summary>
            Emp_Type,
            /// <summary>
            /// 職稱類別
            /// </summary>
            EmpTitleType,
           
            /// <summary>
            /// 登入者的IP Address
            /// </summary>
            IPAddress,
            /// <summary>
            /// 登入者選取的程式代號
            /// </summary>
            ProgramID,
            /// <summary>
            /// 登入者選取的程式名稱
            /// </summary>
            ProgramName
        }

        /// <summary>
        /// save viewstate and zip
        /// </summary>
        /// <param name="state"></param>
        protected override void SavePageStateToPersistenceMedium(object state)
        {
            Pair pair;
            PageStatePersister persister = this.PageStatePersister;
            object viewState;
            if (state is Pair)
            {
                pair = (Pair)state;
                persister.ControlState = pair.First;
                viewState = pair.Second;
            }
            else
            {
                viewState = state;
            }

            LosFormatter formatter = new LosFormatter();
            StringWriter writer = new StringWriter();
            formatter.Serialize(writer, viewState);
            string viewStateStr = writer.ToString();
            byte[] data = Convert.FromBase64String(viewStateStr);
            byte[] compressedData = Compress(data);
            string str = Convert.ToBase64String(compressedData);

            persister.ViewState = str;
            persister.Save();
        }


        protected void updateGrid(DataTable dt, Repeater rpt)
        {


            for (int i = 0; i < rpt.Items.Count; i++)
            {
                string strStatus = ((HiddenField)rpt.Items[i].FindControl("Status")).Value.Trim();
                if (strStatus == "D")
                    break;

                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (rpt.Items[i].FindControl(dt.Columns[j].ColumnName) != null)
                    {
                        Control ctl = rpt.Items[i].FindControl(dt.Columns[j].ColumnName);
                        string strValue = "";
                        if (ctl.GetType().Name.ToLower() == "ocxcontrol_ocxupload_ascx")
                            strValue = ((ocxControl.ocxUpload)rpt.Items[i].FindControl(dt.Columns[j].ColumnName)).newSeq;
                        else
                            strValue = rpt.Items[i].FindControl(dt.Columns[j].ColumnName).value();

                        if (dt.Rows[i][j].GetType() == Type.GetType("System.Boolean"))
                        {
                            bool bol = (strValue == "Y" ? true : false);
                            if ((bol && !(bool)dt.Rows[i][j]) || (!bol && (bool)dt.Rows[i][j]))
                            {
                                dt.Rows[i][j] = bol;
                                if (strStatus != "A" && strStatus != "D")
                                    dt.Rows[i]["Status"] = "U";

                            }

                        }
                        else
                            if (dt.Rows[i][j].ToString() != strValue)
                            {
                                dt.Rows[i][j] = strValue;
                                if (strStatus != "A" && strStatus != "D")
                                    dt.Rows[i]["Status"] = "U";
                            }


                    }
                }

            }


            rpt.DataSource = dt;
            rpt.DataBind();
        }


        /// <summary>
        /// load viewstae and unzip
        /// </summary>
        /// <returns></returns>
        protected override object LoadPageStateFromPersistenceMedium()
        {
            PageStatePersister persister = this.PageStatePersister;
            persister.Load();

            string viewState = persister.ViewState.ToString();
            byte[] data = Convert.FromBase64String(viewState);
            byte[] uncompressedData = DeCompress(data);
            string str = Convert.ToBase64String(uncompressedData);
            LosFormatter formatter = new LosFormatter();
            return new Pair(persister.ControlState, formatter.Deserialize(str));
        }


        /// <summary>
        /// zip
        /// </summary>
        /// <param name="pBytes">Data byte type</param>
        /// <returns></returns>
        public static byte[] Compress(byte[] pBytes)
        {
            MemoryStream mMemory = new MemoryStream();

            Zip.Deflater mDeflater = new Zip.Deflater(Zip.Deflater.BEST_COMPRESSION);
            Zip.Streams.DeflaterOutputStream mStream = new Zip.Streams.DeflaterOutputStream(mMemory, mDeflater, 131072);

            mStream.Write(pBytes, 0, pBytes.Length);
            mStream.Close();

            return mMemory.ToArray();
        }

        /// <summary>
        /// unzip
        /// </summary>
        /// <param name="pBytes">Data byte type</param>
        /// <returns></returns>
        public static byte[] DeCompress(byte[] pBytes)
        {
            Zip.Streams.InflaterInputStream mStream = new Zip.Streams.InflaterInputStream(new MemoryStream(pBytes));

            MemoryStream mMemory = new MemoryStream();
            Int32 mSize=0;

            byte[] mWriteData = new byte[4096];

            while (true)
            {
                mSize = mStream.Read(mWriteData, 0, mWriteData.Length);
                if (mSize > 0)
                {
                    mMemory.Write(mWriteData, 0, mSize);
                }
                else
                {
                    break;
                }
            }

            mStream.Close();
            return mMemory.ToArray();
        } 
        
        /// <summary>
        /// 取得C03
        /// </summary>
        /// <param name="ed"></param>
        /// <returns></returns>
        public string UserParameter(EmpData ed)
        {
            string strName = ((System.Reflection.MemberInfo)(this.Master.GetType().BaseType)).Name;

            if (ed == EmpData.IPAddress)
                return this.Request.UserHostAddress;

            string strValue = "";
            switch (ed)
            {
                case EmpData.ProgramID:
                    strValue=((OrixMvc.Pattern.content)this.Master.Master).ProgramId;                     
                    break;

                case EmpData.ProgramName:
                    strValue = ((OrixMvc.Pattern.content)this.Master.Master).ProgramName;
                    break;

                case  EmpData.EmpCode:
                    strValue = ((OrixMvc.Pattern.content)this.Master.Master).EmployeeId;
                    break;

                case EmpData.EmpChineseName:
                    strValue = ((OrixMvc.Pattern.content)this.Master.Master).EmployeeName;
                    break;


            }

            return strValue;

        }

        protected string convertToEmployeeCode(string CorpAcct)
        {
            string strID = "";
            DataGetting dg = new DataGetting("myConnectionString");
            return dg.GetDataRow("select EMP_CODE from V_OR_EMP where CORP_ACCT='" + CorpAcct + "'")[0].ToString();

        }
       
        /// <summary>
        /// when page is loaded
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoadComplete(EventArgs e)
        {
           
           bool bolAjax=false;
           try
           {
               bolAjax = ((System.Web.UI.ScriptManager)this.Master.FindControl("myScriptManager")).IsInAsyncPostBack;
           }
           catch
           { }

           if (bolAjax)
           {
               this.lock_displayText(base.Controls);
             
           }
            
        }

        /// <summary>
        /// page load before
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreLoad(EventArgs e)
        {
            
            
            if (base.IsPostBack)
            {
                //this.clearMasked(base.Controls);
            }
            else
            {
                this.ErrorPage = "Error.aspx";
            }

         //   this.lock_displayText(base.Controls);


            base.OnPreLoad(e);
        }


        /// <summary>
        /// 由dialog或WebMethod帶入的值，要key住值，
        /// 所以不得設定Readonly,改用Attribute方式設定
        /// </summary>
        private void lock_displayText(ControlCollection ctls)
        {
            //ScriptManager.IsInAsyncPostBack 
                       

            TextBox txb;
            DropDownList dp;


            foreach (object elm in ctls)
            {
                if (((System.Web.UI.Control)elm).Controls.Count > 0)
                    this.lock_displayText(((System.Web.UI.Control)elm).Controls);
                else
                    if (elm.GetType().Name == "TextBox")
                    {                        
                        txb = ((TextBox)elm);
                        if (txb.CssClass.IndexOf("display") != -1 || txb.CssClass.IndexOf("slock") != -1 ) 
                        {
                            txb.Attributes.Add("readonly", "readonly");
                            txb.Attributes.Add("tabindex", "-1");
                        }
                        else
                        {
                            txb.Attributes.Remove("readonly");
                            txb.Attributes.Remove("tabindex");
                        }
                    }
                    else if (elm.GetType().Name == "srvDropDownList" || elm.GetType().Name == "DropDownList")
                    {                        
                        dp = (DropDownList)elm;                        
                        if ((dp.CssClass).IndexOf("display") != -1 || (dp.CssClass).IndexOf("slock") != -1)
                        {
                            dp.Attributes.Add("onmouseover", "LockDp('" + dp.ClientID + "','" + (dp.Width.Value + 0.3).ToString() + "em" + "')");
                            dp.Attributes.Add("onfocus", "LockDp_ie6()");
                            dp.Attributes.Add("onclick", "LockDp_ie6()");
                            dp.Attributes.Add("onmousedown", "LockDp_ie6()");
                            dp.Attributes.Add("ondblclick", "alert('不得進行變更！');");
                            dp.Attributes.Add("tabindex", "-1");
                        }
                        else
                        {
                            dp.Attributes.Remove("tabindex");
                            dp.Attributes.Remove("onmouseover");
                            dp.Attributes.Remove("onfocus");
                            dp.Attributes.Remove("onclick");
                            dp.Attributes.Remove("onmousedown");
                            dp.Attributes.Remove("ondblclick");
                            dp.Attributes.Add("onchange", "document_onChange(this.id,this.value)");
                        }

                      
                    }
            }
        }

        

        /// <summary>
        /// 設定錯誤訊息對話方塊
        /// </summary>
        /// <param name="strMessage">訊息</param>
        public void setMessageBox(string strMessage)
        {
           
           // ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "MyMessageScript", "alert(\"" + strMessage + "\"); \n ", true);

            string strScript = "try{window.parent.errorMessage('錯誤訊息','" + strMessage + "');}\n";
            strScript += "catch(er){window.parent.parent.errorMessage('錯誤訊息','" + strMessage + "');}\n";
            strScript += "finally{}\n";

            //string strScript = "window.parent.document.getElementById('w').innerHTML=\"" + strMessage + "\";\n";
            //strScript += "window.parent.openMessage();\n";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "MyMessageScript", strScript, true);
           // this.setProcessMessage(strMessage, false); 
            //this.myProcessBar.setMessageBox(strMessage);             
        }
             
   
        /// <summary>
        /// 設定狀態的訊息列
        /// </summary>
        /// <param name="strMessage">訊息</param>
        /// <param name="bolError">是否為Error訊息,如處理失敗..等</param>
        public void setProcessMessage(string strMessage, bool bolError)
        {
            //string strScript = "window.parent.document.getElementById('divMessage').innerHTML=\"" + strMessage + "\";\n";
            //strScript += "window.parent.document.getElementById('divMessage').style.color=\"" + (bolError ? "red" : "") + "\";\n";
            string strScript = "";
            if (bolError)
            {
                strScript = "try{window.parent.errorMessage('錯誤訊息','" + strMessage + "');}\n";
                strScript += "catch(er){window.parent.parent.errorMessage('錯誤訊息','" + strMessage + "');}\n";
                strScript += "finally{}\n";
            }
            else
            {
                strScript = "try{window.parent.slideMessage('處理訊息','" + strMessage + "');}\n";
                strScript += "catch(er){window.parent.parent.slideMessage('處理訊息','" + strMessage + "');}\n";
                strScript += "finally{}\n";
            }

            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "MyProcessScript", strScript, true);
        }

        /// <summary>
        /// 設定狀態的訊息列
        /// </summary>
        /// <param name="strMessage">訊息</param>
        /// <param name="bolError">是否為Error訊息,如處理失敗..等</param>
        public void setScript(string strScript)
        {
            

            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "MyScript"+System.DateTime.Now.ToString("mmssfff"), strScript, true);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        public override void VerifyRenderingInServerForm(Control control)
        {
        }


        /// <summary>
        ///  避掉list control衝突
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void checkList(object sender, EventArgs e)
        {
           
            switch (sender.GetType().Name)
            {
                case "DropDownList":

                    if (((DropDownList)sender).Items.FindByValue(((DropDownList)sender).ToolTip.Trim()) != null)
                        if (((DropDownList)sender).ToolTip != "")
                        {
                            for (int i = 0; i < ((DropDownList)sender).Items.Count; i++)
                            {
                                ((DropDownList)sender).Items[i].Selected = false;
                            }
                            ListItem li = ((DropDownList)sender).Items.FindByValue(((DropDownList)sender).ToolTip.Trim());
                            li.Selected = true;
                            //((DropDownList)sender).Items.FindByValue(((DropDownList)sender).ToolTip).Selected = true;
                        }
                                        
                    break;

                case "CheckBox":
                    if (((CheckBox)sender).ToolTip != null)
                    {
                        if (((CheckBox)sender).ToolTip == "Y")
                            ((CheckBox)sender).Checked = true; 
                    }
                    break;

                case "RadioButtonList":
                    if (((RadioButtonList)sender).Items.FindByValue(((RadioButtonList)sender).ToolTip) != null)
                        ((RadioButtonList)sender).Items.FindByValue(((RadioButtonList)sender).ToolTip).Selected = true;

                    break;

                case "ocxControl":

                    break;

                case "ComboBox":

                    if (((ComboBox)sender).Items.FindByValue(((ComboBox)sender).ToolTip) != null)
                        ((ComboBox)sender).Items.FindByValue(((ComboBox)sender).ToolTip).Selected = true;

                    break;
            }

            ((WebControl)sender).ToolTip = "";
        }
    }

}
