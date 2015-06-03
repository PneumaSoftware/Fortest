using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VS2008.Module;

namespace OrixMvc.ocxControl
{
    public partial class upload : System.Web.UI.Page
    {

         
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void upload_Click(object sender, EventArgs e)
        {
            //判斷檔案是否存在
            if (this.myFile.HasFile)
            {
                //取得副檔名
                string ExtName = System.IO.Path.GetExtension(this.myFile.PostedFile.FileName).ToLower();
                string MIME = "";
                DataGetting dg = new DataGetting("myConnectionString");
                MIME = dg.GetDataRow("select MIME_TYPE from or3_mime where FILE_EXT='" + ExtName + "'")[0].ToString().Trim();
                ExtName = ExtName.Replace(".", "");

              /*  switch (ExtName.ToLower())
                {
                    case "gif":
                        MIME = "image/gif";
                        break;

                    case "jpg":
                        MIME = "image/jpg";
                        break;

                    case "png":
                        MIME = "image/png";
                        break;

                    case "doc":
                        MIME = "application/msword";
                        break;

                    case "docx":
                        MIME = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        break;

                    default:
                        break;
                }
                */
                if (MIME != "")
                {
                    
                    byte[] bImage = new byte[this.myFile.PostedFile.InputStream.Length];
                    this.myFile.PostedFile.InputStream.Read(bImage, 0, bImage.Length);
                    Session["bImage"] = bImage;
                    Session["MIME"] = MIME;
                    Session["ExtName"] = ExtName;
                    string strID = this.Request.QueryString["ID"].ToString();
                    string strScript = "window.parent.document.getElementById('"+ strID +"').click();\n";                    
                    strScript+="document.getElementById('divUpload').style.display = '';\n";
                    strScript += "document.getElementById('divWaiting').style.display = 'none';\n";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "saveImage", strScript, true);


                   

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "errorExt", "alert('副檔名有誤!');", true);
                }
            }
        }
    }
}
