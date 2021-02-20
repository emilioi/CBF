using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CBF_API.Models
{
    public class NFLSchedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Week { get; set; }
        public string StartTime { get; set; }
        public string EndedTime { get; set; }
        public string VenueAllegiance { get; set; }
        public string ScheduleStatus { get; set; }
        public string OriginalStartTime { get; set; }
        public string DelayedOrPostponedReason { get; set; }
        public string PlayedStatus { get; set; }
        public string Attendance { get; set; }
        public string Officials { get; set; }
        public string Broadcasters { get; set; }
        public string Weather { get; set; }
        public int HomeTeamId { get; set; }
        public int VisitingTeamID { get; set; }
        public string HomeTeamShort { get; set; }
        public string VisitingTeamShort { get; set; }
        public DateTime GameDate { get; set; }
        public DateTime? CutOffDate { get; set; }
        public int Venue_ID { get; set; }
        public string SeasonCode { get; set; }
    }
    public class NFLTeam
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Team_Id { get; set; }
        public string Abbreviation { get; set; }
        public string SportType { get; set; }
        public string City { get; set; }
        public string Team_Name { get; set; }
        public string Venue_ID { get; set; }
        public string LogoImageSrc { get; set; }

        public string SportTypeName
        {
            get
            {
                if(SportType == "1")
                {
                    return "NFL";
                }else if (SportType == "2")
                {
                    return "NHL";
                }
                else
                {
                    return "";
                }
            }
        }
    }
    public class NFLVenue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Venue_ID { get; set; }
        public string Venue_Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string GeoCoordinates { get; set; }
    }
    public class NFLScore
    {

        public string CurrentQuarter { get; set; }
        public string CurrentQuarterSecondsRemaining { get; set; }
        public string CurrentIntermission { get; set; }
        public string TeamInPossession { get; set; }
        public string CurrentDown { get; set; }
        public string CurrentYardsRemaining { get; set; }
        public string LineOfScrimmage { get; set; }
        public string AwayScoreTotal { get; set; }
        public string HomeScoreTotal { get; set; }
        public string Quarters { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Schedule_Id { get; set; }
    }
    public class NFLFilter
    {
        public int Id { get; set; }
        public string Week { get; set; }
        public string StartTime { get; set; }
        public string EndedTime { get; set; }
    }
    public class NFLFilterResponses : APIResponses
    {
        public List<NFLFilter> NFLFilters { get; set; }
    }
    public class NFLScheduleResponses : APIResponses
    {
        public List<NFLSchedule> NFLSchedule { get; set; }
    }
    public class NFLScheduleResponse : APIResponses
    {
        public NFLSchedule NFLSchedule { get; set; }
    }
    public class NFLScheduleList
    {

        public string Week { get; set; }
        public DateTime GameDate { get; set; }
        public DateTime CutOffDate { get; set; }
        public string HomeTeamName { get; set; }
        public string HomeTeamSortName { get; set; }
        public string VisitingTeamName { get; set; }
        public string VisitingTeamSortName { get; set; }
        public string Venue_Name { get; set; }
        public string HomeTeamLogo { get; set; }
        public string VenueTeamLogo { get; set; }
    }
    public class NFLScheduleListResponses : APIResponses
    {
        public List<NFLScheduleList> NFLScheduleList { get; set; }
    }
    public class ScheduleMenuAPIResponse : APIResponses
    {
        public List<ScheduleMenu> scheduleMenus { get; set; }
    }
    public class ScheduleMenu
    {
        [Key]
        public int Pool_ID { get; set; }
        public string Pool_Name { get; set; }
        public int theCount { get; set; }
    }
    public class ScheduleWeekListAPIResponse : APIResponses
    {
        public List<qrySurvScheduleList> ScheduleWeekLists { get; set; }
        public int TotalCount { get; set; }
    }
    public class SurvScheduleListAPIResponse : APIResponses
    {
        public SurvScheduleList survScheduleList { get; set; }
    }
    public class PoolScheduleRequest
    {
        public int Pool_ID { get; set; }
        public int ScheduleID { get; set; }
        public string Pool_Name { get; set; }
        public int Week_Number { get; set; }
        public string Description { get; set; }
        public int VisitingTeamID { get; set; }
        public int HomeTeamID { get; set; }
        public int? WinnerTeamID { get; set; }

    }
    public class NFLScheduleBySeasoncode
    {
        public string seasoncode { get; set; }
    }
    public class NFLSheduleByWeekResponse:APIResponses
    {
        public List<NFLScheduleByWeek> WeekList { get; set; }
    }
    public class NFLSheduleBySeasoncodeResponse : APIResponses
    {
        public List<NFLScheduleBySeasoncode> SeasoncodeList { get; set; }
    }
    public class NFLScheduleByWeek
    {
      
        public int week { get; set; }
    }
    public class NFLScheduleFilter :APIResponses
    {
        public List<NFLSchedule> nFLSchedules { get; set; }
    }
    public class PoolDefaultAPIRequest
    {

        public List<WeeklyDefaults> weeklyDefaults { get; set; }
    }
    public class PoolDefaultAPIResponse : APIResponses
    {
        public List<WeeklyDefaults> weeklyDefaults { get; set; }
    }

    public class currentWeek : APIResponses
    {
        public currentWeek currentWeekNo { get; set; }
    }

    public class Pool_Defaults
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column(Order = 0), Key]
        public int Pool_Id { get; set; }
        [Column(Order = 1), Key]
        public int WeekNumber { get; set; }
        [Column(Order = 2), Key]
        public int Schedule_Id { get; set; }
        [Column(Order = 3), Key]
        public int Team_Id { get; set; }
        public int Rank { get; set; }
    }

    public class ImageBase64
    {
        public string base64image { get; set; }
        public string fileExtention { get; set; }
        public string fileName { get; set; }
    }

    public class NFLEliminationProcess
    {

        public int PoolId { get; set; }

        public int WeekNumber { get; set; }
        public int? ScheduleId { get; set; }
        public int? MemberID { get; set; }
        public int? EntryId { get; set; }
        public string EntryName { get; set; }
        public string ActualWinner { get; set; }
        public string YourWinner { get; set; }
        public string Email_Address { get; set; }
    }
}
