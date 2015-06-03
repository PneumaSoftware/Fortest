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
    public partial class WE0101 : PageParent
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


        /// <summary>
        /// 是否由WF010呼叫
        /// </summary>
        /// <returns></returns>
        public bool  bolWF0101
        {
            set { ViewState["bolWF0101"] = value; }
            get { return (ViewState["bolWF0101"] == null ? false : (bool )ViewState["bolWF0101"]); }
        }

        /// <summary>
        /// 是否由WE030呼叫
        /// </summary>
        /// <returns></returns>
        public bool bolWE030
        {
            set { ViewState["bolWE030"] = value; }
            get { return (ViewState["bolWE030"] == null ? false : (bool)ViewState["bolWE030"]); }
        }
        /// <summary>
        /// GRID DATATALBE
        /// </summary>
        /// <returns></returns>
        public DataTable dtGrid
        {
            set { ViewState["dtGrid"] = value; }
            get { return (ViewState["dtGrid"] == null ? null : (DataTable)ViewState["dtGrid"]); }
        }

        /// <summary>
        /// GRID DATATALBE
        /// </summary>
        /// <returns></returns>
        public DataTable dtFunc
        {
            set { ViewState["dtFunc"] = value; }
            get { return (ViewState["dtFunc"] == null ? null : (DataTable)ViewState["dtFunc"]); }
        }

        /// <summary>
        /// GRID FROM ADDIndex
        /// </summary>
        /// <returns></returns>
        public int func_add
        {
            set { ViewState["func_add"] = value; }
            get { return (ViewState["func_add"] == null ? 0 : (int)ViewState["func_add"]); }
        }


        public bool bolProject
        {
            set { ViewState["bolProject"] = value; }
            get { return (ViewState["bolProject"] == null ? false : (bool)ViewState["bolProject"]); }
        }

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

                WE010 myForm = new WE010();
                string strSQL = myForm.callFromWF0101();


                strSQL += " and CUST_NO=''";
                this.Master.Master.nowStatus = "Add";
                this.Master.queryString = strSQL;                
                this.Master.bolExit = false;
                //this.Master.setParms("Add");
            }

            if (this.Request.UrlReferrer.AbsolutePath.IndexOf("WE030") != -1)
            {
                bolWE030 = true;

                WE010 myForm = new WE010();
                string strSQL = myForm.callFromWF0101();


                strSQL += " and CUST_NO='"+this.Request.QueryString["CUST_NO"].ToString().Trim()+"'";
                this.Master.Master.nowStatus = "Upd";
                this.Master.queryString = strSQL;
                this.Master.bolExit = false;
                //this.Master.setParms("Add");
            }

            //3.key fields
            this.Master.KeyFields = "CUST_NO";




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


            if (this.Master.Master.nowStatus == "Add")
            {
                if (this.CUST_GEN_QUOTA.Text.Trim().toInt() != 0 || this.CUST_VP_QUOTA.Text.Trim().toInt() != 0 || this.CUST_AR_QUOTA.Text.Trim().toInt() != 0)
                    this.bolProject = true;

                if (this.bolWF0101)
                {
                    this.CUST_STS.Checked = true;
                    this.INVOICE.SelectedIndex = 1;
                }
            }

            if (this.Master.Master.nowStatus == "Upd")
            {
                if (CUST_STS.ToolTip!="Y" )
                    this.CUST_STS.Editing(false);
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

            if (this.bolWF0101)
                strStatus = "Add";

            if (this.bolWE030)
                strStatus = "Upd";

            switch (strStatus)
            {
                case "Add":
                case "Copy":
                case "Upd":
                    if (dg.GetDataRow("select top 1 'Y' from OR_CUSTOM where UNIF_NO='" + this.UNIF_NO.Text.Trim() + "' and CUST_NO!='" + this.CUST_NO.Text + "'")[0].ToString().Trim() == "Y" && this.UNIF_NO.Text.Trim() != "")
                    {
                        this.setMessageBox("統編重複，請查詢修改資料或更正統一編號！");
                        return false;
                    }

                    if (this.UNIF_NO.Text.Trim() == "" && this.chkINVOICE.Text == "")
                    {
                        this.setScript("checkFunction('1');");
                        return false;
                    }

                    if ( this.chkExist.Text == "")
                    {
                        if (dg.GetDataRow("select top 1 'Y' from OR_CUSTOM where CUST_NAME='" + this.CUST_NAME.Text + "'  and CUST_NO!='" + this.CUST_NO.Text + "'")[0].ToString().Trim() == "Y")
                        {
                            this.setScript("checkFunction('2');");
                            return false;
                        }
                    }
                  //  if (this.CUST_NO.Text.Trim() == "")
                  //      strMessage += "[客戶代號]";

                    if (this.CUST_NAME.Text.Trim() == "")
                        strMessage += "[客戶名稱]";

                    if (this.CUST_SNAME.Text.Trim() == "")
                        strMessage += "[客戶簡稱]";

                  //  if (this.CONTACT.Text.Trim() == "" && !this.CUST_STS.Checked )
                  //      strMessage += "[聯絡人]";

                    //潛在客戶: 客戶名稱,客戶簡稱
                    //一般客戶: 客戶名稱,客戶簡稱,  行業別,租賃公會行業別,登記地址,聯絡地址,實收資本額,組織型態,發票方式
                    //必須輸入欄位為: 客戶名稱,客戶簡稱,  行業別,租賃公會行業別,登記地址,聯絡地址,實收資本額,組織型態,發票方式
               
                    //成交客
                    if (this.IS_TRANSACTION.Checked || !this.CUST_STS.Checked )
                    {
                        if (this.CUST_TYPE_CODE.SelectedValue.Trim() == "")
                            strMessage += "[行業別]";

                        if (this.CUST_UNION.SelectedValue.Trim() == "")
                            strMessage += "[租賃公會行業別]";

                        if (this.NATIONALITY.SelectedValue.Trim() == "")
                            strMessage += "[國別]";

                    //    if (this.UNIF_NO.Text.Trim() == "")
                    //        strMessage += "[統一編號]";

                        if (this.SALES_RGT_ADDR.Text.Trim() == "")
                            strMessage += "[營業登記地址]";

                        if (this.CTAC_ADDR.Text.Trim() == "")
                            strMessage += "[連絡地址]";

                        if (this.REAL_CAPT_AMT.Text.Trim() == "0")
                            strMessage += "[實收資本額]";

                        if (this.ORG_TYPE.Text.Trim() == "")
                            strMessage += "[組織型態]";

                        if (this.INVOICE.Text.Trim() == "")
                            strMessage += "[發票開立方式]";

                    }

                 
                    

                        if (strMessage != "")
                    {
                        strMessage += " 必須輸入！";
                        this.setMessageBox(strMessage);
                        return false;
                    }

                    if (strMessage != "")
                    {
                        strMessage = (strMessage.Substring(0, 4) == "\\r\\n" ? strMessage.Substring(4) : strMessage);
                        this.setMessageBox(strMessage);
                        return false;
                    }

                    if (this.Master.Master.nowStatus == "Add" || this.bolWF0101==true)
                    {
                        if (dg.GetDataRow("select 'Y' from OR_CUSTOM where Cust_NO='" + this.CUST_NO.Text.rpsText() + "'")[0].ToString() == "Y")
                        {
                            this.setMessageBox("客戶代號已存在,請確認!");
                            return false;
                        }
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


            DataTable dt =  dts.GetTable("OR_CUSTOM", "CUST_NO='" + this.CUST_NO.Text.rpsText()  + "'");
            DataRow dr = null;

            if (bolWF0101)
                strStatus = "Add";

            if (this.bolWE030)
                strStatus = "Upd";

            switch (strStatus)
            {

                case "Add":
                case "Copy":
                case "Upd":
                    if (dt.Rows.Count == 0)
                    {
                        if (CUST_NO.Text.Trim() == "")
                        {
                            if (this.UNIF_NO.Text.Trim() != "")
                                this.CUST_NO.Text = this.UNIF_NO.Text.Trim();
                            else
                                this.CUST_NO.Text=dg.GetDataRow(" EXEC s_GetNumber 'S', 'C', "+ DateTime.Now.Year +", "+ DateTime.Now.Month)[0].ToString();
                        }
                        dr = dt.NewRow();
                        dr["CUST_NO"] = this.CUST_NO.Text.Trim();
                        dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");
                        dr["capt_str"] = "5";
                        dr["build_sts"] = "1"; 
                    }
                    else
                        dr = dt.Rows[0];

                    if (strStatus == "Upd" && this.CUST_STS.Checked)
                    { 
                        
                        if (this.CUST_TYPE_CODE.SelectedValue != "" && this.CUST_UNION.SelectedValue.Trim() != "" &&
                            this.SALES_RGT_ADDR.Text.Trim() != "" && this.CTAC_ADDR.Text.Trim() != ""
                            && this.REAL_CAPT_AMT.Text.Trim() == ""&& this.IS_TRANSACTION.Text.Trim() != ""
                            && this.INVOICE.Text.Trim() != "")
                        {
                            this.CUST_STS.Checked=false;
                        }
                    }

                   
                    dr["CUST_SNAME"] = this.CUST_SNAME.Text.Trim();
                    dr["CUST_BLOC_CODE"] = this.CUST_BLOC_CODE.Text.Trim();
                    dr["CUST_TYPE_CODE"] = this.CUST_TYPE_CODE.SelectedValue.Trim();
                    dr["CUST_UNION"] = this.CUST_UNION.SelectedValue.Trim();
                    dr["NATIONALITY"] = this.NATIONALITY.SelectedValue.Trim();
                    dr["CUST_NAME"] = this.CUST_NAME.Text.Trim();
                    dr["UNIF_NO"] = this.UNIF_NO.Text.Trim();
                    dr["EN_NAME"] = this.EN_NAME.Text.Trim();
                    dr["EN_SNAME"] = this.EN_SNAME.Text.Trim();
                    dr["CONTACT"] = this.CONTACT.Text.Trim();
                    dr["CTAC_TITLE"] = this.CTAC_TITLE.Text.Trim();
                    dr["CTAC_EXT"] = this.CTAC_EXT.Text.Trim();
                    dr["STOCKCODE"] = this.STOCKCODE.Text.Trim();
                    dr["CONTACT2"] = this.CONTACT2.Text.Trim();
                    dr["CTAC_TITLE2"] = this.CTAC_TITLE2.Text.Trim();
                    dr["CTAC_EXT2"] = this.CTAC_EXT2.Text.Trim();
                    dr["TAKER"] = this.TAKER.Text.Trim();
                    dr["FLOT_DATE"] = this.FLOT_DATE.Text.Trim().Replace("/", "");
                    dr["BUILD_DATE"] = this.BUILD_DATE.Text.Trim().Replace("/", "");
                    dr["PHONE1"] = this.PHONE1.Text.Trim();
                    dr["PHONE2"] = this.PHONE2.Text.Trim();
                    dr["FACSIMILE"] = this.FACSIMILE.Text.Trim();
                    dr["EMP_PSNS"] = this.EMP_PSNS.Text.Trim();
                    dr["SALES_RGT_ADDR"] = this.SALES_RGT_ADDR.Text.Trim();
                    dr["RGT_CAPT_AMT"] = this.RGT_CAPT_AMT.Text.Trim();
                    dr["ZIP_CODE"] = this.ZIP_CODE.Text.Trim();
                    dr["RGT_ZIP_CODE"] = this.RGT_ZIP_CODE.Text.Trim();
                    dr["CTAC_ADDR"] = this.CTAC_ADDR.Text.Trim();
                    dr["REAL_CAPT_AMT"] = this.REAL_CAPT_AMT.Text.Trim();
                    dr["SPEC_COND"] = this.SPEC_COND.Text.Trim();
                    dr["IS_COND_AUTH"] = this.IS_COND_AUTH.Checked ? "Y" : "N";
                    dr["IS_BIZ_CUST"] = this.IS_BIZ_CUST.Checked ? "Y" : "N";
                    dr["HONEST_AGREEMENT"] = this.HONEST_AGREEMENT.Checked ? "Y" : "N";
                    dr["SECRET_PROMISE"] = this.SECRET_PROMISE.Checked ? "Y" : "N";
                   // dr["IS_TRANSACTION"] = this.IS_TRANSACTION.Checked ? "Y" : "N";
                    dr["CUST_STS"] = this.CUST_STS.Checked ? "Y" : "N";
                    dr["ORG_TYPE"] = this.ORG_TYPE.SelectedValue;
                    dr["INVOICE"] = this.INVOICE.SelectedValue.Trim();
                    dr["MAIN_BUS_ITEM"] = this.MAIN_BUS_ITEM.Text.Trim();
                    dr["PARENT_COMP_NAME"] = this.PARENT_COMP_NAME.Text.Trim();
                    dr["PARENT_COMP_STOCK_CODE"] = this.PARENT_COMP_STOCK_CODE.Text.Trim();
                    dr["BACKGROUND"] = this.BACKGROUND.Text.Trim();

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
                if (bolWF0101 || bolWE030)
                {
                    this.setScript("window.parent.setData('Custom','" + this.CUST_NO.Text.Trim() + "','" + this.CUST_NAME.Text.Trim() + "');");
                    //this.Master.Master.nowStatus="Add";
                    if (bolWF0101)
                    {
                        this.Master.setParms("Add");
                        this.setParms();
                        this.setProcessMessage("新增處理成功!", false);
                        return 0;
                    }

                    if (bolWE030)
                    {
                        this.setProcessMessage("修改處理成功!", false);
                        return 0;
                    }
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

                if (bolWE030)
                {
                    this.setProcessMessage("修改處理失敗!", false);
                    return 0;
                }

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


        protected void SetAddress(object sender, EventArgs e)
        {
            if (((DropDownList)sender).ID == "RGT_ZIP_CODE")
            {
                if (this.SALES_RGT_ADDR.Text == "" && this.RGT_CITY_CODE.SelectedValue != "" && this.RGT_ZIP_CODE.SelectedValue != "")
                    this.SALES_RGT_ADDR.Text = this.RGT_CITY_CODE.SelectedItem.Text.Trim() + this.RGT_ZIP_CODE.SelectedItem.Text.Trim().Substring(0,this.RGT_ZIP_CODE.SelectedItem.Text.Trim().Length-4);                
            }
            else
            {
                if (this.CTAC_ADDR.Text == "" && this.CITY_CODE.SelectedValue != "" && this.ZIP_CODE.SelectedValue != "")
                    this.CTAC_ADDR.Text = this.CITY_CODE.SelectedItem.Text.Trim() + this.ZIP_CODE.SelectedItem.Text.Trim().Substring(0, this.ZIP_CODE.SelectedItem.Text.Trim().Length - 4);
            }

        }

        protected void RGT_ZIP_CODE_LOAD(object sender, EventArgs e)
        {
            string strCITY = this.RGT_CITY_CODE.SelectedValue;

            this.hiddenRGT_CITY_CODE.Value = strCITY;

            this.RGT_ZIP_CODE.Items.Clear();
            dg.ListBinding(this.RGT_ZIP_CODE, "select '' ZIP_CODE,'請選擇..' ZIP_NAME union all  select ZIP_CODE,ZIP_NAME=zone_name+' '+zip_code from or3_zip where city_code='" + strCITY + "' and ZIP_CODE!='' order by zip_code ");

            this.SetAddress(this.RGT_ZIP_CODE, null);
        }

        protected void ZIP_CODE_LOAD(object sender, EventArgs e)
        {
            string strCITY = this.CITY_CODE.SelectedValue;

            this.hiddenCityCode.Value = strCITY;

            this.ZIP_CODE.Items.Clear();
            dg.ListBinding(this.ZIP_CODE, "select '' ZIP_CODE,'請選擇..' ZIP_NAME union all  select ZIP_CODE,ZIP_NAME=zone_name+' '+zip_code from or3_zip where city_code='" + strCITY + "' and ZIP_CODE!='' order by zip_code ");

            this.SetAddress(this.ZIP_CODE, null);
        }

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