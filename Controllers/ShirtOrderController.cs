using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using P06.Models;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace P06.Controllers
{
    public class ShirtOrderController : Controller
    {
        private AppDbContext _dbContext;

        public ShirtOrderController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize]
        public IActionResult Index()
        {
            // TODO P06 Task 1-1 Refer to MugOrder. Change the LINQ query to load related entities.
            DbSet<ShirtOrder> dbs = _dbContext.ShirtOrder;
            List<ShirtOrder> model = null;
            if (User.IsInRole("Admin"))
                model = dbs.Include(so => so.Pokedex).Include(so => so.UserCodeNavigation).ToList();
            else
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                // model = dbs.Where(so => so.UserCode == userId)
                //             .ToList();
                model = dbs.Where(so => so.UserCode == userId).Include(so => so.Pokedex).ToList();
            }
            return View(model);
        }

        [Authorize]
        public IActionResult Create()
        {
            DbSet<Pokedex> dbsPokes = _dbContext.Pokedex;
            var lstPokes = dbsPokes.ToList();
            ViewData["pokes"] = new SelectList(lstPokes, "Id", "Name");

            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(ShirtOrder shirtOrder)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                shirtOrder.UserCode = userId;
                _dbContext.ShirtOrder.Add(shirtOrder);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(shirtOrder);
        }

        [Authorize]
        public IActionResult Update(int id)
        {
            DbSet<ShirtOrder> dbs = _dbContext.ShirtOrder;
            ShirtOrder tOrder = dbs.Where(mo => mo.Id == id).FirstOrDefault();

            if (tOrder != null)
            {
                DbSet<Pokedex> dbsPokes = _dbContext.Pokedex;
                var lstPokes = dbsPokes.ToList();
                ViewData["pokes"] = new SelectList(lstPokes, "Id", "Name");

                return View(tOrder);
            }
            else
            {
                TempData["Msg"] = "Shirt order not found!";
                return RedirectToAction("Index");
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult Update(ShirtOrder shirtOrder)
        {
            if (ModelState.IsValid)
            {
                DbSet<ShirtOrder> dbs = _dbContext.ShirtOrder;
                ShirtOrder tOrder = dbs.Where(mo => mo.Id == shirtOrder.Id).FirstOrDefault();

                if (tOrder != null)
                {
                    tOrder.Name = shirtOrder.Name;
                    tOrder.Color = shirtOrder.Color;
                    tOrder.PokedexId = shirtOrder.PokedexId;
                    tOrder.Qty = shirtOrder.Qty;
                    tOrder.Price = shirtOrder.Price;
                    tOrder.FrontPosition = shirtOrder.FrontPosition;

                    if (_dbContext.SaveChanges() == 1)
                        TempData["Msg"] = "Shirt order updated!";
                    else
                        TempData["Msg"] = "Failed to update database!";
                }
                else
                {
                    TempData["Msg"] = "Shirt order not found!";
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
            DbSet<ShirtOrder> dbs = _dbContext.ShirtOrder;
            ShirtOrder tOrder = dbs.Where(so => so.Id == id).FirstOrDefault();

            if (tOrder != null)
            {
                dbs.Remove(tOrder);

                if (_dbContext.SaveChanges() == 1)
                    TempData["Msg"] = "Shirt order deleted!";
                else
                    TempData["Msg"] = "Failed to update database!";
            }
            else
            {
                TempData["Msg"] = "Shirt order not found!";
            }
            return RedirectToAction("Index");
        }


        // TODO P06 Task 2-3 Refer to MugOrder. Implement PrintOrder action here.

        [Authorize]
        public IActionResult PrintOrder(int id)
        {
            DbSet<ShirtOrder> dbs = _dbContext.ShirtOrder;
            ShirtOrder model = dbs.Where(so => so.Id == id)
                                .Include(so => so.Pokedex)
                                .Include(so => so.UserCodeNavigation)
                                .FirstOrDefault();
            if (model != null)
                return new ViewAsPdf(model)
                {
                    PageSize = Rotativa.AspNetCore.Options.Size.B5,
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
                };
            else
            {
                TempData["Msg"] = "Order not found!";
                return RedirectToAction("Index");
            }

        }

        // TODO P06 Task 2-4 Refer to MugOrder. Add shirt order PrintOrder.cshtml view

    }
}
