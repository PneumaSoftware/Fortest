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
    public partial class ocxYM : System.Web.UI.UserControl
    {

        public override string ClientID
        {

            get
            {
                return this.txtYM.ClientID;
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
                    strValue = DateTime.Parse(value.Replace(",", "")).ToString("yyyy/MM");
                }
                catch { }


                this.txtYM.Text = strValue.ToString();
            }
            get
            {
                if (this.txtYM.Text == "")
                    return "";
                else
                    return txtYM.Text.Replace("_", "").ToString();
            }
        }

    }
}