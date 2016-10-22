using PresentationBuilder.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace PresentationBuilder.APIs
{
    public class PdfAPIController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage split(int id)
        {
			//if (PdfHelper.splitToImages())
			//	return new HttpResponseMessage(HttpStatusCode.OK);

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}