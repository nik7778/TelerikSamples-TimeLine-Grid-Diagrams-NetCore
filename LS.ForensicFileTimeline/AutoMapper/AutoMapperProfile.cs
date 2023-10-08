using AutoMapper;
using LS.ForensicFileTimeline.Models;

namespace LS.ForensicFileTimeline.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<FileCsv, FileModel>();          
        }
    }
}
