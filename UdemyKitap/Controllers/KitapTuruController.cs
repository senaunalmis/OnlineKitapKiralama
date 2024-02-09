using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UdemyKitap.Models;
using UdemyKitap.Utility;

namespace UdemyKitap.Controllers
{
    [Authorize(Roles = UserRoles.Role_Admin)]
    public class KitapTuruController : Controller
	{
		private readonly IKitapTuruRepository _kitapTuruRepository;
		public KitapTuruController(IKitapTuruRepository context)
		{
            _kitapTuruRepository = context;
		}

		public IActionResult Index()
		{   //index çağırılınca veri tabanına Kitap türlerinin listesini çekecek objKitapTuruList e yazdıracak
			  List<KitapTuru> objKitapTuruList = _kitapTuruRepository.TumunuGetir().ToList();
			return View(objKitapTuruList);
		}

		public IActionResult Ekle()
        {   
            return View();
        }

        [HttpPost]
        public IActionResult Ekle(KitapTuru kitapTuru)
        {
			if (ModelState.IsValid)
			{
				//dbye parametre olarak gelen veriyi kaydedicez Dependecy injection verisini al
				_kitapTuruRepository.Ekle(kitapTuru); //eklemeye hazırlan
                _kitapTuruRepository.Kaydet(); //veritabanına ekle her seferinde git gel yapmamsın 1 kerelik save changes ile veri tabanına eklensin
                TempData["Basarili"] = "Yeni Kitap Türü Başarıyla Oluşturuldu! ";

                return RedirectToAction("Index", "KitapTuru"); //bana ekledikten sonra tekrar listele diyorum}
			}
			return View();
		}

        public IActionResult Guncelle(int? id)
        {
            if(id==0 || id==null)
                return NotFound();

            KitapTuru? kitapTuruVt = _kitapTuruRepository.Get(u=>u.Id==id); //Expression<Func<T, bool>> filtre

            if (kitapTuruVt==null)
                return NotFound();

            return View(kitapTuruVt); //güncellenen nesne kullanıcıya görünecek
        }

        [HttpPost]
        public IActionResult Guncelle(KitapTuru kitapTuru)
        {
            if (ModelState.IsValid)
            {
                //dbye parametre olarak gelen veriyi kaydedicez Dependecy injection verisini al
                _kitapTuruRepository.Guncelle(kitapTuru); //eklemeye hazırlan
                _kitapTuruRepository.Kaydet(); //veritabanına ekle her seferinde git gel yapmamsın 1 kerelik save changes ile veri tabanına eklensin
                TempData["Basarili"] = "Yeni Kitap Türü Başarıyla Güncellendi! ";
                return RedirectToAction("Index", "KitapTuru"); //bana ekledikten sonra tekrar listele diyorum}
            }
            return View();
        }
        public IActionResult Sil(int? id)
        {
            if (id == 0 || id == null)
                return NotFound();

            KitapTuru? kitapTuruVt = _kitapTuruRepository.Get(u => u.Id == id);

            if (kitapTuruVt == null)
                return NotFound();

            return View(kitapTuruVt); //güncellenen nesne kullanıcıya görünecek
        }
        [HttpPost, ActionName("Sil")]
        public IActionResult SilPost(int? id)
        {
            KitapTuru? kitapTuru = _kitapTuruRepository.Get(u => u.Id == id);
            if (kitapTuru == null)
                return NotFound();

            _kitapTuruRepository.Sil(kitapTuru);
            _kitapTuruRepository.Kaydet();
            TempData["Basarili"] = " Kayıt Silme İşlemi Başarılı! ";

            return RedirectToAction("Index", "KitapTuru");

                     
        }
    }
}
