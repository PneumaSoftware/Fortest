using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Web.SessionState;
namespace OrixMvc
{
    /// <summary>
    /// $codebehindclassname$ 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class imageViewer1 : IHttpHandler,  IReadOnlySessionState 
    {

        public void ProcessRequest(HttpContext context)
        {
            byte[] bytes = (byte[])context.Session["img"];

            string mime = (string)context.Session["mime"];

            context.Response.ContentType = mime;
            context.Response.BinaryWrite(bytes);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
