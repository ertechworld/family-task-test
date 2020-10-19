using Domain.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Commands
{
   public class CreateTaskCommand
    {
        public string Subject { get; set; }
        public bool IsComplete { get; set; }
        public Nullable<System.Guid> AssignedToId { get; set; }       

        [ForeignKey("AssignedToId")]
        public Member Member { get; set; }
    }
}
