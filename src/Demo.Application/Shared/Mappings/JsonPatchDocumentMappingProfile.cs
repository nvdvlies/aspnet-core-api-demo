using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace Demo.Application.Shared.Mappings
{
    public class JsonPatchDocumentMappingProfile : Profile
    {
        public JsonPatchDocumentMappingProfile()
        {
            CreateMap(typeof(JsonPatchDocument<>), typeof(JsonPatchDocument<>));
            CreateMap(typeof(Operation<>), typeof(Operation<>));
        }
    }
}
