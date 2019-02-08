using Moq;
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
    }
}
