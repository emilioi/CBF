using CBF_API.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web.Http;

namespace CBF_API.Helpers
{
    public class EmailHelper
    {
        #region Email Helper

        private static IHostingEnvironment _env;
        public EmailHelper(IHostingEnvironment env)
        {
            _env = env;
        }
        public static bool SendMail(string toAddress, string subject, string mailContent, bool IsBodyHtml)
        {
            mailContent = SetLogo(mailContent);
            bool result = false;

            SmtpClient smtpClient = new SmtpClient();
            MailMessage message = new MailMessage();


            try
            {
                smtpClient = GetSMTPClientObject();
                MailAddress senderAddress = new MailAddress(GetSenderAddress());
                message.From = new MailAddress("\"" + "CLUB BIG FUN" + "\" <" + senderAddress + ">");
                message = GetEmails(toAddress, message);
                //
                //****************************************

                //var webRoot = _env.WebRootPath;
                //var file = System.IO.Path.Combine(webRoot, "/emp/CBF-Logo-.svg");
                //LinkedResource res = new LinkedResource(file);
                //res.ContentId = Guid.NewGuid().ToString();
                //string htmlBody = @"<img src='cid:" + res.ContentId + @"'/>";
                //mailContent = mailContent.Replace("#LogoUrl#", htmlBody);
                //AlternateView alternateView = AlternateView.CreateAlternateViewFromString(mailContent, null, MediaTypeNames.Text.Html);
                //alternateView.LinkedResources.Add(res);
                //message.AlternateViews.Add(alternateView);

                //**************************************
                //
                message.Subject = "[CLUB BIG FUN ] " + subject;
                message.IsBodyHtml = IsBodyHtml;
                message.Body = mailContent;



                if (!String.IsNullOrEmpty(toAddress))
                {
                    smtpClient.Send(message);
                }
                else
                {
                    // GNF.LogFailedEmail(toAddress);
                    return false;

                }

                result = true;
                // GNF.LogSentEmail(toAddress);

            }
            catch (Exception ex)
            { //Try to send using Backup SMTP In the case if SMTP failes
                if (!SendUsingBackupSMTP(message))
                {
                    result = false;
                    // GNF.LogFailedEmail(toAddress);
                }
            }

            return result;
        }

        public bool SendMailDirect(string toAddress, string subject, string mailContent, bool IsBodyHtml)
        {
            //mailContent = SetLogo(mailContent);
            bool result = false;

            SmtpClient smtpClient = new SmtpClient();
            MailMessage message = new MailMessage();


            try
            {
                smtpClient = GetSMTPClientObject();
                MailAddress senderAddress = new MailAddress(GetSenderAddress());
                message.From = new MailAddress("\"" + "CLUB BIG FUN" + "\" <" + senderAddress + ">");
                message = GetEmails(toAddress, message);
                //
                //****************************************
                mailContent = SetLogo(mailContent);
                //var webRoot = _env.WebRootPath;
                //var file = System.IO.Path.Combine(webRoot, "../Temp/CBF-Logo-.svg");
                //LinkedResource res = new LinkedResource(file);
                //res.ContentId = Guid.NewGuid().ToString();
                //string htmlBody = @"<img src='cid:" + res.ContentId + @"'/>";
                //mailContent = mailContent.Replace("#LogoUrl#", htmlBody);
                //AlternateView alternateView = AlternateView.CreateAlternateViewFromString(mailContent, null, MediaTypeNames.Text.Html);
                //alternateView.LinkedResources.Add(res);
                //message.AlternateViews.Add(alternateView);

                //**************************************
                //
                message.Subject = "[CLUB BIG FUN ] " + subject;
                message.IsBodyHtml = IsBodyHtml;
                message.Body = mailContent;



                if (!String.IsNullOrEmpty(toAddress))
                {
                    smtpClient.Send(message);
                }
                else
                {
                    // GNF.LogFailedEmail(toAddress);
                    return false;

                }

                result = true;
                // GNF.LogSentEmail(toAddress);

            }
            catch (Exception ex)
            { //Try to send using Backup SMTP In the case if SMTP failes
                if (!SendUsingBackupSMTP(message))
                {
                    result = false;
                    // GNF.LogFailedEmail(toAddress);
                }
            }

            return result;
        }
        //private static AlternateView getEmbeddedImage()
        //{
        //    var webRoot = _env.WebRootPath;
        //    var file = System.IO.Path.Combine(webRoot, "/emp/CBF-Logo-.svg");
        //    LinkedResource res = new LinkedResource(file);
        //    res.ContentId = Guid.NewGuid().ToString();
        //    string htmlBody = @"<img src='cid:" + res.ContentId + @"'/>";
        //    AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
        //    alternateView.LinkedResources.Add(res);
        //    return alternateView;
        //}
        private static string SetLogo(string mailContent)
        {
            try
            {
                string msg = mailContent.Replace("#LogoUrl#", @"<img style='margin:0 auto; height:70px' src='" + EmailConfiguration.Get_LogoUrl.ToString() + @"'/>");

                return msg;
            }
            catch (Exception eX)
            {
                return mailContent;
            }
        }

