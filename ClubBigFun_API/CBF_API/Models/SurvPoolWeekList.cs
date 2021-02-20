using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CBF_API.Models
{
    public class SurvPoolWeekList
    {
        [Column(Order = 0), Key]
        public int PoolID { get; set; }
        [Column(Order = 1), Key]
        public  int WeekNumber { get; set; }
        public DateTime? CutOff { get; set; }
        public bool Start { get; set; }
        public DateTime? Updated { get; set; }
         
    }
    public class SurvPoolWeekListResponse: APIResponses
    {
        public List<SurvPoolWeekList> SurvPoolWeekList { get; set; }
    }
    public class SurvPoolWeekListMapped
    {
       // [Column(Order = 0), Key]
        public int PoolID { get; set; }
      //  [Column(Order = 1), Key]
        public  int WeekNumber { get; set; }
        public DateTime? CutOff { get; set; }
        public bool Start { get; set; }
        public DateTime? Updated { get; set; }
        [NotMapped]
        public string CutOffDateString
        {
            get
            {
                return Convert.ToDateTime(CutOff).ToString("dddd, dd MMMM yyyy hh:mm tt");
            }
        }
        public string Pool_Name { get; set; }
    }
}
