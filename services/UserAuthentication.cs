using System.Security.Claims;
using Clerk.BackendAPI.Helpers.Jwks;
namespace AwakenedApi.services;


public class UserAuthentication
{
    private string ClerkSecret { get; init; }
    
    public UserAuthentication(IConfiguration configuration)
    {
        ClerkSecret = configuration["CLERK_SECRET_KEY"];
    }  
    
    public  async Task<bool> IsSignedInAsync(HttpContext request)
    {
        var options = new AuthenticateRequestOptions(
            secretKey: ClerkSecret,
            authorizedParties:["http://localhost:3000", "https://awakened-frontend.vercel.app"]
        );
        var requestState = await AuthenticateRequest.AuthenticateRequestAsync(request.Request, options);
        if (!requestState.IsAuthenticated)
        {
            return false;
        }
        var userId = requestState.Claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        request.Items["userId"] = userId;
        return true;
    }
    
}