using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

public class JwtAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private const string TokenKey = "authToken";
    private string _token;

    public JwtAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public async Task SetToken(string token)
    {
        _token = token;

        await _localStorage.SetItemAsync(TokenKey, token);

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task RemoveToken()
    {
        _token = null;
        await _localStorage.RemoveItemAsync(TokenKey);
        _httpClient.DefaultRequestHeaders.Authorization = null;

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public Task Initialization => _initializationTcs.Task;

    private readonly TaskCompletionSource<bool> _initializationTcs = new();

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        if (!OperatingSystem.IsBrowser())
        {
            // ⚠️ Gør intet – browseren håndterer det senere
            return anonymous;
        }

        try
        {
            if (string.IsNullOrWhiteSpace(_token))
            {
                _token = await _localStorage.GetItemAsync<string>(TokenKey);
            }

            if (!string.IsNullOrWhiteSpace(_token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            }

            var claims = string.IsNullOrWhiteSpace(_token)
                ? Array.Empty<Claim>()
                : ParseClaimsFromJwt(_token);

            var identity = new ClaimsIdentity(claims, string.IsNullOrWhiteSpace(_token) ? null : "jwt");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
        finally
        {
            // ✅ KUN her signalerer vi at initialization er færdig – uanset om token var fundet
            _initializationTcs.TrySetResult(true);
        }
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string token)
    {
        var payload = token.Split('.')[1];

        // Fix padding
        payload = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');

        var jsonBytes = Convert.FromBase64String(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
    }
}
