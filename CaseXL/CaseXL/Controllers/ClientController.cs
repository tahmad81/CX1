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
using System.Web.Security;
using CaseXL.Models;
using System.Web.Routing;

namespace CaseXL.Controllers
{
    public class ClientController : Controller
    {
        // [Authorize(Roles = "Lawyer")]

        public IMembershipService MembershipService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {

            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }
            base.Initialize(requestContext);
        }


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
                    SubscriptionNo = SessionBase.User.SubscriptionNo
                };
                context.Add(user);
                context.SaveChanges();
                MembershipCreateStatus createStatus = this.MembershipService.CreateUser(model.UserName, model.Password, model.Email);
                if (createStatus == MembershipCreateStatus.Success)
                {
                    if (!Roles.GetAllRoles().Contains("Client"))
                    {
                        Roles.CreateRole("Client");
                    }
                    Roles.AddUserToRole(model.UserName, "Client");
                }
                Common.EmailSender email = new EmailSender();
                string header = "<div style='height:20px; background:blue;color:white;border-radius:4px;text-align:center;font-weight:bold'>Do not reply to this email </div><br/><br/><br/>";
                string message = "Dear " + model.First_Name + " " + model.Last_Name + "<br/>" + "You have been subscribed to caseLinq, You username and passowrd is below" + "<br/>" + "Username = " + model.UserName + " " + "Password = " + " " + model.Password + "<br/> thanks,<br/>CaseLinq.";
                new EmailSender { To = user.Email, Body = header + message }.Send();
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        public ActionResult _CaseLinking(int? clientid)
        {
            return PartialView("_CaseUserLinking", new ViewModels.CaseLinkingVM() { UserId = clientid.GetValueOrDefault() });
        }
        public ActionResult _UserCases([DataSourceRequest] DataSourceRequest request, int? userid)
        {
            return Json(Common.Repository.GetCasesByClient(userid.GetValueOrDefault()).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult _UpdateUserCases(int? userid, [DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<CaseXL.ViewModels.CaseVM> updated)
        {
            using (Data.CaseXLEntities context = new CaseXLEntities())
            {
                var delObjects = updated.Where(a => a.CaseClientId != null && a.IsSelected == false).Select(a => a.CaseClientId).ToList();
                context.Delete(context.Client_Cases.Where(a => delObjects.Contains(a.Id)));
                var addObjects = (from addobj in updated.Where(a => a.CaseClientId == null && a.IsSelected == true)
                                  select new CaseXL.Data.Client_Case
                                  {
                                      ClientId = userid,
                                      CaseId = addobj.Case_Id
                                  }).ToList();

                context.Add(addObjects);
                context.SaveChanges();
            }
            return Json(true);
        }
    }
}
