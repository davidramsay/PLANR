using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace PLANR.Models
{
    public partial class Event
    {
        public int Eventid { get; set; }
        public int? Categoryid { get; set; }
        public string EventName { get; set; }
        public string EventDesc { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:h:mm tt}")]
        public DateTime EventStart { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:h:mm tt}")]
        public DateTime EventEnd { get; set; }

        public virtual Category Category { get; set; }
    }
}
