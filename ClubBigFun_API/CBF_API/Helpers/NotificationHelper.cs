using CBF_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static CBF_API.Helpers.enumeration;

namespace CBF_API.Helpers
{
    public class NotificationHelper
    {


        //public static bool AdminRegistration(string Email, Administrators administrators, Email_Templates templates)
        //{
        //    try
        //    {
        //        string mailContent = templates.Body;
        //        mailContent = mailContent.Replace("#username#", administrators.Login_Id);

        //        Email_Notification objNotification = new Email_Notification();
        //        EmailHelper.SendMail(Email, templates.Subject, mailContent, true);

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return false;
        //}
        //public static bool MemberRegistration(string Email, Members Members, Email_Templates templates)
        //{
        //    try
        //    {
        //        string mailContent = templates.Template_Description;
        //        mailContent = mailContent.Replace("#username#", Members.Login_Id);

        //        Email_Notification objNotification = new Email_Notification();
        //        EmailHelper.SendMail(Email, templates.Template_Subject, mailContent, true);

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return false;
        //}
        internal static void SendNotificationOnRegistration(Members objMembers, CbfDbContext _context)
        {

            Email_Templates templates = _context.Email_Templates.FirstOrDefault(x => x.EmailID == eEmailTemplates.MemberRegistration);

            string subject = templates.Subject;
            string body = "";
            CbfDbContext db = new CbfDbContext();

            string message = templates.Body;
            message = message.Replace("#Email#", objMembers.Email_Address);
            body = message;
            SaveNotifications(objMembers.Email_Address, subject, body, eNotificationType.Registration, _context, true);
            SendMemberRegistrationNotificationToAdmin(objMembers, _context);

        }
        internal static void SendMemberRegistrationNotificationToAdmin(Members objMembers, CbfDbContext _context)
        {
            try
            {
                Email_Templates templates = _context.Email_Templates.FirstOrDefault(x => x.EmailID == eEmailTemplates.NewMemberNotification);
                if (templates != null)
                {
                    string subject = templates.Subject;
                    string body = "";
                    CbfDbContext db = new CbfDbContext();

                    string message = templates.Body;
                    message = message.Replace("#Name#", objMembers.First_Name + " " + objMembers.Last_Name);
                    message = message.Replace("#Email#", objMembers.Email_Address);
                    body = message;
                    SaveNotifications(templates.FromAddress, subject, body, eNotificationType.Registration, _context);
                    //SaveNotifications("maheshpandey100@gmail.com", subject, body, eNotificationType.Registration, _context, true);
                }
            }
            catch (Exception ex)
            { }

        }
        internal static void SendNotificationOnMemberConfirmation(Members objMembers, CbfDbContext _context)
        {

            Email_Templates templates = _context.Email_Templates.FirstOrDefault(x => x.EmailID == "MemConf");

            var callbackUrl = new Uri(@GlobalConfig.PublicWebUrl + "/reset-password?member_Id=?user_Id=" + objMembers.Member_Id + "&code=" + objMembers.Password);
            string subject = templates.Subject;
            string body = "";
            CbfDbContext db = new CbfDbContext();
            subject = subject.Replace("#USERID#", objMembers.Login_Id);

            string message = templates.Body;
            message = message.Replace("#FIRSTNAME#", objMembers.First_Name);
            message = message.Replace("#USERID#", objMembers.Login_Id);
            message = message.Replace("#LINK#", callbackUrl.ToString());
            body = message;
            SaveNotifications(objMembers.Email_Address, subject, body, eNotificationType.MemberConfirmation, _context);

        }
        internal static void SendNotificationMemberListConfirmation(Members objMembers, CbfDbContext _context)
        {

            Email_Templates templates = _context.Email_Templates.FirstOrDefault(x => x.EmailID == "MListConf");

            string subject = templates.Subject;
            string body = "";
            CbfDbContext db = new CbfDbContext();

            string message = templates.Body;

            body = message;
            SaveNotifications(objMembers.Email_Address, subject, body, eNotificationType.MailingListConfirmation, _context, true);

        }
        internal static void SendNotificationClubJoinConfirmation(Members objMembers, Pool_Master objclub, List<SurvEntries> survEntries, CbfDbContext _context)
        {

            Email_Templates templates = _context.Email_Templates.FirstOrDefault(x => x.EmailID == "ClubJoinConf");

            string subject = templates.Subject;
            string body = "";
            CbfDbContext db = new CbfDbContext();

            string Tickets = string.Join(", ", survEntries.Select(x => x.EntryName));
            string message = templates.Body;
            message = message.Replace("#FIRSTNAME#", objMembers.First_Name);
            message = message.Replace("#CLUBLIST#", "Club Name: <b>" + objclub.Pool_Name + " </b><br><br> With following tickets: <br><b>" + Tickets + "</b>");
            body = message;
            SaveNotifications(objMembers.Email_Address, subject, body, eNotificationType.ClubJoinConfirmation, _context);

        }
        internal static void SendNotificationClubJoinNotification(Members objMembers, CbfDbContext _context)
        {

            Email_Templates templates = _context.Email_Templates.FirstOrDefault(x => x.EmailID == "ClubJoinNoti");

            string subject = templates.Subject;
            string body = "";
            CbfDbContext db = new CbfDbContext();

            string message = templates.Body;

            body = message;
            SaveNotifications(objMembers.Email_Address, subject, body, eNotificationType.ClubJoinNotification, _context);

        }
        internal static void SendNotificationTicketPurchase(Token objToken, List<SurvEntries> survEntries, CbfDbContext _context)
        {

            Email_Templates templates = _context.Email_Templates.FirstOrDefault(x => x.EmailID == "TicketPurchaseNoti");
            if (templates != null)
            {
                Members objMember = _context.Members.Find(objToken.UserId);
                if (objMember != null)
                {
                    string subject = templates.Subject;
                    string body = "";
                    CbfDbContext db = new CbfDbContext();
                    string message = templates.Body;
                    body = message;

                    SaveNotifications(objMember.Email_Address, subject, body, eNotificationType.TicketPurchaseNotification, _context);
                }
            }
        }
        internal static void SendNotificationTicketPickConfirmation(Members objMembers, CbfDbContext _context)
        {

            Email_Templates templates = _context.Email_Templates.FirstOrDefault(x => x.EmailID == "TicketPickConf");

            string subject = templates.Subject;
            string body = "";
            CbfDbContext db = new CbfDbContext();

            string message = templates.Body;

            body = message;
            SaveNotifications(objMembers.Email_Address, subject, body, eNotificationType.TicketPickConfirmation, _context);

        }
        internal static void SendNotificationAdminRegistration(Administrators objMembers, CbfDbContext _context)
        {

            Email_Templates templates = _context.Email_Templates.Where(x => x.EmailID.ToLower() == eEmailTemplates.AdminRegis.ToLower()).FirstOrDefault();

            string subject = templates.Subject;
            string body = "";
            CbfDbContext db = new CbfDbContext();

            string message = templates.Body;

            message = message.Replace("#NAME#", objMembers.First_Name);
            message = message.Replace("#USERID#", objMembers.Login_Id);
            message = message.Replace("#EMAIL#", objMembers.Email_Address);
            message = message.Replace("#PASSWORD#", objMembers.Password);
            message = message.Replace("#ADMINTYPE#", objMembers.Admin_Type);

            body = message;
            SaveNotifications(objMembers.Email_Address, subject, body, eNotificationType.AdminRegistration, _context,true);

        }
        internal static void SendNotificationMemberReset(Members objMembers, CbfDbContext _context)
        {

            Email_Templates templates = _context.Email_Templates.FirstOrDefault(x => x.EmailID == "MemberReset");

            string subject = templates.Subject;
            string body = "";
            CbfDbContext db = new CbfDbContext();

            string message = templates.Body;

            body = message;
            SaveNotifications(objMembers.Email_Address, subject, body, eNotificationType.MemberResetPassword, _context, true);

        }
        internal static void SendNotificationAdminReset(Administrators objAdmin, CbfDbContext _context)
        {

            Email_Templates templates = _context.Email_Templates.FirstOrDefault(x => x.EmailID == "AdminReset");

            string subject = templates.Subject;
            string body = "";
            CbfDbContext db = new CbfDbContext();

            string message = templates.Body;

            message = message.Replace("#name#", objAdmin.First_Name);
            message = message.Replace("#USERID#", objAdmin.Email_Address);
            body = message;

            SaveNotifications(objAdmin.Email_Address, subject, body, eNotificationType.AdminResetPassword, _context, true);

        }
        internal static void SendNotificationAdminForget(Administrators objMembers, CbfDbContext _context)
        {

            Email_Templates templates = _context.Email_Templates.FirstOrDefault(x => x.EmailID == "AdminForget");

            var callbackUrl = new Uri(@GlobalConfig.AdminWebUrl + "/reset-password?member_Id=" + objMembers.Member_Id + "&code=" + objMembers.Password);
            string subject = templates.Subject;
            string body = "";
            CbfDbContext db = new CbfDbContext();

            subject = subject.Replace("#email#", objMembers.Email_Address);

            string message = templates.Body;
            message = message.Replace("#EMAIL#", objMembers.Email_Address);
            message = message.Replace("#email#", objMembers.Email_Address);
            message = message.Replace("#forgetpasswordLink#", callbackUrl.ToString());
            message = message.Replace("#LoginLink#", callbackUrl.ToString());
            body = message;

            SaveNotifications(objMembers.Email_Address, subject, body, eNotificationType.AdminForgetPassword, _context, true);

        }
        internal static void SendNotificationMemberForget(Members objMembers, CbfDbContext _context)
        {

            Email_Templates templates = _context.Email_Templates.FirstOrDefault(x => x.EmailID == "MemberForget");

            var callbackUrl = new Uri(@GlobalConfig.PublicWebUrl + "/reset-password?member_Id=" + objMembers.Member_Id + "&code=" + objMembers.Password);
            string subject = templates.Subject;
            string body = "" + callbackUrl + "";
            CbfDbContext db = new CbfDbContext();

            string message = templates.Body;
            message = message.Replace("#email#", objMembers.Email_Address);
            message = message.Replace("#LoginLink#", callbackUrl.ToString());
            body = message;
            SaveNotifications(objMembers.Email_Address, subject, body, eNotificationType.MemberForgetPassword, _context, true);

        }




