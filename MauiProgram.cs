using DataHandlerLibrary.Models;
using DataHandlerLibrary.Services;
using EntityFrameworkDatabaseLibrary.Data;
using EntityFrameworkDatabaseLibrary.Models;
using EposRetail.Services;
using Microsoft.Extensions.Logging;

namespace EposRetail;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
        builder.Services.AddSingleton<ScreenInfoService>();
		builder.Services.AddScoped<DatabaseInitialization>(); // Changed from AddSingleton
		builder.Services.AddScoped<ProductServices>();        // Changed from AddSingleton
		builder.Services.AddScoped<DepartmentServices>();     // Changed from AddSingleton
        builder.Services.AddScoped<VatServices>();            // Changed from AddSingleton
		builder.Services.AddScoped<SalesTransactionServices>(); // Changed from AddSingleton
		builder.Services.AddScoped<SalesItemTransactionServices>(); // Changed from AddSingleton
        builder.Services.AddScoped<GeneralServices>();        // Changed from AddSingleton
		builder.Services.AddSingleton<CheckoutService>();     // Keep as singleton if it doesn't use DbContext directly
		builder.Services.AddScoped<PosUserServices>();       // Changed from AddSingleton
		builder.Services.AddScoped<UserSiteAccessServices>(); // Changed from AddSingleton
		builder.Services.AddScoped<SiteServices>();          // Changed from AddSingleton
        builder.Services.AddScoped<UserManagementServices>(); // Changed from AddSingleton
        builder.Services.AddScoped<VoidedProductServices>();
		builder.Services.AddScoped<DayLogServices>();
        builder.Services.AddScoped<PromotionServices>();
        builder.Services.AddScoped<MigrateDataServices>();
		builder.Services.AddScoped<PayoutServices>(); // Ensure DbContext is scoped
		builder.Services.AddScoped<PrinterServices>(); // Ensure DbContext is scoped

        builder.Services.AddSingleton<PosUser>();
		builder.Services.AddSingleton<Site>();
		builder.Services.AddSingleton<Till>();
		builder.Services.AddSingleton<DayLog>();
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
