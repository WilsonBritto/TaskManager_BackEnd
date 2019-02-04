using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TaskManagerAPI.Core;
using TaskManagerAPI.Core.Domain;

namespace TaskManagerAPI.Controllers
{
    public class TaskController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public TaskController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IHttpActionResult GetTaskById(int Id)
        {
            return Ok(_unitOfWork.Tasks.Get(Id));
        }
    }
}
