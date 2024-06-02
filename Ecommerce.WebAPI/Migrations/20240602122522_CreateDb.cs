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
                    { new Guid("167c2dfb-4437-4bd6-835d-c585cfeb2bd9"), new DateOnly(2024, 6, 2), "https://picsum.photos/200/?random=4", "Electronic", new DateOnly(2024, 6, 2) },
                    { new Guid("1b0e93d9-7393-42be-b269-aee91220134c"), new DateOnly(2024, 6, 2), "https://picsum.photos/200/?random=9", "Furniture", new DateOnly(2024, 6, 2) },
                    { new Guid("24635f81-9ad8-440c-9222-89cb4f587f36"), new DateOnly(2024, 6, 2), "https://picsum.photos/200/?random=3", "Sports", new DateOnly(2024, 6, 2) },
                    { new Guid("70326099-ba93-464e-b2da-d844e55e3048"), new DateOnly(2024, 6, 2), "https://picsum.photos/200/?random=5", "Books", new DateOnly(2024, 6, 2) },
                    { new Guid("790e367c-d46b-4d35-b990-2b2515919fc5"), new DateOnly(2024, 6, 2), "https://picsum.photos/200/?random=1", "Clothing", new DateOnly(2024, 6, 2) },
                    { new Guid("ad1c229a-5b4a-451a-b921-f24d24c4ce8e"), new DateOnly(2024, 6, 2), "https://picsum.photos/200/?random=8", "Toys", new DateOnly(2024, 6, 2) }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "avatar", "created_date", "email", "name", "password", "role", "salt", "updated_date" },
                values: new object[,]
                {
                    { new Guid("1eaf5f98-8b7f-45b6-b3e0-4f9e9dfe0d2a"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", new DateOnly(2024, 6, 2), "adnan@admin.com", "Adnan", "aTOP+hAkD0/Z2mYp1QCfcq+Npbn61orRNFjICr+0X5o=", UserRole.Admin, new byte[] { 50, 13, 241, 243, 68, 133, 4, 67, 4, 29, 255, 81, 77, 226, 156, 22 }, new DateOnly(2024, 6, 2) },
                    { new Guid("35f470a5-4c64-477b-b62c-28aa4207977d"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", new DateOnly(2024, 6, 2), "binh@admin.com", "Binh", "C7/E/ngNfDBALu4EHI2EpXNOTfn2nA1D01F5wCqEabU=", UserRole.Admin, new byte[] { 55, 213, 188, 127, 9, 49, 107, 82, 131, 8, 75, 180, 112, 58, 73, 121 }, new DateOnly(2024, 6, 2) },
                    { new Guid("72369aa7-6f65-4520-bdc0-fe091af6b0e6"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", new DateOnly(2024, 6, 2), "customer1@customer.com", "Customer1", "SYBO6WtjJ8oREBdxTkCzXBe2wLx/Y0fzNceQxhp3c4M=", UserRole.Customer, new byte[] { 168, 83, 85, 44, 210, 26, 193, 27, 176, 29, 122, 76, 200, 208, 65, 131 }, new DateOnly(2024, 6, 2) },
                    { new Guid("a50fd241-ddb6-4448-a18e-7348ea9de55b"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", new DateOnly(2024, 6, 2), "yuanke@admin.com", "Yuanke", "xX9Ti1Z9yiDXKG8TpfMQND5Gft8jGlivpCH7i8tPsLI=", UserRole.Admin, new byte[] { 253, 105, 195, 244, 162, 47, 144, 34, 47, 121, 156, 80, 132, 160, 11, 94 }, new DateOnly(2024, 6, 2) },
                    { new Guid("da0389a8-1ab2-4c3d-89ab-5a81aaa8afe8"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", new DateOnly(2024, 6, 2), "john@example.com", "Admin1", "NFb2GSH6LDbiN0eCVTp5lTtZD2xyyAc0SJFF+OP/1/I=", UserRole.Admin, new byte[] { 139, 197, 38, 246, 105, 44, 187, 96, 211, 51, 172, 156, 62, 253, 225, 17 }, new DateOnly(2024, 6, 2) }
                });

            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "id", "category_id", "created_date", "description", "inventory", "price", "title", "updated_date" },
                values: new object[,]
                {
                    { new Guid("665b2ab2-9a0d-44ce-bde4-5c6ee804bb18"), new Guid("70326099-ba93-464e-b2da-d844e55e3048"), new DateOnly(2024, 6, 2), "Description for Books Product 1", 100, 400, "Books Product 1", new DateOnly(2024, 6, 2) },
                    { new Guid("7516a148-f681-4d3f-a851-717536f0591a"), new Guid("167c2dfb-4437-4bd6-835d-c585cfeb2bd9"), new DateOnly(2024, 6, 2), "Description for Electronic Product 1", 100, 700, "Electronic Product 1", new DateOnly(2024, 6, 2) },
                    { new Guid("cf2f8e22-eeb0-4743-beee-9c2dfde39f11"), new Guid("790e367c-d46b-4d35-b990-2b2515919fc5"), new DateOnly(2024, 6, 2), "Description for Clothing Product 1", 100, 700, "Clothing Product 1", new DateOnly(2024, 6, 2) },
                    { new Guid("d140ddb7-0f4b-49c6-ad3b-97cb8e473aeb"), new Guid("1b0e93d9-7393-42be-b269-aee91220134c"), new DateOnly(2024, 6, 2), "Description for Furniture Product 1", 100, 600, "Furniture Product 1", new DateOnly(2024, 6, 2) }
                });

            migrationBuilder.InsertData(
                table: "images",
                columns: new[] { "id", "created_date", "data", "product_id", "updated_date" },
                values: new object[,]
                {
                    { new Guid("02c52d03-5fec-4e97-957a-640a8ef8eeb1"), new DateOnly(2024, 6, 2), new byte[] { 117, 242, 231, 229, 102, 103, 219, 6, 224, 172, 107, 255, 142, 119, 248, 104, 51, 204, 167, 152, 42, 126, 74, 149, 15, 129, 53, 147, 101, 108, 95, 248, 108, 209, 103, 174, 168, 20, 228, 64, 0, 98, 162, 226, 98, 129, 204, 234, 41, 109, 21, 59, 17, 197, 40, 131, 120, 148, 241, 252, 70, 88, 164, 25, 198, 217, 88, 166, 82, 85, 16, 58, 49, 106, 14, 228, 68, 183, 195, 251, 61, 103, 112, 15, 170, 119, 31, 197, 183, 223, 29, 89, 149, 164, 175, 43, 68, 233, 223, 240 }, new Guid("d140ddb7-0f4b-49c6-ad3b-97cb8e473aeb"), new DateOnly(2024, 6, 2) },
                    { new Guid("05ab684a-f3ed-42a1-955a-b6569fc14263"), new DateOnly(2024, 6, 2), new byte[] { 43, 199, 103, 214, 54, 108, 145, 185, 210, 26, 222, 240, 76, 167, 194, 1, 112, 217, 74, 253, 170, 207, 219, 71, 182, 41, 247, 124, 2, 57, 15, 174, 221, 180, 53, 122, 201, 27, 20, 187, 148, 196, 37, 191, 157, 109, 234, 233, 23, 195, 147, 20, 135, 211, 0, 188, 203, 75, 95, 37, 239, 60, 116, 152, 39, 43, 139, 205, 85, 191, 21, 119, 123, 68, 89, 107, 103, 93, 118, 41, 84, 175, 186, 222, 77, 0, 18, 222, 138, 36, 12, 62, 155, 202, 128, 43, 252, 245, 81, 148 }, new Guid("665b2ab2-9a0d-44ce-bde4-5c6ee804bb18"), new DateOnly(2024, 6, 2) },
                    { new Guid("08c6c250-c0f3-462b-bba2-15c14abbb2e6"), new DateOnly(2024, 6, 2), new byte[] { 169, 141, 183, 18, 52, 69, 242, 11, 26, 45, 117, 7, 59, 202, 140, 240, 17, 193, 71, 206, 144, 208, 177, 122, 11, 128, 232, 201, 4, 188, 70, 41, 143, 235, 91, 205, 158, 139, 71, 217, 83, 168, 148, 131, 27, 86, 230, 229, 225, 72, 66, 58, 38, 186, 253, 140, 162, 8, 144, 199, 218, 19, 54, 211, 44, 176, 195, 135, 68, 21, 238, 42, 132, 229, 84, 197, 21, 98, 151, 187, 228, 84, 17, 228, 201, 254, 106, 147, 0, 8, 5, 247, 188, 208, 27, 74, 188, 56, 76, 177 }, new Guid("d140ddb7-0f4b-49c6-ad3b-97cb8e473aeb"), new DateOnly(2024, 6, 2) },
                    { new Guid("28577d37-e262-4d3c-9c46-7cc459a92df5"), new DateOnly(2024, 6, 2), new byte[] { 17, 220, 244, 86, 228, 102, 29, 141, 101, 114, 231, 198, 222, 103, 29, 228, 0, 191, 150, 215, 160, 186, 170, 76, 135, 204, 138, 223, 219, 234, 75, 151, 223, 225, 58, 39, 62, 216, 232, 6, 253, 212, 83, 130, 130, 3, 152, 185, 92, 224, 159, 168, 176, 153, 88, 57, 38, 86, 155, 98, 200, 228, 40, 63, 150, 2, 69, 145, 157, 233, 204, 136, 121, 185, 173, 108, 108, 130, 99, 223, 201, 219, 148, 90, 216, 194, 189, 0, 131, 24, 249, 154, 211, 98, 137, 19, 169, 46, 25, 87 }, new Guid("cf2f8e22-eeb0-4743-beee-9c2dfde39f11"), new DateOnly(2024, 6, 2) },
                    { new Guid("394d8531-e84e-4c9f-b1e6-10e08875d710"), new DateOnly(2024, 6, 2), new byte[] { 179, 160, 19, 18, 11, 153, 144, 40, 13, 127, 116, 78, 36, 130, 253, 13, 81, 169, 202, 236, 0, 201, 218, 181, 137, 77, 82, 102, 121, 205, 2, 139, 161, 190, 169, 64, 110, 250, 169, 236, 184, 111, 232, 141, 175, 185, 240, 116, 154, 97, 72, 193, 180, 200, 117, 185, 185, 218, 128, 129, 226, 235, 247, 11, 40, 7, 206, 182, 44, 51, 157, 33, 51, 171, 97, 222, 253, 29, 147, 200, 216, 171, 65, 246, 42, 148, 124, 162, 71, 89, 210, 118, 221, 59, 20, 27, 133, 196, 83, 51 }, new Guid("cf2f8e22-eeb0-4743-beee-9c2dfde39f11"), new DateOnly(2024, 6, 2) },
                    { new Guid("3c7cad8b-e696-4405-a672-b40a62804b13"), new DateOnly(2024, 6, 2), new byte[] { 140, 8, 128, 107, 122, 150, 128, 177, 85, 202, 108, 67, 255, 142, 82, 39, 222, 50, 250, 114, 205, 224, 223, 176, 31, 172, 95, 40, 147, 130, 67, 80, 189, 249, 65, 133, 7, 162, 233, 199, 77, 35, 204, 56, 131, 3, 141, 183, 141, 71, 89, 192, 142, 103, 99, 0, 59, 160, 135, 121, 18, 98, 241, 146, 176, 24, 155, 109, 122, 95, 58, 99, 233, 165, 101, 103, 203, 151, 0, 130, 151, 130, 8, 83, 136, 175, 61, 238, 57, 215, 150, 143, 96, 189, 64, 4, 224, 10, 65, 254 }, new Guid("cf2f8e22-eeb0-4743-beee-9c2dfde39f11"), new DateOnly(2024, 6, 2) },
                    { new Guid("537799c6-cab2-448c-998c-a1ac10890dca"), new DateOnly(2024, 6, 2), new byte[] { 142, 184, 12, 141, 68, 136, 80, 10, 211, 150, 120, 236, 180, 91, 33, 251, 176, 157, 239, 96, 98, 22, 237, 143, 68, 57, 45, 21, 198, 33, 157, 80, 180, 95, 117, 14, 28, 199, 38, 71, 123, 93, 0, 198, 74, 42, 150, 154, 33, 167, 131, 29, 191, 116, 94, 83, 159, 172, 148, 136, 207, 73, 239, 153, 206, 69, 180, 44, 182, 202, 116, 208, 72, 89, 120, 52, 237, 37, 174, 33, 63, 30, 255, 97, 116, 96, 68, 90, 129, 250, 97, 201, 31, 205, 32, 89, 23, 216, 17, 65 }, new Guid("7516a148-f681-4d3f-a851-717536f0591a"), new DateOnly(2024, 6, 2) },
                    { new Guid("5ac12e7e-ae89-4754-9bc2-c33410f9bd56"), new DateOnly(2024, 6, 2), new byte[] { 9, 102, 81, 166, 97, 189, 67, 211, 151, 78, 10, 176, 133, 137, 180, 230, 30, 191, 76, 207, 228, 203, 247, 139, 209, 229, 33, 181, 112, 8, 190, 158, 193, 83, 104, 179, 93, 202, 154, 206, 224, 188, 173, 175, 106, 252, 172, 68, 119, 146, 92, 232, 245, 29, 108, 15, 206, 91, 7, 149, 55, 212, 127, 207, 155, 6, 161, 58, 96, 149, 111, 217, 154, 39, 200, 77, 126, 147, 246, 204, 239, 59, 208, 160, 144, 192, 222, 81, 178, 151, 47, 191, 236, 53, 63, 221, 156, 188, 100, 95 }, new Guid("7516a148-f681-4d3f-a851-717536f0591a"), new DateOnly(2024, 6, 2) },
                    { new Guid("6a423516-4b60-4d11-ba7c-afcc3960f863"), new DateOnly(2024, 6, 2), new byte[] { 234, 206, 98, 198, 25, 227, 23, 132, 144, 23, 68, 226, 40, 14, 138, 254, 96, 20, 223, 126, 82, 48, 21, 253, 122, 182, 31, 65, 178, 252, 25, 56, 149, 17, 9, 183, 111, 88, 28, 210, 161, 140, 216, 231, 143, 6, 52, 90, 76, 41, 56, 163, 37, 36, 150, 241, 201, 33, 16, 194, 169, 225, 182, 15, 0, 199, 151, 255, 192, 94, 43, 202, 84, 182, 174, 135, 130, 216, 35, 150, 117, 177, 81, 249, 79, 231, 59, 233, 193, 160, 117, 126, 252, 210, 194, 64, 92, 204, 96, 239 }, new Guid("665b2ab2-9a0d-44ce-bde4-5c6ee804bb18"), new DateOnly(2024, 6, 2) },
                    { new Guid("6e20ac84-d1c4-401c-a869-e8efec084a06"), new DateOnly(2024, 6, 2), new byte[] { 240, 106, 136, 163, 74, 70, 163, 115, 32, 27, 234, 250, 167, 205, 133, 101, 150, 155, 10, 168, 51, 13, 255, 11, 189, 221, 92, 163, 141, 160, 72, 57, 249, 101, 68, 51, 63, 7, 93, 173, 15, 167, 154, 252, 44, 202, 39, 204, 40, 68, 144, 231, 50, 251, 153, 48, 165, 159, 148, 162, 3, 182, 33, 191, 125, 40, 176, 204, 143, 172, 150, 31, 63, 41, 64, 133, 111, 12, 198, 119, 110, 66, 109, 198, 105, 54, 193, 118, 202, 5, 35, 27, 167, 26, 168, 208, 187, 208, 144, 207 }, new Guid("7516a148-f681-4d3f-a851-717536f0591a"), new DateOnly(2024, 6, 2) },
                    { new Guid("a0aba926-6c13-4d70-a4ed-a66be7c98e23"), new DateOnly(2024, 6, 2), new byte[] { 37, 238, 12, 48, 168, 168, 219, 49, 134, 99, 171, 107, 234, 244, 139, 6, 124, 53, 217, 190, 46, 240, 47, 0, 150, 81, 118, 70, 27, 11, 248, 191, 71, 65, 158, 95, 203, 185, 126, 11, 94, 228, 94, 158, 113, 78, 91, 0, 199, 157, 213, 55, 241, 106, 119, 169, 30, 234, 72, 230, 30, 25, 160, 233, 168, 111, 17, 93, 114, 194, 66, 150, 133, 20, 125, 59, 21, 182, 223, 15, 14, 127, 214, 20, 4, 228, 11, 75, 109, 230, 24, 219, 50, 203, 107, 151, 66, 19, 80, 67 }, new Guid("d140ddb7-0f4b-49c6-ad3b-97cb8e473aeb"), new DateOnly(2024, 6, 2) },
                    { new Guid("c8d54e8a-abae-44d9-ba8f-d038bc790f41"), new DateOnly(2024, 6, 2), new byte[] { 145, 1, 121, 182, 83, 95, 117, 94, 58, 28, 25, 34, 116, 191, 251, 5, 240, 169, 213, 113, 54, 113, 6, 34, 152, 38, 231, 238, 120, 4, 141, 152, 70, 21, 147, 95, 123, 46, 146, 223, 42, 138, 38, 12, 27, 46, 249, 241, 157, 168, 62, 222, 11, 157, 58, 62, 18, 156, 164, 135, 3, 179, 198, 118, 236, 100, 84, 186, 233, 63, 171, 210, 200, 242, 43, 212, 190, 163, 97, 253, 114, 149, 185, 182, 34, 30, 241, 208, 242, 51, 45, 71, 241, 212, 29, 82, 152, 4, 108, 116 }, new Guid("665b2ab2-9a0d-44ce-bde4-5c6ee804bb18"), new DateOnly(2024, 6, 2) }
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
