using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Net.Http.Headers;

namespace Site {
    public class Program {
        public static void Main(string[] args) {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // avoid default claims mapping (from JWT-token to User.Claims) - example: use "sub" as UserId
            var builder = WebApplication.CreateBuilder(args);
            
            // Add services to the container.
            ConfigureServices(builder.Services);
            WebApplication app = builder.Build();
            app.Urls.Add(SettingsCore.Settings.Site_applicationUrl);


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment()) {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services) {
            services.AddControllersWithViews();

            // HttpClient used for accessing API
            services.AddHttpClient("APIClient", client => {
                client.BaseAddress = new Uri("");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            });

            services.AddAuthentication(options => {
                        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                    })
                    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options => {
                        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        options.Authority = SettingsCore.Settings.IPD_applicationUrl;
                        options.ClientId = SettingsCore.Settings.Site_ClientId;
                        options.ResponseType = "code";
                        //options.UsePkce = true;  // true is default value
                        //options.CallbackPath = new PathString(); // use default value which is ".../signin-oidc"
                        //options.SignedOutCallbackPath =   // also used default uri ".../signout-callback-oidc"
                        options.Scope.Add("openid");
                        options.Scope.Add("profile");
                        options.ClaimActions.Remove("nbf"); // removes action that delete "nbf" (not before) claim (so it will be actually sent to client)
                        options.ClaimActions.DeleteClaim("sid"); // delete "session id" claim from response to client
                        options.SaveTokens = true;
                        options.ClientSecret = SettingsCore.Settings.Site_secret;
                        options.GetClaimsFromUserInfoEndpoint = true;
                    });
        }
    }
}