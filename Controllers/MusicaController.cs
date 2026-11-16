using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTBMusics.DTO;
using MTBMusics.Interfaces;
using MTBMusics.Models;

namespace MTBMusics.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MusicaController : ControllerBase
{
    private readonly IMusicaRepository _musicaRepository;
    private readonly IGeneroRepository _generoRepository;

    public MusicaController(IMusicaRepository musicaRepository, IGeneroRepository generoRepository)
    {
        _musicaRepository = musicaRepository;
        _generoRepository = generoRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 10, [FromQuery] string? busca = null)
    {
        if (pagina < 1 || tamanhoPagina is < 1 or > 100)
            return BadRequest("Página deve ser positiva e o tamanhoPagina deve estar entre 1 e 100.");

        var (items, total) = await _musicaRepository.ListarPaginado(pagina, tamanhoPagina, busca);
        return Ok(new { items, total, pagina, tamanhoPagina });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        var musica = await _musicaRepository.BuscarPorId(id);
        return musica is null ? NotFound() : Ok(musica);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Cadastrar([FromBody] MusicaDTO dto)
    {
        var generos = await BuscarGeneros(dto.IdGeneros ?? new List<int>());
        if (generos is null) return BadRequest("Um ou mais IDs de gênero não existem.");

        var musica = Converter(dto, generos);

        await _musicaRepository.Cadastrar(musica);

        return CreatedAtAction(nameof(BuscarPorId), new { id = musica.IdMusica }, musica);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] MusicaDTO dto)
    {
        var atual = await _musicaRepository.BuscarPorId(id);
        if (atual is null) return NotFound();

        var idsGeneros = (dto.IdGeneros != null && dto.IdGeneros.Count > 0)
            ? dto.IdGeneros
            : atual.IdGenero.Select(g => g.IdGenero).ToList();

        var generos = await BuscarGeneros(idsGeneros);
        if (generos is null) return BadRequest("Um ou mais IDs de gênero não existem.");

        await _musicaRepository.Atualizar(id, Converter(dto, generos));

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Excluir(int id)
    {
        var musica = await _musicaRepository.BuscarPorId(id);
        if (musica is null) return NotFound();
        await _musicaRepository.Excluir(id);
        return NoContent();
    }

    private async Task<List<Genero>?> BuscarGeneros(List<int> ids)
    {
        var distintos = ids.Distinct().ToArray();
        var generos = await _generoRepository.BuscarPorIds(distintos);
        return generos.Count == distintos.Length ? generos : null;
    }

    private static Musica Converter(MusicaDTO dto, List<Genero> generos) => new()
    {
        Titulo = dto.Titulo,
        Artista = dto.Artista,
        DataLancamento = dto.DataLancamento,
        DuracaoSegundos = dto.DuracaoSegundos,
        Letra = dto.Letra,
        IdGenero = generos
    };
}
