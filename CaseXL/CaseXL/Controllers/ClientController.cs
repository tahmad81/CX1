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
        // [Authorize(Roles = "Lawyer")]
        public ActionResult Index(string returnUrl)
        {
            return View();
        }
        public ActionResult _Clients([DataSourceRequest] DataSourceRequest request)
        {
            return Json(Common.Repository.GetClients().ToDataSourceResult(request));
        }
        public ActionResult _ClientCreate([DataSourceRequest] DataSourceRequest request, ClientVM model)
        {
            using (CaseXL.Data.CaseXLEntities context = new CaseXLEntities())
            {
                App_User user = new App_User()
                {
                    Email = model.Email,
                    FirmId = SessionBase.Firm.ID,
                    FirstName = model.First_Name,
                    LastName = model.Last_Name,
                    Is_Client = true,

                };
                context.Add(user);
                model.Cases.ForEach(cas =>
                {
                    Data.Client_Case clCase = new Client_Case()
                    {
                        CaseId = cas.ID,
                        ClientId = user.Id
                    };

                });
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
    }
}
