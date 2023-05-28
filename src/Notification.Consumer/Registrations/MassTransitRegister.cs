using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notification.Consumer.Constants;
using Notification.Consumer.Consumers;
using Notification.Consumer.Settings;
using RabbitMQ.Client;

namespace Notification.Consumer.Registrations;

public static class MassTransitRegister
{
  public static void RegisterMassTransit(this IServiceCollection serviceCollection, IConfiguration configuration)
  {
    var busSettings = new BusSettings();
    configuration.GetSection(nameof(BusSettings)).Bind(busSettings);

    serviceCollection.AddMassTransit(x =>
    {
      x.AddConsumer<SmsConsumer>();
      x.AddConsumer<EmailConsumer>();
      x.AddConsumer<PushNotificationConsumer>();

      x.UsingRabbitMq((context, cfg) =>
      {
        // observe pre - post - fault states of consumer, if we need 
        //cfg.ConnectConsumeObserver(new ConsumeObserver(new ConsumerLogger()));

        cfg.Host(new Uri($"{busSettings.ClusterAddress}"), a =>
        {
          a.Username(busSettings.UserName);
          a.Password(busSettings.Password);
        });

        // ps: if we want this configurable on appsetting we can move, but for now this is enough i think
        cfg.ReceiveEndpoint(QueueNames.SmsConsumerQueueName, ep =>
        {
          ep.AutoDelete = false;

          ep.Durable = true;

          ep.ExchangeType = ExchangeType.Fanout;

          ep.UseMessageRetry(r => { r.Interval(3, TimeSpan.FromMilliseconds(3000)); });

          ep.PrefetchCount = 10;

          // circuit breaker
          ep.UseKillSwitch(options => options
            .SetActivationThreshold(10)
            .SetTripThreshold(0.15)
            .SetRestartTimeout(m: 1));

          ep.ConfigureConsumer<SmsConsumer>(context);
        });

        cfg.ReceiveEndpoint(QueueNames.EmailConsumerQueueName, ep =>
        {
          ep.AutoDelete = false;

          ep.Durable = true;

          ep.ExchangeType = ExchangeType.Fanout;

          ep.UseMessageRetry(r => { r.Interval(3, TimeSpan.FromMilliseconds(3000)); });

          ep.PrefetchCount = 10;

          ep.UseKillSwitch(options => options
            .SetActivationThreshold(10)
            .SetTripThreshold(0.15)
            .SetRestartTimeout(m: 1));

          ep.ConfigureConsumer<EmailConsumer>(context);
        });

        cfg.ReceiveEndpoint(QueueNames.PushNotificationConsumerQueueName, ep =>
        {
          ep.AutoDelete = false;

          ep.Durable = true;

          ep.ExchangeType = ExchangeType.Fanout;

          ep.UseMessageRetry(r => { r.Interval(3, TimeSpan.FromMilliseconds(3000)); });

          ep.PrefetchCount = 10;

          ep.UseKillSwitch(options => options
            .SetActivationThreshold(10)
            .SetTripThreshold(0.15)
            .SetRestartTimeout(m: 1));

          ep.ConfigureConsumer<PushNotificationConsumer>(context);
        });
      });
    });
  }
}