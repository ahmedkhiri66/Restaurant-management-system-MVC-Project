using Microsoft.AspNetCore.Mvc;
using RestaurantManagementSystem.Data;
using System.Linq;

namespace RestaurantManagementSystem.Controllers
{
    public class AnalyticsController : Controller
    {
        public IActionResult Index()
        {
            var orders = DataRepository.GetOrders();

            if (!orders.Any())
            {
                ViewBag.Message = "No order data available for analytics";
                return View();
            }

            // Sales by category
            var menuItems = DataRepository.GetMenuItems();
            var categories = DataRepository.GetCategories();

            var salesByCategory = categories.Select(c => new
            {
                Category = c.Name,
                Revenue = orders.SelectMany(o => o.Items)
                               .Where(i => menuItems.Any(m => m.Id == i.MenuItemId && m.CategoryId == c.Id))
                               .Sum(i => i.Subtotal)
            }).ToList();

            ViewBag.SalesByCategoryLabels = salesByCategory.Select(s => s.Category).ToList();
            ViewBag.SalesByCategoryData = salesByCategory.Select(s => s.Revenue).ToList();

            // Sales by hour
            var salesByHour = Enumerable.Range(10, 12).Select(hour => new
            {
                Hour = $"{hour}:00",
                Revenue = orders.Where(o => o.OrderTime.Hour == hour)
                               .Sum(o => o.Subtotal)
            }).ToList();

            ViewBag.SalesByHourLabels = salesByHour.Select(s => s.Hour).ToList();
            ViewBag.SalesByHourData = salesByHour.Select(s => s.Revenue).ToList();

            // Top selling items
            var topItems = orders.SelectMany(o => o.Items)
                                .GroupBy(i => i.MenuItemName)
                                .Select(g => new
                                {
                                    Item = g.Key,
                                    Quantity = g.Sum(i => i.Quantity)
                                })
                                .OrderByDescending(x => x.Quantity)
                                .Take(5)
                                .ToList();

            ViewBag.TopItemsLabels = topItems.Select(t => t.Item).ToList();
            ViewBag.TopItemsData = topItems.Select(t => t.Quantity).ToList();

            return View();
        }
    }
}