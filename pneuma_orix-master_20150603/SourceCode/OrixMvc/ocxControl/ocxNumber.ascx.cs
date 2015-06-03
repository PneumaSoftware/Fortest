using System;
using System.Web.UI.WebControls;
using VS2008.Module;
using System.Web.UI;

namespace OrixMvc.ocxControl
{
    /// <summary>
    /// class library
    /// Author      ：  Alinta 
    /// Build Date  ：  2011/10/01 
    /// Modify Date ：  2012/05/03 by Alinta 
    /// Purpose     ：  Number's User Control
    /// </summary>  
    public partial class ocxNumber : System.Web.UI.UserControl
    {

       /// <summary>
       /// on Text Changed
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
      
        public delegate void TextDelegate(object sender, EventArgs e);

        /// <summary>
        /// delegate OnTextChanged
        /// </summary>
        public event TextDelegate TextChanged;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnTextChanged(object sender, EventArgs e)
		{
            if (TextChanged != null)
			{
                TextChanged(this, e);
			}
            TextChanged = null;
		}         

        /// <summary>
        /// 小數點位數
        /// </summary>
        public string  MASK
        {
            set { ViewState["MASK"] = value; }
            get
            {
                return (ViewState["MASK"] == null ? "" : (string)ViewState["MASK"]);
            }
        }

        /// <summary>
        /// 是否接受負值
        /// </summary>
        public bool Minus
        {
            set { ViewState["Minus"] = value; }
            get
            {
                return (ViewState["Minus"] == null ? false  : (bool)ViewState["Minus"]);
            }
        }

        /// <summary>
        /// 是否開放編輯
        /// </summary>
        public bool bolEnabled
        {
            set { ViewState["bolEnabled"] = value; }
            get
            {
                return (ViewState["bolEnabled"] == null ? true : (bool)ViewState["bolEnabled"]);
            }
        }


        /// <summary>
        /// 顯示內容
        /// </summary>
        public String Text
        {
            set
            {
                double dblNumber = 0;

                try
                {
                    dblNumber = double.Parse(value.Replace(",", ""));
                }
                catch { }


                this.myNumber.Text = dblNumber.ToString("###,###,##0.####");
            }
            get
            {
                if (this.myNumber.Text == "")
                    return "0";
                else
                    return myNumber.Text.Replace("_", "").Replace(",", "").Trim().toNumber().ToString();
            }
        }


        public void clientText(double val)
        {            
            if (val.ToString().IndexOf(".")!=-1)
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), this.myNumber.ClientID, "document.getElementById('" + this.myNumber.ClientID + "').value='" + val.ToString("###,###,##0.####") + "'", true);
            else
                ScriptManager.RegisterStartupScript(this.Page,this.Page.GetType(),this.myNumber.ClientID,"document.getElementById('"+ this.myNumber.ClientID+"').value='"+ val.ToString("###,###,##0")+"'",true);
        }

        /// <summary>
        /// 小數點的長度
        /// </summary>
        /*  
         public Int32 decimalLength
        {
            set { ViewState["decimalLength"] = value; }
            get
            {
                return (ViewState["decimalLength"] == null ? 0 : (Int32)ViewState["decimalLength"]);
            }
        }
        */

        public override   string  ClientID
        {
            
            get
            {
                return this.myNumber.ClientID;
            }
        }

    

        /// <summary>
        /// 避掉單引號
        /// </summary>
        /// <returns></returns>
        public string rpsText()
        {
            return this.Text.Replace("'", "").Trim();
        }

        /// <summary>
        /// 網頁初始設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.MASK.Length > 8)
                this.myNumber.Width = ((this.MASK.Length - 3) * 10) + 4;
            else if (this.MASK.Length > 7)
                this.myNumber.Width = ((this.MASK.Length - 3) * 10) + this.MASK.Length;
            else if (this.MASK.Length > 3)
                this.myNumber.Width = ((this.MASK.Length - 3) * 11) + this.MASK.Length;
            else
                this.myNumber.Width = 30;


            string strH = "";
            if (this.ID.IndexOf("HIRE")!=-1)
                strH="";

            this.myNumber.Editing(bolEnabled);
            if (bolEnabled==false)
                this.myNumber.CssClass = "display";

           
           /* if (TextChanged != null)
            {

                this.myNumber.Attributes.Add("onchange", "bolPost=true;window.setTimeout('doPost(\"" + this.myNumber.UniqueID + "\")', 200)");
              //  this.myNumber.AutoPostBack = true;
            }*/
        }


    }
}