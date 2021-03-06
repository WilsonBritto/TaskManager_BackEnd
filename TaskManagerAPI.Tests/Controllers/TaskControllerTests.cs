﻿using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManagerAPI.Core.Domain;
using TaskManagerAPI.Core;
using TaskManagerAPI.Controllers;
using System.Web.Http.Results;
using TaskManagerAPI.Core.Resources;
using System.Net;
using AutoMapper;
using TaskManagerAPI.App_Start;
using System.Net.Http;

namespace TaskManagerAPI.Tests.Controllers
{
    [TestFixture]
    class TaskControllerTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private MapperConfiguration _config;
        private IMapper _mapper;
        private TaskController _controller;

        [SetUp]
        public void SetUp()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _config = new MapperConfiguration(cfg =>
           {
               cfg.AddProfile<MappingProfile>();
           });
            _mapper = _config.CreateMapper();

            _controller = new TaskController(_unitOfWork.Object, _mapper);
        }

        [Test]
        public void GetTaskById_IdNotMatches_ReturnsContent()
        {
            _unitOfWork.Setup(s => s.Tasks.Get(1)).Returns((Task)null);

            var result = _controller.GetTaskById(1);
            var resultObj = result as NegotiatedContentResult<ErrorResource>;

            Assert.That(result, Is.TypeOf<NegotiatedContentResult<ErrorResource>>());
            Assert.That(resultObj.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public void GetTaskById_IdMatches_ReturnsOk()
        {
            var task = new Task { TaskId = 5 };
            _unitOfWork.Setup(s => s.Tasks.Get(5)).Returns(task);

            var result = _controller.GetTaskById(5);
            var resultObj = result as OkNegotiatedContentResult<TaskResource>;

            Assert.That(result, Is.TypeOf<OkNegotiatedContentResult<TaskResource>>());
            Assert.That(resultObj.Content.TaskId, Is.EqualTo(5));
        }

        [Test]
        public void GetAllTasks_WhenNoTasksFound_ReturnsContent()
        {
            _unitOfWork.Setup(s => s.Tasks.GetAll()).Returns((List<Task>)null);

            var result = _controller.GetAllTasks();
            var resultObj = result as NegotiatedContentResult<ErrorResource>;

            Assert.That(result, Is.TypeOf<NegotiatedContentResult<ErrorResource>>());
            Assert.That(resultObj.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
        [Test]
        public void GetAllTasks_WhenTaskFound_ReturnsOk()
        {
            var TaskList = new List<Task>
            {
                new Task {TaskId = 1},
                new Task {TaskId = 2},
                new Task {TaskId = 3}
            };
            _unitOfWork.Setup(s => s.Tasks.GetAll()).Returns(TaskList);

            var result = _controller.GetAllTasks();
            var resultObj = result as OkNegotiatedContentResult<IEnumerable<TaskResource>>;

            Assert.That(result, Is.TypeOf<OkNegotiatedContentResult<IEnumerable<TaskResource>>>());
            Assert.That(resultObj.Content.Count, Is.EqualTo(3));
        }
        [Test]
        public void CreateTask_WhenCalledWithNullTaskObject_ReturnsBadRequest()
        {
            var result = _controller.CreateTask((TaskResource)null);

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }
        [Test]
        public void CreateTask_WhenCalledWithTaskWithoutId_ReturnsOkWithCreatedObjectAndUri()
        {
            var taskResource = new TaskResource { TaskDetails = "Test Task" };
            var task = _mapper.Map<TaskResource, Task>(taskResource);
            _unitOfWork.Setup(s => s.Tasks.Add(It.IsAny<Task>()))
                .Callback((Task t) =>
                {
                    t.TaskId = 5;
                    task = t;
                });
            _controller.Request = new HttpRequestMessage
            {
                RequestUri = new Uri("https://Test")
            };

            var result = _controller.CreateTask(taskResource);
            var resultObj = result as CreatedNegotiatedContentResult<TaskResource>;

            _unitOfWork.Verify(u => u.Complete());
            Assert.That(result, Is.TypeOf<CreatedNegotiatedContentResult<TaskResource>>());
            Assert.That(resultObj.Location.ToString(), Does.Contain(task.TaskId.ToString()));
            Assert.That(resultObj.Content.TaskId, Is.EqualTo(task.TaskId));
            Assert.That(resultObj.Content.TaskDetails, Is.EqualTo(task.TaskDetails));
        }
        [Test]
        public void UpdateTask_WhenIdNotPresentInDb_ReturnsNotFound()
        {
            _unitOfWork.Setup(s => s.Tasks.Get(1)).Returns((Task)null);

            var result = _controller.UpdateTask(1,new TaskResource());
            var resultObj = result as NegotiatedContentResult<ErrorResource>;

            Assert.That(result, Is.TypeOf<NegotiatedContentResult<ErrorResource>>());
            Assert.That(resultObj.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
        [Test]
        public void UpdateTask_WhenCalledWithNullTaskObject_ReturnsBadRequest()
        {
            _unitOfWork.Setup(s => s.Tasks.Get(1)).Returns(new Task());

            var result = _controller.UpdateTask(1, null as TaskResource);

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }
        [Test]
        public void UpdateTask_WhenCalledWithIdNotMatchingWithTaskId_ReturnsBadRequest()
        {
            _unitOfWork.Setup(s => s.Tasks.Get(1)).Returns(new Task());

            var result = _controller.UpdateTask(1, new TaskResource { TaskId = 2 });

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }
        [Test]
        public void UpdateTask_WhenCalledWithIdPresentInDbAndMatchingWithTaskId_ReturnsOk()
        {
            var task = new Task { TaskId = 1, TaskDetails = "Test Task" };
            _unitOfWork.Setup(s => s.Tasks.Get(1)).Returns(task);
            var taskResource = new TaskResource { TaskId = 1, TaskDetails = "Updated test task" };

            var result = _controller.UpdateTask(1, taskResource);
            var resultObj = result as OkNegotiatedContentResult<TaskResource>;

            _unitOfWork.Verify(u => u.Complete());
            Assert.That(result, Is.TypeOf<OkNegotiatedContentResult<TaskResource>>());
            Assert.That(resultObj.Content.TaskDetails, Does.Contain("Updated"));
            Assert.That(task.TaskDetails, Does.Contain("Updated"));
        }
        [Test]
        public void DeleteTask_WhenCalledWithIdNotPresentInDb_ReturnsNotFound()
        {
            _unitOfWork.Setup(s => s.Tasks.Get(1)).Returns(null as Task);

            var result = _controller.DeleteTask(1);
            var resultObj = result as NegotiatedContentResult<ErrorResource>;

            Assert.That(result, Is.TypeOf<NegotiatedContentResult<ErrorResource>>());
            Assert.That(resultObj.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
        [Test]
        public void DeleteTask_WhenCalledWithIdPresentInDb_ReturnsOk()
        {
            _unitOfWork.Setup(s => s.Tasks.Get(1)).Returns(new Task());

            var result = _controller.DeleteTask(1);

            _unitOfWork.Verify(u => u.Tasks.Remove(It.IsAny<Task>()));
            _unitOfWork.Verify(u => u.Complete());
            Assert.That(result, Is.TypeOf<OkResult>());
        }
    }
}
