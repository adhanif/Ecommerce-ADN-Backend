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
                    address_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<OrderStatus>(type: "order_status", nullable: false),
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
                    url = table.Column<string>(type: "varchar", nullable: false),
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
                    { new Guid("4f6e5bcb-fb63-4e21-8b38-7b2044b6a500"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=7", "Electronic", new DateOnly(2024, 5, 16) },
                    { new Guid("53d09644-26d3-4ec2-84da-7c6629ff6797"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=1", "Furniture", new DateOnly(2024, 5, 16) },
                    { new Guid("6a2c792c-377f-4db9-aceb-e02dec8fc9c4"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=8", "Sports", new DateOnly(2024, 5, 16) },
                    { new Guid("84546ec1-cf78-4892-9362-a890f6eb6d27"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=9", "Clothing", new DateOnly(2024, 5, 16) },
                    { new Guid("a557a0fd-f2ba-47fc-9d04-47829a5f72da"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=5", "Books", new DateOnly(2024, 5, 16) },
                    { new Guid("ef861389-ae57-44b8-81c1-e86f845a9ff8"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=4", "Toys", new DateOnly(2024, 5, 16) }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "avatar", "created_date", "email", "name", "password", "role", "salt", "updated_date" },
                values: new object[,]
                {
                    { new Guid("2d0971cb-ec03-43b1-8649-f03557101896"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", new DateOnly(2024, 5, 16), "customer1@customer.com", "Customer1", "bAJpU7dI3c6SUpneQVgZlomfjCxa+q094bT293KfKQU=", UserRole.Admin, new byte[] { 44, 136, 210, 104, 3, 2, 158, 134, 221, 164, 123, 125, 132, 222, 89, 67 }, new DateOnly(2024, 5, 16) },
                    { new Guid("563f8856-4fd5-4277-8dea-37114068b7c1"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", new DateOnly(2024, 5, 16), "john@example.com", "Admin1", "7RW2lvEixc3d+JNfA1axmiSpYHpgHM0hd3sgMCg6eAI=", UserRole.Admin, new byte[] { 144, 188, 129, 196, 170, 212, 63, 205, 139, 2, 91, 41, 116, 139, 52, 4 }, new DateOnly(2024, 5, 16) },
                    { new Guid("6fd5dbac-d20f-47c4-8a04-90a4ffdc5a5a"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", new DateOnly(2024, 5, 16), "binh@admin.com", "Binh", "78zSevoGlW3rNwsCZ9HKfo5qcN4UXbnms/SUQnW117k=", UserRole.Admin, new byte[] { 106, 204, 188, 45, 91, 197, 202, 179, 101, 44, 165, 193, 9, 73, 90, 11 }, new DateOnly(2024, 5, 16) },
                    { new Guid("84d40560-edd0-412c-9f8f-fa69b1669ad8"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", new DateOnly(2024, 5, 16), "yuanke@admin.com", "Yuanke", "sL7nEJjIX2kRRUWE+LQzvDFO0MfakrPqw5YZT/nrAJY=", UserRole.Admin, new byte[] { 76, 235, 244, 65, 139, 232, 160, 38, 247, 43, 15, 118, 21, 224, 254, 105 }, new DateOnly(2024, 5, 16) },
                    { new Guid("f03bbaf5-49a4-4437-ba62-ddd3994cb5d2"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", new DateOnly(2024, 5, 16), "adnan@admin.com", "Adnan", "Z7uKJWCCLwjdBtPnoYZpe0sslja48vLy0pkkKh2QT8Q=", UserRole.Admin, new byte[] { 54, 194, 96, 209, 95, 121, 103, 11, 153, 21, 235, 111, 98, 85, 188, 57 }, new DateOnly(2024, 5, 16) }
                });

            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "id", "category_id", "created_date", "description", "inventory", "price", "title", "updated_date" },
                values: new object[,]
                {
                    { new Guid("82d0e0cb-1aaf-48cc-b545-aafd488a5eec"), new Guid("4f6e5bcb-fb63-4e21-8b38-7b2044b6a500"), new DateOnly(2024, 5, 16), "Description for Electronic Product 2", 100, 700, "Electronic Product 2", new DateOnly(2024, 5, 16) },
                    { new Guid("979fd8f4-435d-4dcf-9f06-e90c44c063ee"), new Guid("4f6e5bcb-fb63-4e21-8b38-7b2044b6a500"), new DateOnly(2024, 5, 16), "Description for Electronic Product 1", 100, 600, "Electronic Product 1", new DateOnly(2024, 5, 16) },
                    { new Guid("9c0295d0-9d42-4008-9806-71e5abd47d0e"), new Guid("84546ec1-cf78-4892-9362-a890f6eb6d27"), new DateOnly(2024, 5, 16), "Description for Clothing Product 2", 100, 200, "Clothing Product 2", new DateOnly(2024, 5, 16) },
                    { new Guid("9f734442-ecc3-47f4-a74a-e591bb4793fb"), new Guid("53d09644-26d3-4ec2-84da-7c6629ff6797"), new DateOnly(2024, 5, 16), "Description for Furniture Product 2", 100, 500, "Furniture Product 2", new DateOnly(2024, 5, 16) },
                    { new Guid("a41a4f1e-983a-479a-9559-241f52bc9185"), new Guid("53d09644-26d3-4ec2-84da-7c6629ff6797"), new DateOnly(2024, 5, 16), "Description for Furniture Product 1", 100, 500, "Furniture Product 1", new DateOnly(2024, 5, 16) },
                    { new Guid("d334efda-e650-414f-8be8-c57f76959a6c"), new Guid("84546ec1-cf78-4892-9362-a890f6eb6d27"), new DateOnly(2024, 5, 16), "Description for Clothing Product 1", 100, 200, "Clothing Product 1", new DateOnly(2024, 5, 16) },
                    { new Guid("e2818a20-d14c-42f9-9eea-9982aaebc091"), new Guid("a557a0fd-f2ba-47fc-9d04-47829a5f72da"), new DateOnly(2024, 5, 16), "Description for Books Product 2", 100, 500, "Books Product 2", new DateOnly(2024, 5, 16) },
                    { new Guid("e8939199-16e1-41a5-b18c-3883e6507998"), new Guid("a557a0fd-f2ba-47fc-9d04-47829a5f72da"), new DateOnly(2024, 5, 16), "Description for Books Product 1", 100, 200, "Books Product 1", new DateOnly(2024, 5, 16) }
                });

            migrationBuilder.InsertData(
                table: "images",
                columns: new[] { "id", "created_date", "data", "product_id", "updated_date", "url" },
                values: new object[,]
                {
                    { new Guid("1588b168-e86c-4f3d-a5bf-959d92ec07cb"), new DateOnly(2024, 5, 16), new byte[] { 207, 127, 167, 203, 224, 211, 55, 45, 84, 115, 99, 66, 207, 85, 14, 40, 34, 49, 165, 3, 84, 36, 49, 182, 32, 249, 66, 120, 207, 143, 55, 192, 190, 74, 248, 169, 254, 248, 251, 168, 199, 17, 216, 241, 31, 227, 83, 37, 207, 13, 170, 31, 107, 140, 209, 92, 73, 19, 157, 20, 163, 91, 62, 39, 69, 169, 249, 135, 88, 50, 61, 135, 158, 208, 123, 12, 191, 209, 175, 18, 167, 172, 75, 203, 185, 89, 239, 26, 223, 157, 138, 95, 62, 251, 56, 244, 73, 209, 107, 0 }, new Guid("979fd8f4-435d-4dcf-9f06-e90c44c063ee"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=848" },
                    { new Guid("1e4fec82-0226-4607-9d3c-60926b326c3f"), new DateOnly(2024, 5, 16), new byte[] { 108, 36, 44, 14, 168, 74, 66, 40, 23, 215, 80, 192, 225, 44, 120, 18, 160, 115, 50, 39, 251, 91, 102, 23, 67, 145, 12, 44, 49, 55, 74, 108, 84, 8, 209, 150, 3, 8, 149, 114, 70, 174, 123, 156, 5, 233, 42, 143, 176, 99, 131, 211, 54, 186, 202, 77, 86, 214, 234, 139, 119, 90, 255, 146, 105, 69, 101, 198, 88, 77, 169, 128, 71, 58, 161, 37, 211, 158, 159, 2, 107, 20, 253, 72, 144, 246, 16, 2, 246, 215, 84, 53, 87, 220, 154, 1, 107, 172, 101, 160 }, new Guid("a41a4f1e-983a-479a-9559-241f52bc9185"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=943" },
                    { new Guid("22da72b6-8cd5-4187-a517-5c37a672423b"), new DateOnly(2024, 5, 16), new byte[] { 115, 154, 213, 122, 162, 150, 20, 144, 33, 209, 74, 94, 89, 101, 157, 25, 32, 194, 87, 220, 243, 191, 157, 63, 180, 88, 169, 65, 65, 239, 66, 146, 139, 175, 30, 237, 182, 160, 140, 181, 182, 156, 199, 16, 26, 93, 27, 101, 33, 127, 150, 175, 176, 98, 56, 145, 58, 170, 182, 161, 74, 74, 246, 205, 172, 134, 86, 127, 102, 245, 126, 127, 231, 161, 86, 5, 171, 56, 108, 143, 149, 187, 205, 188, 40, 182, 81, 150, 212, 129, 253, 12, 48, 181, 236, 192, 138, 28, 229, 99 }, new Guid("e2818a20-d14c-42f9-9eea-9982aaebc091"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=358" },
                    { new Guid("25e71ef2-4f39-4d21-aa85-425ed3b71e9c"), new DateOnly(2024, 5, 16), new byte[] { 99, 59, 237, 227, 180, 70, 28, 58, 35, 164, 43, 152, 70, 80, 210, 145, 206, 66, 248, 244, 130, 125, 165, 80, 42, 239, 126, 172, 185, 65, 75, 204, 214, 73, 193, 96, 97, 141, 112, 105, 41, 43, 248, 140, 136, 234, 18, 244, 11, 103, 101, 230, 184, 130, 154, 89, 49, 173, 92, 6, 196, 126, 82, 110, 172, 167, 95, 109, 232, 94, 198, 112, 251, 147, 217, 57, 106, 36, 1, 64, 150, 92, 146, 54, 127, 187, 38, 87, 124, 31, 56, 234, 52, 88, 175, 75, 111, 108, 231, 140 }, new Guid("979fd8f4-435d-4dcf-9f06-e90c44c063ee"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=486" },
                    { new Guid("4f654ab1-88f3-48d7-be08-05c17ffa50ef"), new DateOnly(2024, 5, 16), new byte[] { 240, 243, 233, 80, 246, 72, 73, 254, 128, 58, 128, 9, 210, 67, 80, 34, 185, 219, 213, 114, 17, 165, 239, 139, 202, 61, 36, 159, 222, 63, 181, 88, 206, 247, 175, 57, 106, 167, 180, 68, 143, 77, 117, 66, 33, 86, 140, 62, 152, 183, 216, 87, 77, 164, 25, 91, 168, 15, 20, 160, 60, 64, 51, 58, 69, 27, 196, 254, 55, 1, 134, 107, 50, 218, 77, 245, 213, 40, 181, 191, 49, 206, 47, 81, 138, 146, 87, 139, 54, 48, 101, 126, 214, 137, 153, 233, 83, 57, 52, 146 }, new Guid("e8939199-16e1-41a5-b18c-3883e6507998"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=315" },
                    { new Guid("59e6bf7e-6e48-4222-bd81-2270b1b6643f"), new DateOnly(2024, 5, 16), new byte[] { 63, 69, 187, 185, 98, 217, 225, 206, 128, 98, 8, 240, 27, 50, 56, 100, 1, 148, 116, 104, 81, 186, 18, 138, 167, 237, 75, 5, 156, 224, 23, 209, 223, 178, 202, 73, 198, 224, 218, 195, 117, 144, 35, 185, 249, 227, 50, 111, 202, 201, 170, 245, 186, 121, 192, 38, 196, 72, 219, 119, 155, 238, 3, 110, 99, 61, 190, 12, 234, 147, 151, 29, 103, 206, 116, 167, 176, 125, 230, 155, 161, 29, 7, 34, 41, 10, 181, 109, 70, 150, 45, 21, 65, 89, 59, 131, 91, 246, 152, 196 }, new Guid("979fd8f4-435d-4dcf-9f06-e90c44c063ee"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=923" },
                    { new Guid("5f7686ff-9689-448d-862e-8dccbb00031e"), new DateOnly(2024, 5, 16), new byte[] { 219, 126, 78, 164, 23, 197, 250, 107, 163, 21, 78, 172, 237, 157, 188, 137, 151, 220, 229, 36, 94, 197, 243, 92, 236, 52, 234, 143, 115, 8, 164, 124, 158, 251, 236, 213, 126, 248, 104, 217, 40, 177, 168, 131, 198, 219, 0, 202, 183, 122, 1, 43, 86, 210, 181, 91, 215, 161, 121, 49, 246, 163, 132, 215, 153, 17, 60, 201, 128, 158, 192, 34, 22, 67, 90, 187, 128, 231, 44, 5, 5, 126, 186, 230, 22, 19, 174, 231, 109, 160, 169, 139, 129, 63, 48, 188, 14, 48, 187, 112 }, new Guid("e8939199-16e1-41a5-b18c-3883e6507998"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=199" },
                    { new Guid("704c75ef-a820-426d-9ba8-4de8e235f4e7"), new DateOnly(2024, 5, 16), new byte[] { 110, 225, 61, 194, 244, 121, 94, 218, 115, 61, 220, 192, 180, 235, 20, 161, 127, 131, 200, 85, 193, 153, 204, 192, 133, 201, 15, 99, 171, 131, 100, 85, 206, 88, 102, 111, 140, 94, 118, 199, 99, 175, 159, 216, 112, 218, 255, 136, 69, 30, 206, 243, 198, 253, 70, 180, 93, 18, 196, 60, 59, 222, 158, 0, 1, 60, 26, 75, 192, 13, 200, 139, 227, 171, 172, 77, 42, 130, 99, 175, 27, 198, 149, 20, 28, 248, 67, 44, 178, 103, 77, 3, 215, 125, 40, 47, 214, 88, 228, 96 }, new Guid("e2818a20-d14c-42f9-9eea-9982aaebc091"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=590" },
                    { new Guid("76eb08f4-f04b-4938-a59d-2c3ac47db834"), new DateOnly(2024, 5, 16), new byte[] { 132, 96, 218, 107, 213, 184, 57, 17, 35, 184, 181, 178, 206, 28, 216, 14, 13, 33, 76, 206, 71, 103, 80, 49, 227, 32, 99, 202, 73, 121, 120, 71, 20, 80, 116, 219, 103, 51, 43, 19, 88, 103, 133, 160, 212, 128, 7, 83, 204, 171, 187, 239, 109, 126, 8, 48, 19, 208, 203, 154, 3, 195, 126, 207, 230, 39, 234, 207, 102, 101, 255, 24, 21, 237, 38, 38, 243, 185, 81, 201, 253, 130, 215, 63, 51, 226, 71, 55, 36, 155, 66, 113, 198, 223, 36, 186, 251, 144, 123, 61 }, new Guid("9f734442-ecc3-47f4-a74a-e591bb4793fb"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=521" },
                    { new Guid("814d5793-a4ac-4bb5-9240-51147dfefee1"), new DateOnly(2024, 5, 16), new byte[] { 155, 179, 229, 75, 84, 127, 184, 94, 141, 242, 9, 3, 110, 5, 220, 242, 250, 29, 9, 33, 182, 100, 206, 56, 140, 90, 229, 92, 19, 185, 112, 197, 84, 29, 52, 94, 50, 206, 28, 254, 53, 13, 242, 118, 254, 249, 87, 48, 156, 117, 142, 211, 30, 95, 25, 44, 33, 254, 70, 105, 160, 222, 1, 172, 255, 236, 194, 15, 36, 51, 57, 160, 15, 188, 161, 30, 123, 67, 140, 4, 108, 79, 226, 23, 232, 177, 46, 156, 251, 45, 165, 8, 135, 82, 253, 12, 163, 255, 175, 175 }, new Guid("9f734442-ecc3-47f4-a74a-e591bb4793fb"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=485" },
                    { new Guid("862cf239-0da7-4a78-9b11-5615994dcda7"), new DateOnly(2024, 5, 16), new byte[] { 130, 58, 136, 142, 174, 255, 171, 201, 215, 236, 110, 217, 94, 82, 107, 125, 146, 150, 156, 88, 104, 175, 233, 250, 220, 184, 35, 62, 137, 239, 82, 239, 68, 255, 116, 8, 219, 223, 34, 231, 225, 125, 164, 149, 72, 125, 33, 190, 168, 185, 240, 69, 164, 174, 254, 120, 60, 141, 254, 199, 129, 116, 152, 116, 138, 124, 162, 237, 86, 144, 166, 127, 146, 79, 194, 180, 91, 190, 132, 66, 16, 54, 106, 254, 96, 195, 88, 170, 145, 247, 53, 140, 194, 137, 140, 22, 68, 196, 1, 46 }, new Guid("e8939199-16e1-41a5-b18c-3883e6507998"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=133" },
                    { new Guid("9187d35a-8c6b-47ba-bfb9-579949ca2238"), new DateOnly(2024, 5, 16), new byte[] { 193, 164, 123, 162, 175, 179, 175, 219, 250, 246, 236, 101, 188, 27, 218, 129, 245, 206, 215, 197, 247, 95, 240, 121, 9, 26, 221, 79, 73, 52, 41, 45, 89, 86, 40, 89, 131, 93, 143, 117, 5, 15, 87, 85, 168, 105, 200, 172, 230, 127, 166, 114, 181, 224, 14, 82, 11, 85, 243, 209, 115, 227, 234, 39, 115, 158, 62, 3, 28, 158, 66, 16, 8, 42, 34, 186, 172, 27, 94, 197, 147, 171, 186, 37, 14, 45, 90, 28, 105, 101, 37, 212, 206, 95, 22, 217, 173, 84, 254, 107 }, new Guid("9c0295d0-9d42-4008-9806-71e5abd47d0e"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=459" },
                    { new Guid("96778fdc-e53f-4bc8-805b-cd5e5fc03e4b"), new DateOnly(2024, 5, 16), new byte[] { 117, 205, 87, 146, 171, 179, 192, 24, 176, 233, 198, 157, 237, 94, 19, 65, 228, 39, 32, 105, 252, 237, 133, 116, 91, 251, 61, 183, 250, 71, 107, 94, 220, 147, 207, 25, 96, 37, 83, 131, 148, 75, 165, 5, 12, 17, 36, 197, 178, 51, 251, 37, 238, 135, 203, 240, 51, 42, 2, 154, 50, 44, 122, 226, 38, 14, 196, 238, 2, 193, 150, 221, 164, 46, 32, 83, 248, 34, 43, 179, 57, 191, 224, 141, 252, 147, 80, 89, 56, 95, 226, 176, 90, 104, 45, 164, 201, 96, 32, 158 }, new Guid("a41a4f1e-983a-479a-9559-241f52bc9185"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=317" },
                    { new Guid("a2fbfd7f-e5eb-4bea-a059-e39a453e7357"), new DateOnly(2024, 5, 16), new byte[] { 52, 30, 144, 221, 51, 8, 75, 118, 197, 187, 76, 75, 65, 8, 4, 205, 228, 36, 40, 59, 101, 4, 150, 78, 38, 110, 112, 206, 0, 133, 67, 129, 52, 81, 16, 131, 224, 143, 195, 12, 254, 56, 255, 36, 34, 25, 27, 72, 242, 87, 156, 192, 223, 126, 80, 96, 75, 35, 202, 181, 103, 167, 74, 58, 34, 71, 139, 34, 168, 163, 71, 75, 233, 124, 200, 200, 112, 213, 206, 233, 147, 167, 244, 202, 215, 53, 230, 26, 1, 234, 132, 112, 110, 241, 34, 20, 152, 142, 247, 9 }, new Guid("d334efda-e650-414f-8be8-c57f76959a6c"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=208" },
                    { new Guid("a82759a7-f445-421f-a16d-6f92ba5c2ffb"), new DateOnly(2024, 5, 16), new byte[] { 76, 251, 74, 62, 153, 188, 10, 154, 132, 13, 103, 9, 78, 60, 110, 119, 138, 66, 141, 86, 25, 172, 187, 77, 136, 4, 214, 13, 55, 73, 1, 169, 202, 109, 193, 0, 162, 86, 101, 48, 207, 254, 142, 149, 129, 101, 218, 222, 130, 226, 242, 112, 183, 77, 77, 101, 225, 74, 175, 117, 22, 154, 101, 251, 250, 216, 56, 44, 55, 182, 200, 237, 128, 150, 109, 187, 100, 122, 231, 0, 100, 149, 65, 254, 139, 223, 61, 233, 5, 161, 2, 220, 186, 94, 160, 109, 130, 169, 231, 51 }, new Guid("9f734442-ecc3-47f4-a74a-e591bb4793fb"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=706" },
                    { new Guid("ab179685-c3e3-44f8-bca3-20bf17503e40"), new DateOnly(2024, 5, 16), new byte[] { 43, 170, 16, 126, 115, 0, 201, 133, 170, 5, 44, 217, 151, 99, 142, 13, 246, 130, 76, 90, 175, 29, 221, 144, 5, 148, 56, 169, 205, 21, 131, 21, 139, 20, 171, 213, 208, 31, 71, 214, 118, 22, 186, 191, 12, 117, 127, 79, 218, 30, 142, 155, 75, 228, 81, 83, 108, 208, 157, 230, 180, 173, 151, 170, 84, 252, 122, 40, 238, 56, 146, 156, 129, 48, 40, 59, 30, 108, 14, 37, 22, 141, 203, 20, 27, 250, 51, 247, 38, 92, 50, 34, 28, 189, 73, 216, 118, 45, 180, 232 }, new Guid("d334efda-e650-414f-8be8-c57f76959a6c"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=835" },
                    { new Guid("b4ef8109-c9b5-40e6-99ee-bbdc32a7f435"), new DateOnly(2024, 5, 16), new byte[] { 111, 73, 46, 219, 79, 128, 186, 93, 181, 59, 35, 73, 5, 1, 43, 47, 113, 142, 244, 103, 206, 25, 27, 23, 169, 54, 6, 254, 160, 101, 242, 158, 251, 8, 129, 186, 184, 84, 102, 206, 188, 146, 191, 178, 95, 67, 48, 237, 224, 88, 39, 225, 39, 245, 176, 43, 37, 196, 48, 158, 123, 32, 186, 244, 143, 191, 96, 213, 92, 56, 91, 126, 1, 36, 17, 122, 37, 105, 23, 37, 231, 72, 11, 62, 3, 181, 158, 7, 148, 46, 52, 213, 178, 56, 167, 135, 139, 249, 181, 60 }, new Guid("82d0e0cb-1aaf-48cc-b545-aafd488a5eec"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=446" },
                    { new Guid("c14cfd9d-796b-4177-a9e9-5af8f1629bdc"), new DateOnly(2024, 5, 16), new byte[] { 227, 52, 30, 161, 33, 85, 61, 237, 209, 32, 150, 67, 163, 85, 46, 77, 247, 56, 91, 138, 134, 118, 187, 245, 149, 88, 232, 90, 107, 201, 166, 25, 88, 6, 54, 138, 73, 154, 208, 120, 68, 134, 96, 133, 143, 2, 178, 8, 86, 223, 65, 251, 97, 102, 7, 27, 19, 80, 247, 242, 39, 20, 206, 122, 142, 11, 242, 87, 105, 66, 25, 107, 165, 61, 221, 9, 204, 142, 0, 84, 99, 200, 83, 50, 40, 142, 3, 152, 139, 224, 130, 69, 46, 246, 145, 44, 43, 55, 151, 186 }, new Guid("9c0295d0-9d42-4008-9806-71e5abd47d0e"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=907" },
                    { new Guid("c34b6d1a-dd83-4b83-9e6d-5067dcb57f58"), new DateOnly(2024, 5, 16), new byte[] { 197, 250, 44, 73, 46, 169, 26, 230, 68, 0, 74, 12, 117, 41, 87, 235, 194, 184, 227, 92, 131, 195, 80, 128, 69, 205, 208, 92, 10, 88, 87, 35, 186, 213, 66, 188, 188, 233, 71, 230, 104, 158, 255, 182, 20, 240, 97, 16, 131, 154, 82, 137, 95, 167, 34, 109, 18, 65, 38, 248, 186, 48, 29, 2, 207, 13, 201, 107, 239, 192, 248, 89, 91, 181, 18, 14, 182, 112, 161, 76, 244, 238, 199, 37, 30, 94, 31, 225, 131, 70, 103, 243, 72, 14, 138, 92, 133, 250, 78, 217 }, new Guid("a41a4f1e-983a-479a-9559-241f52bc9185"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=204" },
                    { new Guid("cb9fdbcd-bdff-4aba-9cb0-5f820ca3d36e"), new DateOnly(2024, 5, 16), new byte[] { 166, 214, 22, 113, 168, 208, 129, 11, 212, 181, 227, 142, 197, 244, 242, 212, 63, 230, 185, 168, 33, 195, 7, 209, 255, 116, 223, 71, 226, 153, 196, 182, 202, 51, 55, 198, 108, 57, 48, 63, 153, 56, 114, 94, 155, 250, 129, 166, 101, 164, 196, 111, 182, 16, 89, 219, 163, 170, 88, 228, 53, 94, 133, 92, 89, 25, 174, 227, 45, 147, 147, 213, 175, 222, 120, 54, 164, 53, 102, 34, 72, 140, 123, 96, 103, 175, 215, 139, 253, 164, 108, 24, 186, 40, 84, 43, 99, 247, 113, 65 }, new Guid("9c0295d0-9d42-4008-9806-71e5abd47d0e"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=843" },
                    { new Guid("e0fd7305-b90a-43ef-97c3-0e3d73d124a3"), new DateOnly(2024, 5, 16), new byte[] { 158, 142, 16, 215, 84, 159, 81, 80, 107, 102, 134, 230, 131, 252, 82, 204, 226, 156, 118, 28, 204, 20, 164, 130, 101, 216, 65, 205, 127, 187, 80, 24, 153, 45, 106, 6, 127, 153, 153, 90, 234, 101, 212, 41, 31, 47, 255, 182, 119, 165, 119, 35, 54, 103, 233, 222, 100, 16, 254, 172, 225, 25, 168, 6, 180, 196, 7, 13, 76, 27, 2, 240, 252, 190, 52, 167, 102, 184, 233, 16, 162, 110, 45, 204, 60, 120, 7, 174, 253, 207, 93, 85, 183, 73, 66, 153, 95, 36, 56, 132 }, new Guid("82d0e0cb-1aaf-48cc-b545-aafd488a5eec"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=933" },
                    { new Guid("e11414f4-95fe-4431-b53c-e9c6267db620"), new DateOnly(2024, 5, 16), new byte[] { 173, 67, 86, 63, 154, 16, 3, 71, 150, 201, 151, 14, 152, 164, 35, 132, 20, 61, 238, 212, 97, 117, 25, 70, 156, 72, 231, 83, 31, 36, 96, 150, 224, 74, 205, 206, 130, 68, 219, 74, 62, 158, 233, 59, 165, 118, 108, 203, 113, 206, 246, 152, 40, 252, 186, 122, 247, 91, 115, 220, 250, 212, 211, 52, 50, 11, 122, 254, 132, 242, 48, 128, 148, 10, 174, 135, 204, 140, 95, 26, 119, 228, 36, 67, 202, 162, 5, 181, 248, 206, 62, 95, 160, 52, 151, 164, 221, 106, 196, 56 }, new Guid("d334efda-e650-414f-8be8-c57f76959a6c"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=302" },
                    { new Guid("e612c171-f8b6-442f-ab8c-400915754333"), new DateOnly(2024, 5, 16), new byte[] { 84, 150, 10, 165, 202, 101, 17, 62, 128, 74, 112, 155, 13, 74, 78, 108, 237, 157, 21, 73, 200, 117, 95, 170, 201, 130, 160, 29, 46, 40, 203, 59, 52, 217, 93, 255, 39, 37, 144, 126, 20, 114, 168, 56, 75, 101, 88, 64, 187, 246, 0, 75, 213, 178, 12, 84, 251, 155, 25, 254, 17, 73, 202, 243, 106, 121, 20, 117, 78, 237, 123, 26, 236, 184, 230, 16, 173, 188, 160, 60, 23, 130, 27, 180, 81, 47, 126, 35, 240, 26, 131, 140, 227, 233, 240, 82, 150, 224, 43, 217 }, new Guid("e2818a20-d14c-42f9-9eea-9982aaebc091"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=136" },
                    { new Guid("f04e4e24-7044-4e4b-a00a-391d3a096603"), new DateOnly(2024, 5, 16), new byte[] { 174, 19, 54, 13, 225, 252, 99, 223, 112, 234, 16, 211, 83, 101, 107, 67, 101, 193, 215, 19, 29, 197, 16, 80, 240, 202, 161, 112, 202, 143, 16, 131, 229, 180, 84, 186, 219, 159, 85, 117, 122, 58, 12, 46, 213, 26, 23, 197, 138, 116, 67, 205, 23, 241, 236, 121, 249, 219, 60, 107, 152, 97, 206, 165, 216, 105, 233, 71, 250, 136, 121, 12, 92, 184, 252, 176, 143, 130, 181, 90, 170, 102, 112, 247, 168, 196, 240, 119, 28, 40, 4, 63, 218, 221, 165, 254, 144, 249, 213, 222 }, new Guid("82d0e0cb-1aaf-48cc-b545-aafd488a5eec"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=490" }
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
