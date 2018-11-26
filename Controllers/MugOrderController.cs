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
    public class MugOrderController : Controller
    {
        private AppDbContext _dbContext;

        public MugOrderController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize]
        public IActionResult Index()
        {
            DbSet<MugOrder> dbs = _dbContext.MugOrder;

            List<MugOrder> model = null;
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
        public IActionResult Create(MugOrder mugOrder)
        {
            if (ModelState.IsValid)
            {
                DbSet<MugOrder> dbs = _dbContext.MugOrder;
                var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                mugOrder.UserCode = userid;
                dbs.Add(mugOrder);
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
            DbSet<MugOrder> dbs = _dbContext.MugOrder;
            MugOrder tOrder = dbs.Where(mo => mo.Id == id).FirstOrDefault();

            if (tOrder != null)
            {
                DbSet<Pokedex> dbsPokes = _dbContext.Pokedex;
                var lstPokes = dbsPokes.ToList();
                ViewData["pokes"] = new SelectList(lstPokes, "Id", "Name");
                return View(tOrder);
            }
            else
            {
                TempData["Msg"] = "Mug order not found!";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult Update(MugOrder mugOrder)
        {
            if (ModelState.IsValid)
            {
                DbSet<MugOrder> dbs = _dbContext.MugOrder;
                MugOrder tOrder = dbs.Where(mo => mo.Id == mugOrder.Id)
                                     .FirstOrDefault();

                if (tOrder != null)
                {
                    tOrder.Color = mugOrder.Color;
                    tOrder.PokedexId = mugOrder.PokedexId;
                    tOrder.Qty = mugOrder.Qty;

                    if (_dbContext.SaveChanges() == 1)
                        TempData["Msg"] = "Mug order updated!";
                    else
                        TempData["Msg"] = "Failed to update database!";
                }
                else
                {
                    TempData["Msg"] = "Mug order not found!";
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
            DbSet<MugOrder> dbs = _dbContext.MugOrder;

            MugOrder tOrder = dbs.Where(mo => mo.Id == id).FirstOrDefault();

            if (tOrder != null)
            {
                dbs.Remove(tOrder);

                if (_dbContext.SaveChanges() == 1)
                    TempData["Msg"] = "Mug order deleted!";
                else
                    TempData["Msg"] = "Failed to update database!";
            }
            else
            {
                TempData["Msg"] = "Mug order not found!";
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult PrintOrder(int id)
        {
            DbSet<MugOrder> dbs = _dbContext.MugOrder;
            MugOrder model = dbs.Where(mo => mo.Id == id)
                                .Include(mo => mo.Pokedex)
                                .Include(mo => mo.UserCodeNavigation)
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
