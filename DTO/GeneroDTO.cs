using System.ComponentModel.DataAnnotations;

namespace MTBMusics.DTO;

public class GeneroDTO
{
    [Required(ErrorMessage = "O nome do gênero é obrigatório.")]
    [StringLength(50, ErrorMessage = "O nome do gênero não pode ultrapassar 50 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [StringLength(255, ErrorMessage = "A descrição não pode ultrapassar 255 caracteres.")]
    public string? Descricao { get; set; }
}
