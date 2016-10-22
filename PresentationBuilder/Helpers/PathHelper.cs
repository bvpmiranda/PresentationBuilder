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
	public class PathHelper
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
	}
}