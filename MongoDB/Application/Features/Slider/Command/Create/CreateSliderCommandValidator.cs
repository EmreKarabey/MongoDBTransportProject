using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.Slider.Command.Create
{
    public class CreateSliderCommandValidator:AbstractValidator<CreateSliderCommand>
    {
        public CreateSliderCommandValidator()
        {
            RuleFor(n => n.Title).NotEmpty().WithMessage("Title cannot be blank");
            RuleFor(n => n.SubTitle).NotEmpty().WithMessage("SubTitle cannot be blank");
            RuleFor(n => n.ImageURL).NotEmpty().WithMessage("ImageURL cannot be blank");
            RuleFor(n => n.Description).NotEmpty().WithMessage("Description cannot be blank");
        }
    }
}
