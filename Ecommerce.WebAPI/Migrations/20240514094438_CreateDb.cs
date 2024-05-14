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
                    created_date = table.Column<DateOnly>(type: "date", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
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
                name: "orders",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    address_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<OrderStatus>(type: "order_status", nullable: false),
                    created_date = table.Column<DateOnly>(type: "date", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_orders", x => x.id);
                    table.ForeignKey(
                        name: "fk_orders_addresses_address_id",
                        column: x => x.address_id,
                        principalTable: "addresses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_orders_users_user_id",
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
                    { new Guid("0c28da19-31dd-4eef-9f2f-1e93fcf8e77b"), "https://picsum.photos/200/?random=2", "Toys", null },
                    { new Guid("502436c6-4f21-4f57-a87c-296aa4b5db05"), "https://picsum.photos/200/?random=7", "Clothing", null },
                    { new Guid("6ccb83ea-d5cc-4ade-9dea-010d2ec65206"), "https://picsum.photos/200/?random=8", "Books", null },
                    { new Guid("7d2c5831-6ed0-437b-83a1-f353b895434b"), "https://picsum.photos/200/?random=1", "Furniture", null },
                    { new Guid("c67e5ba5-9616-4254-a1ec-6f011df2f140"), "https://picsum.photos/200/?random=3", "Electronic", null },
                    { new Guid("e07d7d47-80d7-47e0-b6f4-15d60de5142b"), "https://picsum.photos/200/?random=8", "Sports", null }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "avatar", "email", "name", "password", "salt", "updated_date", "user_role" },
                values: new object[,]
                {
                    { new Guid("04cfef1b-a78c-4b3a-bd19-20cd779bf20a"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", "adnan@admin.com", "Adnan", "vaw0sCJDV4B4nZQeDdyDI9ateQX2G+TdLODtTdhGHCI=", new byte[] { 11, 198, 51, 61, 51, 115, 183, 9, 22, 50, 67, 126, 120, 46, 162, 93 }, null, UserRole.Admin },
                    { new Guid("59c2ba90-3ca7-4c19-8c58-db8b6998b169"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", "yuanke@admin.com", "Yuanke", "jNPvxc0kTNSI+HehNqrBa+DSx/GFYotyIJ2p0OwQu7s=", new byte[] { 8, 57, 125, 91, 65, 18, 46, 89, 121, 198, 251, 238, 147, 0, 72, 68 }, null, UserRole.Admin },
                    { new Guid("97664198-3d6c-4991-8671-a043526a743a"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", "binh@admin.com", "Binh", "Ch0nl9NeaWMKnuUph5EsCC3NN4/87k4/f2Hi2zkGaEo=", new byte[] { 27, 254, 59, 116, 10, 120, 29, 61, 234, 197, 81, 94, 65, 225, 89, 106 }, null, UserRole.Admin },
                    { new Guid("98a89923-daca-472d-bc4e-d13240291835"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", "john@example.com", "Admin1", "C9FdLx/9YDyKdPpwspwCUbAJReK7OYpg3cLxP/1vWdc=", new byte[] { 76, 197, 78, 176, 83, 174, 92, 255, 28, 31, 254, 128, 139, 60, 146, 183 }, null, UserRole.Admin },
                    { new Guid("e105ecbf-7282-4e67-906b-85816e9b8f77"), "https://picsum.photos/200/?random=System.Func`1[System.Int32]", "customer1@customer.com", "Customer1", "ttv5UpAMy+L0LDc3nGdfoXOIodVhbS+/QH0Kh3gigSY=", new byte[] { 4, 194, 172, 53, 154, 203, 166, 239, 218, 7, 188, 49, 105, 85, 235, 235 }, null, UserRole.Admin }
                });

            migrationBuilder.CreateIndex(
                name: "city",
                table: "addresses",
                column: "phone_number",
                unique: true);

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
                name: "ix_orders_address_id",
                table: "orders",
                column: "address_id");

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
                name: "addresses");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
