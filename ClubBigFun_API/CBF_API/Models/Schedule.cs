using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBF_API.Models
{
    public class Schedule
    {
    }
    public class ScoreDetail
    {
        public string Description { get; set; }
        public int NFLScheduleID { get; set; }
        public string HomeTeam { get; set; }
        public string VisitingTeam { get; set; }
        public string AwayScoreTotal { get; set; }
        public string HomeScoreTotal { get; set; }
    }
}
