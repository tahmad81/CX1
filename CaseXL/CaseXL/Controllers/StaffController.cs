using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
namespace CaseXL.Controllers
{
    public class StaffController : Controller
    {
        //
        // GET: /Staff/
        public static List<ViewModels.StaffVM> Staffs;
        public StaffController()
        {

        }
        public ActionResult Index()
        {
            //ViewBag.Partners = CaseXL.Common.Repository.GetPartners(1);
            return View();
        }
        public ActionResult _Staff([DataSourceRequest] DataSourceRequest request)
        {
            using (CaseXL.Data.CaseXLEntities context = new CaseXL.Data.CaseXLEntities())
            {

                var data = (from st in context.Staffs
                            select new ViewModels.StaffVM
                            {
                                ID = st.ID,
                                FirstName = st.FirstName,
                                LastName = st.LastName,
                                PartnerName = st.Partner.FristName + " " + st.Partner.LastName,
                                PartnerID = st.Partner.ID,
                                Firm_ID = st.Firm.ID
                            }).ToList();


                return Json(data.ToDataSourceResult(request));
            }
        }
        public ActionResult _StaffEdit([DataSourceRequest] DataSourceRequest request, ViewModels.StaffVM model)
        {
            using (CaseXL.Data.CaseXLEntities context = new CaseXL.Data.CaseXLEntities())
            {
                if (ModelState.IsValid & model != null)
                {
                    var staffToUpdate = context.Staffs.Where(a => a.ID == model.ID).FirstOrDefault();
                    staffToUpdate.FirstName = model.FirstName;
                    staffToUpdate.LastName = model.LastName;
                    staffToUpdate.Partner_ID = model.PartnerID;
                    context.SaveChanges();
                }
                return Json(ModelState.ToDataSourceResult(), JsonRequestBehavior.AllowGet);

            }
        }
        public ActionResult _StaffCreate([DataSourceRequest] DataSourceRequest request, ViewModels.StaffVM model)
        {
            using (CaseXL.Data.CaseXLEntities context = new CaseXL.Data.CaseXLEntities())
            {
                if (ModelState.IsValid && model != null)
                {
                    CaseXL.Data.Staff staffToAdd = new Data.Staff()
                    {
                        FirstName = model.FirstName.Trim(),
                        LastName = model.LastName.Trim(),
                        Firm_ID = 1,
                        Partner_ID = model.PartnerID
                    };
                    context.Add(staffToAdd);
                    context.SaveChanges();
                }
                return Json(new[] { model }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult _StaffDelete([DataSourceRequest] DataSourceRequest request, ViewModels.StaffVM model)
        {
            using (CaseXL.Data.CaseXLEntities context = new CaseXL.Data.CaseXLEntities())
            {
                if (model != null)
                {
                    var objToDelete = context.Staffs.Where(a => a.ID == model.ID).FirstOrDefault();
                    if (objToDelete != null)
                    {
                        context.Delete(objToDelete);
                        context.SaveChanges();
                    }
                }
                return Json(ModelState.ToDataSourceResult(), JsonRequestBehavior.AllowGet);

            }
        }
    }
}