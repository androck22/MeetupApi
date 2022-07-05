using FluentValidation;
using MeetupApi.Contracts.Models.Meetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetupApi.Contracts.Validators
{
    public class EditMeetupRequestValidator : AbstractValidator<EditMeetupRequest>
    {
        public EditMeetupRequestValidator()
        {
            RuleFor(x => x.NewName).NotEmpty();
            RuleFor(x => x.NewDescription).NotEmpty();
            RuleFor(x => x.NewOrganizer).NotEmpty();
            RuleFor(x => x.NewEventDate).NotEmpty();
            RuleFor(x => x.NewLocationInfo.NewHouse)
                .NotEmpty()
                .InclusiveBetween(1, 300)
                .WithMessage("Номер дома должен находиться в диапазоне от: 1 до 300");
            RuleFor(x => x.NewLocationInfo.NewBuilding)
                .InclusiveBetween(1, 10)
                .WithMessage("Корпуса домов должны попадать в диапазон от: 1 до 10");
            RuleFor(x => x.NewLocationInfo.NewStreet).NotEmpty();
        }
    }
}
