using E_commerce.Constants;
using E_commerce.Service.Interfaces;
using E_commerce.Settings;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

namespace E_commerce.Middlewares
{
    public class RefreshTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtConfig _jwtConfig;

        public RefreshTokenMiddleware(RequestDelegate next, IOptions<JwtConfig> options)
        {
            _next = next;
            _jwtConfig = options.Value;
        }

        public async Task InvokeAsync(HttpContext context, IRefreshTokenService refreshTokenService)
        {
            if (!context.Request.Cookies.TryGetValue(CookieConstants.AccessToken, out var accessToken))
            {
                await _next(context);
                return;
            }

            if (!IsExpired(accessToken))
            {
                await _next(context);
                return;
            }

            if (!context.Request.Cookies.TryGetValue(CookieConstants.RefreshToken, out var refreshToken))
            {
                await _next(context);
                return;
            }

            var response = await refreshTokenService.RefreshAsync(refreshToken);


            if (response.ResponseCode == 200)
            {
                context.Items[CookieConstants.AccessToken] = response.AccessToken;
                context.Response.Cookies.Append(CookieConstants.AccessToken, response.AccessToken!,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddMinutes(_jwtConfig.ExpiryMinutes)
                    });

                context.Response.Cookies.Append(CookieConstants.RefreshToken,response.RefreshToken!,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddDays(_jwtConfig.RefreshTokenExpiryDays)
                    });
            }
            else
            {
                context.Response.Cookies.Delete(CookieConstants.AccessToken);
                context.Response.Cookies.Delete(CookieConstants.RefreshToken);
            }

            await _next(context);
        }

        private static bool IsExpired(string jwt)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();

                if (!handler.CanReadToken(jwt))
                    return false;

                var token = handler.ReadJwtToken(jwt);

                return token.ValidTo <= DateTime.UtcNow;
            }
            catch
            {
                return false;
            }
        }
    }
}
