using AutoMapper;
using RopeDetection.CommonData.ViewModels.FileViewModel;
using RopeDetection.CommonData.ViewModels.LabelViewModel;
using RopeDetection.CommonData.ViewModels.TrainViewModel;
using RopeDetection.CommonData.ViewModels.UserViewModel;
using RopeDetection.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RopeDetection.Services.Mapper
{
    public static class ObjectMapper
    {
        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                // This line ensures that internal properties are also mapped over.
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<AspnetRunDtoMapper>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        });
        public static IMapper Mapper => Lazy.Value;
    }

    public class AspnetRunDtoMapper : Profile
    {
        public AspnetRunDtoMapper()
        {
            CreateMap<User, UserModel>()
                .ReverseMap();

            CreateMap<User, UserShortModel>()
               .ReverseMap();

            CreateMap<FileData, FileDataModel>()
         .ReverseMap();

            CreateMap<ModelObjectType, LabelModel>()
                .ReverseMap();

            CreateMap<Model, CreateModel>()
                .ReverseMap();

            CreateMap<Model, ModelResponse>()
                .ForMember(dest => dest.LabelPath, opt => opt.MapFrom(src => src.TrainedModel == null ? string.Empty : src.TrainedModel.LabelPath))
                .ReverseMap();

            CreateMap<AnalysisHistory, TrainResponse>()
                .ReverseMap();
            //  CreateMap<RatingRow, RatingRowModel>()
            //.ReverseMap();

            //  CreateMap<LinkToDocument, LinkToDocumentModel>()
            //  .ReverseMap();


            //  CreateMap<SectionRow, SectionRowModel>()
            //  .ReverseMap();


            //  CreateMap<UserDailyDataRow, UserDailyDataRowModel>().ReverseMap();
        }
    }
}
