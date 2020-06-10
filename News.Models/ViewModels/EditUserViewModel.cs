using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace News.Models.ViewModels
{
    public class EditUserViewModel
    {
        public int Id { get; set; }

        [Display(Name = "نام کامل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "لطفا حداکثر 100 کاراکتر وارد نمایید")]
        public string FullName { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "لطفا حداکثر 100 کاراکتر وارد نمایید")]
        [EmailAddress(ErrorMessage = "آدرس ایمیل فرمت صحیحی ندارد")]
        public string Email { get; set; }

        [Display(Name = "رمز عبور جدید")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "لطفا حداکثر ۶ کاراکتر وارد کنید")]
        public string NewPassword { get; set; }

        [Display(Name = "تکرار رمز عبور جدید")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "رمز جدید با رمز وارد شده مطابقت ندارد")]
        public string ConfirmPassword { get; set; }
    }
}
