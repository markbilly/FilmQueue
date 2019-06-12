using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodDiary.WebApi.DataAccess;
using FoodDiary.WebApi.Domain;
using FoodDiary.WebApi.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDiary.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/symptoms")]
    public class SymptomsController : ControllerBase
    {
        private readonly ISymptomReader _symptomReader;
        private readonly ISymptomWriter _symptomWriter;
        private readonly IUnitOfWork _unitOfWork;

        public SymptomsController(
            ISymptomReader symptomReader,
            ISymptomWriter symptomWriter,
            IUnitOfWork unitOfWork)
        {
            _symptomReader = symptomReader;
            _symptomWriter = symptomWriter;
            _unitOfWork = unitOfWork;
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<QueryResponse<string>>), 200)]
        public async Task<IActionResult> QuerySymptoms(string searchTerm)
        {
            var queryResults = await _symptomReader.Query(searchTerm);

            return Ok(QueryResponse<string>.FromEnumerable(queryResults, r => r.Name));
        }

        [HttpPost]
        public async Task<IActionResult> CreateSymptom([FromBody] CreateSymptomRequest createSymptomRequest)
        {
            if (string.IsNullOrWhiteSpace(createSymptomRequest.Name))
            {
                return BadRequest("Must provide a name");
            }

            await _unitOfWork.Execute(async () =>
            {
                await _symptomWriter.Create(createSymptomRequest.Name);
            });

            return Ok();
        }
    }
}
