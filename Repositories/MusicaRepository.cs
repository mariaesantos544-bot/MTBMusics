using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MTBMusics.Context;
using MTBMusics.Interfaces;
using MTBMusics.Models;

namespace MTBMusics.Repositories;

public class MusicaRepository : IMusicaRepository
{
    private readonly MTBMusicsContext _context;

    public MusicaRepository(MTBMusicsContext context)
    {
        _context = context;
    }

    public async Task Atualizar(int id, Musica musica)
    {
        var musicaExistente = await _context.Musica.Include(m => m.IdGenero).FirstOrDefaultAsync(m => m.IdMusica == id);

        if (musicaExistente != null)
        {
            musicaExistente.Titulo = musica.Titulo;
            musicaExistente.Artista = musica.Artista;
            musicaExistente.DuracaoSegundos = musica.DuracaoSegundos;
            musicaExistente.DataLancamento = musica.DataLancamento;
            musicaExistente.Letra = musica.Letra;
            musicaExistente.DataAtualizacao = DateOnly.FromDateTime(DateTime.UtcNow);

            var novosIds = musica.IdGenero.Select(g => g.IdGenero).ToHashSet();
            var generosParaRemover = musicaExistente.IdGenero.Where(g => !novosIds.Contains(g.IdGenero)).ToList();
            foreach (var genero in generosParaRemover)
            {
                musicaExistente.IdGenero.Remove(genero);
            }

            var idsAtuais = musicaExistente.IdGenero.Select(g => g.IdGenero).ToHashSet();
            foreach (var genero in musica.IdGenero.Where(g => !idsAtuais.Contains(g.IdGenero)))
            {
                var trackedGenero = await _context.Genero.FindAsync(genero.IdGenero);
                if (trackedGenero != null)
                {
                    musicaExistente.IdGenero.Add(trackedGenero);
                }
                else
                {
                    _context.Attach(genero);
                    musicaExistente.IdGenero.Add(genero);
                }
            }

            await _context.SaveChangesAsync();
        }
    }

    public async Task<Musica?> BuscarPorId(int id)
    {
        return await _context.Musica.AsNoTracking().Include(m => m.IdGenero).FirstOrDefaultAsync(m => m.IdMusica == id);
    }

    public async Task Cadastrar(Musica musica)
    {
        if (musica.DataCadastro == default)
        {
            musica.DataCadastro = DateOnly.FromDateTime(DateTime.UtcNow);
        }

        await _context.Musica.AddAsync(musica);

        foreach (var genero in musica.IdGenero)
        {
            _context.Entry(genero).State = EntityState.Unchanged;
        }

        await _context.SaveChangesAsync();
    }

    public async Task Excluir(int id)
    {
        var musicaBuscada = await _context.Musica.Include(m => m.IdGenero).FirstOrDefaultAsync(m => m.IdMusica == id);
        if (musicaBuscada != null)
        {
            musicaBuscada.IdGenero.Clear();
            _context.Musica.Remove(musicaBuscada);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<(List<Musica> musicas, int total)> ListarPaginado(int pagina = 1, int tamanhoPagina = 10, string? busca = null)
    {
        var query = _context.Musica.AsNoTracking().Include(m => m.IdGenero).AsQueryable();

        if (!string.IsNullOrWhiteSpace(busca))
        {
            query = query.Where(m => m.Titulo.Contains(busca) || m.Artista.Contains(busca));
        }

        int total = await query.CountAsync();

        var items = await query
            .OrderBy(m => m.Titulo).ThenBy(m => m.IdMusica)
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToListAsync();

        return (items, total);
    }
}
