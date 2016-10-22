using PresentationBuilder.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace PresentationBuilder.APIs
{
	public class AccountAPIController : ApiController
	{

		[HttpPost]
		public HttpResponseMessage download(int id)
		{
			return ZipPresentation.zipPresentation(id);
		}

	}
}