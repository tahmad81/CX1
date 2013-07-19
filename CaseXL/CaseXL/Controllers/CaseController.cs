using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CaseXL.Controllers
{
    public class CaseController : Controller
    {
        //
        // GET: /Case/
        [Authorize()]
        public ActionResult Index()
        {
            
            return View();
        }

    }
}
