﻿// <auto-generated />
using System;
using TradeApi.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TradeApi.Migrations
{
    [DbContext(typeof(TradeApiDbContext))]
    [Migration("20221209123013_initial")]
    partial class initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TradeApi.Domain.Share", b =>
                {
                    b.Property<Guid>("ShareId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)");

                    b.HasKey("ShareId");

                    b.ToTable("Shares");
                });

            modelBuilder.Entity("TradeApi.Domain.Trade", b =>
                {
                    b.Property<Guid>("TradeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<Guid>("ShareId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("TradeDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("TraderId")
                        .HasColumnType("uuid");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("TradeId");

                    b.HasIndex("ShareId");

                    b.HasIndex("TraderId");

                    b.ToTable("Trades");
                });

            modelBuilder.Entity("TradeApi.Domain.Trader", b =>
                {
                    b.Property<Guid>("TraderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("character varying(11)");

                    b.HasKey("TraderId");

                    b.ToTable("Traders");
                });

            modelBuilder.Entity("TradeApi.Domain.TraderPortfolio", b =>
                {
                    b.Property<Guid>("TraderPortfolioId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<Guid>("ShareId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TraderId")
                        .HasColumnType("uuid");

                    b.HasKey("TraderPortfolioId");

                    b.HasIndex("ShareId");

                    b.HasIndex("TraderId");

                    b.ToTable("TraderPortfolioes");
                });

            modelBuilder.Entity("TradeApi.Domain.Trade", b =>
                {
                    b.HasOne("TradeApi.Domain.Share", "Share")
                        .WithMany("Trades")
                        .HasForeignKey("ShareId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TradeApi.Domain.Trader", "Trader")
                        .WithMany("Trades")
                        .HasForeignKey("TraderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Share");

                    b.Navigation("Trader");
                });

            modelBuilder.Entity("TradeApi.Domain.TraderPortfolio", b =>
                {
                    b.HasOne("TradeApi.Domain.Share", "Share")
                        .WithMany("TraderPortfolios")
                        .HasForeignKey("ShareId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TradeApi.Domain.Trader", "Trader")
                        .WithMany("TraderPortfolios")
                        .HasForeignKey("TraderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Share");

                    b.Navigation("Trader");
                });

            modelBuilder.Entity("TradeApi.Domain.Share", b =>
                {
                    b.Navigation("TraderPortfolios");

                    b.Navigation("Trades");
                });

            modelBuilder.Entity("TradeApi.Domain.Trader", b =>
                {
                    b.Navigation("TraderPortfolios");

                    b.Navigation("Trades");
                });
#pragma warning restore 612, 618
        }
    }
}
