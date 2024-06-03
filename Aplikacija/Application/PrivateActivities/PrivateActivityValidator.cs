using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Application.Activities;
using Domain;
using FluentValidation;

namespace Application.PrivateActivities
{
    public class PrivateActivityValidator : AbstractValidator<PrivateActivity> 
    {
        public PrivateActivityValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Date).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Sport).NotEmpty();
            RuleFor(x => x.MaxParticipants).NotEmpty();

        }
    }
}