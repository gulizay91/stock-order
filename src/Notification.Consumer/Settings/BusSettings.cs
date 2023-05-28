namespace Notification.Consumer.Settings;

public record BusSettings
{
  public string ClusterAddress { get; init; }
  public string UserName { get; init; }
  public string Password { get; init; }
}