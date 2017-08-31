using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UMC.Web.Models
{
    public class MenuViewModel
    {
        public int ID { set; get; }
        [Required]
        public string Name { set; get; }
        [Required]
        public string URL { set; get; }

        public int? DisplayOrder { set; get; }
        [Required]
        public int GroupID { set; get; }

        [ForeignKey("GroupID")]
        public virtual MenuGroupViewModel MenuGroup { set; get; }

        public string Target { set; get; }

        public bool Status { set; get; }
    }
}