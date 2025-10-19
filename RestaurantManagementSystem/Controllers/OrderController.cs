
using Microsoft.AspNetCore.Mvc;
using RestaurantManagementSystem.Data;
using RestaurantManagementSystem.Models;
using System;
using System.Linq;

namespace RestaurantManagementSystem.Controllers
{
    public class OrderController : Controller
    {
        // GET: /Order
        public IActionResult Index()
        {
            return View(DataRepository.GetOrders());
        }

        // GET: /Order/Create
        public IActionResult Create()
        {
            var items = DataRepository.GetMenuItems().Where(m => m.IsAvailable).ToList();
            if (!items.Any())
            {
                TempData["Message"] = "No menu items available";
                return RedirectToAction("Index");
            }
            ViewBag.MenuItems = items;
            return View();
        }

        // POST: /Order/Create
        [HttpPost]
        public IActionResult Create(Order order, int[] menuItemIds, int[] quantities)
        {
            if (menuItemIds == null || !menuItemIds.Any())
            {
                ModelState.AddModelError("", "Please select items");
                ViewBag.MenuItems = DataRepository.GetMenuItems().Where(m => m.IsAvailable).ToList();
                return View(order);
            }

            order.OrderTime = DateTime.Now;
            order.Status = OrderStatus.Pending;
            order.Items = menuItemIds.Select((id, i) => new OrderItem
            {
                MenuItemId = id,
                MenuItemName = DataRepository.GetMenuItem(id)?.Name,
                Quantity = quantities[i],
                UnitPrice = DataRepository.GetMenuItem(id)?.Price ?? 0
            }).ToList();

            order.Subtotal = order.Items.Sum(i => i.Subtotal);
            order.Tax = order.Subtotal * 0.085m;
            order.Total = order.Subtotal + order.Tax;

            DataRepository.AddOrder(order);
            return RedirectToAction("Index");
        }
        public IActionResult Details(int id)
        {
            var order = DataRepository.GetOrder(id);
           
            return View(order);
        }

        // GET: /Order/Delete/5
        public IActionResult Delete(int id)
        {
            var order = DataRepository.GetOrder(id);
            if (order == null) return NotFound();
            return View(order);
        }

        // POST: /Order/Delete/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            DataRepository.DeleteOrder(id);
            return RedirectToAction("Index");
        }
    }
}