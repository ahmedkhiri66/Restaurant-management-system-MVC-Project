using System;
using System.Collections.Generic;

namespace RestaurantManagementSystem.Models
{
    public enum OrderType { DineIn, Takeout, Delivery }
    public enum OrderStatus { Pending, Preparing, Ready, Delivered, Cancelled }

    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderTime { get; set; } = DateTime.Now;
        public OrderType Type { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public string CustomerName { get; set; }
        public string DeliveryAddress { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public DateTime? EstimatedDeliveryTime { get; set; }
    }
}