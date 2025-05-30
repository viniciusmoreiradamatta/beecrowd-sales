﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SalesInfrastructure.Data;

#nullable disable

namespace SalesInfrastructure.Data.Migrations.Sales
{
    [DbContext(typeof(SalesDbContext))]
    [Migration("20250428114346_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SalesDomain.Entities.Product.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("character varying(80)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("character varying(80)");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Products", (string)null);
                });

            modelBuilder.Entity("SalesDomain.Entities.Sale.Sale", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("BranchId")
                        .HasColumnType("uuid");

                    b.Property<bool>("Cancelled")
                        .HasColumnType("boolean");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("SaleDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("SaleNumber")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("SaleNumber"));

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Sales", (string)null);
                });

            modelBuilder.Entity("SalesDomain.Entities.Sale.SaleItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsCancelled")
                        .HasColumnType("boolean");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<Guid>("SaleId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Total")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("ValueMonetaryTaxApplied")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("SaleId");

                    b.ToTable("SaleItems", (string)null);
                });

            modelBuilder.Entity("SalesDomain.Entities.Sale.SaleItem", b =>
                {
                    b.HasOne("SalesDomain.Entities.Sale.Sale", null)
                        .WithMany("Items")
                        .HasForeignKey("SaleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SalesDomain.Entities.Sale.Sale", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
