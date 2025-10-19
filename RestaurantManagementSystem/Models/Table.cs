namespace RestaurantManagementSystem.Models
{
    public class Table
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}