using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebUygulama1.Models
{
    public class KitapTuru
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Kitap Türü Adı boş bırakılamaz veya 25 karakteri geçemez!")]
        [DisplayName("Kitap Türü Adı")]
        [MaxLength(25)]
        public string Ad { get; set; }


    }
}
