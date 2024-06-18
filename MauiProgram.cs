using Microsoft.Extensions.Logging;

namespace _90sWeatherApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Star 4 Radar.ttf", "Star4Radar");
                    fonts.AddFont("Star3000.ttf", "Star3000");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
