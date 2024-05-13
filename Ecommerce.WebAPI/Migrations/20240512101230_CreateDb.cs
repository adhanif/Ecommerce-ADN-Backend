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
                    created_date = table.Column<DateOnly>(type: "date", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
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
                    password = table.Column<string>(type: "varchar", maxLength: 20, nullable: false),
                    salt = table.Column<byte[]>(type: "bytea", nullable: false),
                    avatar = table.Column<string>(type: "varchar(255)", nullable: true),
                    user_role = table.Column<UserRole>(type: "user_role", nullable: false),
                    created_date = table.Column<DateOnly>(type: "date", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
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
                    created_date = table.Column<DateOnly>(type: "date", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
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
                name: "orders",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<OrderStatus>(type: "order_status", nullable: false),
                    created_date = table.Column<DateOnly>(type: "date", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
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
                    created_date = table.Column<DateOnly>(type: "date", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
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
                    created_date = table.Column<DateOnly>(type: "date", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
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
                columns: new[] { "id", "image", "name", "updated_date" },
                values: new object[,]
                {
                    { new Guid("052e4e70-080e-496a-9d52-a7f7e7033ba9"), "https://picsum.photos/200/?random=5", "Sports", null },
                    { new Guid("3029c3ac-6439-4a68-81be-a8fae54b3a16"), "https://picsum.photos/200/?random=1", "Clothing", null },
                    { new Guid("3ed84354-cb1e-4519-b853-548b16bb874b"), "https://picsum.photos/200/?random=2", "Books", null },
                    { new Guid("8b0392cb-8b34-4e85-89cb-c3bf5e78053b"), "https://picsum.photos/200/?random=5", "Furniture", null },
                    { new Guid("a1091241-f7ff-4e57-8fec-4c96e6b2a75d"), "https://picsum.photos/200/?random=6", "Toys", null },
                    { new Guid("c5ccbb3f-6322-4bd0-892e-2812130e7efc"), "https://picsum.photos/200/?random=2", "Electronic", null }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "avatar", "email", "name", "password", "salt", "updated_date", "user_role" },
                values: new object[,]
                {
                    { new Guid("8228a9c7-e68f-4532-ad71-f74fd45b0555"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", "customer1@customer.com", "Customer1", "P1leiiBdgTvsoowsAmFZIcYUtJ5PgJddqYR9u4PLCPo=", new byte[] { 159, 180, 130, 176, 184, 163, 10, 29, 91, 182, 166, 70, 10, 201, 179, 233 }, null, UserRole.Admin },
                    { new Guid("898d0ad0-7ab6-4204-a037-7b1310a46e00"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", "yuanke@admin.com", "Yuanke", "z8kCI8OplfAfOYYeO31KQSJtrTgDqg3jtrEZePxvrVU=", new byte[] { 62, 1, 224, 210, 236, 209, 217, 81, 165, 33, 217, 91, 128, 36, 160, 83 }, null, UserRole.Admin },
                    { new Guid("8b9a5025-299c-4452-8498-5f69f9f2a6f4"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", "binh@admin.com", "Binh", "sKgxDDdmpFTPyDzDEC3Iu+T5mMY9xEFbVc1KCsCbweA=", new byte[] { 32, 179, 217, 105, 120, 44, 249, 139, 182, 169, 7, 88, 209, 240, 68, 1 }, null, UserRole.Admin },
                    { new Guid("a46161b4-cf6f-4d86-9714-c619dfeb792e"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", "adnan@admin.com", "Adnan", "7dKW1SbWwFBfZ3e/8Mc2n/7ZNp9vC8Bno5yv3ir4W6Q=", new byte[] { 159, 30, 102, 36, 188, 7, 74, 35, 144, 2, 41, 185, 162, 4, 155, 203 }, null, UserRole.Admin },
                    { new Guid("e8dc6bfa-f9c1-4049-9471-94e4818f0f1f"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", "john@example.com", "Admin1", "SbHYgcGoopXCfjVR3WAHdZAlcBROJA2nJ6iBAcR4cBI=", new byte[] { 95, 190, 139, 46, 103, 64, 221, 11, 157, 234, 110, 144, 137, 6, 132, 11 }, null, UserRole.Admin }
                });

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
