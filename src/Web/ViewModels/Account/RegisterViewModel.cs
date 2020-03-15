using System.ComponentModel.DataAnnotations;

namespace Microsoft.eShopWeb.Web.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Фамилия обязательное поле.")]
        [StringLength(30, ErrorMessage = "{0} должна быть как минимум {2} символов.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Фамилия")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Имя обязательное поле.")]
        [StringLength(30, ErrorMessage = "{0} должно быть как минимум {2} символов.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Имя")]
        public string SecondName { get; set; }

        [Required(ErrorMessage = "Телефон обязательное поле.")]
        [StringLength(30, ErrorMessage = "{0} должен быть как минимум {2} символов.", MinimumLength = 1)]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Телефон")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email обязательное поле.")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Логин обязательное поле.")]
        [StringLength(20, ErrorMessage = "{0} должно быть как минимум {2} символов.", MinimumLength = 3)]
        [RegularExpression(@"[A-Za-z0-9_]", ErrorMessage = "Некорректный адрес")]
        [Display(Name = "Логин")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Пароль обязательное поле.")]
        [StringLength(10, ErrorMessage = "{0} должен быть как минимум {2} символов.", MinimumLength = 3)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароль и подтверждение пароля не совпадают.")]
        public string ConfirmPassword { get; set; }
    }
}
