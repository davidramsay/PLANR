using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PLANR.Models.ViewModels
{
    public class DashBoardViewModel
    {
        public List<Task> Tasks { get; set; }
        public List<Event> Events { get; set; }
    }
}
