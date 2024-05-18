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
                    { new Guid("1240fd2a-c34b-426f-932d-06226054d473"), new DateOnly(2024, 5, 17), "https://picsum.photos/200/?random=10", "Electronic", new DateOnly(2024, 5, 17) },
                    { new Guid("27b05085-73ca-4884-a330-58d006ea0937"), new DateOnly(2024, 5, 17), "https://picsum.photos/200/?random=1", "Furniture", new DateOnly(2024, 5, 17) },
                    { new Guid("5e202c87-9bd7-4163-bb66-2609f790d745"), new DateOnly(2024, 5, 17), "https://picsum.photos/200/?random=7", "Sports", new DateOnly(2024, 5, 17) },
                    { new Guid("8cc848c4-c89e-4d43-a253-dc7b9d13e515"), new DateOnly(2024, 5, 17), "https://picsum.photos/200/?random=10", "Toys", new DateOnly(2024, 5, 17) },
                    { new Guid("acd768a6-0939-44a5-9964-72999661f1bf"), new DateOnly(2024, 5, 17), "https://picsum.photos/200/?random=1", "Clothing", new DateOnly(2024, 5, 17) },
                    { new Guid("d53428db-3770-452c-9e32-2cce8f101b9b"), new DateOnly(2024, 5, 17), "https://picsum.photos/200/?random=7", "Books", new DateOnly(2024, 5, 17) }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "avatar", "created_date", "email", "name", "password", "role", "salt", "updated_date" },
                values: new object[,]
                {
                    { new Guid("494f0f98-14b8-49eb-a944-54ddce3341b6"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", new DateOnly(2024, 5, 17), "customer1@customer.com", "Customer1", "Q8zgm1xgvZEAzY78sQQHeRdHUT1sG7I1AeBvVPwHUG4=", UserRole.Customer, new byte[] { 191, 16, 205, 220, 216, 65, 162, 231, 86, 181, 136, 238, 37, 94, 55, 207 }, new DateOnly(2024, 5, 17) },
                    { new Guid("766249a0-5173-4685-95e3-805d5fc37319"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", new DateOnly(2024, 5, 17), "adnan@admin.com", "Adnan", "TKgRYL8rcBieG8e0gBOK335UQNb3ncknyEEXG5scnzM=", UserRole.Admin, new byte[] { 62, 129, 197, 205, 178, 3, 146, 240, 248, 68, 19, 40, 40, 44, 33, 19 }, new DateOnly(2024, 5, 17) },
                    { new Guid("991a2489-7e2a-4449-95c4-a7fbbbf33d0e"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", new DateOnly(2024, 5, 17), "yuanke@admin.com", "Yuanke", "bweRA4W5G+FKUOtiPeM8MHUDDS5ENURRJansNxy5TmI=", UserRole.Admin, new byte[] { 28, 205, 52, 197, 89, 26, 201, 167, 57, 50, 135, 215, 238, 97, 164, 22 }, new DateOnly(2024, 5, 17) },
                    { new Guid("aafb7154-e2fb-4236-bae0-01505a6e54ee"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", new DateOnly(2024, 5, 17), "binh@admin.com", "Binh", "bjJdEuxyZuDH7Xxp95YUTaER0azAJvihCLOjMKoZZn8=", UserRole.Admin, new byte[] { 23, 246, 39, 152, 135, 219, 73, 167, 227, 184, 21, 89, 18, 132, 48, 3 }, new DateOnly(2024, 5, 17) },
                    { new Guid("fb6ded65-c273-4901-8acf-aec8f75f24d7"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", new DateOnly(2024, 5, 17), "john@example.com", "Admin1", "A9Xi+dVpHNwak10oMx6U3U3QyBqOy0818qBZhC6m+Eg=", UserRole.Admin, new byte[] { 182, 101, 160, 78, 45, 109, 123, 119, 155, 235, 99, 16, 66, 168, 118, 205 }, new DateOnly(2024, 5, 17) }
                });

            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "id", "category_id", "created_date", "description", "inventory", "price", "title", "updated_date" },
                values: new object[,]
                {
                    { new Guid("17e3a904-7c68-4ef2-bd60-ad01a406497d"), new Guid("acd768a6-0939-44a5-9964-72999661f1bf"), new DateOnly(2024, 5, 17), "Description for Clothing Product 1", 100, 600, "Clothing Product 1", new DateOnly(2024, 5, 17) },
                    { new Guid("2ef4e81f-ca55-4fc4-bfd7-5d30cc30c910"), new Guid("d53428db-3770-452c-9e32-2cce8f101b9b"), new DateOnly(2024, 5, 17), "Description for Books Product 1", 100, 1000, "Books Product 1", new DateOnly(2024, 5, 17) },
                    { new Guid("8945be60-141b-4f60-9e97-3b426a2411ca"), new Guid("27b05085-73ca-4884-a330-58d006ea0937"), new DateOnly(2024, 5, 17), "Description for Furniture Product 1", 100, 400, "Furniture Product 1", new DateOnly(2024, 5, 17) },
                    { new Guid("c7d2dd69-b1c4-452a-94fe-1db98c5a0c10"), new Guid("1240fd2a-c34b-426f-932d-06226054d473"), new DateOnly(2024, 5, 17), "Description for Electronic Product 1", 100, 100, "Electronic Product 1", new DateOnly(2024, 5, 17) }
                });

            migrationBuilder.InsertData(
                table: "images",
                columns: new[] { "id", "created_date", "data", "product_id", "updated_date" },
                values: new object[,]
                {
                    { new Guid("075af4de-47ed-4c24-9e25-d671dff8e248"), new DateOnly(2024, 5, 17), new byte[] { 188, 83, 214, 136, 10, 77, 202, 196, 41, 112, 78, 127, 65, 99, 90, 137, 244, 8, 77, 174, 151, 68, 7, 187, 96, 127, 65, 53, 208, 160, 198, 91, 183, 4, 159, 255, 29, 253, 0, 205, 89, 25, 190, 171, 87, 151, 53, 138, 175, 47, 78, 201, 106, 150, 221, 124, 43, 187, 19, 195, 139, 1, 115, 226, 166, 206, 117, 0, 21, 190, 136, 203, 121, 38, 94, 241, 127, 196, 30, 27, 11, 239, 97, 170, 255, 169, 142, 236, 195, 135, 179, 24, 14, 83, 165, 177, 153, 164, 69, 56 }, new Guid("2ef4e81f-ca55-4fc4-bfd7-5d30cc30c910"), new DateOnly(2024, 5, 17) },
                    { new Guid("33c2718b-47f3-4281-927f-285f3ac88f6a"), new DateOnly(2024, 5, 17), new byte[] { 97, 176, 92, 205, 212, 238, 196, 242, 25, 150, 124, 122, 111, 223, 167, 148, 196, 144, 94, 63, 26, 5, 30, 128, 173, 28, 75, 52, 198, 184, 218, 199, 153, 0, 169, 156, 169, 7, 173, 178, 149, 233, 125, 68, 149, 194, 47, 8, 94, 237, 158, 220, 24, 41, 111, 244, 13, 174, 246, 54, 85, 202, 184, 80, 244, 115, 228, 5, 196, 14, 224, 27, 115, 214, 54, 106, 124, 52, 164, 179, 233, 81, 85, 19, 44, 21, 224, 130, 237, 230, 73, 115, 78, 89, 119, 249, 83, 4, 222, 192 }, new Guid("c7d2dd69-b1c4-452a-94fe-1db98c5a0c10"), new DateOnly(2024, 5, 17) },
                    { new Guid("40c28233-4574-4910-a7e9-6235feb3f961"), new DateOnly(2024, 5, 17), new byte[] { 102, 30, 226, 217, 140, 168, 43, 64, 143, 43, 14, 155, 123, 225, 162, 203, 224, 45, 137, 88, 197, 29, 162, 141, 26, 225, 48, 231, 194, 56, 180, 134, 228, 109, 210, 250, 226, 82, 17, 187, 100, 49, 234, 172, 32, 70, 120, 128, 210, 21, 116, 42, 104, 54, 194, 113, 52, 169, 213, 179, 181, 24, 73, 232, 27, 95, 194, 178, 186, 120, 49, 50, 231, 98, 68, 132, 159, 202, 3, 152, 23, 197, 252, 105, 63, 183, 153, 255, 9, 96, 4, 91, 181, 96, 251, 169, 169, 223, 176, 4 }, new Guid("8945be60-141b-4f60-9e97-3b426a2411ca"), new DateOnly(2024, 5, 17) },
                    { new Guid("54b56cba-b56b-47cc-a897-4be91e4951bd"), new DateOnly(2024, 5, 17), new byte[] { 105, 248, 195, 181, 86, 195, 228, 80, 232, 236, 126, 98, 2, 143, 207, 252, 108, 188, 192, 153, 105, 207, 243, 63, 107, 173, 127, 44, 234, 79, 46, 43, 31, 204, 10, 161, 158, 253, 119, 130, 251, 118, 33, 38, 194, 201, 53, 32, 14, 128, 14, 29, 76, 118, 220, 74, 92, 44, 43, 64, 95, 171, 251, 99, 154, 114, 168, 199, 84, 53, 34, 165, 153, 46, 152, 230, 167, 52, 188, 32, 23, 16, 19, 164, 174, 18, 149, 42, 253, 209, 87, 224, 216, 122, 185, 14, 102, 199, 35, 124 }, new Guid("8945be60-141b-4f60-9e97-3b426a2411ca"), new DateOnly(2024, 5, 17) },
                    { new Guid("6be9f424-175f-45e1-ae70-44dc0912cfd2"), new DateOnly(2024, 5, 17), new byte[] { 36, 88, 18, 173, 228, 158, 141, 111, 223, 84, 39, 202, 240, 14, 107, 42, 161, 46, 131, 192, 102, 166, 89, 50, 190, 85, 227, 49, 214, 85, 107, 50, 121, 187, 138, 159, 225, 216, 144, 143, 60, 212, 66, 9, 107, 222, 3, 97, 170, 150, 182, 196, 38, 154, 150, 122, 145, 186, 187, 106, 36, 5, 81, 164, 41, 114, 190, 62, 91, 88, 123, 228, 75, 18, 115, 225, 201, 10, 4, 125, 24, 175, 38, 244, 230, 246, 155, 149, 133, 206, 95, 173, 251, 231, 213, 89, 192, 195, 144, 1 }, new Guid("c7d2dd69-b1c4-452a-94fe-1db98c5a0c10"), new DateOnly(2024, 5, 17) },
                    { new Guid("7582bda1-32b7-4d83-984a-9ff7c709a54e"), new DateOnly(2024, 5, 17), new byte[] { 33, 138, 103, 149, 147, 46, 42, 21, 201, 89, 1, 39, 50, 87, 60, 129, 250, 65, 200, 163, 27, 132, 251, 215, 29, 209, 12, 73, 182, 212, 169, 137, 3, 15, 169, 227, 110, 133, 149, 90, 199, 76, 128, 245, 219, 141, 23, 9, 188, 208, 83, 41, 204, 210, 107, 130, 159, 113, 209, 149, 155, 80, 242, 122, 55, 156, 165, 180, 225, 28, 177, 80, 39, 32, 198, 211, 25, 233, 31, 26, 38, 237, 42, 164, 137, 40, 4, 38, 214, 162, 46, 115, 49, 150, 16, 42, 150, 108, 108, 10 }, new Guid("2ef4e81f-ca55-4fc4-bfd7-5d30cc30c910"), new DateOnly(2024, 5, 17) },
                    { new Guid("84896d58-97a9-449d-8c6a-caf2f1ffb39d"), new DateOnly(2024, 5, 17), new byte[] { 42, 220, 19, 198, 208, 144, 28, 235, 42, 32, 132, 1, 62, 103, 252, 238, 92, 112, 202, 219, 210, 172, 72, 18, 96, 165, 194, 176, 252, 211, 214, 159, 34, 68, 46, 107, 32, 189, 56, 87, 180, 245, 142, 89, 124, 205, 91, 41, 119, 228, 44, 99, 186, 153, 128, 123, 198, 14, 238, 172, 253, 80, 2, 144, 58, 70, 158, 89, 152, 99, 122, 92, 52, 13, 74, 151, 166, 5, 46, 193, 243, 48, 49, 45, 151, 73, 32, 234, 87, 57, 168, 228, 118, 192, 158, 77, 199, 204, 241, 117 }, new Guid("17e3a904-7c68-4ef2-bd60-ad01a406497d"), new DateOnly(2024, 5, 17) },
                    { new Guid("908dbc76-033a-40f5-afa2-f89dbbf79d10"), new DateOnly(2024, 5, 17), new byte[] { 36, 158, 114, 240, 220, 248, 124, 182, 224, 139, 251, 246, 172, 72, 77, 156, 166, 102, 127, 163, 5, 233, 206, 74, 252, 116, 174, 188, 215, 55, 187, 184, 134, 14, 81, 207, 30, 97, 156, 176, 232, 18, 56, 157, 152, 75, 46, 148, 2, 166, 167, 85, 204, 0, 91, 26, 44, 196, 54, 203, 255, 203, 30, 145, 127, 55, 22, 113, 19, 190, 141, 53, 184, 80, 172, 113, 11, 146, 71, 157, 176, 160, 222, 213, 79, 186, 169, 97, 95, 39, 57, 80, 156, 75, 125, 31, 243, 254, 204, 77 }, new Guid("17e3a904-7c68-4ef2-bd60-ad01a406497d"), new DateOnly(2024, 5, 17) },
                    { new Guid("ba2366f1-c53b-4420-8478-58ebd3507827"), new DateOnly(2024, 5, 17), new byte[] { 133, 149, 170, 60, 217, 6, 92, 190, 114, 153, 16, 238, 185, 254, 131, 134, 96, 68, 38, 150, 170, 128, 9, 146, 155, 102, 149, 84, 217, 236, 151, 39, 196, 245, 85, 189, 73, 42, 230, 205, 119, 61, 251, 107, 122, 177, 202, 56, 39, 28, 30, 146, 22, 160, 91, 156, 194, 42, 177, 245, 140, 133, 70, 113, 116, 89, 225, 129, 248, 208, 221, 116, 41, 102, 72, 179, 61, 95, 237, 237, 39, 108, 10, 73, 173, 21, 35, 118, 120, 255, 117, 229, 33, 153, 108, 11, 233, 190, 119, 42 }, new Guid("c7d2dd69-b1c4-452a-94fe-1db98c5a0c10"), new DateOnly(2024, 5, 17) },
                    { new Guid("c95bd4c9-5fa0-4b9a-b5ef-f4744b02bb33"), new DateOnly(2024, 5, 17), new byte[] { 140, 141, 100, 241, 31, 222, 52, 49, 104, 123, 95, 51, 117, 157, 158, 198, 165, 202, 145, 235, 112, 203, 196, 77, 46, 215, 153, 237, 90, 77, 240, 58, 236, 38, 150, 186, 194, 92, 9, 27, 16, 67, 204, 160, 41, 148, 243, 104, 21, 100, 74, 11, 92, 215, 39, 22, 255, 101, 246, 64, 147, 150, 58, 78, 96, 119, 42, 119, 139, 237, 6, 86, 85, 220, 63, 106, 117, 97, 60, 222, 25, 69, 104, 46, 155, 248, 179, 185, 80, 16, 64, 13, 35, 78, 30, 121, 145, 133, 141, 70 }, new Guid("2ef4e81f-ca55-4fc4-bfd7-5d30cc30c910"), new DateOnly(2024, 5, 17) },
                    { new Guid("da147dcb-2155-45ee-8ace-edcf6542dcfa"), new DateOnly(2024, 5, 17), new byte[] { 222, 165, 60, 209, 54, 122, 131, 184, 105, 184, 197, 188, 147, 103, 242, 82, 92, 78, 251, 49, 122, 81, 110, 227, 147, 33, 69, 118, 145, 38, 77, 57, 162, 104, 2, 181, 144, 92, 207, 92, 140, 252, 215, 218, 217, 141, 128, 149, 209, 168, 198, 241, 39, 209, 30, 174, 76, 140, 134, 247, 13, 226, 22, 20, 158, 63, 121, 126, 159, 112, 22, 172, 52, 110, 131, 164, 188, 159, 216, 18, 152, 144, 151, 186, 245, 15, 33, 245, 161, 29, 236, 179, 206, 141, 187, 13, 121, 55, 83, 35 }, new Guid("17e3a904-7c68-4ef2-bd60-ad01a406497d"), new DateOnly(2024, 5, 17) },
                    { new Guid("e46a8d27-15f6-426d-914b-afc3e237255d"), new DateOnly(2024, 5, 17), new byte[] { 232, 130, 214, 100, 143, 255, 184, 200, 100, 59, 141, 158, 52, 162, 3, 56, 112, 92, 122, 12, 12, 21, 3, 69, 40, 210, 22, 246, 148, 140, 82, 150, 151, 183, 73, 117, 203, 9, 174, 134, 224, 87, 44, 127, 158, 109, 193, 23, 63, 183, 70, 74, 188, 153, 139, 136, 176, 215, 110, 19, 84, 5, 184, 10, 21, 197, 201, 107, 121, 59, 49, 45, 229, 127, 188, 252, 97, 237, 15, 6, 70, 185, 59, 32, 224, 233, 230, 194, 58, 137, 201, 68, 213, 125, 165, 239, 168, 132, 208, 129 }, new Guid("8945be60-141b-4f60-9e97-3b426a2411ca"), new DateOnly(2024, 5, 17) }
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
