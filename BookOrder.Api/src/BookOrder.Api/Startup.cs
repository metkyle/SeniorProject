using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Authentication;
using GSS.Authentication.CAS;
using GSS.Authentication.CAS.AspNetCore;
using System.Security.Claims;
using Microsoft.Extensions.Options;

namespace BookOrder.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).
                AddCookie(options =>
                {
                    options.LoginPath = "/login";
                    options.LogoutPath = "/logout";
                    options.Events = new CookieAuthenticationEvents
                    {
                        OnSigningOut = context =>
                        {
                            // Single Sign-Out
                            var casUrl = new Uri(Configuration["Authentication:CAS:CasServerUrlBase"]);
                            var serviceUrl = new Uri(context.Request.GetEncodedUrl()).GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped);
                            var redirectUri = UriHelper.BuildAbsolute(casUrl.Scheme, new HostString(casUrl.Host, casUrl.Port), casUrl.LocalPath, "/logout", QueryString.Create("service", serviceUrl));

                            var logoutRedirectContext = new RedirectContext<CookieAuthenticationOptions>(
                                context.HttpContext,
                                context.Scheme,
                                context.Options,
                                context.Properties,
                                redirectUri
                            );
                            context.Response.StatusCode = 204; //Prevent RedirectToReturnUrl
                            context.Options.Events.RedirectToLogout(logoutRedirectContext);
                            return Task.CompletedTask;
                        }
                    };
                }).AddCAS(options =>
                {
                    options.CallbackPath = "/signin-cas";
                    options.CasServerUrlBase = Configuration["Authentication:CAS:CasServerUrlBase"];
                    options.SaveTokens = true;
                    options.Events = new CasEvents
                    {
                        OnCreatingTicket = context =>
                        {
                            // first_name, family_name, display_name, email, verified_email
                            var assertion = context.Assertion;
                            if (assertion == null || !assertion.Attributes.Any()) return Task.CompletedTask;
                            if (!(context.Principal.Identity is ClaimsIdentity identity)) return Task.CompletedTask;
                            var email = assertion.Attributes["email"]?.FirstOrDefault();
                            if (!string.IsNullOrEmpty(email))
                            {
                                identity.AddClaim(new Claim(ClaimTypes.Email, email));
                            }
                            var name = assertion.Attributes["display_name"]?.FirstOrDefault();
                            if (!string.IsNullOrEmpty(name))
                            {
                                identity.AddClaim(new Claim(ClaimTypes.Name, name));
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddMvc();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSession();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseSession();
            app.UseMvc();
            app.UseAuthentication();

            // Choose an authentication type
            app.Map("/login", branch =>
            {
                branch.Run(async context =>
                {
                    var scheme = "CAS";
                    if (!string.IsNullOrEmpty(scheme))
                    {
                        // By default the client will be redirect back to the URL that issued the challenge (/login?authscheme=foo),
                        // send them to the home page instead (/).
                        await context.ChallengeAsync(scheme, new AuthenticationProperties { RedirectUri = "/" });
                        return;
                    }

                    context.Response.ContentType = "text/html";
                    await context.Response.WriteAsync(@"<!DOCTYPE html><html><head><meta charset=""utf-8""></head><body>");
                    await context.Response.WriteAsync("<p>Choose an authentication scheme:</p>");
                    foreach (var type in context.RequestServices.GetRequiredService<IOptions<AuthenticationOptions>>().Value.Schemes)
                    {
                        if (string.IsNullOrEmpty(type.DisplayName)) continue;
                        await context.Response.WriteAsync($"<a href=\"?authscheme={type.Name}\">{type.DisplayName ?? type.Name}</a><br>");
                    }
                    await context.Response.WriteAsync("</body></html>");
                });
            });

            // Sign-out to remove the user cookie.
            app.Map("/logout", branch =>
            {
                branch.Run(async context =>
                {
                    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    context.Response.Redirect("/");
                });
            });

            app.Run(async context =>
            {
                // CookieAuthenticationOptions.AutomaticAuthenticate = true (default) causes User to be set
                var user = context.User;

                // This is what [Authorize] calls
                // var user = await context.Authentication.AuthenticateAsync(AuthenticationManager.AutomaticScheme);

                // Deny anonymous request beyond this point.
                if (user == null || !user.Identities.Any(identity => identity.IsAuthenticated))
                {
                    // This is what [Authorize] calls
                    // The cookie middleware will intercept this 401 and redirect to /login
                    await context.ChallengeAsync();
                    return;
                }

                // Display user information
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync(@"<!DOCTYPE html><html><head><meta charset=""utf-8""></head><body>");
                await context.Response.WriteAsync($"<h1>Hello {user.Identity.Name ?? "anonymous"}</h1>");
                await context.Response.WriteAsync("<ul>");
                foreach (var claim in user.Claims)
                {
                    await context.Response.WriteAsync($"<li>{claim.Type}: {claim.Value}<br>");
                }
                await context.Response.WriteAsync("</ul>");
                await context.Response.WriteAsync("Tokens:<ol>");
                await context.Response.WriteAsync($"<li>Access Token: {await context.GetTokenAsync("access_token")}</li>");
                await context.Response.WriteAsync($"<li>Refresh Token: {await context.GetTokenAsync("refresh_token")}</li>");
                await context.Response.WriteAsync($"<li>Token Type: {await context.GetTokenAsync("token_type")}</li>");
                await context.Response.WriteAsync($"<li>Expires At: {await context.GetTokenAsync("expires_at")}</li>");
                await context.Response.WriteAsync("</ol>");
                await context.Response.WriteAsync("<a href=\"/logout\">Logout</a><br>");
                await context.Response.WriteAsync("</body></html>");
            });
        }
    }
}
