using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;
using Application.Services.Repository;
using AutoMapper;
using MediatR;
using Persistence.Paginate;

namespace Application.Features.Slider.Queries.GetList
{
    public class GetListSliderQuery:IRequest<Paginate<GetListSliderDto>>
    {
        public Application.PageResult.PageResult pageResult { get; set; }
    }
    public class GetListSliderHandler : IRequestHandler<GetListSliderQuery, Paginate<GetListSliderDto>>
    {
        private readonly ISliderRepository _sliderRepository;
        private readonly IMapper _mapper;

        public GetListSliderHandler(ISliderRepository sliderRepository, IMapper mapper)
        {
            _sliderRepository = sliderRepository;
            _mapper = mapper;
        }

        public async Task<Paginate<GetListSliderDto>> Handle(GetListSliderQuery request, CancellationToken cancellationToken)
        {
            var list = await _sliderRepository.GetListAsync(request.pageResult.Size,request.pageResult.Index);

            var mapper = _mapper.Map<Paginate<GetListSliderDto>>(list);

            return mapper;
        }
    }
}
