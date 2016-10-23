using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PresentationBuilder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PresentationBuilder.Controllers
{
	public class HomeController : Controller
	{

        private ApplicationUserManager _userManager;

        public HomeController()
        {
        }

        public HomeController(ApplicationUserManager userManager)
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

        public ActionResult Index()
		{
			if (Request.IsAuthenticated)
			{
				return RedirectToAction("Index", "Presentations");
			}
			else
			{
				return RedirectToAction("Login", "Account");
			}
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

            var info = new AboutViewModel();
            info.UserInfo = UserManager.FindById(User.Identity.GetUserId()) ?? new ApplicationUser { Id = null };

            return View(info);
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}