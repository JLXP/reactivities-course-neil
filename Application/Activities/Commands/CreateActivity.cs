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
        private readonly IValidator<Command> _validator;

        public Handler(AppDbContext context, IMapper mapper, IValidator<Command> validator)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<string> Handle(Command request, CancellationToken cancellationToken)
        {

            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var activity = _mapper.Map<Activity>(request.ActivityDTO);
            _context.Activities.Add(activity);
            await _context.SaveChangesAsync(cancellationToken);
            return activity.Id;
        }


    }
}