        internal static void sendNotificationPickTicket(PickbyEntryNotification objTickets, CbfDbContext _context)
        {

            Email_Templates templates = _context.Email_Templates.FirstOrDefault(x => x.EmailID == "SURVPickConf");

            //var callbackUrl = new Uri(@GlobalConfig.PublicWebUrl + "/reset-password?member_Id=" + objToken.UserId + "&code=" + objMembers.Password);
            string subject = templates.Subject;
            string body = "";
            CbfDbContext db = new CbfDbContext();

            string message = templates.Body;
            message = message.Replace("#POOLNAME#", objTickets.Pool_Name);
            message = message.Replace("#ENTRYNAME#", objTickets.EntryName);
            message = message.Replace("#USERID#", objTickets.First_Name);

            string PicksDetails = "";
            PicksDetails += "<b>";
            PicksDetails += "Week Number: " + objTickets.WeekNumber + " <br />";
            PicksDetails += "Your Pick: " + objTickets.PickedWinnerTeamName + " <br />";

            if (objTickets.PickedWinnerTeamName.Trim() == objTickets.VisitingTeamName.Trim())
            {
                PicksDetails += "Opponent: " + objTickets.HomeTeamName + " <br />";
            }
            else
            {
                PicksDetails += "Opponent: " + objTickets.VisitingTeamName + " <br />";
            }
            PicksDetails += "</b>";
            message = message.Replace("#PICKS#", PicksDetails);

            //message = message.Replace("#LoginLink#", callbackUrl.ToString());
            body = message;
            SaveNotifications(objTickets.Email_Address, subject, body, eNotificationType.MemberForgetPassword, _context, true);
        }


