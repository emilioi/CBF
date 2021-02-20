﻿// <auto-generated />
using System;
using CBF_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CBF_API.Migrations
{
    [DbContext(typeof(CbfDbContext))]
    [Migration("20190731174334_pooldefault")]
    partial class pooldefault
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CBF_API.Models.Administrators", b =>
                {
                    b.Property<int>("Member_Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Admin_Type")
                        .HasMaxLength(100);

                    b.Property<DateTime>("DTS");

                    b.Property<string>("Email_Address")
                        .HasMaxLength(100);

                    b.Property<int>("Failed_Attempt");

                    b.Property<string>("First_Name")
                        .HasMaxLength(100);

                    b.Property<string>("Image_Name");

                    b.Property<string>("Image_Url");

                    b.Property<bool>("Is_Active");

                    b.Property<bool>("Is_Deleted");

                    b.Property<bool>("Is_Email_Verified");

                    b.Property<bool>("Is_Locked");

                    b.Property<bool>("Is_Temp_Password");

                    b.Property<int>("Last_Edited_By");

                    b.Property<DateTime>("Last_Failed_Login");

                    b.Property<DateTime>("Last_Login");

                    b.Property<string>("Last_Name")
                        .HasMaxLength(100);

                    b.Property<string>("Login_Id")
                        .HasMaxLength(100);

                    b.Property<string>("Password");

                    b.Property<string>("Phone_Number")
                        .HasMaxLength(40);

                    b.HasKey("Member_Id");

                    b.ToTable("Administrators");
                });

            modelBuilder.Entity("CBF_API.Models.Club", b =>
                {
                    b.Property<int>("Pool_ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Cut_Off");

                    b.Property<DateTime>("DTS");

                    b.Property<string>("Description")
                        .HasMaxLength(250);

                    b.Property<bool>("Is_Active");

                    b.Property<bool>("Is_Deleted");

                    b.Property<bool>("Is_Started");

                    b.Property<int>("Last_Edited_By");

                    b.Property<string>("Pool_Name")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<decimal>("Price");

                    b.Property<bool>("Ruler_Season");

                    b.Property<string>("Rules_URL")
                        .HasMaxLength(250);

                    b.Property<int>("Sport_Type");

                    b.HasKey("Pool_ID");

                    b.ToTable("Clubs");
                });

            modelBuilder.Entity("CBF_API.Models.Email_Notification", b =>
                {
                    b.Property<int>("Notification")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DTS");

                    b.Property<string>("Email_Content");

                    b.Property<DateTime>("Failed_Error")
                        .HasMaxLength(500);

                    b.Property<string>("From_Email")
                        .HasMaxLength(100);

                    b.Property<bool>("Is_Sent");

                    b.Property<int>("Member_Id");

                    b.Property<string>("Sent_On");

                    b.Property<string>("Subject")
                        .HasMaxLength(500);

                    b.Property<string>("To_Email")
                        .HasMaxLength(100);

                    b.HasKey("Notification");

                    b.ToTable("Email_Notification");
                });

            modelBuilder.Entity("CBF_API.Models.Email_Templates", b =>
                {
                    b.Property<int>("Template_Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DTS");

                    b.Property<bool>("Is_Deleted");

                    b.Property<int>("Last_Edited_By");

                    b.Property<string>("Template_Description")
                        .IsRequired();

                    b.Property<string>("Template_For")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<string>("Template_Subject")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.HasKey("Template_Id");

                    b.ToTable("Email_Templates");
                });

            modelBuilder.Entity("CBF_API.Models.EntryMenu", b =>
                {
                    b.Property<int>("Pool_ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Pool_Name");

                    b.Property<int>("TheCount");

                    b.HasKey("Pool_ID");

                    b.ToTable("EntryMenus");
                });

            modelBuilder.Entity("CBF_API.Models.Lookups", b =>
                {
                    b.Property<int>("Lookup_Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Is_Deleted");

                    b.Property<string>("Lookup_Name")
                        .HasMaxLength(80);

                    b.Property<string>("Lookup_Type")
                        .HasMaxLength(80);

                    b.Property<string>("Lookup_Value")
                        .HasMaxLength(80);

                    b.HasKey("Lookup_Id");

                    b.ToTable("Lookups");
                });

            modelBuilder.Entity("CBF_API.Models.MailingList", b =>
                {
                    b.Property<int>("MailingList_ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<string>("Referer");

                    b.HasKey("MailingList_ID");

                    b.ToTable("MailingList");
                });

            modelBuilder.Entity("CBF_API.Models.Members", b =>
                {
                    b.Property<int>("Member_Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address_Line1")
                        .HasMaxLength(100);

                    b.Property<string>("Address_Line2")
                        .HasMaxLength(100);

                    b.Property<string>("City")
                        .HasMaxLength(50);

                    b.Property<string>("Country")
                        .HasMaxLength(10);

                    b.Property<DateTime>("DTS");

                    b.Property<string>("Email_Address")
                        .HasMaxLength(100);

                    b.Property<int>("Failed_Attempt");

                    b.Property<string>("First_Name")
                        .HasMaxLength(100);

                    b.Property<string>("Image_Name");

                    b.Property<string>("Image_Url");

                    b.Property<bool>("Is_Active");

                    b.Property<bool>("Is_Deleted");

                    b.Property<bool>("Is_Email_Verified");

                    b.Property<bool>("Is_Locked");

                    b.Property<bool>("Is_Temp_Password");

                    b.Property<int>("Last_Edited_By");

                    b.Property<DateTime>("Last_Failed_Login");

                    b.Property<DateTime>("Last_Login");

                    b.Property<string>("Last_Name")
                        .HasMaxLength(100);

                    b.Property<string>("Login_Id")
                        .HasMaxLength(100);

                    b.Property<string>("Password");

                    b.Property<string>("Phone_Number")
                        .HasMaxLength(40);

                    b.Property<string>("State")
                        .HasMaxLength(100);

                    b.Property<string>("User_Type")
                        .HasMaxLength(100);

                    b.HasKey("Member_Id");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("CBF_API.Models.MostPickedList", b =>
                {
                    b.Property<int>("Pool_Id");

                    b.Property<int>("Team_Id");

                    b.Property<string>("Abbreviation");

                    b.Property<string>("LogoImageSrc");

                    b.Property<string>("SportTypeName");

                    b.Property<int>("Sport_Type");

                    b.Property<string>("Team_Name");

                    b.HasKey("Pool_Id", "Team_Id");

                    b.ToTable("MostPickedLists");
                });

            modelBuilder.Entity("CBF_API.Models.NFLSchedule", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Attendance");

                    b.Property<string>("Broadcasters");

                    b.Property<DateTime?>("CutOffDate");

                    b.Property<string>("DelayedOrPostponedReason");

                    b.Property<string>("EndedTime");

                    b.Property<DateTime>("GameDate");

                    b.Property<int>("HomeTeamId");

                    b.Property<string>("HomeTeamShort");

                    b.Property<string>("Officials");

                    b.Property<string>("OriginalStartTime");

                    b.Property<string>("PlayedStatus");

                    b.Property<string>("ScheduleStatus");

                    b.Property<string>("StartTime");

                    b.Property<string>("VenueAllegiance");

                    b.Property<int>("Venue_ID");

                    b.Property<int>("VisitingTeamID");

                    b.Property<string>("VisitingTeamShort");

                    b.Property<string>("Weather");

                    b.Property<string>("Week");

                    b.HasKey("Id");

                    b.ToTable("NFLSchedule");
                });

            modelBuilder.Entity("CBF_API.Models.NFLScheduleList", b =>
                {
                    b.Property<string>("Week");

                    b.Property<DateTime>("GameDate");

                    b.Property<DateTime>("CutOffDate");

                    b.Property<string>("HomeTeamLogo");

                    b.Property<string>("HomeTeamName");

                    b.Property<string>("HomeTeamSortName");

                    b.Property<string>("VenueTeamLogo");

                    b.Property<string>("Venue_Name");

                    b.Property<string>("VisitingTeamName");

                    b.Property<string>("VisitingTeamSortName");

                    b.HasKey("Week", "GameDate", "CutOffDate");

                    b.ToTable("NFLScheduleList");
                });

            modelBuilder.Entity("CBF_API.Models.NFLScore", b =>
                {
                    b.Property<int>("Score_Id");

                    b.Property<string>("AwayScoreTotal");

                    b.Property<string>("CurrentDown");

                    b.Property<string>("CurrentIntermission");

                    b.Property<string>("CurrentQuarter");

                    b.Property<string>("CurrentQuarterSecondsRemaining");

                    b.Property<string>("CurrentYardsRemaining");

                    b.Property<string>("HomeScoreTotal");

                    b.Property<string>("LineOfScrimmage");

                    b.Property<string>("Quarters");

                    b.Property<string>("TeamInPossession");

                    b.HasKey("Score_Id");

                    b.ToTable("NFLScore");
                });

            modelBuilder.Entity("CBF_API.Models.NFLTeam", b =>
                {
                    b.Property<int>("Team_Id");

                    b.Property<string>("Abbreviation");

                    b.Property<string>("City");

                    b.Property<string>("LogoImageSrc");

                    b.Property<string>("SportType");

                    b.Property<string>("Team_Name");

                    b.Property<string>("Venue_ID");

                    b.HasKey("Team_Id");

                    b.ToTable("NFLTeam");
                });

            modelBuilder.Entity("CBF_API.Models.NFLVenue", b =>
                {
                    b.Property<int>("Venue_ID");

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<string>("GeoCoordinates");

                    b.Property<string>("Venue_Name");

                    b.HasKey("Venue_ID");

                    b.ToTable("NFLVenue");
                });

            modelBuilder.Entity("CBF_API.Models.PicksCount", b =>
                {
                    b.Property<int>("EntryID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("NumOfPicks");

                    b.Property<int>("PoolID");

                    b.HasKey("EntryID");

                    b.ToTable("PicksCount");
                });

            modelBuilder.Entity("CBF_API.Models.Pool_Defaults", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Pool_Id");

                    b.Property<int>("Rank");

                    b.Property<int>("Schedule_Id");

                    b.Property<int>("Team_Id");

                    b.Property<int>("WeekNumber");

                    b.HasKey("Id");

                    b.ToTable("Pool_Defaults");
                });

            modelBuilder.Entity("CBF_API.Models.Pool_Master", b =>
                {
                    b.Property<int>("Pool_ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Cut_Off");

                    b.Property<DateTime>("DTS");

                    b.Property<string>("Description")
                        .HasMaxLength(250);

                    b.Property<bool>("Is_Active");

                    b.Property<bool>("Is_Deleted");

                    b.Property<bool>("Is_Started");

                    b.Property<int>("Last_Edited_By");

                    b.Property<string>("Pool_Name")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<decimal>("Price");

                    b.Property<bool>("Ruler_Season");

                    b.Property<string>("Rules_URL")
                        .HasMaxLength(250);

                    b.Property<int>("Sport_Type");

                    b.HasKey("Pool_ID");

                    b.ToTable("Pool_Master");
                });

            modelBuilder.Entity("CBF_API.Models.qrySurvEntries", b =>
                {
                    b.Property<int>("PoolID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<int>("Defaults");

                    b.Property<bool>("Eliminated");

                    b.Property<int>("EntryID");

                    b.Property<string>("EntryName");

                    b.Property<string>("FullName");

                    b.Property<string>("Login_ID");

                    b.Property<int>("MemberID");

                    b.Property<string>("Pool_Name");

                    b.Property<decimal>("Price");

                    b.HasKey("PoolID");

                    b.ToTable("qrySurvEntries");
                });

            modelBuilder.Entity("CBF_API.Models.qrySurvScheduleList", b =>
                {
                    b.Property<int>("ScheduleID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CutOff");

                    b.Property<string>("Description");

                    b.Property<string>("HomeLogoImageSrc");

                    b.Property<int>("HomeTeam");

                    b.Property<string>("HomeTeamName");

                    b.Property<string>("HomeTeamNameShort");

                    b.Property<int>("PoolID");

                    b.Property<string>("Pool_Name");

                    b.Property<bool>("Start");

                    b.Property<string>("VisitingLogoImageSrc");

                    b.Property<int>("VisitingTeam");

                    b.Property<string>("VisitingTeamName");

                    b.Property<string>("VisitingTeamNameShort");

                    b.Property<int>("WeekNumber");

                    b.Property<string>("Winner");

                    b.Property<string>("WinnerLogoImageSrc");

                    b.Property<string>("WinnerTeamName");

                    b.Property<string>("WinnerTeamNameShort");

                    b.HasKey("ScheduleID");

                    b.ToTable("qrySurvScheduleList");
                });

            modelBuilder.Entity("CBF_API.Models.ScheduleMenu", b =>
                {
                    b.Property<int>("Pool_ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Pool_Name");

                    b.Property<int>("theCount");

                    b.HasKey("Pool_ID");

                    b.ToTable("ScheduleMenus");
                });

            modelBuilder.Entity("CBF_API.Models.Sports_Type", b =>
                {
                    b.Property<int>("SportType");

                    b.Property<string>("SportTypeName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("SportType");

                    b.ToTable("Sports_Type");
                });

            modelBuilder.Entity("CBF_API.Models.SurvEntries", b =>
                {
                    b.Property<int>("EntryID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<DateTime>("CreatedDT");

                    b.Property<int>("Defaults");

                    b.Property<bool>("Eliminated");

                    b.Property<string>("EntryName")
                        .HasMaxLength(50);

                    b.Property<DateTime>("LastUpdated");

                    b.Property<int>("MemberID");

                    b.Property<int>("PoolID");

                    b.HasKey("EntryID");

                    b.ToTable("SurvEntries");
                });

            modelBuilder.Entity("CBF_API.Models.SurvEntryPicks", b =>
                {
                    b.Property<int>("EntryID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created");

                    b.Property<bool>("Defaulted");

                    b.Property<bool>("Eliminated");

                    b.Property<int>("ScheduleID");

                    b.Property<int>("Winner");

                    b.HasKey("EntryID");

                    b.ToTable("SurvEntryPicks");
                });

            modelBuilder.Entity("CBF_API.Models.SurvPoolWeekList", b =>
                {
                    b.Property<int>("PoolID");

                    b.Property<int>("WeekNumber");

                    b.Property<DateTime?>("CutOff");

                    b.Property<bool>("Start");

                    b.Property<DateTime?>("Updated");

                    b.HasKey("PoolID", "WeekNumber");

                    b.ToTable("survPoolWeekList");
                });

            modelBuilder.Entity("CBF_API.Models.SurvPoolWeekListMapped", b =>
                {
                    b.Property<int>("PoolID");

                    b.Property<int>("WeekNumber");

                    b.Property<DateTime?>("CutOff");

                    b.Property<string>("Pool_Name");

                    b.Property<bool>("Start");

                    b.Property<DateTime?>("Updated");

                    b.HasKey("PoolID", "WeekNumber");

                    b.ToTable("SurvPoolWeekListMapped");
                });

            modelBuilder.Entity("CBF_API.Models.SurvScheduleList", b =>
                {
                    b.Property<int>("ScheduleID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasMaxLength(50);

                    b.Property<int>("HomeTeam");

                    b.Property<int>("PoolID");

                    b.Property<int>("VisitingTeam");

                    b.Property<int>("WeekNumber");

                    b.Property<int>("Winner");

                    b.HasKey("ScheduleID");

                    b.ToTable("SurvScheduleList");
                });

            modelBuilder.Entity("CBF_API.Models.Team_List", b =>
                {
                    b.Property<int>("TeamID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DTS");

                    b.Property<bool>("Is_Deleted");

                    b.Property<int>("Last_Edited_By");

                    b.Property<int>("SportType");

                    b.Property<string>("TeamLogo")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("TeamName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("TeamNameShort")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("TeamID");

                    b.ToTable("Team_List");
                });

            modelBuilder.Entity("CBF_API.Models.TellAFriend", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<DateTime>("CreatedDT");

                    b.Property<string>("FriendEmail")
                        .HasMaxLength(256);

                    b.Property<string>("FriendName")
                        .HasMaxLength(256);

                    b.Property<string>("Referer")
                        .HasMaxLength(256);

                    b.Property<string>("YourEmail")
                        .HasMaxLength(256);

                    b.Property<string>("YourName")
                        .HasMaxLength(256);

                    b.HasKey("ID");

                    b.ToTable("TellAFriend");
                });

            modelBuilder.Entity("CBF_API.Models.UserSession", b =>
                {
                    b.Property<long>("TokenID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AppDomainName");

                    b.Property<string>("BrowserUniqueID");

                    b.Property<bool>("Expired");

                    b.Property<DateTime?>("ExpiredOn");

                    b.Property<DateTime?>("IssuedOn");

                    b.Property<string>("Key");

                    b.Property<string>("RequestBrowsertypeVersion");

                    b.Property<string>("SessionGUID");

                    b.Property<string>("UserHostIPAddress");

                    b.Property<int>("UserID");

                    b.HasKey("TokenID");

                    b.ToTable("UserSession");
                });

            modelBuilder.Entity("CBF_API.Models.WeekMenu", b =>
                {
                    b.Property<int>("Pool_ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Pool_Name");

                    b.Property<int>("WeekCount");

                    b.HasKey("Pool_ID");

                    b.ToTable("WeekMenu");
                });
#pragma warning restore 612, 618
        }
    }
}
