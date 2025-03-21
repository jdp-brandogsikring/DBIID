using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

public class AuthStartupService
{
    private readonly JwtAuthenticationStateProvider _authProvider;

    public Task Initialization => _initialization.Task;
    private readonly TaskCompletionSource<bool> _initialization = new();

    public bool IsReady => _initialization.Task.IsCompletedSuccessfully;

    public AuthStartupService(AuthenticationStateProvider provider)
    {
        _authProvider = (JwtAuthenticationStateProvider)provider;
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        try
        {
            await _authProvider.Initialization; // venter på token-indlæsning
            _initialization.TrySetResult(true);
        }
        catch
        {
            _initialization.TrySetResult(false); // stadig klar, men uden auth
        }
    }
}
