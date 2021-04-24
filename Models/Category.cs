using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace PLANR.Models
{
    public partial class Category
    {
        public Category()
        {
            Goals = new HashSet<Goal>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Categoryid { get; set; }
        public int Userid { get; set; }
        public string CategoryName { get; set; }
        public string CategoryAbbreviation { get; set; }
        public virtual ICollection<Goal> Goals { get; set; }
        public User User { get; set; }
    }
}
