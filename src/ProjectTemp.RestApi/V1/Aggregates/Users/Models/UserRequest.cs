using System.ComponentModel.DataAnnotations;

namespace ProjectTemp.RestApi.V1.Aggregates.Users.Models
{
    public class UserRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        public string Description { get; set; }
    }
}