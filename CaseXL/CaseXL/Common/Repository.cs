using CaseXL.Data;
using CaseXL.Infrastructure;
using CaseXL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaseXL.Common
{
    public class Repository
    {
        public static List<Infrastructure.ComboModelBase> GetPartners(int firmID)
        {
            using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
            {
                var data = (from prt in context.Partners.Where(a => a.Firm_ID == firmID)
                            select new Infrastructure.ComboModelBase
                            {
                                ID = prt.ID,
                                Name = prt.FristName.Trim() + " " + prt.LastName.Trim()
                            }).ToList();
                data.Insert(0, new Infrastructure.ComboModelBase() { Name = "Please Select" });
                return data.ToList();
            }
        }
        public static List<ViewModels.IssuesVM> GetAllCasesWithIssues()
        {
            using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
            {
                var data = from issue in context.Issues
                           select new ViewModels.IssuesVM
                           {
                               //Case_ID = issue.Case.ID,
                               //Case_Name = issue.Case.Caption,
                               //Issue_Exp = issue.Issue_Expl,
                               //Case_Type = issue.Case.ID,
                               Issue_Name = issue.Issue_Name
                           };
                return data.ToList();
            }
        }
        public static List<ViewModels.IssuesVM> GetIssuesByFactCase(int caseid, int factID)
        {
            using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
            {
                var lnkedIssues = (from lnked in context.Issues_Facts.Where(a => a.Case_Id == caseid && a.Fact_Id == factID)
                                   select new ViewModels.IssuesVM
                                   {
                                       Id = lnked.Issue_Id.Value,
                                       Issue_Fact_Id = lnked.Id,
                                       Case_ID = lnked.Case_Id.GetValueOrDefault(),
                                       IsSelected = true,
                                       Issue_Name = lnked.Issue.Issue_Name,
                                       FactID = lnked.Fact_Id
                                   }).ToList();
                var iids = lnkedIssues.Select(a => a.Id).ToArray();
                var allIssues = (from issues in context.Issues.Where(aa => aa.Case_ID == caseid && !iids.Contains(aa.Id))
                                 select new ViewModels.IssuesVM
                                 {
                                     Id = issues.Id,
                                     Case_ID = issues.Case_ID,
                                     Issue_Name = issues.Issue_Name,
                                     IsSelected = false,
                                     FactID = issues.Fact_Id
                                 }).ToList();
                lnkedIssues.AddRange(allIssues);
                return lnkedIssues;
            }
        }
        public static List<ViewModels.IssuesVM> GetIssuesByCases(int caseId)
        {
            using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
            {
                var data = from issue in context.Issues.Where(a => a.Case_ID == caseId)
                           select new ViewModels.IssuesVM
                           {
                               // Case_ID = issue.Case.ID,
                               Id = issue.Id,
                               // Case_Name = issue.Case.Caption,
                               // Issue_Exp = issue.Issue_Expl,
                               // Case_Type = issue.Case.ID,
                               Issue_Name = issue.Issue_Name,
                               IsSelected = true,
                               Description = issue.Description
                               //Issue_Evaluation = issue.Evaluation,
                               //Issue_Question = issue.Question,
                           };
                return data.ToList();
            }
        }
        public static List<Infrastructure.ComboModelBase> GetCases()
        {
            if (SessionBase.User.Is_Client)
            {
                using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
                {

                    var data = from cases in context.Client_Cases.Where(a => a.ClientId == SessionBase.User.Id)
                               select new Infrastructure.ComboModelBase
                               {
                                   ID = cases.Case.ID,
                                   Name = cases.Case.Caption

                               };
                    return data.ToList();
                }
            }
            else
            {
                using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
                {

                    var data = from cases in context.Cases
                               where cases.Firm_ID == Infrastructure.SessionBase.Firm.ID
                               select new Infrastructure.ComboModelBase
                               {
                                   ID = cases.ID,
                                   Name = cases.Caption

                               };
                    return data.ToList();
                }
            }

        }
        public static List<Infrastructure.ComboModelBase> GetCaseType()
        {
            using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
            {
                var data = from cases in context.Case_Types
                           select new Infrastructure.ComboModelBase
                           {
                               ID = cases.ID,
                               Name = cases.Name
                           };
                return data.ToList();
            }

        }
        public static List<ViewModels.FactsVM> GetFacts(int caseid)
        {
            using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
            {
                var data = from facts in context.Facts.Where(a => a.Case_Id == caseid)
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
                           };
                return data.ToList();
            }
        }
        public static List<ViewModels.WitnessVM> GetWitnessesByCaseFact(int caseid, int factID)
        {
            using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
            {
                var lnkedwitnesses = (from lnked in context.Witness_Facts.Where(a => a.CaseId == caseid && a.FactId == factID)
                                      select new ViewModels.WitnessVM
                                      {
                                          ID = lnked.Id,
                                          IsSelected = true,
                                          Email = lnked.Witness.Email,
                                          FullName = lnked.Witness.Name,
                                          WitnessId = lnked.Witness.Id,
                                          Issue_Witness_Id = lnked.Id,
                                      }).ToList();
                var iids = lnkedwitnesses.Select(a => a.WitnessId).ToArray();
                var allwitness = (from lnked in context.Witnesses.Where(aa => aa.CaseId == caseid && !iids.Contains(aa.Id))
                                  select new ViewModels.WitnessVM
                                  {
                                      ID = lnked.Id,
                                      Email = lnked.Email,
                                      FullName = lnked.Name,
                                      WitnessId = lnked.Id
                                  }).ToList();
                lnkedwitnesses.AddRange(allwitness);
                return lnkedwitnesses;
            }

        }
        public static List<ViewModels.ExhibitVM> GetExhibitsByFactCase(int caseid, int factID)
        {
            using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
            {

                var lnkedIssues = (from lnked in context.Exhibits_Facts.Where(a => a.CaseId == caseid && a.FactId == factID)
                                   select new ViewModels.ExhibitVM
                                   {
                                       IsSelected = true,
                                       Author = lnked.Exhibit.Author,
                                       Description = lnked.Exhibit.Description,
                                       Name = lnked.Exhibit.Name,
                                       Recipient = lnked.Exhibit.Recepient,
                                       Source = lnked.Exhibit.Source,
                                       Type = lnked.Exhibit.Type,
                                       Exhibit_Id = lnked.Exhibit.Id,
                                       Fact_Exhibit = lnked.Id,
                                       Date = lnked.Exhibit.Date,

                                   }).ToList();
                var iids = lnkedIssues.Select(a => a.Exhibit_Id).ToArray();
                var allIssues = (from lnked in context.Exhibits.Where(aa => aa.CaseId == caseid && !iids.Contains(aa.Id))
                                 select new ViewModels.ExhibitVM
                                 {
                                     Author = lnked.Author,
                                     Description = lnked.Description,
                                     Name = lnked.Name,
                                     Recipient = lnked.Recepient,
                                     Source = lnked.Source,
                                     Type = lnked.Type,
                                     Exhibit_Id = lnked.Id,
                                     Date = lnked.Date
                                 }).ToList();
                lnkedIssues.AddRange(allIssues);
                return lnkedIssues;

            }
        }
        public static List<Infrastructure.ComboModelBase> GetEvaluations()
        {
            using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
            {
                var data = (from eva in context.Evaluations
                            select
                                new Infrastructure.ComboModelBase
                                {
                                    ID = eva.Id,
                                    Name = eva.Evaluation_Text

                                }).ToList();
                data.Insert(0, new Infrastructure.ComboModelBase() { Name = "Please Select" });
                return data.ToList();
            }
        }
        public static List<ViewModels.WitnessVM> GetWitnessesByCase(int caseid)
        {
            using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
            {
                var allwitness = (from lnked in context.Witnesses.Where(aa => aa.CaseId == caseid)
                                  select new ViewModels.WitnessVM
                                  {
                                      ID = lnked.Id,
                                      Email = lnked.Email,
                                      FullName = lnked.Name,
                                      WitnessId = lnked.Id,
                                      Docs_Authored = lnked.Docs_Authored,
                                      Docs_Received = lnked.Docs_Received
                                  }).ToList();
                return allwitness;
            }
        }
        public static List<ViewModels.QuestionVM> GetQuestionsByWitness(int witnessId)
        {
            using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
            {
                var data = from quest in context.Questions.Where(a => a.Witness_Id == witnessId)
                           select new ViewModels.QuestionVM
                           {
                               Id = quest.Id,
                               Answer = quest.Answer,
                               Question = quest.Question1,
                               Witness_Id = quest.Witness_Id
                           };
                return data.ToList();
            }
        }
        public static List<ViewModels.ExhibitVM> GetExhibitsByCase(int caseId)
        {
            using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
            {
                var data = from quest in context.Exhibits.Where(a => a.CaseId == caseId)
                           select new ViewModels.ExhibitVM
                           {
                               Exhibit_Id = quest.Id,
                               Author = quest.Author,
                               Date = quest.Date,
                               Description = quest.Description,
                               Name = quest.Name,
                               Recipient = quest.Recepient,
                               Source = quest.Source,
                               Type = quest.Exhibits_Type.Typer,
                               Type_Id = quest.Exhibits_Type.Id
                           };
                return data.ToList();
            }

        }
        public static List<Infrastructure.ComboModelBase> GetExhibitTypes()
        {
            using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
            {
                var data = (from prt in context.Exhibits_Types
                            select new Infrastructure.ComboModelBase
                            {
                                ID = prt.Id,
                                Name = prt.Typer
                            }).ToList();
                data.Insert(0, new Infrastructure.ComboModelBase() { Name = "Please Select" });
                return data.ToList();

            }
        }
        public static List<ViewModels.QuestionVM> GetQuestionsByIssues(int issueId)
        {
            using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
            {
                var data = from quest in context.Issue_Questions.Where(a => a.Issue_Id == issueId)
                           select new ViewModels.QuestionVM
                           {
                               Id = quest.Id,
                               Answer = quest.Answer,
                               Question = quest.Question,
                               Issue_Id = quest.Issue_Id
                           };
                return data.ToList();
            }
        }
        #region Refactor
        public static List<DocsVM> GetDocuments(int type, int parentId)
        {
            using (CaseXLEntities context = new CaseXLEntities())
            {
                return (from docs in context.Documents
                        where (docs.Doc_Type == type) && (docs.Doc_ParentId == parentId)
                        select new DocsVM { Id = docs.Id, Link = docs.Doc_Url, Name = docs.Doc_Name, Parent_Id = docs.Doc_ParentId.Value }).ToList<DocsVM>();
            }
        }
        public static List<ComboModelBase> GetWitnesses()
        {
            using (CaseXLEntities context = new CaseXLEntities())
            {
                var data = (from a in
                                (from witness in context.Witness_Facts
                                 where witness.CaseId == SessionBase.CaseId
                                 select new { ID = witness.Witness.Id, Name = witness.Witness.Name }).Distinct()
                            select new ComboModelBase { ID = a.ID, Name = a.Name }).ToList();
                return data;
            }
        }

        public static List<ComboModelBase> GetFactsByWitness(int witnessid)
        {
            using (CaseXLEntities context = new CaseXLEntities())
            {
                //ParameterExpression expression;
                var data = (from a in context.Witness_Facts
                            where a.WitnessId == witnessid
                            select new Infrastructure.ComboModelBase
                            {
                                ID = a.Id,
                                Name = a.Fact.Description
                            }).ToList();
                return data;
            }

        }

        public static List<ComboModelBase> CaseTypes()
        {
            List<ComboModelBase> list = new List<ComboModelBase>();
            ComboModelBase item = new ComboModelBase
            {
                Name = "Select Case Type"
            };
            list.Add(item);
            ComboModelBase base3 = new ComboModelBase
            {
                ID = 1,
                Name = "Civil"
            };
            list.Add(base3);
            ComboModelBase base4 = new ComboModelBase
            {
                ID = 2,
                Name = "Criminal"
            };
            list.Add(base4);
            ComboModelBase base5 = new ComboModelBase
            {
                ID = 3,
                Name = "Adminstrative"
            };
            list.Add(base5);
            return list;
        }
        public static List<CaseVM> GetCaseDetails()
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                if (SessionBase.Firm.ID != 0)
                {

                    return (from cases in entities.Cases
                            where cases.Firm_ID == SessionBase.Firm.ID
                            select new CaseVM { Case_Id = cases.ID, Caption = cases.Caption, Case_Number = cases.Case_Number, Case_Type = cases.Case_Type_ID.Value }).ToList();
                }
                else
                {
                    var data = (from cases in entities.Cases
                                where cases.Firm_ID == Infrastructure.SessionBase.User.Id
                                select new CaseVM { Case_Id = cases.ID, Caption = cases.Caption, Case_Number = cases.Case_Number, Case_Type = cases.Case_Type_ID.Value }).ToList();
                    return data.ToList();
                }
            }


        }
        public static List<FactWitnessQuestionVM> GetQuestionsByFact(int Id)
        {
            using (CaseXLEntities context = new CaseXLEntities())
            {
                return (from quest in context.Fact_Witness_Questions
                        where quest.Witness_Fact_Id == Id
                        select new FactWitnessQuestionVM { Id = quest.Id, Answer = quest.Answer, Question = quest.Question, Asked = quest.Asked, Question_No = quest.Question_Number.GetValueOrDefault() }).ToList();
            }
        }




        #endregion
        #region WebAdmin
        public static List<ViewModels.AppUserVM> GetTrialUsers()
        {
            using (CaseXL.Data.CaseXLEntities context = new CaseXLEntities())
            {
                var data = from users in context.App_Users
                           select new ViewModels.AppUserVM()
                           {
                               Id = users.Id,
                               Full_Name = users.FirstName + " " + users.LastName,
                               Email = users.Email,
                               Signup_Date = users.Signupdate
                           };
                return data.ToList();
            }

        }
        public static List<ViewModels.ClientVM> GetClients()
        {
            using (CaseXL.Data.CaseXLEntities context = new CaseXLEntities())
            {
                var data = from users in context.App_Users.Where(a => a.Is_Client && a.FirmId == SessionBase.Firm.ID)
                           select new ViewModels.ClientVM()
                           {
                               Id = users.Id,
                               UserName = users.UserName,
                               Email = users.Email,
                               First_Name = users.FirstName,
                               Last_Name = users.LastName,

                           };
                return data.ToList();
            }

        }
        public static List<ViewModels.CaseVM> GetCasesByClient(int userid)
        {
            using (CaseXLEntities context = new CaseXLEntities())
            {

                var usercases = (from cases in context.Client_Cases.Where(a => a.ClientId == userid)
                                 select new ViewModels.CaseVM
                                 {
                                     ID = cases.Id,
                                     Caption = cases.Case.Caption,
                                     Case_Number = cases.Case.Case_Number,
                                     IsSelected = true,
                                     CaseType = CaseTypes().Where(a => a.ID == cases.Case.Case_Type_ID).Select(a => a.Name).FirstOrDefault(),
                                     CaseClientId = cases.Id,
                                     Case_Id = cases.Case.ID

                                 }).ToList();
                var ids = usercases.Select(a => a.Case_Id).ToList();
                var allcases = (from cases in context.Cases.Where(a => a.Firm_ID == SessionBase.Firm.ID && !ids.Contains(a.ID))
                                select new ViewModels.CaseVM
                                {
                                    ID = cases.ID,
                                    Caption = cases.Caption,
                                    Case_Number = cases.Case_Number,
                                    IsSelected = false,
                                    CaseType = CaseTypes().Where(a => a.ID == cases.Case_Type_ID).Select(a => a.Name).FirstOrDefault(),
                                    Case_Id = cases.ID
                                }).ToList();

                usercases.AddRange(allcases);
                return usercases;

            }
        }
        #endregion
    }
}