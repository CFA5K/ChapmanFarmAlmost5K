// Chapman Farm Almost 5K.
// Copyright (C) Eugene Bekker.

using Blazored.LocalStorage;
using CFA5K.WebApp.Providers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace CFA5K.WebApp;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        builder.Services.AddBlazoredLocalStorage();
        builder.Services.AddAuthorizationCore();
        builder.Services.AddScoped<AuthenticationStateProvider, SupabaseAuthenticationStateProvider>();
        builder.Services.AddLocalAppServices();

        var app = builder.Build();
        var log = app.Services.GetRequiredService<ILogger<Program>>();
        var sup = app.Services.GetRequiredService<Supabase.Client>();

        log.LogInformation("Initializing Supabase client...");
        await sup.InitializeAsync();
        log.LogInformation("Loading session from persistence...");
        sup.Auth.LoadSession();
        log.LogInformation("Running the app...");
        await app.RunAsync();
    }
}
