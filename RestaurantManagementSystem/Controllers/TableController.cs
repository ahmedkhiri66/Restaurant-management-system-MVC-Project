using Microsoft.AspNetCore.Mvc;
using RestaurantManagementSystem.Data;
using RestaurantManagementSystem.Models;
using System;
using System.Linq;

namespace RestaurantManagementSystem.Controllers
{
    public class TableController : Controller
    {
        public IActionResult Index()
        {
            return View(DataRepository.GetTables());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleAvailability(int id)
        {
            var table = DataRepository.GetTable(id);
            if (table == null)
            {
                return NotFound();
            }

            table.IsAvailable = !table.IsAvailable;
            DataRepository.UpdateTable(table);

            TempData["TableStatus"] = $"Table {table.Name} is now {(table.IsAvailable ? "Available" : "Occupied")}";
            return RedirectToAction(nameof(Index));
        }
    }
}