using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PresentationBuilder
{
	/// <summary>
	/// Summary description for image
	/// </summary>
	public class sound : IHttpHandler
	{

		public void ProcessRequest(HttpContext context)
		{
			if (context.Request.IsAuthenticated)
			{
				int PresentationPageId = Convert.ToInt32(context.Request.QueryString.Get("Id"));

				String path = PresentationBuilder.Helpers.PathHelper.path();

				var entities = new PresentationBuilder.Models.PresentationBuilderEntities();

				var userName = HttpContext.Current.User.Identity.Name;

				var page = (from p in entities.PresentationPages
							where p.PresentationPageId == PresentationPageId &&
								  p.Presentation.AspNetUser.UserName == userName &&
								  p.SoundPath.Trim().Length > 0
							select new
							{
								PresentationId = p.PresentationId,
								SoundPath = p.SoundPath
							}).FirstOrDefault();

				if (page != null)
				{
					path = System.IO.Path.Combine(path, page.PresentationId.ToString(), page.SoundPath);

					context.Response.Clear();

					context.Response.ContentType = "audio/basic";
					context.Response.AddHeader("content-disposition", "inline;filename=" + System.IO.Path.GetFileName(path));
					context.Response.WriteFile(path);
					context.Response.End();
				}
				else
				{
					context.Response.StatusCode = Convert.ToInt32(System.Net.HttpStatusCode.NotFound);
					context.Response.End();
				}
			}
			else
			{
				context.Response.StatusCode = Convert.ToInt32(System.Net.HttpStatusCode.Unauthorized);
				context.Response.End();
			}
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}