using IdentitySample.Models;
using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;
using ORCA30523.Models;
using System.Net;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace ORCA30523.Controllers
{
    public class TicketController : Controller
    {
        // GET: Post
        private ApplicationDbContext _dbContext;

        public TicketController()
        {
            _dbContext = new ApplicationDbContext();
        }
        // GET: Message
        [Authorize]
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page, string class2)
        {
            var manager = new UserManager<ApplicationUser>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var posts = from s in _dbContext.Tickets.Where(s => s.ToEmail.Equals(currentUser.Email) || s.FromEmail.Equals(currentUser.Email))
                        select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                posts = posts.Where(s => s.Subject.Contains(searchString)
                            || s.ToEmail.Contains(searchString) || s.FromEmail.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    posts = posts.OrderByDescending(s => s.ToEmail);
                    posts = posts.OrderByDescending(s => s.Subject);
                    break;
                case "Date":
                    posts = posts.OrderBy(s => s.DatePosted);
                    posts = posts.OrderBy(s => s.CreateDate);
                    break;
                case "date_desc":
                    posts = posts.OrderByDescending(s => s.DatePosted);
                    posts = posts.OrderByDescending(s => s.CreateDate);
                    break;
                default:
                    posts = posts.OrderBy(s => s.ToEmail);
                    break;
            }
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(posts.ToPagedList(pageNumber, pageSize));
        }

        public ViewResult Details(int? id, string receiver, string from1)
        {
            var adam = from s in _dbContext.Tickets.Where(s => s.ToEmail.Equals(from1) && s.FromEmail.Equals(receiver) || s.FromEmail.Equals(from1) && s.ToEmail.Equals(receiver))
                       select s;
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Ticket post = _dbContext.Tickets.Find(id);
            //if (post == null)
            //{
            //    return HttpNotFound();
            //}
            return View(adam);
        }
        public ActionResult Create(string class2)
        {
            ViewBag.ToEmail = class2;
            return View();
        }

        // GET: Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Ticket model)
        {
            //var manager = new UserManager<ApplicationUser>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<ApplicationUser>(new ApplicationDbContext()));
            //var currentUser = manager.FindById(User.Identity.GetUserId());
            //var user = User.Identity.GetUserName();
            var ticket = new Ticket
            {
                Body = model.Body,
                Subject = model.Subject,
                CreateDate = DateTime.Now.ToString(),
                FromEmail = User.Identity.GetUserName(),
                ToEmail = model.ToEmail,
                DatePosted = DateTime.Now.ToString()
            };
            if (ModelState.IsValid)
            {
                _dbContext.Tickets.Add(ticket);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ticket);
        }
        public ActionResult Respond(string receiver)
        {
            ViewBag.ToEmail = receiver;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Respond(Ticket post)
        {
            var respond = new Ticket
            {
                Body = post.Body,
                Subject = "RE:",
                FromEmail = User.Identity.GetUserName(),
                ToEmail = post.ToEmail,
                //CreateDate = DateTime.Now.ToString(),
                DatePosted = DateTime.Now.ToString()
            };

            if (ModelState.IsValid)
            {
                _dbContext.Tickets.Add(respond);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(respond);
        }

        //    public ActionResult Create([Bind(Include = "ToEmail,FromEmail,Subject,Body,DatePosted,LastDate")]Ticket post)
        //    {
        //        var manager = new UserManager<ApplicationUser>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<ApplicationUser>(new ApplicationDbContext()));
        //        var currentUser = manager.FindById(User.Identity.GetUserId());
        //        try
        //        {

        //           // if (ModelState.IsValid)
        //            //{
        //                _dbContext.Tickets.Add(post);
        //                _dbContext.SaveChanges();
        //                return RedirectToAction("Index"/*, routeValues: new { searchString = currentUser.Email }*/);
        //    //}
        //}
        //        catch (RetryLimitExceededException /* dex */)
        //        {
        //            //Log the error (uncomment dex variable name and add a line here to write a log.
        //            ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
        //        }

        //        return View(post);
        //    }
        public ActionResult Add(Ticket post)
        {
            _dbContext.Tickets.Add(post);
            _dbContext.SaveChanges();
            return RedirectToAction("Index"/*, routeValues: new { searchString = currentUser.Email }*/);
        }

        public ActionResult Edit(int id)
        {

            var post = _dbContext.Tickets.SingleOrDefault(v => v.ID.Equals(id));

            if (post == null)
                return HttpNotFound();

            return View(post);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            var manager = new UserManager<ApplicationUser>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var postToUpdate = _dbContext.Tickets.Find(id);
            if (TryUpdateModel(postToUpdate, "",
               new string[] { "Subject", "Body", "ToEmail", "FromEmail", "CreateDate", "DatePosted" }))
            {
                try
                {
                    _dbContext.SaveChanges();

                    return RedirectToAction("Index" /*, routeValues: new { searchString = currentUser.Email}*/);
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(postToUpdate);
        }


        [HttpPost]
        public ActionResult Update(Ticket post)
        {
            var manager = new UserManager<ApplicationUser>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            var postInDb = _dbContext.Tickets.SingleOrDefault(v => v.ID.Equals(post.ID));

            if (postInDb == null)
                return HttpNotFound();

            postInDb.Subject = post.Subject;
            postInDb.Body = post.Body;
            postInDb.ToEmail = post.ToEmail;
            postInDb.FromEmail = post.FromEmail;
            postInDb.DatePosted = post.DatePosted;
            postInDb.CreateDate = post.CreateDate;
            _dbContext.SaveChanges();

            return RedirectToAction("Index"/*, routeValues: new { searchString = currentUser.Email }*/);
        }



        public ActionResult Delete(int id)
        {
            var post = _dbContext.Tickets.SingleOrDefault(v => v.ID.Equals(id));

            if (post == null)
                return HttpNotFound();

            return View(post);
        }

        [HttpPost]
        public ActionResult DoDelete(int id)
        {
            var manager = new UserManager<ApplicationUser>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            var post = _dbContext.Tickets.SingleOrDefault(v => v.ID.Equals(id));

            if (post == null)
                return HttpNotFound();

            _dbContext.Tickets.Remove(post);
            _dbContext.SaveChanges();

            return RedirectToAction("Index"/*,routeValues: new { searchString = currentUser.Email }*/);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}