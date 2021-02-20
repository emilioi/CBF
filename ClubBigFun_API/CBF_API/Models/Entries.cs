using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CBF_API.Models
{
    public class EntryMenuAPIResponse : APIResponses
    {
        public List<EntryMenu> EntryMenus { get; set; }
    }
    public class EntryMenuResponse : APIResponses
    {
        public EntryMenu EntryMenus { get; set; }
    }
    public class EntryMenu
    {
        [Key]
        public int Pool_ID { get; set; }
        public string Pool_Name { get; set; }
        public int TheCount { get; set; }
    }
    public class EntryAPIResponse : APIResponses
    {
        public SurvEntries SurvEntries { get; set; }
        public Pool_Master Pool_Master { get; set; }
    }
    public class EntryPickAPIResponse : APIResponses
    {
        public SurvEntryPicks SurvEntryPicks { get; set; }
    }
    public class EntryPickListAPIResponse : APIResponses
    {
        public List<SurvEntryPicks> SurvEntryPicks { get; set; }
    }
    public class EntryListAPIResponse : APIResponses
    {
        public List<qrySurvEntries> EntryWeekLists { get; set; }
    }
    public class EntryReferralListAPIResponse : APIResponses
    {
        public List<MemberReferralDetail> members { get; set; }
    }
    public class MemberReferralDetail
    {

        public string FullName { get; set; }
        public string Login_ID { get; set; }
        public int MemberID { get; set; }
        public int Count { get; set; }
        public List<qrySurvEntries> EntryWeekLists { get; set; }
    }
    public class qrySurvEntries
    {
        [Key]
        public int EntryID { get; set; }

        public int PoolID { get; set; }
        public string Pool_Name { get; set; }
        public string EntryName { get; set; }
        public bool Eliminated { get; set; }
        public string Login_ID { get; set; }
        public int MemberID { get; set; }
        public string FullName { get; set; }
        public bool Active { get; set; }
        public decimal Price { get; set; }
        public int Defaults { get; set; }
    }
    public class qrySurvEntriesNew
    {

        public int EntryID { get; set; }
        public int MemberID { get; set; }
        public int PoolID { get; set; }
        public string EntryName { get; set; }
        public bool Eliminated { get; set; }
        public bool Active { get; set; }

        public string Pool_Name { get; set; }


        public string Login_ID { get; set; }

        public string FullName { get; set; }

        public decimal Price { get; set; }
        public int Defaults { get; set; }
    }
    public class EntryWeekAPIResponse : APIResponses
    {
        public qrySurvEntries EntryWeekLists { get; set; }
    }

    public class TicketsAPIResponse : APIResponses
    {
        public List<SurvEntries> SurvEntries { get; set; }
    }
    public class TicketByPoolId
    {
        [Key]
        public int EntryID { get; set; }
        public string EntryName { get; set; }
        public string Name { get; set; }
    }
    public class TicketByPoolIdResponses : APIResponses
    {
        public List<TicketByPoolId> TicketByPoolId { get; set; }
    }
    public class TicketsDetailAPIResponse : APIResponses
    {
        public List<SurvEntryPicksDetails> SurvEntries { get; set; }
    }

    public class PickbyEntryNotification
    {
        public int? ScheduleID { get; set; }
        public int? PoolID { get; set; }
        public string Pool_Name { get; set; }
        public int? WeekNumber { get; set; }
        public string Description { get; set; }
        public int? HomeTeam { get; set; }
        public string HomeTeamName { get; set; }
        public string HomeTeamNameShort { get; set; }
        public string HomeLogoImageSrc { get; set; }
        public int? VisitingTeam { get; set; }
        public string VisitingTeamName { get; set; }
        public string VisitingTeamNameShort { get; set; }
        public string VisitingLogoImageSrc { get; set; }
        public int? Winner { get; set; }
        public string ActualWinnerTeamName { get; set; }
        public string ActualWinnerTeamNameShort { get; set; }
        public string ActualWinnerLogoImageSrc { get; set; }
        public string PickedWinnerTeamName { get; set; }
        public string PickedWinnerTeamNameShort { get; set; }
        public string PickedWinnerLogoImageSrc { get; set; }
        public DateTime CutOff { get; set; }
        public bool Start { get; set; }
        public int? EntryID { get; set; }
        public string EntryName { get; set; }
        public string First_Name { get; set; }
        public string Email_Address { get; set; }
    }

    public class Defaulted_Report
    {
        public string EntryName { get; set; }
        public string Pool_Name { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email_Address { get; set; }
        public int PoolID { get; set; }
        public int MemberID { get; set; }
        public int EntryID { get; set; }

    }
    public class Defaulted_ReportAPIResponse : APIResponses
    {
        public List<Defaulted_Report> Defaulted_Report { get; set; }
    }
}
