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
        [Display(Name = "Event Name")]
        [Required, StringLength(50)]
        public string EventName { get; set; }
        [Display(Name = "Event Description")]
        [Required, StringLength(50)]
        public string EventDesc { get; set; }
        [Display(Name = "Start")]

        public DateTime EventStart { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:h:mm tt}")]
        [Display(Name = "End")]
        public DateTime EventEnd { get; set; }

        public virtual Category Category { get; set; }
    }
}
