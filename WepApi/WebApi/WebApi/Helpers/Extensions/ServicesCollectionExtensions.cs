using Microsoft.Extensions.Configuration;
using OpenAI.GPT3;
using OpenAI.GPT3.Managers;

namespace WebApi.Helpers.Extensions
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddOpenAIServices(this IServiceCollection services, IConfiguration configuration)
        {
            var gpt3 = new OpenAIService(new OpenAiOptions
            {
                ApiKey = configuration.GetValue<string>("OpenAIKey")
            });

            services.AddSingleton(gpt3);

            return services;
        }
    }
}
