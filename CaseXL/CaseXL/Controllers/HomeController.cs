﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using CaseXL.Infrastructure;
using CaseXL.Common;
using CaseXL.Data;
using CaseXL.ViewModels;
using AuthorizeNet;
using CaseXL.Auth_Service;
namespace CaseXL.Controllers
{
    public class HomeController : Controller
    {
        public CaseXL.Auth_Service.ServiceSoapClient _webservice = new ServiceSoapClient();
        private long _subscriptionId = 0;
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
        [Authorize]
        public ActionResult Main(int? caseID, string msg)
        {

            SetfirmInSession();

            var cases = Common.Repository.GetCases();
            Session["caseid"] = caseID;
            ViewBag.Cases = new SelectList(cases, "ID", "Name", caseID.GetValueOrDefault());

            return View();
        }

        public ActionResult Test(int? id)
        {

            return null;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Checkboxes(string[] checkedNodes, List<CaseXL.ViewModels.IssuesVM> model)
        {

            return null;
        }
        public ActionResult _IssueLinking(int? caseid, int? factID)
        {

            return PartialView("_IssueLinking", new ViewModels.IssueLinkingVM() { CaseID = caseid, FactID = factID });

        }
        public ActionResult _WitnessLinking(int? caseid, int? factID)
        {

            return PartialView("_WitnessesLinking", new ViewModels.WitnessesLinkingVM() { CaseID = caseid, FactID = factID });

        }
        public ActionResult _ExhibitsLinking(int? caseid, int? factID)
        {

            return PartialView("_ExhibitLinking", new ViewModels.ExhibitLinkingVM() { CaseID = caseid, FactID = factID });

        }

        #region Issues
        public ActionResult _Issues([DataSourceRequest] DataSourceRequest request, int? caseid, int? factID)
        {
            var issues = Common.Repository.GetIssuesByFactCase(caseid.Value, factID.Value);

            return Json(issues.ToDataSourceResult(request));

        }

        public ActionResult _UpdateIssues(int? factID, int? caseid, [DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<CaseXL.ViewModels.IssuesVM> updated)
        {
            using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
            {
                var delObjects = updated.Where(a => a.IsSelected == false).Select(a => a.Issue_Fact_Id).ToList();
                context.Delete(context.Issues_Facts.Where(a => delObjects.Contains(a.Id)));

                var addObjects = (from addobj in updated.Where(a => a.IsSelected == true && a.Issue_Fact_Id == null)
                                  select new CaseXL.Data.Issues_Fact
                                  {
                                      Case_Id = caseid,
                                      Fact_Id = factID,
                                      Issue_Id = addobj.Id
                                  }).ToList();

                context.Add(addObjects);
                context.SaveChanges();
            }
            return Json(true);
        }
        #endregion

        #region Facts
        public ActionResult _GetFacts(int caseid)
        {
            Session["caseid"] = caseid;
            return PartialView("_Facts");
        }
        public ActionResult _Facts([DataSourceRequest] DataSourceRequest request)
        {
            using (CaseXL.Data.CaseXLEntities context = new CaseXL.Data.CaseXLEntities())
            {

                var data = (from facts in context.Facts.Where(a => a.Case_Id == int.Parse(Session["caseid"].ToString()))
                            select new ViewModels.FactsVM
                            {
                                CaseID = facts.Case_Id,
                                Date = facts.Date,
                                Description = facts.Description,
                                Evaluation = facts.Description,
                                Source = facts.Source,
                                ID = facts.Id,
                                Evaluation_Id = facts.Evaluation1.Id,
                                Evaluation_Text = facts.Evaluation1.Evaluation_Text
                            }).OrderBy(o => o.Date).ToList();


                return Json(data.ToDataSourceResult(request));
            }
        }
        public ActionResult _FactDelete([DataSourceRequest] DataSourceRequest request, ViewModels.FactsVM model)
        {
            using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
            {
                var objtoDelete = context.Facts.Where(a => a.Id == model.ID).FirstOrDefault();
                if (objtoDelete != null)
                {
                    context.Delete(objtoDelete);
                    context.SaveChanges();
                }
                return Json(new[] { model }.ToDataSourceResult(request, ModelState));
            }

        }
        public ActionResult _FactsEdit([DataSourceRequest] DataSourceRequest request, ViewModels.FactsVM model)
        {
            using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
            {
                var objtoUpdate = context.Facts.Where(a => a.Id == model.ID).FirstOrDefault();
                if (objtoUpdate != null)
                {
                    objtoUpdate.Evaluation_Id = model.Evaluation_Id;
                    objtoUpdate.Date = model.Date;
                    objtoUpdate.Source = model.Source;
                    objtoUpdate.Description = model.Description;

                    context.SaveChanges();
                }
                return Json(new[] { model }.ToDataSourceResult(request, ModelState));
            }

        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult _FactsCreate([DataSourceRequest] DataSourceRequest request, ViewModels.FactsVM model)
        {
            using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
            {
                var objtoUpdate = new CaseXL.Data.Fact();

                if (objtoUpdate != null)
                {
                    objtoUpdate.Evaluation_Id = model.Evaluation_Id;
                    objtoUpdate.Date = model.Date;
                    objtoUpdate.Source = model.Source;
                    objtoUpdate.Description = model.Description;
                    objtoUpdate.Case_Id = int.Parse(Session["caseid"].ToString());
                    context.Add(objtoUpdate);
                    context.SaveChanges();
                    model.ID = objtoUpdate.Id;
                    model.CaseID = objtoUpdate.Case_Id;
                }

                return Json(new[] { model }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
            }

        }
        #endregion
        #region Witness
        public ActionResult _Witnesses([DataSourceRequest] DataSourceRequest request, int? caseid, int? factID)
        {
            var witnesses = Common.Repository.GetWitnessesByCaseFact(caseid.Value, factID.Value);
            return Json(witnesses.ToDataSourceResult(request));
        }
        public ActionResult _UpdateWitnesses(int? factID, int? caseid, [DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<CaseXL.ViewModels.WitnessVM> updated)
        {
            using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
            {
                var delObjects = updated.Where(a => a.IsSelected == false && a.Issue_Witness_Id != null).Select(a => a.ID).ToList();
                context.Delete(context.Witness_Facts.Where(a => delObjects.Contains(a.Id)));
                var addObjects = (from addobj in updated.Where(a => a.IsSelected == true && a.Issue_Witness_Id == null)
                                  select new CaseXL.Data.Witness_Fact
                                  {
                                      CaseId = caseid,
                                      FactId = factID,
                                      WitnessId = addobj.WitnessId
                                  }).ToList();

                context.Add(addObjects);
                context.SaveChanges();
            }
            return Json(true);
        }
        public ActionResult _GetWitnesses(int? caseid)
        {
            Session["caseid"] = caseid;
            return PartialView("_Witnesses");


        }
        public ActionResult _WitnessesByCase([DataSourceRequest] DataSourceRequest request)
        {
            var data = Common.Repository.GetWitnessesByCase(int.Parse(Session["caseid"].ToString()));
            return Json(data.ToDataSourceResult(request));

        }
        public ActionResult _UpdateWitnessesByCase([DataSourceRequest] DataSourceRequest request, ViewModels.WitnessVM model)
        {
            using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
            {
                var data = context.Witnesses.Where(a => a.Id == model.WitnessId).FirstOrDefault();
                data.Email = model.Email;
                data.Name = model.FullName;
                data.Docs_Authored = model.Docs_Authored;
                data.Docs_Received = model.Docs_Received;
                context.SaveChanges();

            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        public ActionResult _CreateWitnessByCase([DataSourceRequest] DataSourceRequest request, ViewModels.WitnessVM model)
        {
            using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
            {
                CaseXL.Data.Witness witness = new Data.Witness();
                witness.Email = model.Email;
                witness.Name = model.FullName;
                witness.CaseId = int.Parse(Session["caseid"].ToString());
                witness.Docs_Authored = model.Docs_Authored;
                witness.Docs_Received = model.Docs_Received;
                context.Add(witness);
                context.SaveChanges();
                model.WitnessId = model.ID = witness.Id;

            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        public ActionResult _DeleteWitnessByCase([DataSourceRequest] DataSourceRequest request, ViewModels.WitnessVM model)
        {
            using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
            {
                var data = context.Witnesses.Where(a => a.Id == model.WitnessId).FirstOrDefault();
                context.Delete(data);
                context.SaveChanges();
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        public ActionResult _QuestionsByWitness([DataSourceRequest] DataSourceRequest request, int? witnessId)
        {
            var data = Common.Repository.GetQuestionsByWitness(witnessId.GetValueOrDefault());
            return Json(data.ToDataSourceResult(request));
        }
        public ActionResult _QuestionsByWitnessUpdate([DataSourceRequest] DataSourceRequest request, ViewModels.QuestionVM model)
        {
            using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
            {
                var data = context.Questions.Where(a => a.Id == model.Id).FirstOrDefault();
                data.Question1 = model.Question;
                data.Answer = model.Answer;
                context.SaveChanges();
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        public ActionResult _QuestionsByWitnessCreate([DataSourceRequest] DataSourceRequest request, ViewModels.QuestionVM model, int? witnessId)
        {
            using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
            {
                var data = new CaseXL.Data.Question();
                data.Question1 = model.Question;
                data.Answer = model.Answer;
                data.Witness_Id = witnessId;
                context.Add(data);
                context.SaveChanges();
                model.Id = data.Id;
                model.Witness_Id = witnessId;
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        public ActionResult _QuestionsByWitnessDelete([DataSourceRequest] DataSourceRequest request, ViewModels.QuestionVM model)
        {
            using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
            {
                var data = context.Questions.Where(a => a.Id == model.Id).FirstOrDefault();
                context.Delete(data);
                context.SaveChanges();
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        #endregion
        #region Exhibits
        public ActionResult _Exhibits([DataSourceRequest] DataSourceRequest request, int? caseid, int? factID)
        {
            var issues = Common.Repository.GetExhibitsByFactCase(caseid.Value, factID.Value);

            return Json(issues.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

        }
        public ActionResult _UpdateExhibits(int? factID, int? caseid, [DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<CaseXL.ViewModels.ExhibitVM> updated)
        {
            using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
            {
                var delObjects = updated.Where(a => a.IsSelected == false && a.Fact_Exhibit != null).Select(a => a.Fact_Exhibit).ToList();
                context.Delete(context.Exhibits_Facts.Where(a => delObjects.Contains(a.Id)));
                var addObjects = (from addobj in updated.Where(a => a.IsSelected == true && a.Fact_Exhibit == null)
                                  select new CaseXL.Data.Exhibits_Fact
                                  {
                                      CaseId = caseid,
                                      FactId = factID,
                                      ExhibitId = addobj.Exhibit_Id

                                  }).ToList();
                context.Add(addObjects);
                context.SaveChanges();
            }
            return Json(true);
        }

        public ActionResult _GetExhibits(int? caseid)
        {
            Session["caseid"] = caseid;
            return PartialView("_Exhibits");


        }
        public JsonResult _ExhibitsByCase([DataSourceRequest] DataSourceRequest request)
        {
            var data = Common.Repository.GetExhibitsByCase(int.Parse(Session["caseid"].ToString()));
            return Json(data.ToDataSourceResult(request));
        }
        public ActionResult _DeleteExhibitByCase([DataSourceRequest] DataSourceRequest request, ViewModels.ExhibitVM model)
        {
            using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
            {
                var data = context.Exhibits.Where(a => a.Id == model.Exhibit_Id).FirstOrDefault();
                context.Delete(data);
                context.SaveChanges();
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        public ActionResult _CreateExhibitByCase([DataSourceRequest] DataSourceRequest request, ViewModels.ExhibitVM model)
        {
            using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
            {
                CaseXL.Data.Exhibit exhibit = new Data.Exhibit();
                exhibit.Author = model.Author;
                exhibit.CaseId = int.Parse(Session["caseid"].ToString());
                exhibit.Date = model.Date;
                exhibit.Description = model.Description;
                exhibit.Name = model.Name;
                exhibit.Recepient = model.Recipient;
                exhibit.Source = model.Source;
                exhibit.Type_Id = model.Type_Id;
                context.Add(exhibit);
                context.SaveChanges();
                model.Exhibit_Id = exhibit.Id;
                model.CaseId = exhibit.CaseId;

            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        public ActionResult _UpdateExhibitByCase([DataSourceRequest] DataSourceRequest request, ViewModels.ExhibitVM model)
        {
            using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
            {
                var objtoUpdate = context.Exhibits.Where(a => a.Id == model.Exhibit_Id).FirstOrDefault();

                objtoUpdate.Author = model.Author;
                objtoUpdate.CaseId = int.Parse(Session["caseid"].ToString());
                objtoUpdate.Date = model.Date;
                objtoUpdate.Description = model.Description;
                objtoUpdate.Name = model.Name;
                objtoUpdate.Recepient = model.Recipient;
                objtoUpdate.Source = model.Source;
                objtoUpdate.Type_Id = model.Type_Id;
                context.Add(objtoUpdate);
                context.SaveChanges();
                model.Exhibit_Id = objtoUpdate.Id;

            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        #endregion
        #region IssuesByCase
        public ActionResult _GetIssues(int? caseid)
        {
            Session["caseid"] = caseid;
            return PartialView("_Issues");


        }
        public ActionResult _IssuesByCase([DataSourceRequest] DataSourceRequest request)
        {
            var data = Common.Repository.GetIssuesByCases(int.Parse(Session["caseid"].ToString()));
            return Json(data.ToDataSourceResult(request));

        }
        public ActionResult _UpdateIssueByCase([DataSourceRequest] DataSourceRequest request, ViewModels.IssuesVM model)
        {
            using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
            {
                var data = context.Issues.Where(a => a.Id == model.Id).FirstOrDefault();
                data.Issue_Name = model.Issue_Name;
                data.Description = model.Description;
                context.SaveChanges();
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        public ActionResult _CreateIssueByCase([DataSourceRequest] DataSourceRequest request, ViewModels.IssuesVM model)
        {
            using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
            {
                CaseXL.Data.Issue issue = new Data.Issue();
                issue.Issue_Name = model.Issue_Name;
                issue.Description = model.Description;
                issue.Case_ID = int.Parse(Session["caseid"].ToString());
                context.Add(issue);
                context.SaveChanges();
                model.Id = issue.Id;
                model.Case_ID = issue.Case_ID;
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        public ActionResult _DeleteIssueByCase([DataSourceRequest] DataSourceRequest request, ViewModels.IssuesVM model)
        {
            using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
            {
                var data = context.Issues.Where(a => a.Id == model.Id).FirstOrDefault();
                if (data != null)
                {
                    context.Delete(data);
                    context.SaveChanges();
                }
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        #region Questions

        public ActionResult _QuestionsByIssues([DataSourceRequest] DataSourceRequest request, int? issueId)
        {
            var data = Common.Repository.GetQuestionsByIssues(issueId.GetValueOrDefault());
            return Json(data.ToDataSourceResult(request));
        }
        public ActionResult _QuestionsByIssuesUpdate([DataSourceRequest] DataSourceRequest request, ViewModels.QuestionVM model)
        {
            using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
            {
                var data = context.Issue_Questions.Where(a => a.Id == model.Id).FirstOrDefault();
                data.Question = model.Question;
                data.Answer = model.Answer;
                context.SaveChanges();
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        public ActionResult _QuestionsByIssuesCreate([DataSourceRequest] DataSourceRequest request, ViewModels.QuestionVM model, int? issueId)
        {
            using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
            {
                var data = new CaseXL.Data.Issue_Question();
                data.Question = model.Question;
                data.Answer = model.Answer;
                data.Issue_Id = issueId;
                context.Add(data);
                context.SaveChanges();
                model.Id = data.Id;
                model.Issue_Id = issueId;
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        public ActionResult _QuestionsByIssuesDelete([DataSourceRequest] DataSourceRequest request, ViewModels.QuestionVM model)
        {
            using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
            {
                var data = context.Issue_Questions.Where(a => a.Id == model.Id).FirstOrDefault();
                context.Delete(data);
                context.SaveChanges();
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }


        #endregion
        #endregion
        #region Documents
        public ActionResult _ExbDocs([DataSourceRequest] DataSourceRequest request, int? exbId)
        {

            return Json(Common.Repository.GetDocuments(1, exbId.Value).ToDataSourceResult(request));
        }
        public ActionResult _CreateDoc([DataSourceRequest] DataSourceRequest request, ViewModels.DocsVM model, int? exbId)
        {
            using (CaseXL.Data.CaseXLEntities context = new CaseXL.Data.CaseXLEntities())
            {
                CaseXL.Data.Document doc = new Data.Document()
                {
                    Doc_Type = 1,
                    Doc_Name = model.Name,
                    Doc_Url = model.Link,
                    Doc_ParentId = exbId.Value,


                };
                context.Add(doc);
                context.SaveChanges();
                model.Id = doc.Id;

                return Json(new[] { model }.ToDataSourceResult(request, ModelState));
            }
        }
        public ActionResult _DeleteDoc([DataSourceRequest] DataSourceRequest request, ViewModels.DocsVM model)
        {
            using (CaseXL.Data.CaseXLEntities context = new CaseXL.Data.CaseXLEntities())
            {
                var objToDelete = context.Documents.Where(a => a.Id == model.Id).FirstOrDefault();
                context.Delete(objToDelete);
                context.SaveChanges();
                return Json(new[] { model }.ToDataSourceResult(request, ModelState));
            }
        }
        #endregion
        #region Subscription
        #region HelpingMethods
        private IGatewayResponse CreateTransaction(SubscriptionVM model)
        {
            //4111111111111111
            //1216 date.
            var month = DateTime.Today.ToString("MMM");
            var request = new AuthorizationRequest(model.CCNumber, model.Exp_Date.Month.ToString() + model.Exp_Date.Year.ToString(), 19.00M,
                     "Transaction for the month " + month);
            var appKey = System.Configuration.ConfigurationManager.AppSettings["appKey"];
            var transid = System.Configuration.ConfigurationManager.AppSettings["transId"];
            //step 2 - create the gateway, sending in your credentials
            var gate = new Gateway(appKey, transid);
            //step 3 - make some money
            request.Email = SessionBase.User.Email;
            request.FirstName = SessionBase.User.FirstName;
            request.LastName = SessionBase.User.LastName;
            gate.TestMode = false;
            request.EmailCustomer = "true";
            request.TestRequest = "false";
            request.FooterEmailReceipt = "You will be billed each month";
            var response = gate.Send(request);

            return response;
        }
        private ARBCreateSubscriptionResponseType CreateSubscription(SubscriptionVM model)
        {

            MerchantAuthenticationType authentication = PopulateMerchantAuthentication();
            ARBSubscriptionType subscription = new ARBSubscriptionType();
            PopulateSubscription(subscription, false, model);
            ARBCreateSubscriptionResponseType response;
            response = _webservice.ARBCreateSubscription(authentication, subscription);
            if (response.resultCode == MessageTypeEnum.Ok)
            {
                _subscriptionId = response.subscriptionId;
            }
            return response;
        }
        private void PopulateSubscription(ARBSubscriptionType sub, bool bForUpdate, SubscriptionVM model)
        {
            CreditCardType creditCard = new CreditCardType();
            var user = SessionBase.User;
            sub.name = user.FirstName + " " + user.LastName;

            creditCard.cardNumber = model.CCNumber;
            var year = model.Exp_Date.ToString("yyyy") + "-" + model.Exp_Date.ToString("MM");
            creditCard.expirationDate = year;  // required format for API is YYYY-MM
            sub.payment = new PaymentType();
            sub.payment.Item = creditCard;
            sub.billTo = new NameAndAddressType();
            sub.billTo.firstName = user.FirstName;
            sub.billTo.lastName = user.LastName;
            // Create a subscription that is 12 monthly payments starting on Jan 1, 2019
            sub.paymentSchedule = new PaymentScheduleType();
            sub.paymentSchedule.startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month + 1, DateTime.Today.Day);
            sub.paymentSchedule.startDateSpecified = true;
            sub.paymentSchedule.totalOccurrences = 12;
            sub.paymentSchedule.totalOccurrencesSpecified = true;

            sub.amount = 19.00M;
            sub.amountSpecified = true;
            // Interval can't be updated once a subscription is created.
            sub.paymentSchedule.interval = new PaymentScheduleTypeInterval();
            sub.paymentSchedule.interval.length = 1;
            sub.paymentSchedule.interval.unit = ARBSubscriptionUnitEnum.months;

        }
        private bool ValidateSubscription()
        {
            var user = SessionBase.User;
            using (CaseXL.Data.CaseXLEntities context = new CaseXLEntities())
            {
                long subId = context.App_Users.Where(a => a.UserName == user.UserName).FirstOrDefault().SubscriptionNo;
                CaseXL.Auth_Service.ARBGetSubscriptionStatusResponseType result = _webservice.ARBGetSubscriptionStatus(PopulateMerchantAuthentication(), subId);
                return (result.status == ARBSubscriptionStatusEnum.active && result.resultCode != MessageTypeEnum.Error) ? true : false;
            }
        }
        private static MerchantAuthenticationType PopulateMerchantAuthentication()
        {
            MerchantAuthenticationType authentication = new MerchantAuthenticationType();
            authentication.name = System.Configuration.ConfigurationManager.AppSettings["appKey"];
            authentication.transactionKey = System.Configuration.ConfigurationManager.AppSettings["transId"];
            return authentication;
        }
        private bool AddSubscriptionNo(SubscriptionVM model, string msg, long subId)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                App_User user = (from a in entities.App_Users
                                 where a.UserName == SessionBase.User.UserName
                                 select a).FirstOrDefault<App_User>();
                if (user != null)
                {
                    user.SubscriptionNo = subId;
                    user.CreditCard = model.CCNumber;
                    user.CVNNo = model.CVNNumber;
                    user.Exp_Date = model.Exp_Date; 
                     //user.ex = model.Exp_Date,
                    

                    entities.SaveChanges();
                    return true;
                }
            }
            return false;
        }
        #endregion
        [Authorize()]
        public ActionResult ClientSubscription()
        {
            return View();

        }
        [Authorize()]
        public ActionResult SubscriptionExpired()
        {
            return View();

        }
        [HttpPost()]
        [Authorize()]
        public ActionResult ClientSubscription(ViewModels.SubscriptionVM model)
        {
            string msg = null;
            IGatewayResponse response = this.CreateTransaction(model);
            if (response.Approved)
            {
                ARBCreateSubscriptionResponseType type = this.CreateSubscription(model);
                if (type.resultCode == MessageTypeEnum.Ok)
                {
                    this.AddSubscriptionNo(model, msg, type.subscriptionId);
                    return RedirectToAction("Index", "Client");
                }
                else
                {
                    ModelState.AddModelError("", "Subscription error: " + type.messages[0].text);
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("", "Transaction error: " + response.Message);
                return View(model);
            }


        }
        public ActionResult _GetClientModule()
        {
            if (SessionBase.User.SubscriptionNo != 0)
            {
                if (ValidateSubscription())
                {
                    return RedirectToAction("Index", "Client");
                }
                else
                    return RedirectToAction("SubscriptionExpired");

            }
            return RedirectToAction("ClientSubscription");
        }
        #endregion
        private void SetfirmInSession()
        {
            using (CaseXL.Data.CaseXLEntities context = new CaseXL.Data.CaseXLEntities())
            {
                var user = context.App_Users.Where(a => a.UserName == HttpContext.User.Identity.Name).FirstOrDefault();
                if (!user.Is_Trial)
                    SessionBase.Firm = user.Firm;
                else
                {
                    var firm = new Firm()
                    {
                        Firm_Code = "0",
                        Firm_Name = "Trial_Firm",
                        ID = 0,

                    };
                    SessionBase.Firm = firm;

                }
                SessionBase.User = user;
            }
        }
        #region REfactor
        public ActionResult _FactsByWitness([DataSourceRequest] DataSourceRequest request, int? id)
        {
            if (id.HasValue)
            {
                return Json(Repository.GetFactsByWitness(id.Value), JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        public ActionResult _GetTrialinq(int? caseid)
        {
            base.Session["caseid"] = caseid;
            return base.PartialView("_Trialinq");
        }
        public ActionResult _QuestionsByFacts([DataSourceRequest] DataSourceRequest request, int? Id)
        {
            var questionsByFact = Repository.GetQuestionsByFact(Id.GetValueOrDefault());
            return Json(questionsByFact.ToDataSourceResult(request));
        }
        public ActionResult _QuestionsByFactsCreate(int? Id, [DataSourceRequest] DataSourceRequest request, FactWitnessQuestionVM model)
        {
            using (CaseXLEntities context = new CaseXLEntities())
            {
                Fact_Witness_Question entity = new Fact_Witness_Question
                {
                    Question = model.Question,
                    Answer = model.Answer,
                    Witness_Fact_Id = model.Fact_Id,
                    Asked = model.Asked,
                    Question_Number = model.Question_No
                };
                context.Add(entity);
                context.SaveChanges();
                model.Id = entity.Id;
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult _QuestionsByFactsUpdate([DataSourceRequest] DataSourceRequest request, FactWitnessQuestionVM model)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                Fact_Witness_Question question = (from a in entities.Fact_Witness_Questions
                                                  where a.Id == model.Id
                                                  select a).FirstOrDefault<Fact_Witness_Question>();
                question.Question = model.Question;
                question.Answer = model.Answer;
                question.Asked = model.Asked;
                question.Question_Number = model.Question_No;
                entities.SaveChanges();
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        public ActionResult _QuestionsFactsDelete([DataSourceRequest] DataSourceRequest request, FactWitnessQuestionVM model)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                Fact_Witness_Question entity = (from a in entities.Fact_Witness_Questions
                                                where a.Id == model.Id
                                                select a).FirstOrDefault<Fact_Witness_Question>();
                entities.Delete(entity);
                entities.SaveChanges();
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        #endregion
    }
}