        internal static void sendNotificationPasswordResetSuccessfully(Members objmembers, CbfDbContext _context)
        {

            Email_Templates templates = _context.Email_Templates.FirstOrDefault(x => x.EmailID == "GeneralNotification");

            //var callbackUrl = new Uri(@GlobalConfig.PublicWebUrl + "/reset-password?member_Id=" + objToken.UserId + "&code=" + objMembers.Password);
            string subject = "Password  Reset Successfully";
            string body = "";
            CbfDbContext db = new CbfDbContext();

            string message = templates.Body;
            string MessageDetails = "Hello <b>" + objmembers.First_Name + ",<br /><br /><b>";
            MessageDetails += "Your password has been reset successfully.<br />";
            message = message.Replace("#BODY#", MessageDetails);

            //message = message.Replace("#LoginLink#", callbackUrl.ToString());
            body = message;
            SaveNotifications(objmembers.Email_Address, subject, body, eNotificationType.GeneralNotification, _context, true);
        }

        internal static void sendNotoficationToNewOwnerMoveTicket(Members oldMember, Members newMember, SurvEntries entry, CbfDbContext _context)
        {

            Email_Templates templates = _context.Email_Templates.FirstOrDefault(x => x.EmailID == "GeneralNotification");

            //var callbackUrl = new Uri(@GlobalConfig.PublicWebUrl + "/reset-password?member_Id=" + objToken.UserId + "&code=" + objMembers.Password);
            string subject = "You got a ticket by " + oldMember.First_Name;
            string body = "";
            CbfDbContext db = new CbfDbContext();

            string message = templates.Body;
            string MessageDetails = "Hello <b>" + newMember.First_Name + ",<br /><br /><b>";
            MessageDetails += "You got a following ticket by " + oldMember.First_Name + ".<br />";
            MessageDetails += "<b>Ticket Name: " + entry.EntryName + "<b><br />";
            message = message.Replace("#BODY#", MessageDetails);

            //message = message.Replace("#LoginLink#", callbackUrl.ToString());
            body = message;
            SaveNotifications(newMember.Email_Address, subject, body, eNotificationType.GeneralNotification, _context, true);
        }

