using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PresentationBuilder
{
	/// <summary>
	/// Summary description for image
	/// </summary>
	public class image : IHttpHandler
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
							where p.PresentationId == PresentationPageId &&
								  p.Presentation.AspNetUser.UserName == userName
							select new
							{
								PresentationId = p.PresentationId,
								ImagePath = p.ImagePath
							}).FirstOrDefault();

				if (page != null)
				{
					path = System.IO.Path.Combine(path, page.PresentationId.ToString(), page.ImagePath);

					context.Response.Clear();

					context.Response.ContentType = "image/" + System.IO.Path.GetExtension(path).Substring(1);
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