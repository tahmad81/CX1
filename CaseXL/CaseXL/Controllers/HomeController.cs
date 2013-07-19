using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using CaseXL.Infrastructure;
using CaseXL.Data;
using CaseXL.Common;
using CaseXL.ViewModels;
namespace CaseXL.Controllers
{
    public class HomeController : Controller
    {
        // Methods
        public ActionResult _CreateDoc([DataSourceRequest] DataSourceRequest request, DocsVM model, int? exbId)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                Document entity = new Document
                {
                    Doc_Type = 1,
                    Doc_Name = model.Name,
                    Doc_Url = model.Link,
                    Doc_ParentId = new int?(exbId.Value)
                };
                entities.Add(entity);
                entities.SaveChanges();
                model.Id = entity.Id;
                return base.Json(QueryableExtensions.ToDataSourceResult(new DocsVM[] { model }, request, base.ModelState));
            }
        }

        public ActionResult _CreateExhibitByCase([DataSourceRequest] DataSourceRequest request, ExhibitVM model)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                Exhibit entity = new Exhibit
                {
                    Author = model.Author,
                    CaseId = new int?(int.Parse(base.Session["caseid"].ToString())),
                    Date = model.Date,
                    Description = model.Description,
                    Name = model.Name,
                    Recepient = model.Recipient,
                    Source = model.Source,
                    Type_Id = new int?(model.Type_Id)
                };
                entities.Add(entity);
                entities.SaveChanges();
                model.Exhibit_Id = entity.Id;
                model.CaseId = entity.CaseId;
            }
            return base.Json(QueryableExtensions.ToDataSourceResult(new ExhibitVM[] { model }, request, base.ModelState));
        }

        public ActionResult _CreateIssueByCase([DataSourceRequest] DataSourceRequest request, IssuesVM model)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                Issue entity = new Issue
                {
                    Issue_Name = model.Issue_Name,
                    Description = model.Description,
                    Case_ID = int.Parse(base.Session["caseid"].ToString())
                };
                entities.Add(entity);
                entities.SaveChanges();
                model.Id = entity.Id;
                model.Case_ID = entity.Case_ID;
            }
            return base.Json(QueryableExtensions.ToDataSourceResult(new IssuesVM[] { model }, request, base.ModelState));
        }

        public ActionResult _CreateWitnessByCase([DataSourceRequest] DataSourceRequest request, WitnessVM model)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                Witness entity = new Witness
                {
                    Email = model.Email,
                    Name = model.FullName,
                    CaseId = new int?(int.Parse(base.Session["caseid"].ToString()))
                };
                entities.Add(entity);
                entities.SaveChanges();
                model.WitnessId = model.ID = entity.Id;
            }
            return base.Json(QueryableExtensions.ToDataSourceResult(new WitnessVM[] { model }, request, base.ModelState));
        }

        public ActionResult _DeleteDoc([DataSourceRequest] DataSourceRequest request, DocsVM model)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                Document entity = (from a in entities.Documents
                                   where a.Id == model.Id
                                   select a).FirstOrDefault<Document>();
                entities.Delete(entity);
                entities.SaveChanges();
                return base.Json(QueryableExtensions.ToDataSourceResult(new DocsVM[] { model }, request, base.ModelState));
            }
        }

        public ActionResult _DeleteExhibitByCase([DataSourceRequest] DataSourceRequest request, ExhibitVM model)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                Exhibit entity = (from a in entities.Exhibits
                                  where a.Id == model.Exhibit_Id
                                  select a).FirstOrDefault<Exhibit>();
                entities.Delete(entity);
                entities.SaveChanges();
            }
            return base.Json(QueryableExtensions.ToDataSourceResult(new ExhibitVM[] { model }, request, base.ModelState));
        }

        public ActionResult _DeleteIssueByCase([DataSourceRequest] DataSourceRequest request, IssuesVM model)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                Issue entity = (from a in entities.Issues
                                where a.Id == model.Id
                                select a).FirstOrDefault<Issue>();
                if (entity != null)
                {
                    entities.Delete(entity);
                    entities.SaveChanges();
                }
            }
            return base.Json(QueryableExtensions.ToDataSourceResult(new IssuesVM[] { model }, request, base.ModelState));
        }

        public ActionResult _DeleteWitnessByCase([DataSourceRequest] DataSourceRequest request, WitnessVM model)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                Witness entity = (from a in entities.Witnesses
                                  where a.Id == model.WitnessId
                                  select a).FirstOrDefault<Witness>();
                entities.Delete(entity);
                entities.SaveChanges();
            }
            return base.Json(QueryableExtensions.ToDataSourceResult(new WitnessVM[] { model }, request, base.ModelState));
        }

        public ActionResult _ExbDocs([DataSourceRequest] DataSourceRequest request, int? exbId)
        {
            return base.Json(QueryableExtensions.ToDataSourceResult(Repository.GetDocuments(1, exbId.Value), request));
        }

        public ActionResult _Exhibits([DataSourceRequest] DataSourceRequest request, int? caseid, int? factID)
        {
            List<ExhibitVM> exhibitsByFactCase = Repository.GetExhibitsByFactCase(caseid.Value, factID.Value);
            return base.Json(QueryableExtensions.ToDataSourceResult(exhibitsByFactCase, request), JsonRequestBehavior.AllowGet);
        }

        public JsonResult _ExhibitsByCase([DataSourceRequest] DataSourceRequest request)
        {
            List<ExhibitVM> exhibitsByCase = Repository.GetExhibitsByCase(int.Parse(base.Session["caseid"].ToString()));
            return base.Json(QueryableExtensions.ToDataSourceResult(exhibitsByCase, request));
        }

        public ActionResult _ExhibitsLinking(int? caseid, int? factID)
        {
            ExhibitLinkingVM model = new ExhibitLinkingVM
            {
                CaseID = caseid,
                FactID = factID
            };
            return this.PartialView("_ExhibitLinking", model);
        }

        public ActionResult _FactDelete([DataSourceRequest] DataSourceRequest request, FactsVM model)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                Fact entity = (from a in entities.Facts
                               where a.Id == model.ID
                               select a).FirstOrDefault<Fact>();
                if (entity != null)
                {
                    entities.Delete(entity);
                    entities.SaveChanges();
                }
                return base.Json(QueryableExtensions.ToDataSourceResult(new FactsVM[] { model }, request, base.ModelState));
            }
        }

        public ActionResult _Facts([DataSourceRequest] DataSourceRequest request)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                List<FactsVM> list = (from facts in entities.Facts
                                      where facts.Case_Id == int.Parse(this.Session["caseid"].ToString())
                                      select new FactsVM { CaseID = facts.Case_Id, Date = facts.Date, Description = facts.Description, Evaluation = facts.Description, Source = facts.Source, ID = facts.Id, Evaluation_Id = facts.Evaluation1.Id, Evaluation_Text = facts.Evaluation1.Evaluation_Text } into o
                                      orderby o.Date
                                      select o).ToList<FactsVM>();
                return base.Json(QueryableExtensions.ToDataSourceResult(list, request));
            }
        }

        public ActionResult _FactsByWitness([DataSourceRequest] DataSourceRequest request, int? id)
        {
            if (id.HasValue)
            {
                return base.Json(Repository.GetFactsByWitness(id.Value), JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult _FactsCreate([DataSourceRequest] DataSourceRequest request, FactsVM model)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                Fact entity = new Fact();
                if (entity != null)
                {
                    entity.Evaluation_Id = new int?(model.Evaluation_Id);
                    entity.Date = model.Date;
                    entity.Source = model.Source;
                    entity.Description = model.Description;
                    entity.Case_Id = int.Parse(base.Session["caseid"].ToString());
                    entities.Add(entity);
                    entities.SaveChanges();
                    model.ID = entity.Id;
                    model.CaseID = entity.Case_Id;
                }
                return base.Json(QueryableExtensions.ToDataSourceResult(new FactsVM[] { model }, request, base.ModelState), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult _FactsEdit([DataSourceRequest] DataSourceRequest request, FactsVM model)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                Fact fact = (from a in entities.Facts
                             where a.Id == model.ID
                             select a).FirstOrDefault<Fact>();
                if (fact != null)
                {
                    fact.Evaluation_Id = new int?(model.Evaluation_Id);
                    fact.Date = model.Date;
                    fact.Source = model.Source;
                    fact.Description = model.Description;
                    entities.SaveChanges();
                }
                return base.Json(QueryableExtensions.ToDataSourceResult(new FactsVM[] { model }, request, base.ModelState));
            }
        }

        public ActionResult _GetExhibits(int? caseid)
        {
            base.Session["caseid"] = caseid;
            return base.PartialView("_Exhibits");
        }

        public ActionResult _GetFacts(int caseid)
        {
            base.Session["caseid"] = caseid;
            return base.PartialView("_Facts");
        }

        public ActionResult _GetIssues(int? caseid)
        {
            base.Session["caseid"] = caseid;
            return base.PartialView("_Issues");
        }

        public ActionResult _GetTrialinq(int? caseid)
        {
            base.Session["caseid"] = caseid;
            return base.PartialView("_Trialinq");
        }

        public ActionResult _GetWitnesses(int? caseid)
        {
            base.Session["caseid"] = caseid;
            return base.PartialView("_Witnesses");
        }

        public ActionResult _IssueLinking(int? caseid, int? factID)
        {
            IssueLinkingVM model = new IssueLinkingVM
            {
                CaseID = caseid,
                FactID = factID
            };
            return this.PartialView("_IssueLinking", model);
        }

        public ActionResult _Issues([DataSourceRequest] DataSourceRequest request, int? caseid, int? factID)
        {
            List<IssuesVM> issuesByFactCase = Repository.GetIssuesByFactCase(caseid.Value, factID.Value);
            return base.Json(QueryableExtensions.ToDataSourceResult(issuesByFactCase, request));
        }

        public ActionResult _IssuesByCase([DataSourceRequest] DataSourceRequest request)
        {
            List<IssuesVM> issuesByCases = Repository.GetIssuesByCases(int.Parse(base.Session["caseid"].ToString()));
            return base.Json(QueryableExtensions.ToDataSourceResult(issuesByCases, request));
        }

        public ActionResult _QuestionsByFacts([DataSourceRequest] DataSourceRequest request, int? Id)
        {
            List<FactWitnessQuestionVM> questionsByFact = Repository.GetQuestionsByFact(Id.GetValueOrDefault());
            return base.Json(QueryableExtensions.ToDataSourceResult(questionsByFact, request));
        }

        public ActionResult _QuestionsByFactsCreate(int? Id, [DataSourceRequest] DataSourceRequest request, FactWitnessQuestionVM model)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                Fact_Witness_Question entity = new Fact_Witness_Question();
                entity.Question = model.Question;
                entity.Answer = model.Answer;
                entity.Witness_Fact_Id = model.Witness_Id.Value;
                entity.Asked = model.Asked;
                entities.Add(entity);
                entities.SaveChanges();
                model.Id = entity.Id;
            }
            return base.Json(QueryableExtensions.ToDataSourceResult(new FactWitnessQuestionVM[] { model }, request, base.ModelState));
        }

        public ActionResult _QuestionsByFactsUpdate([DataSourceRequest] DataSourceRequest request, FactWitnessQuestionVM model)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                //ParameterExpression expression;
                Fact_Witness_Question question = entities.Fact_Witness_Questions.Where(a => a.Id == model.Id).FirstOrDefault();
                question.Question = model.Question;
                question.Answer = model.Answer;
                question.Asked = model.Asked;
                entities.SaveChanges();
            }
            return base.Json(QueryableExtensions.ToDataSourceResult(new FactWitnessQuestionVM[] { model }, request, base.ModelState));
        }

        public ActionResult _QuestionsByIssues([DataSourceRequest] DataSourceRequest request, int? issueId)
        {
            List<QuestionVM> questionsByIssues = Repository.GetQuestionsByIssues(issueId.GetValueOrDefault());
            return base.Json(QueryableExtensions.ToDataSourceResult(questionsByIssues, request));
        }

        public ActionResult _QuestionsByIssuesCreate([DataSourceRequest] DataSourceRequest request, QuestionVM model, int? issueId)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                Issue_Question entity = new Issue_Question
                {
                    Question = model.Question,
                    Answer = model.Answer,
                    Issue_Id = issueId
                };
                entities.Add(entity);
                entities.SaveChanges();
                model.Id = entity.Id;
                model.Issue_Id = issueId;
            }
            return base.Json(QueryableExtensions.ToDataSourceResult(new QuestionVM[] { model }, request, base.ModelState));
        }

        public ActionResult _QuestionsByIssuesDelete([DataSourceRequest] DataSourceRequest request, QuestionVM model)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                Issue_Question entity = (from a in entities.Issue_Questions
                                         where a.Id == model.Id
                                         select a).FirstOrDefault<Issue_Question>();
                entities.Delete(entity);
                entities.SaveChanges();
            }
            return base.Json(QueryableExtensions.ToDataSourceResult(new QuestionVM[] { model }, request, base.ModelState));
        }

        public ActionResult _QuestionsByIssuesUpdate([DataSourceRequest] DataSourceRequest request, QuestionVM model)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                Issue_Question question = (from a in entities.Issue_Questions
                                           where a.Id == model.Id
                                           select a).FirstOrDefault<Issue_Question>();
                question.Question = model.Question;
                question.Answer = model.Answer;
                entities.SaveChanges();
            }
            return base.Json(QueryableExtensions.ToDataSourceResult(new QuestionVM[] { model }, request, base.ModelState));
        }

        public ActionResult _QuestionsByWitness([DataSourceRequest] DataSourceRequest request, int? witnessId)
        {
            List<QuestionVM> questionsByWitness = Repository.GetQuestionsByWitness(witnessId.GetValueOrDefault());
            return base.Json(QueryableExtensions.ToDataSourceResult(questionsByWitness, request));
        }

        public ActionResult _QuestionsByWitnessCreate([DataSourceRequest] DataSourceRequest request, QuestionVM model, int? witnessId)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                Question entity = new Question
                {
                    Question1 = model.Question,
                    Answer = model.Answer,
                    Witness_Id = witnessId
                };
                entities.Add(entity);
                entities.SaveChanges();
                model.Id = entity.Id;
                model.Witness_Id = witnessId;
            }
            return base.Json(QueryableExtensions.ToDataSourceResult(new QuestionVM[] { model }, request, base.ModelState));
        }

        public ActionResult _QuestionsByWitnessDelete([DataSourceRequest] DataSourceRequest request, QuestionVM model)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                Question entity = (from a in entities.Questions
                                   where a.Id == model.Id
                                   select a).FirstOrDefault<Question>();
                entities.Delete(entity);
                entities.SaveChanges();
            }
            return base.Json(QueryableExtensions.ToDataSourceResult(new QuestionVM[] { model }, request, base.ModelState));
        }

        public ActionResult _QuestionsByWitnessUpdate([DataSourceRequest] DataSourceRequest request, QuestionVM model)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                Question question = (from a in entities.Questions
                                     where a.Id == model.Id
                                     select a).FirstOrDefault<Question>();
                question.Question1 = model.Question;
                question.Answer = model.Answer;
                entities.SaveChanges();
            }
            return base.Json(QueryableExtensions.ToDataSourceResult(new QuestionVM[] { model }, request, base.ModelState));
        }

        public ActionResult _QuestionsFactsDelete([DataSourceRequest] DataSourceRequest request, FactWitnessQuestionVM model)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                Fact_Witness_Question entity = entities.Fact_Witness_Questions.Where(a => a.Id == model.Id).FirstOrDefault();
                entities.Delete(entity);
                entities.SaveChanges();
            }
            return base.Json(QueryableExtensions.ToDataSourceResult(new FactWitnessQuestionVM[] { model }, request, base.ModelState));
        }

        public ActionResult _UpdateExhibitByCase([DataSourceRequest] DataSourceRequest request, ExhibitVM model)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                Exhibit entity = (from a in entities.Exhibits
                                  where a.Id == model.Exhibit_Id
                                  select a).FirstOrDefault<Exhibit>();
                entity.Author = model.Author;
                entity.CaseId = new int?(int.Parse(base.Session["caseid"].ToString()));
                entity.Date = model.Date;
                entity.Description = model.Description;
                entity.Name = model.Name;
                entity.Recepient = model.Recipient;
                entity.Source = model.Source;
                entity.Type_Id = new int?(model.Type_Id);
                entities.Add(entity);
                entities.SaveChanges();
                model.Exhibit_Id = entity.Id;
            }
            return base.Json(QueryableExtensions.ToDataSourceResult(new ExhibitVM[] { model }, request, base.ModelState));
        }

        public ActionResult _UpdateExhibits(int? factID, int? caseid, [DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<ExhibitVM> updated)
        {
            Func<ExhibitVM, Exhibits_Fact> selector = null;
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                List<int?> delObjects = (from a in updated
                                         where !a.IsSelected && a.Fact_Exhibit.HasValue
                                         select a.Fact_Exhibit).ToList<int?>();
                entities.Delete(from a in entities.Exhibits_Facts
                                              where delObjects.Contains(a.Id)
                                              select a);
                if (selector == null)
                {
                    selector = addobj => new Exhibits_Fact { CaseId = caseid, FactId = factID, ExhibitId = new int?(addobj.Exhibit_Id) };
                }
                List<Exhibits_Fact> list = (from a in updated
                                            where a.IsSelected && !a.Fact_Exhibit.HasValue
                                            select a).Select<ExhibitVM, Exhibits_Fact>(selector).ToList<Exhibits_Fact>();
                entities.Add(list);
                entities.SaveChanges();
            }
            return base.Json(true);
        }

        public ActionResult _UpdateIssueByCase([DataSourceRequest] DataSourceRequest request, IssuesVM model)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                Issue issue = (from a in entities.Issues
                               where a.Id == model.Id
                               select a).FirstOrDefault<Issue>();
                issue.Issue_Name = model.Issue_Name;
                issue.Description = model.Description;
                entities.SaveChanges();
            }
            return base.Json(QueryableExtensions.ToDataSourceResult(new IssuesVM[] { model }, request, base.ModelState));
        }

        public ActionResult _UpdateIssues(int? factID, int? caseid, [DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<IssuesVM> updated)
        {
            Func<IssuesVM, Issues_Fact> selector = null;
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                List<int?> delObjects = (from a in updated
                                         where !a.IsSelected
                                         select a.Issue_Fact_Id).ToList<int?>();
                entities.Delete(from a in entities.Issues_Facts
                                              where delObjects.Contains(a.Id)
                                              select a);
                if (selector == null)
                {
                    selector = addobj => new Issues_Fact { Case_Id = caseid, Fact_Id = factID, Issue_Id = new int?(addobj.Id) };
                }
                List<Issues_Fact> list = (from a in updated
                                          where a.IsSelected && !a.Issue_Fact_Id.HasValue
                                          select a).Select<IssuesVM, Issues_Fact>(selector).ToList<Issues_Fact>();
                entities.Add(list);
                entities.SaveChanges();
            }
            return base.Json(true);
        }

        public ActionResult _UpdateWitnesses(int? factID, int? caseid, [DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<WitnessVM> updated)
        {
            Func<WitnessVM, Witness_Fact> selector = null;
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                List<int> delObjects = (from a in updated
                                        where !a.IsSelected && a.Issue_Witness_Id.HasValue
                                        select a.ID).ToList<int>();
                entities.Delete(from a in entities.Witness_Facts
                                              where delObjects.Contains(a.Id)
                                              select a);
                if (selector == null)
                {
                    selector = addobj => new Witness_Fact { CaseId = caseid, FactId = factID, WitnessId = new int?(addobj.WitnessId) };
                }
                List<Witness_Fact> list = (from a in updated
                                           where a.IsSelected && !a.Issue_Witness_Id.HasValue
                                           select a).Select<WitnessVM, Witness_Fact>(selector).ToList<Witness_Fact>();
                entities.Add(list);
                entities.SaveChanges();
            }
            return base.Json(true);
        }

        public ActionResult _UpdateWitnessesByCase([DataSourceRequest] DataSourceRequest request, WitnessVM model)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                Witness witness = (from a in entities.Witnesses
                                   where a.Id == model.WitnessId
                                   select a).FirstOrDefault<Witness>();
                witness.Email = model.Email;
                witness.Name = model.FullName;
                entities.SaveChanges();
            }
            return base.Json(QueryableExtensions.ToDataSourceResult(new WitnessVM[] { model }, request, base.ModelState));
        }

        public ActionResult _Witnesses([DataSourceRequest] DataSourceRequest request, int? caseid, int? factID)
        {
            List<WitnessVM> witnessesByCaseFact = Repository.GetWitnessesByCaseFact(caseid.Value, factID.Value);
            return base.Json(QueryableExtensions.ToDataSourceResult(witnessesByCaseFact, request));
        }

        public ActionResult _WitnessesByCase([DataSourceRequest] DataSourceRequest request)
        {
            List<WitnessVM> witnessesByCase = Repository.GetWitnessesByCase(int.Parse(base.Session["caseid"].ToString()));
            return base.Json(QueryableExtensions.ToDataSourceResult(witnessesByCase, request));
        }

        public ActionResult _WitnessLinking(int? caseid, int? factID)
        {
            WitnessesLinkingVM model = new WitnessesLinkingVM
            {
                CaseID = caseid,
                FactID = factID
            };
            return this.PartialView("_WitnessesLinking", model);
        }

        public ActionResult About()
        {
            return base.View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Checkboxes(string[] checkedNodes, List<IssuesVM> model)
        {
            return null;
        }

        public ActionResult Index()
        {
            ((dynamic)base.ViewBag).Message = "Welcome to ASP.NET MVC!";
            return base.View();
        }

        [Authorize]
        public ActionResult Main(int? caseID, string msg)
        {
            this.SetfirmInSession();
            List<ComboModelBase> cases = Repository.GetCases();
            base.Session["caseid"] = caseID;
            ((dynamic)base.ViewBag).Cases = new SelectList(cases, "ID", "Name", caseID.GetValueOrDefault());
            return base.View();
        }

        private void SetfirmInSession()
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                SessionBase.Firm = (from a in entities.App_Users
                                    where a.UserName == this.HttpContext.User.Identity.Name
                                    select a).FirstOrDefault<App_User>().Firm;
            }
        }

        public ActionResult Test(int? id)
        {
            return null;
        }
    }





}
