using CaseXL.Common;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using CaseXL.Data;
using CaseXL.Infrastructure;
using CaseXL.ViewModels;

namespace CaseXL.Controllers
{
    public class ClientController : Controller
    {
        [Authorize(Roles = "Lawyer")]
        public ActionResult Index(string returnUrl)
        {
            return View();
        }
        public ActionResult _Clients([DataSourceRequest] DataSourceRequest request)
        {
           return Json(Common.Repository.GetClients().ToDataSourceResult(request));
        }
    }
}
