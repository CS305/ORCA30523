using IdentitySample.Models;
using Microsoft.AspNet.Identity.Owin;
using PagedList;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace IdentitySample.Controllers
{
    public class UsersAdminController : Controller
    {
        private ApplicationDbContext _dbContext; 
        public UsersAdminController()
        {
            _dbContext = new ApplicationDbContext();
        }

        public UsersAdminController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        private ApplicationUserManager _userManager;
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

        //
        // GET: /Users/
        public async Task<ActionResult> Index()
        {
            var experts = from m in _dbContext.Users.Where(m => m.register != 0 && !m.Roles.Select(y => y.RoleId).Contains("ea07d5c7-466b-4c48-ac1d-3df199ae8cd8"))
                          select m;
            //if (!string.IsNullOrEmpty(stringName))
            //{
            //    experts = experts.Where(s => s.register.ToString().Equals(stringName));
            //}
            return View(await experts.ToListAsync());
        }

        public ViewResult Index2(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "email" : "";
            ViewBag.NameSortParm2 = sortOrder == "firstName2" ? "firstName" : "firstName2";
            ViewBag.NameSortParm3 = sortOrder == "lastName2" ? "lastName" : "lastName2";
            ViewBag.NameSortParm4 = sortOrder == "register2" ? "register" : "register2";
            //ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "register_desc" : "Register";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            var experts2 = from t in _dbContext.Users
                           select t;
            if (!String.IsNullOrEmpty(searchString))
            {
                experts2 = experts2.Where(s => s.lastName.Contains(searchString) || s.firstName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "email":
                    experts2 = experts2.OrderByDescending(s => s.Email);
                    break;
                case "firstName2":
                    experts2 = experts2.OrderBy(s => s.firstName);
                    break;
                case "firstName":
                    experts2 = experts2.OrderByDescending(s => s.firstName);
                    break;
                case "lastName2":
                    experts2 = experts2.OrderBy(s => s.lastName);
                    break;
                case "lastName":
                    experts2 = experts2.OrderByDescending(s => s.lastName);
                    break;
                case "register2":
                    experts2 = experts2.OrderBy(s => s.register);
                    break;
                case "register":
                    experts2 = experts2.OrderByDescending(s => s.register);
                    break;
                //case "register_desc":
                //experts2 = experts2.OrderBy(s => s.register);
                //break;
                //case "Register":
                //experts2 = experts2.OrderByDescending(s => s.register);
                //break;
                default:
                    experts2 = experts2.OrderBy(s => s.Email);
                    break;
            }
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(experts2.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /Users/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);

            ViewBag.RoleNames = await UserManager.GetRolesAsync(user.Id);

            return View(user);
        }

        //
        // GET: /Users/Create
        public async Task<ActionResult> Create()
        {
            //Get the list of Roles
            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
            return View();
        }

        //
        // POST: /Users/Create
        [HttpPost]
        public async Task<ActionResult> Create(RegisterViewModel userViewModel, params string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = userViewModel.Email,
                    Email = userViewModel.Email,
                    Address = userViewModel.Address,
                    State = userViewModel.State,
                    City = userViewModel.City,
                    PostalCode = userViewModel.PostalCode,
                    register = userViewModel.register,
                    number = userViewModel.number,
                    prefix = userViewModel.prefix,
                    isDeleted = userViewModel.isDeleted,
                    expertise = userViewModel.expertise,
                    expertise2 = userViewModel.expertise2,
                    expertise3 = userViewModel.expertise3
                };
                user.Address = userViewModel.Address;
                user.City = userViewModel.City;
                user.State = userViewModel.State;
                user.PostalCode = userViewModel.PostalCode;
                user.firstName = userViewModel.firstName;
                user.lastName = userViewModel.lastName;
                user.number = userViewModel.number;
                user.register = userViewModel.register;
                user.prefix = userViewModel.prefix;
                user.isDeleted = userViewModel.isDeleted;
                user.expertise3 = userViewModel.expertise3;
                user.expertise2 = userViewModel.expertise2;
                user.expertise = userViewModel.expertise;
                var adminresult = await UserManager.CreateAsync(user, userViewModel.Password);

                //Add User to the selected Roles 
                if (adminresult.Succeeded)
                {
                    if (selectedRoles != null)
                    {
                        var result = await UserManager.AddToRolesAsync(user.Id, selectedRoles);
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", result.Errors.First());
                            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
                            return View();
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", adminresult.Errors.First());
                    ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
                    return View();

                }
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
            return View();
        }

        //
        // GET: /Users/Edit/1
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var userRoles = await UserManager.GetRolesAsync(user.Id);

            return View(new EditUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                Address = user.Address,
                City = user.City,
                State = user.State,
                PostalCode = user.PostalCode,
                firstName = user.firstName,
                lastName = user.lastName,
                number = user.number,
                register = user.register,
                prefix = user.prefix,
                isDeleted = user.isDeleted,
                expertise = user.expertise,
                expertise2 = user.expertise2,
                expertise3 = user.expertise3,
                RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
                {
                    Selected = userRoles.Contains(x.Name),
                    Text = x.Name,
                    Value = x.Name
                })
            });
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Email,Id,prefix, Address, City, State,PostalCode,firstName,lastName,number,register, isDeleted, expertise,expertise2,expertise3")] EditUserViewModel editUser, params string[] selectedRole)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(editUser.Id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                user.UserName = editUser.Email;
                user.Email = editUser.Email;
                user.Address = editUser.Address;
                user.City = editUser.City;
                user.State = editUser.State;
                user.PostalCode = editUser.PostalCode;
                user.firstName = editUser.firstName;
                user.lastName = editUser.lastName;
                user.number = editUser.number;
                user.register = editUser.register;
                user.prefix = editUser.prefix;
                user.isDeleted = editUser.isDeleted;
                user.expertise3 = editUser.expertise3;
                user.expertise2 = editUser.expertise2;
                user.expertise = editUser.expertise;

                var userRoles = await UserManager.GetRolesAsync(user.Id);

                selectedRole = selectedRole ?? new string[] { };

                var result = await UserManager.AddToRolesAsync(user.Id, selectedRole.Except(userRoles).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                if (User.IsInRole("Admin"))
                {
                    result = await UserManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRole).ToArray<string>());
                }

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index2");
                }
                else
                {
                    return RedirectToAction("Index2", new RouteValueDictionary(new { controller = "Home", action = "Index2" }));
                }
            }
            ModelState.AddModelError("", "Something failed.");
            return View();
        }

        //
        // GET: /Users/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = await UserManager.FindByIdAsync(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                var result = await UserManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}