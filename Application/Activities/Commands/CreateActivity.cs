using System;
using Application.Activities.DTO;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Activities.Commands;

public class CreateActivity
{

    public class Command : IRequest<string>
    {
        public required CreateActivityDTO ActivityDTO { get; set; }
    }

    public class Handler : IRequestHandler<Command, string>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        public async Task<string> Handle(Command request, CancellationToken cancellationToken)
        {

            var activity = _mapper.Map<Activity>(request.ActivityDTO);
            _context.Activities.Add(activity);
            await _context.SaveChangesAsync(cancellationToken);
            return activity.Id;
        }


    }
}
