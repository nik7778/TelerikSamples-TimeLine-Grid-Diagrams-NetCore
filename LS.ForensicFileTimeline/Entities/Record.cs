using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.ForensicFileTimeline.Entities
{
    public class Record
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Day { get; set; }
        public string Time { get; set; }
        public string Client { get; set; }
        public decimal Duration { get; set; }
        public bool DNB { get; set; }
        public string Status { get; set; }
        public string Memo { get; set; }
        public string Staff { get; set; }
    }
}