        internal static void sendNotoficationToOldOwnerMoveTicket(Members oldMember, Members newMember, SurvEntries entry, CbfDbContext _context)
        {

            Email_Templates templates = _context.Email_Templates.FirstOrDefault(x => x.EmailID == "GeneralNotification");

            //var callbackUrl = new Uri(@GlobalConfig.PublicWebUrl + "/reset-password?member_Id=" + objToken.UserId + "&code=" + objMembers.Password);
            string subject = "Your ticket has been transferred to " + newMember.First_Name;
            string body = "";
            CbfDbContext db = new CbfDbContext();

            string message = templates.Body;
            string MessageDetails = "Hello <b>" + oldMember.First_Name + ",<br /><br /><b>";
            MessageDetails += "Your following ticket has been transferred to " + newMember.First_Name + ".<br />";
            MessageDetails += "<b>Ticket Name: " + entry.EntryName + "<b><br />";
            message = message.Replace("#BODY#", MessageDetails);

            //message = message.Replace("#LoginLink#", callbackUrl.ToString());
            body = message;
            SaveNotifications(oldMember.Email_Address, subject, body, eNotificationType.GeneralNotification, _context, true);
        }

        internal static void sendNotoficationToNewOwnerMoveTicket(Members oldMember, Members newMember, string entry, CbfDbContext _context)
        {

            Email_Templates templates = _context.Email_Templates.FirstOrDefault(x => x.EmailID == "GeneralNotification");

            //var callbackUrl = new Uri(@GlobalConfig.PublicWebUrl + "/reset-password?member_Id=" + objToken.UserId + "&code=" + objMembers.Password);
            string subject = "You got a ticket by " + oldMember.First_Name;
            string body = "";
            CbfDbContext db = new CbfDbContext();

            string message = templates.Body;
            string MessageDetails = "Hello <b>" + newMember.First_Name + ",<br /><br /><b>";
            MessageDetails += "You got a following tickets by " + oldMember.First_Name + ".<br />";
            MessageDetails += "<b>" + entry + "<b><br />";
            message = message.Replace("#BODY#", MessageDetails);

            //message = message.Replace("#LoginLink#", callbackUrl.ToString());
            body = message;
            SaveNotifications(newMember.Email_Address, subject, body, eNotificationType.GeneralNotification, _context, true);
        }

