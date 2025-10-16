using System;
using Application.Activities.DTO;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.Activities.Queries;

public class GetActivityList
{

    public class Query : IRequest<List<ActivityDto>> { }

    public class Handler : IRequestHandler<Query, List<ActivityDto>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetActivityList> _logger;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;
        public Handler(AppDbContext context, ILogger<GetActivityList> logger, IMapper mapper, IUserAccessor userAccessor)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _userAccessor = userAccessor;
        }

        public async Task<List<ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
        {

            return await _context.Activities
            .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider,
                new { currentUserId = _userAccessor.GetUserId() })
            .ToListAsync(cancellationToken);
        }
    }
}
