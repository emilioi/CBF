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
    [Migration("20190715115611_email_templates---")]
    partial class email_templates
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.8-servicing-32085")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

                    b.Property<byte>("Is_Active");

                    b.Property<byte>("Is_Deleted");

                    b.Property<byte>("Is_Email_Verified");

                    b.Property<byte>("Is_Locked");

                    b.Property<byte>("Is_Temp_Password");

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
#pragma warning restore 612, 618
        }
    }
}
