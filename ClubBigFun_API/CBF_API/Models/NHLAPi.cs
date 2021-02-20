using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace CBF_API.Models
{
    public class NHLSchedule
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
    }
    public class NHLTeam
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
    }
    public class NHLVenue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Venue_ID { get; set; }
        public string Venue_Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string GeoCoordinates { get; set; }
    }
    public class NHLScore
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
    public class NHLFilter
    {
        public int Id { get; set; }
        public string Week { get; set; }
        public string StartTime { get; set; }
        public string EndedTime { get; set; }
    }
    public class NHLFilterResponses : APIResponses
    {
        public List<NHLFilter> NHLFilters { get; set; }
    }
    public class NHLScheduleResponses : APIResponses
    {
        public List<NHLSchedule> NHLSchedule { get; set; }
    }
    public class NHLScheduleList
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
    public class NHLScheduleListResponses : APIResponses
    {
        public List<NHLScheduleList> NHLScheduleList { get; set; }
    }
    
}

