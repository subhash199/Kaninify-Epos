using DataHandlerLibrary.Services;
using EntityFrameworkDatabaseLibrary.Data;
using EposRetail.Services;
using Microsoft.Extensions.Logging;

namespace EposRetail;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
        builder.Services.AddSingleton<ScreenInfoService>();
		builder.Services.AddSingleton<DatabaseInitialization>();
		builder.Services.AddSingleton<ProductServices>();
		builder.Services.AddSingleton<DepartmentServices>();
        builder.Services.AddSingleton<VatServices>();
        builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
