using AutoMapper;
using Domain.Commands;
using Domain.DataModels;
using Domain.ViewModel;

namespace WebApi.AutoMapper
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<CreateTaskCommand, Domain.DataModels.Task>();
            CreateMap<UpdateTaskCommand, Domain.DataModels.Task>();
            //CreateMap<Domain.DataModels.Task,TaskVm>();

            //CreateMap<Domain.DataModels.Task, TaskVm>().ReverseMap()
            //    .ForMember(x => x.Member.Id, map => map.MapFrom(source => source.Member.Id)).ReverseMap();

            CreateMap<TaskVm, Domain.DataModels.Task>()                    
                    .ForMember(dest => dest.Member, act => act.MapFrom(src => src.Member))                

                    .ReverseMap();
        }
    }
}
