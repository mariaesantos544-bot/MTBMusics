using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MTBMusics.DTO;
using MTBMusics.Interfaces;
using MTBMusics.Models;

namespace MTBMusics.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GeneroController : ControllerBase
{
    private readonly IGeneroRepository _generoRepository;

    public GeneroController(IGeneroRepository generoRepository)
    {
        _generoRepository = generoRepository;
    }

    /// <summary>
    /// Lista todos os gêneros musicais em ordem alfabética.
    /// </summary>
    /// <returns>Lista de gêneros</returns>
    [HttpGet]
    public async Task<IActionResult> Listar() => Ok(await _generoRepository.Listar());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        var genero = await _generoRepository.BuscarPorId(id);
        return genero is null ? NotFound() : Ok(genero);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Cadastrar([FromBody] GeneroDTO dto)
    {
        var genero = new Genero
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao
        };

        try
        {
            await _generoRepository.Cadastrar(genero);
        }
        catch (DbUpdateException erro)
            when (erro.GetBaseException() is SqlException { Number: 2601 or 2627 })
        {
            return Conflict("Já existe um gênero com esse nome.");
        }

        return CreatedAtAction(nameof(BuscarPorId), new { id = genero.IdGenero }, genero);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] GeneroDTO dto)
    {
        if (await _generoRepository.BuscarPorId(id) is null) return NotFound();

        try
        {
            await _generoRepository.Atualizar(id, new Genero
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao
            });
        }
        catch (DbUpdateException erro)
            when (erro.GetBaseException() is SqlException { Number: 2601 or 2627 })
        {
            return Conflict("Já existe um gênero com esse nome.");
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Excluir(int id)
    {
        if (await _generoRepository.BuscarPorId(id) is null) return NotFound();
        await _generoRepository.Excluir(id);
        return NoContent();
    }
}
