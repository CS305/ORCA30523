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

        [Display(Name = "To: ")]
        public string ToEmail { get; set; }

        [Display(Name = "From: ")]
        public string FromEmail { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DatePosted { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime LastDate { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}