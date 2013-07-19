using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AuthorizeNet;
using DotNetOpenAuth.OpenId.Extensions.OAuth;

namespace CaseXL.Controllers
{
    public class AccountsController : Controller
    {
        //
        // GET: /Accounts/

        public ActionResult Register()
        {
            //var request = new AuthorizeNet.AuthorizationRequest("4111111111111111", "1216", 840.00M,
            //   "Test Transaction");
            //request.FirstName = "Test Customer";
            //request.LastName = "Test Customer";
            //request.Email = "tauseef-ahmad@hotmail.com";
            //request.EmailCustomer = "true";
            //request.TransId = new Random().Next(300).ToString(); 
            ////step 2 - create the gateway, sending in your credentials
            //var gate = new Gateway("5qFn7V6TU7Y", "5VyWa482Wk854p4d");
            
            ////step 3 - make some money
            //var response = gate.Send(request);
            //ViewBag.message = response.Message;
            return View();
        }
        [HttpPost()]
        public ActionResult RegisterData()
        {
           
            
            var request = new AuthorizeNet.AuthorizationRequest("4111111111111111", "1216", 840.00M,
               "Test Transaction");
            request.FirstName = "Test Customer";
            request.LastName = "Test Customer";
            request.Email = "tauseef-ahmad@hotmail.com";
            request.EmailCustomer = "true";
            //request.TransId = new Random().Next(300).ToString();
            //step 2 - create the gateway, sending in your credentials
            var gate = new Gateway("5qFn7V6TU7Y", "5VyWa482Wk854p4d");

            //step 3 - make some money
            var response = gate.Send(request);
            
            ViewBag.message = response.Message;
           // Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
       
    }
}
