using System;
using System.Web.UI.WebControls;


namespace OrixMvc.ocxControl
{
    /// <summary>
    /// class library
    /// Author      ：  Alinta 
    /// Build Date  ：  2011/10/01 
    /// Modify Date ：  2012/05/03 by Alinta 
    /// Purpose     ：  Date's User Control
    /// </summary>  
    public partial class ocxYear : System.Web.UI.UserControl
    {

        public override string ClientID
        {

            get
            {
                return this.txtYear.ClientID;
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

              
                this.txtYear.Text = value.ToString();
            }
            get
            {
                if (this.txtYear.Text == "")
                    return "";
                else
                    return txtYear.Text.Replace("_", "").ToString();
            }
        }

    }
}