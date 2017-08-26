using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using UMC.Model.Models;

namespace UMC.Web.Models
{
    public class MenuGroupViewModel
    {
        public int ID { set; get; }

        [Required(ErrorMessage = "Bạn cần nhập tên")]
        public string Name { set; get; }

        public virtual MenuViewModel Menu { set; get; }
    }
}