        internal static Email_Notification SendMail(string toAddress, string subject, string mailContent, bool IsBodyHtml, Email_Notification objNotify)
        {
            mailContent = SetLogo(mailContent);
            bool result = false;

            SmtpClient smtpClient = new SmtpClient();
            MailMessage message = new MailMessage();


            try
            {
                smtpClient = GetSMTPClientObject();
                MailAddress senderAddress = new MailAddress(GetSenderAddress());
                message.From = new MailAddress("\"" + "CLUB BIG FUN" + "\" <" + senderAddress + ">");
                message = GetEmails(toAddress, message);

                message.Subject = "[CLUB BIG FUN ] " + subject;
                message.IsBodyHtml = IsBodyHtml;
                message.Body = mailContent;



                if (!String.IsNullOrEmpty(toAddress))
                {
                    smtpClient.Send(message);
                }
                else
                {
                    throw new Exception("Email To is empty");
                }

                result = true;
                if (objNotify != null)
                {
                    objNotify.Is_Sent = true;
                    objNotify.Sent_On = DateTime.Now;
                }

            }
            catch (Exception ex)
            {
                //if (!SendUsingBackupSMTP(message))
                //{
                //    result = false;
                //}
                if (objNotify != null)
                {
                    objNotify.Failed_Error = ex.Message;
                    objNotify.Is_Sent = false;
                    objNotify.Sent_On = DateTime.Now;
                }
            }

            return objNotify;
        }

        public static bool SendMailWithMultipleAttachment(string toAddress, string subject, string mailContent, bool IsBodyHtml, List<System.Net.Mail.Attachment> lstAttachment)
        {
            mailContent = SetLogo(mailContent);
            bool result = false;

            SmtpClient smtpClient = new SmtpClient();
            MailMessage message = new MailMessage();

            try
            {
                smtpClient = GetSMTPClientObject();
                MailAddress senderAddress = new MailAddress(GetSenderAddress());
                message.From = new MailAddress("\"" + "[CLUB BIG FUN]" + "\" <" + senderAddress + ">");
                message = GetEmails(toAddress, message);
                //****************************************

                //var webRoot = _env.WebRootPath;
                //var file = System.IO.Path.Combine(webRoot, "/emp/CBF-Logo-.svg");
                //LinkedResource res = new LinkedResource(file);
                //res.ContentId = Guid.NewGuid().ToString();
                //string htmlBody = @"<img src='cid:" + res.ContentId + @"'/>";
                //mailContent = mailContent.Replace("#LogoUrl#", htmlBody);
                //AlternateView alternateView = AlternateView.CreateAlternateViewFromString(mailContent, null, MediaTypeNames.Text.Html);
                //alternateView.LinkedResources.Add(res);
                //message.AlternateViews.Add(alternateView);

                //**************************************
                message.Subject = "[CLUB BIG FUN] " + subject;
                message.IsBodyHtml = IsBodyHtml;
                message.Body = mailContent;

                foreach (Attachment a in lstAttachment)
                {
                    message.Attachments.Add(a);

                }


                if (!String.IsNullOrEmpty(toAddress))
                {
                    smtpClient.Send(message);
                }
                else
                {
                    // GNF.LogFailedEmail(toAddress);
                    return false;

                }

                result = true;
                // GNF.LogSentEmail(toAddress);

            }
            catch (Exception ex)
            { //Try to send using Backup SMTP In the case if SMTP failes
                if (!SendUsingBackupSMTP(message))
                {
                    result = false;
                    // GNF.LogFailedEmail(toAddress);
                }
            }

            return result;
        }

        #endregion


        #region Configuration
        private static string GetSenderAddress()
        {

            return GlobalConfig.FromAddress;//

        }

