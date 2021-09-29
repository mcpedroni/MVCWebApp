using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Controllers {
    public class SellersController : Controller {

        private readonly SellerService _sellerService;

        public SellersController(SellerService sellerService) {
            _sellerService = sellerService;
        }
        public IActionResult Index() {
            var list = _sellerService.FindAll(); 
            return View(list);
        }

        public IActionResult Create() {
            
            return View();
        }        
        
        [HttpPost]
        [ValidateAntiForgeryToken] //previne que alguem aproveite a sessao de autenticacao e envia dados maliciosos.
        //insert seller on database
        public IActionResult Create(Seller seller) {
            _sellerService.Insert(seller);
            //retorna pra tela Index
            return RedirectToAction(nameof(Index));
        }
    }
}
