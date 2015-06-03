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
    public partial class content : System.Web.UI.MasterPage
    {

        #region 引用Module設定
        //*******************begin Module***********************  
        /// <summary>
        /// dss：將dataset存入sql server
        /// cts：將sql command 存入 sql server
        /// dg ：取得sql server的資料
        /// </summary>

        public VS2008.Module.DataSetToSql dss = new VS2008.Module.DataSetToSql();
        public VS2008.Module.CommandToSql cts = new VS2008.Module.CommandToSql();
        public VS2008.Module.DataGetting dg = new VS2008.Module.DataGetting();

        //**********************end Module***********************   
        #endregion

        /// <summary>
        /// 取得Program Id
        /// </summary>
        /// <returns></returns>
        public string ProgramId
        {
            set { ViewState["ProgramId"] = value; }
            get
            {
                if (ViewState["ProgramId"] == null)
                    setParms();

                return (ViewState["ProgramId"] == null ? "" : (String)ViewState["ProgramId"]);
            }
        }


        /// <summary>
        /// 取得Page Id
        /// </summary>
        /// <returns></returns>
        public string pageID
        {
            set { ViewState["pageID"] = value; }
            get
            {
                if (ViewState["pageID"] == null)
                    setParms();

                return (ViewState["pageID"] == null ? "" : (String)ViewState["pageID"]);
            }
        }


        /// <summary>
        /// 取得Program Name
        /// </summary>
        /// <returns></returns>
        public string ProgramName
        {
            set { ViewState["ProgramName"] = value; }
            get
            {
                if (ViewState["ProgramName"] == null)
                    setParms();

                return (ViewState["ProgramName"] == null ? "" : (String)ViewState["ProgramName"]);
            }
        }


        /// <summary>
        /// 取得sub Program Id
        /// </summary>
        /// <returns></returns>
        public string subProgramId
        {
            set { ViewState["subProgramId"] = value; }
            get
            {
                return (ViewState["subProgramId"] == null ? "" : (String)ViewState["subProgramId"]);
            }
        }

        /// <summary>
        /// 取得sub Program Name
        /// </summary>
        /// <returns></returns>
        public string subProgramName
        {
            set { ViewState["subProgramName"] = value; }
            get
            {
                return (ViewState["subProgramName"] == null ? "" : (String)ViewState["subProgramName"]);
            }
        }



            /// <summary>
        /// 取得corp Acct
        /// </summary>
        /// <returns></returns>
        public string CorpAcct
        {
            set { ViewState["CorpAcct"] = value; }
            get
            {
                if (ViewState["CorpAcct"] == null)
                    setParms();

                return (ViewState["CorpAcct"] == null ? "" : (String)ViewState["CorpAcct"]);
            }
        }


        /// <summary>
        /// 取得Employee Id
        /// </summary>
        /// <returns></returns>
        public string EmployeeId
        {
            set { ViewState["EmployeeId"] = value; }
            get
            {
                if (ViewState["EmployeeId"] == null)
                    setParms();

                return (ViewState["EmployeeId"] == null ? "" : (String)ViewState["EmployeeId"]);
            }
        }

        /// <summary>
        /// 取得Employee Name
        /// </summary>
        /// <returns></returns>
        public string EmployeeName
        {
            set { ViewState["EmployeeName"] = value; }           

            get { 
                if (ViewState["EmployeeName"] == null)
                    setParms();

                return (ViewState["EmployeeName"] == null ? "" : (String)ViewState["EmployeeName"]); 
            }

        }

        /// <summary>
        /// 取得USER Id
        /// </summary>
        /// <returns></returns>
        public string UserId
        {
            set { ViewState["UserId"] = value; }
            get
            {
                if (ViewState["UserId"] == null)
                    setParms();

                return (ViewState["UserId"] == null ? "" : (String)ViewState["UserId"]);
            }
        }

    

        public string btnENABLE
        {
            set { ViewState["btnENABLE"] = value; }
            get
            {
                if (ViewState["btnENABLE"] == null)
                    setParms();

                return (ViewState["btnENABLE"] == null ? "" : (String)ViewState["btnENABLE"]);
            }
        }

        /// <summary>
        /// 作業狀態 Add:新增,Query:查詢,Upd:修改,Del:刪除
        /// </summary>               
        public string nowStatus
        {
            set { ViewState["nowStatus"] = value; }
            get { 
                if (ViewState["nowStatus"] == null)
                    setParms();

                return (ViewState["nowStatus"] == null ? "Query" : (String)ViewState["nowStatus"]); 
            }

        }

        public string nowStatusName
        {
            set { ViewState["nowStatusName"] = value; }
            get
            {
                if (ViewState["nowStatusName"] == null)
                    setParms();

                return (ViewState["nowStatusName"] == null ? "查詢" : (String)ViewState["nowStatusName"]);
            }

        }

        /// <summary>
        /// 是否顯示程式代號及名稱
        /// </summary>               
        public bool bolShowProgram
        {
            set { ViewState["bolShowProgram"] = value; }
            get
            { 
                return (ViewState["bolShowProgram"] == null ? false : (bool)ViewState["bolShowProgram"]);
            }

        }

        public string hint
        {
            set { ViewState["hint"] = value; }
            get
            {
                return (ViewState["hint"] == null ? "" : (string)ViewState["hint"]);
            }
        }

        public string hintmemo
        {
            set { ViewState["hintmemo"] = value; }
            get
            {
                return (ViewState["hintmemo"] == null ? "" : (string)ViewState["hintmemo"]);
            }
        }

        public string bolHint
        {
            set { ViewState["bolHint"] = value; }
            get
            {
                return (ViewState["bolHint"] == null ? "" : (string)ViewState["bolHint"]);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void toExcel(System.Object obj, EventArgs err)
        {
            DataGetting dg = new VS2008.Module.DataGetting();
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            string qryString = (String)Session["qryString"];

            DataTable dt = dg.GetDataTable(qryString);

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

        public void Page_Load(object sender, EventArgs e)
        {
            this.setParms();
        }


        /// <summary>
        /// 取得任何在頁面上的物件
        /// </summary>
        /// <param name="strID">物件ID</param>
        /// <returns>物件本身</returns>
        public object masterFindControl(string strID)
        {
            object obj ;

            if (this.body.FindControl("searchArea")!=null)
            {
                obj = this.body.FindControl("searchArea").FindControl(strID);
                if (obj != null)
                    return obj;
            }

            if (this.body.FindControl("editingArea") != null)
            {
                obj = this.body.FindControl("editingArea").FindControl(strID);
                if (obj != null)
                    return obj;
            }

            if (this.body.FindControl("masterFormView") != null)
            {
                if (this.body.FindControl("masterFormView").FindControl("editingArea") != null)
                {
                    obj = this.body.FindControl("masterFormView").FindControl("editingArea").FindControl(strID);
                    if (obj != null)
                        return obj;
                }
            }
            
            obj = this.PopWindow.FindControl(strID);
            if (obj != null)
                return obj;


            return null;
        }


        private void setParms(){

            string strSQL = "";
            if (!IsPostBack)
            {
                if (Session["CorpAcct"] == null | Session["EmployeeId"] == null || Session["EmployeeName"] == null || Session["ProgramId"] == null || Session["ProgramName"] == null)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "rtnClose", "window.parent.location.replace('login.aspx');", true);
                    return;
                   // this.Page.Response.Redirect("error.aspx");
                }
                else
                {
                    //判斷有無程式權限
                    string strFuncId = Path.GetFileName(Request.Path).Substring(0, 5);

                    string strMenu = "select UF.FUNC_ID from OR3_USER_FUNC as UF inner join " +
                                    "(select UF.FUNC_ID, MAX(case UF.USER_ID when '" + ((String)Session["UserId"]).Trim() + "' then 1 else 0 end) as F2 from OR3_USER_FUNC as UF " +
                                    "inner join (select USER_ID,GROUP_ID from OR3_USERS where USER_ID='" + ((String)Session["UserId"]).Trim() + "') as U " +
                                    "on UF.USER_ID in (U.USER_ID,U.GROUP_ID) " +
                                    "group by UF.FUNC_ID) as SUB " +
                                    "on UF.FUNC_ID=SUB.FUNC_ID and ((SUB.F2=1 and UF.USER_ID='" + ((String)Session["UserId"]).Trim() + "') or (SUB.F2=0 and UF.USER_ID=(select GROUP_ID from OR3_USERS where USER_ID='" + ((String)Session["UserId"]).Trim() + "')))" +
                                    "inner join OR3_FUNCTION as F on UF.FUNC_ID=F.FUNC_ID " +
                                    "where F.FUNC_TYPE='P' and UF.FUNC_ID = '" + strFuncId + "'";
                    DataRow dr = dg.GetDataRow(strMenu);

                    if (dr == null || dr[0].ToString() == "")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "rtnClose", "window.parent.location.replace('NoAuthority.aspx');", true);
                        return;
                    }

                    this.CorpAcct = ((String)Session["CorpAcct"]).Trim();
                    this.EmployeeId = ((String)Session["EmployeeId"]).Trim();
                    this.UserId = ((String)Session["UserId"]).Trim();
                    this.EmployeeName = ((String)Session["EmployeeName"]).Trim();
                    this.ProgramId = ((String)Session["ProgramId"]).Trim();
                    this.ProgramName = ((String)Session["ProgramName"]).Trim();
                    if (Session["nowStatus"] != null)
                        this.nowStatus = ((String)Session["nowStatus"]).Trim();
                    else
                        this.nowStatus = "Query";

                    if (Session["nowStatusName"] != null)
                        this.nowStatusName = ((String)Session["nowStatusName"]).Trim();
                    else
                        this.nowStatusName = "查詢";
                }

                string strID= Path.GetFileName(Request.Path).Substring(0,5);
                this.pageID = Path.GetFileName(Request.Path).Substring(0, 6).Replace(".","");
                if (strID.ToUpper() != this.ProgramId.ToUpper())
                {
                    DataRow dr = dg.GetDataRow("select FUNC_ID,FUNC_NAME from OR3_FUNCTION where FUNC_ID='" + strID + "'");
                    this.subProgramId = dr[0].ToString();
                    this.subProgramName = dr[1].ToString();
                    this.bolShowProgram = true;
                }

                //設定權限
                 strSQL = " select ";
                strSQL += " (case when CAN_ADD='0' then '[id*=btnAdd],' else '' end)+";
                strSQL += " (case when CAN_UPDATE='0' then '[id*=btnUpd],' else '' end)+";
                strSQL += " (case when CAN_DELETE='0' then '[id*=btnDel],' else '' end)+";
                strSQL += " (case when CAN_EXPORT='0' then '[id*=excel],' else '' end)+";
                strSQL += " (case when CAN_COPY='0' then '[id*=btnCopy],' else '' end)+";
                strSQL += " isnull((select '[id*='+ CONTROL_ID +']'+',' from OR3_FUNC_BTN a inner join or3_user_func_Btn b on a.FUNC_ID=b.FUNC_ID and a.SEQ_NO=b.SEQ_NO";
                strSQL += " where b.ENABLE=0 and b.USER_ID=c.USER_ID and b.FUNC_ID=c.FUNC_ID for xml path('')),'')  as control";
                strSQL += " from or3_user_func c where USER_ID='" + this.UserId.Trim() + "' and FUNC_ID='" + this.ProgramId + "'";
                this.btnENABLE = dg.GetDataRow(strSQL)[0].ToString();
                this.btnENABLE=this.btnENABLE+"[id*=xxxxxx]";
               

                


            }
            else
            {
                Session["CorpAcct"] = this.CorpAcct;
                Session["EmployeeId"] = this.EmployeeId;
                Session["EmployeeName"] = this.EmployeeName;
                Session["ProgramId"] = this.ProgramId;
                Session["pageID"] = this.pageID;
                Session["ProgramName"] = this.ProgramName;
                Session["nowStatus"] = this.nowStatus;
                Session["nowStatusName"] = this.nowStatusName;
                Session["UserId"] = this.UserId;
                

            }
            // &#13;
            //設定hint
            string strScript = "";
            strSQL = "select hint,ctrl_id from or3_hint where AVAILABLE=1 and func_id='" + this.ProgramId + "' and prg_id='" + this.pageID + "'";
            DataTable dt = dg.GetDataTable(strSQL);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                strScript += "document.getElementById('" + dt.Rows[i]["ctrl_id"].ToString().Trim() + "').setAttribute('title','" + dt.Rows[i]["hint"].ToString().Trim().Replace("\r\n", "\\r\\n") + "') ;\n";
                strScript += "document.getElementById('" + dt.Rows[i]["ctrl_id"].ToString().Trim() + "').className=document.getElementById('" + dt.Rows[i]["ctrl_id"].ToString().Trim() + "').className+' hint';\n";
            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setHint", strScript, true);
            this.bolHint = dg.GetDataRow("select hint from OR3_USERS where USER_ID='" + this.UserId + "'")[0].ToString();

            //設定help文件
            string[] files = System.IO.Directory.GetFiles(Request.PhysicalApplicationPath + "helps\\", this.ProgramId + ".*");
            string strFile = "";
            if (files.Length > 0)
            {
                strFile = files[0].ToString().ToLower().Replace((Request.PhysicalApplicationPath + "helps\\").ToLower(), "");

            }
            strScript = "try{window.parent.setHELP('" + strFile + "');}\n";
            strScript += "catch(er){window.parent.parent.setHELP('" + strFile + "');}\n";
            strScript += "finally{}\n";


            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setHelp", strScript, true); 

            
        }
    }
}
