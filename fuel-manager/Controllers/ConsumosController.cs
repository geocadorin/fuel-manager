using fuel_manager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fuel_manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsumosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ConsumosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var model = await _context.Consumos.ToListAsync();
            return Ok(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Consumo model)
        {
            _context.Consumos.Add(model);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetById", new { id = model.Id }, model);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var model = await _context.Consumos.FirstOrDefaultAsync(c => c.Id == id);
            if (model == null) return NotFound();

            GerarLinks(model);
            return Ok(model);
        }

        [HttpPut]
        public async Task<ActionResult> Update(Consumo model)
        {
            
            if (await _context.Consumos.AsNoTracking().FirstOrDefaultAsync(c => c.Id == model.Id) == null) return NotFound();

            _context.Consumos.Update(model);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var model = await _context.Consumos.FindAsync(id);
            if (model == null) return NotFound();
            _context.Consumos.Remove(model);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private void GerarLinks(Consumo model)
        {
            model.Links.Add(new LinkDto(model.Id, Url.ActionLink(), "self", "GET"));
            model.Links.Add(new LinkDto(model.Id, Url.ActionLink(), "update", "PUT"));
            model.Links.Add(new LinkDto(model.Id, Url.ActionLink(), "delete", "Delete"));
        }
    }
}
