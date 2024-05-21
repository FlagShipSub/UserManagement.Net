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


        public int TrailCout { get; set; } = 3;


        //navigation 
        [ForeignKey("ApplictionUser")]
        public string Id { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public OtpValidation(string otp, DateTime createdDateTime, DateTime updatedDateTime, DateTime expiryDateTime, int trailCout , string id)
        {
            Otp = otp;
            CreatedDateTime = createdDateTime;
            UpdatedDateTime = updatedDateTime;
            ExpiryDateTime = expiryDateTime;
            TrailCout = trailCout;
            Id = id;
        }

        public OtpValidation()
        {
        }
    }
}
