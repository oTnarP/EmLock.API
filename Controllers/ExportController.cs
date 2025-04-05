using EmLock.API.Data;
using EmLock.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Npgsql.Internal;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using iText.IO.Font.Constants;
using iText.Kernel.Font;

namespace EmLock.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExportController : ControllerBase
    {
        private readonly DataContext _context;

        public ExportController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("devices/csv")]
        [Authorize(Roles = "Admin,Dealer")]
        public IActionResult ExportDevicesCsv()
        {
            var devices = _context.Devices.ToList();

            var csv = new StringBuilder();
            csv.AppendLine("IMEI,Model,OwnerName,OwnerPhone,IsLocked");

            foreach (var device in devices)
            {
                csv.AppendLine($"{device.IMEI},{device.Model},{device.OwnerName},{device.OwnerPhone},{device.IsLocked}");
            }

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", "devices.csv");
        }

        [HttpGet("devices/pdf")]
        [Authorize(Roles = "Admin,Dealer")]
        public IActionResult ExportDevicesPdf()
        {
            var devices = _context.Devices.ToList();

            using var ms = new MemoryStream();
            using var writer = new PdfWriter(ms);
            using var pdf = new PdfDocument(writer);
            var document = new iText.Layout.Document(pdf);

            document.Add(new Paragraph("Device List"));
            foreach (var device in devices)
            {
                document.Add(new Paragraph($"IMEI: {device.IMEI}, Model: {device.Model}, Owner: {device.OwnerName}, Phone: {device.OwnerPhone}, Locked: {device.IsLocked}"));
            }

            document.Close();
            return File(ms.ToArray(), "application/pdf", "devices.pdf");
        }
        [HttpGet("emi-logs/{imei}/csv")]
        [Authorize(Roles = "Admin,Shopkeeper")]
        public async Task<IActionResult> ExportEmiLogsCsv(string imei)
        {
            var logs = await _context.EmiLogs
                .Include(e => e.EmiSchedule)
                .ThenInclude(s => s.Device)
                .Where(e => e.EmiSchedule.Device.IMEI == imei)
                .ToListAsync();

            var builder = new StringBuilder();
            builder.AppendLine("PaymentDate,AmountPaid,EmiScheduleId");

            foreach (var log in logs)
            {
                builder.AppendLine($"{log.PaidAt:yyyy-MM-dd},{log.AmountPaid},{log.EmiScheduleId}");
            }

            var csvBytes = Encoding.UTF8.GetBytes(builder.ToString());
            return File(csvBytes, "text/csv", $"EmiLogs_{imei}.csv");
        }


        [HttpGet("emi-logs/{imei}/pdf")]
        [Authorize(Roles = "Admin,Shopkeeper")]
        public async Task<IActionResult> ExportEmiLogsPdf(string imei)
        {
            var logs = await _context.EmiLogs
                .Include(e => e.EmiSchedule)
                .ThenInclude(s => s.Device)
                .Where(e => e.EmiSchedule.Device.IMEI == imei)
                .ToListAsync();

            using var stream = new MemoryStream();
            var writer = new PdfWriter(stream);
            var pdf = new PdfDocument(writer);
            var document = new iText.Layout.Document(pdf);

            var boldTitle = new Paragraph($"EMI Logs for Device: {imei}")
            .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
            .SetFontSize(14);

            document.Add(boldTitle);


            var table = new Table(3, true);
            table.AddHeaderCell("Date");
            table.AddHeaderCell("Amount Paid");
            table.AddHeaderCell("Schedule ID");

            foreach (var log in logs)
            {
                table.AddCell(log.PaidAt.ToString("yyyy-MM-dd"));
                table.AddCell(log.AmountPaid.ToString("F2"));
                table.AddCell(log.EmiScheduleId.ToString());
            }

            document.Add(table);
            document.Close();

            return File(stream.ToArray(), "application/pdf", $"EmiLogs_{imei}.pdf");
        }

    }
}
