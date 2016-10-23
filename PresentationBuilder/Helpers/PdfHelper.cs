using GhostscriptSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PresentationBuilder.Helpers
{
    public class PdfHelper
    {

        public static string pathTemp()
        {
            string path = HttpContext.Current.Server.MapPath("~/");

            if (path.EndsWith("\\"))
            {
                path = path.Substring(0, path.Length - 1);
            }

            path = path.Substring(0, path.LastIndexOf("\\")) + "\\PresentationBuilderDocuments\\ImageTemp\\";

            return path;
        }

        /// <summary>
        /// Split PDF file into images with File pattern name OUTPUT{0}.jpg Total Page to export to image = 100;
        /// </summary>
        /// <param name="path">PDF path source</param>
        /// <param name="output">Images PDF output</param>
        /// <returns></returns>
        public static void splitToImages(string path, string outputPath)
        {
            splitToImages(path, outputPath, "IMG_OUTPUT%d.jpg", 20);
        }


        /// <summary>
        /// Split PDF file into images
        /// </summary>
        /// <param name="path">PDF path source</param>
        /// <param name="output">Images PDF output</param>
        /// <param name="filename">File pattern name OUTPUT{0}.jpg</param>
        /// <param name="multipleFilePageCount">Total Page to export to image</param>
        /// <returns></returns>
        public static void splitToImages(string path, string outputPath, string filename = "IMG_OUTPUT%d.jpg", int multipleFilePageCount = 100)
        {
            try
            {
                if (!Directory.Exists(outputPath))
                    Directory.CreateDirectory(outputPath);                

                GhostscriptWrapper.GeneratePageThumbs(path, Path.Combine(outputPath, filename), 1, multipleFilePageCount, 100, 100);
            }
            catch (Exception exc)
            {
               
                //throw;
            }
        }
    }
}