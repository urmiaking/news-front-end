using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace News.Models.ViewModels
{
    public class ForgetPasswordViewModel
    {
        [Display(Name = "آدرس ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "لطفا حداکثر 100 کاراکتر وارد نمایید")]
        [EmailAddress(ErrorMessage = "آدرس ایمیل فرمت صحیحی ندارد")]
        public string Email { get; set; }
    }
}
