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

namespace OrixMvc
{
    public partial class imageViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["img"] != null)
            {
                byte[] bytes = (byte[])Session["img"];

                string strExtName = (String)Session["ExtName"];
                Response.Buffer = true;
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = (string )Session["mime"];
                Response.AddHeader("Content-Length", bytes.Length.ToString());
                Response.AppendHeader("Content-Disposition", "attachment; filename=\"dwonload." + strExtName + "\"");
               
                Response.AppendHeader("Pragma", "public");

                Session["mime"] = null ;
                Session["img"] = null;
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.End();

               /* Response.ClearContent();
                Response.ClearHeaders();
                Response.Buffer = true;
                Response.ContentType = ReturnExtension(file.Extension.ToLower());
                Response.AddHeader("Content-Length", getContent.Length.ToString());
                Response.AddHeader("Content-Disposition", "attachment; filename=" + FileName);
                Response.BinaryWrite(getContent);
                Response.Flush();
                Response.End();
                */

            }

        }
    }
}
