using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FlowerShop.API.Migrations
{
    /// <inheritdoc />
    public partial class AddOccasionModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OccasionId",
                table: "Flowers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Occasions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Icon = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Occasions", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Flowers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Name", "OccasionId", "Price", "Stock" },
                values: new object[] { 4, "12 kırmızı gül ile hazırlanmış tutkulu buket", "/images/ask-i-memnu.jpg", "Aşk-ı Memnu", 4, 185.00m, 50 });

            migrationBuilder.UpdateData(
                table: "Flowers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Name", "OccasionId", "Price", "Stock" },
                values: new object[] { 4, "Pembe ve beyaz güller ile romantik buket", "/images/kalp-hirsizi.jpg", "Kalp Hırsızı", 4, 165.00m, 40 });

            migrationBuilder.UpdateData(
                table: "Flowers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Name", "OccasionId", "Price", "Stock" },
                values: new object[] { 4, "Beyaz güller ve lilyumlarla gelin buketi", "/images/ebedi-mutluluk.jpg", "Ebedi Mutluluk", 2, 285.00m, 25 });

            migrationBuilder.UpdateData(
                table: "Flowers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Name", "OccasionId", "Price", "Stock" },
                values: new object[] { 4, "Krem güller ve baby breath ile düğün buketi", "/images/sonsuz-ask.jpg", "Sonsuz Aşk", 2, 245.00m, 30 });

            migrationBuilder.UpdateData(
                table: "Flowers",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Description", "ImageUrl", "Name", "OccasionId", "Price", "Stock" },
                values: new object[] { "Renkli gerbera ve güller ile neşeli buket", "/images/nese-patlamasi.jpg", "Neşe Patlaması", 1, 125.00m, 60 });

            migrationBuilder.InsertData(
                table: "Occasions",
                columns: new[] { "Id", "CreatedAt", "Description", "Icon", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Doğum günü kutlamaları için", "bi-gift", true, "Doğum Günü" },
                    { 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Düğün törenleri için", "bi-heart", true, "Düğün" },
                    { 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Yıldönümü kutlamaları için", "bi-calendar-heart", true, "Yıldönümü" },
                    { 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "14 Şubat sevgililer günü için", "bi-heart-fill", true, "Sevgililer Günü" },
                    { 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Anneler günü için", "bi-flower1", true, "Anneler Günü" },
                    { 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Babalar günü için", "bi-person-heart", true, "Babalar Günü" },
                    { 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Yeni işe başlama tebriği", "bi-briefcase", true, "Yeni İş Tebriği" },
                    { 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Hastalık durumlarında", "bi-heart-pulse", true, "Geçmiş Olsun" },
                    { 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Teşekkür etmek için", "bi-hand-thumbs-up", true, "Teşekkür" },
                    { 10, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Taziye için", "bi-flower2", true, "Başsağlığı" },
                    { 11, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Bebek doğumu tebriği", "bi-baby", true, "Yeni Bebek" },
                    { 12, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Mezuniyet kutlaması", "bi-mortarboard", true, "Mezuniyet" },
                    { 13, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Terfi tebriği", "bi-trophy", true, "Terfi" },
                    { 14, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Nişan töreni için", "bi-gem", true, "Nişan" },
                    { 15, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Evlilik teklifi için", "bi-ring", true, "Evlilik Teklifi" },
                    { 16, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Özür dilemek için", "bi-emoji-frown", true, "Özür Dileme" },
                    { 17, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Başarılı karne için", "bi-journal-check", true, "Karne Hediyesi" },
                    { 18, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Yeni eve taşınma", "bi-house-heart", true, "Yeni Ev" },
                    { 19, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Bayram kutlamaları", "bi-star", true, "Bayram" },
                    { 20, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Genel kutlamalar", "bi-balloon", true, "Kutlama" }
                });

            migrationBuilder.InsertData(
                table: "Flowers",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Description", "ImageUrl", "IsActive", "Name", "OccasionId", "Price", "Stock", "UpdatedAt" },
                values: new object[,]
                {
                    { 6, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Sarı ve turuncu çiçeklerle enerjik buket", "/images/mutluluk-dansi.jpg", true, "Mutluluk Dansı", 1, 135.00m, 45, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pembe güller ve karanfiller ile özel buket", "/images/anne-sevgisi.jpg", true, "Anne Sevgisi", 5, 155.00m, 55, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 8, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Beyaz ve pembe çiçeklerle zarif buket", "/images/sefkat-eli.jpg", true, "Şefkat Eli", 5, 145.00m, 50, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Beyaz lilyum ve papatyalarla iyileşme buketi", "/images/sifa-dilegi.jpg", true, "Şifa Dileği", 8, 95.00m, 35, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 10, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Sarı çiçeklerle moral verici buket", "/images/umut-isigi.jpg", true, "Umut Işığı", 8, 105.00m, 40, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 11, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Beyaz çiçeklerle saygı buketi", "/images/huzur-bahcesi.jpg", true, "Huzur Bahçesi", 10, 175.00m, 20, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 12, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Beyaz güller ve lilyumlarla taziye buketi", "/images/sessiz-dua.jpg", true, "Sessiz Dua", 10, 195.00m, 15, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 13, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Sarı güller ve orkidelerle tebrik buketi", "/images/basari-yolculugu.jpg", true, "Başarı Yolculuğu", 7, 165.00m, 30, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 14, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Turuncu ve kırmızı çiçeklerle motivasyon buketi", "/images/zirve-tirmanisi.jpg", true, "Zirve Tırmanışı", 7, 155.00m, 35, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 15, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pembe ve beyaz çiçeklerle bebek buketi", "/images/minik-melek.jpg", true, "Minik Melek", 11, 125.00m, 40, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 16, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pastel renkli çiçeklerle hoş geldin buketi", "/images/yeni-hayat.jpg", true, "Yeni Hayat", 11, 135.00m, 35, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 17, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Mavi ve beyaz çiçeklerle mezuniyet buketi", "/images/diploma-sevinci.jpg", true, "Diploma Sevinci", 12, 145.00m, 25, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 18, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Yeşil ve beyaz çiçeklerle akademik başarı buketi", "/images/bilgi-agaci.jpg", true, "Bilgi Ağacı", 12, 155.00m, 20, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 19, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Beyaz güller ve mavi çiçeklerle özür buketi", "/images/pismanlik-cicegi.jpg", true, "Pişmanlık Çiçeği", 16, 115.00m, 30, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 20, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Beyaz ve yeşil çiçeklerle barışma buketi", "/images/baris-dali.jpg", true, "Barış Dalı", 16, 125.00m, 25, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flowers_OccasionId",
                table: "Flowers",
                column: "OccasionId");

            migrationBuilder.CreateIndex(
                name: "IX_Occasions_Name",
                table: "Occasions",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Flowers_Occasions_OccasionId",
                table: "Flowers",
                column: "OccasionId",
                principalTable: "Occasions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flowers_Occasions_OccasionId",
                table: "Flowers");

            migrationBuilder.DropTable(
                name: "Occasions");

            migrationBuilder.DropIndex(
                name: "IX_Flowers_OccasionId",
                table: "Flowers");

            migrationBuilder.DeleteData(
                table: "Flowers",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Flowers",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Flowers",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Flowers",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Flowers",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Flowers",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Flowers",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Flowers",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Flowers",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Flowers",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Flowers",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Flowers",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Flowers",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Flowers",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Flowers",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DropColumn(
                name: "OccasionId",
                table: "Flowers");

            migrationBuilder.UpdateData(
                table: "Flowers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Name", "Price", "Stock" },
                values: new object[] { 1, "Klasik kırmızı gül", "/images/red-rose.jpg", "Kırmızı Gül", 15.00m, 100 });

            migrationBuilder.UpdateData(
                table: "Flowers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Name", "Price", "Stock" },
                values: new object[] { 1, "Zarif beyaz gül", "/images/white-rose.jpg", "Beyaz Gül", 12.00m, 80 });

            migrationBuilder.UpdateData(
                table: "Flowers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Name", "Price", "Stock" },
                values: new object[] { 2, "Canlı sarı lale", "/images/yellow-tulip.jpg", "Sarı Lale", 8.00m, 60 });

            migrationBuilder.UpdateData(
                table: "Flowers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CategoryId", "Description", "ImageUrl", "Name", "Price", "Stock" },
                values: new object[] { 3, "Egzotik mor orkide", "/images/purple-orchid.jpg", "Mor Orkide", 45.00m, 25 });

            migrationBuilder.UpdateData(
                table: "Flowers",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Description", "ImageUrl", "Name", "Price", "Stock" },
                values: new object[] { "Karışık çiçek buketi", "/images/mixed-bouquet.jpg", "Karma Buket", 35.00m, 40 });
        }
    }
}
