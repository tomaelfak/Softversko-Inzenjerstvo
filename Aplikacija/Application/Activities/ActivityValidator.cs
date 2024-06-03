using System.Data;
using Domain;
using FluentValidation;

namespace Application.Activities
{
    public class ActivityValidator : AbstractValidator<Activity>
    {
        public ActivityValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Date).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Sport).NotEmpty();
            RuleFor(x => x.MaxParticipants).NotEmpty();
            

        }

    }
}