using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Drawing.Imaging;
using System.Collections;
using System.Drawing;
using iTextSharp.text.pdf;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;


namespace PresentationBuilder.Core.Util
{
    public static class PDF
    {

        
        //public class TiffImage
        //{
        //    private string myPath;
        //    private Guid myGuid;
        //    private FrameDimension myDimension;
        //    public ArrayList myImages = new ArrayList();
        //    private int myPageCount;
        //    private Bitmap myBMP;

        //    public TiffImage(string path)
        //    {
        //        MemoryStream ms;
        //        Image myImage;

        //        myPath = path;
        //        using (FileStream fs = new FileStream(myPath, FileMode.Open))
        //        {
        //            Stream oStream = new System.IO.File( @"C:\MyFile.pdf", "application/pdf");
        //            myImage = Image.FromStream(fs as Stream);
        //            myGuid = myImage.FrameDimensionsList[0];
        //            myDimension = new FrameDimension(myGuid);
        //            myPageCount = myImage.GetFrameCount(myDimension);
        //            for (int i = 0; i < myPageCount; i++)
        //            {
        //                ms = new MemoryStream();
        //                myImage.SelectActiveFrame(myDimension, i);
        //                myImage.Save(ms, ImageFormat.Bmp);
        //                myBMP = new Bitmap(ms);
        //                myImages.Add(myBMP);
        //                ms.Close();
        //            }
        //        }
        //    }
        //}

        public static void ExtractImagesFromPDF(string sourcePdf, string outputPath)
        {
            // NOTE:  This will only get the first image it finds per page.
            PdfReader pdf = new PdfReader(sourcePdf);
            RandomAccessFileOrArray raf = new iTextSharp.text.pdf.RandomAccessFileOrArray(sourcePdf);

            try
            {
                for (int pageNumber = 1; pageNumber <= pdf.NumberOfPages; pageNumber++)
                {


                    PdfDictionary pg = pdf.GetPageN(pageNumber);

                    // recursively search pages, forms and groups for images.
                    PdfObject obj = FindImageInPDFDictionary(pg);
                    if (obj != null)
                    {

                        int XrefIndex = Convert.ToInt32(((PRIndirectReference)obj).Number.ToString(System.Globalization.CultureInfo.InvariantCulture));
                        PdfObject pdfObj = pdf.GetPdfObject(XrefIndex);
                        PdfStream pdfStrem = (PdfStream)pdfObj;
                        byte[] bytes = PdfReader.GetStreamBytesRaw((PRStream)pdfStrem);
                        if ((bytes != null))
                        {
                            using (System.IO.MemoryStream memStream = new System.IO.MemoryStream(bytes))
                            {
                                memStream.Position = 0;
                                System.Drawing.Image img = System.Drawing.Image.FromStream(memStream);
                                // must save the file while stream is open.
                                if (!Directory.Exists(outputPath))
                                    Directory.CreateDirectory(outputPath);

                                string path = Path.Combine(outputPath, String.Format(@"{0}.jpg", pageNumber));
                                System.Drawing.Imaging.EncoderParameters parms = new System.Drawing.Imaging.EncoderParameters(1);
                                parms.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Compression, 0);
                                System.Drawing.Imaging.ImageCodecInfo jpegEncoder = GetEncoder(ImageFormat.Jpeg);// Utilities.GetImageEncoder("JPEG");
                                img.Save(path, jpegEncoder, parms);
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                pdf.Close();
                raf.Close();
            }


        }

        private static PdfObject FindImageInPDFDictionary(PdfDictionary pg)
        {
            PdfDictionary res =
                (PdfDictionary)PdfReader.GetPdfObject(pg.Get(PdfName.RESOURCES));


            PdfDictionary xobj =
              (PdfDictionary)PdfReader.GetPdfObject(res.Get(PdfName.XOBJECT));
            if (xobj != null)
            {
                foreach (PdfName name in xobj.Keys)
                {

                    PdfObject obj = xobj.Get(name);
                    if (obj.IsIndirect())
                    {
                        PdfDictionary tg = (PdfDictionary)PdfReader.GetPdfObject(obj);

                        PdfName type =
                          (PdfName)PdfReader.GetPdfObject(tg.Get(PdfName.SUBTYPE));

                        //image at the root of the pdf
                        if (PdfName.IMAGE.Equals(type))
                        {
                            return obj;
                        }// image inside a form
                        else if (PdfName.FORM.Equals(type))
                        {
                            return FindImageInPDFDictionary(tg);
                        } //image inside a group
                        else if (PdfName.GROUP.Equals(type))
                        {
                            return FindImageInPDFDictionary(tg);
                        }

                    }
                }
            }

            return null;

        }


        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        public static  void splitPDF1()
        {
            //string inputPdfPath = @"E:\Pessoal\Livros\Armazenando dados com Redis - Casa do Codigo.pdf";

            //ExtractImagesFromPDF(inputPdfPath, @"E:\Pessoal\Livros\img\");


            //int desired_x_dpi = 96;
            //int desired_y_dpi = 96;

            //string inputPdfPath = @"E:\Pessoal\Livros\1.pdf";
            //TiffImage oTiffImage = new TiffImage(inputPdfPath);

            //string outputDirectory = Environment.CurrentDirectory;
            ////ImageMagickNET.
            //try
            //{

            //    using (ImageList imageList = new ImageList())
            //    {
            //        imageList.ReadImages(inputPdfPath);
            //        int pageIndex = 0;
            //        foreach (Image page in imageList)
            //        {
            //            page.Write(outputDirectory + "\\Page." + pageIndex + ".jpg");
            //            pageIndex++;
            //        }
            //    }

            //    //using (Image firstPage = new Image())
            //    //{
            //    //    firstPage.Read(inputFile + "[0]");
            //    //    firstPage.Write(outputDirectory + "\\FirstPage.jpg");
            //    //}
            //    //_rasterizer.Close();
            //}
            //catch (Exception)
            //{
                
            //    throw;
            //}
          
        }
    }
}