using Microsoft.Extensions.DependencyInjection;
using Notification.Consumer.Clients;

namespace Notification.Consumer.Registrations;

public static class HttpClientRegister
{
  public static void RegisterHttpClients(this IServiceCollection serviceCollection)
  {
    // not important this part, just divide provider client
    // serviceCollection.AddHttpClient<ISmsProviderClient, SmsProviderClient>();
    serviceCollection.AddSingleton<ISmsProviderClient, SmsProviderClient>();
    serviceCollection.AddSingleton<IEmailProviderClient, EmailProviderClient>();
    serviceCollection.AddSingleton<IPushNotificationProviderClient, PushNotificationProviderClient>();
  }
}