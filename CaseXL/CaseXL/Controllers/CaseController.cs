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
    public class CaseController : Controller
    {
        //
        // GET: /Case/
        [Authorize()]
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult _Cases([DataSourceRequest] DataSourceRequest request)
        {
            var data = Repository.GetCaseDetails();
            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult _CreateCase([DataSourceRequest] DataSourceRequest request, CaseVM model)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                Case entity = new Case
                {
                    Caption = model.Caption,
                    Case_Number = model.Case_Number,
                    Case_Type_ID = new int?(model.Case_Type),
                    Firm_ID = SessionBase.Firm.ID == 0 ? SessionBase.User.Id : SessionBase.Firm.ID
                };
                entities.Add(entity);
                entities.SaveChanges();
                model.Case_Id = entity.ID;
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult _DeleteCase([DataSourceRequest] DataSourceRequest request, CaseVM model)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                Case entity = (from a in entities.Cases
                               where a.ID == model.Case_Id
                               select a).FirstOrDefault<Case>();
                entities.Delete(entity);
                entities.SaveChanges();
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }



        public ActionResult _UpdateCase([DataSourceRequest] DataSourceRequest request, CaseVM model)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                var casetUpdate = (from a in entities.Cases
                                   where a.ID == model.Case_Id
                                   select a).FirstOrDefault();
                casetUpdate.Caption = model.Caption;
                casetUpdate.Case_Number = model.Case_Number;
                casetUpdate.Case_Type_ID = new int?(model.Case_Type);
                entities.SaveChanges();
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }











    }
}
