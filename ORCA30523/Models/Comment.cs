using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace ORCA30523.Models
{
    public class Comment
    {
        [Key]
        public string CommentID { get; set; }
        //public string PostID { get; set; }
        public string Body { get; set; }
        public string Username { get; set; }
        public DateTime DatePosted { get; set; }
        public virtual Post ParentPost { get; set; }
    }
}