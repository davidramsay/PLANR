using System;
using System.Collections.Generic;

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
        public string GoalName { get; set; }
        public DateTime? GoalDate { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<Objective> Objectives { get; set; }
    }
}
