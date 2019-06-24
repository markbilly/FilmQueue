using FilmQueue.WebApi.Infrastructure.Events;
using FilmQueue.WebApi.Infrastructure.FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Infrastructure
{
    [ApiController]
    public class ApiController : ControllerBase
    {
        protected IActionResult FromFailedValidationResult(ValidationFailedEvent failedEvent)
        {
            if (failedEvent.ValidationResult.IsNotFoundResult())
            {
                return NotFound();
            }

            failedEvent.ValidationResult.AddToModelState(ModelState, null);
            return BadRequest(ModelState);
        }
    }
}
