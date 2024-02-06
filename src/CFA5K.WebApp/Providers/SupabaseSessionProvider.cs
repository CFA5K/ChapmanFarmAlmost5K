// Chapman Farm Almost 5K.
// Copyright (C) Eugene Bekker.

using Newtonsoft.Json;
using Supabase.Gotrue.Interfaces;
using Supabase.Gotrue;
using Blazored.LocalStorage;

namespace CFA5K.WebApp.Providers;

/// <summary>
/// This Session Provider supplies a strategy for caching/destroying
/// a session provided by Gotrue, locally for the user.
/// </summary>
/// <remarks>
/// Adapted from:
/// https://github.com/acupofjose/supasharp-todo/blob/master/SupasharpTodo.Shared/Providers/SupabaseSessionProvider.cs
/// </remarks>
public class SupabaseSessionProvider : IGotrueSessionPersistence<Session>
{
    private const string CacheKey = ".gotrue.cache";

    private readonly ILogger<SupabaseSessionProvider> _logger;
    private readonly ISyncLocalStorageService _localStorage;

    public SupabaseSessionProvider(ILogger<SupabaseSessionProvider> logger,
        ISyncLocalStorageService localStorage)
    {
        _logger = logger;
        _localStorage = localStorage;
    }

    public void DestroySession() => _localStorage.RemoveItem(CacheKey);

    public void SaveSession(Session session)
    {
        try
        {
            var serialized = JsonConvert.SerializeObject(session);
            _localStorage.SetItem(CacheKey, serialized);
        }
        catch (Exception err)
        {
            _logger.LogError(err, "failed to save session with");
        }
    }

    public Session? LoadSession()
    {
        try
        {
            var json = _localStorage.GetItem<string>(CacheKey);
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }

            var session = JsonConvert.DeserializeObject<Session>(json);
            if (!(session?.Expired() ?? true))
            {
                return session;
            }

            return null;
        }
        catch (Exception err)
        {
            _logger.LogError(err, "failed to load session");
            return null;
        }
    }
}