        private static SmtpClient GetSMTPClientObject()
        {

            SmtpClient smtpClient = new SmtpClient();

            smtpClient.Host = GlobalConfig.SMTPHost;
            smtpClient.Port = Convert.ToInt32(GlobalConfig.SMTPPort);
            smtpClient.UseDefaultCredentials = EmailConfiguration.UseDefaultCredentials;
            smtpClient.Timeout = 60000;
            smtpClient.Credentials = new NetworkCredential(GlobalConfig.SenderAddress, GlobalConfig.SenderPassword);// GlobalConfig.AppSettings["EmailPassword"]);
            smtpClient.EnableSsl = Convert.ToBoolean(EmailConfiguration.SmtpEnableSsl);



            return smtpClient;
        }

        private static SmtpClient GetBackupSMTPClientObject()
        {

            SmtpClient smtpClient = new SmtpClient();

            smtpClient.Host = GlobalConfig.SMTPHost;
            smtpClient.Port = Convert.ToInt32(GlobalConfig.SMTPPort);
            smtpClient.UseDefaultCredentials = EmailConfiguration.UseDefaultCredentials;
            smtpClient.Timeout = 60000;
            smtpClient.Credentials = new NetworkCredential(GlobalConfig.SenderAddress, GlobalConfig.SenderPassword);
            smtpClient.EnableSsl = Convert.ToBoolean(EmailConfiguration.SmtpEnableSsl);



            return smtpClient;
        }

        internal static System.Net.Mail.MailMessage GetEmails(string toAddress, System.Net.Mail.MailMessage message)
        {
            message.To.Add(toAddress);
            string CCEmail = EmailConfiguration.EmailCopy;
            if (CCEmail.Trim() != "")
            {
                message.CC.Add(CCEmail);
            }
            string BCCEmail = EmailConfiguration.EmailBlindCopy;
            if (BCCEmail.Trim() != "")
            {
                message.Bcc.Add(BCCEmail);


            }

            return message;
        }

