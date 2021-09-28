using AppKurs.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppKurs.Models
{
    public class SolvedTask
    {
        public string UserId { get; set; }
        public int TaskId { get; set; }
        public bool Solved { get; set; }

        public ApplicationUser User { get; set; }
        public UserTask Task { get; set; }
    }
}
