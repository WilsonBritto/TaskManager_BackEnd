using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskManagerAPI.Core.Domain;
using TaskManagerAPI.Core.Repositories;

namespace TaskManagerAPI.Persisitance.Repositories
{
    public class TaskRepository : Repository<Task>, ITaskRepository
    {
        public TaskRepository(TaskManagerDBContext Context)
            :base(Context)
        {
        }

        public TaskManagerDBContext TaskManagerDBContext
        {
            get { return Context as TaskManagerDBContext; }
        }
    }
}