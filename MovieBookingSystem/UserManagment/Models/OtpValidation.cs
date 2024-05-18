using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagment.Models
{
    public class OtpValidation
    {
        [Key]
        public int OptId { get; set; }

        public string Otp { get; set; }

        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        public DateTime UpdatedDateTime {  get; set; }

        public DateTime ExpiryDateTime { get; set; } = DateTime.Now.AddMinutes(30);


        public string TrailCout { get; set; }


        //navigation 
        [ForeignKey("ApplictionUser")]
        public string Id { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

    }
}
