using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json.Linq;

namespace Kickit.API
{
    public class EventBrite
    {
        public List<EventBriteEvent> Search(string zipCode,DateTime dateSelected,DateTime endDate)
            
        {           
            string apiDetails = string.Empty;
            string startDateAPI = dateSelected.ToString(("yyyy-MM-dd") + "T00:00:00");// pass Start date to API
            string endDateAPI  = endDate.ToString(("yyyy-MM-dd") + "T00:00:00"); //pass End date to API

          
            string url =
               @"https://www.eventbriteapi.com/v3/events/search/?location.address="+zipCode+"&start_date.range_start="+startDateAPI+"&start_date.range_end="+endDateAPI+"&token=CV6HX5TNIPF76F3VLGFC";

            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream()) 
            using (StreamReader reader = new StreamReader(stream))
            {
                apiDetails = reader.ReadToEnd();
            }

            JObject eventbritejson = JObject.Parse(apiDetails);
            JArray eventsArray = (JArray) eventbritejson["events"];
            List<EventBriteEvent> eventList = new List<EventBriteEvent>();
            foreach (JToken detail in eventsArray)    //      
            {
                string name = (string) detail["name"]["text"];

                string description = "";
                if (detail["description"]!=null) {
                     //description = ((string)detail["description"]["text"]).Substring(0, 70);
                    description = ((string)detail["description"]["text"]);
                }
                DateTime startTime = (DateTime) detail["start"]["local"];
                DateTime endtime = (DateTime) detail["end"]["local"];
                string eventUrl = (string) detail["url"];
                eventList.Add(new EventBriteEvent(name, description, startTime, endtime, eventUrl));
            }

            return eventList.OrderBy(x=>x.StartTime).ToList();
          
        }

    }
    }