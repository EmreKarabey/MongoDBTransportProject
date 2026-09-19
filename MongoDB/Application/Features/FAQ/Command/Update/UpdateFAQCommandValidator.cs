using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.FAQ.Command.Update
{
    public class UpdateFAQCommandValidator : AbstractValidator<UpdateFAQCommand>
    {
        public UpdateFAQCommandValidator()
        {
            RuleFor(c => c.FaqId).NotEmpty();
            RuleFor(c => c.Question).NotEmpty().WithMessage("Question cannot be blank");
            RuleFor(c => c.Description).NotEmpty().WithMessage("Description cannot be blank");
        }
    }
}
