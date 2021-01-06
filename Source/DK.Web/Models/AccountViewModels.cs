using System.ComponentModel.DataAnnotations;

namespace DK.Web.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu cũ")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Mật khẩu phải có độ dài từ {0} đến {2} kí tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu mới")]
        [Compare(nameof(NewPassword), ErrorMessage = "Mật khẩu mới chưa khớp.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Display(Name = "Vai trò")]
        public bool IsAdmin{ get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Mật khẩu phải có độ dài từ {0} đến {2} kí tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare(nameof(Password), ErrorMessage = "Mật khẩu mới chưa khớp.")]
        public string ConfirmPassword { get; set; }
    }
}
