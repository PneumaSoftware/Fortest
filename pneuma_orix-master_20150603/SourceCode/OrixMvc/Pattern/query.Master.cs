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
using VS2008.Module;
using System.IO;

namespace OrixMvc.Pattern
{

    /// <summary>
    /// Alinta：create delegate for functionbar events
    /// </summary>   
    /// <param name="strStatus">作業狀態</param>    
    public delegate void StatusDelegate(string strStatus);


    /// <summary>
    /// Alinta：create delegate for QueryArea's command  events
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void QueryDelegate(object sender, System.Web.UI.WebControls.RepeaterCommandEventArgs e);


    /// <summary>
    /// Alinta：資料選取後
    /// </summary>       
    public delegate void SelectDelegate();


    public partial class query : System.Web.UI.MasterPage
    {


        /// <summary>
        /// 將MasterPage的Status event傳回主網頁
        /// </summary>
        public event StatusDelegate StatusEvent;

        /// <summary>
        /// 將MasterPage的Select event傳回主網頁
        /// </summary>
        public event SelectDelegate SelectEvent;


        /// <summary>
        /// 將MasterPage的Query event傳回主網頁:(不是按下修改及刪除時)
        /// </summary>
        public event QueryDelegate QueryEvent;


        /// <summary>
        /// 取得detail page
        /// </summary>
        /// <returns></returns>
        public string pageDetail
        {
            set { ViewState["pageDetail"] = value; }
            get { return (ViewState["pageDetail"] == null ? "" : (String)ViewState["pageDetail"]); }
        }

        /// <summary>
        /// 取得query String
        /// </summary>
        /// <returns></returns>
        public string queryString
        {
            set { ViewState["queryString"] = value; }
            get { return (ViewState["queryString"] == null ? "" : (String)ViewState["queryString"]); }
        }

        /// <summary>
        /// 取得detail query String
        /// </summary>
        /// <returns></returns>
        public string dqueryString
        {
            set { ViewState["dqueryString"] = value; }
            get { return (ViewState["dqueryString"] == null ? "" : (String)ViewState["dqueryString"]); }
        }

        /// <summary>
        /// 取得query Sort field
        /// </summary>
        /// <returns></returns>
        public string querySort
        {
            set { ViewState["querySort"] = value; }
            get { return (ViewState["querySort"] == null ? "" : (String)ViewState["querySort"]); }
        }


        protected DataTable queryTable
        {
            set { ViewState["queryTable"] = value; }
            get { return (ViewState["queryTable"] == null ? null : (DataTable)ViewState["queryTable"]); }
        }
        
        
        protected int rowStart
        {
            set { ViewState["rowStart"] = value; }
            get { return (ViewState["rowStart"] == null ? 0 : (int)ViewState["rowStart"]); }
        }


        protected int rowEnd
        {
            set { ViewState["rowEnd"] = value; }
            get { return (ViewState["rowEnd"] == null ? 0 : (int)ViewState["rowEnd"]); }
        }

        protected int rowCount
        {
            set { ViewState["rowCount"] = value; }
            get { return (ViewState["rowCount"] == null ? 0 : (int)ViewState["rowCount"]); }
        }

        protected int lastPage
        {
            set { ViewState["lastPage"] = value; }
            get { return (ViewState["lastPage"] == null ? 0 : (int)ViewState["lastPage"]); }
        }

        public int GridIndex
        {
            set { ViewState["rowCount"] = value; }
            get { return (ViewState["rowCount"] == null ? 0 : (int)ViewState["rowCount"]); }
        }

        public bool bolAdd
        {
            set { ViewState["bolAdd"] = value; }
            get { return (ViewState["bolAdd"] == null ? true : (bool)ViewState["bolAdd"]); }
        }

        public bool bolDetail
        {
            set { ViewState["bolDetail"] = value; }
            get { return (ViewState["bolDetail"] == null ? false : (bool)ViewState["bolDetail"]); }
        }

        public bool bolUpd
        {
            set { ViewState["bolUpd"] = value; }
            get { return (ViewState["bolUpd"] == null ? true : (bool)ViewState["bolUpd"]); }
        }

        public bool bolCopy
        {
            set { ViewState["bolCopy"] = value; }
            get { return (ViewState["bolCopy"] == null ? true : (bool)ViewState["bolCopy"]); }
        }

        public bool bolDel
        {
            set { ViewState["bolDel"] = value; }
            get { return (ViewState["bolDel"] == null ? true : (bool)ViewState["bolDel"]); }
        }

        public bool bolGrid
        {
            set { ViewState["bolGrid"] = value; }
            get { return (ViewState["bolGrid"] == null ? true : (bool)ViewState["bolGrid"]); }
        }

        public String txtUpd
        {
            set { ViewState["txtUpd"] = value; }
            get { return (ViewState["txtUpd"] == null ? "修改" : (String)ViewState["txtUpd"]); }
        }

