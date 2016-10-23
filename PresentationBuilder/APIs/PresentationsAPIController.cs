using PresentationBuilder.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Net;
using System.IO;

namespace PresentationBuilder.APIs
{
	public class PresentationsAPIController : ApiController
	{
		[HttpPost]
		public HttpResponseMessage download(int id)
		{
			return ZipHelper.zipPresentation(id);
		}

		[HttpPost]
		public Models.JsonReturn delete(int id)
		{
			var jsonReturn = new Models.JsonReturn();

			try
			{
				var context = new PresentationBuilder.Models.PresentationBuilderEntities();

				var presentation = (from p in context.Presentations where p.PresentationId == id select p).First();

				context.Presentations.Remove(presentation);

                var presentationPath = Path.Combine(PathHelper.path(), presentation.PresentationId.ToString());
                
                DirectoryInfo di = new DirectoryInfo( presentationPath );

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
                Directory.Delete(presentationPath);
                
				context.SaveChanges();
			}
			catch (Exception ex)
			{
				jsonReturn.isValid = false;
				jsonReturn.messages.Add(ex.Message);
			}

			return jsonReturn;
		}

		[HttpPost]
		public Models.JsonReturn deletePage(int id)
		{
			var jsonReturn = new Models.JsonReturn();

			try
			{
				var context = new PresentationBuilder.Models.PresentationBuilderEntities();

				var presentationPage = (from p in context.PresentationPages where p.PresentationPageId == id select p).First();

				context.PresentationPages.Remove(presentationPage);

				System.IO.File.Delete(System.IO.Path.Combine(PathHelper.path(), presentationPage.PresentationId.ToString(), presentationPage.ImagePath));

				if (presentationPage.SoundPath != null && presentationPage.SoundPath.Trim().Length > 0)
				{
					System.IO.File.Delete(System.IO.Path.Combine(PathHelper.path(), presentationPage.PresentationId.ToString(), presentationPage.SoundPath));
				}

				context.SaveChanges();
			}
			catch (Exception ex)
			{
				jsonReturn.isValid = false;
				jsonReturn.messages.Add(ex.Message);
			}

			return jsonReturn;
		}

		[HttpPost]
		public Models.JsonReturn deleteAudio(int id)
		{
			var jsonReturn = new Models.JsonReturn();

			try
			{
				var context = new PresentationBuilder.Models.PresentationBuilderEntities();

				var presentationPage = (from p in context.PresentationPages where p.PresentationPageId == id select p).First();

				System.IO.File.Delete(System.IO.Path.Combine(PathHelper.path(), presentationPage.PresentationId.ToString(), presentationPage.SoundPath));

				presentationPage.SoundPath = null;

				context.SaveChanges();
			}
			catch (Exception ex)
			{
				jsonReturn.isValid = false;
				jsonReturn.messages.Add(ex.Message);
			}

			return jsonReturn;
		}

		[HttpPost]
		public Models.JsonReturn save(PresentationBuilder.Models.Presentation model)
		{
			var jsonReturn = new Models.JsonReturn();

			if (ModelState.IsValid)
			{
				var context = new PresentationBuilder.Models.PresentationBuilderEntities();

				context.Entry(model).State = System.Data.Entity.EntityState.Modified;

				context.SaveChanges();
			}
			else
			{
				jsonReturn.isValid = false;
			}

			return jsonReturn;
		}
	}
}