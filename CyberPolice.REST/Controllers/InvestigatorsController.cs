using CyberPolice.Infrastructure;
using CyberPolice.Infrastructure.Models;
using CyberPolice.REST.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CyberPolice.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Investigator,Analyst")]
    public class InvestigatorsController : ControllerBase
    {
        private readonly ICrudServiceAsyncDb<InvestigatorModel> _service;

        public InvestigatorsController(ICrudServiceAsyncDb<InvestigatorModel> service)
        {
            _service = service;
        }

        // Перегляд - усі три ролі (обмеження на рівні класу)
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

        // Створення, оновлення, видалення - лише адміністратор
        [HttpPost]
        [Authorize(Roles = "Admin")]
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
            return CreatedAtAction(nameof(GetById), new { id = newInvestigator.Id }, new InvestigatorApiModel
            {
                Id = newInvestigator.Id,
                FullName = newInvestigator.FullName,
                Rank = newInvestigator.Rank,
                Specialization = newInvestigator.Specialization,
                CasesSolved = newInvestigator.CasesSolved
            });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var existing = await _service.ReadAsync(id);
            if (existing == null) return NotFound();

            await _service.RemoveAsync(existing);
            return NoContent();
        }
    }
}