using System.Collections.Generic;
using System.Threading.Tasks;
using MTBMusics.Models;

namespace MTBMusics.Interfaces;

public interface IGeneroRepository
{
    Task Cadastrar(Genero genero);
    Task Atualizar(int id, Genero genero);
    Task Excluir(int id);
    Task<Genero?> BuscarPorId(int id);
    Task<List<Genero>> Listar();
    Task<List<Genero>> BuscarPorIds(IReadOnlyCollection<int> ids);
}
