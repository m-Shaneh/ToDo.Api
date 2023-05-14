using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ToDo.Api.Models;
using ToDo.Core.Models;

namespace ToDo.Api.MapperProfiles
{
    public class TodoItemProfile : Profile
    {
        public TodoItemProfile()
        {
            CreateMap<TodoItemDto, TodoItem>();
        }
    }
}
