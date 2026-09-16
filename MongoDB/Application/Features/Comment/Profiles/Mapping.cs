using Application.Features.Comment.Command.Create;
using Application.Features.Comment.Command.Update;
using Application.Features.Comment.Queries.GetById;
using Application.Features.Comment.Queries.GetList;
using AutoMapper;
using Domain.Entities;
using Persistence.Paginate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Comment.Profiles
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Comment, CreateCommentCommand>().ReverseMap();
            CreateMap<Domain.Entities.Comment, UpdateCommentCommand>().ReverseMap();

            CreateMap<Domain.Entities.Comment, GetListCommentDto>().ReverseMap();
            CreateMap<Paginate<Domain.Entities.Comment>, Paginate<GetListCommentDto>>().ReverseMap();

            CreateMap<Domain.Entities.Comment, GetByIdCommentDto>().ReverseMap();
        }
    }
}
