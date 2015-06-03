using System;
using System.Web.UI.WebControls;
using VS2008.Module;
using System.Web.UI;
using System.Data;
using System.Web;

namespace OrixMvc.ocxControl
{
    /// <summary>
    /// class library
    /// Author      ：  Alinta 
    /// Build Date  ：  2011/10/01 
    /// Modify Date ：  2012/05/03 by Alinta 
    /// Purpose     ：  Number's User Control
    /// </summary>  
    public partial class ocxUpload : System.Web.UI.UserControl
    {

        /// <summary>
        /// check, 編輯是否開放
        /// </summary>
        public bool bolLock()
        {

            return (txtUpload.CssClass.IndexOf("slock") != -1);

        }

        public string newSeq
        {
            set { ViewState["newSeq"] = value; }
            get { return (ViewState["newSeq"] == null ? "" : (string)ViewState["newSeq"]); }
        }

        public string Seq
        {
            set { ViewState["Seq"] = value; }
            get { return (ViewState["Seq"] == null ? "" : (string)ViewState["Seq"]); }
        }

        public bool bolUpload
        {
            set { ViewState["bolUpload"] = value; }
            get { return (ViewState["bolUpload"] == null ? true : (bool)ViewState["bolUpload"]); }
        }

        public byte[] bImage
        {
            set {
                if (newSeq=="")
                    newSeq=Seq;

                if (value == null)
                    return;

                if (value.Length!=0)
                    Session[newSeq] = value; 
            }
            get { return (Session[newSeq] == null ? null : (byte[])Session[newSeq]); }
        }

        public string ExtName
        {
            set {
                if (newSeq == "")
                    newSeq = Seq;

                if (value == "")
                    return;

                Session[newSeq + "ExtName"] = value; }
            get { return (Session[newSeq + "ExtName"] == null ? "" : (string)Session[newSeq + "ExtName"]); }
        }

        public string MIME
        {
            set {
                if (newSeq == "")
                    newSeq = Seq;

                if (value == "")
                    return;

                Session[newSeq + "MIME"] = value; }
            get { return (Session[newSeq + "MIME"] == null ? "" : (string)Session[newSeq + "MIME"]); }
        }


        /// <summary>
        /// 網頁初始設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.bolUpload = !this.bolLock();

            }

            string strScript = " document.getElementById('" + this.view.ClientID + "').disabled = false;";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "disView", strScript, true);
           
        }



        protected void SaveImage(object sender, EventArgs e)
        {
            if (this.Seq == "")
            {
                this.Seq = System.DateTime.Now.ToString("yyMMddHHmmssfff");
                this.newSeq = this.Seq;
            }
            else
            {
                if (newSeq != "")
                {
                    Session[newSeq] = null;
                    Session[newSeq + "ExtName"] = null;
                    Session[newSeq + "MIME"] = null;
                }

                    this.newSeq = System.DateTime.Now.ToString("yyMMddHHmmssfff");
                
            }
            this.bImage = (byte[])Session["bImage"];
            this.ExtName = (string)Session["ExtName"];
            this.MIME = (string)Session["MIME"];
            Session["bImage"] = null;
            Session["ExtName"] = null;
            Session["MIME"] = null;

            string strScript = "document.getElementById('"+this.btnImage.ClientID+"spanSeq').style.display='';\n";
            strScript += "try { loadImage('" + this.ID + "'); } catch(err){}finally{}\n";

            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "showView",strScript  ,true);
        }

        public void View(){
            this.view_Click(this.view, null);
        }

        protected void view_Click(object sender, EventArgs e)
        {
            if (Seq=="")
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "errorExt", "alert('尚未上傳檔案, 無法檢視!');", true);
                return;
            }

            if (this.bImage != null)
            {
                if (this.bImage.Length == 0)
                    this.bImage = null;
            }

            string strExtName = "";
            string strMime = "";
            byte[] byteImage=null;

            if (this.Seq != "" && this.MIME  == "")
            {
                DataGetting dg = new DataGetting("myConnectionString");
                
                DataTable dt = dg.GetDataTable("select a.file_type,a.binary_file,b.mime_type from OR3_FILE_STORE a left join OR3_MIME b on '.'+a.FILE_TYPE=b.file_ext where a.FILE_SEQ='" + this.Seq + "'");
                if (dt.Rows.Count > 0)
                {
                    strExtName = dt.Rows[0]["FILE_TYPE"].ToString();
                    strMime  = dt.Rows[0]["mime_type"].ToString();
                    byteImage = (byte[])dt.Rows[0]["BINARY_FILE"];

                    //Session["ExtName"] = this.ExtName;
                    //Session["mime"] = MIME;
                    //Session["img"] = (byte[])dt.Rows[0]["BINARY_FILE"];
                }

            }
            else
            {
                strExtName = this.ExtName;
                strMime = this.MIME;
                byteImage = this.bImage;
            }
           
           // Response.Write(byteImage.Length);
            //return;
           // else
           //// {//
             //   Session["mime"] = this.MIME;
            //    Session["img"] = this.bImage;
           // }
            //Image1.ImageUrl = "~/ImageViewer.ashx?dt="+System.DateTime.Now.ToString("HHmms");


          //  byte[] bytes = (byte[])Session["img"];

       
            
           // Session["ExtName"] = this.ExtName;

          
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = strMime;
            Response.AddHeader("Content-Length", byteImage.Length.ToString());
            Response.AppendHeader("Content-Disposition", "attachment; filename=dwonload." + strExtName);
             Response.AppendHeader("Pragma", "public");


             Response.BinaryWrite(byteImage);
            Response.Flush();
            Response.End();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "set", "window.parent.closeLoading();", true);


            
            return;
            
           // string strScript = "window.parent.document.getElementById('iframeFile').src='" + "ImageViewer.aspx?dt=" + System.DateTime.Now.ToString("HHmmss") + "';\n";
           // strScript += "window.parent.document.getElementById('divPopWindow').style.display='';\n";
            string strScript = "window.open('" + "ImageViewer.aspx?dt=" + System.DateTime.Now.ToString("HHmmss") + "','_blank');\n";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "openImage", strScript , true);
            
            //this.Response.Redirect("ImageViewer.aspx");
        }

        public bool ByteArrayToFile(string _FileName, byte[] _ByteArray)
        {
            try
            {
                // Open file for reading
                System.IO.FileStream _FileStream =
                   new System.IO.FileStream(_FileName, System.IO.FileMode.Create,
                                            System.IO.FileAccess.Write);
                // Writes a block of bytes to this stream using data from
                // a byte array.
                _FileStream.Write(_ByteArray, 0, _ByteArray.Length);

                // close file stream
                _FileStream.Close();

                return true;
            }
            catch (Exception _Exception)
            {
                // Error
                Console.WriteLine("Exception caught in process: {0}",
                                  _Exception.ToString());
            }

            // error occured, return false
            return false;
        }

    }
}