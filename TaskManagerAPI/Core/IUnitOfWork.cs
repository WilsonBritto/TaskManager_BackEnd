using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Core.Repositories;

namespace TaskManagerAPI.Core
{
    public interface IUnitOfWork : IDisposable
    {
        ITaskRepository Tasks { get; }
        int Complete();
    }
}
