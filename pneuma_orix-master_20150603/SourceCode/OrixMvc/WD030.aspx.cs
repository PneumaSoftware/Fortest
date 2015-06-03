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
using System.Data.OleDb;

namespace OrixMvc
{
    public partial class WD030 : OrixMvc.Pattern.PageParent
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


        protected string IS_TRANSFER
        {
            set { ViewState["IS_TRANSFER"] = value; }
            get { return (ViewState["IS_TRANSFER"] == null ? "" : (string)ViewState["IS_TRANSFER"]); }
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

             * 
            //3.雖有編輯功能, 但不顯示修改的欄位
            this.Master.bolUpd_Show = false;*/

           // this.Master.editSave = false;
            this.Master.bolSave = false;
            string strReturn = dg.GetDataRow("select top 1 IS_TRANSFER  from OR3_PAPER_USE_DTL_TMP")[0].ToString();

            if (strReturn == "N")
                this.IS_TRANSFER = "N";


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
            this.yearMonth.Text = System.DateTime.Now.ToString("yyyy/MM");

            
        }
        #endregion


       




        #region Save_Click：儲存鍵觸發
        /// <summary>
        /// 儲存鍵觸發Event,
        /// 本頁作業：無  
        /// </summary>
        private void Save_Click(string strType)
        {



            

        }
        #endregion


