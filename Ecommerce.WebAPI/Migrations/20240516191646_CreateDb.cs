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
                    { new Guid("170da1f9-b1ef-48c5-9b9b-413a8fc8d01a"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=6", "Furniture", new DateOnly(2024, 5, 16) },
                    { new Guid("21aab617-f05c-45af-a243-00aeea95d739"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=4", "Clothing", new DateOnly(2024, 5, 16) },
                    { new Guid("2495b034-b4d8-412a-b068-56f849db44a6"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=7", "Toys", new DateOnly(2024, 5, 16) },
                    { new Guid("40246fbf-142a-438f-8c8a-7540dc44de81"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=4", "Books", new DateOnly(2024, 5, 16) },
                    { new Guid("975f1654-d810-4b3d-a24a-c74003eaa8f4"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=1", "Electronic", new DateOnly(2024, 5, 16) },
                    { new Guid("dba67a93-662b-428a-a151-63e63f78caec"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=2", "Sports", new DateOnly(2024, 5, 16) }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "avatar", "created_date", "email", "name", "password", "role", "salt", "updated_date" },
                values: new object[,]
                {
                    { new Guid("18aeecb1-20e9-4d24-99f4-0261b878de87"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", new DateOnly(2024, 5, 16), "binh@admin.com", "Binh", "d7bQ34nJAN0ZNmvH2yTW/pZ3JZ4ChHuosWTLOhgjWNk=", UserRole.Admin, new byte[] { 239, 83, 78, 173, 76, 32, 252, 145, 10, 101, 79, 98, 134, 123, 104, 77 }, new DateOnly(2024, 5, 16) },
                    { new Guid("4955acf2-54eb-41b6-9126-fd5e4d2885f4"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", new DateOnly(2024, 5, 16), "customer1@customer.com", "Customer1", "+9fhCxwcfaVYWdFt7oAvzhiYxgwR+1SIVveOZyuGUcQ=", UserRole.Customer, new byte[] { 5, 215, 142, 251, 120, 155, 77, 83, 22, 184, 155, 52, 160, 111, 46, 255 }, new DateOnly(2024, 5, 16) },
                    { new Guid("967694a8-fd5b-4e75-ac47-49067da422fe"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", new DateOnly(2024, 5, 16), "adnan@admin.com", "Adnan", "yy8+9ubFCs0LVbdVkGXvPIbCHPHjcX1vCYx72AYY+xE=", UserRole.Admin, new byte[] { 51, 184, 225, 92, 179, 181, 131, 37, 211, 36, 37, 183, 137, 6, 122, 140 }, new DateOnly(2024, 5, 16) },
                    { new Guid("d0518d68-d70e-429e-8b9c-ca85ea3dc847"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", new DateOnly(2024, 5, 16), "john@example.com", "Admin1", "vLhXb6gzeAFmWhGt93LrsInh/tsogVni+vb3Pm+3Co4=", UserRole.Admin, new byte[] { 30, 103, 38, 195, 186, 185, 190, 23, 155, 17, 3, 110, 85, 143, 229, 42 }, new DateOnly(2024, 5, 16) },
                    { new Guid("f83914fc-de99-4f5a-abf2-ddbdfa757dc6"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", new DateOnly(2024, 5, 16), "yuanke@admin.com", "Yuanke", "HG2foYXwgqXaVM16hEyzRX1EJVI/DEdImERdR+RQGc8=", UserRole.Admin, new byte[] { 175, 187, 65, 81, 99, 45, 6, 139, 195, 247, 69, 249, 136, 231, 5, 183 }, new DateOnly(2024, 5, 16) }
                });

            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "id", "category_id", "created_date", "description", "inventory", "price", "title", "updated_date" },
                values: new object[,]
                {
                    { new Guid("1dc5b688-ba98-413b-a3cd-16c859795b05"), new Guid("975f1654-d810-4b3d-a24a-c74003eaa8f4"), new DateOnly(2024, 5, 16), "Description for Electronic Product 1", 100, 200, "Electronic Product 1", new DateOnly(2024, 5, 16) },
                    { new Guid("2fc8dc91-ff62-42c7-a270-02473c889e8f"), new Guid("170da1f9-b1ef-48c5-9b9b-413a8fc8d01a"), new DateOnly(2024, 5, 16), "Description for Furniture Product 1", 100, 800, "Furniture Product 1", new DateOnly(2024, 5, 16) },
                    { new Guid("3d12811d-14d7-4f44-9045-51ef12cda350"), new Guid("40246fbf-142a-438f-8c8a-7540dc44de81"), new DateOnly(2024, 5, 16), "Description for Books Product 1", 100, 400, "Books Product 1", new DateOnly(2024, 5, 16) },
                    { new Guid("631c35de-307f-44bd-bbfa-91f225e69660"), new Guid("21aab617-f05c-45af-a243-00aeea95d739"), new DateOnly(2024, 5, 16), "Description for Clothing Product 1", 100, 400, "Clothing Product 1", new DateOnly(2024, 5, 16) },
                    { new Guid("944abf16-6585-4444-a6ef-ff4e9b25251c"), new Guid("975f1654-d810-4b3d-a24a-c74003eaa8f4"), new DateOnly(2024, 5, 16), "Description for Electronic Product 2", 100, 100, "Electronic Product 2", new DateOnly(2024, 5, 16) },
                    { new Guid("9b6a211e-b040-45c9-82ca-8b4c9478a79e"), new Guid("170da1f9-b1ef-48c5-9b9b-413a8fc8d01a"), new DateOnly(2024, 5, 16), "Description for Furniture Product 2", 100, 300, "Furniture Product 2", new DateOnly(2024, 5, 16) },
                    { new Guid("a6a3a84d-1e1b-4bb3-ab50-36aba259e330"), new Guid("21aab617-f05c-45af-a243-00aeea95d739"), new DateOnly(2024, 5, 16), "Description for Clothing Product 2", 100, 1000, "Clothing Product 2", new DateOnly(2024, 5, 16) },
                    { new Guid("d805d6ce-ab8e-42cc-bc62-97fe7fc07343"), new Guid("40246fbf-142a-438f-8c8a-7540dc44de81"), new DateOnly(2024, 5, 16), "Description for Books Product 2", 100, 500, "Books Product 2", new DateOnly(2024, 5, 16) }
                });

            migrationBuilder.InsertData(
                table: "images",
                columns: new[] { "id", "created_date", "data", "product_id", "updated_date", "url" },
                values: new object[,]
                {
                    { new Guid("01795c2c-a567-4cd2-87da-4c629f260536"), new DateOnly(2024, 5, 16), new byte[] { 197, 149, 177, 44, 17, 80, 19, 83, 124, 127, 229, 168, 52, 98, 1, 7, 71, 209, 42, 12, 141, 11, 160, 54, 233, 75, 130, 135, 214, 207, 52, 164, 2, 37, 114, 251, 58, 66, 116, 86, 160, 1, 80, 76, 8, 128, 122, 188, 121, 40, 209, 187, 229, 190, 2, 130, 47, 179, 17, 122, 249, 118, 70, 134, 207, 45, 253, 215, 240, 104, 211, 58, 52, 117, 132, 58, 75, 184, 239, 103, 34, 185, 90, 74, 158, 110, 48, 70, 233, 158, 117, 128, 167, 131, 20, 17, 51, 116, 136, 11 }, new Guid("9b6a211e-b040-45c9-82ca-8b4c9478a79e"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=944" },
                    { new Guid("0fe636f0-38c3-4f09-9667-edec4719082e"), new DateOnly(2024, 5, 16), new byte[] { 67, 182, 118, 90, 51, 241, 6, 49, 202, 109, 133, 88, 191, 174, 82, 77, 141, 220, 56, 207, 6, 137, 29, 65, 144, 207, 197, 71, 237, 52, 69, 106, 220, 168, 25, 80, 179, 136, 74, 242, 195, 115, 146, 198, 2, 45, 235, 151, 18, 124, 73, 150, 67, 174, 122, 79, 211, 109, 114, 173, 242, 34, 234, 168, 119, 146, 103, 12, 255, 110, 221, 10, 20, 71, 23, 227, 87, 188, 201, 173, 151, 191, 38, 78, 46, 66, 201, 216, 193, 81, 133, 151, 83, 58, 221, 8, 160, 29, 117, 92 }, new Guid("2fc8dc91-ff62-42c7-a270-02473c889e8f"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=491" },
                    { new Guid("1ccbf8e2-a7b9-42ce-9ee1-8bcd15c23b0e"), new DateOnly(2024, 5, 16), new byte[] { 236, 242, 238, 219, 115, 158, 132, 210, 98, 136, 195, 7, 71, 93, 53, 145, 47, 235, 84, 88, 206, 98, 222, 23, 191, 189, 67, 142, 92, 58, 207, 217, 187, 107, 238, 98, 40, 186, 38, 122, 245, 61, 170, 187, 3, 147, 40, 192, 109, 247, 137, 102, 234, 252, 3, 20, 167, 148, 82, 253, 8, 47, 79, 36, 52, 193, 4, 87, 138, 98, 44, 17, 42, 124, 59, 17, 123, 214, 114, 240, 134, 230, 115, 72, 217, 72, 51, 80, 189, 251, 233, 33, 78, 220, 194, 235, 128, 160, 96, 254 }, new Guid("944abf16-6585-4444-a6ef-ff4e9b25251c"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=127" },
                    { new Guid("2220d1fd-69f2-4c00-9260-3cf15b09546e"), new DateOnly(2024, 5, 16), new byte[] { 10, 199, 58, 98, 247, 91, 87, 250, 36, 180, 199, 214, 34, 153, 23, 227, 247, 137, 99, 8, 243, 173, 199, 133, 82, 136, 16, 130, 158, 243, 99, 208, 146, 45, 195, 145, 207, 136, 102, 48, 78, 183, 96, 139, 32, 135, 227, 171, 152, 173, 133, 200, 27, 37, 38, 215, 115, 58, 222, 118, 30, 89, 179, 200, 153, 126, 212, 84, 239, 1, 76, 18, 150, 187, 246, 136, 69, 166, 236, 207, 109, 172, 244, 236, 23, 224, 79, 10, 217, 107, 173, 6, 176, 234, 209, 170, 18, 56, 228, 68 }, new Guid("1dc5b688-ba98-413b-a3cd-16c859795b05"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=359" },
                    { new Guid("2517f8e7-9428-4942-ae3f-19b5400efde2"), new DateOnly(2024, 5, 16), new byte[] { 179, 55, 54, 62, 142, 192, 214, 79, 212, 32, 234, 93, 46, 217, 3, 45, 1, 227, 55, 254, 200, 120, 153, 180, 59, 47, 41, 10, 13, 90, 172, 114, 217, 49, 12, 36, 22, 169, 54, 140, 1, 25, 24, 93, 110, 142, 224, 247, 1, 252, 155, 233, 124, 131, 237, 160, 68, 10, 114, 92, 161, 82, 136, 67, 52, 39, 30, 91, 27, 74, 70, 177, 142, 77, 141, 2, 169, 213, 37, 81, 72, 242, 122, 65, 105, 70, 54, 155, 249, 53, 100, 123, 237, 246, 141, 94, 199, 72, 42, 183 }, new Guid("631c35de-307f-44bd-bbfa-91f225e69660"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=716" },
                    { new Guid("2e3a4adb-3a29-4d21-90b1-72bde6c55613"), new DateOnly(2024, 5, 16), new byte[] { 123, 187, 68, 227, 178, 240, 207, 223, 64, 16, 238, 41, 126, 75, 155, 201, 231, 125, 98, 88, 232, 13, 135, 133, 79, 179, 217, 63, 130, 204, 244, 17, 77, 20, 146, 222, 156, 194, 157, 154, 22, 66, 30, 148, 75, 175, 243, 239, 26, 181, 224, 190, 144, 193, 155, 248, 42, 17, 123, 142, 11, 136, 107, 123, 210, 21, 1, 210, 75, 128, 225, 201, 56, 35, 22, 55, 251, 210, 71, 130, 73, 207, 211, 139, 76, 224, 10, 175, 139, 201, 5, 77, 238, 223, 156, 77, 52, 193, 220, 232 }, new Guid("d805d6ce-ab8e-42cc-bc62-97fe7fc07343"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=823" },
                    { new Guid("2f13e8eb-e688-4422-af5f-85e53b56c1d5"), new DateOnly(2024, 5, 16), new byte[] { 69, 255, 128, 39, 89, 122, 133, 122, 119, 217, 184, 230, 130, 20, 166, 217, 131, 112, 255, 36, 234, 126, 31, 23, 172, 24, 234, 43, 183, 201, 201, 246, 197, 204, 220, 87, 22, 24, 19, 152, 61, 26, 177, 159, 131, 58, 47, 94, 212, 133, 119, 198, 110, 173, 60, 61, 76, 109, 45, 239, 30, 211, 207, 68, 48, 2, 94, 11, 111, 8, 78, 43, 124, 48, 151, 0, 9, 36, 97, 93, 44, 12, 83, 134, 89, 124, 133, 56, 139, 242, 82, 95, 66, 203, 226, 250, 110, 148, 128, 63 }, new Guid("3d12811d-14d7-4f44-9045-51ef12cda350"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=815" },
                    { new Guid("30820fdb-d33f-4e13-aa04-cbbb15d56cba"), new DateOnly(2024, 5, 16), new byte[] { 232, 121, 118, 108, 214, 39, 211, 97, 212, 160, 78, 61, 70, 71, 160, 164, 5, 224, 63, 240, 195, 114, 86, 224, 151, 235, 174, 83, 3, 165, 232, 145, 96, 136, 203, 67, 62, 129, 56, 6, 50, 73, 44, 101, 253, 253, 40, 166, 203, 18, 163, 19, 234, 91, 12, 227, 56, 141, 188, 99, 134, 5, 40, 12, 36, 122, 3, 11, 115, 83, 254, 237, 165, 38, 150, 210, 173, 171, 26, 227, 123, 35, 143, 116, 103, 253, 52, 130, 178, 202, 64, 158, 110, 50, 196, 178, 197, 135, 178, 246 }, new Guid("a6a3a84d-1e1b-4bb3-ab50-36aba259e330"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=257" },
                    { new Guid("38649d90-a02f-4875-acb8-1ab30530547b"), new DateOnly(2024, 5, 16), new byte[] { 58, 206, 88, 204, 218, 120, 119, 186, 179, 246, 243, 153, 200, 33, 236, 4, 137, 254, 92, 166, 255, 111, 138, 60, 166, 223, 227, 166, 57, 146, 89, 252, 92, 90, 120, 145, 117, 100, 201, 248, 230, 85, 203, 197, 13, 159, 198, 100, 232, 131, 14, 64, 191, 70, 42, 45, 81, 122, 247, 32, 199, 254, 52, 98, 81, 166, 45, 188, 28, 239, 236, 245, 153, 177, 219, 226, 196, 117, 227, 217, 205, 161, 111, 154, 7, 167, 103, 171, 78, 227, 93, 13, 240, 168, 202, 212, 236, 4, 186, 1 }, new Guid("3d12811d-14d7-4f44-9045-51ef12cda350"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=310" },
                    { new Guid("6517aafb-c289-437b-a114-052632d39c37"), new DateOnly(2024, 5, 16), new byte[] { 154, 1, 57, 180, 175, 214, 174, 32, 242, 226, 90, 178, 127, 25, 126, 241, 144, 187, 218, 122, 108, 252, 133, 102, 216, 130, 221, 158, 33, 139, 40, 176, 231, 188, 201, 107, 250, 237, 156, 135, 172, 233, 157, 24, 2, 97, 93, 169, 64, 62, 110, 7, 201, 151, 247, 196, 7, 42, 114, 65, 41, 242, 237, 164, 207, 16, 218, 110, 12, 75, 186, 101, 54, 119, 106, 195, 92, 126, 48, 115, 49, 247, 184, 241, 18, 69, 161, 132, 81, 178, 240, 141, 127, 20, 172, 145, 167, 122, 215, 179 }, new Guid("631c35de-307f-44bd-bbfa-91f225e69660"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=121" },
                    { new Guid("897abb36-aca0-4334-b22c-89d43204153f"), new DateOnly(2024, 5, 16), new byte[] { 41, 164, 107, 57, 208, 68, 185, 214, 38, 179, 138, 101, 218, 101, 60, 53, 133, 60, 216, 62, 100, 207, 153, 240, 226, 254, 219, 116, 213, 248, 205, 231, 78, 252, 60, 37, 126, 141, 92, 89, 206, 81, 152, 39, 128, 143, 60, 79, 168, 99, 119, 64, 94, 90, 57, 82, 213, 185, 71, 13, 129, 238, 59, 131, 179, 5, 65, 159, 185, 245, 174, 139, 154, 36, 75, 10, 197, 96, 203, 3, 6, 125, 226, 227, 223, 71, 111, 192, 53, 191, 70, 85, 157, 107, 149, 147, 238, 233, 13, 248 }, new Guid("1dc5b688-ba98-413b-a3cd-16c859795b05"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=707" },
                    { new Guid("8d8b566d-6ae0-4564-a722-6e57895cfab0"), new DateOnly(2024, 5, 16), new byte[] { 119, 68, 96, 117, 94, 201, 27, 148, 85, 200, 199, 99, 225, 103, 151, 29, 113, 46, 106, 170, 238, 91, 37, 218, 94, 214, 137, 137, 36, 188, 246, 244, 215, 138, 80, 250, 80, 190, 167, 184, 207, 80, 221, 220, 187, 229, 48, 158, 252, 69, 144, 80, 174, 47, 35, 86, 122, 236, 72, 73, 216, 7, 208, 202, 45, 167, 138, 3, 168, 245, 163, 69, 201, 133, 222, 120, 128, 197, 12, 29, 75, 143, 205, 193, 178, 36, 106, 233, 109, 183, 61, 156, 167, 97, 164, 198, 197, 104, 84, 111 }, new Guid("9b6a211e-b040-45c9-82ca-8b4c9478a79e"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=240" },
                    { new Guid("8ee6390d-c0e9-4e6f-a2e0-8a2665ba6c70"), new DateOnly(2024, 5, 16), new byte[] { 101, 183, 76, 45, 240, 210, 171, 115, 192, 163, 84, 115, 159, 82, 183, 22, 251, 77, 96, 249, 18, 209, 239, 24, 212, 185, 101, 243, 61, 113, 131, 210, 118, 243, 109, 158, 33, 103, 62, 168, 145, 214, 89, 112, 192, 164, 19, 112, 168, 229, 97, 36, 94, 97, 166, 96, 100, 18, 76, 239, 187, 187, 8, 134, 126, 0, 40, 67, 98, 71, 33, 102, 140, 70, 34, 233, 34, 218, 181, 54, 1, 227, 219, 39, 151, 241, 20, 177, 87, 163, 31, 49, 65, 164, 154, 57, 166, 143, 180, 163 }, new Guid("9b6a211e-b040-45c9-82ca-8b4c9478a79e"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=860" },
                    { new Guid("8f24b5af-20a7-4f75-bb4a-666beb3b8254"), new DateOnly(2024, 5, 16), new byte[] { 145, 109, 231, 35, 19, 218, 230, 96, 104, 234, 209, 148, 218, 21, 196, 205, 84, 53, 178, 221, 138, 220, 146, 85, 156, 137, 232, 62, 1, 153, 81, 18, 215, 30, 34, 117, 71, 138, 22, 254, 226, 131, 66, 154, 163, 194, 18, 195, 128, 91, 90, 124, 16, 48, 140, 51, 246, 146, 152, 72, 229, 243, 206, 20, 74, 46, 40, 217, 64, 155, 213, 163, 53, 158, 247, 114, 85, 166, 3, 226, 13, 224, 182, 188, 188, 139, 106, 108, 159, 218, 96, 75, 217, 39, 122, 13, 131, 174, 36, 53 }, new Guid("631c35de-307f-44bd-bbfa-91f225e69660"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=567" },
                    { new Guid("9faf5e83-0c49-4510-b3a1-97f9d45e0263"), new DateOnly(2024, 5, 16), new byte[] { 154, 176, 11, 185, 28, 216, 107, 240, 233, 62, 145, 121, 65, 174, 100, 64, 107, 102, 193, 28, 189, 24, 254, 109, 12, 237, 237, 79, 118, 167, 118, 189, 42, 166, 182, 110, 156, 190, 164, 229, 16, 104, 29, 70, 51, 193, 14, 168, 176, 142, 162, 165, 220, 172, 148, 43, 81, 219, 103, 132, 20, 93, 182, 29, 139, 23, 173, 120, 93, 11, 231, 225, 241, 78, 129, 68, 116, 52, 46, 164, 72, 214, 150, 189, 81, 67, 61, 88, 44, 210, 67, 255, 141, 185, 170, 105, 107, 80, 168, 167 }, new Guid("d805d6ce-ab8e-42cc-bc62-97fe7fc07343"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=777" },
                    { new Guid("b2f347ad-83c9-4319-ba2a-d23f73175ef3"), new DateOnly(2024, 5, 16), new byte[] { 194, 34, 24, 206, 124, 127, 60, 220, 59, 255, 7, 44, 102, 53, 70, 135, 140, 49, 56, 33, 205, 74, 121, 16, 197, 44, 158, 242, 170, 97, 155, 129, 20, 35, 19, 96, 140, 134, 110, 133, 255, 83, 186, 51, 89, 158, 171, 96, 25, 18, 69, 83, 228, 11, 175, 188, 71, 164, 108, 26, 194, 51, 187, 41, 77, 112, 219, 241, 23, 22, 96, 203, 140, 21, 44, 114, 110, 207, 145, 74, 220, 155, 111, 202, 165, 202, 203, 200, 86, 11, 195, 178, 33, 144, 242, 10, 209, 21, 20, 92 }, new Guid("a6a3a84d-1e1b-4bb3-ab50-36aba259e330"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=188" },
                    { new Guid("b563af15-b1d5-4443-8171-70d51bf17547"), new DateOnly(2024, 5, 16), new byte[] { 176, 143, 99, 243, 238, 203, 136, 143, 89, 65, 51, 53, 26, 41, 32, 119, 166, 81, 34, 56, 20, 195, 60, 55, 75, 198, 243, 222, 95, 245, 132, 171, 130, 219, 212, 29, 84, 148, 84, 58, 201, 223, 157, 250, 254, 46, 251, 116, 39, 94, 112, 106, 186, 97, 160, 93, 100, 218, 166, 254, 132, 223, 235, 30, 230, 67, 115, 70, 121, 143, 6, 38, 36, 185, 37, 207, 155, 184, 224, 24, 71, 62, 248, 227, 44, 117, 155, 10, 107, 34, 164, 116, 188, 241, 54, 230, 40, 214, 36, 80 }, new Guid("3d12811d-14d7-4f44-9045-51ef12cda350"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=223" },
                    { new Guid("bc321c47-4b8e-4351-a973-b9beaa91af9c"), new DateOnly(2024, 5, 16), new byte[] { 128, 228, 62, 137, 4, 2, 88, 226, 141, 162, 197, 69, 5, 214, 190, 10, 251, 33, 79, 216, 58, 229, 241, 22, 138, 142, 7, 162, 100, 210, 41, 200, 208, 95, 14, 111, 146, 56, 241, 179, 15, 175, 192, 152, 233, 92, 103, 74, 92, 161, 30, 30, 242, 166, 245, 176, 157, 65, 15, 152, 186, 118, 69, 174, 208, 229, 3, 10, 154, 230, 192, 14, 75, 174, 135, 63, 215, 72, 70, 126, 139, 248, 180, 198, 137, 29, 127, 178, 156, 65, 109, 126, 229, 61, 213, 69, 149, 3, 53, 41 }, new Guid("2fc8dc91-ff62-42c7-a270-02473c889e8f"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=514" },
                    { new Guid("cea59fcf-dfe8-44b7-94e2-826100c9fec3"), new DateOnly(2024, 5, 16), new byte[] { 238, 97, 219, 55, 31, 234, 55, 206, 4, 22, 66, 67, 151, 138, 3, 189, 58, 238, 107, 140, 154, 168, 221, 158, 54, 104, 220, 94, 28, 43, 78, 234, 132, 59, 177, 194, 244, 82, 223, 5, 28, 188, 56, 66, 49, 146, 16, 109, 163, 56, 190, 101, 2, 227, 133, 35, 106, 139, 207, 55, 110, 247, 214, 201, 186, 64, 144, 125, 59, 82, 51, 245, 66, 198, 236, 104, 218, 119, 124, 241, 250, 105, 69, 65, 224, 167, 123, 224, 236, 183, 9, 223, 71, 192, 151, 79, 50, 40, 157, 94 }, new Guid("d805d6ce-ab8e-42cc-bc62-97fe7fc07343"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=407" },
                    { new Guid("ced42b39-8416-46b4-aab1-ef39e85be564"), new DateOnly(2024, 5, 16), new byte[] { 124, 98, 99, 54, 11, 221, 235, 244, 167, 144, 210, 145, 130, 104, 215, 131, 239, 16, 208, 48, 135, 252, 12, 92, 22, 109, 16, 206, 156, 11, 16, 184, 69, 224, 237, 13, 164, 161, 177, 212, 188, 81, 200, 21, 75, 138, 108, 108, 168, 90, 226, 167, 198, 248, 65, 190, 184, 28, 235, 221, 65, 139, 53, 85, 56, 250, 190, 170, 141, 16, 146, 158, 119, 73, 138, 87, 233, 23, 116, 49, 41, 45, 75, 95, 42, 96, 182, 21, 90, 104, 182, 167, 155, 98, 119, 170, 40, 79, 14, 2 }, new Guid("a6a3a84d-1e1b-4bb3-ab50-36aba259e330"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=172" },
                    { new Guid("d2633997-15b6-4872-8002-b7f5a6a39039"), new DateOnly(2024, 5, 16), new byte[] { 198, 109, 121, 134, 219, 137, 205, 5, 44, 68, 85, 168, 83, 157, 9, 67, 14, 168, 123, 215, 162, 114, 35, 192, 126, 77, 124, 64, 177, 68, 73, 33, 224, 103, 177, 22, 78, 95, 127, 99, 121, 189, 157, 0, 181, 23, 29, 186, 35, 159, 68, 250, 178, 215, 146, 72, 184, 70, 127, 108, 114, 144, 23, 167, 217, 33, 3, 188, 16, 216, 16, 69, 43, 85, 6, 250, 166, 93, 60, 222, 38, 201, 162, 7, 19, 26, 20, 97, 79, 247, 165, 108, 86, 123, 7, 67, 91, 214, 81, 221 }, new Guid("2fc8dc91-ff62-42c7-a270-02473c889e8f"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=762" },
                    { new Guid("daee13f1-a6a6-4f83-984d-4dcb90c241e1"), new DateOnly(2024, 5, 16), new byte[] { 132, 154, 75, 175, 195, 24, 195, 25, 138, 215, 104, 1, 17, 116, 127, 135, 24, 73, 213, 160, 29, 235, 191, 167, 216, 108, 231, 119, 24, 182, 5, 72, 166, 252, 118, 58, 246, 190, 70, 17, 136, 82, 0, 86, 53, 1, 161, 64, 202, 219, 202, 103, 211, 141, 250, 186, 176, 132, 254, 194, 196, 83, 240, 131, 138, 145, 80, 78, 216, 244, 165, 229, 190, 214, 103, 158, 4, 37, 114, 69, 249, 200, 137, 26, 132, 159, 253, 115, 112, 112, 168, 148, 138, 204, 113, 248, 38, 145, 53, 228 }, new Guid("944abf16-6585-4444-a6ef-ff4e9b25251c"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=262" },
                    { new Guid("f656a8d7-05a4-4574-bbfd-d1a2f4f3562b"), new DateOnly(2024, 5, 16), new byte[] { 86, 41, 215, 119, 173, 114, 103, 33, 247, 137, 40, 237, 164, 97, 145, 19, 56, 155, 202, 144, 253, 172, 19, 83, 143, 178, 134, 125, 125, 238, 92, 127, 47, 85, 173, 65, 155, 21, 216, 122, 220, 226, 206, 123, 74, 223, 100, 188, 230, 133, 191, 187, 185, 227, 199, 211, 178, 71, 140, 126, 36, 23, 180, 134, 200, 59, 179, 92, 118, 99, 145, 6, 119, 144, 42, 245, 148, 6, 168, 87, 193, 115, 126, 30, 223, 54, 215, 216, 253, 13, 96, 39, 241, 123, 168, 107, 167, 34, 47, 233 }, new Guid("944abf16-6585-4444-a6ef-ff4e9b25251c"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=108" },
                    { new Guid("ffea8644-7a3e-4e5f-98c0-16b0e3f5787d"), new DateOnly(2024, 5, 16), new byte[] { 195, 237, 107, 19, 99, 80, 75, 199, 37, 54, 129, 138, 2, 1, 57, 81, 200, 19, 82, 91, 43, 14, 201, 238, 48, 18, 155, 42, 220, 223, 135, 88, 188, 79, 137, 139, 55, 222, 237, 64, 50, 42, 195, 44, 95, 142, 169, 81, 225, 34, 167, 139, 71, 122, 245, 161, 223, 127, 139, 135, 222, 178, 91, 33, 206, 78, 183, 249, 235, 147, 212, 179, 136, 175, 83, 68, 87, 34, 235, 14, 240, 203, 235, 251, 142, 160, 122, 215, 61, 50, 142, 78, 64, 165, 7, 14, 170, 52, 24, 82 }, new Guid("1dc5b688-ba98-413b-a3cd-16c859795b05"), new DateOnly(2024, 5, 16), "https://picsum.photos/200/?random=754" }
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
