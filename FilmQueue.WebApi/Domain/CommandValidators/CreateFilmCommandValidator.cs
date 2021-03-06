﻿using FilmQueue.WebApi.Domain.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain.CommandValidators
{
    public class CreateFilmCommandValidator : AbstractValidator<CreateFilmCommand>
    {
        public CreateFilmCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty()
                .WithMessage("Title is a required field.");

            RuleFor(x => x.Title).MaximumLength(200)
                .WithMessage("Title cannot be longer than 200 characters");

            RuleFor(x => x.RuntimeInMinutes).GreaterThan(0)
                .WithMessage("Runtime is not valid. Must be positive whole number.");
        }
    }
}
