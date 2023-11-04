using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebUygulama1.Models;
using WebUygulama1.Models.Utility;

namespace WebUygulama1.Controllers
{
    
    public class KitapController : Controller
    {
        

        private readonly IKitapRepository _kitapRepository;
        private readonly IKitapTuruRepository _kitapTuruRepository;
        public readonly IWebHostEnvironment _webHostEnvironment;

        public KitapController(IKitapRepository kitapRepository, IKitapTuruRepository kitapTuruRepository,IWebHostEnvironment webHostEnvironment)
        {
            _kitapRepository = kitapRepository;
            _kitapTuruRepository = kitapTuruRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        [Authorize(Roles = "Admin,Ogrenci")]
        public IActionResult Index()
        {
            List<Kitap> objKitapList = _kitapRepository.GetAll(includeProps:"kitapTuru").ToList();
            
            return View(objKitapList);
        }
        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult EkleGuncelle(int? id)
        {
            IEnumerable<SelectListItem> kitapTuruList = _kitapTuruRepository.GetAll().Select(k => new SelectListItem
            {
                Text = k.Ad,
                Value = k.Id.ToString()
            });
            ViewBag.KitapTuruList=kitapTuruList;
            if (id == null || id == 0)
            {
                return View();
            }
            else
            {
                Kitap? kitapVt = _kitapRepository.Get(u => u.Id == id);
                if (kitapVt == null)
                {
                    return NotFound();
                }

                return View(kitapVt);


            }
           
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult EkleGuncelle(Kitap kitap,IFormFile? file)
        {


            if (ModelState.IsValid)
            {
               
               if (file !=null)
               {
                   string wwwRootPath = _webHostEnvironment.WebRootPath;
                   string kitapPath = Path.Combine(wwwRootPath, @"img");

                    using (var fileStream = new FileStream(Path.Combine(kitapPath, file.FileName), FileMode.Create))
                   {
                       file.CopyTo(fileStream);
                   }
                   kitap.ResimUrl = @"\img\" + file.FileName;
               }
               

               
               if (kitap.Id == 0)
               {
                    _kitapRepository.Ekle(kitap);
                    TempData["basarili"] = "Yeni Kitap başarıyla oluşturuldu.";
               }
               else
               {
                    _kitapRepository.Guncelle(kitap);
                    TempData["basarili"] = "Yeni Kitap başarıyla Güncellendi.";
               }

               
               _kitapRepository.Kaydet();
               return RedirectToAction("Index", "Kitap");
            }

            return View();
        }

        /*
        public IActionResult Guncelle(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Kitap? kitapVt = _kitapRepository.Get(u=>u.Id==id);
            if (kitapVt == null)
            {
                return NotFound();
            }

            return View( kitapVt);
        }

        [HttpPost]
        public IActionResult Guncelle(Kitap kitap)
        {
            if (ModelState.IsValid)
            {


                _kitapRepository.Guncelle(kitap);
                _kitapRepository.Kaydet();
                TempData["basarili"] = "Kitap  başarıyla güncellendi.";
                return RedirectToAction("Index", "Kitap");
            }

            return View();
        }
        */
        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult Sil(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Kitap? kitapVt = _kitapRepository.Get(u=> u.Id==id);
            if (kitapVt == null)
            {
                return NotFound();
            }

            _kitapRepository.Sil(kitapVt);
            _kitapRepository.Kaydet();

            TempData["basarili"] = "Kitap başarıyla Silindi.";
            return RedirectToAction("Index", "Kitap");
        }
        


    }
}
