using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class PrivateActivity : Activity
    {
        public Team Team { get; set; }

        public Guid TeamId { get; set; }
        
        
    }
}