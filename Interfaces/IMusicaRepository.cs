using System.Collections.Generic;
using System.Threading.Tasks;
using MTBMusics.Models;

namespace MTBMusics.Interfaces;

public interface IMusicaRepository
{
    Task Cadastrar(Musica musica);
    Task Atualizar(int id, Musica musica);
    Task Excluir(int id);
    Task<Musica?> BuscarPorId(int id);
    Task<(List<Musica> musicas, int total)> ListarPaginado(int pagina = 1, int tamanhoPagina = 10, string? busca = null);
}