        private static bool SendUsingBackupSMTP(MailMessage message)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                smtpClient = GetBackupSMTPClientObject();
                smtpClient.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        #endregion
        public class EmailConfiguration
        {
            public static string EditorEmail = "info@nowgray.com";
            public static string EmailBlindCopy = "";
            public static string EmailCopy = "";
            public static string Get_LogoUrl = GlobalConfig.Get_LogoUrl;// "http://cbfpublic-nowgray-com.nt1-p2stl.ezhostingserver.com/assets/CBF-logo.png";// "data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiPz4KPHN2ZyB3aWR0aD0iMTMxcHgiIGhlaWdodD0iMTI0cHgiIHZpZXdCb3g9IjAgMCAxMzEgMTI0IiB2ZXJzaW9uPSIxLjEiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyIgeG1sbnM6eGxpbms9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkveGxpbmsiPgogICAgPCEtLSBHZW5lcmF0b3I6IFNrZXRjaCA1NiAoODE1ODgpIC0gaHR0cHM6Ly9za2V0Y2guY29tIC0tPgogICAgPHRpdGxlPm5vdW5fVHJvcGh5XzgyNTk5NCAoMSk8L3RpdGxlPgogICAgPGRlc2M+Q3JlYXRlZCB3aXRoIFNrZXRjaC48L2Rlc2M+CiAgICA8ZyBpZD0iTW9iaWxlIiBzdHJva2U9Im5vbmUiIHN0cm9rZS13aWR0aD0iMSIgZmlsbD0ibm9uZSIgZmlsbC1ydWxlPSJldmVub2RkIj4KICAgICAgICA8ZyBpZD0iMS4tTW9iaWxlLUxvZ2luIiB0cmFuc2Zvcm09InRyYW5zbGF0ZSgtMTQxLjAwMDAwMCwgLTEzNC4wMDAwMDApIiBmaWxsLXJ1bGU9Im5vbnplcm8iPgogICAgICAgICAgICA8ZyBpZD0ibm91bl9Ucm9waHlfODI1OTk0LSgxKSIgdHJhbnNmb3JtPSJ0cmFuc2xhdGUoMTQxLjAwMDAwMCwgMTM0LjAwMDAwMCkiPgogICAgICAgICAgICAgICAgPGcgaWQ9Ikdyb3VwIj4KICAgICAgICAgICAgICAgICAgICA8cGF0aCBkPSJNNjUuNzUzNDA2NiwxMTMuODcyMzcyIEM4MC4wNTg3MjUzLDExMy44NzIzNzIgOTEuNjk1MDMzLDEwMi4zMDU3NjcgOTEuNjk1MDMzLDg4LjA4NjEzOTUgTDkxLjY5NTAzMyw1NC40NTE4NjA1IEwzOS44MTE3ODAyLDU0LjQ1MTg2MDUgTDM5LjgxMTc4MDIsODguMDg2MTM5NSBDMzkuODExNzgwMiwxMDIuMzA0MzI2IDUxLjQ0ODA4NzksMTEzLjg3MjM3MiA2NS43NTM0MDY2LDExMy44NzIzNzIgWiBNNTcuMTE4Mjg1Nyw3MC42NzcxMTYzIEw2NS43NTE5NTYsNTguMTUzMTE2MyBMNzQuMzg1NjI2NCw3MC42NzcxMTYzIEw4OS4wMzYxNzU4LDc0Ljk2ODA5MyBMNzkuNzIyMTk3OCw4Ny4wMDE4NjA1IEw4MC4xNDI4NTcxLDEwMi4xNzc0NDIgTDY1Ljc1MTk1Niw5Ny4wOTA1NTgxIEw1MS4zNjEwNTQ5LDEwMi4xNzc0NDIgTDUxLjc4MTcxNDMsODcuMDAwNDE4NiBMNDIuNDY3NzM2Myw3NC45NjY2NTEyIEw1Ny4xMTgyODU3LDcwLjY3NzExNjMgWiIgaWQ9IlNoYXBlIiBmaWxsPSIjMDAwMDAwIj48L3BhdGg+CiAgICAgICAgICAgICAgICAgICAgPHBhdGggZD0iTTI5LjY1OTM4NDYsODguMDg2MTM5NSBDMjkuNjU5Mzg0NiwxMDcuODY5OTA3IDQ1Ljg1MDQxNzYsMTIzLjk2Mzk1MyA2NS43NTM0MDY2LDEyMy45NjM5NTMgQzg1LjY1NDk0NTEsMTIzLjk2Mzk1MyAxMDEuODQ1OTc4LDEwNy44Njk5MDcgMTAxLjg0NTk3OCw4OC4wODYxMzk1IEwxMDEuODQ1OTc4LDQ0LjM2MDI3OTEgTDI5LjY1OTM4NDYsNDQuMzYwMjc5MSBMMjkuNjU5Mzg0Niw4OC4wODYxMzk1IFogTTM1LjI5OTEyMDksNDkuOTY2MjMyNiBMOTYuMjA3NjkyMyw0OS45NjYyMzI2IEw5Ni4yMDc2OTIzLDg4LjA4NjEzOTUgQzk2LjIwNzY5MjMsMTA0Ljc3NzExNiA4Mi41NDY0MTc2LDExOC4zNTggNjUuNzUzNDA2NiwxMTguMzU4IEM0OC45NjAzOTU2LDExOC4zNTggMzUuMjk5MTIwOSwxMDQuNzc4NTU4IDM1LjI5OTEyMDksODguMDg2MTM5NSBMMzUuMjk5MTIwOSw0OS45NjYyMzI2IFoiIGlkPSJTaGFwZSIgZmlsbD0iIzAwMDAwMCI+PC9wYXRoPgogICAgICAgICAgICAgICAgICAgIDxwb2x5Z29uIGlkPSJQYXRoIiBmaWxsPSIjRjdDNTNEIiBwb2ludHM9Ijk1LjcyMzIwODggMTguODE0ODM3MiA5Mi42OTQ0NjE1IDI0LjkxODIzMjYgODUuOTE4OTQ1MSAyNS44OTcyNTU4IDkwLjgyMTgwMjIgMzAuNjQ2NzQ0MiA4OS42NjQyNjM3IDM3LjM1NTcyMDkgOTUuNzIzMjA4OCAzNC4xODc5NTM1IDEwMS43ODM2MDQgMzcuMzU1NzIwOSAxMDAuNjI2MDY2IDMwLjY0Njc0NDIgMTA1LjUyODkyMyAyNS44OTcyNTU4IDk4Ljc1NDg1NzEgMjQuOTE4MjMyNiI+PC9wb2x5Z29uPgogICAgICAgICAgICAgICAgICAgIDxwb2x5Z29uIGlkPSJQYXRoIiBmaWxsPSIjRjdDNTNEIiBwb2ludHM9IjcwLjU5OTY5MjMgOS43OTg4ODM3MiA2NS43NTM0MDY2IDAuMDM2MDQ2NTExNiA2MC45MDU2NzAzIDkuNzk4ODgzNzIgNTAuMDY4NjE1NCAxMS4zNjMzMDIzIDU3LjkxMDI4NTcgMTguOTYxOTA3IDU2LjA1OTM4NDYgMjkuNjkwNzkwNyA2NS43NTM0MDY2IDI0LjYyNTUzNDkgNzUuNDQ1OTc4IDI5LjY5MDc5MDcgNzMuNTk1MDc2OSAxOC45NjE5MDcgODEuNDM2NzQ3MyAxMS4zNjMzMDIzIj48L3BvbHlnb24+CiAgICAgICAgICAgICAgICAgICAgPHBvbHlnb24gaWQ9IlBhdGgiIGZpbGw9IiNGN0M1M0QiIHBvaW50cz0iMzguODEyMzUxNiAyNC45MTgyMzI2IDM1Ljc4MDcwMzMgMTguODE0ODM3MiAzMi43NTE5NTYgMjQuOTE4MjMyNiAyNS45NzY0Mzk2IDI1Ljg5NzI1NTggMzAuODc5Mjk2NyAzMC42NDY3NDQyIDI5LjcyMTc1ODIgMzcuMzU1NzIwOSAzNS43ODA3MDMzIDM0LjE4Nzk1MzUgNDEuODQxMDk4OSAzNy4zNTU3MjA5IDQwLjY4NTAxMSAzMC42NDY3NDQyIDQ1LjU4NjQxNzYgMjUuODk3MjU1OCI+PC9wb2x5Z29uPgogICAgICAgICAgICAgICAgICAgIDxwb2x5Z29uIGlkPSJQYXRoIiBmaWxsPSIjMDAwMDAwIiBwb2ludHM9IjU3LjA0NDMwNzcgOTQuNDA0MzcyMSA2NS43NTM0MDY2IDkxLjMyNzQ0MTkgNzQuNDYxMDU0OSA5NC40MDQzNzIxIDc0LjIwNzIwODggODUuMjE5NzIwOSA3OS44NDI1OTM0IDc3LjkzOTc2NzQgNzAuOTc4Mjg1NyA3NS4zNDI5NzY3IDY1Ljc1MzQwNjYgNjcuNzY0NTU4MSA2MC41MjcwNzY5IDc1LjM0Mjk3NjcgNTEuNjYyNzY5MiA3Ny45Mzk3Njc0IDU3LjI5ODE1MzggODUuMjIxMTYyOCI+PC9wb2x5Z29uPgogICAgICAgICAgICAgICAgICAgIDxwYXRoIGQ9Ik0xMDkuMDcyNjE1LDU3LjE5MTM5NTMgTDEwOS4wNzI2MTUsODguMDg2MTM5NSBDMTA5LjA3MjYxNSw4OC4xNjk3Njc0IDEwOS4wNjY4MTMsODguMjUxOTUzNSAxMDkuMDY2ODEzLDg4LjMzNTU4MTQgTDEzMS4wMjY2ODEsODguMzM1NTgxNCBMMTIyLjMyNDgzNSw3Mi43NjM0ODg0IEwxMzEuMDI4MTMyLDU3LjE5MTM5NTMgTDEwOS4wNzI2MTUsNTcuMTkxMzk1MyBMMTA5LjA3MjYxNSw1Ny4xOTEzOTUzIFoiIGlkPSJQYXRoIiBmaWxsPSIjMDAwMDAwIj48L3BhdGg+CiAgICAgICAgICAgICAgICAgICAgPHBhdGggZD0iTTAuNDc4NjgxMzE5LDg4LjMzNDEzOTUgTDIzLjM1ODE5NzgsODguMzM0MTM5NSBDMjMuMzU4MTk3OCw4OC4yNTA1MTE2IDIzLjM1MjM5NTYsODguMTY4MzI1NiAyMy4zNTIzOTU2LDg4LjA4NDY5NzcgTDIzLjM1MjM5NTYsNTcuMTkxMzk1MyBMMC40Nzg2ODEzMTksNTcuMTkxMzk1MyBMOS4xODE5NzgwMiw3Mi43NjM0ODg0IEwwLjQ3ODY4MTMxOSw4OC4zMzQxMzk1IFoiIGlkPSJQYXRoIiBmaWxsPSIjMDAwMDAwIj48L3BhdGg+CiAgICAgICAgICAgICAgICA8L2c+CiAgICAgICAgICAgIDwvZz4KICAgICAgICA8L2c+CiAgICA8L2c+Cjwvc3ZnPg==";
            public static bool IsApplicationUnderDevelopment = false;
            public static object SmtpEnableSsl = GlobalConfig.SSL;
            public static bool UseDefaultCredentials = false;
        }

    }
}
