using System;
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
                .Annotation("Npgsql:Enum:user_role", "customer,admin");

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
                    avatar = table.Column<string>(type: "varchar(10485760)", nullable: true),
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
                    total = table.Column<int>(type: "integer", nullable: false),
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
                    { new Guid("029f17c3-11fb-46a4-ac76-f2271dca62c7"), new DateOnly(2024, 5, 22), "https://picsum.photos/200/?random=2", "Books", new DateOnly(2024, 5, 22) },
                    { new Guid("054f0d6c-ef43-4114-b6b3-6331c6d93c95"), new DateOnly(2024, 5, 22), "https://picsum.photos/200/?random=5", "Toys", new DateOnly(2024, 5, 22) },
                    { new Guid("19a1b81c-6982-4579-9690-5e01004cc5a0"), new DateOnly(2024, 5, 22), "https://picsum.photos/200/?random=1", "Clothing", new DateOnly(2024, 5, 22) },
                    { new Guid("5b4bd1d1-df4c-439b-9aff-2f15d291b59d"), new DateOnly(2024, 5, 22), "https://picsum.photos/200/?random=9", "Sports", new DateOnly(2024, 5, 22) },
                    { new Guid("c8ce938c-1128-4925-99d9-244558d03378"), new DateOnly(2024, 5, 22), "https://picsum.photos/200/?random=1", "Furniture", new DateOnly(2024, 5, 22) },
                    { new Guid("d409c004-cf79-4198-bff4-47901850c581"), new DateOnly(2024, 5, 22), "https://picsum.photos/200/?random=6", "Electronic", new DateOnly(2024, 5, 22) }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "avatar", "created_date", "email", "name", "password", "role", "salt", "updated_date" },
                values: new object[,]
                {
                    { new Guid("12354e22-a4c9-44e8-b7ea-b94f5b65b0ce"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", new DateOnly(2024, 5, 22), "yuanke@admin.com", "Yuanke", "xhqfhjpc+GjtcBWekUObuwmZ88XagBnrG78gTG+kLxA=", UserRole.Admin, new byte[] { 229, 114, 205, 227, 11, 194, 175, 95, 202, 180, 221, 147, 154, 23, 218, 107 }, new DateOnly(2024, 5, 22) },
                    { new Guid("4fac147c-a3e2-4bd7-ac92-32886cb512d4"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", new DateOnly(2024, 5, 22), "customer1@customer.com", "Customer1", "d+qvivdK6GSeYNMD7Izpv/sps7EzdgF9y8gzB8dQThI=", UserRole.Customer, new byte[] { 48, 77, 218, 165, 62, 199, 18, 141, 92, 157, 255, 120, 202, 94, 107, 213 }, new DateOnly(2024, 5, 22) },
                    { new Guid("7a95e0aa-7c6b-4018-92ef-a15be47cd311"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", new DateOnly(2024, 5, 22), "john@example.com", "Admin1", "hJ3ym2lDQX7Ggs7veqkXGR37v8MiruC+BxM6lJYVNAg=", UserRole.Admin, new byte[] { 124, 167, 241, 90, 150, 218, 53, 35, 235, 247, 62, 114, 66, 159, 181, 89 }, new DateOnly(2024, 5, 22) },
                    { new Guid("ad884e12-4ed0-4001-8d2e-bd75939327ed"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", new DateOnly(2024, 5, 22), "binh@admin.com", "Binh", "Lqx7DdFDrYeDyg5Fdc4RkXYjeBBMe5An8PUcxzYy19w=", UserRole.Admin, new byte[] { 21, 206, 135, 133, 100, 6, 9, 40, 180, 231, 96, 2, 79, 88, 113, 175 }, new DateOnly(2024, 5, 22) },
                    { new Guid("fccfe50f-1991-4e55-b2ff-45bc21aa53d8"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", new DateOnly(2024, 5, 22), "adnan@admin.com", "Adnan", "ddzEptxJ/J+XwY2Cj7egOKk6HniPz+b3CDTIDBWYtyo=", UserRole.Admin, new byte[] { 135, 13, 130, 160, 31, 227, 244, 174, 6, 151, 211, 238, 17, 148, 124, 248 }, new DateOnly(2024, 5, 22) }
                });

            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "id", "category_id", "created_date", "description", "inventory", "price", "title", "updated_date" },
                values: new object[,]
                {
                    { new Guid("35e09e82-f8c1-481e-bc38-e2e82abc21e2"), new Guid("d409c004-cf79-4198-bff4-47901850c581"), new DateOnly(2024, 5, 22), "Description for Electronic Product 1", 100, 200, "Electronic Product 1", new DateOnly(2024, 5, 22) },
                    { new Guid("50b0be44-c149-4bd1-8f35-6d8445910b00"), new Guid("029f17c3-11fb-46a4-ac76-f2271dca62c7"), new DateOnly(2024, 5, 22), "Description for Books Product 1", 100, 600, "Books Product 1", new DateOnly(2024, 5, 22) },
                    { new Guid("8f5a76a5-7b3c-4006-925f-bf326f729e91"), new Guid("19a1b81c-6982-4579-9690-5e01004cc5a0"), new DateOnly(2024, 5, 22), "Description for Clothing Product 1", 100, 400, "Clothing Product 1", new DateOnly(2024, 5, 22) },
                    { new Guid("cd016f01-fe89-4e68-901c-4282727a2bbf"), new Guid("c8ce938c-1128-4925-99d9-244558d03378"), new DateOnly(2024, 5, 22), "Description for Furniture Product 1", 100, 1000, "Furniture Product 1", new DateOnly(2024, 5, 22) }
                });

            migrationBuilder.InsertData(
                table: "images",
                columns: new[] { "id", "created_date", "data", "product_id", "updated_date" },
                values: new object[,]
                {
                    { new Guid("0b10f017-aa0f-4518-b0bc-4b3682b9c863"), new DateOnly(2024, 5, 22), new byte[] { 137, 175, 10, 117, 217, 158, 50, 58, 248, 23, 44, 221, 19, 41, 122, 170, 217, 111, 227, 30, 117, 73, 69, 41, 216, 76, 63, 111, 87, 19, 210, 130, 15, 225, 76, 114, 65, 255, 183, 142, 88, 132, 151, 113, 3, 137, 132, 223, 221, 28, 175, 225, 50, 137, 13, 130, 165, 220, 112, 99, 117, 152, 226, 64, 97, 239, 253, 24, 195, 254, 184, 186, 168, 55, 95, 153, 11, 197, 11, 168, 230, 251, 64, 159, 241, 48, 212, 85, 4, 128, 8, 209, 180, 35, 169, 207, 157, 32, 47, 148 }, new Guid("35e09e82-f8c1-481e-bc38-e2e82abc21e2"), new DateOnly(2024, 5, 22) },
                    { new Guid("203d02f4-6dff-4f61-bacb-5850e7250dd5"), new DateOnly(2024, 5, 22), new byte[] { 226, 16, 110, 161, 90, 89, 4, 123, 127, 253, 50, 100, 153, 211, 187, 222, 153, 244, 51, 133, 125, 44, 124, 86, 17, 118, 251, 208, 6, 44, 97, 38, 180, 5, 241, 30, 117, 106, 108, 186, 179, 182, 29, 152, 87, 44, 8, 162, 43, 35, 241, 229, 214, 107, 36, 162, 224, 27, 142, 15, 136, 211, 197, 134, 109, 213, 58, 25, 154, 107, 195, 80, 210, 59, 205, 172, 7, 48, 47, 168, 56, 240, 216, 237, 228, 158, 59, 163, 3, 105, 133, 227, 218, 204, 90, 244, 168, 27, 4, 115 }, new Guid("50b0be44-c149-4bd1-8f35-6d8445910b00"), new DateOnly(2024, 5, 22) },
                    { new Guid("91873563-3623-42ba-b7a8-e6f7abfd4026"), new DateOnly(2024, 5, 22), new byte[] { 27, 43, 17, 70, 129, 171, 82, 221, 115, 118, 86, 57, 139, 226, 231, 5, 14, 169, 191, 72, 76, 211, 118, 38, 152, 89, 91, 51, 239, 135, 68, 168, 45, 109, 194, 164, 101, 72, 72, 199, 229, 248, 219, 153, 172, 178, 35, 118, 101, 49, 14, 17, 132, 25, 48, 122, 87, 175, 178, 222, 184, 170, 2, 216, 17, 28, 52, 128, 212, 1, 11, 70, 50, 109, 128, 204, 235, 33, 214, 122, 1, 157, 61, 124, 25, 200, 102, 120, 53, 201, 143, 52, 241, 173, 70, 41, 151, 33, 14, 148 }, new Guid("8f5a76a5-7b3c-4006-925f-bf326f729e91"), new DateOnly(2024, 5, 22) },
                    { new Guid("aea8608c-48ed-4304-af46-f2912190a1f3"), new DateOnly(2024, 5, 22), new byte[] { 12, 236, 122, 202, 204, 241, 8, 35, 106, 148, 94, 201, 47, 157, 192, 192, 69, 82, 35, 88, 36, 222, 212, 166, 45, 113, 212, 104, 23, 113, 47, 18, 212, 24, 125, 55, 200, 141, 247, 135, 6, 203, 7, 35, 96, 11, 47, 18, 245, 39, 234, 60, 237, 175, 106, 119, 119, 60, 253, 136, 107, 154, 158, 247, 177, 140, 255, 200, 185, 58, 147, 167, 162, 80, 23, 231, 119, 7, 209, 51, 102, 17, 83, 106, 122, 175, 64, 103, 233, 230, 121, 138, 165, 98, 102, 28, 195, 3, 86, 125 }, new Guid("35e09e82-f8c1-481e-bc38-e2e82abc21e2"), new DateOnly(2024, 5, 22) },
                    { new Guid("ba113946-16e2-4b6d-8dd9-8c17fc29ee91"), new DateOnly(2024, 5, 22), new byte[] { 220, 40, 97, 192, 199, 139, 86, 229, 64, 182, 245, 253, 141, 111, 171, 9, 118, 13, 244, 158, 102, 209, 234, 213, 168, 38, 121, 38, 112, 3, 242, 190, 24, 37, 187, 125, 176, 177, 76, 43, 117, 4, 158, 183, 105, 187, 241, 130, 195, 104, 169, 254, 198, 167, 22, 60, 32, 84, 171, 2, 255, 185, 174, 116, 145, 232, 177, 75, 50, 10, 217, 22, 181, 90, 233, 215, 137, 116, 122, 49, 99, 77, 250, 16, 139, 247, 224, 11, 112, 51, 15, 62, 170, 246, 94, 220, 47, 164, 125, 220 }, new Guid("cd016f01-fe89-4e68-901c-4282727a2bbf"), new DateOnly(2024, 5, 22) },
                    { new Guid("bd494cbc-e783-4522-bd3e-088655f867ea"), new DateOnly(2024, 5, 22), new byte[] { 199, 54, 13, 189, 175, 139, 45, 66, 1, 243, 67, 14, 193, 7, 132, 176, 197, 207, 83, 149, 235, 177, 147, 51, 193, 220, 14, 23, 4, 104, 201, 86, 80, 85, 1, 26, 195, 53, 31, 211, 98, 83, 82, 38, 86, 110, 117, 228, 188, 94, 10, 136, 55, 139, 145, 33, 86, 142, 146, 149, 215, 216, 234, 202, 140, 141, 131, 69, 140, 4, 162, 83, 51, 162, 104, 8, 35, 254, 246, 217, 67, 131, 214, 99, 188, 38, 107, 188, 16, 212, 32, 144, 207, 175, 15, 217, 109, 166, 255, 78 }, new Guid("50b0be44-c149-4bd1-8f35-6d8445910b00"), new DateOnly(2024, 5, 22) },
                    { new Guid("e2bb3e58-b822-48fa-b9a8-68ed17f17fad"), new DateOnly(2024, 5, 22), new byte[] { 69, 223, 109, 144, 174, 28, 150, 211, 35, 51, 251, 155, 72, 199, 83, 123, 213, 161, 147, 156, 185, 215, 205, 150, 230, 93, 13, 200, 102, 235, 53, 231, 208, 245, 213, 249, 177, 19, 112, 52, 139, 221, 0, 215, 97, 203, 112, 122, 162, 220, 11, 48, 175, 0, 30, 114, 55, 167, 62, 52, 178, 144, 24, 223, 90, 25, 8, 112, 249, 34, 110, 183, 254, 4, 144, 18, 6, 216, 78, 17, 236, 197, 106, 143, 238, 66, 95, 230, 138, 245, 213, 23, 47, 205, 210, 128, 131, 71, 183, 78 }, new Guid("8f5a76a5-7b3c-4006-925f-bf326f729e91"), new DateOnly(2024, 5, 22) },
                    { new Guid("e4e8f1b2-3716-4dd7-87bd-58b04ca9e0d6"), new DateOnly(2024, 5, 22), new byte[] { 240, 73, 157, 153, 62, 103, 132, 163, 103, 110, 162, 229, 187, 68, 142, 169, 170, 33, 165, 108, 236, 206, 48, 118, 111, 242, 38, 199, 123, 87, 141, 199, 187, 250, 138, 225, 183, 4, 56, 48, 170, 147, 73, 211, 186, 249, 228, 141, 255, 227, 138, 88, 126, 51, 239, 71, 251, 59, 85, 129, 71, 207, 32, 216, 155, 57, 43, 138, 41, 164, 56, 190, 164, 167, 141, 188, 100, 9, 97, 211, 230, 130, 150, 171, 214, 35, 151, 197, 2, 94, 73, 135, 198, 224, 115, 118, 45, 81, 162, 210 }, new Guid("8f5a76a5-7b3c-4006-925f-bf326f729e91"), new DateOnly(2024, 5, 22) },
                    { new Guid("e6fbc2d7-9169-4cc0-a455-8b017047e0e5"), new DateOnly(2024, 5, 22), new byte[] { 254, 248, 156, 42, 80, 63, 207, 80, 224, 54, 29, 224, 40, 62, 40, 173, 181, 193, 108, 124, 51, 5, 254, 192, 181, 115, 75, 174, 83, 26, 199, 113, 168, 214, 167, 101, 240, 16, 100, 106, 30, 26, 250, 119, 158, 113, 40, 90, 151, 80, 78, 218, 22, 130, 25, 50, 74, 145, 10, 174, 142, 90, 102, 162, 115, 219, 38, 220, 175, 11, 153, 50, 132, 228, 116, 229, 215, 244, 196, 243, 165, 10, 76, 212, 1, 233, 133, 25, 75, 205, 90, 27, 60, 192, 67, 5, 114, 149, 241, 96 }, new Guid("cd016f01-fe89-4e68-901c-4282727a2bbf"), new DateOnly(2024, 5, 22) },
                    { new Guid("eb9eec9d-619c-4f92-a9bd-7c7ec6a285a7"), new DateOnly(2024, 5, 22), new byte[] { 100, 216, 242, 55, 231, 163, 71, 182, 85, 241, 69, 71, 9, 67, 73, 63, 173, 183, 53, 206, 179, 42, 197, 30, 68, 212, 9, 238, 83, 251, 219, 93, 149, 156, 219, 225, 95, 28, 18, 156, 114, 133, 49, 48, 83, 250, 200, 12, 155, 35, 101, 58, 85, 205, 119, 255, 156, 43, 238, 125, 3, 62, 113, 115, 130, 101, 153, 71, 202, 211, 54, 56, 132, 52, 243, 198, 10, 151, 99, 184, 84, 253, 253, 11, 9, 235, 33, 243, 148, 248, 153, 55, 76, 143, 231, 21, 243, 100, 72, 150 }, new Guid("35e09e82-f8c1-481e-bc38-e2e82abc21e2"), new DateOnly(2024, 5, 22) },
                    { new Guid("ec5132fd-af00-410e-a4d2-999be7c81940"), new DateOnly(2024, 5, 22), new byte[] { 204, 217, 174, 163, 71, 98, 83, 216, 56, 131, 122, 178, 143, 251, 59, 99, 20, 56, 132, 216, 26, 5, 210, 35, 84, 219, 8, 156, 34, 248, 206, 227, 143, 242, 254, 142, 100, 208, 75, 226, 3, 72, 177, 216, 172, 47, 229, 47, 52, 194, 163, 244, 185, 4, 122, 219, 83, 213, 92, 170, 130, 73, 222, 234, 116, 59, 219, 218, 44, 1, 199, 40, 237, 127, 232, 196, 132, 92, 100, 49, 246, 176, 47, 104, 66, 142, 80, 40, 181, 144, 191, 3, 234, 19, 150, 188, 182, 13, 132, 82 }, new Guid("cd016f01-fe89-4e68-901c-4282727a2bbf"), new DateOnly(2024, 5, 22) },
                    { new Guid("f557376d-b0c2-4fb3-91a8-ea4ae1d1db34"), new DateOnly(2024, 5, 22), new byte[] { 0, 175, 95, 0, 96, 248, 16, 72, 52, 148, 1, 152, 82, 36, 66, 60, 27, 197, 45, 205, 20, 225, 231, 172, 23, 102, 191, 203, 45, 28, 13, 66, 75, 31, 116, 72, 254, 79, 211, 164, 106, 49, 222, 214, 210, 160, 108, 34, 193, 6, 175, 199, 225, 140, 128, 3, 14, 19, 162, 19, 234, 234, 91, 139, 96, 127, 51, 89, 147, 138, 171, 62, 221, 15, 27, 249, 155, 202, 103, 47, 241, 254, 28, 200, 1, 155, 62, 203, 179, 244, 98, 225, 198, 99, 145, 72, 196, 112, 12, 4 }, new Guid("50b0be44-c149-4bd1-8f35-6d8445910b00"), new DateOnly(2024, 5, 22) }
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
