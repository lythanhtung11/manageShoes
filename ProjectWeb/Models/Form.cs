using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWeb.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Không được để trống !")]
        [MinLength(5, ErrorMessage = "Ít nhất 5 ký tự")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Không được để trống !")]
        [MinLength(5, ErrorMessage = "Ít nhất 5 ký tự")]
        public string Password { get; set; }

    }
    public class Register
    {
        [Required(ErrorMessage ="Không được để trống !")]
        [MinLength(5,ErrorMessage ="Ít nhất 5 ký tự")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Không được để trống !")]
        [MinLength(5, ErrorMessage = "Ít nhất 5 ký tự")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Không được để trống !")]
        [MinLength(5, ErrorMessage = "Ít nhất 5 ký tự")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Không được để trống !")]
        [Compare("Password", ErrorMessage = "Mật khẩu không trùng khớp !")]
        public string ConfirmPassword { get; set; }

    }

}
