using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace PLANR.Models
{
    public partial class Task
    {
        public int Taskid { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public int Objectiveid { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy}", ApplyFormatInEditMode = true)]
        public DateTime TaskDueDate { get; set; }
        public bool TaskStatus { get; set; }

        public virtual Objective Objective { get; set; }
    }
}
