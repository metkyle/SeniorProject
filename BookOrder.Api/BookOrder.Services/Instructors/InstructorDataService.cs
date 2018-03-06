namespace BookOrder.Services.Instructors
{
    using BookOrder.Repositories.Instructors;
    using System.Collections.Generic;

    public class InstructorDataService
    {
        private InstructorDataRepository instructorDataRepository { get; }

        //TODO wire up dependency injection, discuss what this means to the team
        public InstructorDataService()
        {
            instructorDataRepository = new InstructorDataRepository();
        }

        //TODO change the object type to a concrete type, make this behavior into an interface
        public IEnumerable<object> GenerateInstructorsForDepartment(int departmentId)
        {
            return instructorDataRepository.GetListOfInstructorsFromDepartmentId(departmentId);
        }

        public IEnumerable<object> GenerateInstructorsForCourse(int courseId)
        {
            return instructorDataRepository.GetListOfInstructorsFromDepartmentId(courseId);
        }
    }
}
