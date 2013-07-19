using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc;
using Kendo.Mvc.UI;

namespace CaseXL.Controllers
{

    public class IssuesController : Controller
    {
        //
        // GET: /Issues/
        public ActionResult Index(ViewModels.IssuesVM model, int? id)
        {
            model.Issues = Common.Repository.GetAllCasesWithIssues();
            // model.Issue_Name = model.Issue_Exp = "";
            return View(model);
        }
        public ActionResult _IssuesDelete([DataSourceRequest] DataSourceRequest request, ViewModels.StaffVM model)
        {

            return null;

        }
        public ActionResult Issue_Create(ViewModels.IssuesVM model)
        {
            try
            {
                using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
                {
                    CaseXL.Data.Issue issue = new Data.Issue()
                    {
                        Case_ID = int.Parse(model.SelectedCase),
                        Issue_Expl = model.Issue_Exp,
                        Issue_Name = model.Issue_Name
                    };
                    context.Add(issue);
                    context.SaveChanges();
                    model.Issues = Common.Repository.GetIssuesByCases(model.Case_ID);
                }
            }
            catch { }
            return RedirectToAction("Index");

        }
        public ActionResult _IssuesUpdate([DataSourceRequest] DataSourceRequest request, ViewModels.StaffVM model)
        {

            return null;

        }
        public ActionResult Evaluate(int? caseID, string msg)
        {
            ShowMessage(msg);
            var cases = Common.Repository.GetCases();
            Session["caseid"] = caseID;
            ViewBag.Cases = new SelectList(cases, "ID", "Name", caseID.GetValueOrDefault());
            return View();
        }
        [HttpPost()]
        public ActionResult Evaluate_Save(ViewModels.IssuesVM model)
        {

            using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
            {
                var objToUpdate = context.Issues.Where(a => a.Id == model.Id).FirstOrDefault();
                objToUpdate.Issue_Name = model.Issue_Name;
                objToUpdate.Issue_Expl = model.Issue_Exp;
                objToUpdate.Question = model.Issue_Question;
                objToUpdate.Evaluation = model.Issue_Evaluation;
                context.SaveChanges();
            }
            return RedirectToAction("Evaluate", new { @caseID = model.Case_ID, @msg = "Evaluation Saved!" });
        }
      
        public JsonResult _Issues()
        {
            //int? caseid = (int?)Session["caseid"];
            int? caseid = 1;
            if (caseid.HasValue)
            {
                return Json(Common.Repository.GetIssuesByCases(caseid.Value), JsonRequestBehavior.AllowGet);
            }
            return null;

        }
        void ShowMessage(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                ViewData["OperationMsg"] = message;
            }

        }
    }

}
