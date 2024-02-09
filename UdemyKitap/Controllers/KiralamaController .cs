using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UdemyKitap.Models;
using UdemyKitap.Utility;
namespace UdemyKitap.Controllers
{
    [Authorize(Roles = UserRoles.Role_Admin)]
    public class KiralamaController : Controller
	{
        private readonly IKiralamaRepository _kiralamaRepository;
        private readonly IKitapRepository _kitapRepository;
        public readonly IWebHostEnvironment _webHostEnvironment;
		public KiralamaController(IKiralamaRepository kiralamaRepository, IKitapRepository kitapRepository, IWebHostEnvironment webHostEnvironment)
		{
            _kiralamaRepository = kiralamaRepository;
            _kitapRepository = kitapRepository;
            _webHostEnvironment = webHostEnvironment;
		}

		public IActionResult Index()
		{   
            List<Kiralama> objKiralamaList = _kiralamaRepository.TumunuGetir(includeProps:"Kitap").ToList();
            return View(objKiralamaList);
		}

		public IActionResult EkleGuncelle(int? id)
        {
            IEnumerable<SelectListItem> KitapList = _kitapRepository.TumunuGetir()
                .Select(k => new SelectListItem
                {
                    Text = k.KitapAdi,
                    Value = k.Id.ToString()
                });
            ViewBag.KitapList = KitapList; //viewe aktarmayı kolaylaştırır
            
            //ekle
            if (id == null || id == 0)
            {
                return View();
            }          

            //guncelle
            else
            {
                Kiralama? kiralamaVt = _kiralamaRepository.Get(u => u.Id == id); //Expression<Func<T, bool>> filtre

                if (kiralamaVt == null)
                    return NotFound();

                return View(kiralamaVt);
            }

        }

        [HttpPost]
        public IActionResult EkleGuncelle(Kiralama kiralama)
        {
           // var errors= ModelState.Values.SelectMany(x => x.Errors);// model state ile ilgili hataları görmek için koyulur

			if (ModelState.IsValid)
			{
                if(kiralama.Id == 0) {
                    //eklemeye hazırlan
                    _kiralamaRepository.Ekle(kiralama);
                    TempData["Basarili"] = "Yeni Kiralama Kaydı Başarıyla Oluşturuldu! ";
                }
                else
                {
                    _kiralamaRepository.Guncelle(kiralama);
                    TempData["Basarili"] = "Kiralama Kayıt Başarıyla Güncellendi! ";
                }
                //dbye parametre olarak gelen veriyi kaydedicez Dependecy injection verisini al
                
                _kitapRepository.Kaydet(); //veritabanına ekle her seferinde git gel yapmamsın 1 kerelik save changes ile veri tabanına eklensin
                

                return RedirectToAction("Index", "Kiralama"); //bana ekledikten sonra tekrar listele diyorum}
			}
			return View();
		}

        public IActionResult Sil(int? id)
        {
            IEnumerable<SelectListItem> KitapList = _kitapRepository.TumunuGetir()
               .Select(k => new SelectListItem
               {
                   Text = k.KitapAdi,
                   Value = k.Id.ToString()
               });
            ViewBag.KitapList = KitapList; //viewe aktarmayı kolaylaştırır
            if (id == 0 || id == null)
                return NotFound();

            Kiralama? kiralamaVt = _kiralamaRepository.Get(u => u.Id == id);

            if (kiralamaVt == null)
                return NotFound();

            return View(kiralamaVt); //güncellenen nesne kullanıcıya görünecek
        }
        [HttpPost, ActionName("Sil")]
        public IActionResult SilPost(int? id)
        {
            Kiralama? kiralama = _kiralamaRepository.Get(u => u.Id == id);
            if (kiralama == null)
                return NotFound();

            _kiralamaRepository.Sil(kiralama);
            _kitapRepository.Kaydet();
            TempData["Basarili"] = " Kayıt Silme İşlemi Başarılı! ";

            return RedirectToAction("Index", "Kiralama");

                     
        }
    }
}
