// Chapman Farm Almost 5K.
// Copyright (C) Eugene Bekker.

using Supabase.Gotrue.Interfaces;
using Supabase.Gotrue;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace CFA5K.WebApp.Providers;

/// <summary>
/// Creates a link between <see cref="_supabase"/> and <see cref="AuthenticatedState"/>
/// to provide support for using Gotrue with Blazor's built in Authentication handler.
/// </summary>
/// <remarks>
/// Adapted from:
/// https://github.com/acupofjose/supasharp-todo/blob/master/SupasharpTodo.Shared/Providers/SupabaseAuthenticationStateProvider.cs
/// </remarks>
public class SupabaseAuthenticationStateProvider : AuthenticationStateProvider, IDisposable
{
    public const string AuthenticationType = "supabase";

    private readonly Supabase.Client _supabase;
    private readonly ILogger<SupabaseAuthenticationStateProvider> _logger;

    public SupabaseAuthenticationStateProvider(
        ILogger<SupabaseAuthenticationStateProvider> logger,
        Supabase.Client supabase)
    {
        _logger = logger;
        _supabase = supabase;
        _supabase.Auth.AddStateChangedListener(SupabaseAuthStateChanged);

        _logger.LogInformation("initialized");
    }

    public void Dispose()
    {
        _supabase.Auth.RemoveStateChangedListener(SupabaseAuthStateChanged);
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        _supabase.Auth.LoadSession();

        if (_supabase.Auth.CurrentUser == null)
        {
            _logger.LogInformation("an authenticated user not found, returning as anonymous.");
            return Task.FromResult(AnonymousState);
        }

        return Task.FromResult(AuthenticatedState);
    }

    private AuthenticationState AnonymousState => new(new ClaimsPrincipal(new ClaimsIdentity()));

    /// <summary>
    /// Creates an <see cref="AuthenticationState"/> that is either
    /// Anonymous or Authenticated if Gotrue has a current user.
    /// </summary>
    private AuthenticationState AuthenticatedState
    {
        get
        {
            var user = _supabase.Auth.CurrentUser;

            if (user == null)
            {
                return AnonymousState;
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Authentication, AuthenticationType),
            };
            AddIf(claims, ClaimTypes.NameIdentifier, user.Id);
            AddIf(claims, ClaimTypes.Email, user.Email);
            AddIf(claims, ClaimTypes.Role, user.Role);

            var metaKeys = user.UserMetadata.Keys;
            var nameKey = metaKeys.FirstOrDefault(x => x == "preferred_username")
                ?? metaKeys.FirstOrDefault(x => x == "user_name")
                ?? metaKeys.FirstOrDefault(x => x == "email");
            var name = nameKey == null
                ? user.Email
                : user.UserMetadata[nameKey].ToString();
            AddIf(claims, ClaimTypes.Name, name);

            var firstId = user.Identities.FirstOrDefault();
            var providers = string.Join(",", user.Identities
                .Where(x => !string.IsNullOrEmpty(x.Provider))
                .Select(x => x.Provider));
            if (firstId != null)
            {
                if (firstId.Provider != null)
                {
                    claims.Add(new(ClaimTypes.AuthenticationMethod, firstId.Provider));
                    claims.Add(new(ClaimTypes.System, providers));
                }
            }

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, AuthenticationType)));
        }
    }

    List<Claim> AddIf(List<Claim> list, string name, string? value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            list.Add(new(name, value));
        }
        return list;
    }

    /// <summary>
    /// Adds a listener on the supabase client that will notify
    /// components of a change in authentication state in real-time.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="state"></param>
    private void SupabaseAuthStateChanged(IGotrueClient<User, Session> sender, Constants.AuthState state)
    {
        switch (state)
        {
            case Constants.AuthState.SignedIn:
                NotifyAuthenticationStateChanged(Task.FromResult(AuthenticatedState));
                break;
            case Constants.AuthState.SignedOut:
                NotifyAuthenticationStateChanged(Task.FromResult(AnonymousState));
                break;
        }
    }
}
