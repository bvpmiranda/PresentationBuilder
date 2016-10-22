using System.Web;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace PresentationBuilder.Helpers
{
    public class PdfHelper
    {
        public static bool slpit()
        {           
            //inputDocument.PageCount

            string path = HttpContext.Current.Server.MapPath("~/");

            if (path.EndsWith("\\"))
            {
                path = path.Substring(0, path.Length - 1);
            }

            path = path.Substring(0, path.LastIndexOf("\\")) + "\\PresentationBuilderDocuments\\";

            var pdfName = path + "Teste.pdf";
            PdfDocument inputDocument = PdfReader.Open(pdfName, PdfDocumentOpenMode.ReadOnly);

            return false;
        }
    }
}