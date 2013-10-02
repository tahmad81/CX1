using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using CaseXL.Models;
//using SafetyPlus.Entities;
using System.Data.Entity;
using Telerik.OpenAccess.FetchOptimization;
using AuthorizeNet;
using CaseXL.Auth_Service;
using CaseXL.Infrastructure;
using CaseXL.Data;
using CaseXL.Common;
namespace SafetyPlus.WebUI_WebAPI.Controllers
{
    public class AccountController : Controller
    {

        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }
        public CaseXL.Auth_Service.ServiceSoapClient _webservice = new ServiceSoapClient();
        private long _subscriptionId = 0;
        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }
            base.Initialize(requestContext);
        }

        // **************************************
        // URL: /Account/LogOn
        // **************************************

        public ActionResult LogOn()
        {
            return View();
        }
        [HttpPost]

        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ValidateUser(model.UserName, model.Password))
                {
                    //if (IsTrial(model))
                    //{
                    //    if (IsTrialValid(model))
                    //    {
                    //        FormsService.SignIn(model.UserName, model.RememberMe);
                    //        return RedirectToAction("Main", "Home");
                    //    }
                    //    else
                    //    {
                    //        ModelState.AddModelError("", "Dear user we are sorry but your trial period has expired");
                    //    }
                    //}
                    //else
                    //{

                    //    if (ValidateSubscription(model))
                    //    {
                    //        FormsService.SignIn(model.UserName, model.RememberMe);
                    //        return RedirectToAction("Main", "Home");
                    //    }
                    //    else
                    //    {
                    //        ModelState.AddModelError("", "Not valid subscription");
                    //    }
                    //}
                    var data=Roles.GetRolesForUser(model.UserName);
                    FormsService.SignIn(model.UserName, model.RememberMe);
                    return RedirectToAction("Main", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }

            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }


        // **************************************
        // URL: /Account/LogOff
        // **************************************
        public ActionResult LogOff()
        {
            FormsService.SignOut();
            return RedirectToAction("LogOn", "Account");
        }
        // **************************************
        // URL: /Account/Register
        // **************************************
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult Welcome(int? subId, string name, bool? trial)
        {
            ViewBag.subId = subId;
            ViewBag.name = name;
            ViewBag.trial = trial;
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                string msg = string.Empty;
                MembershipCreateStatus createStatus = this.MembershipService.CreateUser(model.UserName, model.Password, model.Email);
                if (createStatus == MembershipCreateStatus.Success)
                {
                    if (!Roles.GetAllRoles().Contains("Lawyer"))
                    {
                        Roles.CreateRole("Lawyer");
                    }
                    Roles.AddUserToRole(model.UserName, "Lawyer");
                    if (this.CreateUser(model, out msg))
                    {
                        // IGatewayResponse response = this.CreateTransaction(model);
                        //if (response.Approved)
                        //{
                        //    ARBCreateSubscriptionResponseType type = this.CreateSubscription(model);
                        //    if (type.resultCode == MessageTypeEnum.Ok)
                        //    {
                        //        this.FormsService.SignIn(model.UserName, false);
                        //        this.AddSubscriptionNo(model, msg, type.subscriptionId);
                        //        return RedirectToAction("Welcome", new { subId = type.subscriptionId, name = model.FirstName + " " + model.LastName });
                        //    }
                        //      
                        //    Membership.DeleteUser(model.UserName);
                        //    this.DeleteUser(model, out msg);
                        //    return View(model);
                        //}
                        //ModelState.AddModelError("", "Transaction error: " + response.Message);
                        //Membership.DeleteUser(model.UserName);
                        //this.DeleteUser(model, out msg);
                        //return View(model);
                        this.FormsService.SignIn(model.UserName, false);
                        return RedirectToAction("Welcome", new { name = model.FirstName + " " + model.LastName });
                    }
                    ModelState.AddModelError("", "Unable to create user (" + msg + ")");
                    Membership.DeleteUser(model.UserName);
                    return View(model);
                }
                ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
            }
            ViewBag.PasswordLength = this.MembershipService.MinPasswordLength;
            return View(model);
        }
        #region Trial
        public ActionResult Trial_Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Trial_Register(RegisterModel model)
        {
            string msg = string.Empty;
            model.IsTrial = true;
            model.SignupDate = DateTime.Today;

            MembershipCreateStatus createStatus = this.MembershipService.CreateUser(model.UserName, model.Password, model.Email);
            if (createStatus == MembershipCreateStatus.Success)
            {

                if (Create_TrialUser(model, out msg))
                {
                    FormsService.SignIn(model.UserName, false);
                    return RedirectToAction("Welcome", new { subId = 0, name = model.FirstName + " " + model.LastName, trial = true });
                }
                else
                {
                    ModelState.AddModelError("", msg);
                    Membership.DeleteUser(model.UserName);
                    this.DeleteUser(model, out msg);
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));

            }

            return View(model);
        }
        #endregion
        //public ActionResult Register(RegisterModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Attempt to register the user
        //        // MembershipCreateStatus createStatus = MembershipService.CreateUser(model.UserName, model.Password, model.Email);

        //        //if (createStatus == MembershipCreateStatus.Success)
        //        {
        //            //FormsService.SignIn(model.UserName, false /* createPersistentCookie */);
        //            var res = CreateTransaction(model);
        //            if (res.Approved)
        //            {
        //                var ress = CreateSubscription(model);
        //                if (ress.resultCode == MessageTypeEnum.Ok)
        //                {
        //                    CreateUser(model, ress.subscriptionId);
        //                    FormsService.SignIn(model.UserName, false /* createPersistentCookie */);

        //                    return RedirectToAction("Welcome", new { @subId = ress.subscriptionId, @name = model.FirstName + " " + model.LastName });

        //                }
        //                else
        //                {
        //                    ModelState.AddModelError("", ress.messages[0].text);
        //                    return View(model);
        //                }

        //            }
        //            else
        //            {
        //                ModelState.AddModelError("", res.Message);
        //                return View(model);
        //            }

        //            return RedirectToAction("Index", "Home");


        //        }
        //        ////  else
        //        //  {
        //        //      ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
        //        //  }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    ViewBag.PasswordLength = MembershipService.MinPasswordLength;
        //    return View(model);
        //}

        // **************************************
        // URL: /Account/ChangePassword
        // **************************************
        #region Subscription
        private IGatewayResponse CreateTransaction(RegisterModel model)
        {
            //4111111111111111
            //1216 date.
            var month = DateTime.Today.ToString("MMM");
            var request = new AuthorizationRequest(model.CCNumber, model.Exp_Date.Month.ToString() + model.Exp_Date.Year.ToString(), 50.00M,
                     "Transaction for the month " + month);
            var appKey = System.Configuration.ConfigurationManager.AppSettings["appKey"];
            var transid = System.Configuration.ConfigurationManager.AppSettings["transId"];
            //step 2 - create the gateway, sending in your credentials
            var gate = new Gateway(appKey, transid);
            //step 3 - make some money
            request.Email = model.Email;
            request.FirstName = model.FirstName;
            request.LastName = model.LastName;
            gate.TestMode = false;
            request.EmailCustomer = "true";
            request.TestRequest = "false";
            request.FooterEmailReceipt = "You will be billed each month";
            var response = gate.Send(request);
            return response;
        }
        private ARBCreateSubscriptionResponseType CreateSubscription(RegisterModel model)
        {

            MerchantAuthenticationType authentication = PopulateMerchantAuthentication();
            ARBSubscriptionType subscription = new ARBSubscriptionType();
            PopulateSubscription(subscription, false, model);
            ARBCreateSubscriptionResponseType response;
            response = _webservice.ARBCreateSubscription(authentication, subscription);

            if (response.resultCode == MessageTypeEnum.Ok)
            {
                _subscriptionId = response.subscriptionId;

                // Console.WriteLine("A subscription with an ID of '{0}' was successfully created.", _subscriptionId);
            }


            return response;
        }
        private void PopulateSubscription(ARBSubscriptionType sub, bool bForUpdate, RegisterModel model)
        {
            CreditCardType creditCard = new CreditCardType();

            sub.name = model.FirstName + " " + model.LastName;

            creditCard.cardNumber = model.CCNumber;
            var year = model.Exp_Date.ToString("yyyy") + "-" + model.Exp_Date.ToString("MM");
            creditCard.expirationDate = year;  // required format for API is YYYY-MM
            sub.payment = new PaymentType();
            sub.payment.Item = creditCard;
            sub.billTo = new NameAndAddressType();
            sub.billTo.firstName = model.FirstName;
            sub.billTo.lastName = model.LastName;
            // Create a subscription that is 12 monthly payments starting on Jan 1, 2019
            sub.paymentSchedule = new PaymentScheduleType();
            sub.paymentSchedule.startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month + 1, DateTime.Today.Day);
            sub.paymentSchedule.startDateSpecified = true;
            sub.paymentSchedule.totalOccurrences = 12;
            sub.paymentSchedule.totalOccurrencesSpecified = true;

            sub.amount = 50.00M;
            sub.amountSpecified = true;
            // Interval can't be updated once a subscription is created.
            sub.paymentSchedule.interval = new PaymentScheduleTypeInterval();
            sub.paymentSchedule.interval.length = 1;
            sub.paymentSchedule.interval.unit = ARBSubscriptionUnitEnum.months;

        }
        private bool ValidateSubscription(LogOnModel model)
        {
            using (CaseXL.Data.CaseXLEntities context = new CaseXLEntities())
            {

                long subId = context.App_Users.Where(a => a.UserName == model.UserName).FirstOrDefault().SubscriptionNo;
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
        #endregion
        #region PrivateMethods
        private void CreateUser(RegisterModel model, long subId)
        {
            MembershipCreateStatus createStatus = MembershipService.CreateUser(model.UserName, model.Password, model.Email);
            using (CaseXL.Data.CaseXLEntities context = new CaseXL.Data.CaseXLEntities())
            {
                var fid = context.Firms.Where(a => a.Firm_Code == model.Firm_Code).FirstOrDefault();
                CaseXL.Data.App_User user = new CaseXL.Data.App_User()
                {
                    Email = model.Email,
                    CreditCard = model.CCNumber,
                    CVNNo = model.CVNNumber,
                    FirmId = fid == null ? 0 : fid.ID,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    SubscriptionNo = subId,
                    Signupdate = DateTime.Today,
                    UserName = model.UserName
                };
                context.Add(user);
                context.SaveChanges();

            }
        }
        private bool CreateUser(RegisterModel model, out string msg)
        {
            msg = string.Empty;
            Firm firm = null;
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                try
                {
                    firm = (from a in entities.Firms
                            where a.Firm_Code == model.Firm_Code
                            select a).FirstOrDefault<Firm>();
                    if (firm == null)
                    {
                        firm = new Firm()
                        {
                            Firm_Code = model.Firm_Code,
                            Firm_Name = model.Firm_Code

                        };
                        entities.Add(firm);
                        entities.SaveChanges();
                    }
                }
                catch
                {
                    msg = "some problem during creating Firm";
                    return false;
                }


                try
                {
                    App_User entity = new App_User
                    {
                        Email = model.Email,
                        CreditCard = string.IsNullOrEmpty(model.CCNumber) ? "0" : model.CCNumber,
                        CVNNo = string.IsNullOrEmpty(model.CVNNumber) ? "0" : model.CVNNumber,
                        FirmId = firm.ID,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        SubscriptionNo = 0,
                        Signupdate = DateTime.Today,
                        UserName = model.UserName,
                        Is_Trial = false
                    };
                    entities.Add(entity);
                    entities.SaveChanges();
                }
                catch
                {
                    msg = "some problem during creating user";
                    return false;
                }
            }
            return true;
        }
        private bool DeleteUser(RegisterModel model, out string msg)
        {
            msg = string.Empty;
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                App_User entity = (from a in entities.App_Users
                                   where a.UserName == model.UserName
                                   select a).FirstOrDefault<App_User>();
                if (entity != null)
                {
                    entities.Delete(entity);
                    entities.SaveChanges();
                    return true;
                }
            }
            return false;
        }
        private bool AddSubscriptionNo(RegisterModel model, string msg, long subId)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                App_User user = (from a in entities.App_Users
                                 where a.UserName == model.UserName
                                 select a).FirstOrDefault<App_User>();
                if (user != null)
                {
                    user.SubscriptionNo = subId;
                    entities.SaveChanges();
                    return true;
                }
            }
            return false;
        }
        private bool Create_TrialUser(RegisterModel model, out string msg)
        {
            try
            {
                msg = "";
                using (CaseXLEntities entities = new CaseXLEntities())
                {
                    App_User entity = new App_User
                    {
                        Email = model.Email,
                        CreditCard = string.IsNullOrEmpty(model.CCNumber) ? "0" : model.CCNumber,
                        CVNNo = string.IsNullOrEmpty(model.CVNNumber) ? "0" : model.CVNNumber,
                        FirmId = 0,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        SubscriptionNo = 0,
                        Signupdate = DateTime.Today,
                        UserName = model.UserName,
                        Is_Trial = true
                    };
                    entities.Add(entity);
                    entities.SaveChanges();
                    return true;
                }
            }
            catch (Exception exp)
            {
                msg = "some pr1oblem during creating user " + exp.Message;
                return false;
            }

        }
        private App_User user;
        private bool IsTrial(LogOnModel model)
        {
            try
            {
                using (CaseXL.Data.CaseXLEntities context = new CaseXLEntities())
                {
                    user = context.App_Users.Where(a => a.UserName == model.UserName).FirstOrDefault();
                    return user.Is_Trial;
                }
            }
            catch { throw; }
        }
        private bool IsTrialValid(LogOnModel model)
        {
            if ((user.Signupdate.Value.AddDays(30)) >= DateTime.Today)
            {
                return true;
            }
            else
                return false;
        }
        #endregion
        #region ResetPassword

        public ActionResult ChangePassword(int tokenV)
        {
            ((dynamic)base.ViewBag).PasswordLength = this.MembershipService.MinPasswordLength;
            ChangePasswordModel model = new ChangePasswordModel
            {
                PwdKey = tokenV
            };
            return base.View(model);
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                RestPwd entity = (from a in entities.RestPwds
                                  where ((a.UserName == model.UserName) && (a.RandomCode == model.PwdToken)) && (a.Id == model.PwdKey)
                                  select a).FirstOrDefault<RestPwd>();
                if (entity == null)
                {
                    base.ModelState.AddModelError("", "Information provided is not correct");
                    return base.View(model);
                }
                App_User user = (from a in entities.App_Users
                                 where a.UserName == model.UserName
                                 select a).FirstOrDefault<App_User>();
                Membership.DeleteUser(model.UserName);
                var st = this.MembershipService.CreateUser(model.UserName, model.NewPassword, user.Email);
                if (st == MembershipCreateStatus.Success)
                {
                    entities.Delete(entity);
                    entities.SaveChanges();
                    return base.RedirectToAction("ChangePasswordSuccess");
                }
                ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
            }
            ViewBag.PasswordLength = this.MembershipService.MinPasswordLength;
            return base.View(model);
        }




        // **************************************
        // URL: /Account/ChangePasswordSuccess
        // **************************************

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        public ActionResult ResetPassword()
        {
            ViewBag.PasswordLength = this.MembershipService.MinPasswordLength;
            return base.View();
        }
        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            string email = model.Email;
            using (CaseXLEntities entities = new CaseXLEntities())
            {
                App_User user = (from a in entities.App_Users
                                 where a.Email == email
                                 select a).FirstOrDefault<App_User>();
                if (user == null)
                {
                    base.ModelState.AddModelError("", "No user exsists with the provided email");
                    return base.View(model);
                }
                int num = new Random().Next(40, 97876);
                RestPwd entity = new RestPwd
                {
                    RandomCode = new int?(num),
                    UserName = user.UserName
                };
                entities.Add(entity);
                entities.SaveChanges();
                string str = "http://www.caselinq.com/Account/ChangePassword?tokenV=" + entity.Id;
                string str2 = "<div style='height:20px; background:blue;color:white;border-radius:4px;text-align:center;font-weight:bold'>Do not reply to this email </div><br/><br/><br/>";
                string str3 = string.Concat(new object[] { str2, "Dear ", user.UserName, ",<br/><br/>Your password token is ", num, ".<br/>Please click on the link below for further instructions<br/>", str, "<br/><br/>thanks,<br/>Caselinq Team .<br/><br/><br/>" });
                new EmailSender { To = user.Email, Body = str3 }.Send();
                return base.RedirectToAction("EmailSent", new { email = user.Email });
            }
        }
        public ActionResult EmailSent(string email)
        {
            ViewBag.data = email;
            return base.View();
        }
        #endregion







    }
}
