using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using pare.Sample.Domain;
using pare.Sample.Entities;

namespace pare.Sample.Migrations
{
    [DbContext(typeof(SampleContext))]
    [Migration("20170509191042_v1.0.0")]
    partial class v100
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("pare.Sample.Entities.Country", b =>
                {
                    b.Property<short>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Alpha2Code");

                    b.Property<string>("Description");

                    b.Property<DateTime>("Modified");

                    b.HasKey("Id");

                    b.ToTable("Country");
                });

            modelBuilder.Entity("pare.Sample.Entities.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<short>("CountryId");

                    b.Property<string>("Firstname")
                        .HasMaxLength(100);

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Surname")
                        .HasMaxLength(100);

                    b.Property<int>("Title");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("pare.Sample.Entities.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CustomerId");

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Number")
                        .HasMaxLength(8);

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("pare.Sample.Entities.OrderItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Modified");

                    b.Property<int>("OrderId");

                    b.Property<int>("ProductId");

                    b.Property<int>("Qty");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderItem");
                });

            modelBuilder.Entity("pare.Sample.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Cost");

                    b.Property<string>("Description")
                        .HasMaxLength(100);

                    b.Property<DateTime>("Modified");

                    b.HasKey("Id");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("pare.Sample.Entities.Title", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Description");

                    b.Property<string>("Display");

                    b.HasKey("Id");

                    b.ToTable("Title");
                });

            modelBuilder.Entity("pare.Sample.Entities.Customer", b =>
                {
                    b.HasOne("pare.Sample.Entities.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("pare.Sample.Entities.Order", b =>
                {
                    b.HasOne("pare.Sample.Entities.Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("pare.Sample.Entities.OrderItem", b =>
                {
                    b.HasOne("pare.Sample.Entities.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("pare.Sample.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
