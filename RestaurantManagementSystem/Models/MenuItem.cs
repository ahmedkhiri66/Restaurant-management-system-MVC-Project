﻿namespace RestaurantManagementSystem.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public int PreparationTime { get; set; } //بالدقايق 
        public bool IsAvailable { get; set; } = true;
        public int DailyOrderCount { get; set; } = 0;
    }
}