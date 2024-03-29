﻿using fuel_manager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fuel_manager.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VeiculosController(AppDbContext context)
        {
            _context = context;
        }

        //[Authorize(Roles = "Administrador")]
        [Authorize(Roles = "Administrador, Usuario")]
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var model = await _context.Veiculos.ToListAsync();
            return Ok(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Veiculo model)
        {
            if(model.anoFabricacao <=0 || model.anoModelo <= 0)
            {
                return BadRequest(new {message = "Ano modelo e ano fabricação são obrigatórios!"});
            }

            _context.Veiculos.Add(model);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetById", new {id = model.Id}, model);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var model = await _context.Veiculos
                .Include(t => t.Usuarios).ThenInclude(t => t.Usuario)
                .Include(t => t.Consumos)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (model == null) return NotFound();

            GerarLinks(model);
            return Ok(model);
        }

        [HttpPut]
        public async Task<ActionResult> Update(Veiculo model)
        {
            if (model.anoFabricacao <= 0 || model.anoModelo <= 0 && model.Id == null)
            {
                return BadRequest(new { message = "Ano modelo e ano fabricação são obrigatórios!" });
            }

            if(await _context.Veiculos.AsNoTracking().FirstOrDefaultAsync(c => c.Id == model.Id) == null) return NotFound();
            
            _context.Veiculos.Update(model);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var model = await _context.Veiculos.FindAsync(id);
            if(model == null) return NotFound();
            _context.Veiculos.Remove(model);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private void GerarLinks(Veiculo model)
        {
            model.Links.Add(new LinkDto(model.Id, Url.ActionLink(), "self", "GET"));
            model.Links.Add(new LinkDto(model.Id, Url.ActionLink(), "update", "PUT"));
            model.Links.Add(new LinkDto(model.Id, Url.ActionLink(), "delete", "Delete"));
        }

        [HttpPost("{id}/usuarios")]
        public async Task<ActionResult> AddUsuario(int id, VeiculoUsuarios model)
        {
            if (id != model.VeiculoId) return BadRequest();
            _context.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetById", new {id = model.VeiculoId}, model);
        }

        [HttpDelete("{id}/usuarios/{usuarioId}")]
        public async Task<ActionResult> RemoverUsuario(int id, int usuarioId)
        {
            var model = await _context.VeiculoUsuarios
                .Where(c => c.VeiculoId == id && c.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();

            if (model == null) return NotFound();

            _context.VeiculoUsuarios.Remove(model);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
