using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodDiary.WebApi.DataAccess;
using FoodDiary.WebApi.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDiary.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    public class SymptomsController : ControllerBase
    {
        private readonly ISymptomReader _symptomReader;
        private readonly ISymptomWriter _symptomWriter;

        public SymptomsController(
            ISymptomReader symptomReader,
            ISymptomWriter symptomWriter)
        {
            _symptomReader = symptomReader;
            _symptomWriter = symptomWriter;
        }

        [Route("api/symptomseverityoptions")]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        [HttpGet]
        public IActionResult Get()
        {
            var names = Enum.GetNames(typeof(DataAccess.Models.SymptomSeverity));

            return Ok(names);
        }
        
        [Route("api/symptoms")]
        [ProducesResponseType(typeof(IEnumerable<QueryResponse<string>>), 200)]
        [HttpGet]
        public async Task<IActionResult> QuerySymptoms(string searchTerm)
        {
            var queryResults = await _symptomReader.Query(searchTerm);

            return Ok(QueryResponse<string>.FromEnumerable(queryResults, r => r.Name));
        }

        [Route("api/symptoms")]
        [HttpPost]
        public async Task<IActionResult> CreateSymptom([FromBody] CreateSymptomRequest createSymptomRequest)
        {
            if (string.IsNullOrWhiteSpace(createSymptomRequest.Name))
            {
                return BadRequest("Must provide a name");
            }

            await _symptomWriter.Create(createSymptomRequest.Name);

            return Ok();
        }
    }
}
