using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Features.Groups.Commands.Models;

namespace WhatsappClone.Core.Features.Groups.Commands.Validation
{
    public class RemoveMembersValidator : AbstractValidator<RemoveMemberCommand>
    {

        public RemoveMembersValidator()
        {
            ApplyValidations();
            ApplyCustomValidations();


        }



        public void ApplyValidations()
        {

            RuleFor(x => x.GroupId)
                .NotEmpty()
                .WithMessage("GroupId is Required")
                .NotNull()
                .WithMessage("GroupId can't be null");




            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("Add at least 1 member")
                .NotNull()
                .WithMessage("List can't be null");













        }


        public void ApplyCustomValidations()
        {

        }
    }
}
