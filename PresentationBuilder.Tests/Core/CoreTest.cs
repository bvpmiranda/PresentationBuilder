using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using GhostscriptSharp;
using System.IO;


namespace PracticalDeveloperTest.Tests
{
    [TestClass]
    public class UnitTestRequirements
    {
        [TestMethod]
        public void Example4()
        {
        }

        [TestClass]
        public class ZipHelperTests
        {

            private readonly string TEST_FILE_LOCATION = @"E:\Pessoal\Livros\Pessoal.pdf";
            private readonly string SINGLE_FILE_LOCATION = @"E:\Pessoal\Livros\img\output.jpg";
            private readonly string MULTIPLE_FILE_LOCATION = @"E:\Pessoal\Livros\img\output%d.jpg";
            private readonly string OUTPUT_FOLDER = @"E:\Pessoal\Livros\img\";

            private readonly int MULTIPLE_FILE_PAGE_COUNT = 2000;

            [TestMethod]
            public void GenerateSinglePageThumbnail()
            {
                //PresentationBuilder.Helpers.PdfHelper.splitToImages(TEST_FILE_LOCATION, OUTPUT_FOLDER);


                //for (var i = 1; i <= MULTIPLE_FILE_PAGE_COUNT; i++)
                //    Assert.IsTrue(File.Exists(String.Format("output{0}.jpg", i)));


                //GhostscriptWrapper.GeneratePageThumb(TEST_FILE_LOCATION, SINGLE_FILE_LOCATION, 1, 100, 100);
                //Assert.IsTrue(File.Exists(SINGLE_FILE_LOCATION));
            }

            [TestMethod]
            public void GenerateMultiplePageThumbnails()
            {
                int iTotal = GetNumPages(TEST_FILE_LOCATION);
                GhostscriptWrapper.GeneratePageThumbs(TEST_FILE_LOCATION, MULTIPLE_FILE_LOCATION, 1, MULTIPLE_FILE_PAGE_COUNT, 100, 100);
                for (var i = 1; i <= MULTIPLE_FILE_PAGE_COUNT; i++)
                    Assert.IsTrue(File.Exists(String.Format("output{0}.jpg", i)));
            }

            public static int GetNumPages(string path)
            {
                if (path != null)
                {
                    try
                    {
                        using (var stream = new StreamReader(File.OpenRead(path)))
                        {
                            var regex = new System.Text.RegularExpressions.Regex(@"/Type\s*/Page[^s]");
                            var matches = regex.Matches(stream.ReadToEnd());

                            return matches.Count;
                        }
                    }
                    catch (Exception e)
                    {
                        return 0;
                    }
                }
                return 0;
            }

        }
    }
}
