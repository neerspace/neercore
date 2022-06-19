﻿// <auto-generated />


#nullable disable

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SeniorTemplate.Data.Context;

namespace MiddleTemplate.Data.Migrations
{
    [DbContext(typeof(SqliteDbContext))]
    partial class SqliteDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.6");

            modelBuilder.Entity("MiddleTemplate.Data.Entities.Tea", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("Updated")
                        .ValueGeneratedOnUpdate()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Teas", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("b377fbb8-bae6-4b24-8d8a-3f707dc889ba"),
                            Created = new DateTime(2022, 6, 17, 23, 10, 17, 11, DateTimeKind.Utc).AddTicks(7905),
                            Name = "Earl Gray",
                            Price = 20m
                        },
                        new
                        {
                            Id = new Guid("e04e349f-448d-40d8-aa86-cba7e68984af"),
                            Created = new DateTime(2022, 6, 17, 23, 10, 17, 11, DateTimeKind.Utc).AddTicks(8577),
                            Name = "Rose Tea",
                            Price = 20m
                        },
                        new
                        {
                            Id = new Guid("b3d29f03-9eb8-4277-9d2a-f81432325273"),
                            Created = new DateTime(2022, 6, 17, 23, 10, 17, 11, DateTimeKind.Utc).AddTicks(8581),
                            Name = "English Breakfast Tea",
                            Price = 18m
                        },
                        new
                        {
                            Id = new Guid("66ee4c28-b49e-40c3-8cf2-8ac08e2e7a16"),
                            Created = new DateTime(2022, 6, 17, 23, 10, 17, 11, DateTimeKind.Utc).AddTicks(8582),
                            Name = "Big Sur Tea",
                            Price = 25m
                        },
                        new
                        {
                            Id = new Guid("6a84d852-42b2-4569-8d07-3aca01683a59"),
                            Created = new DateTime(2022, 6, 17, 23, 10, 17, 11, DateTimeKind.Utc).AddTicks(8595),
                            Name = "Big Sur Tea",
                            Price = 25m
                        },
                        new
                        {
                            Id = new Guid("5eec791a-b0b3-4c30-8c3e-b9629f690a12"),
                            Created = new DateTime(2022, 6, 17, 23, 10, 17, 11, DateTimeKind.Utc).AddTicks(8596),
                            Name = "Jasmine Pearls",
                            Price = 41m
                        },
                        new
                        {
                            Id = new Guid("5fac5dc1-3178-4d72-b32a-3c193bb01a8c"),
                            Created = new DateTime(2022, 6, 17, 23, 10, 17, 11, DateTimeKind.Utc).AddTicks(8598),
                            Name = "Dragonwell",
                            Price = 30m
                        },
                        new
                        {
                            Id = new Guid("7dab1670-640c-40a3-84d6-568cb487d3b4"),
                            Created = new DateTime(2022, 6, 17, 23, 10, 17, 11, DateTimeKind.Utc).AddTicks(8599),
                            Name = "White Peach Tea",
                            Price = 29m
                        },
                        new
                        {
                            Id = new Guid("868fddce-fcc3-4c14-a2dc-faa3bc719dfd"),
                            Created = new DateTime(2022, 6, 17, 23, 10, 17, 11, DateTimeKind.Utc).AddTicks(8600),
                            Name = "Vanilla Berry Tea",
                            Price = 21m
                        },
                        new
                        {
                            Id = new Guid("40b76c62-2569-4fa1-80f9-18cdbb1125a9"),
                            Created = new DateTime(2022, 6, 17, 23, 10, 17, 11, DateTimeKind.Utc).AddTicks(8602),
                            Name = "Chaga Chai Mushroom Tea",
                            Price = 20m
                        },
                        new
                        {
                            Id = new Guid("b48e8bec-fbcd-4775-8add-d11c99e5c6ef"),
                            Created = new DateTime(2022, 6, 17, 23, 10, 17, 11, DateTimeKind.Utc).AddTicks(8603),
                            Name = "Naked Pu-erh Tea",
                            Price = 27m
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
