using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.eShopWeb.Infrastructure.Identity;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
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

            [Required(ErrorMessage = "Укажите страну доставки.")]
            [StringLength(35, ErrorMessage = "{0} должна быть как минимум {2} символов.", MinimumLength = 3)]
            [Display(Name = "Страна")]
            public string Country { get; set; }

            [Required(ErrorMessage = "Укажите город доставки.")]
            [StringLength(35, ErrorMessage = "{0} должен быть как минимум {2} символов.", MinimumLength = 3)]
            [Display(Name = "Город")]
            public string City { get; set; }

            [Required(ErrorMessage = "Укажите улицу доставки.")]
            [StringLength(35, ErrorMessage = "{0} должна быть как минимум {2} символов.", MinimumLength = 3)]
            [Display(Name = "Улица")]
            public string Street { get; set; }

            [Required(ErrorMessage = "Укажите номер дома доставки.")]
            [Display(Name = "Номер дома")]
            public string HomeNumber { get; set; }

            [Required(ErrorMessage = "Укажите номер квартиры доставки.")]
            [Display(Name = "Номер квартиры")]
            public string Flat { get; set; }

            [Required(ErrorMessage = "Логин обязательное поле.")]
            [StringLength(20, ErrorMessage = "{0} должно быть как минимум {2} символов.", MinimumLength = 3)]
            [Display(Name = "Логин")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "Пароль обязательное поле.")]
            [StringLength(10, ErrorMessage = "{0} должен быть как минимум {2} символов.", MinimumLength = 3)]
            [DataType(DataType.Password)]
            [Display(Name = "Пароль")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Подтвердите пароль")]
            [Compare("Password", ErrorMessage = "Пароль и подтверждение пароля не совпадают.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.UserName, Email = Input.Email, FirstName = Input.FirstName, 
                    SecondName = Input.SecondName, PhoneNumber = Input.PhoneNumber, Country = Input.Country, City = Input.City, Street = Input.Street, 
                    HomeNumber = int.Parse(Input.HomeNumber), Flat = int.Parse(Input.Flat)};
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Подтверждение почты",
                        $"Подтвердите свою почту перейдя по ссылке <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>нажмите здесь</a>.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

           
            return Page();
        }
    }
}
