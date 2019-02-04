using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskManagerAPI.Core;
using TaskManagerAPI.Core.Repositories;
using TaskManagerAPI.Persisitance.Repositories;

namespace TaskManagerAPI.Persisitance
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TaskManagerDBContext _Context;

        public UnitOfWork(TaskManagerDBContext Context)
        {
            _Context = Context;
            Tasks = new TaskRepository(Context);
        }
        public ITaskRepository Tasks { get; private set; }

        public int Complete()
        {
            return _Context.SaveChanges();
        }

        public void Dispose()
        {
            _Context.Dispose();
        }
    }
}