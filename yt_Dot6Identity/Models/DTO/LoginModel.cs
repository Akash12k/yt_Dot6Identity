using System.ComponentModel.DataAnnotations;

namespace yt_Dot6Identity.Models.DTO
{
    public class LoginModel
    {
        [Required]
        public string Username {  get; set; }
        [Required]
        public string password { get; set; }
    }
}
