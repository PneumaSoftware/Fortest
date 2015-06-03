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
    public partial class WB040 : OrixMvc.Pattern.PageParent
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

        public String nowRow
        {
            set { ViewState["nowRow"] = value; }
            get { return (ViewState["nowRow"] == null ? "" : (String)ViewState["nowRow"]); }
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


            this.Master.DisplayEvent += new displayDelegate(Display_Command);

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


        protected void GridFunc_Click(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            DataTable dtGridSource;
            switch (((Button)sender).ID)
            {
                case "btnDetail":

                    this.nowRow = (((RepeaterItem)((Button)sender).Parent.Parent.Parent).ItemIndex + 1).ToString();
                    string strScript = "document.getElementById('tr" + this.nowRow + "').className='crow';";
                    this.setScript(strScript);

                    string strAPLY = e.CommandName;
                    string strSQL = "exec s_WB040_Grid2 ";
                    strSQL += " @PQUOTA_APLY_NO='" + e.CommandName.Split(',')[0].ToString() + "'";

                    DataTable dt = dg.GetDataTable(strSQL);
                    /*
                     * <td class="number"><%# Eval("UPPER_LIMIT", "{0:###,###,###,##0}")%></td>                   
                        <td><%# Eval("Aply_no")%></td>                   
                        <td class="number"><%# Eval("L_THIS", "{0:###,###,###,##0}")%></td>                        
                        <td class="number"><%# Eval("CON_SUR", "{0:###,###,###,##0}")%></td>                        
                        <td class="number"><%# Eval("UNUSE_QUTA", "{0:###,###,###,##0}")%></td>      */

                    string strRec = e.CommandName.Split(',')[1].ToString();
                    var query = from row in dt.AsEnumerable()
                                group row by row.Field<string>("CUST_NO") into grp
                                select new
                                {
                                    Id = grp.Key,
                                    sum1 = grp.Max(r => r.Field<Decimal>("UPPER_LIMIT")),
                                    sum2 = grp.Sum(r => r.Field<Decimal>("L_THIS")),
                                    sum3 = grp.Sum(r => r.Field<Decimal>("CON_SUR")),
                                    sum4 = grp.Max(r => r.Field<Decimal>("UPPER_LIMIT")) - grp.Sum(r => r.Field<Decimal>("L_THIS")),
                                    sum5 = grp.Max(r => r.Field<Decimal>("UPPER_LIMIT")) - grp.Sum(r => r.Field<Decimal>("CON_SUR"))

                                };
                    DataRow dr;
    //                [下午 03:35:24] Louis: 要判斷Grid 1的是否循環
//[下午 03:35:52] Alinta(亮晶晶): 公式在哪裏
                    //[下午 03:36:19] Louis: if 是否循環='N' 則=動用額度上限-本次申請[下午 03:36:58]
            //Louis: ='Y' 則=動用額度上限-契約餘額

                    foreach (var grp in query)
                    {
                        dr = dt.NewRow();
                        dr["CUST_NO"] = grp.Id;
                        dr["UPPER_LIMIT"] = grp.sum1;
                        dr["L_THIS"] = grp.sum2;
                        dr["CON_SUR"] = grp.sum3;
                        if (strRec=="N") 
                            dr["UNUSE_QUTA"] = grp.sum4;
                        else
                            dr["UNUSE_QUTA"] = grp.sum5;
                        dt.Rows.Add(dr);
                      //  Response.Write(String.Format("The Sum of '{0}' is {1}", grp.Id, grp.sum));
                    }

                    DataView dv = dt.DefaultView;
                    dv.Sort = "CUST_NO";
                    this.rptDetail.DataSource = dv ;
                    this.rptDetail.DataBind();
                    this.upGrid2.Update();

                    break;

                case "btbAdd":
                   
                   
                    break;
            }            
        }

        

      


        


        /// <summary>
        /// 資料查詢
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Display_Command()
        {


            string strMessage = "";
            if (PCUST_NO.Text.Trim() == "")
            {

                strMessage = "請輸入客戶代號！";
                this.setMessageBox(strMessage);
                return;

            }

           /* @PCUST_NO varchar(10)='',
	@PCUST_SNAME nvarchar(50)=''
           
              */
            string strSQL = "exec s_WB040_Grid ";
            strSQL += " @PCUST_NO='" + this.PCUST_NO.Text.rpsText() + "'";
            strSQL += ",@PCUST_SNAME='" + this.PCUST_SNAME.Text.rpsText() + "'";

            DataTable dt = dg.GetDataTable(strSQL);

            this.rptQuery.DataSource = dt;
            this.rptQuery.DataBind();
            //this.upGrid1.Update();

            this.rptDetail.DataSource = null;
            this.rptDetail.DataBind();
            this.upGrid2.Update();






            //  if (SelectEvent != null)
            //      SelectEvent();


        }

    }
}
