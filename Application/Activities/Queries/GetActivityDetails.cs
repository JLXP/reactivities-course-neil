using System;
using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities.Queries;

public class GetActivityDetails
{

    public class Query : IRequest<Result<Activity>>
    {
        public required string Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<Activity>>
    {
        private readonly AppDbContext _context;

        public Handler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Activity>> Handle(Query request, CancellationToken cancellationToken)
        {
            var activity = await _context.Activities
            .Include(x => x.Attendees)
            .ThenInclude(x => x.User)
            .FirstOrDefaultAsync(x => request.Id == x.Id, cancellationToken);

            if (activity == null) Result<Activity>.Failure("Activity not found", 404);
            return Result<Activity>.Success(activity!);
        }
    }


}
