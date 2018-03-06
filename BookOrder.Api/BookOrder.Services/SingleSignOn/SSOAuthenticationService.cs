namespace BookOrder.Services.SingleSignOn
{
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class SSOAuthenticationService
    {
        public bool IsSessionAuthenticated(HttpContext httpContext)
        {
            return httpContext.Session.GetString(httpContext.Session.Id)
                ?.Equals("true", StringComparison.OrdinalIgnoreCase) ?? false;
        }

        public void AddSSOToSession(HttpContext httpContext, string userData)
        {
            httpContext.Session.SetString(httpContext.Session.Id, "true");
            //TODO make the request to the EWU SSO and parse user information here
            httpContext.Session.SetString("test", GatherUserInformationFromAuthService(userData).Result);
            httpContext.Session.SetString("user", JsonConvert.SerializeObject(userData));
        }

        //TODO wire this up to return a user object, remove magic "user" string and add to a constants file or enum
        public string RetrieveUserDetailsFromSession(HttpContext httpContext)
        {
            AddSSOToSession(httpContext, @"ST-422573-sael56LqdCMFH0g6sWR5-i02");
            return httpContext.Session.GetString("user");
        }

        public void DeleteUserSession(HttpContext httpContext)
        {
            //Consideration: This clears everything except the session cookie... if this is a problem then revisit the solution
            httpContext.Session.Clear();
        }

        public string GenerateSSOUrl()
        {
            return SSOConfigurationSettings.CASLoginUrl + SSOConfigurationSettings.CASLoginParameters;
        }

        private async Task<string> GatherUserInformationFromAuthService(string userTicket)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(SSOConfigurationSettings.CASAuthenticationUrl);
                var result = await client.GetAsync(
                    $"?service=https%3A%2F%2Fcanvas.ewu.edu%2Flogin%2Fcas&ticket={userTicket}");
                string resultContent = await result.Content.ReadAsStringAsync();
                return resultContent;
            }
        }
    }
}
