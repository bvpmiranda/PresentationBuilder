using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace PresentationBuilder.Helpers
{
    public class PdfHelper
    {
        public static bool slpit()
        {
            var filename = "";
            PdfDocument inputDocument = PdfReader.Open(filename, PdfDocumentOpenMode.ReadOnly);
            //inputDocument.PageCount

            string path = HttpContext.Current.Server.MapPath("~/");

            if (path.EndsWith("\\"))
            {
                path = path.Substring(0, path.Length - 1);
            }

            path = path.Substring(0, path.LastIndexOf("\\")) + "\\PresentationBuilderDocuments\\";

            var pdfName = path + "Teste.pdf";

            return false;
        }
    }
}