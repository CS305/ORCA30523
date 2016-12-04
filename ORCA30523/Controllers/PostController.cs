using IdentitySample.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ORCA30523.Models;
using System.Net;
using System.Data;
using System.Data.Entity.Infrastructure;

namespace ORCA30523.Controllers
{
    public class PostController : Controller
    {
        // GET: Post
        private ApplicationDbContext _dbContext;
        public PostController()
        {
            _dbContext = new ApplicationDbContext();
        }
        // GET: Message
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
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
            var posts = from s in _dbContext.Posts
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                posts = posts.Where(s => s.ToEmail.Contains(searchString)
                                       || s.FromEmail.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    posts = posts.OrderByDescending(s => s.ToEmail);
                    break;
                case "Date":
                    posts = posts.OrderBy(s => s.DatePosted);
                    break;
                case "date_desc":
                    posts = posts.OrderByDescending(s => s.DatePosted);
                    break;
                default:
                    posts = posts.OrderBy(s => s.ToEmail);
                    break;
            }
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(posts.ToPagedList(pageNumber, pageSize));
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Subject,Body,ToEmail,FromEmail")] Post post)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    _dbContext.Posts.Add(post);
                    _dbContext.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(post);
        }
        public ActionResult Add(Post post)
        {
            _dbContext.Posts.Add(post);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var post = _dbContext.Posts.SingleOrDefault(v => v.PostID.Equals(id));

            if (post == null)
                return HttpNotFound();

            return View(post);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var postToUpdate = _dbContext.Posts.Find(id);
            if (TryUpdateModel(postToUpdate, "",
               new string[] { "Subject", "Body", "ToEmail", "FromEmail", "PostDate", "LastDate" }))
            {
                try
                {
                    _dbContext.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(postToUpdate);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = _dbContext.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        [HttpPost]
        public ActionResult Update(Post post)
        {
            var postInDb = _dbContext.Posts.SingleOrDefault(v => v.PostID.Equals(post.PostID));

            if (postInDb == null)
                return HttpNotFound();

            postInDb.Subject = post.Subject;
            postInDb.Body = post.Body;
            postInDb.ToEmail = post.ToEmail;
            postInDb.FromEmail = post.FromEmail;
            postInDb.DatePosted = post.DatePosted;
            postInDb.LastDate = post.LastDate;
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }


        public ActionResult New()
        {
            return View();
        }
        public ActionResult Delete(int id)
        {
            var post = _dbContext.Posts.SingleOrDefault(v => v.PostID.Equals(id));

            if (post == null)
                return HttpNotFound();

            return View(post);
        }

        [HttpPost]
        public ActionResult DoDelete(int id)
        {
            var post = _dbContext.Posts.SingleOrDefault(v => v.PostID.Equals(id));

            if (post == null)
                return HttpNotFound();

            _dbContext.Posts.Remove(post);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
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