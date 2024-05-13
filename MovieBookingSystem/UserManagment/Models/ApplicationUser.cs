using Microsoft.AspNetCore.Identity;

namespace UserManagment.Models
{
    public class ApplicationUser : IdentityUser
    {

        public string Country { get; set; } = "India";

        public string State { get; set; }

        public string City { get; set; }

        public string PhoneNumber { get; set; }

        public string Gender { get; set; }
        public bool IsRemoved { get; set; }

    }

    public enum Gender
    {
        MALE,
        FEMALE

    }

}
