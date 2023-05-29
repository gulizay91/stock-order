using Microsoft.EntityFrameworkCore;
using Order.Api.Persistence.Entities;
using Order.Api.Persistence.Enums;

namespace Order.Api.Persistence;

public sealed class OrderDatabaseContext : DbContext
{
  public OrderDatabaseContext(DbContextOptions<OrderDatabaseContext> options) : base(options)
  {
    //Database.EnsureDeleted();
    Database.EnsureCreated();
  }

  public DbSet<Entities.Order> Order { get; set; } = null!;

  public DbSet<OrderNotification> OrderNotification { get; set; } = null!;

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    // ps: dataanotation or fluent api validations not working in memory db!
    // ref: https://github.com/dotnet/efcore/issues/7228
    if (!optionsBuilder.IsConfigured) optionsBuilder.UseInMemoryDatabase("stock-order");
  }


  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Entities.Order>().HasKey(t => t.Id);
    modelBuilder.Entity<Entities.Order>().HasIndex(t => t.ClientId);
    modelBuilder.Entity<Entities.Order>().Property(t => t.OrderType).HasConversion<string>().IsRequired();
    modelBuilder.Entity<Entities.Order>().Property(t => t.Price).IsRequired();
    modelBuilder.Entity<Entities.Order>().Property(t => t.DayOfMonth).IsRequired();
    modelBuilder.Entity<Entities.Order>().Property(t => t.CryptoSymbol).HasConversion<string>().IsRequired();
    modelBuilder.Entity<Entities.Order>().Property(t => t.Status).HasDefaultValue(OrderStatus.Active)
      .HasConversion<string>();
    modelBuilder.Entity<Entities.Order>().HasIndex(t => new { t.ClientId, t.Status }).IsUnique()
      .HasFilter($"[{nameof(Entities.Order.Status)}] = '{nameof(OrderStatus.Active)}'");
    modelBuilder.Entity<Entities.Order>().OwnsOne(t => t.AuditInformation).Property(t => t.CreatedDate).IsRequired();
    modelBuilder.Entity<Entities.Order>().OwnsOne(t => t.AuditInformation).Property(t => t.LastModifiedDate);
    modelBuilder.Entity<Entities.Order>()
      .HasMany(b => b.OrderNotifications)
      .WithOne()
      .HasForeignKey("OrderId");
    modelBuilder.Entity<Entities.Order>()
      .Navigation(b => b.OrderNotifications)
      .UsePropertyAccessMode(PropertyAccessMode.Property);

    modelBuilder.Entity<OrderNotification>().HasKey(t => t.Id);
    modelBuilder.Entity<OrderNotification>().Property(t => t.Id).ValueGeneratedOnAdd();
    modelBuilder.Entity<OrderNotification>().Property("OrderId");
    modelBuilder.Entity<OrderNotification>().Property(t => t.NotificationType).HasConversion<string>().IsRequired();
    modelBuilder.Entity<OrderNotification>().HasIndex("OrderId", nameof(NotificationType)).IsUnique();
  }
}