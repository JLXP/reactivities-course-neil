using Application.Activities.DTO;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.Activities.Queries;

public class GetActivityList
{
    private const int MaxPageSize = 50;

    public class Query : IRequest<Result<PagedList<ActivityDto, DateTime?>>>
    {
        public DateTime? Cursor { get; set; }
        private int _pageSize;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

    }

    public class Handler : IRequestHandler<Query, Result<PagedList<ActivityDto, DateTime?>>>
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

        public async Task<Result<PagedList<ActivityDto, DateTime?>>> Handle(Query request, CancellationToken cancellationToken)
        {

            var query = _context.Activities
                .OrderBy(x => x.Date)
                .AsQueryable();

            if (request.Cursor.HasValue)
            {
                query = query.Where(x => x.Date >= request.Cursor.Value);
            }

            var activities = await _context.Activities
            .Take(request.PageSize + 1)
            .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider,
                new { currentUserId = _userAccessor.GetUserId() })
            .ToListAsync(cancellationToken);

            DateTime? nextCursor = null;
            if (activities.Count > request.PageSize)
            {
                nextCursor = activities.Last().Date;
                activities.RemoveAt(activities.Count - 1);
            }

            return Result<PagedList<ActivityDto, DateTime?>>.Success(
                new PagedList<ActivityDto, DateTime?>{}

            );
            
        }
    }
}
