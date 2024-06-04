using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OturumYonetimi_Core.Data;
using OturumYonetimi_Core.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace OturumYonetimi_Core.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Giris()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Giris(Kullanicilar kullanici, string ReturnUrl)
        {
            // Kullanıcı adı ve şifre kontrolü
            var login = _context.Kullanicilars.Where(x => x.KullaniciAdi == kullanici.KullaniciAdi && x.Sifre == kullanici.Sifre).FirstOrDefault();
            // Eğer kullanıcı adı ve şifre doğruysa
            if (login != null)
            {
                // Kullanıcıya ait claimler oluşturuluyor listeye ekleniyor.
                var talep = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, kullanici.KullaniciAdi.ToString())
                };
                // Kullanıcıya ait claimler ile kimlik oluşturuluyor. çünkü kimlik oluşturulmadan kullanıcı oturumu açılamaz.
                ClaimsIdentity kimlik = new ClaimsIdentity(talep, "Login");
                // Kimlik oluşturulduktan sonra ClaimsPrincipal oluşturuluyor. çünkü ClaimsPrincipal olmadan kullanıcı oturumu açılamaz.
                ClaimsPrincipal kural = new ClaimsPrincipal(kimlik);
           
                HttpContext.SignInAsync(kural);
                // Eğer ReturnUrl boş değilse
                if (!string.IsNullOrEmpty(ReturnUrl))
                {
                    // ReturnUrl'e yönlendirme yapılıyor.
                    return Redirect(ReturnUrl);
                }
                // Eğer ReturnUrl boş ise
                else
                {
                    // Anasayfaya yönlendirme yapılıyor.
                    return RedirectToAction("Index", "Home");
                }

            }
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
