using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
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
        
        public async Task<IActionResult> Index()
        {
            var sellerList = await _sellerService.FindAllSellersAsync();
            return View(sellerList);
        }

        public async Task<IActionResult> Create()
        {
            var departmentList = await _departmentService.FindAllAsync();
            var viewModel = new SellerFormViewModel { Departments = departmentList };
            return View(viewModel);
        }
        
        [HttpPost] //Notation: Indicando que essa ação será de Post
        [ValidateAntiForgeryToken] //Previque que a aplicação sofra ataques CSRF (quando alguem aproveita uma secao de autenticacao para enviar dados maliciosos).
        public async Task<IActionResult> Create(SellerFormViewModel sellerViewModel)
        {
            if (!ModelState.IsValid) //Validacao a nivel de controller, caso o JS esteja desativado no browser.
            {
                var l_departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel() { Seller = sellerViewModel.Seller, Departments = l_departments };

                return View(viewModel);
            }
            await _sellerService.CreateSellerAsync(sellerViewModel.Seller);
            return RedirectToAction(nameof(Index)); //Se o nome do metodo "Index" for mudado, irá gerar um erro
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided!" } ); //Chamando tela de erro e passando parametro do método Error(), a partir da criacao de um objeto anônimo.
            }

            var l_sellerResult = await _sellerService.FindByIdAsync(id.Value); //como o valor da variavel "id" pode ser nulo, temos que por o "id.Value" (para pegar o valor, caso exista, tem que estar acompanhado do .Value).

            if (l_sellerResult == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found!" } );
            }

            return View(l_sellerResult);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _sellerService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "id not provided!" } );
            }

            var l_sellerResult = await _sellerService.FindByIdAsync(id.Value);

            if (l_sellerResult == null)
            {
                return RedirectToAction(nameof(Error), new { message = "id not found!" } );
            }

            return View(l_sellerResult);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "id not provided" } );
            }

            var l_seller = await _sellerService.FindByIdAsync(id.Value);
            if (l_seller == null)
            {
                return RedirectToAction(nameof(Error), new { message = "id not found" } );
            }

            List<Department> l_departments = await _departmentService.FindAllAsync();
            SellerFormViewModel viewModel = new SellerFormViewModel() { Seller = l_seller, Departments = l_departments };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SellerFormViewModel sellerFormViewModel)
        {
            if (id != sellerFormViewModel.Seller.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id mismatch" } );
            }

            try
            {
                await _sellerService.UpdateAsync(sellerFormViewModel.Seller); //Pode retornar uma exception.
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException e) 
            {
                return RedirectToAction(nameof(Error), new { message = e.Message } ); //NotFoundException e DbConcurrencyException são excessoes, e excessoes já carregam mensagens.
            }
            catch (DbConcurrencyException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel()
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier //Massete do Framework para pegar o ID interno da requisição.
            };

            return View(viewModel);
        }
    }
}