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
    public partial class ocxTime : System.Web.UI.UserControl
    {

        public override string ClientID
        {

            get
            {
                return this.txtTime.ClientID;
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
                    strValue = DateTime.Parse(value.Replace(",", "")).ToString("HH:mm");
                }
                catch { }


                this.txtTime.Text = strValue.ToString();
            }
            get
            {
                if (this.txtTime.Text == "")
                    return "";
                else
                    return txtTime.Text.Replace("_", "").ToString();
            }
        }

    }
}