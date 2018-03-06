namespace BookOrder.Services.Courses
{
    using BookOrder.Core.Models;
    using BookOrder.Repositories.Courses;
    using System.Collections.Generic;

    public interface ICourseDataService
    {
        IEnumerable<Course> GenerateCoursesForInstructor(int instructorId);
    }

    public class CourseDataService : ICourseDataService
    {
        private CourseDataRepository courseDataRepository { get; }

        //TODO wire up dependency injection, discuss what this means to the team
        public CourseDataService()
        {
            courseDataRepository = new CourseDataRepository();
        }

        //TODO change the object type to a concrete type, make this behavior into an interface
        public IEnumerable<Course> GenerateCoursesForInstructor(int instructorId)
        {
            return courseDataRepository.GetListOfCoursesFromInstructorId(instructorId);
        }

        public void DeleteCourse(int courseId)
        {
            //TODO allow this if authenticated
            courseDataRepository.DeleteCourseById(courseId);
        }

        public void addCSVCourse(CSVCourse[] csvCourses)
        {
            courseDataRepository.SaveCourseToDatabase(csvCourses);
        }

        public int SaveCourse(Course courseObj, int instructorId)
        {
            return courseDataRepository.SaveCourseToDatabase(courseObj, instructorId);
        }

        public void SetCourseSubmitted(Course courseObj)
        {
            courseDataRepository.SetCourseSubmitted(courseObj);
        }

        public void CourseSubmitUpdate(int courseId)
        {
            courseDataRepository.submitCourse(courseId);
        }
    }
}
