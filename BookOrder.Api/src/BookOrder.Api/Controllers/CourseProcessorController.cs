namespace BookOrder.Api.Controllers
{
    using BookOrder.Core.Models;
    using BookOrder.Repositories.Courses;
    using BookOrder.Services.Courses;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    [Route("api/[controller]")]
    public class CourseProcessorController
    {
        public CourseDataService courseDataService { get; }

        public CourseProcessorController()
        {
            courseDataService = new CourseDataService();
        }

        [HttpGet]
        public IEnumerable<Course> Get()
        {
            return courseDataService.GenerateCoursesForInstructor(1);
        }

        [HttpGet("{id}")]
        public IEnumerable<Course> Get(int id)
        {
            return courseDataService.GenerateCoursesForInstructor(id);
        }

        [HttpPost]
        public Course Post([FromBody]JObject courseDetailsToSave)
        {
            var course = courseDetailsToSave.SelectToken("Course").ToObject<Course>();
            var instructorId = courseDetailsToSave.SelectToken("InstructorId").ToObject<int>();//TODO use later with sproc
            var id = courseDataService.SaveCourse(course, instructorId);
            course.CourseId = id;
            return course;
            // return course;
        }

        [HttpPut]
        public void Put([FromBody]JObject courseDetailsToUpdate)
        {
            var course = courseDetailsToUpdate.SelectToken("Course").ToObject<Course>();
            courseDataService.SetCourseSubmitted(course);
        }

        [HttpDelete]
        public void Delete([FromBody]JObject courseDetailsToDelete)
        {
            var courseId = courseDetailsToDelete.SelectToken("courseId").ToObject<int>();
            courseDataService.DeleteCourse(courseId);
        }
        
    }
}
