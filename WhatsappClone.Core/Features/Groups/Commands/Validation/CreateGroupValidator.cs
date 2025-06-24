using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Core.Features.Groups.Commands.Models;

namespace WhatsappClone.Core.Features.Groups.Commands.Validation
{
    public class CreateGroupValidator : AbstractValidator<CreateGroupCommand>
    {

        public CreateGroupValidator()
        {
            ApplyValidations();
            ApplyCustomValidations();


        }



        public void ApplyValidations()
        {

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Group Name is Required");




            RuleFor(x => x.UserIDs)
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
