
using RestaurantManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantManagementSystem.Data
{
    public static class DataRepository
    {
        // Menu Items Data
        private static List<MenuItem> _menuItems = new List<MenuItem>
        {
            new MenuItem { Id = 1, Name = "Cheeseburger", Description = "Classic cheeseburger with fries",
                          Price = 8.99m, CategoryId = 1, PreparationTime = 15, IsAvailable = true },
            new MenuItem { Id = 2, Name = "Veggie Burger", Description = "Plant-based burger with sweet potato fries",
                          Price = 9.99m, CategoryId = 1, PreparationTime = 10, IsAvailable = true },
            new MenuItem { Id = 3, Name = "Caesar Salad", Description = "Fresh romaine with Caesar dressing",
                          Price = 7.49m, CategoryId = 2, PreparationTime = 8, IsAvailable = true },
            new MenuItem { Id = 4, Name = "Margherita Pizza", Description = "Classic tomato and mozzarella pizza",
                          Price = 12.99m, CategoryId = 3, PreparationTime = 20, IsAvailable = true },
            new MenuItem { Id = 5, Name = "Chocolate Cake", Description = "Rich chocolate cake with ice cream",
                          Price = 6.99m, CategoryId = 4, PreparationTime = 5, IsAvailable = true }
        };

        // Categories Data
        private static List<MenuCategory> _categories = new List<MenuCategory>
        {
            new MenuCategory { Id = 1, Name = "Burgers" },
            new MenuCategory { Id = 2, Name = "Salads" },
            new MenuCategory { Id = 3, Name = "Pizzas" },
            new MenuCategory { Id = 4, Name = "Desserts" }
        };

        // Orders Data
        private static List<Order> _orders = new List<Order>();
        private static List<Table> _tables = new List<Table>
        {
            new Table { Id = 1, Name = "Table 1", Capacity = 4, IsAvailable = true },
            new Table { Id = 2, Name = "Table 2", Capacity = 6, IsAvailable = true },
            new Table { Id = 3, Name = "Table 3", Capacity = 2, IsAvailable = true },
            new Table { Id = 4, Name = "Table 4", Capacity = 8, IsAvailable = true }
        };

        private static DateTime _lastAvailabilityReset = DateTime.Today;
        private static int _nextOrderId = 1;
        private static int _nextOrderItemId = 1;

        #region Menu Items Operations

        public static List<MenuItem> GetMenuItems() => _menuItems;

        public static MenuItem GetMenuItem(int id) => _menuItems.FirstOrDefault(m => m.Id == id);

        public static void AddMenuItem(MenuItem item)
        {
            item.Id = _menuItems.Count > 0 ? _menuItems.Max(m => m.Id) + 1 : 1;
            _menuItems.Add(item);
        }

        public static void UpdateMenuItem(MenuItem item)
        {
            var index = _menuItems.FindIndex(m => m.Id == item.Id);
            if (index >= 0)
            {
                _menuItems[index] = item;
            }
        }

        public static void DeleteMenuItem(int id)
        {
            _menuItems.RemoveAll(m => m.Id == id);
        }

        #endregion

        #region Categories Operations

        public static List<MenuCategory> GetCategories() => _categories;

        public static MenuCategory GetCategory(int id) => _categories.FirstOrDefault(c => c.Id == id);

        #endregion

        #region Orders Operations

        public static List<Order> GetOrders() => _orders;

        public static Order GetOrder(int id) => _orders.FirstOrDefault(o => o.Id == id);

        public static void AddOrder(Order order)
        {
            order.Id = _nextOrderId++;
            order.Items.ForEach(item => item.Id = _nextOrderItemId++);
            _orders.Add(order);
        }

        public static void UpdateOrder(Order order)
        {
            var index = _orders.FindIndex(o => o.Id == order.Id);
            if (index >= 0)
            {
                _orders[index] = order;
            }
        }

        public static void DeleteOrder(int id)
        {
            _orders.RemoveAll(o => o.Id == id);
        }

        #endregion

        #region Tables Operations

        public static List<Table> GetTables() => _tables;

        public static Table GetTable(int id) => _tables.FirstOrDefault(t => t.Id == id);

        public static void UpdateTable(Table table)
        {
            var index = _tables.FindIndex(t => t.Id == table.Id);
            if (index >= 0)
            {
                _tables[index] = table;
            }
        }

        #endregion

        #region Business Logic Methods

        public static void CheckAndResetAvailability()
        {
            if (DateTime.Today > _lastAvailabilityReset)
            {
                foreach (var item in _menuItems)
                {
                    item.DailyOrderCount = 0;
                    item.IsAvailable = true;
                }
                _lastAvailabilityReset = DateTime.Today;
            }
        }

        public static void IncrementDailyOrderCount(int menuItemId)
        {
            var item = GetMenuItem(menuItemId);
            if (item != null)
            {
                item.DailyOrderCount++;
                if (item.DailyOrderCount > 50)
                {
                    item.IsAvailable = false;
                }
            }
        }

        public static void ProcessOrderStatusUpdates()
        {
            foreach (var order in _orders.Where(o => o.Status == OrderStatus.Pending &&
                   (DateTime.Now - o.OrderTime).TotalMinutes > 5))
            {
                order.Status = OrderStatus.Preparing;
            }

            foreach (var order in _orders.Where(o => o.Status == OrderStatus.Preparing))
            {
                var maxPrepTime = order.Items.Max(i => GetMenuItem(i.MenuItemId)?.PreparationTime ?? 0);
                if ((DateTime.Now - order.OrderTime).TotalMinutes > maxPrepTime)
                {
                    order.Status = OrderStatus.Ready;
                }
            }
        }

        #endregion
    }
}