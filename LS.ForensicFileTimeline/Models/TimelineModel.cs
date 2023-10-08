using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.ForensicFileTimeline.Models
{
    public class TimelineModel
    {
        public DateTime Date { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
        public List<TimelineEventModelAction> Actions { get; set; }      
    }
}
