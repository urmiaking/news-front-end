using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace News.Models.DomainModels
{
    public class NewsGroup
    {
        public NewsGroup()
        {

        }

        [Display(Name = "گروه خبر")]
        public int Id { get; set; }

        [Display(Name = "عنوان گروه")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [MaxLength(200)]
        public string GroupTitle { get; set; }

        public virtual List<News> News { get; set; }
    }
}
