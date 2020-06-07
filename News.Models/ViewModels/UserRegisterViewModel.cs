using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace News.Models.ViewModels
{
    public class UserRegisterViewModel
    {

        [Display(Name = "نام کامل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "لطفا حداکثر 100 کاراکتر وارد نمایید")]
        public string FullName { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "لطفا حداکثر 100 کاراکتر وارد نمایید")]
        [EmailAddress(ErrorMessage = "آدرس ایمیل فرمت صحیحی ندارد")]
        public string Email { get; set; }

        [Display(Name = "رمز عبور")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Password { get; set; }

        [Display(Name = "تکرار رمز عبور")]
        [Compare("Password", ErrorMessage = "رمز عبور مطابقت ندارد")]
        [DataType(DataType.Password)]
        public string PasswordRepeat { get; set; }
    }
}
