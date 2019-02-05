﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TaskManagerAPI.Core;
using TaskManagerAPI.Core.Domain;
using TaskManagerAPI.Core.Resources;

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
            var task = _unitOfWork.Tasks.Get(Id);
            if (task == null)
                return Content(HttpStatusCode.NotFound, new { errorCode = HttpStatusCode.NotFound, error = "Task with Id = "+Id.ToString()+" not found" });
            return Ok(Mapper.Map<Task, TaskResource>(task));
        }
        [HttpGet]
        public IHttpActionResult GetAllTasks()
        {
            var tasks = _unitOfWork.Tasks.GetAll();
            if (tasks == null)
                return Content(HttpStatusCode.NotFound, new { errorCode = HttpStatusCode.NotFound, error = "No Tasks Found" });
            return Ok(tasks.Select(t => Mapper.Map<Task, TaskResource>(t)));
        }
        [HttpPost]
        public IHttpActionResult CreateTask([FromBody]TaskResource taskResource)
        {
            if (taskResource == null)
                return BadRequest();

            var task = Mapper.Map<TaskResource, Task>(taskResource);
            _unitOfWork.Tasks.Add(task);
            _unitOfWork.Complete();
            taskResource = Mapper.Map<Task, TaskResource>(task);

            var newUri = new Uri(Request.RequestUri +"/"+ taskResource.TaskId.ToString());

            return Created(newUri, taskResource);
        }
        [HttpPut]
        public IHttpActionResult UpdateTask([FromUri]int Id, [FromBody] TaskResource taskResource)
        {
            var task = _unitOfWork.Tasks.Get(Id);
            if (task == null)
                return Content(HttpStatusCode.NotFound, new { errorCode = HttpStatusCode.NotFound, error = "Task with Id = " + Id.ToString() + " not found" });

            if (taskResource == null || (taskResource.TaskId != null && taskResource.TaskId != Id))
                return BadRequest();

            task.ParentId = taskResource.ParentId;
            task.TaskDetails = taskResource.TaskDetails;
            task.StartDate = taskResource.StartDate;
            task.EndDate = taskResource.EndDate;
            task.Priority = taskResource.Priority;
            task.IsTaskEnded = taskResource.IsTaskEnded;

            _unitOfWork.Complete();

            Mapper.Map(task, taskResource);

            return Ok(taskResource);
        }
        [HttpDelete]
        public IHttpActionResult DeleteTask([FromUri] int Id)
        {
            var task = _unitOfWork.Tasks.Get(Id);
            if (task == null)
                return Content(HttpStatusCode.NotFound, new { errorCode = HttpStatusCode.NotFound, error = "Task with Id = " + Id.ToString() + " not found" });

            _unitOfWork.Tasks.Remove(task);
            _unitOfWork.Complete();

            return Ok();
        }

    }
}
