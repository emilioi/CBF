using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBF_API.Helpers
{
    public class enumeration
    {
        public static class eRoleType
        {

            public const string
                Admin = "Admin",
                SuperAdmin = "SuperAdmin",
                GroupAdmin = "GroupAdmin",
                Member = "Member";

        }
        public enum eNotificationType
        {
            Registration = 0,
            MemberConfirmation = 1,
            MailingListConfirmation = 2,
            ClubJoinConfirmation = 3,
            ClubJoinNotification = 4,
            TicketPurchaseNotification = 5,
            TicketPickConfirmation = 6,
            AdminRegistration = 7,
            AdminResetPassword = 8,
            MemberResetPassword = 9,
            AdminForgetPassword = 10,
            MemberForgetPassword = 11,
            GeneralNotification = 12
        }

        public static class eEmailTemplates
        {
            public const string
                    AdminForget = "AdminForget",
                    AdminRegis = "AdminRegis",
                    AdminReset = "AdminReset",
                    ClubJoinConf = "ClubJoinConf",
                    //ClubJoinNoti = "ClubJoinNoti",
                    MemberForget = "MemberForget",
                    MemberRegistration = "MemberRegistration",
                    MemberReset = "MemberReset",
                    MemConf = "MemConf",
                    MListConf = "MListConf",
                    SURVENConf = "SURVENConf",
                    SURVPickConf = "SURVPickConf",
                    TelAFriend = "TelAFriend",
            NewMemberNotification = "NewMemberNotification",
            // TicketPickConf = "TicketPickConf",
            GeneralNotification = "GeneralNotification";

            // TicketPurchaseNoti = "TicketPurchaseNoti";
        }
        public enum eLevel
        {
            Low,
            Medium,
            High
        }
    }
}
