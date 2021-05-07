using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace PLANR.Models
{
    public partial class Goal
    {
        public Goal()
        {
            Objectives = new HashSet<Objective>();
        }

        public int Goalid { get; set; }
        public int Categoryid { get; set; }
        [Display(Name = "Goal Name")]
        [Required, StringLength(15)]
        public string GoalName { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Due Date")]

        public DateTime? GoalDate { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<Objective> Objectives { get; set; }
    }
}
