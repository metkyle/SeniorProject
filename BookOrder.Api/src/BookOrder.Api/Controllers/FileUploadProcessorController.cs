using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.IO;
using System.Net.Http;
using System.Web;

namespace BookOrder.Api.Controllers{
    using BookOrder.Core.Models;
    using BookOrder.Repositories.Courses;
    using BookOrder.Services.Courses;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Http;
    

    [Route("api/[controller]")]
    public class FileUploadProcessorController : Controller{

        static CSVParser theParser;
        static CSVCourse[] theCourses;
        
        HttpContext context;
        IHostingEnvironment envi;
        static String filePath;
        private CourseDataService courseDataService {get;}

       public FileUploadProcessorController(IHttpContextAccessor contextAccess, IHostingEnvironment envi)
        {
            context = contextAccess.HttpContext;
            this.envi = envi;
            this.courseDataService = new CourseDataService();
        }

        [HttpPost]
        public HttpResponseMessage UploadJsonFile()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            var httpRequest = context.Request;
            if(httpRequest.Form.Files.Count > 0){
                foreach (IFormFile file in httpRequest.Form.Files){
                    var postedFile = httpRequest.Form.Files[0];
                    
                    filePath = Path.Combine("UploadFile/", postedFile.FileName);
                   

                    using(var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        postedFile.CopyTo(fileStream);
                        
                    }
                    theParser = new CSVParser(filePath);
                    theCourses = theParser.getRecords();
                    this.courseDataService.addCSVCourse(theCourses);
                   
                }
            }
            return response;
        }

        [HttpGet]
        public CSVCourse[] Get()
        {
            if (theCourses == null)
                return null;
            else
                return theCourses;
        }

       // [HttpGet]
       // [Route("api/[action]")]
        public void exportCSV(CSVCourse[] theRecords)
        {
            var engine = new FileHelpers.FileHelperEngine<CSVCourse>();
            
            engine.WriteFile("CSVOUTPUT.txt", theRecords);
            Console.Write("export post function");
        }

    }

}