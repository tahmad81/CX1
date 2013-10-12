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
            return Json(Common.Repository.GetClients().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
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
                    CVNNo = string.Empty,
                    CreditCard = string.Empty,
                    Exp_Date = DateTime.Today,
                    UserName = model.UserName,


                };
                context.Add(user);
                context.SaveChanges();
                Common.EmailSender email = new EmailSender();
                string header = "<div style='height:20px; background:blue;color:white;border-radius:4px;text-align:center;font-weight:bold'>Do not reply to this email </div><br/><br/><br/>";
                string message = "Dear " + model.First_Name + " " + model.Last_Name + "<br/>" + "You have been subscribed to caseLinq, You username and passowrd is below" + "<br/>" + "Username = " + model.UserName + " " + "Password = " + " " + model.Password + "<br/> thanks,<br/>CaseLinq.";
                new EmailSender { To = user.Email, Body = header + message }.Send();
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
    }
}
