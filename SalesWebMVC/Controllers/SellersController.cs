using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Models;
using SalesWebMVC.Models.ViewModels;
using SalesWebMVC.Services;
using SalesWebMVC.Services.Exceptions;

namespace SalesWebMVC.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }
        
        public IActionResult Index()
        {
            var sellerList = _sellerService.FindAllSellers();
            return View(sellerList);
        }

        public IActionResult Create()
        {
            var departmentList = _departmentService.FindAll();
            var viewModel = new SellerFormViewModel { Departments = departmentList };
            return View(viewModel);
        }
        
        [HttpPost] //Notation: Indicando que essa ação será de Post
        [ValidateAntiForgeryToken] //Previque que a aplicação sofra ataques CSRF (quando alguem aproveita uma secao de autenticacao para enviar dados maliciosos).
        public IActionResult Create(SellerFormViewModel l_sellerViewModel)
        {
            _sellerService.CreateSeller(l_sellerViewModel.Seller);
            return RedirectToAction(nameof(Index)); //Se o nome do metodo "Index" for mudado, irá gerar um erro
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound(); //Objeto que instancia uma resposta basica.
            }

            var l_sellerResult = _sellerService.FindById(id.Value); //como o valor da variavel "id" pode ser nulo, temos que por o "id.Value" (para pegar o valor, caso exista, tem que estar acompanhado do .Value).

            if (l_sellerResult == null)
            {
                return NotFound();
            }

            return View(l_sellerResult);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _sellerService.Remove(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var l_sellerResult = _sellerService.FindById(id.Value);

            if (l_sellerResult == null)
            {
                return NotFound();
            }

            return View(l_sellerResult);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var l_seller = _sellerService.FindById(id.Value);
            if (l_seller == null)
            {
                return NotFound();
            }

            List<Department> l_departments = _departmentService.FindAll();
            SellerFormViewModel viewModel = new SellerFormViewModel() { Seller = l_seller, Departments = l_departments };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, SellerFormViewModel sellerFormViewModel)
        {
            if (id != sellerFormViewModel.Seller.Id)
            {
                return BadRequest();
            }

            try
            {
                _sellerService.Update(sellerFormViewModel.Seller); //Pode retornar uma exception.
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (DbConcurrencyException)
            {
                return BadRequest();
            }
        }
    }
}