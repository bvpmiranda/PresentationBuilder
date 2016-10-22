using System;
using System.Collections.Generic;
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
            return View();
        }

		[Authorize]
		public ActionResult Presentation(int id)
		{
			return View(new PresentationBuilder.Models.Presentation { PresentationId = id });
		}

		[Authorize]
		[HttpPost]
		public ActionResult UploadZip(HttpPostedFileBase file)
		{
			var context = new PresentationBuilder.Models.PresentationBuilderEntities();

			var UserId = (from u in context.AspNetUsers where u.UserName == User.Identity.Name select u.Id).First();


			int id = PresentationBuilder.Helpers.ZipHelper.unzipPresentation(file.InputStream, UserId);

			return RedirectToAction("Presentation", new { id = id });
		}
    }
}