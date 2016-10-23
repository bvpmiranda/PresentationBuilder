using PresentationBuilder.Helpers;
using PresentationBuilder.Models;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;

namespace PresentationBuilder.Controllers
{
	public class PresentationsController : Controller
	{
		private ApplicationUserManager _userManager;

		public PresentationsController()
		{
		}

		public PresentationsController(ApplicationUserManager userManager)
		{
			UserManager = userManager;
		}

		public ApplicationUserManager UserManager
		{
			get
			{
				return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			}
			private set
			{
				_userManager = value;
			}
		}

		[Authorize]
		public ActionResult Index()
		{
			var context = new PresentationBuilderEntities();

			var viewModel = new PresentationViewModel();

			viewModel.Presentations = (from p in context.Presentations.Include("AspNetUser")
									   where p.AspNetUser.UserName == User.Identity.Name
									   orderby p.Name
									   select p).ToList();

			viewModel.UserInfo = UserManager.FindById(User.Identity.GetUserId()) ?? new ApplicationUser { PasswordHash = null };

			return View(viewModel);
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
        [ValidateInput(false)]        
        public ActionResult AddPresentation(Presentation model)
        {
            if (ModelState.IsValid)
            {
                var context = new PresentationBuilderEntities();

                //context.Entry(model).State = System.Data.Entity.EntityState.Modified;

                //context.SaveChanges();

                Presentation p = context.Presentations.FirstOrDefault(x => x.PresentationId == model.PresentationId);
                p.Description = model.Description;
                p.Name = model.Name;
                //        f.Name = NewName;
                //        public int PresentationId { get; set; }
                //public string UserId { get; set; }
                //public string Name { get; set; }
                //public System.DateTime Date { get; set; }

                //public string Description { get; set; }

                context.SaveChanges();

                return RedirectToAction("Index", new { Message = "Presentation Saved !" });
            }

            return View(model);
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

						System.IO.File.Delete(Path.Combine(pathWork, file.FileName));

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

				newPresentationPage.PresentationPageId = 0;

				context.Entry(newPresentationPage).State = System.Data.Entity.EntityState.Unchanged;

				context.PresentationPages.Add(newPresentationPage);
			}

			context.SaveChanges();
		}


		[Authorize]
		[HttpPost]
		public ActionResult ChangeOrder(string presentationPageId, string newOrder)
		{
			var uploadReturn = new UploadReturn();

			try
			{
				if (string.IsNullOrEmpty(presentationPageId))
				{
					uploadReturn.uploadStatus = uploadStatus.Error;
					uploadReturn.message = "presentationPageId is null";
				}
				else
				{
					int intPresentationPageId = Convert.ToInt32(presentationPageId);

					var context = new PresentationBuilderEntities();

					var presentationPage = (from p in context.PresentationPages where p.PresentationPageId == intPresentationPageId select p).First();

					presentationPage.Order = Convert.ToByte(newOrder);

					context.SaveChanges();

					return Json(new { Message = "Order Changed" });
				}


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