using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using DeltaForce.OpenAI.DependencyInjection.Extentions.ServiceCollections;

namespace DeltaForce.OpenAI.App.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder.Services.AddHttpClients(new Dictionary<string, string>()
            {
                {"TempAPI", "http://192.168.254.93/" }
            });

            builder.Services.AddSingleton<MainPage>();

            builder.UseMauiApp<App>().ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            }).UseMauiCommunityToolkitMediaElement();
#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}