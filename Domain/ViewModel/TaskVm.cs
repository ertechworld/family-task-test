using Domain.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.ViewModel
{
    public class TaskVm
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public bool IsComplete { get; set; }
        public Nullable<System.Guid> AssignedToId { get; set; }
        [ForeignKey("AssignedToId")]
        public virtual Member Member { get; set; }
    }
}
