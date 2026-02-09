using FluentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Features.Groups.Commands.Models;

namespace WhatsappClone.Core.Features.Groups.Commands.Validation
{
    public class AddListOfMembersValidator : AbstractValidator<AddListOfMembersCommand>
    {

        public AddListOfMembersValidator()
        {
            ApplyValidations();
            ApplyCustomValidations();


        }



        public void ApplyValidations()
        {

            RuleFor(x => x.groupId)
                .NotEmpty()
                .WithMessage("GroupId is Required");




            RuleFor(x => x.members)
                .NotEmpty()
                .WithMessage("Add at least 1 member")
                .NotNull()
                .WithMessage("List can be null");













        }


        public void ApplyCustomValidations()
        {

        }
    }
}
