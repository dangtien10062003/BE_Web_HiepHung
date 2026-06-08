using Microsoft.EntityFrameworkCore;
using MyHiep.Api.Models;

namespace MyHiep.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<PriceItem> PriceItems => Set<PriceItem>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<BookingDetail> BookingDetails => Set<BookingDetail>();
    public DbSet<StoreSettings> StoreSettings => Set<StoreSettings>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Booking>()
            .Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(40);
        modelBuilder.Entity<Booking>()
            .Property(x => x.EstimatedWeight)
            .HasPrecision(18, 2);
        modelBuilder.Entity<Booking>()
            .HasOne(x => x.Service)
            .WithMany(x => x.Bookings)
            .HasForeignKey(x => x.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Service>().HasData(
            new Service { Id = 1, Name = "Giặt sấy theo kg", Description = "Phân loại màu, giặt sạch và sấy thơm cho nhu cầu hằng ngày.", SortOrder = 1 },
            new Service { Id = 2, Name = "Giặt hấp / giặt khô", Description = "Xử lý vest, áo khoác và đồ cao cấp cần chăm sóc kỹ.", SortOrder = 2 },
            new Service { Id = 3, Name = "Giặt chăn ga gối nệm", Description = "Làm sạch chăn ga, gối, nệm mỏng theo từng bộ.", SortOrder = 3 },
            new Service { Id = 4, Name = "Giặt giày dép", Description = "Vệ sinh giày, khử mùi và làm khô đúng cách.", SortOrder = 4 },
            new Service { Id = 5, Name = "Ủi đồ", Description = "Ủi thẳng áo sơ mi, quần tây, đồng phục và trang phục công sở.", SortOrder = 5 },
            new Service { Id = 6, Name = "Giao nhận tận nhà", Description = "Nhận và trả đồ trong khu vực bán kính 3km.", SortOrder = 6 }
        );

        modelBuilder.Entity<PriceItem>().HasData(
            new PriceItem { Id = 1, Name = "Giặt thường", PriceText = "15.000đ/kg", Note = "Phù hợp quần áo thường ngày", SortOrder = 1 },
            new PriceItem { Id = 2, Name = "Giặt + sấy thơm", PriceText = "25.000đ/kg", Note = "Sấy khô, thơm lâu", SortOrder = 2 },
            new PriceItem { Id = 3, Name = "Giặt hấp vest / áo khoác / đồ cao cấp", PriceText = "Từ 50.000đ/cái", Note = "Xử lý theo chất liệu", SortOrder = 3 },
            new PriceItem { Id = 4, Name = "Giặt chăn ga gối nệm", PriceText = "40.000đ - 80.000đ/bộ", Note = "Tùy kích thước", SortOrder = 4 },
            new PriceItem { Id = 5, Name = "Giặt giày", PriceText = "Từ 50.000đ/đôi", Note = "Vệ sinh, khử mùi", SortOrder = 5 },
            new PriceItem { Id = 6, Name = "Ủi đồ", PriceText = "Từ 10.000đ/cái", Note = "Ủi phẳng, treo gọn", SortOrder = 6 }
        );

        modelBuilder.Entity<Role>().HasData(new Role { Id = 1, Name = "Admin" });
        modelBuilder.Entity<User>().HasData(new User { Id = 1, UserName = "admin", PasswordHash = "demo-change-me", RoleId = 1 });
        modelBuilder.Entity<StoreSettings>().HasData(new StoreSettings { Id = 1 });
    }
}
