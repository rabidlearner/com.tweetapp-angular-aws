using com.tweetapp.api.Models;
using com.tweetapp.api.Repo.Implementation;
using com.tweetapp.api.Repo.IRepo;
using MassTransit;

namespace com.tweetapp.api.Helpers
{
    public static class RabbitMqHelper
    {
        public static void ConfigureRabbitMq(this IServiceCollection services)
        {            
            services.AddMassTransit(x =>
            {
                x.AddConsumer<TweetsRepo>();

                //x.SetKebabCaseEndpointNameFormatter();

                x.UsingRabbitMq((context, cfg) => {
                    cfg.Host("amqp://guest:guest@localhost:5672");
                    cfg.ReceiveEndpoint("tweet-queue", c =>
                    {
                        c.ConfigureConsumer<TweetsRepo>(context);
                    });
            });
            });
            //services.AddMassTransitHostedService();
        }
    }
}
