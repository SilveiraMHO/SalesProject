using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Models;
using SalesWebMVC.Services;

namespace SalesWebMVC.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;

        public SellersController(SellerService sellerService)
        {
            _sellerService = sellerService;
        }
        
        public IActionResult Index()
        {
            var sellerList = _sellerService.FindAllSellers();
            return View(sellerList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost] //Notation: Indicando que essa ação será de Post
        [ValidateAntiForgeryToken] //Previque que a aplicação sofra ataques CSRF (quando alguem aproveita uma secao de autenticacao para enviar dados maliciosos).
        public IActionResult Create(Seller l_seller)
        {
            _sellerService.CreateSeller(l_seller);
            return RedirectToAction(nameof(Index)); //Se o nome "Index" for mudado,irá gerar um erro
        }
    }
}