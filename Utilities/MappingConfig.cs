using API_QUIZZ.Models;
using API_QUIZZ.Models.Dtos;
using AutoMapper;

namespace API_QUIZZ.Utilities
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            //questions
            CreateMap<Questions, QuestionRandomDto>().ReverseMap();
            CreateMap<Questions, QuestionDto>().ReverseMap();

            //Response
            CreateMap<Response, ResponseCreateDto>().ReverseMap();

            //User
            CreateMap<User,UserDto>().ReverseMap();
        }
    }
}
