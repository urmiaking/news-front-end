using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace News.Models.ViewModels
{
    public class NewsListViewModel
    {
        public int Id { get; set; }

        [Display(Name = "عنوان خبر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(maximumLength: 200, ErrorMessage = "حداکثر ۲۰۰ کاراکتر مجاز است")]
        public string Title { get; set; }

        [Display(Name = "تاریخ انتشار")]
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }

        [Display(Name = "عکس خبر")]
        [DataType(DataType.ImageUrl)]
        public string ImageName { get; set; }

        [Display(Name = "تعداد بازدید")]
        public int VisitCount { get; set; }
    }
}
