using Food_Scape.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.qrcode;
using Microsoft.VisualBasic;
using NuGet.LibraryModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Diagnostics.Metrics;
using System.Drawing;

namespace Food_Scape.Repositories
{
    public class TransactionRepository
    {
        private FoodScapeContext _context;

        public TransactionRepository(FoodScapeContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This method creates a new IPN (Instant Payment Notification) transaction record in the database.
        /// </summary>
        /// <param name="data"></param>
        /// <returns> Boolean Value</returns>
        public bool createIPN (Ipn data)
        {
            try
            {
                _context.Ipns.Add(data);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// // This method retrieves all IPN transaction records from the database.
        /// </summary>
        /// <returns> IEnumerable<Ipn> collection</returns>
        public IEnumerable<Ipn> GetAllTransactions()
        {
            // Get all Transactions
            var ipns = _context.Ipns;
            return ipns;
        }

        public byte[] GenerateTicketPdf(Ipn transaction)
        {
            // Create a new PDF document
            Document document = new Document();

            // Create a new memory stream to hold the PDF file bytes
            MemoryStream stream = new MemoryStream();

            // Create a new PDF writer that writes to the memory stream
            PdfWriter writer = PdfWriter.GetInstance(document, stream);

            // Open the document for writing
            document.Open();

            // Add some content to the document
            Paragraph paragraph = new Paragraph($"Ticket for Payment ID: {transaction.PaymentId}");
            document.Add(paragraph);
            document.Add(new Paragraph("\n"));

            PdfPTable table = new PdfPTable(2);
            table.AddCell("Event:");
            table.AddCell("FoodScape");
            table.AddCell("Date:");
            table.AddCell("Aug 12,2023");
            table.AddCell("Location:");
            table.AddCell("Sample Location");


            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;
            table.HorizontalAlignment = Element.ALIGN_CENTER;
            document.Add(table);

            Paragraph footer = new Paragraph("This ticket is non-refundable and non-transferable.");
            footer.Alignment = Element.ALIGN_CENTER;
            document.Add(footer);

            // Close the document
            document.Close();

            // Return the PDF file bytes
            return stream.ToArray();
        }

    }
}
