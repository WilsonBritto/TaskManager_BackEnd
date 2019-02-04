using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskManagerAPI.Core.Domain
{
    public class Task
    {
        public int TaskId { get; set; }
        public int? ParentId { get; set; }
        public string TaskDetails { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Priority { get; set; }
        public bool IsTaskEnded { get; set; }
    }
}