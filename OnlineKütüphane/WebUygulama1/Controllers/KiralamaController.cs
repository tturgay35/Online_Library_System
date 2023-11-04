using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using WebUygulama1.Models;
using WebUygulama1.Models.Utility;

namespace WebUygulama1.Controllers
{
    [Authorize(Roles = UserRoles.Role_Admin)]
    public class KiralamaController : Controller
    {
        private readonly IKiralamaRepository _kiralamaRepository;
        private readonly IKitapRepository _kitapRepository;
        public readonly IWebHostEnvironment _webHostEnvironment;

        public KiralamaController(IKiralamaRepository kiralamaRepository, IKitapRepository kitapRepository,IWebHostEnvironment webHostEnvironment)
        {
            _kiralamaRepository = kiralamaRepository;
            _kitapRepository = kitapRepository;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            List<Kiralama> objKiralamaList = _kiralamaRepository.GetAll(includeProps:"Kitap").ToList();
            
            return View(objKiralamaList);
        }

        public IActionResult EkleGuncelle(int? id)
        {
            IEnumerable<SelectListItem> kitapList = _kitapRepository.GetAll().Select(k => new SelectListItem
            {
                Text = k.KitapAdi,
                Value = k.Id.ToString()
            });
            ViewBag.KitapList=kitapList;
            if (id == null || id == 0)
            {
                return View();
            }
            else
            {
                Kiralama? kiralamaVt = _kiralamaRepository.Get(u => u.Id == id);
                if (kiralamaVt == null)
                {
                    return NotFound();
                }

                return View(kiralamaVt);


            }
           
        }

        [HttpPost]
        public IActionResult EkleGuncelle(Kiralama kiralama)
        {


            if (ModelState.IsValid)
            {
                
               if (kiralama.Id == 0)
               {
                    _kiralamaRepository.Ekle(kiralama);
                    TempData["basarili"] = "Yeni kiralama kaydı başarıyla oluşturuldu.";
               }
               else
               {
                    _kiralamaRepository.Guncelle(kiralama);
                    TempData["basarili"] = "kiralama işlemi başarıyla Güncellendi.";
               }

               
               _kiralamaRepository.Kaydet();
               return RedirectToAction("Index", "Kiralama");
            }

            return View();
        }

        
        
        public IActionResult Sil(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Kiralama? kiralamaVt = _kiralamaRepository.Get(u=> u.Id==id);
            if (kiralamaVt == null)
            {
                return NotFound();
            }

            _kiralamaRepository.Sil(kiralamaVt);
            _kiralamaRepository.Kaydet();

            TempData["basarili"] = "Kiralama başarıyla Silindi.";
            return RedirectToAction("Index", "Kiralama");
        }
        


    }
}
