using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskManagerAPI.Core.Domain;
using TaskManagerAPI.Core.Resources;

namespace TaskManagerAPI.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Task, TaskResource>();
            CreateMap<TaskResource, Task>();
        }
    }
}