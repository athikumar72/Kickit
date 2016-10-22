using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kickit.API
{
    public class EventBriteEvent
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string EventUrl { get; set; }

        public EventBriteEvent(string name, string description, DateTime startTime, DateTime endTime, string eventUrl)
        {
            Name = name;
            Description = description;
            StartTime = startTime;
            EndTime = endTime;
            EventUrl = eventUrl;
        }
    }
}