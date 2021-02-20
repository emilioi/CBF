using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CBF_API.Models
{
    public class WeeksMenuAPIResponse : APIResponses
    {
        public List<WeekMenu> weekMenus { get; set; }
    }
    public class PoolWeekListAPIResponse : APIResponses
    {
        public List<SurvPoolWeekList> survPoolWeekLists { get; set; }
    }

    public class WeeksNumberAPIResponse : APIResponses
    {
        public List<int> weekNumbers { get; set; }
    }
    public class WeekMenu
    {
        [Key]
        public int Pool_ID { get; set; }
        public string Pool_Name { get; set; }
        public int WeekCount { get; set; }
    }

    public class PoolWeekRequest
    {
        public int Pool_ID { get; set; }
        public string Pool_Name { get; set; }
        public short Week_Number { get; set; }
        public DateTime? Cut_Off_Date { get; set; }
        public bool Start { get; set; }
    }

    public class SurvEntries_WithoutPicks_All
    {
        public int? EntryID { get; set; }
        public int? WeekNumber { get; set; }
        public int? PoolID { get; set; }
        public int? DefaultTeamID { get; set; }
        public string Email_Address { get; set; }
        public int? Member_Id { get; set; }
        public int? ScheduleID { get; set; }
        public string EntryName { get;  set; }
        public string DefaultTeamName { get;  set; }
    }
}
