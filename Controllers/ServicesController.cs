using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyHiep.Api.Data;
using MyHiep.Api.Models;

namespace MyHiep.Api.Controllers;

[ApiController]
[Route("api")]
public class ServicesController(AppDbContext db) : ControllerBase
{
    [HttpGet("services")]
    public Task<List<Service>> GetServices() => db.Services.Where(x => x.IsActive).OrderBy(x => x.SortOrder).ToListAsync();

    [HttpGet("admin/services")]
    public Task<List<Service>> AdminServices() => db.Services.OrderBy(x => x.SortOrder).ToListAsync();

    [HttpPost("admin/services")]
    public async Task<ActionResult<Service>> Create(Service service)
    {
        db.Services.Add(service);
        await db.SaveChangesAsync();
        return service;
    }

    [HttpPut("admin/services/{id:int}")]
    public async Task<IActionResult> Update(int id, Service service)
    {
        if (id != service.Id) return BadRequest();
        db.Services.Update(service);
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("admin/services/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var service = await db.Services.FindAsync(id);
        if (service is null) return NotFound();
        db.Services.Remove(service);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
