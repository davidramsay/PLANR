using System;
using System.Collections.Generic;

#nullable disable

namespace PLANR.Models
{
    public partial class Objective
    {
        public Objective()
        {
            Records = new HashSet<Record>();
            Tasks = new HashSet<Task>();
        }

        public int Objectiveid { get; set; }
        public int Goalid { get; set; }
        public string MetricName { get; set; }
        public DateTime ObjectiveDueDate { get; set; }

        public virtual Goal Goal { get; set; }
        public virtual ICollection<Record> Records { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
