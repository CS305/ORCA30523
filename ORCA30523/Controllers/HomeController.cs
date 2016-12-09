using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using IdentitySample.Models;
using System;
using System.Linq;
using PagedList;

namespace IdentitySample.Controllers
{
    [Authorize]
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private ApplicationDbContext db;

        public HomeController()
        {
            db = new ApplicationDbContext();
        }

        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var experts = from s in db.Users.Where(s => s.Roles.Select(y => y.RoleId).Contains("ea07d5c7-466b-4c48-ac1d-3df199ae8cd8"))
                          select s;
            if (!String.IsNullOrEmpty(searchString)) 
            {
                experts = experts.Where(s => s.lastName.Contains(searchString) || s.firstName.Contains(searchString) || 
                s.expertise.Contains(searchString) || s.expertise2.Contains(searchString) || s.expertise3.Contains(searchString));
            }
            switch (sortOrder)
            { 
                case "name_desc":
                    experts = experts.OrderByDescending(s => s.lastName);
                    break;
                default:
                    experts = experts.OrderBy(s => s.lastName);
                    break;
            }
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(experts.ToPagedList(pageNumber, pageSize));
        }
        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;


        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        public HomeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
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
    }
}