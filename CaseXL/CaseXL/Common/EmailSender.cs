using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
namespace CaseXL.Common
{

    public class EmailSender
    {
        private List<string> _files = new List<string>();

        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtmlBody { get; set; }
        public string Password { get; set; }
        public EmailProviderType EmailProvider { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
        public EmailSender()
        {
            //this.Password = "Developer123#";
            //this.EmailProvider = EmailProviderType.Gmail;
            //this.From = "tauseefahmadk@gmail.com";
            //this.IsHtmlBody = true;
            //this.Subject = "Caselinq:Password notification.";

            this.Password = "E11is0720";
            this.EmailProvider = EmailProviderType.CaseLinq;
            this.From = "no-reply@caselinq.com";
            this.IsHtmlBody = true;
            this.Subject = "Caselinq:Password notification.";
        }
        /// <summary>
        /// Get or Set files to attach with email.
        /// </summary>
        public string[] AttachmentFileArray
        {
            get
            {
                int a = 0;
                if (_files != null)
                    a = _files.Count;
                string[] f = new string[a];
                int count = -1;
                foreach (string s in _files)
                {
                    count++;
                    f[count] = s;
                }
                return f;
            }
            set
            {
                _files = new List<string>();
                foreach (string s in value)
                {
                    _files.Add(s);
                }
            }
        }

        /// <summary>
        /// Get or Set files to attach with email.
        /// </summary>
        public List<string> AttachmentFileList
        {
            get
            {
                return _files;
            }
            set
            {
                _files = value;
            }
        }

        /// <summary>
        /// Set a file to attach with email.
        /// </summary>
        public string AttachFile
        {
            set
            {
                _files = new List<string>();
                _files.Add(value);
            }
        }

        public enum EmailProviderType
        {
            Yahoo,
            Gmail,
            Hotmail,
            CaseLinq,
            Other
        }

        public void Send()
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(From);
            mail.To.Add(To);
            mail.Subject = Subject;
            mail.Body = Body;
            mail.IsBodyHtml = IsHtmlBody;
            foreach (string s in _files)
            {
                mail.Attachments.Add(new Attachment(s));
            }

            SetProvider();


            SendMail(mail);
        }

        public void Send(MailMessage mail, string password, EmailProviderType emailProvider)
        {
            Password = password;
            EmailProvider = emailProvider;

            SetProvider();

            SendMail(mail);
        }

        public void Send(string from, string to, string subject, string body,
            bool isHtmlBody, string password, EmailProviderType emailProvider)
        {
            From = from;
            To = to;
            Subject = subject;
            Body = body;
            IsHtmlBody = isHtmlBody;
            Password = password;
            EmailProvider = emailProvider;

            Send();
        }

        private void SetProvider()
        {
            switch (EmailProvider)
            {
                case EmailProviderType.Yahoo:
                    Host = "smtp.mail.yahoo.com";
                    Port = 587;
                    EnableSSL = false;
                    break;
                case EmailProviderType.Gmail:
                    Host = "smtp.gmail.com";
                    Port = 587;
                    EnableSSL = true;
                    break;
                case EmailProviderType.Hotmail:
                    Host = "smtp.live.com";
                    Port = 587;
                    EnableSSL = true;
                    break;
                case EmailProviderType.CaseLinq:
                    this.Host = "smtpout.secureserver.net";
                    this.Port = 80;
                    this.EnableSSL = false;
                    break;
                default:
                    break;
            }
        }

        private void SendMail(MailMessage mail)
        {
            CheckMailParameters();

            SmtpClient smtp = new SmtpClient(Host, Port);
            smtp.Credentials = new NetworkCredential(From, Password);
            smtp.EnableSsl = EnableSSL;
            smtp.Send(mail);

            smtp = null;
            mail = null;
        }

        private void CheckMailParameters()
        {
            if (From + "" == "")
                throw new Exception("Email (From) address is blank.");
            if (To + "" == "")
                throw new Exception("Email (To) address is blank.");
            if (Password + "" == "")
                throw new Exception("Password is blank.");
            if (Host == "")
                throw new Exception("Host is blank.");
            if (Port == 0)
                throw new Exception("Port is blank.");
        }
    }

}