using System.ComponentModel.DataAnnotations;

namespace LogiTrack.Models
{
    public class InventoryItem
    {
        [Key]
        public int ItemId { get; set; }

        [Required]
        public string Name { get; set; }

        public int Quantity { get; set; }

        [Required]
        public string Location { get; set; }

        public void DisplayInfo()
        {
            Console.WriteLine($"Item: {Name} | Quantity: {Quantity} | Location: {Location}");
        }
    }
}