        public bool bolQuery
        {
            set { ViewState["bolQuery"] = value; }
            get { return (ViewState["bolQuery"] == null ? false : (bool)ViewState["bolQuery"]); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                this.setParms();

        }

        public int nowPage()
        {
            return this.goPage.Text.toInt();
        }
        private void setParms()
        {
            this.first.Enabled = false;
            this.prev.Enabled = false;
            this.next.Enabled = false;
            this.last.Enabled = false;
            this.excel.Enabled = false;
            
        }

        public void setSqlQuery(int intPage)
        {

            if (!this.bolGrid) //無Grid, 無binding
                return;


            DataGetting dg = new VS2008.Module.DataGetting();
            int cnt = this.pageCount.SelectedValue.toInt();
            int start = (cnt * (intPage - 1)) + 1;
            int end = start + cnt - 1;

            string strSQL = "";
            if (queryString.ToLower().IndexOf("exec ") != -1)
            {

                string queryString1 = queryString.Replace("'", "''");
                string server = ConfigurationManager.AppSettings["ServerName"].ToString();   
                strSQL += "  declare @cnt int ";
                strSQL += "select @cnt=COUNT(*) from ";
                strSQL += " ( select * from openQuery(" + server + ",";
                strSQL += "'SET FMTONLY OFF;" + queryString1 + "'";
                strSQL += " )) S  ";
                strSQL += " select @CNT cnt,* from ( select ROW_NUMBER() OVER(ORDER BY " + this.querySort + " ) AS Row,* FROM ";
                strSQL += " ( select * from openQuery(" + server + ",";
                strSQL += "'SET FMTONLY OFF;" + queryString1 + "'";
                strSQL += " )) Ss ) sss where row between " + start.ToString() + " and " + end.ToString();
            }
            else
            {
                strSQL += "  declare @cnt int ";
                strSQL += "select @cnt=COUNT(*) from ";
                strSQL += " ( ";
                strSQL += queryString;
                strSQL += " ) S  ";
                strSQL += " select @CNT cnt,* from ( select ROW_NUMBER() OVER(ORDER BY " + this.querySort + " ) AS Row,* FROM (";
                strSQL += queryString;
                strSQL += " ) Ss ) sss where row between " + start.ToString() + " and " + end.ToString();
            }

            
            this.queryTable = dg.GetDataTable(strSQL);


            this.goPage.Text = intPage.ToString();
            this.rowStart = start;
            this.rowEnd = end;

            if (this.queryTable.Rows.Count > 0)
                this.rowCount = this.queryTable.Rows[0]["cnt"].ToString().toInt();

            this.rptQuery.DataSource = this.queryTable;
            this.rptQuery.DataBind();

            lastPage = (this.rowCount / this.pageCount.SelectedValue.toInt()) + ((this.rowCount % this.pageCount.SelectedValue.toInt()) > 0 ? 1 : 0);
            this.first.Enabled = rowEnd > 0 && intPage != 1;
            this.prev.Enabled = rowEnd > 0 && intPage != 1;
            this.next.Enabled = rowEnd > 0 && intPage != lastPage;
            this.last.Enabled = rowEnd > 0 && intPage != lastPage;
            this.excel.Enabled = rowEnd > 0;

            this.bolQuery = true;

            // ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setCol", "setColumn()", true); 
        }


        protected void Query_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //dataitem is supposed to be a string object, so you can cast it to string, no need to call ToString()
            DataRowView item = (DataRowView)e.Item.DataItem;
            if (item.Row.Table.Columns["btnUpd"] != null)
                ((Button)e.Item.FindControl("btnUpd")).Visible = (Boolean)item["btnUpd"];

            if (item.Row.Table.Columns["btnDel"] != null)
                ((Button)e.Item.FindControl("btnDel")).Visible = (Boolean)item["btnDel"];
            
            // find the label with "lbl" ID, use e.Item as the Naming Container
           
        }

        /// <summary>
        /// 任何會變更頁次的元件觸發時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void changePage(object sender, EventArgs e)
        {

            if (rowEnd == 0)
                return;

            string strID = ((WebControl)sender).ID;
            int cnt = nowPage();
            lastPage=(this.rowCount / this.pageCount.SelectedValue.toInt()) + ((this.rowCount % this.pageCount.SelectedValue.toInt()) > 0 ? 1 : 0);

           

            switch (strID)
            {
                case "pageCount":
                    cnt = 1;                    
                    break;
                case "goPage":
                    if (cnt < 1 || cnt > lastPage)
                    {
                        string strScript = "";

                        strScript += "alert(\"您所選取的頁次不存在!!\"); \n ";
                        strScript += "documnet.getElementById('"+ this.goPage.ClientID+"').focus();";
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "pageError", strScript, true);
                        return;
                    }
                    break;
                case "first":
                    cnt = 1;                   
                    break;

                case "last":

                    cnt =lastPage ;                   
                    break;

                case "next":
                    cnt++;                  
                    break;

                case "prev":
                    cnt--;
                   
                    break;
            }

         

            this.goPage.Text=cnt.ToString();
            this.setSqlQuery(cnt);

        }

    

