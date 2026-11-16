using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MTBMusics.Models;

[Table("Musica")]
public partial class Musica
{
    [Key]
    public int IdMusica { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string Titulo { get; set; } = null!;

    [StringLength(150)]
    [Unicode(false)]
    public string Artista { get; set; } = null!;

    public DateOnly DataLancamento { get; set; }

    public int DuracaoSegundos { get; set; }

    [Unicode(false)]
    public string? Letra { get; set; }

    public DateOnly DataCadastro { get; set; }

    public DateOnly? DataAtualizacao { get; set; }

    [ForeignKey("IdMusica")]
    [InverseProperty("IdMusica")]
    public virtual ICollection<Genero> IdGenero { get; set; } = new List<Genero>();
}
