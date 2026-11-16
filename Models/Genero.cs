using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace MTBMusics.Models;

[Table("Genero")]
[Index("Nome", Name = "UQ__Genero__7D8FE3B28B6888A2", IsUnique = true)]
public partial class Genero
{
    [Key]
    public int IdGenero { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Nome { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string? Descricao { get; set; }

    [JsonIgnore]
    [ForeignKey("IdGenero")]
    [InverseProperty("IdGenero")]
    public virtual ICollection<Musica> IdMusica { get; set; } = new List<Musica>();
}