        /// <summary>
        /// 
        /// </summary>
        public void toExcel(System.Object obj, EventArgs err)
        {
            DataGetting dg = new VS2008.Module.DataGetting();
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            DataTable dt = dg.GetDataTable(this.queryString);

            string[] aryCol = this.qryColumn.Text.Split(',');
            for (int i = 0; i < aryCol.Length; i++)
            {
                if (aryCol[i].ToString().Trim() != "")
                {
                    //  dt.Columns[i].ColumnName = aryCol[i].ToString();
                    BoundField bfield = new BoundField();
                    //Initalize the DataField value.

                    bfield.DataField = dt.Columns[i].ColumnName;
                    //Initialize the HeaderText field value.
                    bfield.HeaderText = aryCol[i].ToString();
                    //Add the newly created bound field to the GridView.
                    //bfield.ControlStyle..AddAttributesToRender("vnd.ms-excel.numberformat:@");
                    dgExcel.Columns.Add(bfield);

                }
            }

            dgExcel.DataSource = dt;
            dgExcel.DataBind();

            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=FileName.xls");

            Response.Write("<meta http-equiv=Content-Type content=text/html;charset=utf-8>");

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Response.ContentType = "application/vnd.ms-excel";

            System.IO.StringWriter stringWrite = new System.IO.StringWriter();

            Response.Write("<style>");

            Response.Write("td{mso-number-format:\"\\@\";}");
            Response.Write("</style>");


            System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);

            dgExcel.RenderControl(htmlWrite);


            Response.Write(stringWrite.ToString());

            Response.End();


        }


        /// <summary>
        /// 取得查詢區域的物件
        /// </summary>
        /// <param name="strID"></param>
        /// <returns></returns>
        public Control masterRepeater(string strID)
        {
            return (Control)this.rptQuery.Items[this.GridIndex].FindControl("queryBody").FindControl(strID);
        }

        protected void Grid_Click(object sender, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {

            this.GridIndex = ((RepeaterItem)e.Item).ItemIndex;                
            switch (e.CommandName)
            {                
              /*  case "Upd":
                case "Del":
                case "Copy":
                case "Detail":

                    this.Master.nowStatus = e.CommandName;
                    Session["nowStatus"] = e.CommandName;

                    if (StatusEvent != null)
                        StatusEvent(this.Master.nowStatus);

                    this.toPage();

                    break;
                */
              

                default :
                    this.Master.nowStatus = e.CommandName;
                    Session["nowStatus"] = e.CommandName;
                    this.Master.nowStatusName = ((System.Web.UI.WebControls.Button)(e.CommandSource)).Text;
                    Session["nowStatusName"] = this.Master.nowStatusName;
                  //  Session["nowStatusnName"] = ((System.Web.UI.WebControls.Button)(sender)).Text;

                    if (StatusEvent != null)
                        StatusEvent(this.Master.nowStatus);

                    if (e.CommandName != "print" && this.Master.nowStatus!="")
                        this.toPage();

                    // if (QueryEvent != null)
                        //QueryEvent(sender,e);
                                        
                    break;

            }
        }


        /// <summary>
        /// 資料查詢
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Status_Click(object sender,CommandEventArgs  e)
        {

           
            this.Master.nowStatus = e.CommandName;
            Session["nowStatus"] = e.CommandName;
            try
            {
                Session["nowStatusName"] = ((System.Web.UI.WebControls.Button)(sender)).Text;
            }
            catch { Session["nowStatusName"] = "新增"; }
            if (StatusEvent != null)
                StatusEvent(this.Master.nowStatus);
                        
            switch (e.CommandName)
            {
                case "Query":
                    
                    break;
                
                case "Add":                   
                    

                    this.toPage();    
                    break;
            
                default:
                    if (this.Master.nowStatus == "Add" || this.Master.nowStatus == "Upd")
                    {
                        this.toPage();
                        Session["nowStatus"] = this.Master.nowStatus;            
                    } 
                    break;
            }

            
            //if (e.CommandName!="Query")
                


            if (SelectEvent != null)
                SelectEvent();


        }

        /// <summary>
        /// 前往編輯頁
        /// </summary>
        private void toPage()
        {
            string strScript = "";
            if (this.pageDetail.ToLower().IndexOf("wa0601") != -1 || this.pageDetail.ToLower().IndexOf("wb0101") != -1 || this.pageDetail.ToLower().IndexOf("wa1301") != -1)
            {
               strScript=" window.parent.document.getElementById('frameContent').style.display = 'none';\n";
                strScript+=" window.parent.document.getElementById('frameDetail').style.display = '';";
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "pageChange", strScript, true);
            }
            else
            {
                Session["dqueryString"] = this.dqueryString;
                strScript = "pageDetail = \"" + this.pageDetail + "\";\ncontentChange('frameDetail');";
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "pageChange", strScript, true);
            }
        }


        /// <summary>
        /// 取得master page上任何在頁面的物件
        /// </summary>
        /// <param name="strID">物件ID</param>
        /// <returns>物件本身</returns>
        public object masterFindControl(string strID)
        {
            return this.masterFindControl(strID);
        }



    }
}
