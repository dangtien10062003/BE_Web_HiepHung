using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyHiep.Api.Data;
using MyHiep.Api.Models;

namespace MyHiep.Api.Controllers;

[ApiController]
[Route("api")]
public class StoreSettingsController(AppDbContext db) : ControllerBase
{
    [HttpGet("store-settings")]
    public async Task<StoreSettings> Get()
    {
        return await db.StoreSettings.FirstAsync(x => x.Id == 1);
    }

    [HttpPut("admin/store-settings")]
    public async Task<IActionResult> Update(StoreSettings settings)
    {
        var entity = await db.StoreSettings.FirstAsync(x => x.Id == 1);
        entity.BrandName = settings.BrandName;
        entity.Address = settings.Address;
        entity.Hotline = settings.Hotline;
        entity.ZaloUrl = settings.ZaloUrl;
        entity.FacebookUrl = settings.FacebookUrl;
        entity.OpeningHours = settings.OpeningHours;
        entity.DeliveryPolicy = settings.DeliveryPolicy;
        await db.SaveChangesAsync();
        return NoContent();
    }
}
