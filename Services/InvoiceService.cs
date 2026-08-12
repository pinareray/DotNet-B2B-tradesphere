using System.Globalization;
using DotNet_B2B_tradesphere.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace DotNet_B2B_tradesphere.Services;

public class InvoiceService : IInvoiceService
{
    private static readonly CultureInfo Tr = CultureInfo.GetCultureInfo("tr-TR");

    public byte[] GenerateInvoicePdf(Order order, Dealer dealer)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(10).FontColor(Colors.Grey.Darken3));

                page.Header().Element(header => ComposeHeader(header, order));
                page.Content().Element(content => ComposeContent(content, order, dealer));
                page.Footer().AlignCenter().Text(text =>
                {
                    text.Span("TradeSphere B2B  •  ").FontColor(Colors.Grey.Medium);
                    text.Span("Sayfa ").FontColor(Colors.Grey.Medium);
                    text.CurrentPageNumber();
                    text.Span(" / ");
                    text.TotalPages();
                });
            });
        });

        return document.GeneratePdf();
    }

    private static void ComposeHeader(IContainer container, Order order)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(col =>
            {
                col.Item().Text("TradeSphere B2B")
                    .FontSize(22)
                    .Bold()
                    .FontColor(Colors.Blue.Darken3);
                col.Item().Text("Kurumsal Sipariş Faturası")
                    .FontSize(11)
                    .FontColor(Colors.Grey.Darken1);
            });

            row.ConstantItem(180).AlignRight().Column(col =>
            {
                col.Item().Text($"Sipariş No: #{order.Id}").Bold().FontSize(12);
                col.Item().Text($"Fatura Tarihi: {order.OrderDate.ToLocalTime():dd.MM.yyyy HH:mm}");
            });
        });
    }

    private static void ComposeContent(IContainer container, Order order, Dealer dealer)
    {
        container.PaddingVertical(20).Column(col =>
        {
            col.Item().Background(Colors.Grey.Lighten4).Padding(12).Column(info =>
            {
                info.Item().Text("Bayi Bilgileri").Bold().FontSize(12).FontColor(Colors.Blue.Darken3);
                info.Item().PaddingTop(6).Text($"Şirket Adı: {dealer.CompanyName}");
                info.Item().Text($"Vergi No: {dealer.TaxNumber}");
                if (!string.IsNullOrWhiteSpace(dealer.Email))
                    info.Item().Text($"E-posta: {dealer.Email}");
            });

            col.Item().PaddingTop(20).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(4);
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(2);
                });

                table.Header(header =>
                {
                    header.Cell().Element(HeaderCell).Text("Ürün");
                    header.Cell().Element(HeaderCell).AlignRight().Text("Adet");
                    header.Cell().Element(HeaderCell).AlignRight().Text("Birim Fiyat");
                    header.Cell().Element(HeaderCell).AlignRight().Text("Satır Toplamı");
                });

                foreach (var item in order.OrderItems)
                {
                    var lineTotal = item.UnitPrice * item.Quantity;
                    var productName = item.Product?.Name ?? $"Ürün #{item.ProductId}";

                    table.Cell().Element(BodyCell).Text(productName);
                    table.Cell().Element(BodyCell).AlignRight().Text(item.Quantity.ToString());
                    table.Cell().Element(BodyCell).AlignRight().Text(item.UnitPrice.ToString("C2", Tr));
                    table.Cell().Element(BodyCell).AlignRight().Text(lineTotal.ToString("C2", Tr));
                }
            });

            col.Item().PaddingTop(16).AlignRight().Column(total =>
            {
                total.Item().Text($"Toplam Tutar: {order.TotalAmount.ToString("C2", Tr)}")
                    .FontSize(14)
                    .Bold()
                    .FontColor(Colors.Blue.Darken3);
            });
        });
    }

    private static IContainer HeaderCell(IContainer container)
        => container
            .Background(Colors.Blue.Darken3)
            .Padding(8)
            .DefaultTextStyle(x => x.FontColor(Colors.White).Bold());

    private static IContainer BodyCell(IContainer container)
        => container
            .BorderBottom(1)
            .BorderColor(Colors.Grey.Lighten2)
            .PaddingVertical(8)
            .PaddingHorizontal(8);
}
