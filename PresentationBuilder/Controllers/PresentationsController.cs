using PresentationBuilder.Models;
using System;
using System.Linq;
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
			return View(new Presentation { PresentationId = id });
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
	}
}