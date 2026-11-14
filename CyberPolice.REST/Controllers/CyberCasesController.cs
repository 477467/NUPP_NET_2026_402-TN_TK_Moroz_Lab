using CyberPolice.Infrastructure;
using CyberPolice.Infrastructure.Models;
using CyberPolice.REST.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        // Перегляд справ - без авторизації
        [HttpGet]
        [AllowAnonymous]
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
        [AllowAnonymous]
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

        // Створення - будь-який авторизований користувач
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Create([FromBody] CyberCaseCreateModel model)
        {
            var newCase = new CyberCaseModel
            {
                Title = model.Title,
                Status = "Відкрито",
                OpenedDate = DateTime.Now
            };
            await _service.CreateAsync(newCase);
            return CreatedAtAction(nameof(GetById), new { id = newCase.Id }, new CyberCaseApiModel
            {
                Id = newCase.Id,
                Title = newCase.Title,
                Status = newCase.Status,
                OpenedDate = newCase.OpenedDate
            });
        }

        // Оновлення - слідчий або адміністратор
        [HttpPut("{id}")]
        [Authorize(Roles = "Investigator,Admin")]
        public async Task<ActionResult> Update(int id, [FromBody] CyberCaseCreateModel model)
        {
            var existing = await _service.ReadAsync(id);
            if (existing == null) return NotFound();

            existing.Title = model.Title;
            await _service.UpdateAsync(existing);
            return NoContent();
        }

        // Видалення - лише адміністратор
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