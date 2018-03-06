namespace BookOrder.Services.Terms
{
    using BookOrder.Core.Models;
    using BookOrder.Repositories.Terms;
    using System.Collections.Generic;

    // public interface ITermDataService
    // {
    //     IEnumerable<Term> GenerateCoursesForInstructor(int instructorId);
    // }

    public class TermDataService //: ITermDataService
    {
        private TermDataRepository termDataRepository { get; }

        //TODO wire up dependency injection, discuss what this means to the team
        public TermDataService()
        {
            termDataRepository = new TermDataRepository();
        }

        //TODO change the object type to a concrete type, make this behavior into an interface
        public IEnumerable<Term> GenerateTermsForDept(int deptId)
        {
            return termDataRepository.GetAvailableTermsForDept(deptId);
        }

        public decimal DeleteTerm(Term term)
        {
            //TODO allow this if authenticated
            return termDataRepository.DeleteTerm(term);
        }

        public void AddTerm(Term term)
        {
            termDataRepository.SaveTermToDataBase(term);
        }
    }
}
