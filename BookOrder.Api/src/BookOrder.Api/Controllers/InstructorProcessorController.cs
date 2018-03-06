namespace BookOrder.Api.Controllers
{
    using BookOrder.Repositories.Instructors;
    using BookOrder.Services.Instructors;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;

    [Route("api/[controller]")]
    public class InstructorProcessorController
    {
        public InstructorDataService instructorDataService { get; }

        public InstructorProcessorController()
        {
            instructorDataService = new InstructorDataService();
        }

        [HttpGet]
        public IEnumerable<object> Get()
        {
            //TODO dummy data for now
            return instructorDataService.GenerateInstructorsForDepartment(1);
        }

        [HttpGet("{id}")]
         public IEnumerable<object> Get(int id)
         {
             //TODO dummy data for now
             return instructorDataService.GenerateInstructorsForDepartment(id);
         }

        [HttpPost]
        public void Post([FromBody]string value)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            throw new MethodAccessException();
        }
        
    }
}
