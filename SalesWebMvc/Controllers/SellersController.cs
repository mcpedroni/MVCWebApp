using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Controllers {
    public class SellersController : Controller {

        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellerService, DepartmentService departmentService) {
            _sellerService = sellerService;
            _departmentService = departmentService; 

        }
        public IActionResult Index() {
            var list = _sellerService.FindAll(); 
            return View(list);
        }

        public IActionResult Create() {

            var departments = _departmentService.FindAll();
            var viewModel = new SellerFormViewModel { Departments = departments }; //instancia com uma lista contendo todos departamentos
            return View(viewModel);
        }        
        
        [HttpPost]
        [ValidateAntiForgeryToken] //previne que alguem aproveite a sessao de autenticacao e envia dados maliciosos.
        //insert seller on database
        public IActionResult Create(Seller seller) {
            _sellerService.Insert(seller);
            //retorna pra tela Index
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id) {
            if (id == null) {
                return NotFound();
            }

            var obj = _sellerService.FindById(id.Value);
            if (obj == null) {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id) {
            _sellerService.Remove(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
