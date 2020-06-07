using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace News.Models.ViewModels
{
    public class UserLoginViewModel
    {
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "لطفا حداکثر 100 کاراکتر وارد نمایید")]
        [EmailAddress(ErrorMessage = "آدرس ایمیل فرمت صحیحی ندارد")]
        public string Email { get; set; }

        [Display(Name = "رمز عبور")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Password { get; set; }

        [Display(Name = "مرا بخاطر بسپار")]
        public bool RememberMe { get; set; }
    }
}
