﻿// <auto-generated />
using System;
using Ecommerce.Core.src.ValueObject;
using Ecommerce.WebAPI.src.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ecommerce.WebAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "order_status", new[] { "shipped", "pending", "awaiting_payment", "processing", "shipping", "completed", "refunded", "cancelled" });
            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "user_role", new[] { "customer", "admin" });
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Ecommerce.Core.src.Entity.Address", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar")
                        .HasColumnName("city");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("country");

                    b.Property<DateOnly?>("CreatedDate")
                        .HasColumnType("date")
                        .HasColumnName("created_date");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar")
                        .HasColumnName("phone_number");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar")
                        .HasColumnName("street");

                    b.Property<DateOnly?>("UpdatedDate")
                        .HasColumnType("date")
                        .HasColumnName("updated_date");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasColumnType("varchar")
                        .HasColumnName("zip_code");

                    b.HasKey("Id")
                        .HasName("pk_addresses");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_addresses_user_id");

                    b.ToTable("addresses", (string)null);
                });

            modelBuilder.Entity("Ecommerce.Core.src.Entity.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateOnly?>("CreatedDate")
                        .HasColumnType("date")
                        .HasColumnName("created_date");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("varchar")
                        .HasColumnName("image");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar")
                        .HasColumnName("name");

                    b.Property<DateOnly?>("UpdatedDate")
                        .HasColumnType("date")
                        .HasColumnName("updated_date");

                    b.HasKey("Id")
                        .HasName("pk_categories");

                    b.ToTable("categories", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("a050c449-99ad-4320-8954-94a9cc4ad02b"),
                            CreatedDate = new DateOnly(2024, 5, 21),
                            Image = "https://picsum.photos/200/?random=1",
                            Name = "Electronic",
                            UpdatedDate = new DateOnly(2024, 5, 21)
                        },
                        new
                        {
                            Id = new Guid("4603943d-587b-4d54-b150-d3d6eff1d48c"),
                            CreatedDate = new DateOnly(2024, 5, 21),
                            Image = "https://picsum.photos/200/?random=3",
                            Name = "Clothing",
                            UpdatedDate = new DateOnly(2024, 5, 21)
                        },
                        new
                        {
                            Id = new Guid("cba27af8-9f5b-468a-8cd3-3fd44acdba15"),
                            CreatedDate = new DateOnly(2024, 5, 21),
                            Image = "https://picsum.photos/200/?random=2",
                            Name = "Furniture",
                            UpdatedDate = new DateOnly(2024, 5, 21)
                        },
                        new
                        {
                            Id = new Guid("4c246b6a-f1e2-42ab-8c65-3ae00632f5eb"),
                            CreatedDate = new DateOnly(2024, 5, 21),
                            Image = "https://picsum.photos/200/?random=7",
                            Name = "Books",
                            UpdatedDate = new DateOnly(2024, 5, 21)
                        },
                        new
                        {
                            Id = new Guid("1d83f025-5a7e-484a-893d-113557ce8fc5"),
                            CreatedDate = new DateOnly(2024, 5, 21),
                            Image = "https://picsum.photos/200/?random=6",
                            Name = "Toys",
                            UpdatedDate = new DateOnly(2024, 5, 21)
                        },
                        new
                        {
                            Id = new Guid("ab86db7a-9561-4816-935f-43c0a3a74b05"),
                            CreatedDate = new DateOnly(2024, 5, 21),
                            Image = "https://picsum.photos/200/?random=1",
                            Name = "Sports",
                            UpdatedDate = new DateOnly(2024, 5, 21)
                        });
                });

            modelBuilder.Entity("Ecommerce.Core.src.Entity.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("varchar")
                        .HasColumnName("address");

                    b.Property<DateOnly?>("CreatedDate")
                        .HasColumnType("date")
                        .HasColumnName("created_date");

                    b.Property<int>("Total")
                        .HasColumnType("integer")
                        .HasColumnName("total");

                    b.Property<DateOnly?>("UpdatedDate")
                        .HasColumnType("date")
                        .HasColumnName("updated_date");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_orders");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_orders_user_id");

                    b.ToTable("orders", (string)null);
                });

            modelBuilder.Entity("Ecommerce.Core.src.Entity.OrderProduct", b =>
                {
                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid")
                        .HasColumnName("order_id");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid")
                        .HasColumnName("product_id");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer")
                        .HasColumnName("quantity");

                    b.HasKey("OrderId", "ProductId")
                        .HasName("pk_order_products");

                    b.HasIndex("ProductId")
                        .HasDatabaseName("ix_order_products_product_id");

                    b.ToTable("order_products", (string)null);
                });

            modelBuilder.Entity("Ecommerce.Core.src.Entity.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid")
                        .HasColumnName("category_id");

                    b.Property<DateOnly?>("CreatedDate")
                        .HasColumnType("date")
                        .HasColumnName("created_date");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("varchar")
                        .HasColumnName("description");

                    b.Property<int>("Inventory")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0)
                        .HasColumnName("inventory");

                    b.Property<int>("Price")
                        .HasColumnType("integer")
                        .HasColumnName("price");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar")
                        .HasColumnName("title");

                    b.Property<DateOnly?>("UpdatedDate")
                        .HasColumnType("date")
                        .HasColumnName("updated_date");

                    b.HasKey("Id")
                        .HasName("pk_products");

                    b.HasIndex("CategoryId")
                        .HasDatabaseName("ix_products_category_id");

                    b.HasIndex("Price")
                        .HasDatabaseName("ix_products_price");

                    b.HasIndex("Title")
                        .IsUnique()
                        .HasDatabaseName("title");

                    b.ToTable("products", null, t =>
                        {
                            t.HasCheckConstraint("product_inventory_check", "inventory >= 0");

                            t.HasCheckConstraint("product_price_check", "price > 0");
                        });

                    b.HasData(
                        new
                        {
                            Id = new Guid("fd10059d-b0e6-486d-88b1-f9acbb35b9e4"),
                            CategoryId = new Guid("a050c449-99ad-4320-8954-94a9cc4ad02b"),
                            CreatedDate = new DateOnly(2024, 5, 21),
                            Description = "Description for Electronic Product 1",
                            Inventory = 100,
                            Price = 200,
                            Title = "Electronic Product 1",
                            UpdatedDate = new DateOnly(2024, 5, 21)
                        },
                        new
                        {
                            Id = new Guid("b723cca9-9f42-47ec-9f2e-bf1becbeac06"),
                            CategoryId = new Guid("4603943d-587b-4d54-b150-d3d6eff1d48c"),
                            CreatedDate = new DateOnly(2024, 5, 21),
                            Description = "Description for Clothing Product 1",
                            Inventory = 100,
                            Price = 900,
                            Title = "Clothing Product 1",
                            UpdatedDate = new DateOnly(2024, 5, 21)
                        },
                        new
                        {
                            Id = new Guid("d7d05dcc-686a-4f43-b436-ea460ebe7bc6"),
                            CategoryId = new Guid("cba27af8-9f5b-468a-8cd3-3fd44acdba15"),
                            CreatedDate = new DateOnly(2024, 5, 21),
                            Description = "Description for Furniture Product 1",
                            Inventory = 100,
                            Price = 700,
                            Title = "Furniture Product 1",
                            UpdatedDate = new DateOnly(2024, 5, 21)
                        },
                        new
                        {
                            Id = new Guid("f3b36b51-338b-4372-9bb4-8eb97d02e95a"),
                            CategoryId = new Guid("4c246b6a-f1e2-42ab-8c65-3ae00632f5eb"),
                            CreatedDate = new DateOnly(2024, 5, 21),
                            Description = "Description for Books Product 1",
                            Inventory = 100,
                            Price = 700,
                            Title = "Books Product 1",
                            UpdatedDate = new DateOnly(2024, 5, 21)
                        });
                });

            modelBuilder.Entity("Ecommerce.Core.src.Entity.ProductImage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateOnly?>("CreatedDate")
                        .HasColumnType("date")
                        .HasColumnName("created_date");

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("data");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid")
                        .HasColumnName("product_id");

                    b.Property<DateOnly?>("UpdatedDate")
                        .HasColumnType("date")
                        .HasColumnName("updated_date");

                    b.HasKey("Id")
                        .HasName("pk_images");

                    b.HasIndex("ProductId")
                        .HasDatabaseName("ix_images_product_id");

                    b.ToTable("images", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("16629339-0bed-4003-b288-f445933434aa"),
                            CreatedDate = new DateOnly(2024, 5, 21),
                            Data = new byte[] { 13, 115, 45, 45, 177, 16, 14, 19, 173, 48, 249, 106, 76, 75, 89, 38, 43, 155, 109, 221, 91, 135, 190, 202, 191, 194, 172, 227, 112, 194, 62, 66, 126, 41, 132, 113, 212, 162, 209, 61, 116, 20, 35, 189, 86, 19, 107, 3, 51, 137, 24, 216, 178, 89, 121, 82, 12, 207, 86, 161, 96, 212, 35, 223, 91, 105, 244, 95, 73, 134, 59, 38, 32, 27, 217, 72, 100, 212, 233, 179, 127, 121, 233, 18, 16, 202, 105, 7, 226, 67, 239, 183, 164, 183, 254, 89, 150, 114, 36, 192 },
                            ProductId = new Guid("fd10059d-b0e6-486d-88b1-f9acbb35b9e4"),
                            UpdatedDate = new DateOnly(2024, 5, 21)
                        },
                        new
                        {
                            Id = new Guid("c8051dab-6c9c-45bb-b7ee-88ef56b8b441"),
                            CreatedDate = new DateOnly(2024, 5, 21),
                            Data = new byte[] { 131, 165, 250, 51, 108, 102, 171, 155, 20, 42, 165, 170, 76, 189, 38, 103, 224, 88, 27, 218, 58, 74, 119, 184, 25, 93, 26, 40, 158, 45, 18, 107, 205, 172, 57, 222, 216, 101, 239, 211, 220, 122, 140, 124, 56, 103, 5, 241, 54, 166, 38, 191, 162, 103, 30, 174, 30, 12, 55, 19, 176, 134, 176, 147, 1, 31, 156, 159, 199, 237, 22, 62, 65, 176, 223, 120, 235, 53, 255, 94, 96, 20, 54, 237, 55, 129, 114, 29, 73, 154, 9, 137, 102, 38, 247, 14, 137, 136, 51, 125 },
                            ProductId = new Guid("fd10059d-b0e6-486d-88b1-f9acbb35b9e4"),
                            UpdatedDate = new DateOnly(2024, 5, 21)
                        },
                        new
                        {
                            Id = new Guid("08c4f2dc-2a48-4fd0-9aae-5d627c464d2a"),
                            CreatedDate = new DateOnly(2024, 5, 21),
                            Data = new byte[] { 73, 109, 226, 98, 120, 77, 149, 109, 51, 240, 241, 32, 193, 186, 137, 3, 2, 169, 17, 122, 23, 116, 4, 114, 153, 16, 198, 42, 187, 185, 35, 239, 213, 174, 193, 107, 207, 186, 8, 140, 252, 110, 3, 233, 219, 39, 52, 150, 241, 120, 84, 44, 201, 223, 203, 58, 24, 251, 189, 197, 187, 156, 84, 228, 105, 66, 142, 23, 159, 118, 213, 33, 25, 241, 191, 167, 237, 60, 229, 67, 35, 235, 172, 129, 124, 64, 139, 13, 0, 87, 238, 103, 64, 174, 203, 155, 190, 108, 80, 120 },
                            ProductId = new Guid("fd10059d-b0e6-486d-88b1-f9acbb35b9e4"),
                            UpdatedDate = new DateOnly(2024, 5, 21)
                        },
                        new
                        {
                            Id = new Guid("9435c2ca-4241-4205-96db-7955e2773619"),
                            CreatedDate = new DateOnly(2024, 5, 21),
                            Data = new byte[] { 51, 199, 70, 203, 113, 51, 144, 0, 68, 94, 209, 201, 77, 224, 167, 102, 174, 112, 11, 59, 112, 245, 69, 170, 210, 198, 206, 248, 141, 135, 9, 49, 159, 146, 29, 39, 201, 142, 100, 16, 36, 103, 79, 150, 54, 76, 144, 85, 29, 58, 167, 151, 18, 147, 243, 220, 20, 223, 61, 130, 211, 124, 29, 195, 219, 173, 213, 188, 16, 193, 182, 141, 162, 8, 13, 194, 168, 33, 200, 26, 218, 173, 157, 229, 251, 132, 214, 72, 18, 139, 171, 76, 182, 30, 35, 27, 82, 248, 7, 18 },
                            ProductId = new Guid("b723cca9-9f42-47ec-9f2e-bf1becbeac06"),
                            UpdatedDate = new DateOnly(2024, 5, 21)
                        },
                        new
                        {
                            Id = new Guid("f60fdff4-823e-4b67-bb0b-7f98f24f827f"),
                            CreatedDate = new DateOnly(2024, 5, 21),
                            Data = new byte[] { 204, 135, 131, 97, 12, 26, 84, 65, 213, 218, 2, 130, 252, 26, 135, 136, 125, 213, 116, 152, 82, 221, 83, 16, 102, 182, 215, 224, 106, 109, 59, 23, 206, 159, 241, 80, 6, 44, 143, 110, 79, 42, 55, 173, 116, 207, 66, 238, 160, 178, 92, 190, 46, 150, 172, 164, 231, 138, 235, 109, 25, 252, 167, 112, 82, 69, 52, 129, 213, 88, 235, 172, 246, 177, 32, 9, 54, 68, 184, 44, 178, 236, 209, 28, 114, 147, 31, 89, 150, 188, 32, 202, 145, 214, 70, 106, 207, 62, 135, 8 },
                            ProductId = new Guid("b723cca9-9f42-47ec-9f2e-bf1becbeac06"),
                            UpdatedDate = new DateOnly(2024, 5, 21)
                        },
                        new
                        {
                            Id = new Guid("bcdb71d2-e75f-478f-bb2c-345bc3c7bd55"),
                            CreatedDate = new DateOnly(2024, 5, 21),
                            Data = new byte[] { 98, 163, 145, 122, 189, 255, 229, 73, 27, 68, 171, 201, 163, 129, 210, 22, 181, 193, 18, 90, 187, 214, 135, 140, 128, 235, 18, 156, 174, 27, 188, 53, 220, 250, 132, 148, 62, 168, 114, 132, 45, 174, 127, 221, 221, 195, 68, 104, 253, 50, 93, 150, 24, 125, 132, 20, 86, 208, 15, 113, 76, 138, 62, 60, 160, 227, 154, 167, 31, 137, 87, 49, 23, 95, 53, 63, 195, 139, 178, 4, 87, 33, 36, 79, 119, 159, 23, 175, 201, 241, 201, 112, 43, 202, 7, 69, 157, 129, 174, 161 },
                            ProductId = new Guid("b723cca9-9f42-47ec-9f2e-bf1becbeac06"),
                            UpdatedDate = new DateOnly(2024, 5, 21)
                        },
                        new
                        {
                            Id = new Guid("6ebb277b-958a-4083-9453-9e5b6de97abd"),
                            CreatedDate = new DateOnly(2024, 5, 21),
                            Data = new byte[] { 79, 75, 127, 69, 92, 213, 201, 8, 50, 225, 78, 17, 219, 63, 50, 44, 50, 7, 238, 255, 70, 59, 84, 15, 56, 37, 99, 71, 194, 219, 133, 30, 159, 250, 184, 152, 117, 194, 230, 191, 253, 226, 119, 120, 245, 122, 146, 108, 106, 185, 19, 58, 20, 255, 85, 142, 209, 13, 198, 102, 181, 10, 41, 133, 221, 96, 140, 42, 24, 87, 173, 226, 154, 202, 203, 8, 162, 204, 10, 177, 171, 173, 138, 241, 154, 202, 131, 197, 129, 187, 29, 255, 199, 132, 113, 238, 102, 97, 133, 8 },
                            ProductId = new Guid("d7d05dcc-686a-4f43-b436-ea460ebe7bc6"),
                            UpdatedDate = new DateOnly(2024, 5, 21)
                        },
                        new
                        {
                            Id = new Guid("e723b995-6251-47f1-8ca5-210c942c3cdc"),
                            CreatedDate = new DateOnly(2024, 5, 21),
                            Data = new byte[] { 213, 164, 180, 208, 193, 140, 185, 24, 3, 78, 197, 108, 119, 40, 72, 251, 99, 19, 29, 63, 94, 218, 132, 109, 234, 66, 45, 179, 114, 99, 169, 82, 128, 215, 9, 221, 147, 249, 15, 117, 48, 124, 12, 187, 185, 130, 184, 239, 201, 182, 145, 143, 204, 22, 137, 144, 175, 81, 62, 140, 235, 37, 195, 165, 190, 235, 179, 122, 124, 76, 174, 186, 156, 211, 14, 134, 24, 2, 64, 19, 197, 139, 237, 90, 171, 178, 212, 230, 93, 221, 21, 27, 38, 47, 154, 77, 11, 245, 75, 17 },
                            ProductId = new Guid("d7d05dcc-686a-4f43-b436-ea460ebe7bc6"),
                            UpdatedDate = new DateOnly(2024, 5, 21)
                        },
                        new
                        {
                            Id = new Guid("53fbd0ba-a4ec-406d-8ada-f0954f92a94d"),
                            CreatedDate = new DateOnly(2024, 5, 21),
                            Data = new byte[] { 51, 107, 163, 105, 178, 50, 161, 50, 192, 253, 247, 129, 96, 216, 255, 188, 15, 212, 152, 215, 5, 3, 231, 250, 80, 252, 78, 32, 187, 215, 128, 19, 136, 95, 62, 88, 88, 102, 146, 42, 27, 112, 181, 206, 220, 2, 41, 99, 233, 246, 178, 109, 79, 136, 123, 202, 77, 23, 130, 156, 109, 20, 63, 42, 95, 21, 188, 203, 117, 239, 193, 139, 151, 224, 63, 84, 61, 70, 153, 5, 197, 212, 165, 255, 92, 32, 13, 196, 242, 45, 188, 228, 20, 234, 163, 217, 9, 245, 210, 252 },
                            ProductId = new Guid("d7d05dcc-686a-4f43-b436-ea460ebe7bc6"),
                            UpdatedDate = new DateOnly(2024, 5, 21)
                        },
                        new
                        {
                            Id = new Guid("c42e74e8-66bc-4258-bbe8-98d493f6ced8"),
                            CreatedDate = new DateOnly(2024, 5, 21),
                            Data = new byte[] { 224, 224, 125, 190, 230, 127, 205, 236, 14, 162, 247, 48, 55, 102, 68, 83, 171, 122, 70, 112, 93, 226, 23, 219, 124, 148, 149, 221, 214, 205, 252, 208, 247, 232, 111, 114, 235, 53, 197, 23, 229, 192, 188, 64, 143, 92, 87, 42, 249, 126, 178, 14, 189, 4, 232, 142, 75, 161, 110, 130, 125, 213, 128, 74, 17, 11, 26, 32, 87, 6, 73, 30, 32, 38, 196, 43, 189, 242, 178, 88, 210, 6, 31, 180, 141, 86, 221, 122, 137, 135, 251, 78, 114, 0, 230, 174, 152, 218, 30, 89 },
                            ProductId = new Guid("f3b36b51-338b-4372-9bb4-8eb97d02e95a"),
                            UpdatedDate = new DateOnly(2024, 5, 21)
                        },
                        new
                        {
                            Id = new Guid("5844c871-8fcb-4091-9db9-38d06988171c"),
                            CreatedDate = new DateOnly(2024, 5, 21),
                            Data = new byte[] { 245, 22, 236, 91, 202, 216, 181, 176, 68, 253, 171, 16, 223, 36, 53, 88, 224, 181, 160, 3, 97, 119, 145, 120, 174, 120, 221, 2, 96, 77, 80, 179, 244, 236, 33, 155, 237, 197, 196, 199, 127, 72, 152, 155, 20, 138, 111, 26, 239, 5, 183, 182, 10, 141, 30, 58, 50, 217, 166, 146, 236, 193, 129, 96, 80, 205, 220, 172, 114, 233, 184, 35, 180, 113, 86, 30, 48, 146, 19, 38, 57, 179, 197, 212, 154, 4, 132, 206, 52, 197, 144, 40, 51, 167, 108, 177, 183, 25, 152, 195 },
                            ProductId = new Guid("f3b36b51-338b-4372-9bb4-8eb97d02e95a"),
                            UpdatedDate = new DateOnly(2024, 5, 21)
                        },
                        new
                        {
                            Id = new Guid("ed131bb9-6967-4378-a426-51a84a9806db"),
                            CreatedDate = new DateOnly(2024, 5, 21),
                            Data = new byte[] { 59, 128, 12, 165, 53, 81, 191, 3, 218, 112, 18, 194, 176, 158, 6, 245, 0, 55, 189, 130, 255, 234, 50, 118, 72, 6, 101, 28, 159, 202, 33, 79, 218, 97, 95, 0, 88, 237, 252, 63, 3, 21, 216, 84, 103, 157, 25, 213, 134, 125, 213, 220, 26, 129, 204, 165, 190, 130, 50, 174, 135, 61, 255, 119, 1, 178, 56, 199, 54, 66, 194, 173, 118, 55, 154, 225, 119, 36, 50, 0, 236, 199, 244, 46, 25, 187, 215, 116, 232, 181, 119, 245, 84, 51, 28, 202, 46, 18, 244, 12 },
                            ProductId = new Guid("f3b36b51-338b-4372-9bb4-8eb97d02e95a"),
                            UpdatedDate = new DateOnly(2024, 5, 21)
                        });
                });

            modelBuilder.Entity("Ecommerce.Core.src.Entity.Review", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Content")
                        .HasColumnType("text")
                        .HasColumnName("content");

                    b.Property<DateOnly?>("CreatedDate")
                        .HasColumnType("date")
                        .HasColumnName("created_date");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid")
                        .HasColumnName("product_id");

                    b.Property<float>("Rating")
                        .HasColumnType("real")
                        .HasColumnName("rating");

                    b.Property<DateOnly?>("UpdatedDate")
                        .HasColumnType("date")
                        .HasColumnName("updated_date");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_reviews");

                    b.HasIndex("ProductId")
                        .HasDatabaseName("ix_reviews_product_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_reviews_user_id");

                    b.ToTable("reviews", (string)null);
                });

            modelBuilder.Entity("Ecommerce.Core.src.Entity.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Avatar")
                        .HasColumnType("varchar(255)")
                        .HasColumnName("avatar");

                    b.Property<DateOnly?>("CreatedDate")
                        .HasColumnType("date")
                        .HasColumnName("created_date");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("email");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(1)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("varchar")
                        .HasColumnName("password");

                    b.Property<UserRole>("Role")
                        .HasColumnType("user_role")
                        .HasColumnName("role");

                    b.Property<byte[]>("Salt")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("salt");

                    b.Property<DateOnly?>("UpdatedDate")
                        .HasColumnType("date")
                        .HasColumnName("updated_date");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("ix_users_email");

                    b.ToTable("users", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("563250c6-2007-405f-a8f5-4c2a97df2e18"),
                            Avatar = "https://picsum.photos/200/?random=System.Func`1[System.Int32]",
                            CreatedDate = new DateOnly(2024, 5, 21),
                            Email = "john@example.com",
                            Name = "Admin1",
                            Password = "LKWWHsaBfN6Wna9/4gibUhQ4eUxvwuZKGELOhFFk42Q=",
                            Role = UserRole.Admin,
                            Salt = new byte[] { 216, 91, 108, 251, 150, 139, 124, 91, 134, 14, 216, 130, 214, 3, 220, 200 },
                            UpdatedDate = new DateOnly(2024, 5, 21)
                        },
                        new
                        {
                            Id = new Guid("f230a2ab-3012-478a-9714-32f152c48abc"),
                            Avatar = "https://picsum.photos/200/?random=System.Func`1[System.Int32]",
                            CreatedDate = new DateOnly(2024, 5, 21),
                            Email = "binh@admin.com",
                            Name = "Binh",
                            Password = "0JR6XyOCQDV7tZMb6dc6m5uaDuoRpqFuAmUM6Z7zucQ=",
                            Role = UserRole.Admin,
                            Salt = new byte[] { 43, 254, 227, 165, 63, 251, 120, 144, 228, 183, 194, 158, 112, 223, 8, 68 },
                            UpdatedDate = new DateOnly(2024, 5, 21)
                        },
                        new
                        {
                            Id = new Guid("3bb6a94a-418d-4ef6-8592-b3935b6cd1f0"),
                            Avatar = "https://picsum.photos/200/?random=System.Func`1[System.Int32]",
                            CreatedDate = new DateOnly(2024, 5, 21),
                            Email = "adnan@admin.com",
                            Name = "Adnan",
                            Password = "0HIKEHxu04ToxS04AO1V8J4FGjfq2CapWc1oH076OgY=",
                            Role = UserRole.Admin,
                            Salt = new byte[] { 190, 124, 129, 250, 198, 56, 21, 130, 183, 151, 235, 167, 81, 36, 229, 191 },
                            UpdatedDate = new DateOnly(2024, 5, 21)
                        },
                        new
                        {
                            Id = new Guid("0b87dc2d-4fe5-4cec-b2a0-02da9a87bff3"),
                            Avatar = "https://picsum.photos/200/?random=System.Func`1[System.Int32]",
                            CreatedDate = new DateOnly(2024, 5, 21),
                            Email = "yuanke@admin.com",
                            Name = "Yuanke",
                            Password = "RBD9sZTbizciv6VoI1Iq9G1sHa56/KK9SnOg3+Pf8Vs=",
                            Role = UserRole.Admin,
                            Salt = new byte[] { 4, 248, 6, 75, 199, 103, 148, 126, 148, 93, 43, 146, 142, 158, 223, 224 },
                            UpdatedDate = new DateOnly(2024, 5, 21)
                        },
                        new
                        {
                            Id = new Guid("b2de04a0-d3f2-40af-ac19-007207030a12"),
                            Avatar = "https://picsum.photos/200/?random=System.Func`1[System.Int32]",
                            CreatedDate = new DateOnly(2024, 5, 21),
                            Email = "customer1@customer.com",
                            Name = "Customer1",
                            Password = "0S2w3Q2Evetb60WvOcrXiMPk4/iqRPytuL1ZIgGROLk=",
                            Role = UserRole.Customer,
                            Salt = new byte[] { 157, 104, 6, 46, 13, 98, 17, 38, 129, 31, 72, 191, 20, 33, 56, 169 },
                            UpdatedDate = new DateOnly(2024, 5, 21)
                        });
                });

            modelBuilder.Entity("Ecommerce.Core.src.Entity.Address", b =>
                {
                    b.HasOne("Ecommerce.Core.src.Entity.User", "User")
                        .WithMany("Addresses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_addresses_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Ecommerce.Core.src.Entity.Order", b =>
                {
                    b.HasOne("Ecommerce.Core.src.Entity.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_orders_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Ecommerce.Core.src.Entity.OrderProduct", b =>
                {
                    b.HasOne("Ecommerce.Core.src.Entity.Order", null)
                        .WithMany("OrderProducts")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_order_products_orders_order_id");

                    b.HasOne("Ecommerce.Core.src.Entity.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_order_products_products_product_id");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Ecommerce.Core.src.Entity.Product", b =>
                {
                    b.HasOne("Ecommerce.Core.src.Entity.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired()
                        .HasConstraintName("fk_products_categories_category_id");
                });

            modelBuilder.Entity("Ecommerce.Core.src.Entity.ProductImage", b =>
                {
                    b.HasOne("Ecommerce.Core.src.Entity.Product", null)
                        .WithMany("Images")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_images_products_product_id");
                });

            modelBuilder.Entity("Ecommerce.Core.src.Entity.Review", b =>
                {
                    b.HasOne("Ecommerce.Core.src.Entity.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_reviews_products_product_id");

                    b.HasOne("Ecommerce.Core.src.Entity.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_reviews_users_user_id");

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Ecommerce.Core.src.Entity.Order", b =>
                {
                    b.Navigation("OrderProducts");
                });

            modelBuilder.Entity("Ecommerce.Core.src.Entity.Product", b =>
                {
                    b.Navigation("Images");
                });

            modelBuilder.Entity("Ecommerce.Core.src.Entity.User", b =>
                {
                    b.Navigation("Addresses");
                });
#pragma warning restore 612, 618
        }
    }
}
