using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CBF_API.Models
{
    public class Member_Alerts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Alert_Id { get; set; }
        public string Alert_Name { get; set; }
        public string Alert_Description { get; set; }
        public DateTime ReminderStart { get; set; }
        public DateTime ReminderEnd { get; set; }
        public string AlertColor { get; set; }
        public bool Is_AfterLogin { get; set; }
        public bool One_TimeReminder { get; set; }
        public bool IsExpired { get; set; }
    }
    public class Member_AlertsResponses :APIResponses
    {
        public Member_Alerts Member_Alerts { get; set; }
    }
    public class Member_AlertsListResponses : APIResponses
    {
        public List<Member_Alerts> Member_Alerts { get; set; }
    }
    public class Member_AlertsListAllResponses : APIResponses
    {
        public List<Member_Alerts> Member_Alerts { get; set; }
    }
    public class Member_AlertsByIdResponses : APIResponses
    {
        public Member_Alerts Member_Alerts { get; set; }
    }
}
