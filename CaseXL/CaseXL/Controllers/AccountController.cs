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
        public ActionResult Welcome(int? subId, string name)
        {
            ViewBag.subId = subId;
            ViewBag.name = name;
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                // MembershipCreateStatus createStatus = MembershipService.CreateUser(model.UserName, model.Password, model.Email);

                //if (createStatus == MembershipCreateStatus.Success)
                {
                    //FormsService.SignIn(model.UserName, false /* createPersistentCookie */);
                    var res = CreateTransaction(model);
                    if (res.Approved)
                    {
                        var ress = CreateSubscription(model);
                        if (ress.resultCode == MessageTypeEnum.Ok)
                        {
                            CreateUser(model, ress.subscriptionId);
                            FormsService.SignIn(model.UserName, false /* createPersistentCookie */);
                           
                            return RedirectToAction("Welcome", new { @subId = ress.subscriptionId, @name = model.FirstName + " " + model.LastName });

                        }
                        else
                        {
                            ModelState.AddModelError("", ress.messages[0].text);
                            return View(model);
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("", res.Message);
                        return View(model);
                    }

                    return RedirectToAction("Index", "Home");


                }
                ////  else
                //  {
                //      ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                //  }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePassword
        // **************************************

        [Authorize]
        public ActionResult ChangePassword()
        {
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePasswordSuccess
        // **************************************

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

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
            request.EmailCustomer = "true";
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
        private static MerchantAuthenticationType PopulateMerchantAuthentication()
        {
            MerchantAuthenticationType authentication = new MerchantAuthenticationType();
            authentication.name = System.Configuration.ConfigurationManager.AppSettings["appKey"];
            authentication.transactionKey = System.Configuration.ConfigurationManager.AppSettings["transId"];
            return authentication;
        }
        #endregion
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
                    SignOnDate = DateTime.Today,
                    UserName = model.UserName
                };
                context.Add(user);
                context.SaveChanges();

            }
        }
    }
}
