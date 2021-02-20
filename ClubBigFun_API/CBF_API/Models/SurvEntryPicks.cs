using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CBF_API.Models
{
    public class SurvEntryPicks
    {
        [Column(Order = 0), Key]
        public int EntryID { get; set; }
        [Column(Order = 1), Key]
        public int ScheduleID { get; set; }
        public int Winner { get; set; }
        public bool Eliminated { get; set; }
        public bool Defaulted { get; set; }
        public DateTime Created { get; set; }
        public int WeekNumber { get; set; }
        public bool WinnerNotificationSent { get; set; }
        public bool EliminatedNotificationSent { get; set; }
        public bool DefaultedNotificationSent { get; set; }
    }
    public class qrySurvScheduleList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ScheduleID { get; set; }

        public int PoolID { get; set; }

        public string Pool_Name { get; set; }
        public int WeekNumber { get; set; }
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
        public string WinnerTeamName { get; set; }
        public string WinnerTeamNameShort { get; set; }
        public string WinnerLogoImageSrc { get; set; }
        public DateTime CutOff { get; set; }
        public bool Start { get; set; }

    }
    public class SurvEntries_WithoutPicks
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EntryID { get; set; }
        public string EntryName { get; set; }
        //  public int PoolId { get; set; }
    }
    public class SurvEntries_WithoutPicksResponses : APIResponses
    {
        public List<qrySurvEntriesNew> SurvEntries_WithoutPicks { get; set; }
    }
    [NotMapped]
    public class SurvEntryPicksDetails
    {
        [Key]
        public int EntryID { get; set; }
        public string Pool_Name { get; set; }
        public int? PoolID { get; set; }
        public int? PickTeamId { get; set; }
        public string EntryName { get; set; }
        public bool Eliminated { get; set; }
        public bool Defaulted { get; set; }
        public DateTime LastUpdated { get; set; }

        public DateTime Cut_Off { get; set; }

        public int? ScheduleID { get; set; }
        public string PickName { get; set; }

        public int? WeekNumber { get; set; }
        public bool Is_Started { get; set; }
        public bool WeekClosed { get; set; }
        public string LogoImageSrc { get; set; }
        public string CutOffDateString
        {
            get
            {
                return Convert.ToDateTime(this.Cut_Off).AddMinutes(-1).ToString("dddd, dd MMMM yyyy hh:mm tt");
            }
        }

    }
    public class PickRequest
    {
        public int EntryID { get; set; }
        public int ScheduleID { get; set; }
        public int YourWinner { get; set; }
        public int WeekNumber { get; set; }
    }
    public class PickReport
    {
        [Key]
        public int TicketID { get; set; }
        public string Ticket { get; set; }
        public string Pick { get; set; }
        public string Date { get; set; }
        public string Eliminated { get; set; }
        public string Defaulted { get; set; }
        public int Defaults { get; set; }

    }

    public class CurrentWeek
    {
        public int WeekNumber { get; set; }
    }

    public class CurrentWeekResponses : APIResponses
    {
        public CurrentWeek WeekNumber { get; set; }
    }

    public class PickReportWithLogo
    {
        public int EntryID { get; set; }
        public int WeekNumber { get; set; }
        public string LogoImageSrc { get; set; }

    }

    public class PickReportCount
    {
        public int TotalEliminatedCount { get; set; }
        public int TotalAliveCount { get; set; }
        public int TotalEntriesCount { get; set; }
    }

    public class PickReportResponses : APIResponses
    {
        public List<PickReport> PickReport { get; set; }
    }
    public class PickAnalysisWithTeamResponses : APIResponses
    {
        public List<PickAnalysis> PickAnalysis { get; set; }
        public List<PickAnalysis> PickAnalysisFlag { get; set; }
        public PickReportCount PickReportCount { get; set; }
    }

    public class PickAnalysisWithTeamDynamicResponses : APIResponses
    {
        public dynamic PickReportAnalysis { get; set; }
    }

    public class PickAnalysisAliveByMemberResponses : APIResponses
    {
        public List<TotalAliveByMemberReport> PickAnalysisAlive { get; set; }
        public List<PickAnalysis> PickAnalysisFlag { get; set; }
        public PickReportCount PickReportCount { get; set; }
    }
    public class PickAnalysisEliminatedByMemberResponses : APIResponses
    {
        public List<TotalEliminatedByMemberReport> PickAnalysisEliminated { get; set; }
        public PickReportCount PickReportCount { get; set; }
    }
    public class PickReportWithLogoResponses : APIResponses
    {
        public List<PickReportWithLogo> PickReport { get; set; }
    }
    public class PickCenterSchedule : APIResponses
    {
        public List<SchedulesGroupDates> SchedulesGroup { get; set; }
    }
    public class SchedulesGroupDates
    {
        public string ScheduleDateString { get; set; }
        public DateTime ScheduleDate { get; set; }
        public List<ScheduleGroupTime> ScheduleGroupTime { get; set; }
    }
    public class ScheduleGroupTime
    {
        public string ScheduleTimeString { get; set; }
        public TimeSpan ScheduleTime { get; set; }
        public List<PickCenterSchedulesList> ScheduleWeekLists { get; set; }
    }
    public class PickCenterSchedulesList
    {
        [Key]
        public int ScheduleID { get; set; }
        public int PoolID { get; set; }
        public string Pool_Name { get; set; }
        public int WeekNumber { get; set; }
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
        public string WinnerTeamName { get; set; }
        public string WinnerTeamNameShort { get; set; }
        public string WinnerLogoImageSrc { get; set; }
        public DateTime CutOff { get; set; }
        public bool Start { get; set; }

    }
    public class WeeklyDefaults
    {
        public int ID { get; set; }
        public int Rank { get; set; }
        [Key]
        public int ScheduleID { get; set; }
        public int PoolID { get; set; }
        public string Pool_Name { get; set; }
        public int WeekNumber { get; set; }
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
        public string WinnerTeamName { get; set; }
        public string WinnerTeamNameShort { get; set; }
        public string WinnerLogoImageSrc { get; set; }
        public DateTime CutOff { get; set; }
        public bool Start { get; set; }
    }
    public class ScheduleGroupByDateTime
    {
        public DateTime? Date { get; set; }
        public TimeSpan Time { get; set; }
        public int ScheduleId { get; set; }
    }
}
