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
    [Migration("20190718162603_emails")]
    partial class emails
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.8-servicing-32085")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

                    b.Property<byte>("Is_Deleted");

                    b.Property<int>("Last_Edited_By");

                    b.Property<string>("Template_Description");

                    b.Property<string>("Template_For")
                        .HasMaxLength(250);

                    b.Property<string>("Template_Subject")
                        .HasMaxLength(250);

                    b.HasKey("Template_Id");

                    b.ToTable("Email_Templates");
                });

            modelBuilder.Entity("CBF_API.Models.NFL_Schedule", b =>
                {
                    b.Property<int>("WeekNumber")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CutOffDate");

                    b.Property<string>("Day");

                    b.Property<DateTime>("GameDate");

                    b.Property<string>("HomeTeam")
                        .HasMaxLength(50);

                    b.Property<int>("HomeTeamID");

                    b.Property<string>("HomeTeamShort")
                        .HasMaxLength(50);

                    b.Property<string>("VisitingTeam")
                        .HasMaxLength(50);

                    b.Property<int>("VisitingTeamID");

                    b.Property<string>("VisitingTeamShort")
                        .HasMaxLength(50);

                    b.HasKey("WeekNumber");

                    b.ToTable("NFL_Schedule");
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
                        .HasMaxLength(250);

                    b.Property<decimal>("Price");

                    b.Property<string>("Rules_URL")
                        .HasMaxLength(250);

                    b.Property<int>("Sport_Type");

                    b.HasKey("Pool_ID");

                    b.ToTable("Pool_Master");
                });

            modelBuilder.Entity("CBF_API.Models.Sports_Type", b =>
                {
                    b.Property<int>("SportType")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("SportTypeName")
                        .HasMaxLength(50);

                    b.HasKey("SportType");

                    b.ToTable("Sports_Type");
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
                        .HasMaxLength(50);

                    b.Property<string>("TeamName")
                        .HasMaxLength(50);

                    b.Property<string>("TeamNameShort")
                        .HasMaxLength(50);

                    b.HasKey("TeamID");

                    b.ToTable("Team_List");
                });

            modelBuilder.Entity("CBF_API.Models.Users", b =>
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

                    b.ToTable("Users");
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
#pragma warning restore 612, 618
        }
    }
}
