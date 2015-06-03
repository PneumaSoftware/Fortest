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
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace OrixMvc
{
    public partial class WF0101 : PageParent
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
            {
                this.setParms();
            }
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
            this.Master.KeyFields = "SeqNo";






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

            if (this.Master.Master.nowStatus == "Add")
            {
                this.EMP_CODE.Text = this.Master.Master.CorpAcct;
                this.EMP_NAME.Text = this.Master.Master.EmployeeName;
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
            string strSQL = "";
            string strMessage = "";


            switch (strStatus)
            {
                case "Add":
                case "Copy":
                case "Upd":
                    if (this.EMP_CODE.Text.Trim() == "")
                        strMessage += "[員工代號]";

                    //類別代號>50，可不輸入【客戶名稱】及【聯絡人】
                    if (this.TOPIC.Text.Trim() == "" || Convert.ToInt16(this.TOPIC.Text) < 50)
                    {
                        if (this.CUST_CODE.Text.Trim() == "" && this.SUPL_CODE.Text.Trim() == "")
                        {
                            strMessage += "[供應商代號]及[客戶代號]請擇一輸入";
                            this.setMessageBox(strMessage);
                            return false;
                        }

                        if (this.CTAC.Text.Trim() == "")
                        {
                            strMessage += "[聯絡人]";
                        }
                    }

                    if (this.VISIT_DAT.Text.Trim() == "")
                        strMessage += "[拜訪日期]";

                    if (this.Mes_TStart.Text.Trim() == "")
                        strMessage += "[起時]";

                    if (this.Mes_TStop.Text.Trim() == "")
                        strMessage += "[迄時]";


                    if (this.TOPIC.Text.Trim() == "")
                        strMessage += "[類別]";

                    if (this.REC_MEAT.Text.Trim() == "")
                        strMessage += "[訪談內容]";

                    if (strMessage != "")
                        strMessage += "必須輸入！";

                    if (this.Mes_TStart.Text.Trim() != "" && this.Mes_TStop.Text.Trim() != "")
                    {
                        if (this.Mes_TStop.Text.Trim().CompareTo(this.Mes_TStart.Text.Trim()) <= 0)
                            strMessage += "\\r\\n迄時必須大於起時！";
                    }

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
            DataSetToSql dtsOtc = new DataSetToSql("orixConn");
            CommandToSql ctsOtc = new CommandToSql("orixConn");

            if (!DataCheck(strStatus))
                return 0;


            if (this.SeqNo.Text == "")
                this.SeqNo.Text = "0";

            DataTable dt = dts.GetTable("OR3_VISIT_REC", "SeqNo=" + this.SeqNo.Text + "");
            DataRow dr = null;
            string strAtmNum = "";
            switch (strStatus)
            {

                case "Add":
                case "Copy":
                case "Upd":
                    #region DIPLOMATICEMP
                    string strSql = "";
                    string strReturn = "";

                    if (this.TOPIC.Text.Trim() != "" && Convert.ToInt16(this.TOPIC.Text) < 20)
                    {
                        #region Data
                        string strMes_Start = Convert.ToDateTime(this.VISIT_DAT.Text + " " + this.Mes_TStart.Text).ToString();
                        string strMes_Stop = Convert.ToDateTime(this.VISIT_DAT.Text + " " + this.Mes_TStop.Text).ToString();
                        string strLAW_Content = this.RecTitle.Text;
                        string strCust_Name = this.CUST_NAME.Text;
                        string strRul_Place = this.RulPlace.Text;
                        string strAdd_Name = this.Master.Master.EmployeeName;
                        string strCreate_Date = System.DateTime.Now.ToShortDateString();
                        string strCorp_Acct = this.Master.Master.CorpAcct;
                        string strCDATE = Convert.ToDateTime(this.VISIT_DAT.Text).ToShortDateString();
                        #endregion

                        if (this.AtmNum.Text == "" || dtsOtc.GetTable("DiplomaticEmp", "Atm_Num=" + this.AtmNum.Text + "").Rows.Count == 0)
                        {
                            #region 新增
                            Object[][] oData = new Object[][]{
                                new Object[] {"@Mes_Start", "@Mes_Stop", "@LAW_Content", "@Cust_Name", "@Rul_Place", "@Add_Name", "@Create_Date", "@Corp_Acct", "@CDATE"},
                                new Object[] {strMes_Start, strMes_Stop, strLAW_Content, strCust_Name, strRul_Place, strAdd_Name, strCreate_Date, strCorp_Acct, strCDATE}};

                            strSql = "insert into DiplomaticEmp";
                            strSql += "(Mes_Start, Mes_Stop, LAW_Content, Cust_Name, Rul_Place, Add_Name, Create_Date, Corp_Acct, CDATE)";
                            strSql += "values";
                            strSql += "(@Mes_Start, @Mes_Stop, @LAW_Content, @Cust_Name, @Rul_Place, @Add_Name, @Create_Date, @Corp_Acct, @CDATE)";
                            strSql += "select @@IDENTITY";
                            #endregion

                            strReturn = ctsOtc.ExecuteGetDataForParam(strSql, oData, true);

                            if (strReturn != "false")
                            {
                                //取得Atm_Num
                                strAtmNum = strReturn;
                            }
                            else
                            {
                                return 1;
                            }
                        }
                        else
                        {
                            #region 修改
                            Object[][] oData = new Object[][]{
                                new Object[] {"@Mes_Start", "@Mes_Stop", "@LAW_Content", "@Cust_Name", "@Rul_Place", "@Add_Name", "@Create_Date", "@Corp_Acct", "@CDATE", "@ATM_NUM"},
                                new Object[] {strMes_Start, strMes_Stop, strLAW_Content, strCust_Name, strRul_Place, strAdd_Name, strCreate_Date, strCorp_Acct, strCDATE, this.AtmNum.Text}};

                            strSql = "UPDATE DiplomaticEmp ";
                            strSql += "SET Mes_Start = @Mes_Start, Mes_Stop = @Mes_Stop, LAW_Content = @LAW_Content, ";
                            strSql += "Cust_Name = @Cust_Name, Rul_Place = @Rul_Place, Add_Name = @Add_Name, ";
                            strSql += "Create_Date = @Create_Date, Corp_Acct = @Corp_Acct, CDATE = @CDATE ";
                            strSql += "WHERE ATM_NUM = @ATM_NUM";
                            #endregion

                            if (ctsOtc.ExecuteForParam(strSql, oData, true))
                            {
                                //取得Atm_Num
                                strAtmNum = this.AtmNum.Text;
                            }
                            else
                            {
                                return 1;
                            }
                        }

                    }
                    #endregion

                    #region OR3_VISIT_REC
                    if (dt.Rows.Count == 0)
                    {
                        dr = dt.NewRow();
                        dr["EMP_CODE"] = this.Master.Master.EmployeeId;
                        dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                    }
                    else
                    {
                        dr = dt.Rows[0];
                        dr["EMP_CODE"] = this.convertToEmployeeCode(this.EMP_CODE.Text);

                        if (this.AtmNum.Text.Trim() != "" && this.TOPIC.Text.Trim() != "" && Convert.ToInt16(this.TOPIC.Text) > 20) 
                        {
                            dr["ATM_NUM"] = DBNull.Value;
                            DataTable dtD = dtsOtc.GetTable("DiplomaticEmp", "Atm_Num=" + this.AtmNum.Text.Trim() + "");
                            if (dtD.Rows.Count > 0)
                            {
                                dtD.DeleteRows();
                                dtsOtc.Save();
                            }
                        }
                    }

                    dr["SUPL_CODE"] = this.SUPL_CODE.Text;
                    dr["CUST_CODE"] = this.CUST_CODE.Text;
                    dr["CTAC"] = this.CTAC.Text;
                    dr["VISIT_DAT"] = this.VISIT_DAT.Text.Replace("/", "");
                    dr["Mes_TStart"] = this.Mes_TStart.Text;
                    dr["Mes_TStop"] = this.Mes_TStop.Text;
                    dr["FORECAST_APLY_AMT"] = this.FORECAST_APLY_AMT.Text;
                    dr["INTERVIEW_TOPIC"] = this.TOPIC.Text;
                    dr["Rec_Meat"] = this.REC_MEAT.Text;
                    dr["TRANSPORTATION"] = this.TRANSPORTATION.SelectedValue;
                    dr["APLY_FEE"] = this.APLY_FEE.Text;

                    dr["CASE_SOUR"] = this.CaseSour.SelectedValue;
                    dr["REC_TITLE"] = this.RecTitle.Text;
                    dr["RUL_PLACE"] = this.RulPlace.Text;
                    dr["RUL_OTHER"] = this.RulOther.Text;
                    if (this.TOPIC.Text.Trim() != "" && Convert.ToInt16(this.TOPIC.Text) < 20)
                    {
                        dr["ATM_NUM"] = strAtmNum;
                    }

                    dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                    dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                    dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                    if (dt.Rows.Count == 0)
                        dt.Rows.Add(dr);
                    #endregion
                    break;
                case "Del":
                    #region Delete
                    dt.DeleteRows();

                    if (this.AtmNum.Text.Trim() != "")
                    {
                        DataTable dtD = dtsOtc.GetTable("DiplomaticEmp", "Atm_Num=" + this.AtmNum.Text.Trim() + "");
                        if (dtD.Rows.Count > 0)
                        {
                            dtD.DeleteRows();

                            if (!dtsOtc.Save())
                            {
                                return 1;
                            }
                        }
                    }
                    break;
                    #endregion
            }

            if (dts.Save())
            {
                return 2;
            }
            else
            {
                return 1;
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

        //[WebMethod]
        //public static string getTopic(string INTERVIEW_TOPIC)
        //{
        //    VS2008.Module.DataGetting dgs = new VS2008.Module.DataGetting();

        //    string strSQL = "select typecode, typecode + ' ' + typedesc as typedesc from OR3_COND_DEF WHERE TypeField = 'INTERVIEW_TOPIC' ";
        //    strSQL += "And (End_date is null or End_date > GETDATE())";
        //    strSQL += "Union select typecode, typecode + ' ' + typedesc as typedesc from OR3_COND_DEF WHERE TypeField = 'INTERVIEW_TOPIC' AND TypeCode = '" + INTERVIEW_TOPIC + "'";

        //    DataTable dtI = dgs.GetDataTable(strSQL);
        //    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        //    Dictionary<string, object> row;
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();

        //    foreach (DataRow dr in dtI.Rows)
        //    {
        //        row = new Dictionary<string, object>();
        //        foreach (DataColumn col in dtI.Columns)
        //        {
        //            row.Add(col.ColumnName, dr[col]);
        //        }
        //        rows.Add(row);
        //    }
        //    return serializer.Serialize(rows);
        //}
    }
}