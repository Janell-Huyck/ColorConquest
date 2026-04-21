using ColorConquest.Core.ViewModels;
using ColorConquest.Views;
using Microsoft.Extensions.Logging;

namespace ColorConquest
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>(services => new App(services))
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            // Register all Core services for DI
            builder.Services.AddSingleton<ColorConquest.Core.Services.ThemePreferences>(sp => new ColorConquest.Core.Services.ThemePreferences(AppServices.Preferences));
            builder.Services.AddSingleton<ColorConquest.Core.Services.GameBoardPreferences>(sp => new ColorConquest.Core.Services.GameBoardPreferences(AppServices.Preferences));
            builder.Services.AddSingleton<ColorConquest.Core.Services.GameDisplayPreferences>(sp => new ColorConquest.Core.Services.GameDisplayPreferences(AppServices.Preferences));
            builder.Services.AddSingleton<ColorConquest.Core.Services.TileColorPreferences>(sp => new ColorConquest.Core.Services.TileColorPreferences(AppServices.Preferences));
            builder.Services.AddSingleton<ColorConquest.Core.ViewModels.ThemeViewModel>();
            builder.Services.AddSingleton<SettingsViewModel>(sp =>
                new SettingsViewModel(
                    sp.GetRequiredService<ColorConquest.Core.Services.ThemePreferences>(),
                    sp.GetRequiredService<ColorConquest.Core.Services.GameBoardPreferences>(),
                    sp.GetRequiredService<ColorConquest.Core.Services.GameDisplayPreferences>(),
                    sp.GetRequiredService<ColorConquest.Core.Services.TileColorPreferences>(),
                    sp.GetRequiredService<ColorConquest.Core.ViewModels.ThemeViewModel>()
                )
            );
            builder.Services.AddTransient<SettingsPage>();
            return builder.Build();
        }
    }
}
