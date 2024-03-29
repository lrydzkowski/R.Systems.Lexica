﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using R.Systems.Lexica.Infrastructure.Db;

#nullable disable

namespace R.Systems.Lexica.Infrastructure.Db.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230604071053_InitDb")]
    partial class InitDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("R.Systems.Lexica.Infrastructure.Db.SqlServer.Common.Entities.RecordingEntity", b =>
                {
                    b.Property<long>("RecordingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("recording_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("RecordingId"));

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("file_name");

                    b.Property<string>("Word")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("word");

                    b.Property<int>("WordTypeId")
                        .HasColumnType("integer")
                        .HasColumnName("word_type_id");

                    b.HasKey("RecordingId");

                    b.HasIndex("WordTypeId");

                    b.ToTable("recording", (string)null);
                });

            modelBuilder.Entity("R.Systems.Lexica.Infrastructure.Db.SqlServer.Common.Entities.SetEntity", b =>
                {
                    b.Property<long>("SetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("set_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("SetId"));

                    b.Property<DateTimeOffset>("CreatedAtUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at_utc");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("name");

                    b.HasKey("SetId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("set", (string)null);
                });

            modelBuilder.Entity("R.Systems.Lexica.Infrastructure.Db.SqlServer.Common.Entities.TranslationEntity", b =>
                {
                    b.Property<long>("TranslationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("translation_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("TranslationId"));

                    b.Property<int>("Order")
                        .HasColumnType("integer")
                        .HasColumnName("order");

                    b.Property<string>("Translation")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("translation");

                    b.Property<long>("WordId")
                        .HasColumnType("bigint")
                        .HasColumnName("word_id");

                    b.HasKey("TranslationId");

                    b.HasIndex("WordId");

                    b.ToTable("translation", (string)null);
                });

            modelBuilder.Entity("R.Systems.Lexica.Infrastructure.Db.SqlServer.Common.Entities.WordEntity", b =>
                {
                    b.Property<long>("WordId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("word_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("WordId"));

                    b.Property<int>("Order")
                        .HasColumnType("integer")
                        .HasColumnName("order");

                    b.Property<long>("SetId")
                        .HasColumnType("bigint")
                        .HasColumnName("set_id");

                    b.Property<string>("Word")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("word");

                    b.Property<int>("WordTypeId")
                        .HasColumnType("integer")
                        .HasColumnName("word_type_id");

                    b.HasKey("WordId");

                    b.HasIndex("SetId");

                    b.HasIndex("WordTypeId");

                    b.ToTable("word", (string)null);
                });

            modelBuilder.Entity("R.Systems.Lexica.Infrastructure.Db.SqlServer.Common.Entities.WordTypeEntity", b =>
                {
                    b.Property<int>("WordTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("word_type_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("WordTypeId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("name");

                    b.HasKey("WordTypeId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("word_type", (string)null);

                    b.HasData(
                        new
                        {
                            WordTypeId = 1,
                            Name = "None"
                        },
                        new
                        {
                            WordTypeId = 2,
                            Name = "Noun"
                        },
                        new
                        {
                            WordTypeId = 3,
                            Name = "Verb"
                        },
                        new
                        {
                            WordTypeId = 4,
                            Name = "Adjective"
                        },
                        new
                        {
                            WordTypeId = 5,
                            Name = "Adverb"
                        });
                });

            modelBuilder.Entity("R.Systems.Lexica.Infrastructure.Db.SqlServer.Common.Entities.RecordingEntity", b =>
                {
                    b.HasOne("R.Systems.Lexica.Infrastructure.Db.SqlServer.Common.Entities.WordTypeEntity", "WordType")
                        .WithMany("Recordings")
                        .HasForeignKey("WordTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WordType");
                });

            modelBuilder.Entity("R.Systems.Lexica.Infrastructure.Db.SqlServer.Common.Entities.TranslationEntity", b =>
                {
                    b.HasOne("R.Systems.Lexica.Infrastructure.Db.SqlServer.Common.Entities.WordEntity", "Word")
                        .WithMany("Translations")
                        .HasForeignKey("WordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Word");
                });

            modelBuilder.Entity("R.Systems.Lexica.Infrastructure.Db.SqlServer.Common.Entities.WordEntity", b =>
                {
                    b.HasOne("R.Systems.Lexica.Infrastructure.Db.SqlServer.Common.Entities.SetEntity", "Set")
                        .WithMany("Words")
                        .HasForeignKey("SetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("R.Systems.Lexica.Infrastructure.Db.SqlServer.Common.Entities.WordTypeEntity", "WordType")
                        .WithMany("Words")
                        .HasForeignKey("WordTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Set");

                    b.Navigation("WordType");
                });

            modelBuilder.Entity("R.Systems.Lexica.Infrastructure.Db.SqlServer.Common.Entities.SetEntity", b =>
                {
                    b.Navigation("Words");
                });

            modelBuilder.Entity("R.Systems.Lexica.Infrastructure.Db.SqlServer.Common.Entities.WordEntity", b =>
                {
                    b.Navigation("Translations");
                });

            modelBuilder.Entity("R.Systems.Lexica.Infrastructure.Db.SqlServer.Common.Entities.WordTypeEntity", b =>
                {
                    b.Navigation("Recordings");

                    b.Navigation("Words");
                });
#pragma warning restore 612, 618
        }
    }
}
