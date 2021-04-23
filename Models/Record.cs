using System;
using System.Collections.Generic;

#nullable disable

namespace PLANR.Models
{
    public partial class Record
    {
        public int Recordid { get; set; }
        public int Objectiveid { get; set; }
        public DateTime? RecordDate { get; set; }
        public string MetricData { get; set; }

        public virtual Objective Objective { get; set; }
    }
}
