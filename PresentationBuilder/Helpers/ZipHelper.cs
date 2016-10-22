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
	public class ZipHelper
	{
		public static string path()
		{
			string path = HttpContext.Current.Server.MapPath("~/");

			if (path.EndsWith("\\"))
			{
				path = path.Substring(0, path.Length - 1);
			}

			path = path.Substring(0, path.LastIndexOf("\\")) + "\\PresentationBuilderDocuments\\";

			return path;
		}

		public static HttpResponseMessage zipPresentation(int PresentationId)
		{
			HttpResponseMessage response = new System.Net.Http.HttpResponseMessage(HttpStatusCode.OK);

			var context = new PresentationBuilderEntities();

			var presentation = (from p in context.Presentations.Include("AspNetUser").Include("PresentationPages") where p.PresentationId == PresentationId select p).First();

			var zip = new Ionic.Zip.ZipFile();

			byte[] fileBytes;

			string path = PathHelper.path() + presentation.PresentationId.ToString() + "\\";

			foreach (var page in presentation.PresentationPages)
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

			var manifest = new PresentationManifest
			{
				author = presentation.AspNetUser.FirstName + " " + presentation.AspNetUser.LastName,
				date = presentation.Date,
				name = presentation.Name,
				description = presentation.Description,
				pages = (from p in presentation.PresentationPages
						 where !p.Hidden
						 orderby p.Order
						 select new PresentationManifest.Page
						 {
							 order = p.Order,
							 image = p.ImagePath,
							 sound = p.SoundPath,
							 soundLength = p.SoundLength
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
			response.Content.Headers.ContentDisposition.FileName = presentation.Name.Trim() + ".zip";

			return response;
		}

		public static Presentation unzipPresentation(HttpPostedFileBase file, string UserId)
		{
			if (file.ContentType == "application/x-zip-compressed")
			{
				Stream stream = file.InputStream;

				Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(stream);

				Ionic.Zip.ZipEntry entry;

				entry = (from e in zip.Entries where e.FileName == "manifest.json" select e).FirstOrDefault();

				if (entry == null)
				{
					throw new Exception("Manifest Missing");
				}

				MemoryStream msManifest = new MemoryStream();
				StreamReader srManifest;
				string manifestString;
				PresentationManifest manifest;

				entry.Extract(msManifest);
				msManifest.Position = 0;

				srManifest = new StreamReader(msManifest);
				manifestString = srManifest.ReadToEnd();

				manifest = JsonConvert.DeserializeObject<PresentationManifest>(manifestString);

				Presentation presentation = new Presentation
				{
					UserId = UserId,
					Date = manifest.date,
					Name = manifest.name,
					Description = manifest.description
				};

				var context = new PresentationBuilder.Models.PresentationBuilderEntities();

				context.Presentations.Add(presentation);
				context.SaveChanges();

				string path = PathHelper.path() + presentation.PresentationId.ToString() + "\\";

				System.IO.Directory.CreateDirectory(path);

				foreach (var page in manifest.pages)
				{
					presentation.PresentationPages.Add(new PresentationPage
					{
						Order = page.order,
						ImagePath = page.image,
						SoundPath = page.sound,
						SoundLength = page.soundLength
					});

					entry = (from e in zip.Entries where e.FileName == page.image select e).First();
					entry.Extract(path);

					if (!String.IsNullOrWhiteSpace(page.sound))
					{
						entry = (from e in zip.Entries where e.FileName == page.sound select e).First();
						entry.Extract(path + page.sound);
					}
				}

				context.SaveChanges();

				zip = null;

				return presentation;
			}
			else
			{
				throw new Exception("Invalid File");
			}
		}
	}
}