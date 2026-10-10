using Microsoft.AspNetCore.Mvc;
using CyberPolice.Infrastructure;
using CyberPolice.Infrastructure.Models;
using CyberPolice.REST.Models;

namespace CyberPolice.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CyberCasesController : ControllerBase
    {
        private readonly ICrudServiceAsyncDb<CyberCaseModel> _service;

        public CyberCasesController(ICrudServiceAsyncDb<CyberCaseModel> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CyberCaseApiModel>>> GetAll()
        {
            var cases = await _service.ReadAllAsync();
            var result = cases.Select(c => new CyberCaseApiModel
            {
                Id = c.Id,
                Title = c.Title,
                Status = c.Status,
                OpenedDate = c.OpenedDate
            });
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CyberCaseApiModel>> GetById(int id)
        {
            var c = await _service.ReadAsync(id);
            if (c == null) return NotFound();

            return Ok(new CyberCaseApiModel
            {
                Id = c.Id,
                Title = c.Title,
                Status = c.Status,
                OpenedDate = c.OpenedDate
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CyberCaseCreateModel model)
        {
            var newCase = new CyberCaseModel
            {
                Title = model.Title,
                Status = "Відкрито",
                OpenedDate = DateTime.Now
            };
            await _service.CreateAsync(newCase);
            return CreatedAtAction(nameof(GetById), new { id = newCase.Id }, newCase);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] CyberCaseCreateModel model)
        {
            var existing = await _service.ReadAsync(id);
            if (existing == null) return NotFound();

            existing.Title = model.Title;
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