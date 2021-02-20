using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CBF_API.Models
{
    public class SurvScheduleList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ScheduleID { get; set; }
        public int PoolID { get; set; }
        public int WeekNumber { get; set; }
        [StringLength(50)]
        public string Description { get; set; }
        public int HomeTeam { get; set; }
        public int VisitingTeam { get; set; }
        public int? Winner { get; set; }
        public int? NFLScheduleID { get; set; }
    }

    public class SurvScheduleListResponse : APIResponses
    {
        public SurvScheduleList survSchedule { get; set; }
    }
}
