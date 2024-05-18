namespace UserManagment.Models.Dtos
{
    public class RegisterationRequestDto
    {
        public string UserName { get; set; } 

        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public string Gender { get; set; }
        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }
    }
}
