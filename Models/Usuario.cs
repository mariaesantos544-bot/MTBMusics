using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MTBMusics.Models;

[Table("Usuario")]
[Index("Email", Name = "UQ__Usuario__A9D10534CB621646", IsUnique = true)]
public partial class Usuario
{
    [Key]
    public int IdUsuario { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Nome { get; set; } = null!;

    [StringLength(150)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string Senha { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string TipoUsuario { get; set; } = null!;

    public bool Ativo { get; set; }

    public DateOnly DataCadastro { get; set; }
}
