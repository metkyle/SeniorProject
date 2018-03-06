namespace BookOrder.Tests.Controllers
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using BookOrder.Api.Controllers;
    using System.Linq;

    [TestClass]
    public class CourseProcessorControllerTests
    {
        [TestMethod]
        public void CourseProcessorController_ShouldReturnAListOfCourses()
        {
            var sut = new CourseProcessorController();
            Assert.IsTrue(sut.Get().Count() > 0);
        }
    }
}
