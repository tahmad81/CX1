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
        // Methods
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

        public static List<IssuesVM> GetAllCasesWithIssues()
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                return (from issue in entities.Issues select new IssuesVM { Issue_Name = issue.Issue_Name }).ToList<IssuesVM>();
            }
        }

        public static List<CaseVM> GetCaseDetails()
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                return (from cases in entities.Cases
                        where cases.Firm_ID == SessionBase.Firm.ID
                        select new CaseVM { Case_Id = cases.ID, Caption = cases.Caption, Case_Number = cases.Case_Number, Case_Type = cases.Case_Type_ID.Value }).ToList<CaseVM>();
            }
        }

        public static List<ComboModelBase> GetCases()
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                return (from cases in entities.Cases
                        where cases.Firm_ID == SessionBase.Firm.ID
                        select new ComboModelBase { ID = cases.ID, Name = cases.Caption }).ToList<ComboModelBase>();
            }
        }

        public static List<ComboModelBase> GetCaseType()
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                return (from cases in entities.Case_Types select new ComboModelBase { ID = cases.ID, Name = cases.Name }).ToList<ComboModelBase>();
            }
        }

        public static List<DocsVM> GetDocuments(int type, int parentId)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                return (from docs in entities.Documents
                        where (docs.Doc_Type == type) && (docs.Doc_ParentId == parentId)
                        select new DocsVM { Id = docs.Id, Link = docs.Doc_Url, Name = docs.Doc_Name, Parent_Id = docs.Doc_ParentId.Value }).ToList<DocsVM>();
            }
        }

        public static List<ComboModelBase> GetEvaluations()
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                List<ComboModelBase> source = (from eva in entities.Evaluations select new ComboModelBase { ID = eva.Id, Name = eva.Evaluation_Text }).ToList<ComboModelBase>();
                ComboModelBase item = new ComboModelBase
                {
                    Name = "Please Select"
                };
                source.Insert(0, item);
                return source.ToList<ComboModelBase>();
            }
        }

        public static List<ExhibitVM> GetExhibitsByCase(int caseId)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                return (from quest in entities.Exhibits
                        where quest.CaseId == caseId
                        select new ExhibitVM { Exhibit_Id = quest.Id, Author = quest.Author, Date = quest.Date, Description = quest.Description, Name = quest.Name, Recipient = quest.Recepient, Source = quest.Source, Type = quest.Exhibits_Type.Typer, Type_Id = quest.Exhibits_Type.Id }).ToList<ExhibitVM>();
            }
        }

        public static List<ExhibitVM> GetExhibitsByFactCase(int caseid, int factID)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                List<ExhibitVM> list = (from lnked in entities.Exhibits_Facts
                                        where (lnked.CaseId == caseid) && (lnked.FactId == factID)
                                        select new ExhibitVM { IsSelected = true, Author = lnked.Exhibit.Author, Description = lnked.Exhibit.Description, Name = lnked.Exhibit.Name, Recipient = lnked.Exhibit.Recepient, Source = lnked.Exhibit.Source, Type = lnked.Exhibit.Type, Exhibit_Id = lnked.Exhibit.Id, Fact_Exhibit = lnked.Id, Date = lnked.Exhibit.Date }).ToList<ExhibitVM>();
                int[] iids = (from a in list select a.Exhibit_Id).ToArray<int>();
                List<ExhibitVM> collection = (from lnked in entities.Exhibits
                                              where (lnked.CaseId == caseid) && !iids.Contains<int>(lnked.Id)
                                              select new ExhibitVM { Author = lnked.Author, Description = lnked.Description, Name = lnked.Name, Recipient = lnked.Recepient, Source = lnked.Source, Type = lnked.Type, Exhibit_Id = lnked.Id, Date = lnked.Date }).ToList<ExhibitVM>();
                list.AddRange(collection);
                return list;
            }
        }

        public static List<ComboModelBase> GetExhibitTypes()
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                List<ComboModelBase> source = (from prt in entities.Exhibits_Types select new ComboModelBase { ID = prt.Id, Name = prt.Typer }).ToList<ComboModelBase>();
                ComboModelBase item = new ComboModelBase
                {
                    Name = "Please Select"
                };
                source.Insert(0, item);
                return source.ToList<ComboModelBase>();
            }
        }

        public static List<FactsVM> GetFacts(int caseid)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                return (from facts in entities.Facts
                        where facts.Case_Id == caseid
                        select new FactsVM { CaseID = facts.Case_Id, Date = facts.Date, Description = facts.Description, Evaluation = facts.Description, Source = facts.Source, ID = facts.Id, Evaluation_Id = facts.Evaluation1.Id, Evaluation_Text = facts.Evaluation1.Evaluation_Text }).ToList<FactsVM>();
            }
        }

        public static List<ComboModelBase> GetFactsByWitness(int witnessid)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                //ParameterExpression expression;
                //return (from a in entities.Witness_Facts
                //    where a.WitnessId == witnessid
                //    select a).Select<Witness_Fact, ComboModelBase>(Expression.Lambda<Func<Witness_Fact, ComboModelBase>>(Expression.MemberInit(Expression.New((ConstructorInfo) methodof(ComboModelBase..ctor), new Expression[0]), new MemberBinding[] { Expression.Bind((MethodInfo) methodof(ComboModelBase.set_ID), Expression.Convert(Expression.Property(expression = Expression.Parameter(typeof(Witness_Fact), "fact"), (MethodInfo) methodof(Witness_Fact.get_Id)), typeof(int?))), Expression.Bind((MethodInfo) methodof(ComboModelBase.set_Name), Expression.Property(Expression.Property(expression, (MethodInfo) methodof(Witness_Fact.get_Fact)), (MethodInfo) methodof(Fact.get_Description))) }), new ParameterExpression[] { expression })).ToList<ComboModelBase>();
                return null;
            }
        }

        public static List<IssuesVM> GetIssuesByCases(int caseId)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                return (from issue in entities.Issues
                        where issue.Case_ID == caseId
                        select new IssuesVM { Id = issue.Id, Issue_Name = issue.Issue_Name, IsSelected = true, Description = issue.Description }).ToList<IssuesVM>();
            }
        }

        public static List<IssuesVM> GetIssuesByFactCase(int caseid, int factID)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                List<IssuesVM> list = (from lnked in entities.Issues_Facts
                                       where (lnked.Case_Id == caseid) && (lnked.Fact_Id == factID)
                                       select new IssuesVM { Id = lnked.Issue_Id.Value, Issue_Fact_Id = lnked.Id, Case_ID = lnked.Case_Id.GetValueOrDefault(), IsSelected = true, Issue_Name = lnked.Issue.Issue_Name, FactID = lnked.Fact_Id }).ToList<IssuesVM>();
                int[] iids = (from a in list select a.Id).ToArray<int>();
                List<IssuesVM> collection = (from issues in entities.Issues
                                             where (issues.Case_ID == caseid) && !iids.Contains<int>(issues.Id)
                                             select new IssuesVM { Id = issues.Id, Case_ID = issues.Case_ID, Issue_Name = issues.Issue_Name, IsSelected = false, FactID = issues.Fact_Id }).ToList<IssuesVM>();
                list.AddRange(collection);
                return list;
            }
        }


        public static List<FactWitnessQuestionVM> GetQuestionsByFact(int Id)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {

                //return entities.get_Fact_Witness_Questions().Where<Fact_Witness_Question>(Expression.Lambda<Func<Fact_Witness_Question, bool>>(Expression.Equal(Expression.Property(expression = Expression.Parameter(typeof(Fact_Witness_Question), "a"), (MethodInfo) methodof(Fact_Witness_Question.get_Witness_Fact_Id)), Expression.Constant(Id)), new ParameterExpression[] { expression })).Select<Fact_Witness_Question, FactWitnessQuestionVM>(Expression.Lambda<Func<Fact_Witness_Question, FactWitnessQuestionVM>>(Expression.MemberInit(Expression.New((ConstructorInfo) methodof(FactWitnessQuestionVM..ctor), new Expression[0]), new MemberBinding[] { Expression.Bind((MethodInfo) methodof(QuestionVM.set_Id), Expression.Property(expression = Expression.Parameter(typeof(Fact_Witness_Question), "quest"), (MethodInfo) methodof(Fact_Witness_Question.get_Id))), Expression.Bind((MethodInfo) methodof(QuestionVM.set_Answer), Expression.Property(expression, (MethodInfo) methodof(Fact_Witness_Question.get_Answer))), Expression.Bind((MethodInfo) methodof(QuestionVM.set_Question), Expression.Property(expression, (MethodInfo) methodof(Fact_Witness_Question.get_Question))), Expression.Bind((MethodInfo) methodof(FactWitnessQuestionVM.set_Asked), Expression.Property(expression, (MethodInfo) methodof(Fact_Witness_Question.get_Asked))) }), new ParameterExpression[] { expression })).ToList<FactWitnessQuestionVM>();
                return null;
            }
        }

        public static List<QuestionVM> GetQuestionsByIssues(int issueId)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                return (from quest in entities.Issue_Questions
                        where quest.Issue_Id == issueId
                        select new QuestionVM { Id = quest.Id, Answer = quest.Answer, Question = quest.Question, Issue_Id = quest.Issue_Id }).ToList<QuestionVM>();
            }
        }

        public static List<QuestionVM> GetQuestionsByWitness(int witnessId)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                return (from quest in entities.Questions
                        where quest.Witness_Id == witnessId
                        select new QuestionVM { Id = quest.Id, Answer = quest.Answer, Question = quest.Question1, Witness_Id = quest.Witness_Id }).ToList<QuestionVM>();
            }
        }

        public static List<ComboModelBase> GetWitnesses()
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                return (from a in
                            (from witness in entities.Witness_Facts
                             where witness.CaseId == SessionBase.CaseId
                             select new { ID = witness.Witness.Id, Name = witness.Witness.Name }).Distinct()
                        select new ComboModelBase { ID = a.ID, Name = a.Name }).ToList<ComboModelBase>();
            }
        }

        public static List<WitnessVM> GetWitnessesByCase(int caseid)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                return (from lnked in entities.Witnesses
                        where lnked.CaseId == caseid
                        select new WitnessVM { ID = lnked.Id, Email = lnked.Email, FullName = lnked.Name, WitnessId = lnked.Id }).ToList<WitnessVM>();
            }
        }

        public static List<WitnessVM> GetWitnessesByCaseFact(int caseid, int factID)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                List<WitnessVM> list = (from lnked in entities.Witness_Facts
                                        where (lnked.CaseId == caseid) && (lnked.FactId == factID)
                                        select new WitnessVM { ID = lnked.Id, IsSelected = true, Email = lnked.Witness.Email, FullName = lnked.Witness.Name, WitnessId = lnked.Witness.Id, Issue_Witness_Id = lnked.Id }).ToList<WitnessVM>();
                int[] iids = (from a in list select a.WitnessId).ToArray<int>();
                List<WitnessVM> collection = (from lnked in entities.Witnesses
                                              where (lnked.CaseId == caseid) && !iids.Contains<int>(lnked.Id)
                                              select new WitnessVM { ID = lnked.Id, Email = lnked.Email, FullName = lnked.Name, WitnessId = lnked.Id }).ToList<WitnessVM>();
                list.AddRange(collection);
                return list;
            }
        }
    }





}