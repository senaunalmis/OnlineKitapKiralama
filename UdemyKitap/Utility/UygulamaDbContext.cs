using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UdemyKitap.Models;
//Veri tabanında EF tablo oluşturması için ilgili model sınıflarımızı buraya eklemeliyiz hepsini.

namespace UdemyKitap.Utility
{
	public class UygulamaDbContext : IdentityDbContext
	{
		public UygulamaDbContext(DbContextOptions<UygulamaDbContext> options) : base(options) { }
		
		public DbSet<KitapTuru> KitapTurleri { get; set; } //Ef deki kitap tutu sınıfı dbde KitapTurlerine denk gelmekte
		public DbSet<Kitap> Kitaplar { get; set; }
		public DbSet<Kiralama> Kiralamalar{ get; set;}
		public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
