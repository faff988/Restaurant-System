using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Data;
using Microsoft.AspNetCore.Authorization;

namespace RestaurantSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ReportController(ApplicationDbContext context) { _context = context; }

        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.MenuItem)
                .ToListAsync();

            var reservations = await _context.Reservations.ToListAsync();
            var menuItems = await _context.MenuItems.ToListAsync();

            ViewBag.TotalOrders = orders.Count;
            ViewBag.TotalRevenue = orders.Sum(o => o.TotalAmount);
            ViewBag.TotalReservations = reservations.Count;
            ViewBag.TotalMenuItems = menuItems.Count;
            ViewBag.PendingOrders = orders.Count(o => o.Status == "Pending");
            ViewBag.CompletedOrders = orders.Count(o => o.Status == "Completed");
            ViewBag.RecentOrders = orders.OrderByDescending(o => o.OrderDate).Take(5).ToList();
            ViewBag.TopItems = orders.SelectMany(o => o.OrderDetails)
                .GroupBy(od => od.MenuItem.Name)
                .Select(g => new { Name = g.Key, Count = g.Sum(x => x.Quantity) })
                .OrderByDescending(x => x.Count)
                .Take(5).ToList();

            return View();
        }
        public async Task<IActionResult> ExportPdf()
        {
            var orders = await _context.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.MenuItem).ToListAsync();

            using var ms = new MemoryStream();
            var writer = new iText.Kernel.Pdf.PdfWriter(ms);
            var pdf = new iText.Kernel.Pdf.PdfDocument(writer);
            var document = new iText.Layout.Document(pdf);

            document.Add(new iText.Layout.Element.Paragraph("Restaurant System - Order Report")
                .SetFontSize(20).SetBold());
            document.Add(new iText.Layout.Element.Paragraph($"Generated: {DateTime.Now:MMMM dd, yyyy}")
                .SetFontSize(12));
            document.Add(new iText.Layout.Element.Paragraph($"Total Orders: {orders.Count} | Total Revenue: ${orders.Sum(o => o.TotalAmount):0.00}")
                .SetFontSize(12).SetBold());
            document.Add(new iText.Layout.Element.Paragraph(" "));

            var table = new iText.Layout.Element.Table(5).UseAllAvailableWidth();
            foreach (var h in new[] { "#", "Customer", "Date", "Total", "Status" })
                table.AddHeaderCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(h).SetBold()));

            foreach (var order in orders)
            {
                table.AddCell(order.Id.ToString());
                table.AddCell(order.CustomerName);
                table.AddCell(order.OrderDate.ToString("MMM dd, yyyy"));
                table.AddCell($"${order.TotalAmount:0.00}");
                table.AddCell(order.Status);
            }

            document.Add(table);
            document.Close();

            return File(ms.ToArray(), "application/pdf", "OrderReport.pdf");
        }

        public async Task<IActionResult> ExportExcel()
        {
            var orders = await _context.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.MenuItem).ToListAsync();

            using var workbook = new ClosedXML.Excel.XLWorkbook();
            var ws = workbook.Worksheets.Add("Orders");

            ws.Cell(1, 1).Value = "Order Report - Restaurant System";
            ws.Cell(1, 1).Style.Font.Bold = true;
            ws.Cell(1, 1).Style.Font.FontSize = 16;

            var headers = new[] { "ID", "Customer", "Email", "Date", "Total", "Status" };
            for (int i = 0; i < headers.Length; i++)
            {
                ws.Cell(3, i + 1).Value = headers[i];
                ws.Cell(3, i + 1).Style.Font.Bold = true;
                ws.Cell(3, i + 1).Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.DarkRed;
                ws.Cell(3, i + 1).Style.Font.FontColor = ClosedXML.Excel.XLColor.White;
            }

            for (int i = 0; i < orders.Count; i++)
            {
                ws.Cell(i + 4, 1).Value = orders[i].Id;
                ws.Cell(i + 4, 2).Value = orders[i].CustomerName;
                ws.Cell(i + 4, 3).Value = orders[i].CustomerEmail;
                ws.Cell(i + 4, 4).Value = orders[i].OrderDate.ToString("MMM dd, yyyy");
                ws.Cell(i + 4, 5).Value = (double)orders[i].TotalAmount;
                ws.Cell(i + 4, 6).Value = orders[i].Status;
            }

            ws.Columns().AdjustToContents();

            using var ms = new MemoryStream();
            workbook.SaveAs(ms);
            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "OrderReport.xlsx");
        }
    }


}