// Chapman Farm Almost 5K.
// Copyright (C) Eugene Bekker.

using Blazored.LocalStorage;
using CFA5K.WebApp.Providers;
using Postgrest.Interfaces;
using Supabase;

namespace CFA5K.WebApp;

/// <summary>
/// Application startup extensions.
/// </summary>
/// <remarks>
/// Adpated from:
/// https://github.com/acupofjose/supasharp-todo/blob/master/SupasharpTodo.Shared/Extensions/ServiceCollectionExtensions.cs
/// </remarks>
public static class StartupExtensions
{
    /// <summary>
    /// A helper extension to register the shared code and its dependencies.
    ///
    /// Requires that an <see cref="ILocalStorageProvider"/> has been registered as a Scoped Blazor service
    /// </summary>
    /// <param name="services"></param>
    public static IServiceCollection AddLocalAppServices(this IServiceCollection services)
    {
        services.AddScoped<SupabaseSessionProvider>();
        services.AddScoped(provider =>
        {
            var sessionProvider = provider.GetRequiredService<SupabaseSessionProvider>();
            return new Client(AppGlobals.SupabaseUrl, AppGlobals.SupabasePublicKey,
                new SupabaseOptions
                {
                    AutoRefreshToken = true,
                    AutoConnectRealtime = true,
                    SessionHandler = sessionProvider,
                });
        });

        ////// Register a postgrest cache provider
        ////services.AddScoped<IPostgrestCacheProvider, PostgrestCacheProvider>(p =>
        ////    new PostgrestCacheProvider(p.GetRequiredService<ILocalStorageProvider>()));

        ////// Register State Handlers and Services
        ////services.AddScoped<IAppStateService>(p => new AppStateService(p.GetRequiredService<Client>()));
        ////services.AddScoped<ITodoService>(p =>
        ////    new TodoService(p.GetRequiredService<IAppStateService>(),
        ////        p.GetRequiredService<Client>(),
        ////        p.GetRequiredService<IPostgrestCacheProvider>()));

        Console.WriteLine("Initialized Supabase Core.");

        return services;
    }
}
