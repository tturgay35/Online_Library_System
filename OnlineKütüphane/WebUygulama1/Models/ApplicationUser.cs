using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;

namespace WebUygulama1.Models
{
    public class ApplicationUser:IdentityUser
    
    {
        [Required]
        public int OgrenciNo { get; set; }
        public String? Adres { get; set; }
        public String? Fakulte { get; set; }
        public String? Bolum { get; set; }

    }
}
