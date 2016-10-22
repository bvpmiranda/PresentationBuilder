using PresentationBuilder.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using System.Net.Http;

namespace PresentationBuilder.Helpers
{
	public class ZipPresentation
	{
		public static HttpResponseMessage zipPresentation(int PresentationId)
		{
			var context = new PresentationBuilderEntities();

			var presentation = (from p in context.Presentations.Include("AspNetUser").Include("PresentationPages") where p.PresentationId == PresentationId select p).First();

			return zipPresentation(presentation);
		}

		public static HttpResponseMessage zipPresentation(Presentation Presentation)
		{
			HttpResponseMessage response = new System.Net.Http.HttpResponseMessage(HttpStatusCode.OK);

			var zip = new Ionic.Zip.ZipFile();

			byte[] fileBytes;

			string path = HttpContext.Current.Server.MapPath("~/");

			if (path.EndsWith("\\"))
			{
				path = path.Substring(0, path.Length - 1);
			}

			path = path.Substring(0, path.LastIndexOf("\\")) + "\\PresentationBuilderDocuments\\" + Presentation.PresentationId.ToString() + "\\";

			foreach (var page in Presentation.PresentationPages)
			{
				if (!page.Hidden)
				{
					fileBytes = System.IO.File.ReadAllBytes(path + page.ImagePath);

					zip.AddEntry(page.ImagePath, fileBytes);

					if (page.SoundPath != null)
					{
						fileBytes = System.IO.File.ReadAllBytes(path + page.SoundPath);

						zip.AddEntry(page.SoundPath, fileBytes);
					}
				}
			}

			var manifest = new
			{
				author = Presentation.AspNetUser.FirstName + " " + Presentation.AspNetUser.LastName,
				date = Presentation.Date,
				name = Presentation.Name,
				description = Presentation.Description,
				page = (from p in Presentation.PresentationPages
						where !p.Hidden
						orderby p.Order
						select new
						{
							order = p.Order,
							image = p.ImagePath,
							sound = p.SoundPath,
							soundLength = (p.SoundLength.HasValue ? p.SoundLength.Value.ToString("n0") : null)
						}).ToList()
			};

			string manifestFile = JsonConvert.SerializeObject(manifest);

			zip.AddEntry("manifest.json", manifestFile);

			var zipStream = new MemoryStream();
			zip.Save(zipStream);
			zipStream.Seek(0, SeekOrigin.Begin);

			response.Content = new System.Net.Http.StreamContent(zipStream);

			response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-zip-compressed");
			response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
			response.Content.Headers.ContentDisposition.FileName = Presentation.Name.Trim() + ".zip";

			return response;
		}
	}
}