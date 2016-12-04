using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace ORCA30523.Models
{
    public class Post
    {
        [Key]
        public string PostID { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string ToEmail { get; set; }
        public string FromEmail { get; set; }
        public DateTime DatePosted { get; set; }
        public DateTime LastDate { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}