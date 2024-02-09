using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UdemyKitap.Models;
using UdemyKitap.Utility;
namespace UdemyKitap.Controllers
{
    
    public class KitapController : Controller
	{
        private readonly IKitapRepository _kitapRepository;
        private readonly IKitapTuruRepository _kitapTuruRepository;
        public readonly IWebHostEnvironment _webHostEnvironment;
		public KitapController(IKitapRepository kitapRepository, IKitapTuruRepository kitapTuruRepository, IWebHostEnvironment webHostEnvironment)
		{
            _kitapRepository = kitapRepository;
            _kitapTuruRepository = kitapTuruRepository;
            _webHostEnvironment = webHostEnvironment;
		}

        [Authorize(Roles = "Admin,Ogrenci")]
        public IActionResult Index()
		{   //index çağırılınca veri tabanına Kitap türlerinin listesini çekecek objKitapList e yazdıracak
            //List<Kitap> objKitapList = _kitapRepository.TumunuGetir().ToList();
            List<Kitap> objKitapList = _kitapRepository.TumunuGetir(includeProps:"KitapTuru").ToList();
            return View(objKitapList);
		}

        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult EkleGuncelle(int? id)
        {
            IEnumerable<SelectListItem> KitapTuruList = _kitapTuruRepository.TumunuGetir()
                .Select(k => new SelectListItem
                {
                    Text = k.Ad,
                    Value = k.Id.ToString()
                });
            ViewBag.KitapTuruList = KitapTuruList; //viewe aktarmayı kolaylaştırır
            
            //ekle
            if (id == null || id == 0)
            {
                return View();
            }          

            //guncelle
            else
            {
                Kitap? kitapVt = _kitapRepository.Get(u => u.Id == id); //Expression<Func<T, bool>> filtre

                if (kitapVt == null)
                    return NotFound();

                return View(kitapVt);
            }

        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult EkleGuncelle(Kitap kitap, IFormFile? file)
        {
           // var errors= ModelState.Values.SelectMany(x => x.Errors);// model state ile ilgili hataları görmek için koyulur

			if (ModelState.IsValid)
			{
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string kitapPath = Path.Combine(wwwRootPath, @"img");

                if (file != null) {
                    using (var fileStream = new FileStream(Path.Combine(kitapPath, file.FileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    //url ismini butondan çekme
                    kitap.ResimUrl = @"\img\" + file.FileName;
                }
                
                
                if(kitap.Id == 0) {
                    //eklemeye hazırlan
                    _kitapRepository.Ekle(kitap);
                    TempData["Basarili"] = "Yeni Kitap Başarıyla Oluşturuldu! ";
                }
                else
                {
                    _kitapRepository.Guncelle(kitap);
                    TempData["Basarili"] = "Yeni Kitap Başarıyla Güncellendi! ";
                }
                //dbye parametre olarak gelen veriyi kaydedicez Dependecy injection verisini al
                
                _kitapRepository.Kaydet(); //veritabanına ekle her seferinde git gel yapmamsın 1 kerelik save changes ile veri tabanına eklensin
                

                return RedirectToAction("Index", "Kitap"); //bana ekledikten sonra tekrar listele diyorum}
			}
			return View();
		}

        //public IActionResult Guncelle(int? id)
        //{
        //    if(id==0 || id==null)
        //        return NotFound();

        //    Kitap? kitapVt = _kitapRepository.Get(u=>u.Id==id); //Expression<Func<T, bool>> filtre

        //    if (kitapVt==null)
        //        return NotFound();

        //    return View(kitapVt); //güncellenen nesne kullanıcıya görünecek
        //}

        //[HttpPost]
        //public IActionResult Guncelle(Kitap kitap)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //dbye parametre olarak gelen veriyi kaydedicez Dependecy injection verisini al
        //        _kitapRepository.Guncelle(kitap); //eklemeye hazırlan
        //        _kitapRepository.Kaydet(); //veritabanına ekle her seferinde git gel yapmamsın 1 kerelik save changes ile veri tabanına eklensin
        //        TempData["Basarili"] = "Yeni Kitap Başarıyla Güncellendi! ";
        //        return RedirectToAction("Index", "Kitap"); //bana ekledikten sonra tekrar listele diyorum}
        //    }
        //    return View();
        //}

        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult Sil(int? id)
        {
            if (id == 0 || id == null)
                return NotFound();

            Kitap? kitapVt = _kitapRepository.Get(u => u.Id == id);

            if (kitapVt == null)
                return NotFound();

            return View(kitapVt); //güncellenen nesne kullanıcıya görünecek
        }
        [HttpPost, ActionName("Sil")]
        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult SilPost(int? id)
        {
            Kitap? kitap = _kitapRepository.Get(u => u.Id == id);
            if (kitap == null)
                return NotFound();

            _kitapRepository.Sil(kitap);
            _kitapRepository.Kaydet();
            TempData["Basarili"] = " Kayıt Silme İşlemi Başarılı! ";

            return RedirectToAction("Index", "Kitap");

                     
        }
    }
}
