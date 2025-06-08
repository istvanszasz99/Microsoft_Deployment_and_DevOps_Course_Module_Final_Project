using LogiTrack.Models;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();

// --- Seed and test the database ---
using (var context = new LogiTrackContext())
{
    // Add test inventory item if none exist
    if (!context.InventoryItems.Any())
    {
        context.InventoryItems.Add(new InventoryItem
        {
            Name = "Pallet Jack",
            Quantity = 12,
            Location = "Warehouse A"
        });

        context.SaveChanges();
    }

    // Retrieve and print inventory to confirm
    var items = context.InventoryItems.ToList();
    foreach (var item in items)
    {
        item.DisplayInfo(); // Should print: Item: Pallet Jack | Quantity: 12 | Location: Warehouse A
    }
}

// Remove or comment out the old test block if you want to avoid duplicate output

app.Run();
