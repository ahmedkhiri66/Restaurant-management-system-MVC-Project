
using Microsoft.AspNetCore.Mvc;
using RestaurantManagementSystem.Data;
using RestaurantManagementSystem.Models;
using System.Linq;

namespace RestaurantManagementSystem.Controllers
{
    public class MenuController : Controller
    {
        // GET: Menu
        public IActionResult Index()
        {
            return View(DataRepository.GetMenuItems());
        }

        // GET: Menu/Create
        public IActionResult Create()
        {
            ViewBag.Categories = DataRepository.GetCategories();
            return View();
        }

        // POST: Menu/Create
        [HttpPost]
        public IActionResult Create(MenuItem menuItem)
        {
            if (ModelState.IsValid)
            {
                menuItem.Id = DataRepository.GetMenuItems().Count + 1;
                DataRepository.AddMenuItem(menuItem);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = DataRepository.GetCategories();
            return View(menuItem);
        }

        // GET: Menu/Edit/5
        public IActionResult Edit(int id)
        {
            var item = DataRepository.GetMenuItem(id);
            

            ViewBag.Categories = DataRepository.GetCategories();
            return View(item);
        }

        // POST: Menu/Edit/5
        [HttpPost]
        public IActionResult Edit(int id, MenuItem menuItem)
        {
          

            if (ModelState.IsValid)
            {
                DataRepository.UpdateMenuItem(menuItem);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = DataRepository.GetCategories();
            return View(menuItem);
        }

        // GET: Menu/Delete/5
        public IActionResult Delete(int id)
        {
            var item = DataRepository.GetMenuItem(id);
            if (item == null) return NotFound();

            return View(item);
        }

        // POST: Menu/Delete/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            DataRepository.DeleteMenuItem(id);
            return RedirectToAction(nameof(Index));
        }
    }
}