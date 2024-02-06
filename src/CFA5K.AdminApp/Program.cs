// Chapman Farm Almost 5K.
// Copyright (C) Eugene Bekker.

namespace CFA5K.AdminApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var devLocalPath = Path.Combine(Directory.GetCurrentDirectory(),
            "_IGNORE/appsettings.LOCAL.json");
        if (File.Exists(devLocalPath))
        {
            Console.WriteLine("*** FOUND DEV LOCAL APPSETTINGS");
            builder.Configuration.AddJsonFile(devLocalPath, optional: true);
        }

        builder.Services.AddDbContextFactory<AppDbContext>((services, optionsBuilder) =>
        {
            var constr = builder.Configuration.GetConnectionString(
                AppDbContext.DefaultConnectionStringName);
            optionsBuilder.UseNpgsql(constr);
        });

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
            app.UseHttpsRedirection();
        }

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}
