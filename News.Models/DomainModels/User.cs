using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace News.Models.DomainModels
{
    public abstract class User
    {
        public int Id { get; set; }

        [Display(Name = "ایمیل")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "لطفا حداکثر 100 کاراکتر وارد نمایید")]
        [EmailAddress(ErrorMessage = "آدرس ایمیل فرمت صحیحی ندارد")]
        public string Email { get; set; }

        [Display(Name = "رمز عبور")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "نام کامل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "لطفا حداکثر 100 کاراکتر وارد نمایید")]
        public string FullName { get; set; }

        public UserTypes UserType { get; set; }

        public string ResetPasswordCode { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }

    public enum UserTypes
    {
        admin, user
    }
}
