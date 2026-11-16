using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTBMusics.DTO;
using MTBMusics.Interfaces;
using MTBMusics.Models;

namespace MTBMusics.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioController(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Listar()
    {
        var usuarios = await _usuarioRepository.Listar();
        return Ok(usuarios.Select(UsuarioRespostaDTO.Converter));
    }

    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        if (!PodeAcessar(id)) return Forbid();
        var usuario = await _usuarioRepository.BuscarPorId(id);
        return usuario is null ? NotFound() : Ok(UsuarioRespostaDTO.Converter(usuario));
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Cadastrar([FromBody] UsuarioDTO dto)
    {
        var usuario = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            Senha = dto.Senha,
            TipoUsuario = "Usuario",
            Ativo = true,
            DataCadastro = DateOnly.FromDateTime(DateTime.UtcNow)
        };

        await _usuarioRepository.Cadastrar(usuario);

        return CreatedAtAction(nameof(BuscarPorId), new { id = usuario.IdUsuario }, UsuarioRespostaDTO.Converter(usuario));
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Atualizar(int id, [FromBody] UsuarioAtualizacaoDTO dto)
    {
        if (!PodeAcessar(id)) return Forbid();
        var atual = await _usuarioRepository.BuscarPorId(id);
        if (atual is null) return NotFound();

        await _usuarioRepository.Atualizar(id, new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            Senha = dto.Senha ?? string.Empty,
            TipoUsuario = atual.TipoUsuario,
            Ativo = atual.Ativo,
            DataCadastro = atual.DataCadastro
        });

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Excluir(int id)
    {
        if (!PodeAcessar(id)) return Forbid();
        if (await _usuarioRepository.BuscarPorId(id) is null) return NotFound();
        await _usuarioRepository.Excluir(id);
        return NoContent();
    }

    private bool PodeAcessar(int id) => User.IsInRole("Admin")
        || (int.TryParse(User.FindFirstValue(JwtRegisteredClaimNames.Sub), out var usuarioId) && usuarioId == id);
}
