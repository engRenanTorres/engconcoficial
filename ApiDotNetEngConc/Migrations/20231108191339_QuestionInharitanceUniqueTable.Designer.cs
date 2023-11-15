﻿// <auto-generated />
using System;
using DotnetAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DotnetAPI.Migrations
{
    [DbContext(typeof(DataContextEF))]
    [Migration("20231108191339_QuestionInharitanceUniqueTable")]
    partial class QuestionInharitanceUniqueTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DotnetAPI.Models.Choice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("BaseQuestionId")
                        .HasColumnType("integer");

                    b.Property<char>("Letter")
                        .HasColumnType("character(1)");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("BaseQuestionId");

                    b.ToTable("Choices");
                });

            modelBuilder.Entity("DotnetAPI.Models.Inharitance.BaseQuestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<char>("Answer")
                        .HasColumnType("character(1)");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("Created_at");

                    b.Property<int>("CreatedById")
                        .HasColumnType("integer");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("LastUpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("Last_updated_at");

                    b.Property<string>("Tip")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.ToTable("Questions");

                    b.HasDiscriminator<string>("Discriminator").HasValue("BaseQuestion");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("DotnetAPI.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("Created_at");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<byte[]>("Password")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<string>("SocialMedia")
                        .HasColumnType("text");

                    b.Property<string>("Website")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DotnetAPI.Models.BooleanQuestion", b =>
                {
                    b.HasBaseType("DotnetAPI.Models.Inharitance.BaseQuestion");

                    b.HasDiscriminator().HasValue("BooleanQuestion");
                });

            modelBuilder.Entity("DotnetAPI.Models.MultipleChoicesQuestion", b =>
                {
                    b.HasBaseType("DotnetAPI.Models.Inharitance.BaseQuestion");

                    b.HasDiscriminator().HasValue("MultipleChoicesQuestion");
                });

            modelBuilder.Entity("DotnetAPI.Models.Choice", b =>
                {
                    b.HasOne("DotnetAPI.Models.Inharitance.BaseQuestion", null)
                        .WithMany("Choices")
                        .HasForeignKey("BaseQuestionId");
                });

            modelBuilder.Entity("DotnetAPI.Models.Inharitance.BaseQuestion", b =>
                {
                    b.HasOne("DotnetAPI.Models.User", "CreatedBy")
                        .WithMany("Questions")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("DotnetAPI.Models.Inharitance.BaseQuestion", b =>
                {
                    b.Navigation("Choices");
                });

            modelBuilder.Entity("DotnetAPI.Models.User", b =>
                {
                    b.Navigation("Questions");
                });
#pragma warning restore 612, 618
        }
    }
}