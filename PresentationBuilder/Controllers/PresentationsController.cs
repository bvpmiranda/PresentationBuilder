using PresentationBuilder.Helpers;
using PresentationBuilder.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PresentationBuilder.Controllers
{
    public class PresentationsController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            var context = new PresentationBuilderEntities();

            var presentations = (from p in context.Presentations.Include("AspNetUser")
                                 where p.AspNetUser.UserName == User.Identity.Name
                                 orderby p.Name
                                 select p).ToList();

            return View(presentations);
        }

        [Authorize]
        public ActionResult Presentation(int id)
        {
            var context = new PresentationBuilderEntities();

            var presentation = (from p in context.Presentations.Include("PresentationPages") where p.PresentationId == id select p).First();


            return View(presentation);
        }

        [Authorize]
        [HttpPost]
        public ActionResult UploadZipAsync()
        {
            var uploadReturn = new UploadReturn();

            var context = new PresentationBuilderEntities();

            var UserId = (from u in context.AspNetUsers where u.UserName == User.Identity.Name select u.Id).First();

            try
            {
                var Presentation = Helpers.ZipHelper.unzipPresentation(Request.Files[0], UserId);

                uploadReturn.data = new Presentation
                {
                    PresentationId = Presentation.PresentationId,
                    Name = Presentation.Name
                };
            }
            catch (Exception ex)
            {
                uploadReturn.uploadStatus = uploadStatus.Error;
                uploadReturn.message = ex.Message;
            }

            return Json(uploadReturn, "text/plain");
        }

        [Authorize]
        [HttpPost]
        public ActionResult SaveUploadedFile()
        {
            var uploadReturn = new UploadReturn();

            string fName = "";

            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    //Save file content goes here
                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileNameUpload = Path.GetFileName(file.FileName);

                        //Create Presentation
                        var context = new PresentationBuilderEntities();

                        var UserId = (from u in context.AspNetUsers where u.UserName == User.Identity.Name select u.Id).First();

                        Presentation presentation = new Presentation
                        {
                            UserId = UserId,
                            Date = DateTime.Now,
                            Name = "New Presentation",
                            Description = null
                        };

                        context.Presentations.Add(presentation);
                        context.SaveChanges();

                        uploadReturn.data = presentation;

                        string pathWork = Path.Combine(ZipHelper.path(), presentation.PresentationId.ToString());

                        if (!Directory.Exists(pathWork))
                            Directory.CreateDirectory(pathWork);



                        file.SaveAs(Path.Combine(pathWork, file.FileName));

                        PdfHelper.splitToImages(Path.Combine(pathWork, file.FileName), pathWork);


                        byte bOrder = 0;

                        foreach (string filePresentaion in Directory.EnumerateFiles(pathWork, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".jpg")))
                        {
                            presentation.PresentationPages.Add(new PresentationPage
                            {
                                Order = bOrder++,
                                ImagePath = Path.GetFileName(filePresentaion),
                                SoundPath = null,
                                SoundLength = null
                            });
                        }

                        context.SaveChanges();

                        return Json(new { Message = presentation.PresentationId });
                    }
                }
            }
            catch (Exception ex)
            {
                uploadReturn.uploadStatus = uploadStatus.Error;
                uploadReturn.message = ex.Message;
            }


            return Json(uploadReturn, "text/plain");
        }

        public void DuplicatePresentation(int id)
        {

            var context = new PresentationBuilderEntities();

            var currentPresentation = (from p in context.Presentations.Include("PresentationPages") where p.PresentationId == id select p).First();

            var newPresentation = currentPresentation;

            newPresentation.PresentationId = 0;

            context.Entry(newPresentation).State = System.Data.Entity.EntityState.Unchanged;

            context.Presentations.Add(newPresentation);

            context.SaveChanges();

            foreach (var oPresentationPage in currentPresentation.PresentationPages)
            {
                var newPresentationPage = oPresentationPage;

                newPresentationPage.PresentationId = newPresentation.PresentationId;

                context.Entry(newPresentationPage).State = System.Data.Entity.EntityState.Unchanged;

                context.PresentationPages.Add(newPresentationPage);
            }

            context.SaveChanges();
        }
    }
}