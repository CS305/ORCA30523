using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ORCA30523.Models
{
    public class Ticket
    {
        public int ID { get; set; }
        [Display(Name = "To: ")]
        public string ToEmail { get; set; }
        [Display(Name = "From: ")]
        public string FromEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public string CreateDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public string DatePosted { get; set; }
        
    }
}