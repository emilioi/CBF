using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CBF_API.Models
{
    public class CbfDbContext : DbContext
    {
        public CbfDbContext()
        {

        }
        public CbfDbContext(DbContextOptions<CbfDbContext> options)
            : base(options)
        {

        }
        public DbSet<Countries> Countries { get; set; }
        public DbSet<States> States { get; set; }
        public DbSet<Members> Members { get; set; }
        public DbSet<Email_Templates> Email_Templates { get; set; }
        public DbSet<UserSession> UserSession { get; set; }
        public DbSet<Pool_Master> Pool_Master { get; set; }
        public DbSet<Sports_Type> Sports_Type { get; set; }
        public DbSet<Team_List> Team_List { get; set; }
        public DbSet<Rules> Rules { get; set; }

        public DbSet<Member_Alerts> Member_Alerts { get; set; }
        public DbSet<Email_Notification> Email_Notification { get; set; }
        public DbSet<Lookups> Lookups { get; set; }
        public DbSet<MailingList> MailingList { get; set; }
        public DbSet<TellAFriend> TellAFriend { get; set; }
        public DbSet<NFLSchedule> NFLSchedule { get; set; }
        public DbSet<NFLScore> NFLScore { get; set; }
        public DbSet<NFLTeam> NFLTeam { get; set; }
        public DbSet<NFLVenue> NFLVenue { get; set; }

        /// <summary>
        /// 
        /// 
        /// 
        public DbSet<NHLSchedule> NHLSchedule { get; set; }
        public DbSet<NHLScore> NHLScore { get; set; }
        public DbSet<NHLTeam> NHLTeam { get; set; }
        public DbSet<NHLVenue> NHLVenue { get; set; }

        /// 
        /// 
        /// </summary>
        public DbSet<SurvEntries> SurvEntries { get; set; }
        public DbSet<SurvEntryPicks> SurvEntryPicks { get; set; }
        public DbSet<ErrorException> ErrorExceptions { get; set; }
        public DbSet<MemberAdminLink> MemberAdminLink { get; set; }
        //  public virtual DbSet<MostPickedList> MostPickedLists { get; set; }

        //public virtual DbSet<PicksCount> PicksCount { get; set; }
        public DbSet<SurvScheduleList> SurvScheduleList { get; set; }
        // public virtual DbSet<PickReport> PickReport { get; set; }
        //  public virtual DbSet<NFLScheduleList> NFLScheduleList { get; set; }

        // public virtual DbSet<WeekMenu> WeekMenu { get; set; }
        // public virtual DbSet<ScheduleMenu> ScheduleMenus { get; set; }
        // public virtual DbSet<EntryMenu> EntryMenus { get; set; }
        public DbSet<SurvPoolWeekList> survPoolWeekList { get; set; }
        // public virtual DbSet<qrySurvScheduleList> qrySurvScheduleList { get; set; }
        // public virtual DbSet<qrySurvEntries> qrySurvEntries { get; set; }
        // public virtual DbSet<SurvPoolWeekListMapped> SurvPoolWeekListMapped { get; set; }
        public DbSet<Administrators> Administrators { get; set; }
        public DbSet<Pool_Defaults> Pool_Defaults { get; set; }
        //  public virtual DbSet<Club> Clubs { get; set; }

        //  public virtual DbSet<TicketByPoolId> TicketByPoolId { get; set; }
        //  public virtual DbSet<SurvEntries_WithoutPicks> SurvEntries_WithoutPicks { get; set; }

        //  public virtual DbSet<SurvEntryPicksDetails> SurvEntryPicksDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<SurvPoolWeekList>()
              .HasKey(c => new { c.PoolID, c.WeekNumber });
            modelBuilder.Entity<Pool_Defaults>()
                .HasKey(c => new { c.Pool_Id, c.WeekNumber, c.Schedule_Id, c.Team_Id });
            modelBuilder.Entity<Pool_Defaults>().Property(e => e.Id).Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;
            modelBuilder.Entity<SurvEntryPicks>().HasKey(c => new { c.WeekNumber, c.EntryID });
            modelBuilder.Entity<MemberAdminLink>().HasKey(c => new { c.Member_ID, c.Admin_ID });
            modelBuilder.Query<NFLScheduleBySeasoncode>();
            modelBuilder.Query<NFLScheduleByWeek>();
            modelBuilder.Query<PickReportWithLogo>();
            modelBuilder.Query<SurvEntryPicksDetails>();
            modelBuilder.Query<SurvEntries_WithoutPicks>();
            modelBuilder.Query<TicketByPoolId>();
            modelBuilder.Query<Club>();
            modelBuilder.Query<SurvPoolWeekListMapped>();
            modelBuilder.Query<qrySurvEntries>();
            modelBuilder.Query<qrySurvEntriesNew>();
            modelBuilder.Query<qrySurvScheduleList>();
            modelBuilder.Query<EntryMenu>();
            modelBuilder.Query<ScheduleMenu>();
            modelBuilder.Query<WeekMenu>();
            modelBuilder.Query<NFLScheduleList>();
            modelBuilder.Query<PickReport>();
            modelBuilder.Query<PicksCount>();
            modelBuilder.Query<MostPickedList>();
            modelBuilder.Query<WeeklyDefaults>();
            modelBuilder.Query<Club>();
            //modelBuilder.Entity<SurvPoolWeekListMapped>()
            //  .HasKey(c => new { c.PoolID, c.WeekNumber });
            //modelBuilder.Entity<MostPickedList>()
            // .HasKey(c => new { c.Pool_Id, c.Team_Id });
            //modelBuilder.Entity<NFLScheduleList>()
            //    .HasKey(c => new { c.Week, c.GameDate, c.CutOffDate });
            modelBuilder.Query<ScheduleGroupByDateTime>();
            modelBuilder.Query<PickCenterSchedulesList>();
            modelBuilder.Query<EntryWithoutPicks>();
            modelBuilder.Query<NFLEliminationProcess>();
            modelBuilder.Query<SurvEntries_WithoutPicks_All>();
            modelBuilder.Query<PickbyEntryNotification>();
            modelBuilder.Query<ReferenceList>();
            modelBuilder.Query<ScoreDetail>();
            modelBuilder.Query<CustomInt>();
            modelBuilder.Query<CustomCount>();
            modelBuilder.Query<PickAnalysis>();
            modelBuilder.Query<TotalAliveByMemberReport>();
            modelBuilder.Query<TotalEliminatedByMemberReport>();

            modelBuilder.Query<Dashboard_Stats>();
            modelBuilder.Query<PoolEntryCount>();
            modelBuilder.Query<PickReportCount>();
            modelBuilder.Query<PickCustomCount>();
            modelBuilder.Query<Defaulted_Report>();
        }


    }

}