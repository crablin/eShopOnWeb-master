using System.ComponentModel.DataAnnotations;

namespace Microsoft.eShopWeb.Web.ViewModels.Manage
{
    public class IndexViewModel
    {
        [Display(Name = "Фамилия")]
        public string FirstName { get; set; }

        [Display(Name = "Имя")]
        public string SecondName { get; set; }

        [Display(Name = "Имя пользователя")]
        public string Username { get; set; }

        //public bool IsEmailConfirmed { get; set; }
        
        [Display(Name = "Страна")]
        public string Country { get; set; }

       
        [Display(Name = "Город")]
        public string City { get; set; }

       
        [Display(Name = "Улица")]
        public string Street { get; set; }

       
        [Display(Name = "Номер дома")]
        public int HomeNumber { get; set; }

       
        [Display(Name = "Номер квартиры")]
        public int Flat { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Телефон")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }
    }
}
