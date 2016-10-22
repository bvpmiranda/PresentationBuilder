using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
//using PresentationBuilder.Core.Util;
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
            //PDF.splitPDF1();
           
            //Assert.IsTrue(valueTotal == 12.50M, "The sale total is wrong  - " + valueTotal.ToString());
        }


      
    }

    [TestClass]
    public class GhostscriptSharpTests
    {
        //string inputPdfPath = @"E:\Pessoal\Livros\Armazenando dados com Redis - Casa do Codigo.pdf";

        //ExtractImagesFromPDF(inputPdfPath, @"E:\Pessoal\Livros\img\");

        private readonly string TEST_FILE_LOCATION = @"E:\Pessoal\Livros\Armazenando dados com Redis - Casa do Codigo.pdf";
        private readonly string SINGLE_FILE_LOCATION = @"E:\Pessoal\Livros\img\output.jpg";
        private readonly string MULTIPLE_FILE_LOCATION = @"E:\Pessoal\Livros\img\output%d.jpg";

        private readonly int MULTIPLE_FILE_PAGE_COUNT = 20;

        [TestMethod]
        public void GenerateSinglePageThumbnail()
        {
            GhostscriptWrapper.GeneratePageThumb(TEST_FILE_LOCATION, SINGLE_FILE_LOCATION, 1, 100, 100);
            Assert.IsTrue(File.Exists(SINGLE_FILE_LOCATION));
        }

        [TestMethod]
        public void GenerateMultiplePageThumbnails()
        {
            GhostscriptWrapper.GeneratePageThumbs(TEST_FILE_LOCATION, MULTIPLE_FILE_LOCATION, 1, MULTIPLE_FILE_PAGE_COUNT, 100, 100);
            for (var i = 1; i <= MULTIPLE_FILE_PAGE_COUNT; i++)
                Assert.IsTrue(File.Exists(String.Format("output{0}.jpg", i)));
        }

    }
}
