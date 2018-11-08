using AutoMapper;
using Microsoft.OpenApi.Models;
using ServiceGovernance.Repository.EntityFramework.Entities;
using ServiceGovernance.Repository.Models;

namespace ServiceGovernance.Repository.EntityFramework.Mapping
{
    /// <summary>
    /// Defines mapping for Api descriptions
    /// </summary>
    public class ApiDescriptionMapperProfile : Profile
    {
        /// <summary>
        /// Creates a new instance of the service mapper profile
        /// </summary>
        public ApiDescriptionMapperProfile()
        {
            CreateMap<ServiceApiDescription, ApiDescription>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<string, OpenApiDocument>()
                 .ConstructUsing(src => OpenApiDocumentHelper.ReadFromJson(src));

            CreateMap<OpenApiDocument, string>()
                .ConstructUsing(src => src.ToJson());
        }
    }
}
