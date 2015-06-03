using System;
using System.Web.UI.WebControls;
using System.Web.UI;


namespace OrixMvc.ocxControl
{
    /// <summary>
    /// class library
    /// Author      ：  Alinta 
    /// Build Date  ：  2011/10/01 
    /// Modify Date ：  2012/05/03 by Alinta 
    /// Purpose     ：  Date's User Control
    /// </summary>  
    public partial class ocxDialog : System.Web.UI.UserControl
    {

        public override string ClientID
        {

            get
            {
                return this.txtDialog.ClientID;
            }
        }

        /// <summary>
        /// width 
        /// </summary>
        public string width
        {
            set { ViewState["width"] = value; }
            get
            {
                return (ViewState["width"] == null ? "" : (string)ViewState["width"]);
            }
        }


        /// <summary>
        /// ControlID ","區分 
        /// </summary>
        public string ControlID
        {
            set { ViewState["ControlID"] = value; }
            get
            {
                return (ViewState["ControlID"] == null ? "" : (string)ViewState["ControlID"]);
            }
        }

        /// <summary>
        /// FieldName ","區分 
        /// </summary>
        public string FieldName
        {
            set { ViewState["FieldName"] = value; }
            get
            {
                return (ViewState["FieldName"] == null ? "" : (string)ViewState["FieldName"]);
            }
        }

        /// <summary>
        /// check, 是否顯示輸入欄位
        /// </summary>
        public bool bolVisibleField
        {

            set { ViewState["bolVisibleField"] = value; }
            get
            {
                return (ViewState["bolVisibleField"] == null ? true : (bool)ViewState["bolVisibleField"]);
            }

        }

        /// <summary>
        /// check, 編輯是否開放
        /// </summary>
        public bool bolLock()
        {

            return (txtDialog.CssClass.IndexOf("slock") != -1);
                
        }

        /// <summary>
        /// check, 是否驗證 (defaule true)
        /// </summary>
        public bool bolCheck
        {
            set { ViewState["bolCheck"] = value; }
            get
            {
                return (ViewState["bolCheck"] == null ? true : (bool)ViewState["bolCheck"]);
            }
        }


        /// <summary>
        /// table 來源
        /// 
        /// </summary>
        public string SourceName
        {
            set { ViewState["SourceName"] = value; }
            get
            {
                return (ViewState["SourceName"] == null ? "" : (string)ViewState["SourceName"]);
            }
        }


        /// <summary>
        /// scriptSetValue 來源
        /// 
        /// </summary>
        public string scriptSetValue
        {
            set { ViewState["scriptSetValue"] = value; }
            get
            {
                return (ViewState["scriptSetValue"] == null ? "" : (string)ViewState["scriptSetValue"]);
            }
        }

        /// <summary>
        /// scriptSetValue 來源
        /// 
        /// </summary>
        public string scriptSetValueNull
        {
            set { ViewState["scriptSetValueNull"] = value; }
            get
            {
                return (ViewState["scriptSetValueNull"] == null ? "" : (string)ViewState["scriptSetValueNull"]);
            }
        }

        

        
        /// <summary>
        /// 顯示內容
        /// </summary>
        public String Text
        {
            set
            {                
                this.txtDialog.Text = value;
            }
            get
            {
                if (this.txtDialog.Text == "")
                    return "";
                else
                    return txtDialog.Text.Replace("_", "").ToString();
            }
        }

        protected string getControls()
        {
            string[] aryControls = ControlID.Split(',');
            string strID = "";

            for (int i = 0; i < aryControls.Length; i++)
            {
                if (strID != "")
                    strID += ",";
                WebControl wb = ((WebControl)(((OrixMvc.Pattern.content)this.Page.Master.Master).masterFindControl(aryControls[i].ToString())));
                if (wb != null)
                    strID += wb.ClientID;
            }

            return strID;
        }

        /// <summary>
        /// 網頁初始設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //string strID = ((WebControl)(((OrixMvc.Pattern.content)this.Page.Master.Master).masterFindControl("BLOC_NAME"))).ClientID;

           // ((OrixMvc.Pattern.content)this.Page.Master.Master).masterFindControl("CUST_NO")
          //  return;
            string strScript = "";
            string strScript1 = "";

          //  if (this.Request.Form[this.txtDialog.ClientID + "val"]!=null)
           //     this.Text = this.Request.Form[this.txtDialog.ClientID + "val"].ToString().Trim();

         //   if (!IsPostBack)
        //    {
                string[] aryControls = ControlID.Split(',');
                string[] aryFields=FieldName.Split(',');
                string strID = "";

                strScript += "document.getElementById('" + txtDialog.ClientID + "').value=r.KEY_NO;\n";
                strScript1 += "document.getElementById('" + txtDialog.ClientID + "').value='';\n";

                for (int i = 0; i < aryFields.Length; i++)
                {

                    if (aryFields[i] == "")
                        break;
                    string myRpt = "";
                    string myID = "";
                    
                    if (ControlID!="")
                        myID=aryControls[i].ToString().Trim();

                    if (myID=="")
                        myID = txtDialog.ClientID.Replace(this.ID, aryFields[i].ToString()).Replace("_"+txtDialog.ID,"");

                    if (myID.IndexOf('.') != -1)
                    {
                        myRpt = myID.Split('.')[0].ToString().Trim();
                        myID = myID.Split('.')[1].ToString().Trim();
                    }

                    WebControl wb=null;
                    if (myRpt == "")
                        wb = ((WebControl)(((OrixMvc.Pattern.content)this.Page.Master.Master).masterFindControl(myID)));
                    else
                    {
                        Repeater rpt = ((Repeater)(((OrixMvc.Pattern.content)this.Page.Master.Master).masterFindControl(myRpt)));
                        if (rpt != null)
                        {
                            wb = (WebControl)rpt.Items[0].FindControl(strID);
 
                        }

                    }
                    if (wb != null)
                        strID += wb.ClientID;
                    else
                        strID = myID;

                    if (strID != "")
                    {
                        strScript += "document.getElementById('" + strID + "').value=r." + aryFields[i].ToString() + ";\n";
                        strScript1 += "document.getElementById('" + strID + "').value='';\n";
                    }

                }
                this.scriptSetValue = strScript;
                this.scriptSetValueNull = strScript1;

                this.txtDialog.Attributes.Add("onfocusout",this.txtDialog.ClientID+ "loadDialog();");
        //    }

            if (this.width!="" && this.bolVisibleField )
            this.txtDialog.Width = Unit.Parse(this.width);

            /*
           bool bolAjax = false;
            try
            {
                bolAjax = ((System.Web.UI.ScriptManager)this.Page.Master.Master.FindControl("ScriptManager1")).IsInAsyncPostBack;
            }
            catch
            { }

            if (bolAjax)
            {
          //      ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), this.txtDialog.ClientID + "post", txtDialog.ClientID + "post=true;", true);
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), this.txtDialog.ClientID + "init()", txtDialog.ClientID +"_init()", true);
            }*/
          
        }
    }
}