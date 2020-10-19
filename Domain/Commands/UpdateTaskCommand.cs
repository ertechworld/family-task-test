using Domain.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Commands
{
  public  class UpdateTaskCommand
    {
        public System.Guid Id { get; set; }
        public string Subject { get; set; }
        public bool IsComplete { get; set; }
        public Nullable<System.Guid> AssignedToId { get; set; }
    }
}
