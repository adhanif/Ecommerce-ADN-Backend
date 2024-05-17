﻿using System;
using Ecommerce.Core.src.ValueObject;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ecommerce.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class CreateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:order_status", "shipped,pending,awaiting_payment,processing,shipping,completed,refunded,cancelled")
                .Annotation("Npgsql:Enum:user_role", "admin,customer");

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "varchar", nullable: false),
                    image = table.Column<string>(type: "varchar", nullable: false),
                    created_date = table.Column<DateOnly>(type: "date", nullable: true),
                    updated_date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "varchar(20)", maxLength: 1, nullable: false),
                    email = table.Column<string>(type: "varchar(50)", nullable: false),
                    password = table.Column<string>(type: "varchar", nullable: false),
                    salt = table.Column<byte[]>(type: "bytea", nullable: false),
                    avatar = table.Column<string>(type: "varchar(255)", nullable: true),
                    role = table.Column<UserRole>(type: "user_role", nullable: false),
                    created_date = table.Column<DateOnly>(type: "date", nullable: true),
                    updated_date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "varchar", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "varchar", nullable: false),
                    price = table.Column<int>(type: "integer", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    inventory = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    created_date = table.Column<DateOnly>(type: "date", nullable: true),
                    updated_date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_products", x => x.id);
                    table.CheckConstraint("product_inventory_check", "inventory >= 0");
                    table.CheckConstraint("product_price_check", "price > 0");
                    table.ForeignKey(
                        name: "fk_products_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "addresses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    street = table.Column<string>(type: "varchar", maxLength: 255, nullable: false),
                    city = table.Column<string>(type: "varchar", maxLength: 255, nullable: false),
                    country = table.Column<string>(type: "text", nullable: false),
                    zip_code = table.Column<string>(type: "varchar", nullable: false),
                    phone_number = table.Column<string>(type: "varchar", maxLength: 255, nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_date = table.Column<DateOnly>(type: "date", nullable: true),
                    updated_date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_addresses", x => x.id);
                    table.ForeignKey(
                        name: "fk_addresses_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    address = table.Column<string>(type: "varchar", nullable: false),
                    created_date = table.Column<DateOnly>(type: "date", nullable: true),
                    updated_date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_orders", x => x.id);
                    table.ForeignKey(
                        name: "fk_orders_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "images",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    data = table.Column<byte[]>(type: "bytea", nullable: false),
                    created_date = table.Column<DateOnly>(type: "date", nullable: true),
                    updated_date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_images", x => x.id);
                    table.ForeignKey(
                        name: "fk_images_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reviews",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    rating = table.Column<float>(type: "real", nullable: false),
                    content = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_date = table.Column<DateOnly>(type: "date", nullable: true),
                    updated_date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reviews", x => x.id);
                    table.ForeignKey(
                        name: "fk_reviews_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_reviews_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_products",
                columns: table => new
                {
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_products", x => new { x.order_id, x.product_id });
                    table.ForeignKey(
                        name: "fk_order_products_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_order_products_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "id", "created_date", "image", "name", "updated_date" },
                values: new object[,]
                {
                    { new Guid("0ab1f241-3e56-4190-951f-feb661d931a0"), new DateOnly(2024, 5, 17), "https://picsum.photos/200/?random=5", "Sports", new DateOnly(2024, 5, 17) },
                    { new Guid("6e1efd91-2a1a-4e25-b7a5-dd321ef34994"), new DateOnly(2024, 5, 17), "https://picsum.photos/200/?random=2", "Toys", new DateOnly(2024, 5, 17) },
                    { new Guid("9c74cc19-f991-4d06-863f-0bed68f49d9c"), new DateOnly(2024, 5, 17), "https://picsum.photos/200/?random=2", "Clothing", new DateOnly(2024, 5, 17) },
                    { new Guid("a1a97bba-d45e-4345-9dc2-0756a1fd26c5"), new DateOnly(2024, 5, 17), "https://picsum.photos/200/?random=9", "Books", new DateOnly(2024, 5, 17) },
                    { new Guid("bed61b5c-1a8c-4ba7-bf34-39f11ec14654"), new DateOnly(2024, 5, 17), "https://picsum.photos/200/?random=6", "Electronic", new DateOnly(2024, 5, 17) },
                    { new Guid("fe2e95db-e391-4d21-a0b6-353ee42aff21"), new DateOnly(2024, 5, 17), "https://picsum.photos/200/?random=5", "Furniture", new DateOnly(2024, 5, 17) }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "avatar", "created_date", "email", "name", "password", "role", "salt", "updated_date" },
                values: new object[,]
                {
                    { new Guid("0302e59d-afc4-468d-bc36-1d4877661e8a"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", new DateOnly(2024, 5, 17), "adnan@admin.com", "Adnan", "QLnJl6wdLk7rinx9QMh8HRTi9tYv2QkeoSlUzNa6Uus=", UserRole.Admin, new byte[] { 32, 145, 36, 139, 10, 174, 184, 74, 74, 193, 99, 85, 79, 54, 254, 134 }, new DateOnly(2024, 5, 17) },
                    { new Guid("2e682b25-a2b4-4f2a-9f4a-27650e90a53d"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", new DateOnly(2024, 5, 17), "john@example.com", "Admin1", "SMvjsCy+A4xUWejEQJkFCqKRRTuK5uQKK1CqgUrlIQI=", UserRole.Admin, new byte[] { 212, 11, 32, 207, 232, 96, 93, 180, 171, 58, 250, 238, 93, 30, 64, 8 }, new DateOnly(2024, 5, 17) },
                    { new Guid("582c084b-8dd6-4ff1-af1d-47e2c1d1e9cb"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", new DateOnly(2024, 5, 17), "binh@admin.com", "Binh", "lgdRm93kITzLwgfSZkhXYkRl/k/FSTT7aIfmp4cbe3I=", UserRole.Admin, new byte[] { 53, 178, 133, 20, 16, 164, 134, 107, 199, 219, 66, 206, 186, 38, 166, 226 }, new DateOnly(2024, 5, 17) },
                    { new Guid("7f230f66-fa15-46aa-a2d6-d4fd87bda7d1"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", new DateOnly(2024, 5, 17), "customer1@customer.com", "Customer1", "g+dE8ASIbncn1dkFc3GBFTgozBkB9QUZn9yygJ1IiDU=", UserRole.Customer, new byte[] { 213, 209, 88, 59, 174, 196, 35, 143, 91, 179, 131, 156, 241, 233, 6, 200 }, new DateOnly(2024, 5, 17) },
                    { new Guid("a4194a96-7e25-4f59-811a-1c9bc333aa0c"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", new DateOnly(2024, 5, 17), "yuanke@admin.com", "Yuanke", "LBB0479mkzpB2CGbcLdtAqV8TNouc4ySR8yPfxNxKAo=", UserRole.Admin, new byte[] { 195, 199, 81, 113, 148, 199, 31, 133, 78, 163, 53, 0, 27, 227, 195, 214 }, new DateOnly(2024, 5, 17) }
                });

            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "id", "category_id", "created_date", "description", "inventory", "price", "title", "updated_date" },
                values: new object[,]
                {
                    { new Guid("0ee0dd0c-2298-44d9-8000-003930e15e68"), new Guid("fe2e95db-e391-4d21-a0b6-353ee42aff21"), new DateOnly(2024, 5, 17), "Description for Furniture Product 1", 100, 100, "Furniture Product 1", new DateOnly(2024, 5, 17) },
                    { new Guid("293e0ab9-d34d-4ba7-bb84-2ae8d5a7d48b"), new Guid("a1a97bba-d45e-4345-9dc2-0756a1fd26c5"), new DateOnly(2024, 5, 17), "Description for Books Product 1", 100, 1000, "Books Product 1", new DateOnly(2024, 5, 17) },
                    { new Guid("40e4b9d8-9cff-45a0-9f21-c144b4815ce5"), new Guid("bed61b5c-1a8c-4ba7-bf34-39f11ec14654"), new DateOnly(2024, 5, 17), "Description for Electronic Product 1", 100, 300, "Electronic Product 1", new DateOnly(2024, 5, 17) },
                    { new Guid("49b88301-5cd2-4ad7-ae27-bea81b20fe6e"), new Guid("a1a97bba-d45e-4345-9dc2-0756a1fd26c5"), new DateOnly(2024, 5, 17), "Description for Books Product 2", 100, 500, "Books Product 2", new DateOnly(2024, 5, 17) },
                    { new Guid("735c3d1a-b5ca-4314-8e97-9dec2ff5f164"), new Guid("fe2e95db-e391-4d21-a0b6-353ee42aff21"), new DateOnly(2024, 5, 17), "Description for Furniture Product 2", 100, 700, "Furniture Product 2", new DateOnly(2024, 5, 17) },
                    { new Guid("78dbd9e6-cb59-486a-a32c-98363eeeaf7b"), new Guid("9c74cc19-f991-4d06-863f-0bed68f49d9c"), new DateOnly(2024, 5, 17), "Description for Clothing Product 1", 100, 100, "Clothing Product 1", new DateOnly(2024, 5, 17) },
                    { new Guid("d465aa60-0677-4892-a081-d23e85bf5e90"), new Guid("9c74cc19-f991-4d06-863f-0bed68f49d9c"), new DateOnly(2024, 5, 17), "Description for Clothing Product 2", 100, 200, "Clothing Product 2", new DateOnly(2024, 5, 17) },
                    { new Guid("d888ba7c-08d8-4ff0-ac4e-d86ecd39dbd2"), new Guid("bed61b5c-1a8c-4ba7-bf34-39f11ec14654"), new DateOnly(2024, 5, 17), "Description for Electronic Product 2", 100, 400, "Electronic Product 2", new DateOnly(2024, 5, 17) }
                });

            migrationBuilder.InsertData(
                table: "images",
                columns: new[] { "id", "created_date", "data", "product_id", "updated_date" },
                values: new object[,]
                {
                    { new Guid("05344908-b64b-4126-9ca5-6c9b3333adc1"), new DateOnly(2024, 5, 17), new byte[] { 48, 213, 227, 52, 121, 185, 232, 111, 182, 205, 219, 140, 79, 153, 187, 166, 135, 121, 18, 29, 123, 147, 221, 134, 25, 2, 119, 84, 106, 198, 225, 231, 18, 76, 224, 67, 104, 161, 18, 251, 220, 239, 97, 69, 11, 239, 134, 151, 171, 7, 214, 87, 110, 129, 121, 223, 245, 156, 234, 148, 95, 76, 89, 40, 209, 101, 216, 60, 141, 34, 116, 201, 137, 159, 243, 204, 188, 28, 94, 83, 26, 252, 174, 233, 70, 6, 19, 123, 194, 226, 196, 214, 149, 225, 4, 33, 227, 244, 117, 154 }, new Guid("49b88301-5cd2-4ad7-ae27-bea81b20fe6e"), new DateOnly(2024, 5, 17) },
                    { new Guid("07722378-2124-4bab-ac8e-bf789bad1e7c"), new DateOnly(2024, 5, 17), new byte[] { 5, 239, 207, 228, 37, 21, 140, 91, 28, 171, 64, 24, 33, 179, 33, 50, 49, 129, 50, 56, 0, 250, 158, 85, 120, 128, 19, 182, 56, 172, 7, 20, 85, 135, 237, 37, 130, 137, 173, 48, 33, 67, 144, 167, 94, 2, 70, 7, 171, 32, 6, 70, 8, 85, 34, 199, 116, 179, 160, 114, 212, 106, 187, 213, 112, 203, 117, 83, 202, 236, 57, 81, 127, 47, 159, 30, 107, 50, 69, 203, 2, 186, 72, 72, 2, 32, 198, 226, 251, 104, 112, 15, 51, 203, 54, 193, 8, 129, 250, 6 }, new Guid("d465aa60-0677-4892-a081-d23e85bf5e90"), new DateOnly(2024, 5, 17) },
                    { new Guid("0c2ddeb0-7746-4532-9b91-42c5162fb26f"), new DateOnly(2024, 5, 17), new byte[] { 117, 48, 225, 129, 63, 6, 127, 234, 39, 223, 164, 97, 39, 124, 94, 149, 214, 89, 68, 159, 123, 199, 110, 254, 90, 42, 185, 80, 113, 227, 53, 229, 159, 239, 45, 181, 11, 14, 122, 93, 58, 124, 109, 122, 59, 93, 174, 232, 4, 178, 235, 99, 27, 93, 89, 166, 208, 83, 141, 152, 246, 68, 120, 173, 98, 13, 71, 191, 191, 242, 235, 105, 89, 242, 238, 163, 203, 227, 120, 1, 137, 223, 84, 255, 178, 5, 242, 216, 98, 163, 42, 166, 127, 235, 147, 168, 24, 51, 224, 166 }, new Guid("0ee0dd0c-2298-44d9-8000-003930e15e68"), new DateOnly(2024, 5, 17) },
                    { new Guid("244c830d-1262-4620-9c5c-e89ba7f85f7c"), new DateOnly(2024, 5, 17), new byte[] { 103, 92, 193, 102, 177, 174, 11, 4, 48, 179, 83, 43, 98, 83, 131, 235, 151, 164, 142, 72, 119, 157, 127, 237, 106, 80, 167, 184, 209, 43, 249, 37, 238, 48, 42, 198, 115, 142, 197, 60, 158, 234, 37, 57, 248, 74, 245, 154, 120, 140, 60, 55, 63, 163, 208, 84, 254, 128, 231, 205, 94, 12, 104, 140, 235, 200, 37, 84, 1, 28, 98, 95, 182, 217, 122, 150, 133, 146, 168, 101, 195, 79, 177, 39, 244, 18, 0, 40, 80, 61, 170, 177, 218, 184, 44, 1, 155, 157, 233, 237 }, new Guid("d888ba7c-08d8-4ff0-ac4e-d86ecd39dbd2"), new DateOnly(2024, 5, 17) },
                    { new Guid("271916b2-ae84-4429-9248-c3b3b3f9822a"), new DateOnly(2024, 5, 17), new byte[] { 190, 37, 18, 15, 19, 172, 224, 238, 14, 106, 20, 8, 155, 17, 187, 215, 187, 2, 180, 177, 208, 123, 158, 224, 134, 135, 35, 46, 247, 38, 124, 152, 97, 189, 160, 68, 83, 9, 24, 211, 202, 74, 129, 60, 7, 81, 244, 41, 165, 71, 51, 180, 51, 46, 1, 222, 125, 6, 222, 136, 143, 81, 243, 227, 199, 16, 239, 182, 99, 57, 224, 49, 188, 70, 213, 51, 45, 128, 136, 162, 184, 209, 130, 192, 34, 18, 64, 246, 237, 167, 94, 230, 76, 201, 184, 70, 10, 215, 208, 164 }, new Guid("293e0ab9-d34d-4ba7-bb84-2ae8d5a7d48b"), new DateOnly(2024, 5, 17) },
                    { new Guid("3932ac88-7c1c-4331-a17b-ecff061ebbde"), new DateOnly(2024, 5, 17), new byte[] { 81, 16, 207, 144, 245, 169, 74, 143, 239, 52, 184, 31, 179, 65, 89, 137, 150, 197, 180, 133, 35, 218, 8, 36, 184, 243, 10, 192, 80, 33, 53, 229, 26, 253, 148, 239, 216, 211, 63, 84, 251, 90, 154, 223, 55, 169, 175, 50, 244, 29, 167, 213, 145, 240, 100, 105, 111, 122, 3, 9, 157, 172, 37, 170, 164, 126, 250, 65, 169, 116, 126, 192, 21, 179, 97, 232, 36, 162, 150, 151, 80, 241, 52, 125, 203, 250, 24, 44, 160, 219, 162, 65, 13, 37, 192, 160, 47, 179, 75, 53 }, new Guid("d888ba7c-08d8-4ff0-ac4e-d86ecd39dbd2"), new DateOnly(2024, 5, 17) },
                    { new Guid("47d15849-0e76-443f-938e-c2d88b9b2235"), new DateOnly(2024, 5, 17), new byte[] { 37, 36, 210, 85, 61, 27, 191, 71, 122, 35, 17, 79, 113, 126, 82, 65, 177, 206, 223, 158, 249, 128, 218, 195, 170, 202, 104, 55, 6, 180, 208, 183, 138, 163, 151, 1, 247, 64, 35, 60, 42, 60, 156, 211, 250, 229, 55, 27, 218, 18, 80, 114, 181, 60, 35, 78, 173, 33, 195, 9, 209, 240, 242, 76, 245, 70, 13, 208, 239, 117, 172, 221, 124, 191, 58, 227, 143, 5, 232, 28, 229, 236, 94, 7, 91, 156, 180, 178, 128, 138, 69, 20, 7, 99, 250, 56, 85, 28, 140, 41 }, new Guid("293e0ab9-d34d-4ba7-bb84-2ae8d5a7d48b"), new DateOnly(2024, 5, 17) },
                    { new Guid("57cb90f8-e40b-4d3e-8237-0643f4a4c452"), new DateOnly(2024, 5, 17), new byte[] { 1, 148, 250, 88, 120, 63, 44, 100, 155, 97, 144, 245, 193, 18, 19, 231, 153, 55, 160, 34, 77, 238, 188, 1, 163, 24, 240, 248, 250, 187, 231, 73, 78, 223, 83, 118, 184, 142, 212, 118, 162, 219, 80, 196, 204, 240, 117, 160, 226, 67, 49, 233, 32, 181, 239, 90, 143, 107, 1, 157, 196, 158, 249, 24, 64, 103, 150, 119, 129, 138, 130, 88, 21, 206, 246, 117, 120, 50, 74, 222, 181, 88, 204, 23, 237, 240, 200, 118, 59, 238, 78, 161, 178, 75, 60, 117, 115, 84, 247, 202 }, new Guid("293e0ab9-d34d-4ba7-bb84-2ae8d5a7d48b"), new DateOnly(2024, 5, 17) },
                    { new Guid("5df5cfa3-5229-4046-9c3c-8abc16852cc6"), new DateOnly(2024, 5, 17), new byte[] { 113, 0, 13, 125, 237, 166, 225, 55, 59, 79, 120, 83, 23, 40, 238, 67, 210, 36, 46, 197, 114, 203, 6, 45, 216, 206, 3, 112, 88, 154, 24, 199, 66, 5, 254, 77, 149, 252, 223, 214, 96, 186, 39, 100, 188, 122, 152, 148, 49, 130, 219, 197, 131, 208, 131, 118, 186, 141, 42, 187, 3, 147, 88, 156, 138, 22, 253, 203, 76, 224, 154, 70, 21, 250, 209, 63, 46, 255, 127, 209, 155, 121, 227, 138, 48, 146, 135, 71, 64, 89, 144, 0, 55, 139, 190, 77, 170, 207, 155, 57 }, new Guid("d465aa60-0677-4892-a081-d23e85bf5e90"), new DateOnly(2024, 5, 17) },
                    { new Guid("72f11d2c-21c8-4706-9f14-d3fdfffb57d7"), new DateOnly(2024, 5, 17), new byte[] { 190, 77, 234, 114, 19, 96, 14, 216, 170, 176, 131, 57, 82, 209, 60, 218, 74, 122, 230, 147, 176, 18, 59, 190, 157, 248, 11, 160, 150, 116, 225, 32, 232, 40, 228, 6, 213, 142, 16, 30, 252, 71, 1, 130, 251, 48, 227, 219, 102, 160, 109, 253, 173, 127, 128, 107, 48, 41, 213, 133, 204, 74, 71, 13, 208, 145, 127, 135, 223, 31, 254, 3, 241, 58, 150, 213, 50, 224, 19, 0, 7, 205, 5, 112, 4, 107, 157, 175, 38, 137, 139, 73, 209, 247, 66, 90, 84, 33, 110, 71 }, new Guid("78dbd9e6-cb59-486a-a32c-98363eeeaf7b"), new DateOnly(2024, 5, 17) },
                    { new Guid("7b8751e1-f469-405a-9c5c-a149a993bcc0"), new DateOnly(2024, 5, 17), new byte[] { 199, 7, 116, 194, 56, 160, 72, 239, 80, 58, 87, 98, 55, 153, 125, 251, 80, 175, 149, 200, 218, 14, 146, 129, 199, 163, 133, 221, 55, 159, 130, 210, 70, 57, 217, 53, 230, 172, 253, 241, 252, 190, 150, 179, 193, 133, 159, 123, 245, 58, 221, 88, 66, 195, 128, 25, 155, 54, 233, 75, 101, 59, 205, 109, 205, 119, 206, 118, 106, 172, 138, 242, 196, 159, 127, 36, 51, 172, 150, 207, 225, 233, 181, 5, 253, 208, 63, 184, 194, 168, 39, 251, 255, 169, 240, 179, 200, 48, 109, 27 }, new Guid("735c3d1a-b5ca-4314-8e97-9dec2ff5f164"), new DateOnly(2024, 5, 17) },
                    { new Guid("885fc83c-3189-4b92-91e3-0b1922c2fd68"), new DateOnly(2024, 5, 17), new byte[] { 251, 217, 75, 104, 202, 205, 89, 165, 244, 157, 174, 78, 210, 60, 42, 17, 137, 52, 27, 1, 56, 239, 237, 226, 182, 14, 104, 58, 218, 88, 191, 107, 130, 236, 221, 129, 163, 117, 209, 44, 210, 160, 164, 2, 68, 178, 24, 103, 200, 242, 150, 5, 237, 99, 166, 167, 62, 90, 81, 216, 40, 197, 68, 188, 179, 121, 223, 227, 181, 85, 116, 29, 229, 36, 158, 1, 143, 126, 250, 156, 142, 68, 10, 169, 216, 171, 33, 212, 58, 147, 40, 75, 64, 89, 191, 16, 158, 131, 139, 215 }, new Guid("49b88301-5cd2-4ad7-ae27-bea81b20fe6e"), new DateOnly(2024, 5, 17) },
                    { new Guid("8a6e5db6-f410-49c3-992b-5ce87d892c21"), new DateOnly(2024, 5, 17), new byte[] { 105, 179, 87, 105, 79, 148, 162, 145, 178, 194, 226, 59, 100, 22, 98, 226, 193, 72, 134, 102, 158, 19, 72, 64, 160, 91, 199, 104, 113, 210, 16, 119, 169, 28, 67, 157, 66, 254, 117, 214, 109, 205, 6, 220, 76, 84, 235, 77, 206, 135, 55, 224, 60, 6, 235, 106, 115, 244, 80, 135, 237, 245, 21, 11, 165, 202, 190, 101, 79, 17, 164, 250, 96, 29, 233, 179, 140, 11, 182, 125, 134, 110, 69, 14, 126, 212, 168, 221, 234, 92, 71, 102, 112, 185, 48, 62, 46, 65, 34, 209 }, new Guid("40e4b9d8-9cff-45a0-9f21-c144b4815ce5"), new DateOnly(2024, 5, 17) },
                    { new Guid("8b8a8cb0-7302-472c-8a84-a39ff68ca4c7"), new DateOnly(2024, 5, 17), new byte[] { 165, 32, 84, 205, 250, 254, 37, 65, 140, 96, 187, 158, 249, 235, 68, 44, 76, 254, 251, 15, 231, 13, 120, 122, 73, 137, 59, 215, 245, 121, 93, 223, 113, 118, 107, 5, 96, 162, 244, 127, 212, 21, 74, 246, 226, 98, 97, 11, 59, 233, 174, 26, 151, 179, 213, 148, 219, 96, 48, 242, 203, 136, 189, 27, 224, 33, 77, 220, 188, 42, 213, 137, 254, 13, 112, 124, 137, 91, 255, 240, 53, 22, 75, 81, 167, 192, 165, 26, 212, 180, 214, 180, 152, 209, 152, 134, 153, 46, 81, 255 }, new Guid("40e4b9d8-9cff-45a0-9f21-c144b4815ce5"), new DateOnly(2024, 5, 17) },
                    { new Guid("8c08aad1-1334-47dc-a49e-67b528da9f5d"), new DateOnly(2024, 5, 17), new byte[] { 190, 4, 40, 66, 67, 180, 58, 56, 132, 250, 167, 222, 113, 215, 14, 242, 87, 93, 206, 45, 248, 157, 212, 192, 5, 177, 45, 39, 180, 164, 35, 4, 198, 139, 118, 2, 14, 26, 161, 115, 177, 83, 212, 13, 120, 161, 104, 188, 166, 171, 182, 140, 0, 217, 151, 70, 44, 141, 219, 88, 76, 147, 130, 180, 194, 34, 78, 58, 184, 118, 206, 218, 30, 61, 201, 81, 196, 152, 242, 38, 124, 192, 17, 149, 24, 126, 221, 34, 212, 75, 80, 197, 29, 98, 254, 102, 109, 26, 198, 250 }, new Guid("78dbd9e6-cb59-486a-a32c-98363eeeaf7b"), new DateOnly(2024, 5, 17) },
                    { new Guid("b088f992-2ed9-491d-a2bf-f939a817981e"), new DateOnly(2024, 5, 17), new byte[] { 191, 136, 226, 130, 251, 111, 250, 219, 170, 42, 96, 193, 190, 119, 193, 238, 170, 211, 142, 159, 114, 5, 250, 148, 226, 77, 89, 220, 102, 217, 31, 248, 62, 250, 135, 74, 182, 23, 214, 231, 29, 30, 237, 16, 106, 187, 15, 197, 29, 198, 188, 96, 32, 108, 132, 246, 248, 185, 90, 151, 97, 69, 196, 87, 95, 50, 31, 113, 244, 245, 204, 179, 246, 109, 141, 31, 88, 46, 64, 239, 67, 7, 60, 83, 208, 168, 0, 120, 15, 179, 142, 220, 218, 202, 153, 65, 61, 19, 123, 98 }, new Guid("49b88301-5cd2-4ad7-ae27-bea81b20fe6e"), new DateOnly(2024, 5, 17) },
                    { new Guid("b2acb491-d449-4bdd-a8d6-8a9b8ae777a9"), new DateOnly(2024, 5, 17), new byte[] { 197, 77, 71, 25, 17, 83, 125, 49, 251, 176, 111, 146, 97, 177, 244, 7, 169, 102, 102, 129, 252, 65, 98, 161, 74, 26, 89, 127, 242, 192, 186, 176, 58, 65, 35, 96, 18, 11, 105, 27, 91, 50, 216, 232, 80, 121, 92, 177, 205, 107, 101, 227, 43, 161, 201, 10, 34, 162, 153, 44, 61, 10, 52, 91, 237, 106, 5, 30, 97, 219, 174, 211, 171, 44, 172, 223, 247, 151, 155, 113, 100, 139, 185, 57, 40, 51, 73, 145, 209, 224, 193, 112, 57, 92, 248, 185, 51, 20, 47, 222 }, new Guid("0ee0dd0c-2298-44d9-8000-003930e15e68"), new DateOnly(2024, 5, 17) },
                    { new Guid("bd6a46d8-c832-4938-afad-8b5ac3809b03"), new DateOnly(2024, 5, 17), new byte[] { 32, 212, 107, 54, 119, 0, 227, 124, 108, 227, 150, 204, 1, 187, 3, 156, 24, 48, 138, 238, 104, 169, 142, 225, 136, 131, 14, 163, 71, 225, 27, 180, 182, 160, 194, 18, 253, 178, 22, 173, 11, 184, 59, 22, 180, 158, 119, 105, 91, 232, 18, 84, 94, 129, 223, 107, 12, 245, 199, 237, 74, 39, 61, 246, 168, 88, 219, 153, 7, 91, 71, 53, 142, 234, 10, 173, 124, 3, 25, 15, 108, 171, 11, 145, 96, 86, 139, 59, 109, 30, 151, 208, 35, 4, 108, 97, 222, 46, 215, 148 }, new Guid("d888ba7c-08d8-4ff0-ac4e-d86ecd39dbd2"), new DateOnly(2024, 5, 17) },
                    { new Guid("d5cc621b-5fbe-40a3-83a8-575ad60d1223"), new DateOnly(2024, 5, 17), new byte[] { 180, 131, 34, 46, 28, 107, 179, 204, 123, 105, 24, 233, 230, 232, 227, 224, 125, 48, 9, 1, 72, 118, 202, 137, 86, 111, 211, 77, 143, 164, 141, 186, 119, 173, 188, 216, 196, 129, 8, 2, 148, 74, 45, 64, 246, 237, 192, 74, 118, 253, 190, 95, 248, 194, 0, 188, 30, 181, 194, 51, 108, 60, 18, 72, 243, 10, 86, 133, 210, 45, 185, 53, 116, 11, 117, 137, 86, 85, 247, 59, 127, 95, 88, 53, 215, 109, 81, 23, 248, 8, 15, 238, 220, 8, 9, 84, 209, 163, 87, 72 }, new Guid("40e4b9d8-9cff-45a0-9f21-c144b4815ce5"), new DateOnly(2024, 5, 17) },
                    { new Guid("d6d80525-e717-4d6e-be41-93d86bb936c8"), new DateOnly(2024, 5, 17), new byte[] { 233, 2, 159, 16, 81, 205, 171, 73, 135, 193, 158, 48, 240, 144, 25, 100, 149, 118, 139, 206, 157, 233, 212, 15, 209, 111, 225, 228, 171, 127, 207, 94, 90, 168, 47, 189, 117, 175, 18, 60, 158, 244, 204, 54, 159, 119, 158, 195, 183, 72, 168, 22, 25, 62, 41, 43, 128, 253, 251, 163, 5, 188, 53, 182, 168, 11, 190, 90, 50, 102, 16, 186, 250, 121, 52, 27, 179, 151, 139, 186, 68, 24, 75, 14, 125, 252, 48, 34, 42, 99, 135, 14, 98, 23, 78, 98, 205, 74, 61, 132 }, new Guid("0ee0dd0c-2298-44d9-8000-003930e15e68"), new DateOnly(2024, 5, 17) },
                    { new Guid("ee0b07bf-3950-445a-b21b-7228991e8cab"), new DateOnly(2024, 5, 17), new byte[] { 198, 26, 127, 187, 165, 67, 228, 75, 40, 152, 200, 228, 19, 147, 227, 107, 5, 116, 141, 70, 72, 251, 55, 241, 124, 88, 3, 135, 57, 206, 232, 112, 144, 93, 247, 16, 16, 72, 123, 211, 63, 139, 89, 251, 72, 108, 83, 18, 23, 52, 154, 167, 67, 55, 242, 168, 52, 47, 253, 225, 226, 17, 189, 60, 188, 112, 50, 145, 117, 5, 150, 185, 101, 163, 116, 44, 206, 1, 117, 48, 142, 61, 230, 149, 57, 251, 161, 143, 220, 255, 106, 122, 44, 249, 159, 187, 208, 34, 101, 243 }, new Guid("735c3d1a-b5ca-4314-8e97-9dec2ff5f164"), new DateOnly(2024, 5, 17) },
                    { new Guid("ef173429-0962-4e20-87ba-8c03ef06ff92"), new DateOnly(2024, 5, 17), new byte[] { 200, 44, 166, 220, 61, 86, 51, 66, 101, 95, 54, 128, 118, 122, 253, 203, 72, 136, 58, 99, 178, 189, 235, 154, 89, 119, 120, 140, 179, 59, 7, 168, 191, 102, 3, 32, 34, 210, 115, 254, 191, 188, 15, 54, 108, 49, 83, 134, 191, 237, 229, 222, 17, 150, 190, 18, 205, 80, 33, 180, 93, 143, 215, 152, 144, 168, 131, 245, 90, 39, 152, 59, 108, 49, 167, 63, 58, 239, 57, 209, 236, 182, 19, 64, 101, 239, 44, 50, 120, 124, 108, 53, 174, 140, 30, 5, 163, 80, 199, 99 }, new Guid("735c3d1a-b5ca-4314-8e97-9dec2ff5f164"), new DateOnly(2024, 5, 17) },
                    { new Guid("f5b40fe1-12a8-42ab-99d1-5d7489bd0ceb"), new DateOnly(2024, 5, 17), new byte[] { 37, 217, 218, 221, 16, 115, 24, 242, 67, 83, 227, 135, 154, 20, 102, 105, 164, 72, 117, 43, 201, 224, 143, 76, 69, 10, 204, 25, 62, 224, 105, 28, 218, 2, 74, 58, 231, 41, 101, 214, 85, 12, 192, 122, 95, 158, 155, 156, 44, 29, 99, 152, 247, 249, 239, 167, 5, 198, 228, 189, 70, 124, 15, 229, 202, 220, 253, 195, 159, 79, 249, 31, 133, 179, 252, 69, 33, 187, 220, 17, 165, 190, 83, 141, 61, 204, 126, 247, 199, 203, 116, 114, 180, 39, 7, 97, 81, 169, 111, 177 }, new Guid("78dbd9e6-cb59-486a-a32c-98363eeeaf7b"), new DateOnly(2024, 5, 17) },
                    { new Guid("ff42fd63-a8c3-4711-bcae-180d2d7b51c9"), new DateOnly(2024, 5, 17), new byte[] { 180, 46, 104, 159, 120, 181, 242, 75, 154, 102, 99, 43, 99, 191, 190, 114, 218, 247, 94, 190, 141, 66, 187, 206, 253, 93, 139, 126, 253, 43, 151, 193, 229, 93, 49, 34, 63, 64, 182, 144, 226, 127, 148, 58, 84, 92, 244, 62, 221, 245, 164, 141, 143, 213, 0, 255, 255, 26, 95, 81, 220, 206, 64, 73, 105, 255, 146, 56, 94, 90, 120, 241, 231, 76, 255, 13, 181, 248, 80, 198, 198, 26, 190, 255, 53, 251, 30, 117, 187, 161, 130, 186, 73, 204, 224, 11, 46, 190, 235, 124 }, new Guid("d465aa60-0677-4892-a081-d23e85bf5e90"), new DateOnly(2024, 5, 17) }
                });

            migrationBuilder.CreateIndex(
                name: "ix_addresses_user_id",
                table: "addresses",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_images_product_id",
                table: "images",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_products_product_id",
                table: "order_products",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_orders_user_id",
                table: "orders",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_products_category_id",
                table: "products",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_products_price",
                table: "products",
                column: "price");

            migrationBuilder.CreateIndex(
                name: "title",
                table: "products",
                column: "title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_reviews_product_id",
                table: "reviews",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_reviews_user_id",
                table: "reviews",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "addresses");

            migrationBuilder.DropTable(
                name: "images");

            migrationBuilder.DropTable(
                name: "order_products");

            migrationBuilder.DropTable(
                name: "reviews");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "categories");
        }
    }
}
