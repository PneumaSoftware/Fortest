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
    public partial class WZ0201 : PageParent
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
            this.Master.KeyFields = "USER_ID";

          
            

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
                    string strFUNC_ID = e.CommandName;
                    DataRow drFR = dtGridSource.Select("FUNC_ID='" + strFUNC_ID + "'")[0];
                    drFR["STATUS"] = "D";
                    rptEdit.DataSource = dtGridSource;
                    rptEdit.DataBind();
                    this.upGrid.Update();


                    break;

                case "btnAdd":
                    string strMessage = "";
                    if (this.addFUNC_ID.Text.Trim() == "")
                        strMessage += "[功能代碼]";

                 
                    if (strMessage != "")
                    {
                        strMessage += " 必須輸入！";

                        strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                        this.setMessageBox(strMessage);
                        return;
                    }




                    dtGridSource = GetGridSource(true,this.USER_ID.Text);
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
                    upGrid.Update();

                    this.addFUNC_ID.Clearing();

                   

                    // this.upDetailEditing.Update();
                    break;

              

            }
        }


        private void  SaveGrid()
        {

            
            DataTable dtFUNC = dts.GetTable("OR3_USER_FUNC", "USER_ID='" + this.USER_ID.Text + "'");  
            DataTable dtFUNCB = dts.GetTable("OR3_USER_FUNC_BTN", "USER_ID='" + this.USER_ID.Text + "'");            
            DataRow dr;

            this.updateGrid(this.GetGridSource(false,this.USER_ID.Text), rptEdit);

            DataTable dtEdit = this.GetGridSource(false, this.USER_ID.Text);
            
            /*END 勿動 更新Grid*/




            DataRow[] drCopy;

            for (int i = 0; i < this.rptEdit.Items.Count; i++)
            {
                string FUNC_ID = this.rptEdit.Items[i].FindControl("FUNC_ID").value();
                string AVAILABLE = this.rptEdit.Items[i].FindControl("AVAILABLE").value();
                string CAN_ADD = this.rptEdit.Items[i].FindControl("CAN_ADD").value();
                string CAN_UPDATE = this.rptEdit.Items[i].FindControl("CAN_UPDATE").value();
                string CAN_DELETE = this.rptEdit.Items[i].FindControl("CAN_DELETE").value();
                string CAN_EXPORT = this.rptEdit.Items[i].FindControl("CAN_EXPORT").value();
                string CAN_COPY = this.rptEdit.Items[i].FindControl("CAN_COPY").value();
                CheckBox[] chk = new CheckBox[10];
                chk[0]=(CheckBox)this.rptEdit.Items[i].FindControl("Enable_1");
                chk[1] = (CheckBox)this.rptEdit.Items[i].FindControl("Enable_2");
                chk[2] = (CheckBox)this.rptEdit.Items[i].FindControl("Enable_3");
                chk[3] = (CheckBox)this.rptEdit.Items[i].FindControl("Enable_4");
                chk[4] = (CheckBox)this.rptEdit.Items[i].FindControl("Enable_5");
                chk[5] = (CheckBox)this.rptEdit.Items[i].FindControl("Enable_6");
                chk[6] = (CheckBox)this.rptEdit.Items[i].FindControl("Enable_7");
                chk[7] = (CheckBox)this.rptEdit.Items[i].FindControl("Enable_8");
                chk[8] = (CheckBox)this.rptEdit.Items[i].FindControl("Enable_9");
                chk[9] = (CheckBox)this.rptEdit.Items[i].FindControl("Enable_10");


                string STATUS = this.rptEdit.Items[i].FindControl("STATUS").value();
                if (STATUS == "D")
                {
                    dtFUNC.DeleteRows("FUNC_ID='" + FUNC_ID + "'");
                    dtFUNCB.DeleteRows("FUNC_ID='" + FUNC_ID + "'");
                }
                else
                {
                    drCopy = dtFUNC.Select("FUNC_ID='" + FUNC_ID + "'");
                    if (drCopy.Length == 0)
                        dr = dtFUNC.NewRow();
                    else
                        dr = drCopy[0];

                    dr["FUNC_ID"] = FUNC_ID;
                    dr["AVAILABLE"] = AVAILABLE == "Y";
                    dr["USER_ID"] = this.USER_ID.Text; 
                    dr["CAN_ADD"] = CAN_ADD == "Y";
                    dr["CAN_UPDATE"] = CAN_UPDATE == "Y";
                    dr["CAN_DELETE"] = CAN_DELETE == "Y";
                    dr["CAN_EXPORT"] = CAN_EXPORT == "Y";
                    dr["CAN_COPY"] = CAN_COPY == "Y";


                    if (drCopy.Length== 0)
                    {
                        dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["ADD_DATE"] = DateTime.Now.ToString("yyyyMMdd");
                        dr["ADD_TIME"] = DateTime.Now.ToString("HH:mm:ss");
                    }
                   
                    dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                    dr["LAST_UPD_DATE"] = DateTime.Now.ToString("yyyyMMdd");
                    dr["LAST_UPD_TIME"] = DateTime.Now.ToString("HH:mm:ss");
                    if (drCopy.Length == 0)
                        dtFUNC.Rows.Add(dr);

                    if (chk[0].ToolTip != "" || chk[1].ToolTip != "" || chk[2].ToolTip != "" || chk[3].ToolTip != "" || chk[4].ToolTip != "" ||
                        chk[5].ToolTip != "" || chk[6].ToolTip != "" || chk[7].ToolTip != "" || chk[8].ToolTip != "" || chk[9].ToolTip != "")
                    {

                        for (int j = 1; j <= 10; j++)
                        {
                            if (chk[j - 1].ToolTip != "")
                            {
                                drCopy = dtFUNCB.Select("FUNC_ID='" + FUNC_ID + "' and Seq_No=" + j.ToString());
                                if (drCopy.Length == 0)
                                    dr = dtFUNCB.NewRow();
                                else
                                    dr = drCopy[0];


                                dr["FUNC_ID"] = FUNC_ID;
                                dr["USER_ID"] = this.USER_ID.Text; 
                                dr["Seq_No"] = j;
                                dr["Enable"] = chk[j-1].Checked;


                                if (drCopy.Length == 0)
                                {
                                    dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                                    dr["ADD_DATE"] = DateTime.Now.ToString("yyyyMMdd");
                                    dr["ADD_TIME"] = DateTime.Now.ToString("HH:mm:ss");
                                }

                                dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                                dr["LAST_UPD_DATE"] = DateTime.Now.ToString("yyyyMMdd");
                                dr["LAST_UPD_TIME"] = DateTime.Now.ToString("HH:mm:ss");

                                if (drCopy.Length == 0)
                                    dtFUNCB.Rows.Add(dr);

                            }
                        }
                       
                    }
                }

            }

          


        }


        private string GetGridSql(string strUserid)
        {
            string strSQL = "";
            strSQL += " exec s_WZ0201_Grid @USERID='" + strUserid + "'";           

            return strSQL;
        }



        private DataTable GetGridSource(bool bolAdd,string strUserid)
        {
            DataTable dtGridSource = null;

            if (ViewState["GridSource"] == null)
            {

                dtGridSource = dg.GetDataTable(this.GetGridSql(strUserid));

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
                if (this.addFUNC_ID.SelectedIndex == 0)
                {
                    this.setMessageBox("功能代碼必須選擇!");
                    return null;
                }

                dtGridSource = (DataTable)ViewState["GridSource"];
                this.updateGrid(dtGridSource, rptEdit);
                DataRow dtRow = dtGridSource.NewRow();
                dtRow["FUNC_ID"] = this.addFUNC_ID.Text.Trim();
                dtRow["FUNC_NAME"] = this.addFUNC_ID.SelectedItem.Text.Trim().Split('：')[1].ToString();
                dtRow["AVAILABLE"] = addAVAILABLE.Checked;
                dtRow["CAN_ADD"] = addCAN_ADD.Checked;
                dtRow["CAN_UPDATE"] = addCAN_UPDATE.Checked;
                dtRow["CAN_DELETE"] = addCAN_DELETE.Checked;
                dtRow["CAN_EXPORT"] = addCAN_EXPORT.Checked;
                dtRow["CAN_COPY"] = addCAN_COPY.Checked;
                dtRow["Enable_1"] = addEnable_1.Checked;
                dtRow["Enable_2"] = addEnable_2.Checked;
                dtRow["Enable_3"] = addEnable_3.Checked;
                dtRow["Enable_4"] = addEnable_4.Checked;
                dtRow["Enable_5"] = addEnable_5.Checked;
                dtRow["Enable_6"] = addEnable_6.Checked;
                dtRow["Enable_7"] = addEnable_7.Checked;
                dtRow["Enable_8"] = addEnable_8.Checked;
                dtRow["Enable_9"] = addEnable_9.Checked;
                dtRow["Enable_10"] = addEnable_10.Checked;
                dtRow["BTN_NAME_1"] = txtEnable_1.Text.Trim();
                dtRow["BTN_NAME_2"] = txtEnable_2.Text.Trim();
                dtRow["BTN_NAME_3"] = txtEnable_3.Text.Trim();
                dtRow["BTN_NAME_4"] = txtEnable_4.Text.Trim();
                dtRow["BTN_NAME_5"] = txtEnable_5.Text.Trim();
                dtRow["BTN_NAME_6"] = txtEnable_6.Text.Trim();
                dtRow["BTN_NAME_7"] = txtEnable_7.Text.Trim();
                dtRow["BTN_NAME_8"] = txtEnable_8.Text.Trim();
                dtRow["BTN_NAME_9"] = txtEnable_9.Text.Trim();
                dtRow["BTN_NAME_10"] = txtEnable_10.Text.Trim();
                dtRow["Status"] = "A";


                dtGridSource.Rows.Add(dtRow);
                ViewState["GridSource"] = dtGridSource;

                this.addFUNC_ID.SelectedIndex = 0;
                this.addAVAILABLE.Checked = false;
                this.addCAN_ADD.Checked = false;
                this.addCAN_UPDATE.Checked = false;
                this.addCAN_DELETE.Checked = false;
                this.addCAN_EXPORT.Checked = false;
                this.addCAN_COPY.Checked = false;
                this.addEnable_1.Checked = false;
                this.addEnable_2.Checked = false;
                this.addEnable_3.Checked = false;
                this.addEnable_4.Checked = false;
                this.addEnable_5.Checked = false;
                this.addEnable_6.Checked = false;
                this.addEnable_7.Checked = false;
                this.addEnable_8.Checked = false;
                this.addEnable_9.Checked = false;
                this.addEnable_10.Checked = false;
            }

            return dtGridSource;
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

            DataTable dtGridSource = this.GetGridSource(false,this.USER_ID.Text);
            rptEdit.DataSource = dtGridSource;
            rptEdit.DataBind();
        }
        #endregion

        protected void Type_Change(object sender, EventArgs e)
        {
            if (this.USER_TYPE.SelectedIndex == 1)
            {
                this.EMP_CODE.Text = "";
                this.EMP_CODE.Editing(false);
                this.sUSER_PASS.Text = "";
                this.USER_PASS.Value = "";

                this.PWD_DATE.Text = "";
                this.PWD_DATE.Editing(false);
                this.GROUP_ID.SelectedIndex = 0;
                this.GROUP_ID.Editing(false);
            }
            else
            {

                this.EMP_CODE.Editing(true);   
                this.PWD_DATE.Editing(true);
                this.GROUP_ID.Editing(true);
            }
        }
        //public void Group_Change(object sender, EventArgs e)
        //{
        //    ViewState["GridSource"] = null;
        //        DataTable dtGridSource = this.GetGridSource(false, this.GROUP_ID.SelectedValue);
        //        rptEdit.DataSource = dtGridSource;
        //        rptEdit.DataBind();

        //        this.Hint.Checked = (dg.GetDataRow("select HINT from OR3_USERS where USER_ID='" + this.GROUP_ID.SelectedValue.Trim() + "'")[0].ToString().Trim() == "Y" ? true : false);
            
        //}
        private void setFunction()
        {
            //篩選已經選取的功能
            DataTable dt = dg.GetDataTable("select FUNC_ID='',FUNC_NAME='請選擇...',Show='Y' union all select FUNC_ID,FUNC_NAME=Func_ID+' '+FUNC_NAME,Show='Y'  from OR3_FUNCTION where FUNC_TYPE='P'");

            DataTable dtGrid = this.GetGridSource(false,this.USER_ID.Text);

            for (int i = 0; i < dtGrid.Rows.Count; i++)
            {
                DataRow[] aryDr = dt.Select("FUNC_ID='" + dtGrid.Rows[i]["FUNC_ID"].ToString() + "'");
                if (aryDr.Length > 0)
                    aryDr[0]["Show"] = "";
            }

            this.addFUNC_ID.Items.Clear();

            DataView dv = dt.DefaultView;
            dv.RowFilter = "Show<>''";
            this.addFUNC_ID.DataSource = dv;
            this.addFUNC_ID.DataBind();

            string strScript = "";
            strScript+="document.getElementById('"+ addCAN_ADD.ClientID+"').style.display='none';\n";
            strScript += "document.getElementById('" + addCAN_UPDATE.ClientID + "').style.display='none';\n";
            strScript += "document.getElementById('" + addCAN_DELETE.ClientID + "').style.display='none';\n";
            strScript += "document.getElementById('" + addCAN_EXPORT.ClientID + "').style.display='none';\n";
            strScript += "document.getElementById('" + addCAN_COPY.ClientID + "').style.display='none';\n";
            strScript += "document.getElementById('" + addEnable_1.ClientID + "').style.display='none';\n";
            strScript += "document.getElementById('" + addEnable_2.ClientID + "').style.display='none';\n";
            strScript += "document.getElementById('" + addEnable_3.ClientID + "').style.display='none';\n";
            strScript += "document.getElementById('" + addEnable_4.ClientID + "').style.display='none';\n";
            strScript += "document.getElementById('" + addEnable_5.ClientID + "').style.display='none';\n";
            strScript += "document.getElementById('" + addEnable_6.ClientID + "').style.display='none';\n";
            strScript += "document.getElementById('" + addEnable_7.ClientID + "').style.display='none';\n";
            strScript += "document.getElementById('" + addEnable_8.ClientID + "').style.display='none';\n";
            strScript += "document.getElementById('" + addEnable_9.ClientID + "').style.display='none';\n";
            strScript += "document.getElementById('" + addEnable_10.ClientID + "').style.display='none';\n";

            this.setScript(strScript);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void FuncID_Change(object sender, EventArgs e)
        {
            //string strValue = this.addFUNC_ID.SelectedValue;
            string strSQL = "exec s_WZ0201_FuncBtn '" + this.addFUNC_ID.SelectedValue + "'";
            DataRow dr = dg.GetDataRow(strSQL);
            if (dr[0].ToString()!="")
            {
                string strScript = "";


                strScript += "document.getElementById('" + addAVAILABLE.ClientID + "').checked=true;\n";
                strScript += "document.getElementById('" + addCAN_ADD.ClientID + "').checked=true;\n";
                strScript += "document.getElementById('" + addCAN_UPDATE.ClientID + "').checked=true;\n";
                strScript += "document.getElementById('" + addCAN_DELETE.ClientID + "').checked=true;\n";
                strScript += "document.getElementById('" + addCAN_EXPORT.ClientID + "').checked=true;\n";
                strScript += "document.getElementById('" + addCAN_COPY.ClientID + "').checked=true;\n";
               

                for (int i=1;i<=10;i++)
                {
                    CheckBox chk= (CheckBox)this.Master.Master.masterFindControl("addEnable_"+i.ToString());
                    TextBox txt = (TextBox)this.Master.Master.masterFindControl("txtEnable_" + i.ToString());
                    HtmlGenericControl span = (HtmlGenericControl)this.Master.Master.masterFindControl("addSpan_" + i.ToString());
                    string val=dr["BTN_NAME_"+i.ToString()].ToString().Trim();

                    if (val!="")
                        strScript += "document.getElementById('"+ txt.ClientID + "').value='"+ val+"';\n";

                    strScript+="document.getElementById('"+ chk.ClientID+"').value='"+val +"';\n";
                    strScript += "document.getElementById('" + chk.ClientID + "').checked=true;\n";
                    strScript += "document.getElementById('" + span.ClientID + "').style.display='" + (val == "" ? "none" : "") + "';\n";

                }

                this.setScript(strScript);
            }


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
                    if (dg.GetDataRow("select 'Y' from OR3_USERS where USER_ID='" + this.USER_ID.Text + "'")[0].ToString() == "Y")
                    {
                        strMessage += "\\r\\n使用者ID/群組代號已存在！";
                    }

                    if (this.USER_ID.Text.Trim() == "")
                        strMessage += "\\r\\n使用者ID/群組代號必須輸入！";


                    if (this.EMP_CODE.Text.Trim() == "" && this.USER_TYPE.SelectedIndex != 1)
                        strMessage += "\\r\\n員工代號必須輸入！";

                    if (this.USER_NAME.Text.Trim() == "")
                        strMessage += "\\r\\n使用者姓名必須輸入！";

                    if (this.USER_PASS.Value.Trim() == ""  && this.USER_TYPE.SelectedIndex != 1)
                        strMessage += "\\r\\n使用者密碼必須設定！";

                    if (this.USER_TYPE.SelectedValue == "")
                        strMessage += "\\r\\使用者區分必須選取！";


                    if (strMessage != "")
                    {
                        strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                        this.setMessageBox(strMessage);
                        return false;
                    }

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


            DataTable dtUsers = dts.GetTable("OR3_USERS", "USER_ID='" + this.USER_ID.Text + "'");
            
            

            DataRow dr = null;

            switch (strStatus)
            {

                case "Add":
                case "Copy":
                case "Upd":
                    if (strStatus != "Upd")
                        dr = dtUsers.NewRow();
                    else
                        dr = dtUsers.Rows[0];

                    dr["USER_ID"] = this.USER_ID.Text;
                    dr["USER_NAME"] = this.USER_NAME.Text;
                    dr["GROUP_ID"] = this.GROUP_ID.SelectedValue;
                    dr["EMP_CODE"] = this.EMP_CODE.Text.Trim();
                    if (dr["USER_PASS"].ToString() != this.USER_PASS.Value)
                        dr["PWD_SETTING_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");

                    dr["USER_PASS"] = this.USER_PASS.Value;
                    dr["USER_TYPE"] = this.USER_TYPE.SelectedValue;
                    
                        dr["HINT"] = this.Hint.Checked ? "Y" : "";
                    
                    if (strStatus != "Upd")
                    {
                        dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                    }
                    dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                    dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                    dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                    if (strStatus != "Upd")
                        dtUsers.Rows.Add(dr);

                   
                    this.SaveGrid();

                    break;
                case "Del":
                    DataTable dtFUNC = dts.GetTable("OR3_USER_FUNC", "USER_ID='" + this.USER_ID.Text + "'");
                    DataTable dtFUNCB = dts.GetTable("OR3_USER_FUNC_BTN", "USER_ID='" + this.USER_ID.Text + "'");
                    dtUsers.DeleteRows();
                    dtFUNC.DeleteRows();
                    dtFUNCB.DeleteRows();

                    break;

            }

            if (dts.Save())
                return 2;
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

                    if (this.USER_PASSN.Text.Trim() == "")
                        strMessage += "\\r\\n新密碼必須輸入！";


                    if (this.USER_PASSNS.Text.Trim() == "")
                        strMessage += "\\r\\n確認密碼必須輸入！";


                    if (this.USER_PASSN.Text.Trim() != this.USER_PASSNS.Text.Trim())
                        strMessage += "\\r\\n新密碼及確認密碼必須相同！";



                    if (strMessage != "")
                    {
                        strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                        this.setMessageBox(strMessage);
                        return;
                    }
                    //if (dr["USER_PASS"].ToString() != this.USER_PASS.Value)
//                        dr["PWD_SETTING_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");

                    this.USER_PASS.Value = this.USER_PASSN.Text.GetMD5();
                    this.upPASS.Update();
                    cts.Execute("update OR3_USERS set USER_PASS='"+this.USER_PASS.Value.Trim()+"',PWD_SETTING_DATE='"+ System.DateTime.Now.ToString("yyyyMMdd")+"' WHERE USER_ID='" + this.USER_ID.Text + "'");
                    
                    this.setScript("window.parent.closePopUpWindow();");
                    break;
            }
        }
    }
}