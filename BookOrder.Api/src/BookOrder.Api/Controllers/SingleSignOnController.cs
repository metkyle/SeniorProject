namespace BookOrder.Api.Controllers
{
    using BookOrder.Services.SingleSignOn;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;

    [Route("api/[controller]")]
    public class SingleSignOnController : Controller
    {
        private SSOAuthenticationService ssoAuthenticationService { get; }

        public SingleSignOnController()
        {
            this.ssoAuthenticationService = new SSOAuthenticationService();
        }

        // GET: api/values
        [HttpGet]
        public object Get()
        {
            // return true;//TODO hardcoded for now, when this is implemented, remove commented code and use it
            return ssoAuthenticationService.IsSessionAuthenticated(HttpContext);
        }
        
        [HttpPost]
        public string Post()
        {
            return ssoAuthenticationService.GenerateSSOUrl();
        }

        [HttpPut]
        public string Put([FromBody]string ticket)
        {
            ssoAuthenticationService.AddSSOToSession(HttpContext, ticket);
            return ssoAuthenticationService.RetrieveUserDetailsFromSession(HttpContext);
        }

        
        //TODO use this method for a user who signs out
        [HttpDelete("{id}")]
        public void Delete()
        {
            ssoAuthenticationService.DeleteUserSession(HttpContext);
        }
    }
}
