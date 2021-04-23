using System;
using System.Collections.Generic;

#nullable disable

namespace PLANR.Models
{
    public partial class Event
    {
        public int Eventid { get; set; }
        public int? Categoryid { get; set; }
        public string EventName { get; set; }
        public string EventDesc { get; set; }
        public DateTime? EventDate { get; set; }

        public virtual Category Category { get; set; }
    }
}
