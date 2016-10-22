using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PresentationBuilder.Controllers
{
    public class PresentationsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}