using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UdemyKitap.Models
{
	public class KitapTuru
	{
		[Key] //id primary key
		public int Id { get; set; }

		[Required(ErrorMessage ="Kitap Türü Adı boş bırakılamaz!")] // not null
        [MaxLength(25)] // max 25 harf uzunluğu
        [DisplayName("Kitap Türü Adı")] //ekranda label için burası görünecek
        public string Ad { get; set; }
	}
}
