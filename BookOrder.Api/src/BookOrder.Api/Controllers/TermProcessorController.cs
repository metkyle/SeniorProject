namespace BookOrder.Api.Controllers
{
    using BookOrder.Repositories.Terms;
    using BookOrder.Services.Terms;
    using BookOrder.Core.Models;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    [Route("api/[controller]")]
    public class TermProcessorController
    {
        private TermDataService termDataService { get; }

        public TermProcessorController()
        {
            termDataService = new TermDataService();
        }

        [HttpGet]
        public IEnumerable<object> Get()
        {
            //TODO
            //get actual dept number
            return termDataService.GenerateTermsForDept(0);
        }

        [HttpGet("{id}")]
        public IEnumerable<object> Get(int id)
        {
            //TODO
            //get actual dept number
            return termDataService.GenerateTermsForDept(id);
        }

        [HttpPost]
        public Term Post([FromBody]JObject termDetailsToSave)
        {
            var term = termDetailsToSave.SelectToken("Term").ToObject<Term>();

            termDataService.AddTerm(term);
            return term;
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
            throw new MethodAccessException();
        }

        [HttpDelete]
        public decimal Delete([FromBody]JObject termDetailsToDelete)
        {
            var term = termDetailsToDelete.SelectToken("Term").ToObject<Term>();

            return termDataService.DeleteTerm(term);
        }
    }
}
 