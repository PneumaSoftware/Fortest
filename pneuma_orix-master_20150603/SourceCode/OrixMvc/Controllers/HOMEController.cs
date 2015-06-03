using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrixMvc.Controllers
{
    public class HOMEController : CommonController
    {
        //
        // GET: /MONE/

        public ActionResult Index()
        {
            return Redirect("~/Login.aspx");
        }

    }
}
