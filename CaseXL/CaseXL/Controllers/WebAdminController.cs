using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
namespace CaseXL.Controllers
{
    public class WebAdminController : Controller
    {
        //
        // GET: /WebAdmin/
        [Authorize()]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _Users([DataSourceRequest] DataSourceRequest request)
        {
           return Json(Common.Repository.GetTrialUsers().ToDataSourceResult(request));
        }

    }
}
