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
    public partial class ocxDate : System.Web.UI.UserControl
    {

        public override string ClientID
        {

            get
            {
                return this.txtDate.ClientID;
            }
        }

        /// <summary>
        /// 顯示內容
        /// </summary>
        public String Text
        {
            set
            {
                string strValue = "";

                try
                {
                    strValue = DateTime.Parse(value.Replace(",", "")).ToString("yyyy/MM/dd");
                }
                catch { }


                this.txtDate.Text = strValue.ToString();
            }
            get
            {
                if (this.txtDate.Text == "")
                    return "";
                else
                    return txtDate.Text.Replace("_", "").ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            bool bolAjax = false;
            try
            {
                bolAjax = ((System.Web.UI.ScriptManager)this.Page.Master.Master.FindControl("ScriptManager1")).IsInAsyncPostBack;
            }
            catch
            { }

            if (bolAjax)
            {
                string strScript = "";
                strScript += " document.getElementById('" + this.txtDate.ClientID + "').onblur = function(evt) { \n";
                strScript += " var rtn = chkDate(document.getElementById('" + this.txtDate.ClientID + "')); \n";
                strScript += " if (rtn) { \n";
                strScript += " try { \n";

                strScript += " isDate(document.getElementById('" + this.txtDate.ClientID + "')); \n";

                strScript += " DateDo(document.getElementById('" + this.txtDate.ClientID + "'), document.getElementById('" + this.txtDate.ClientID + "').value); \n";
                strScript += " } \n";
                strScript += " catch (err) { \n";

                strScript += " } \n";

                strScript += " } \n";
                strScript += " }; \n";

                      ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), this.txtDate.ClientID + "post",strScript , true);
                //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), this.txtDialog.ClientID + "init()", txtDialog.ClientID + "_init()", true);
            }

        }

    }


 
}