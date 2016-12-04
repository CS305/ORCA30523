using IdentitySample.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ORCA30523.Models;

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
        public ActionResult Index()
        {
            var posts = _dbContext.Posts.ToList();

            return View(posts);
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
    }
}