        internal static void sendNotoficationToOldOwnerMoveTicket(Members oldMember, Members newMember, string entry, CbfDbContext _context)
        {

            Email_Templates templates = _context.Email_Templates.FirstOrDefault(x => x.EmailID == "GeneralNotification");

            //var callbackUrl = new Uri(@GlobalConfig.PublicWebUrl + "/reset-password?member_Id=" + objToken.UserId + "&code=" + objMembers.Password);
            string subject = "Your ticket has been transferred to " + newMember.First_Name;
            string body = "";
            CbfDbContext db = new CbfDbContext();

            string message = templates.Body;
            string MessageDetails = "Hello <b>" + oldMember.First_Name + ",<br /><br /><b>";
            MessageDetails += "Your following tickets has been transferred to " + newMember.First_Name + ".<br />";
            MessageDetails += "<b>" + entry + "<b><br />";
            message = message.Replace("#BODY#", MessageDetails);

            //message = message.Replace("#LoginLink#", callbackUrl.ToString());
            body = message;
            SaveNotifications(oldMember.Email_Address, subject, body, eNotificationType.GeneralNotification, _context, true);
        }
        internal static void sendNotificationPasswordResetSuccessfullyAdmin(Administrators objadmin, CbfDbContext _context)
        {

            Email_Templates templates = _context.Email_Templates.FirstOrDefault(x => x.EmailID == "GeneralNotification");

            //var callbackUrl = new Uri(@GlobalConfig.PublicWebUrl + "/reset-password?member_Id=" + objToken.UserId + "&code=" + objMembers.Password);
            string subject = "Password  Reset Successfully";
            string body = "";
            CbfDbContext db = new CbfDbContext();

            string message = templates.Body;
            string MessageDetails = "Hello <b>" + objadmin.First_Name + ",<br /><br /><b>";
            MessageDetails += "Your password has been reseted successfully.<br />";
            message = message.Replace("#BODY#", MessageDetails);

            //message = message.Replace("#LoginLink#", callbackUrl.ToString());
            body = message;
            SaveNotifications(objadmin.Email_Address, subject, body, eNotificationType.GeneralNotification, _context, true);
        }

        public static void SaveNotifications(string EmailTo, string Subject, string EmailBody, eNotificationType NotificationType, CbfDbContext _context, bool NotifyNow = false, Email_Templates objTemplate = null, Members members = null, Administrators administrators = null)
        {
            try
            {
                Email_Notification objEmail = new Email_Notification();
                objEmail.Email_Content = EmailBody;
                objEmail.Subject = Subject;
                objEmail.To_Email = EmailTo;
                objEmail.DTS = DateTime.Now;

                try
                {
                    if (objTemplate != null)
                    {
                        objEmail.From_Email = objTemplate.FromAddress;
                    }
                    if (members != null)
                    {
                        objEmail.Member_Id = members.Member_Id;
                    }
                    if (administrators != null)
                    {
                        objEmail.Member_Id = administrators.Member_Id;
                    }
                }
                catch (Exception ex)
                {

                }
                try
                {
                    if (NotifyNow)
                    {
                        objEmail.Is_Sent = EmailHelper.SendMail(objEmail.To_Email, objEmail.Subject, objEmail.Email_Content, true);
                    }
                }
                catch (Exception ex)
                {

                }
                _context.Email_Notification.Add(objEmail);
                _context.Entry(objEmail).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                _context.SaveChanges();


            }
            catch (Exception ex)
            {

            }
        }

        internal static void SendMemberVerificationEmail(Members objMembers, CbfDbContext _context)
        {
            Email_Templates templates = _context.Email_Templates.Where(x => x.EmailID.ToLower() == eEmailTemplates.MemConf.ToLower()).FirstOrDefault();

            var callbackUrl = new Uri(@GlobalConfig.PublicWebUrl + "/reset-password?member_Id=" + objMembers.Member_Id + "&code=" + objMembers.Password);
            string subject = templates.Subject;
            string body = "";// "" + callbackUrl + "";
            CbfDbContext db = new CbfDbContext();

            subject = subject.Replace("#USERID#", objMembers.Login_Id);

            string message = templates.Body;
            message = message.Replace("#FIRSTNAME#", objMembers.First_Name);
            message = message.Replace("#USERID#", objMembers.Login_Id);
            message = message.Replace("#LINK#", callbackUrl.ToString());
            body = message;
            SaveNotifications(objMembers.Email_Address, subject, body, eNotificationType.MemberForgetPassword, _context, true);
        }
    }
}
