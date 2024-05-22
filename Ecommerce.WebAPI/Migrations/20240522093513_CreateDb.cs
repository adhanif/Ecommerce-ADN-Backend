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
                    avatar = table.Column<string>(type: "varchar(1024)", nullable: true),
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
                    { new Guid("00630a56-ea65-47d4-b62c-96cb2e56256f"), new DateOnly(2024, 5, 22), "https://picsum.photos/200/?random=1", "Books", new DateOnly(2024, 5, 22) },
                    { new Guid("14cb513a-83bc-4ab4-9fbb-799c1e32a98a"), new DateOnly(2024, 5, 22), "https://picsum.photos/200/?random=1", "Clothing", new DateOnly(2024, 5, 22) },
                    { new Guid("207df818-b99b-43e1-886d-77f9b901bd8e"), new DateOnly(2024, 5, 22), "https://picsum.photos/200/?random=1", "Furniture", new DateOnly(2024, 5, 22) },
                    { new Guid("28af782d-e008-42f7-bb0a-06125cdb158c"), new DateOnly(2024, 5, 22), "https://picsum.photos/200/?random=5", "Sports", new DateOnly(2024, 5, 22) },
                    { new Guid("537e7a09-a043-4f81-8c48-17172f873dfb"), new DateOnly(2024, 5, 22), "https://picsum.photos/200/?random=10", "Electronic", new DateOnly(2024, 5, 22) },
                    { new Guid("a1170a25-9932-41cc-9057-d5f88e50a9a4"), new DateOnly(2024, 5, 22), "https://picsum.photos/200/?random=9", "Toys", new DateOnly(2024, 5, 22) }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "avatar", "created_date", "email", "name", "password", "role", "salt", "updated_date" },
                values: new object[,]
                {
                    { new Guid("2b38242d-36ef-4dcf-82f5-7b0c39264dc0"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", new DateOnly(2024, 5, 22), "adnan@admin.com", "Adnan", "Y3UoEt+7u0DuktdTqyALiA18MdRcHZB0DmA/gv6ouIc=", UserRole.Admin, new byte[] { 69, 14, 29, 52, 19, 108, 32, 154, 51, 219, 73, 102, 119, 36, 225, 221 }, new DateOnly(2024, 5, 22) },
                    { new Guid("35cf05a9-3227-4528-a876-c45d955d5634"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", new DateOnly(2024, 5, 22), "binh@admin.com", "Binh", "AG293PTJthvXzc4rmb7lnJJNG7sInMzR83xA20scMmg=", UserRole.Admin, new byte[] { 60, 38, 235, 203, 132, 12, 91, 159, 195, 205, 153, 110, 58, 106, 135, 68 }, new DateOnly(2024, 5, 22) },
                    { new Guid("59dd90b8-5428-476e-94a9-29f0f745e33f"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", new DateOnly(2024, 5, 22), "yuanke@admin.com", "Yuanke", "rSFjbSgjTm2KYKoqudDFLmigiuFaH1rCDUU5fO6m/3E=", UserRole.Admin, new byte[] { 189, 242, 155, 230, 58, 162, 185, 63, 244, 154, 22, 84, 71, 168, 119, 129 }, new DateOnly(2024, 5, 22) },
                    { new Guid("cb1a50d4-d0f0-4a2f-8951-0aa69d66dd5d"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", new DateOnly(2024, 5, 22), "john@example.com", "Admin1", "2appOmpbt7554hpc1sNaNwo1445jLtqYGmQZ+Ln2PIg=", UserRole.Admin, new byte[] { 59, 55, 155, 84, 107, 203, 210, 251, 254, 253, 61, 52, 101, 65, 79, 207 }, new DateOnly(2024, 5, 22) },
                    { new Guid("dfee49d3-5f50-4f41-812f-4a622cfa9b46"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", new DateOnly(2024, 5, 22), "customer1@customer.com", "Customer1", "hfsqiMS+NuO/xt8H03WDXlYYJcGskMyM710pYQCZ8JY=", UserRole.Customer, new byte[] { 110, 201, 208, 129, 58, 142, 198, 134, 114, 244, 179, 64, 86, 248, 142, 176 }, new DateOnly(2024, 5, 22) }
                });

            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "id", "category_id", "created_date", "description", "inventory", "price", "title", "updated_date" },
                values: new object[,]
                {
                    { new Guid("4d9249f2-598d-4b95-bc9c-30ae234c556a"), new Guid("207df818-b99b-43e1-886d-77f9b901bd8e"), new DateOnly(2024, 5, 22), "Description for Furniture Product 1", 100, 1000, "Furniture Product 1", new DateOnly(2024, 5, 22) },
                    { new Guid("960cc0ff-091c-4d34-a58d-a6ef7d422ca5"), new Guid("14cb513a-83bc-4ab4-9fbb-799c1e32a98a"), new DateOnly(2024, 5, 22), "Description for Clothing Product 1", 100, 300, "Clothing Product 1", new DateOnly(2024, 5, 22) },
                    { new Guid("db4a3b9f-6aaa-44cd-a8c2-b4e99fb20874"), new Guid("537e7a09-a043-4f81-8c48-17172f873dfb"), new DateOnly(2024, 5, 22), "Description for Electronic Product 1", 100, 400, "Electronic Product 1", new DateOnly(2024, 5, 22) },
                    { new Guid("f0d51461-1de3-45d9-8be1-f6f29a5061a9"), new Guid("00630a56-ea65-47d4-b62c-96cb2e56256f"), new DateOnly(2024, 5, 22), "Description for Books Product 1", 100, 100, "Books Product 1", new DateOnly(2024, 5, 22) }
                });

            migrationBuilder.InsertData(
                table: "images",
                columns: new[] { "id", "created_date", "data", "product_id", "updated_date" },
                values: new object[,]
                {
                    { new Guid("1ab12ec9-f64b-4098-be36-e6e571c71270"), new DateOnly(2024, 5, 22), new byte[] { 159, 109, 103, 84, 101, 167, 99, 222, 51, 187, 60, 0, 149, 251, 36, 181, 91, 2, 204, 49, 169, 63, 226, 1, 46, 197, 136, 164, 7, 176, 89, 111, 119, 201, 31, 208, 230, 24, 94, 169, 175, 81, 59, 143, 244, 135, 81, 44, 239, 193, 209, 145, 31, 152, 103, 56, 84, 36, 163, 250, 212, 162, 129, 90, 95, 115, 251, 112, 95, 10, 41, 154, 195, 175, 111, 126, 6, 201, 226, 65, 101, 209, 33, 159, 246, 86, 119, 71, 70, 187, 102, 173, 50, 179, 241, 14, 233, 172, 126, 164 }, new Guid("db4a3b9f-6aaa-44cd-a8c2-b4e99fb20874"), new DateOnly(2024, 5, 22) },
                    { new Guid("1bdd0e1d-9f33-4b20-b96f-30453c5af1bd"), new DateOnly(2024, 5, 22), new byte[] { 220, 34, 229, 123, 11, 145, 192, 76, 95, 213, 239, 237, 156, 230, 43, 42, 211, 37, 205, 181, 88, 175, 184, 53, 182, 123, 78, 248, 101, 101, 14, 69, 93, 140, 23, 59, 92, 223, 111, 50, 85, 115, 27, 244, 243, 197, 58, 40, 208, 85, 188, 170, 28, 24, 231, 94, 220, 222, 238, 172, 9, 154, 107, 219, 50, 250, 165, 72, 12, 92, 124, 175, 104, 10, 141, 160, 226, 185, 127, 53, 93, 151, 75, 241, 70, 204, 89, 72, 190, 218, 142, 81, 202, 179, 122, 144, 39, 174, 213, 145 }, new Guid("f0d51461-1de3-45d9-8be1-f6f29a5061a9"), new DateOnly(2024, 5, 22) },
                    { new Guid("4524f6af-2f30-4e84-a032-904eeccc8ffd"), new DateOnly(2024, 5, 22), new byte[] { 81, 94, 237, 40, 185, 211, 66, 53, 174, 183, 18, 117, 173, 124, 155, 124, 174, 132, 242, 37, 201, 158, 3, 27, 20, 199, 246, 107, 13, 19, 38, 141, 102, 107, 150, 235, 213, 47, 9, 16, 253, 24, 203, 177, 78, 157, 167, 182, 125, 70, 102, 138, 67, 180, 15, 176, 18, 84, 243, 177, 48, 32, 205, 161, 36, 3, 98, 134, 255, 64, 184, 108, 153, 212, 167, 97, 2, 219, 185, 16, 43, 50, 162, 129, 248, 75, 50, 97, 78, 22, 127, 155, 24, 205, 17, 90, 50, 22, 190, 203 }, new Guid("f0d51461-1de3-45d9-8be1-f6f29a5061a9"), new DateOnly(2024, 5, 22) },
                    { new Guid("466e674e-e64c-46f2-9133-0ac704147e27"), new DateOnly(2024, 5, 22), new byte[] { 143, 229, 48, 210, 203, 203, 47, 64, 190, 89, 141, 34, 135, 18, 26, 103, 6, 11, 128, 74, 116, 142, 56, 139, 106, 143, 213, 51, 200, 99, 7, 95, 139, 152, 208, 97, 139, 199, 128, 183, 27, 167, 114, 179, 228, 16, 125, 54, 223, 134, 196, 197, 21, 133, 45, 7, 23, 132, 230, 26, 127, 226, 155, 8, 29, 235, 221, 225, 250, 43, 170, 168, 251, 246, 58, 200, 121, 214, 111, 130, 84, 21, 212, 122, 218, 76, 207, 97, 74, 48, 108, 137, 202, 158, 36, 247, 227, 199, 118, 61 }, new Guid("db4a3b9f-6aaa-44cd-a8c2-b4e99fb20874"), new DateOnly(2024, 5, 22) },
                    { new Guid("78fc23dd-e5dd-45d4-9039-5f817c7353a3"), new DateOnly(2024, 5, 22), new byte[] { 28, 104, 174, 31, 210, 151, 218, 153, 123, 159, 195, 6, 104, 128, 243, 31, 217, 36, 7, 49, 215, 241, 246, 53, 42, 15, 243, 208, 131, 166, 202, 131, 122, 54, 160, 15, 150, 83, 115, 255, 152, 197, 173, 68, 85, 82, 115, 254, 102, 7, 77, 34, 11, 84, 229, 170, 224, 31, 75, 183, 95, 113, 34, 204, 114, 185, 76, 51, 40, 236, 83, 50, 66, 198, 117, 202, 220, 140, 84, 91, 234, 158, 219, 69, 251, 101, 211, 88, 147, 95, 255, 134, 201, 225, 191, 88, 120, 51, 130, 226 }, new Guid("4d9249f2-598d-4b95-bc9c-30ae234c556a"), new DateOnly(2024, 5, 22) },
                    { new Guid("7c1d613f-d107-4e0e-af05-be3a3ba622f8"), new DateOnly(2024, 5, 22), new byte[] { 113, 35, 91, 151, 2, 70, 83, 229, 55, 34, 140, 55, 126, 178, 155, 116, 204, 11, 159, 250, 48, 161, 237, 143, 96, 184, 133, 181, 83, 232, 42, 74, 9, 231, 180, 241, 153, 169, 238, 29, 80, 200, 85, 248, 96, 211, 56, 248, 238, 27, 117, 30, 186, 216, 17, 76, 246, 96, 125, 191, 164, 187, 82, 10, 211, 127, 111, 15, 222, 58, 175, 201, 223, 186, 16, 241, 5, 242, 122, 161, 155, 68, 223, 163, 234, 94, 167, 57, 45, 130, 53, 194, 115, 171, 69, 224, 106, 244, 189, 237 }, new Guid("4d9249f2-598d-4b95-bc9c-30ae234c556a"), new DateOnly(2024, 5, 22) },
                    { new Guid("80d7b778-a7d4-4eb3-8205-354f8cfde792"), new DateOnly(2024, 5, 22), new byte[] { 164, 226, 128, 72, 180, 89, 1, 178, 205, 246, 218, 104, 143, 5, 231, 20, 124, 249, 47, 230, 246, 248, 80, 45, 138, 210, 247, 192, 34, 145, 57, 153, 83, 226, 223, 181, 60, 147, 86, 182, 243, 119, 156, 210, 216, 247, 245, 107, 81, 55, 138, 157, 174, 155, 62, 219, 251, 15, 58, 180, 157, 209, 129, 98, 247, 91, 193, 83, 131, 73, 73, 188, 238, 69, 234, 2, 177, 141, 65, 48, 128, 58, 34, 193, 71, 240, 252, 178, 68, 97, 48, 138, 162, 176, 60, 62, 163, 0, 65, 122 }, new Guid("960cc0ff-091c-4d34-a58d-a6ef7d422ca5"), new DateOnly(2024, 5, 22) },
                    { new Guid("95930f13-76dc-4ab9-9218-da60b02dd9b4"), new DateOnly(2024, 5, 22), new byte[] { 97, 95, 121, 84, 244, 209, 192, 26, 102, 182, 200, 50, 134, 155, 55, 250, 137, 119, 131, 166, 145, 2, 190, 33, 1, 133, 67, 69, 237, 44, 253, 5, 228, 111, 79, 48, 221, 69, 90, 235, 186, 243, 224, 122, 164, 248, 239, 124, 246, 118, 99, 90, 1, 208, 214, 135, 133, 211, 175, 48, 87, 89, 231, 79, 245, 158, 210, 75, 197, 84, 131, 57, 45, 201, 187, 36, 235, 233, 98, 180, 159, 110, 9, 212, 181, 180, 44, 243, 241, 174, 74, 25, 137, 12, 38, 168, 224, 109, 172, 34 }, new Guid("960cc0ff-091c-4d34-a58d-a6ef7d422ca5"), new DateOnly(2024, 5, 22) },
                    { new Guid("a685adf0-f193-4cd9-a380-0c922acf97d0"), new DateOnly(2024, 5, 22), new byte[] { 54, 159, 90, 113, 139, 252, 215, 3, 68, 232, 214, 174, 200, 90, 33, 225, 150, 107, 199, 40, 21, 183, 66, 167, 234, 122, 232, 229, 213, 55, 246, 229, 47, 174, 253, 174, 223, 27, 22, 243, 12, 30, 234, 57, 202, 173, 226, 138, 95, 127, 165, 127, 5, 206, 54, 64, 190, 170, 71, 97, 241, 131, 231, 66, 93, 215, 54, 77, 47, 70, 192, 169, 94, 112, 174, 195, 110, 23, 27, 220, 185, 81, 104, 179, 61, 231, 76, 214, 59, 117, 250, 31, 78, 31, 0, 46, 18, 145, 227, 254 }, new Guid("960cc0ff-091c-4d34-a58d-a6ef7d422ca5"), new DateOnly(2024, 5, 22) },
                    { new Guid("b7680c81-44fc-4109-b7aa-2cde183bfc6d"), new DateOnly(2024, 5, 22), new byte[] { 97, 37, 251, 117, 134, 117, 212, 39, 104, 109, 46, 213, 157, 246, 108, 174, 215, 192, 94, 189, 35, 41, 162, 242, 253, 221, 57, 91, 25, 160, 174, 175, 108, 110, 89, 110, 246, 151, 232, 90, 243, 183, 154, 75, 152, 185, 102, 7, 165, 155, 122, 56, 251, 84, 111, 154, 181, 94, 249, 31, 2, 126, 82, 14, 209, 45, 159, 105, 179, 216, 166, 168, 101, 94, 182, 86, 231, 62, 149, 199, 184, 57, 127, 30, 120, 164, 255, 182, 187, 78, 111, 248, 2, 189, 176, 148, 182, 179, 148, 41 }, new Guid("4d9249f2-598d-4b95-bc9c-30ae234c556a"), new DateOnly(2024, 5, 22) },
                    { new Guid("df7ab8bb-05b0-4122-bf46-e08c67c4494c"), new DateOnly(2024, 5, 22), new byte[] { 207, 15, 84, 172, 141, 28, 186, 58, 185, 190, 83, 61, 13, 36, 161, 187, 7, 87, 137, 204, 110, 22, 30, 57, 84, 84, 10, 194, 57, 185, 99, 224, 103, 181, 218, 11, 111, 9, 95, 82, 74, 48, 149, 221, 219, 28, 148, 162, 137, 38, 120, 159, 129, 193, 13, 238, 191, 247, 206, 209, 2, 93, 252, 7, 208, 70, 171, 32, 219, 111, 181, 189, 2, 68, 66, 55, 16, 211, 160, 68, 84, 68, 153, 165, 225, 209, 152, 134, 158, 110, 0, 154, 134, 50, 69, 119, 9, 210, 113, 77 }, new Guid("f0d51461-1de3-45d9-8be1-f6f29a5061a9"), new DateOnly(2024, 5, 22) },
                    { new Guid("f4bc387a-4b4b-43f1-8bb4-2f7da5af78a3"), new DateOnly(2024, 5, 22), new byte[] { 29, 111, 94, 52, 179, 81, 183, 108, 63, 6, 198, 26, 181, 220, 28, 230, 168, 127, 59, 61, 114, 54, 190, 52, 34, 181, 243, 167, 172, 85, 171, 174, 196, 30, 92, 137, 110, 108, 151, 111, 214, 213, 72, 241, 109, 140, 128, 244, 0, 252, 200, 147, 252, 173, 75, 160, 193, 80, 113, 65, 173, 84, 54, 98, 90, 217, 218, 187, 15, 154, 34, 101, 107, 116, 87, 88, 250, 174, 1, 81, 103, 95, 112, 187, 39, 64, 96, 128, 41, 86, 86, 26, 98, 112, 38, 35, 250, 244, 61, 158 }, new Guid("db4a3b9f-6aaa-44cd-a8c2-b4e99fb20874"), new DateOnly(2024, 5, 22) }
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
