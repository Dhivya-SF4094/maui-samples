using Microsoft.Extensions.Logging;
using PickerDemo.Control;
using PickerDemo.Handlers;

namespace PickerDemo;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Register custom handler for Windows, Android, and iOS
        builder.ConfigureMauiHandlers(handlers =>
        {
#if ANDROID
            handlers.AddHandler<CustomPicker, CustomPickerHandler>();
#elif WINDOWS
            handlers.AddHandler<CustomPicker, CustomPickerHandler>();
#elif IOS || MACCATALYST
            handlers.AddHandler<CustomPicker, CustomPickerHandler>();
#endif
        });

#if DEBUG
        builder.Services.AddLogging(logging =>
        {
            logging.AddDebug();
        });
#endif

        return builder.Build();
    }
}