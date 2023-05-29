using Microsoft.EntityFrameworkCore;
using Notification.Consumer.Persistence.Entities;

namespace Notification.Consumer.Persistence;

public class NotificationDatabaseContext : DbContext
{
  public NotificationDatabaseContext(DbContextOptions<NotificationDatabaseContext> options) : base(options)
  {
    //Database.EnsureDeleted();
    Database.EnsureCreated();
  }

  public DbSet<OrderNotificationEvent> OrderNotificationEvent { get; set; } = default!;

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    // ps: dataanotation or fluent api validations not working in memory db!
    // ref: https://github.com/dotnet/efcore/issues/7228
    if (!optionsBuilder.IsConfigured) optionsBuilder.UseInMemoryDatabase("stock-notification");
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<OrderNotificationEvent>().HasKey(t => t.Id);
    modelBuilder.Entity<OrderNotificationEvent>().Property(t => t.Id).ValueGeneratedNever();
    modelBuilder.Entity<OrderNotificationEvent>().Property(t => t.ClientId).IsRequired();
    modelBuilder.Entity<OrderNotificationEvent>().HasIndex(t => t.OrderId);
    modelBuilder.Entity<OrderNotificationEvent>().Property(t => t.NotificationType).HasConversion<string>()
      .IsRequired();
    modelBuilder.Entity<OrderNotificationEvent>().Property(t => t.Message).IsRequired();
    modelBuilder.Entity<OrderNotificationEvent>().Property(t => t.CreatedDate).IsRequired();
  }
}