        /// <summary>
        /// 資料查詢
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Import_Click(object sender, EventArgs e)
        {

            try
            {
                if (this.yearMonth.Text.Trim() == "")
                {
                    this.setMessageBox("年月必須輸入!!");
                    return;
                }

                string strName = filePath.FileName;
                if (strName == "")
                {
                    this.setMessageBox("必須選擇匯入檔案來源!!");
                    return;
                }

                if (strName.ToLower().IndexOf(".xls") == -1)
                {
                    this.setMessageBox("必須是excel格式!");
                    return;
                }
                string strExtName = (strName.ToLower().IndexOf(".xlsx") != -1 ? "xlsx" : "xls");
                string strFile = AppDomain.CurrentDomain.BaseDirectory + "WD030." + strExtName;

                filePath.SaveAs(strFile);

           //     OR3_PAPER_USE_DTL  (供WD040修改&轉入)
//OR3_PAPER_USE_DTL_TMP  (EXCEL匯入)
//OR3_PAPER_USE_DTL_HIS   (轉OR3


                DataRow dr;
                DataTable dt = dts.GetTable("OR3_PAPER_USE_DTL_TMP","1=1");
              //  dt.DeleteRows("1=1");
                cts.Execute("Delete from OR3_PAPER_USE_DTL_TMP ");//2015/2/23 改全部清
                // DataTable dtF = dg.GetDataTable("select * from openrowset('Microsoft.Jet.OLEDB.4.0','EXCEL 8.0;HDR=YES;User id=admin;Password=;IMEX=1;DATABASE=" + strFile + "', [sheet1$])");

                OleDbCommand excelCommand = new OleDbCommand(); OleDbDataAdapter excelDataAdapter = new OleDbDataAdapter();

                // string excelConnStr = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + strFile + "; Extended Properties =Excel 8.0;";
                //  string excelConnStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strFile + ";Extended Properties=Excel 12.0 Xml;HDR=YES";
                string excelConnStr = "provider=Microsoft.ACE.OLEDB.12.0;data source=" + strFile + "; Extended Properties='Excel 12.0;HDR=YES'";
                OleDbConnection excelConn = new OleDbConnection(excelConnStr);

                excelConn.Open();

                DataTable dtF = new DataTable(); excelCommand = new OleDbCommand("SELECT * FROM [sheet1$]", excelConn);

                excelDataAdapter.SelectCommand = excelCommand;

                excelDataAdapter.Fill(dtF);
                excelConn.Close();
                excelConn.Dispose();

                int seq = dt.Compute("max(SEQ_NO)","").ToString().toInt();
                for (int i = 0; i < dtF.Rows.Count; i++)
                {

                    /*
                    歐力士合約編號(APLY_NO)
                    期數(PERIOD)
                    機器序號(MAC_NO)	
                    方案代號(CASE_NO)	
                    機器分期月費(未稅) (MONTH_FEE)
                    方案代號(CASE_NO)	
                    免費彩色列印張數(FREE_QTY_COLOR)	
                     * 免費黑白列印張數(FREE_QTY_MONO)	
                     *免費彩色Ａ３加價列印張數(FREE_QTY_A3_COLOR)	
                     *免費黑白Ａ３加價列印張數(FREE_QTY_A3_MONO)	
                     *實際彩色列印張數(USE_QTY_COLOR)	
                     *實際黑白列印張數(USE_QTY_MONO)	
                     *實際彩色Ａ３加價列印張數(USE_QTY_A3_COLOR)	
                     *實際黑白Ａ３加價列印張數(USE_QTY_A3_MONO)	                
                     *彩色本月積數(SUM_A4_COLOR)	
                     *彩色上月積數(SUM_A4_COLOR_LAST)	
                     *黑白本月積數(SUM_A4_MONO)	
                     *黑白上月積數(SUM_A4_MONO_LAST)
                     *彩色Ａ３本月積數(SUM_A3_COLOR)	
                     *	彩色Ａ３上月積數(SUM_A3_COLOR_LAST)	
                     *黑白Ａ３本月積數(SUM_A3_MONO)	
                     *黑白Ａ３上月積數(SUM_A3_MONO_LAST)
                     *彩色單張金額(ONE_PRICE_COLOR)	
                     *黑白單張金額(ONE_PRICE_MONO)	
                     *
                     *彩色Ａ３單張金額(ONE_PRICE_A3_COLOR)	
                     *黑白Ａ３單張金額(ONE_PRICE_A3_MONO)	
                     *彩色金額小計(SUBTOT_COLOR)	
                     *黑白金額小計(SUBTOT_MONO)	
                     *合計(TOTAL)	
                     *發票日期(INV_DATE)
                     *發票號碼(INVO_NO)
                     *金額(INV_AMT)
                     *發票稅額(INV_TAX)
                     *發票總額(CAPITAL)
                    */
                    if (dt.Select("APLY_NO='" + dtF.Rows[i]["歐力士合約編號"].ToString() + "' and isnull(Period,'0')=" + dtF.Rows[i]["期數"].ToString().toNumber() + " and MAC_NO=" + dtF.Rows[i]["機器序號"].ToString().toNumber() + " and YEAR_MONTH='" + System.DateTime.Now.ToString("yyyyMM") + "'").Length > 0)
                    {
                        DataRow drP = dt.Select("APLY_NO='" + dtF.Rows[i]["歐力士合約編號"].ToString() + "' and isnull(Period,'0')=" + dtF.Rows[i]["期數"].ToString().toNumber() + " and MAC_NO=" + dtF.Rows[i]["機器序號"].ToString().toNumber() + " and YEAR_MONTH='" + System.DateTime.Now.ToString("yyyyMM") + "'")[0];
                        drP["EXIST"] = "Y";
                        drP["MSG"] = "資料重覆";

                        continue;
                    }

                    try
                    {
                        

                        dr = dt.NewRow();

                        seq++;
                       
                        dr["APLY_NO"] = dtF.Rows[i]["歐力士合約編號"].ToString();
                        dr["PERIOD"] = dtF.Rows[i]["期數"].ToString();
                        dr["MAC_NO"] = dtF.Rows[i]["機器序號"].ToString();
                        dr["YEAR_MONTH"] = this.yearMonth.Text;
                        dr["STAR_DATE"] = "";
                        dr["PEND_DATE"] = "";
                        dr["UNIF_NO"] = "";
                        // dr["SPEC"]="";
                        dr["CASE_NO"] = dtF.Rows[i]["方案代號"].ToString();
                        dr["MONTH_FEE"] = dtF.Rows[i]["機器分期月費(未稅)"].ToString().toNumber();
                        dr["FREE_QTY_COLOR"] = dtF.Rows[i]["免費彩色列印張數"].ToString().toNumber();
                        dr["FREE_QTY_MONO"] = dtF.Rows[i]["免費黑白列印張數"].ToString().toNumber();
                        dr["FREE_QTY_A3_COLOR"] = dtF.Rows[i]["免費彩色Ａ３加價列印張數"].ToString().toNumber();
                        dr["FREE_QTY_A3_MONO"] = dtF.Rows[i]["免費黑白Ａ３加價列印張數"].ToString().toNumber();
                        dr["USE_QTY_COLOR"] = dtF.Rows[i]["實際彩色列印張數"].ToString().toNumber();
                        dr["USE_QTY_MONO"] = dtF.Rows[i]["實際黑白列印張數"].ToString().toNumber();
                        dr["USE_QTY_A3_COLOR"] = dtF.Rows[i]["實際彩色Ａ３加價列印張數"].ToString().toNumber();
                        dr["USE_QTY_A3_MONO"] = dtF.Rows[i]["實際黑白Ａ３加價列印張數"].ToString().toNumber();
                        dr["SUM_A4_COLOR"] = dtF.Rows[i]["彩色本月積數"].ToString().toNumber();
                        dr["SUM_A4_COLOR_LAST"] = dtF.Rows[i]["彩色上月積數"].ToString().toNumber();
                        dr["SUM_A4_MONO"] = dtF.Rows[i]["黑白本月積數"].ToString().toNumber();
                        dr["SUM_A4_MONO_LAST"] = dtF.Rows[i]["黑白上月積數"].ToString().toNumber();
                        dr["SUM_A3_COLOR"] = dtF.Rows[i]["彩色Ａ３本月積數"].ToString().toNumber();
                        dr["SUM_A3_COLOR_LAST"] = dtF.Rows[i]["彩色Ａ３上月積數"].ToString().toNumber();
                        dr["SUM_A3_MONO"] = dtF.Rows[i]["黑白Ａ３本月積數"].ToString().toNumber();
                        dr["SUM_A3_MONO_LAST"] = dtF.Rows[i]["黑白Ａ３上月積數"].ToString().toNumber();
                        dr["ONE_PRICE_COLOR"] = dtF.Rows[i]["彩色單張金額"].ToString().toNumber();
                        dr["ONE_PRICE_MONO"] = dtF.Rows[i]["黑白單張金額"].ToString().toNumber();
                        dr["ONE_PRICE_A3_COLOR"] = dtF.Rows[i]["彩色Ａ３單張金額"].ToString().toNumber();
                        dr["ONE_PRICE_A3_MONO"] = dtF.Rows[i]["黑白Ａ３單張金額"].ToString().toNumber();
                        dr["SUBTOT_COLOR"] = dtF.Rows[i]["彩色金額小計"].ToString().toNumber();
                        dr["SUBTOT_MONO"] = dtF.Rows[i]["黑白金額小計"].ToString().toNumber();
                        dr["TOTAL"] = dtF.Rows[i]["合計"].ToString().toNumber();
                        dr["INV_DATE"] = dtF.Rows[i]["發票日期"].ToString();
                        dr["INVO_NO"] = dtF.Rows[i]["發票號碼"].ToString();
                        dr["INV_AMT"] = dtF.Rows[i]["金額"].ToString().toNumber();
                        dr["INV_TAX"] = dtF.Rows[i]["發票稅額"].ToString().toNumber();
                        dr["CAPITAL"] = dtF.Rows[i]["發票總額"].ToString().toNumber();
                        dr["ORI_APLY_NO"] = dtF.Rows[i]["歐力士合約編號"].ToString();
                        dr["ORI_PERIOD"] = dtF.Rows[i]["期數"].ToString();
                        dr["XEROX_CON_NO"] = dtF.Rows[i]["全錄合約編號"].ToString();
                        dr["IS_TRANSFER"] = "N";
                        dr["IMPORT_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["SEQ_NO"] = (seq).ToString();
                        dr["ADD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["ADD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["ADD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");

                        dr["LAST_UPD_USER_ID"] = this.Master.Master.EmployeeName;
                        dr["LAST_UPD_DATE"] = System.DateTime.Now.ToString("yyyyMMdd");
                        dr["LAST_UPD_TIME"] = System.DateTime.Now.ToString("HH:mm:ss");

                        dt.Rows.Add(dr);
                    }
                    catch (Exception err)
                    {
                        this.setMessageBox("第" + (i + 1).ToString() + "資料匯入失敗, 請確認" + err.Message.Replace("'", "\""));
                        return;
                    }

                }


                if (dts.Save())
                {
                    this.setProcessMessage("資料匯入成功!!", false);
                    DataTable dtCHK = dg.GetDataTable("exec s_WD030_Excel_Check");
                    for (int i = 0; i < dtCHK.Rows.Count; i++)
                    {
                        this.Message.Text += dtCHK.Rows[i][0].ToString() + Environment.NewLine;
                    }


                    this.setScript("document.getElementById('divImport').style.display='none';\n document.getElementById('divResult').style.display='';");

                }
                else
                    this.setProcessMessage("資料匯入失敗!!", true);
            }
            catch (Exception ee)
            {
                this.setMessageBox("檔案匯入失敗, 請確認" + ee.Message.Replace("'", "\""));
                return;
            } 
        }

    }
}
