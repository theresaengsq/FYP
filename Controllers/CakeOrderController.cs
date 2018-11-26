using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using P06.Models;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace P06.Controllers
{
    public class CakeOrderController : Controller
    {
        private AppDbContext _dbContext;

        public CakeOrderController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize]
        public IActionResult Index()
        {
            DbSet<CakeOrder> dbs = _dbContext.CakeOrder;

            List<CakeOrder> model = null;
            if (User.IsInRole("Admin"))
                model = dbs.Include(mo => mo.Pokedex)
                            .Include(mo => mo.UserCodeNavigation)
                            .ToList();
            else
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                model = dbs.Where(mo => mo.UserCode == userId)
                            .Include(mo => mo.Pokedex)
                            .ToList();
            }
            return View(model);
        }
        
        [Authorize]
        public IActionResult Create()
        {
            DbSet<Pokedex> dbs = _dbContext.Pokedex;
            var lstPokes = dbs.ToList();
            ViewData["pokes"] = new SelectList(lstPokes, "Id", "Name");
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(CakeOrder cakeOrder)
        {
            if (ModelState.IsValid)
            {
                DbSet<CakeOrder> dbs = _dbContext.CakeOrder;
                var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                cakeOrder.UserCode = userid;
                dbs.Add(cakeOrder);
                if (_dbContext.SaveChanges() == 1)
                    TempData["Msg"] = "New order added!";
                else
                    TempData["Msg"] = "Failed to update database!";
            }
            else
            {
                TempData["Msg"] = "Invalid information entered";
            }
            return RedirectToAction("Index");
        }
        
        [Authorize]
        public IActionResult Update(int id)
        {
            DbSet<CakeOrder> dbs = _dbContext.CakeOrder;
            CakeOrder tOrder = dbs.Where(co => co.Id == id).FirstOrDefault();

            if (tOrder != null)
            {
                DbSet<Pokedex> dbsPokes = _dbContext.Pokedex;
                var lstPokes = dbsPokes.ToList();
                ViewData["pokes"] = new SelectList(lstPokes, "Id", "Name");
                return View(tOrder);
            }
            else
            {
                TempData["Msg"] = "Cake order not found!";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult Update(CakeOrder cakeOrder)
        {
            if (ModelState.IsValid)
            {
                DbSet<CakeOrder> dbs = _dbContext.CakeOrder;
                CakeOrder tOrder = dbs.Where(co => co.Id == cakeOrder.Id)
                                     .FirstOrDefault();

                if (tOrder != null)
                {
                    tOrder.Flavor = cakeOrder.Flavor;
                    tOrder.Greeting = cakeOrder.Greeting;
                    tOrder.PokedexId = cakeOrder.PokedexId;
                    tOrder.Qty = cakeOrder.Qty;

                    if (_dbContext.SaveChanges() == 1)
                        TempData["Msg"] = "Cake order updated!";
                    else
                        TempData["Msg"] = "Failed to update database!";
                }
                else
                {
                    TempData["Msg"] = "Cake order not found!";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["Msg"] = "Invalid information entered";
            }
            return RedirectToAction("Index");
        }
        
        [Authorize]
        public IActionResult Delete(int id)
        {
            DbSet<CakeOrder> dbs = _dbContext.CakeOrder;

            CakeOrder tOrder = dbs.Where(co => co.Id == id).FirstOrDefault();

            if (tOrder != null)
            {
                dbs.Remove(tOrder);

                if (_dbContext.SaveChanges() == 1)
                    TempData["Msg"] = "Cake order deleted!";
                else
                    TempData["Msg"] = "Failed to update database!";
            }
            else
            {
                TempData["Msg"] = "Cake order not found!";
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult PrintOrder(int id)
        {
            DbSet<CakeOrder> dbs = _dbContext.CakeOrder;
            CakeOrder model = dbs.Where(co => co.Id == id)
                                .Include(co => co.Pokedex)
                                .Include(co => co.UserCodeNavigation)
                                .FirstOrDefault();
            if (model != null)
                return new ViewAsPdf(model) {
                    PageSize = Rotativa.AspNetCore.Options.Size.B5,
                    PageOrientation= Rotativa.AspNetCore.Options.Orientation.Landscape
                };
            else
            {
                TempData["Msg"] = "Order not found!";
                return RedirectToAction("Index");
            }

        }
    }
}
