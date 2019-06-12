using FoodDiary.WebApi.DataAccess;
using FoodDiary.WebApi.Domain;
using FoodDiary.WebApi.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDiary.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    public class SymptomLogsController : ControllerBase
    {
        private readonly ISymptomLogWriter _symptomLogWriter;
        private readonly ISymptomReader _symptomReader;
        private readonly IUnitOfWork _unitOfWork;

        public SymptomLogsController(
            ISymptomLogWriter symptomLogWriter,
            ISymptomReader symptomReader,
            IUnitOfWork unitOfWork)
        {
            _symptomLogWriter = symptomLogWriter;
            _symptomReader = symptomReader;
            _unitOfWork = unitOfWork;
        }

        [Route("api/symptomseverityoptions")]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        [HttpGet]
        public IActionResult Get()
        {
            var names = Enum.GetNames(typeof(SymptomSeverity));

            return Ok(names);
        }

        [Route("api/symptomlogs")]
        [HttpPost]
        public async Task<IActionResult> LogSymptom([FromBody] CreateSymptomLogRequest createSymptomLogRequest)
        {
            if (string.IsNullOrWhiteSpace(createSymptomLogRequest.SymptomName))
            {
                return BadRequest("Must provide a valid symptom");
            }

            var symptom = await _symptomReader.GetByName(createSymptomLogRequest.SymptomName);

            if (symptom == null)
            {
                return BadRequest("Must provide a valid symptom");
            }

            await _unitOfWork.Execute(async () =>
            {
                await _symptomLogWriter.CreateSymptomLog(symptom.Id, createSymptomLogRequest.Severity);
            });

            return Ok();
        }
    }
}
