using Microsoft.AspNetCore.Identity;

namespace Microsoft.eShopWeb.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string Country { get; set; }
        public string City { get; set; }

        public string Street { get; set; }

        public int HomeNumber { get; set; }

        public int Flat { get; set; }
    }
}
