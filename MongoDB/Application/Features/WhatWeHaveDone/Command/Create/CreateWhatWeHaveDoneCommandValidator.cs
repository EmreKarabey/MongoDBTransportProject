using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.WhatWeHaveDone.Command.Create
{
    public class CreateWhatWeHaveDoneCommandValidator:AbstractValidator<CreateWhatWeHaveDoneCommand>
    {
        public CreateWhatWeHaveDoneCommandValidator()
        {
            RuleFor(n => n.Title).NotEmpty().WithMessage("Title cannot be blank");
            RuleFor(n => n.Description).NotEmpty().WithMessage("Description cannot be blank");
            RuleFor(n => n.ImageURL).NotEmpty().WithMessage("ImageURL cannot be blank");
        }
    }
}
