using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.DataModels
{
    public class Task
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Subject { get; set; }
        public bool IsComplete { get; set; }
        public Nullable<System.Guid> AssignedToId { get; set; }
        [ForeignKey("AssignedToId")]
        public virtual Member Member { get; set; }
    }
}
