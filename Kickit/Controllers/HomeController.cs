using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kickit.Models;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Kickit.API;
using RestSharp;
using RestSharp.Authenticators;

namespace Kickit.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {

            return View();
        }

        //CONTACT VIEW
        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(
            [Bind(Include = "FromName, FromEmail, ReceiverName, ReceiverEmail, DateTime1, DateTime2, DateTime3")] Invitor invitor)
        {
            if (ModelState.IsValid)
            {
                ApplicationDbContext dbContext = new ApplicationDbContext();
                Invitor invite = dbContext.Invitors.Add(invitor);
                dbContext.SaveChanges();
                SendSimpleMessage(invitor);
                return View("Sent", invitor);
            }
            else
            {
                // there is a validation error
                return View();
            }
        }


        //View after invite has been sent
        public ActionResult Sent()
        {
            return View();
        }


        // GET /RecepientForm/17
        [HttpGet]
        public ViewResult RecepientForm(int id)


        {
            try
            {

                ApplicationDbContext dbContext = new ApplicationDbContext();
                var invitordetail = dbContext.Invitors.SingleOrDefault(i => i.Id == id);//Get the single Record by id
                ViewBag.fromName = invitordetail.FromName; //Datas are passed to responseform
                ViewBag.receiverName = invitordetail.ReceiverName;
                ViewBag.date1 = invitordetail.DateTime1; //passing data to viewbag
                ViewBag.date2 = invitordetail.DateTime2;
                ViewBag.date3 = invitordetail.DateTime3;

                //fill in what needs to display on form
                return View();
            }
            catch(Exception e)
            {
                Response.Write(e.Message);
                return  View("Sorry");
            }
        }

        [HttpPost]
        public ViewResult RecepientForm(InvitorRecepientModel responsemodel)
        {          

            if (responsemodel.recepientform != null)
               {            
                DateTime dateSelected = DateTime.Parse(responsemodel.recepientform.DateTime);//selected date in recepient form and passed to API
                DateTime endDate = dateSelected.AddDays(2);//End date to be passed to  API
                var api = new EventBrite();
                List<EventBriteEvent> eventlist = api.Search(responsemodel.recepientform.Zipcode,dateSelected,endDate);//Call the API method 
               // EventBriteEvent firstEventDetails = eventlist.FirstOrDefault();
               // string eventURL = firstEventDetails.EventUrl;
                return View("EventBriteAPI", eventlist);
              
            }
            else
            {
                return View("Sorry");
            }
        }


        //Method to call mailgun

        public static IRestResponse SendSimpleMessage(Invitor invitor)
        {
            RestClient client = new RestClient();
            client.BaseUrl = new Uri("https://api.mailgun.net/v3");
            client.Authenticator =
                   new HttpBasicAuthenticator("api",
                                              "key-8b65bd539d243a3e8c4a03a5c16e6f4b");
            RestRequest request = new RestRequest();
            request.AddParameter("domain",
                                "sandbox90b6af4c9c9f40eaa4ba0073541a6975.mailgun.org", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", "Mailgun Sandbox <postmaster@sandbox90b6af4c9c9f40eaa4ba0073541a6975.mailgun.org>");
            request.AddParameter("to", "Athi <athikumar72@gmail.com>");
            // request.AddParameter("to", "Athi <teamkickitapp@gmail.com>");// This is receiver mail.can change this mail id after add and activate in mailgun account Receipient mail list 
            request.AddParameter("subject", "Hello Teamkickitapp");
            request.AddParameter("text", $"Hi {invitor.ReceiverName} you are invited by {invitor.FromName} .Click this link  to :http://localhost:50941/Home/RecepientForm/?Id={invitor.Id}");
            // request.AddParameter("text", $"Hi {invitor.ReceiverName} you are invited by {invitor.FromName} .Click this link  to :http://kickitapp.azurewebsites.net/Home/RecepientForm/?id={invitor.Id}");//This is message sent to receiver

            request.Method = Method.POST;
            return client.Execute(request);
        }

        public static IRestResponse SendSimpleMessage(InvitorRecepientModel inviteReceientModel,EventBriteEvent firstEventDetail)
        {
            
            RestClient client = new RestClient();
            client.BaseUrl = new Uri("https://api.mailgun.net/v3");
            client.Authenticator =
                   new HttpBasicAuthenticator("api",
                                              "key-8b65bd539d243a3e8c4a03a5c16e6f4b");
            RestRequest request = new RestRequest();
            request.AddParameter("domain",
                                "sandbox90b6af4c9c9f40eaa4ba0073541a6975.mailgun.org", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", "Mailgun Sandbox <postmaster@sandbox90b6af4c9c9f40eaa4ba0073541a6975.mailgun.org>");//Email sender
            request.AddParameter("to", "Gaby <teamkickitapp@gmail.com>");// This is receiver mail.can change this mail id after add and activate in mailgun account Receipient mail list 
            request.AddParameter("subject", "Hello Teamkickitapp");
            request.AddParameter("text", $"Hi {inviteReceientModel.invitor.ReceiverName} you are invited by {inviteReceientModel.invitor.FromName} .Click this link  to :{firstEventDetail.EventUrl}");//This is message sent to receiver
            request.Method = Method.POST;
            return client.Execute(request);
        }

    }
}
