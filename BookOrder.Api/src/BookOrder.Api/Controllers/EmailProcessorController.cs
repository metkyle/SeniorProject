
namespace BookOrder.Api.Controllers{

using BookOrder.Core.Models;
    using BookOrder.Repositories.Courses;
    using BookOrder.Services.Courses;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Http;
    
    using System.Net.Http;
    using System.Web;
    using System.Net.Mail;

[Route("api/[controller]")]
public class EmailProcessorController: Controller{

    private String emailTo;   
    private String emailFrom;
    private String emailBody;
    private String emailSubject;
    private HttpContext context;


    public EmailProcessorController(IHttpContextAccessor contextAccess){this.context = contextAccess.HttpContext;}

    [HttpPost]
    public void getEmailInfo()
    {
            HttpResponseMessage response = new HttpResponseMessage();
            var httpRequest = context.Request;
            
            this.emailTo = httpRequest.Form["emailTo"];
            this.emailFrom = httpRequest.Form["emailFrom"];
            this.emailSubject = httpRequest.Form["emailSubject"];
            this.emailBody = httpRequest.Form["emailBody"];

            sendEmail();
    }

    private void sendEmail()
    {
         MailMessage mail = new MailMessage();
        mail.From = new MailAddress("CSCDBookOrder@mydomain.com", "CSCD Book Ordering");
        mail.To.Add(this.emailTo);
        mail.IsBodyHtml = true;
        mail.Subject = this.emailSubject;

        mail.Body = this.emailBody;
        
        mail.Priority = MailPriority.High;
        SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
        smtp.Credentials = new System.Net.NetworkCredential("ewubookorder@gmail.com", "eagles123");
        smtp.EnableSsl = true;

        smtp.Send(mail); 
    }
 
  }
}
