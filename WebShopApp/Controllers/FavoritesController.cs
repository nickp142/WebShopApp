using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebShopApp.Core.Contracts;

using WebShopApp.Infrastructure.Data;
using WebShopApp.Infrastructure.Data.Entities;


namespace WebShopApp.Controllers
{
    public class FavoritesController : Controller
    {

        private readonly IFavoriteService _favoriteService;
        private readonly UserManager<ApplicationUser> _userManager;

        public FavoritesController(IFavoriteService favoriteService, UserManager<ApplicationUser> userManager)
        {
            _favoriteService = favoriteService;
            _userManager = userManager;
        }

        private string GetUserId() => _userManager.GetUserId(User);

        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var products = await _favoriteService.GetFavoritesAsync(userId);
            return View(products);
        }


        public async Task<IActionResult> Add(int productId)
        {
            var userId = GetUserId();
            await _favoriteService.AddFavoriteAsync(userId, productId);
            return RedirectToAction("Index", "Product");
        }

        [Authorize]
        public async Task<IActionResult> Remove(int productId)
        {
            var userId = GetUserId();
            await _favoriteService.RemoveFavoriteAsync(userId, productId);
            return RedirectToAction("Index");
        }





        // GET: FavoritesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FavoritesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FavoritesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FavoritesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FavoritesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FavoritesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FavoritesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}