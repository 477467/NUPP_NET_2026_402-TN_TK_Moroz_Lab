using Microsoft.AspNetCore.Mvc;
using CyberPolice.Infrastructure;
using CyberPolice.Infrastructure.Models;
using CyberPolice.REST.Models;

namespace CyberPolice.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvestigatorsController : ControllerBase
    {
        private readonly ICrudServiceAsyncDb<InvestigatorModel> _service;

        public InvestigatorsController(ICrudServiceAsyncDb<InvestigatorModel> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvestigatorApiModel>>> GetAll()
        {
            var investigators = await _service.ReadAllAsync();
            var result = investigators.Select(i => new InvestigatorApiModel
            {
                Id = i.Id,
                FullName = i.FullName,
                Rank = i.Rank,
                Specialization = i.Specialization,
                CasesSolved = i.CasesSolved
            });
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InvestigatorApiModel>> GetById(int id)
        {
            var i = await _service.ReadAsync(id);
            if (i == null) return NotFound();

            return Ok(new InvestigatorApiModel
            {
                Id = i.Id,
                FullName = i.FullName,
                Rank = i.Rank,
                Specialization = i.Specialization,
                CasesSolved = i.CasesSolved
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] InvestigatorCreateModel model)
        {
            var newInvestigator = new InvestigatorModel
            {
                FullName = model.FullName,
                Rank = model.Rank,
                Specialization = model.Specialization,
                HireDate = DateTime.Now,
                CasesSolved = 0
            };
            await _service.CreateAsync(newInvestigator);
            return CreatedAtAction(nameof(GetById), new { id = newInvestigator.Id }, newInvestigator);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] InvestigatorCreateModel model)
        {
            var existing = await _service.ReadAsync(id);
            if (existing == null) return NotFound();

            existing.FullName = model.FullName;
            existing.Rank = model.Rank;
            existing.Specialization = model.Specialization;
            await _service.UpdateAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existing = await _service.ReadAsync(id);
            if (existing == null) return NotFound();

            await _service.RemoveAsync(existing);
            return NoContent();
        }
    }
}