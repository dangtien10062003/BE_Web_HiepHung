using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyHiep.Api.Data;
using MyHiep.Api.Models;

namespace MyHiep.Api.Controllers;

[ApiController]
[Route("api")]
public class PricesController(AppDbContext db) : ControllerBase
{
    [HttpGet("prices")]
    public Task<List<PriceItem>> GetPrices() => db.PriceItems.Where(x => x.IsActive).OrderBy(x => x.SortOrder).ToListAsync();

    [HttpGet("admin/prices")]
    public Task<List<PriceItem>> AdminPrices() => db.PriceItems.OrderBy(x => x.SortOrder).ToListAsync();

    [HttpPost("admin/prices")]
    public async Task<ActionResult<PriceItem>> Create(PriceItem item)
    {
        db.PriceItems.Add(item);
        await db.SaveChangesAsync();
        return item;
    }

    [HttpPut("admin/prices/{id:int}")]
    public async Task<IActionResult> Update(int id, PriceItem item)
    {
        if (id != item.Id) return BadRequest();
        db.PriceItems.Update(item);
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("admin/prices/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await db.PriceItems.FindAsync(id);
        if (item is null) return NotFound();
        db.PriceItems.Remove(item);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
