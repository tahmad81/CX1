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
            using (CaseXL.Data.CaseXLEntities context = new Data.CaseXLEntities())
            {
                var data = from cases in context.Cases
                           select new Infrastructure.ComboModelBase
                           {
                               ID = cases.ID,
                               Name = cases.Caption

                           };
                return data.ToList();
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
                                       Recepient = lnked.Exhibit.Recepient,
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
                                     Recepient = lnked.Recepient,
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
                               Recepient = quest.Recepient,
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
    }
}