using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
using CaseXL.Infrastructure;
using CaseXL.Data;
using AuthorizeNet.APICore;
using CaseXL.Auth_Service;

namespace CaseXL.Common
{
    //IF YOU WANT YOU CAN HAVE CONTROL AND SEE CODE YOURSELF 
    // can you please show me the app running? 
  
    public class Util
    {
        public const string DATEFORMAT_FORGRIDS = "{0:MM/dd/yyyy}";
        public const string DATEFORMAT_FORDATEPICKERS = "MM/dd/yyyy";
        public const string EMAIL_REGEX = @"\w+[\w-\.]*\@\w+((-\w+)|(\w*))\.[a-z]{2,3}";
        public const string PHONE_REGEX = @"\(\d{3}\)\s\d{3}\-\d{4}";
        private static CaseXL.Auth_Service.ServiceSoapClient _webservice = new ServiceSoapClient();
        /// <summary>
        /// This is a helper method used to send email.
        /// </summary>
        public static bool ValidateSubscription()
        {
            var user = SessionBase.User;
            using (CaseXL.Data.CaseXLEntities context = new CaseXLEntities())
            {
                long subId = context.App_Users.Where(a => a.UserName == user.UserName).FirstOrDefault().SubscriptionNo;
                CaseXL.Auth_Service.ARBGetSubscriptionStatusResponseType result = _webservice.ARBGetSubscriptionStatus(PopulateMerchantAuthentication(), subId);
                return (result.status == CaseXL.Auth_Service.ARBSubscriptionStatusEnum.active && result.resultCode != MessageTypeEnum.Error) ? true : false;
            }
        }
        private static MerchantAuthenticationType PopulateMerchantAuthentication()
        {
            MerchantAuthenticationType authentication = new MerchantAuthenticationType();
            authentication.name = System.Configuration.ConfigurationManager.AppSettings["appKey"];
            authentication.transactionKey = System.Configuration.ConfigurationManager.AppSettings["transId"];
            return authentication;
        }
    }
}