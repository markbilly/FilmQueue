﻿using FilmQueue.WebApi.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain.Commands
{
    public class CreateFilmCommand : ICommand
    {
        public string UserId { get; set; }
        public string Title { get; set; }
        public int RuntimeInMinutes { get; set; }
    }
}
