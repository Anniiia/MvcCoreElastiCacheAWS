using Microsoft.AspNetCore.Mvc;
using MvcCoreElastiCacheAWS.Models;
using MvcCoreElastiCacheAWS.Repositories;
using MvcCoreElastiCacheAWS.Services;
using System.ComponentModel.DataAnnotations;

namespace MvcCoreElastiCacheAWS.Controllers
{
    public class CochesController : Controller
    {
        private RepositoryCoches repo;
        private ServiceAWSCache service;

        public CochesController(RepositoryCoches repo, ServiceAWSCache service)
        {
            this.repo = repo;
            this.service = service;
        }

        public async Task<IActionResult> SeleccionarFavorito(int idcoche)
        {
            //buscamos el coche dentro del documento XML (repo)

            Coche car = this.repo.FindCoche(idcoche);

            await this.service.AddCochesFavoritosAsync(car);
            return RedirectToAction("Favoritos");

        }

        public async Task<IActionResult> Favoritos()
        {
            List<Coche> cars = await this.service.GetcochesFavoritosAsync();
            return View(cars);
        }

        public async Task<IActionResult> DeleteFavorito(int idcoche)
        {
            await this.service.DeleteCochesFavoritosAync(idcoche);

            return RedirectToAction("Favoritos");
        }
        public IActionResult Index()
        {
            List<Coche> coches = this.repo.GetCoches();
            
            return View(coches);
        }

        public IActionResult Details(int id)
        {
            Coche coche = this.repo.FindCoche(id);

            return View(coche);
        }
    }
}
