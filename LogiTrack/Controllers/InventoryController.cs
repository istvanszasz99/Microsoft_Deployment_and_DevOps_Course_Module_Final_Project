using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LogiTrack.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/inventory")]
[Authorize]
public class InventoryController : ControllerBase
{
    private readonly LogiTrackContext _context;
    private readonly IMemoryCache _cache;
    public InventoryController(LogiTrackContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        const string cacheKey = "inventory_list";
        if (!_cache.TryGetValue(cacheKey, out List<InventoryItem>? items) || items == null)
        {
            items = _context.InventoryItems.AsNoTracking().ToList();
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(30));
            _cache.Set(cacheKey, items, cacheEntryOptions);
        }
        return Ok(items);
    }

    [HttpPost]
    public IActionResult Add([FromBody] InventoryItem item)
    {
        _context.InventoryItems.Add(item);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetAll), new { id = item.ItemId }, item);
    }

    [Authorize(Roles = "Manager")]
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var item = _context.InventoryItems.Find(id);
        if (item == null) return NotFound();
        _context.InventoryItems.Remove(item);
        _context.SaveChanges();
        return NoContent();
    }
}
