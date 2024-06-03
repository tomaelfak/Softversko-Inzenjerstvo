using Application.Activities;
using Domain;
using FluentValidation;

namespace Application.Courts
{
    public class CourtValidator : AbstractValidator<Court>
    {
        public CourtValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Address).NotEmpty();
            RuleFor(x => x.Latitude).NotEmpty();
            RuleFor(x => x.Longitude).NotEmpty();
            RuleFor(x => x.Sport).NotEmpty();


        }

    }
}