using FluentValidation;
using MeetupApi.Contracts.Models.Meetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetupApi.Contracts.Validators
{
    public class AddMeetupRequestValidator : AbstractValidator<AddMeetupRequest>
    {
        public AddMeetupRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Organizer).NotEmpty();
            RuleFor(x => x.EventDate).NotEmpty();
            RuleFor(x => x.LocationInfo.House)
                .NotEmpty()
                .InclusiveBetween(1, 300)
                .WithMessage("Номер дома должен находиться в диапазоне от: 1 до 300");
            RuleFor(x => x.LocationInfo.Building)
                .InclusiveBetween(1, 10)
                .WithMessage("Корпуса домов должны попадать в диапазон от: 1 до 10");
            RuleFor(x => x.LocationInfo.Street).NotEmpty();
        }
    }
}
