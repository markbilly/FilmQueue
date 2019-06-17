﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain
{
    public class CreateSymptomLogRequest
    {
        public string SymptomName { get; set; }
        public SymptomSeverity Severity { get; set; }
    }
}