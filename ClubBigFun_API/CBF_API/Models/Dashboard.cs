using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBF_API.Models
{
    public class Dashboard
    {
        public int PoolCount { get; set; }
        public int MemberCount { get; set; }
        public int MailingCount { get; set; }
        public int AllAdminCount { get; set; }
        public int SuperAdminCount { get; set; }
        public int AdminCount { get; set; }
        public int GroupAdminCount { get; set; }
        public List<Administrators> LatestLogins { get; set; }
        public int RececntMembers { get; set; }
        public List<NFLSchedule> UpcomingMatches { get; set; }
        public List<Administrators> AdminTypeCount { get; set; }
        //
        public Dashboard_Stats dashboard_Stats { get; set; }

        public List<PoolEntryCount> poolEntryCounts { get; set; }

    }
    public class DashboardResponses : APIResponses
    {
        public Dashboard Dashboard { get; set; }
    }
    public class PoolEntryCount
    {
        public int Pool_ID { get; set; }
        public string Pool_Name { get; set; }
        public int Entries_Count { get; set; }
    }
    public class Dashboard_Stats
    {
        public int Online_Users { get; set; }
        public int Total_Entries { get; set; }
        public int Total_Entries_Eliminated { get; set; }
        public int Total_Entries_Alive { get; set; }
    }
}
