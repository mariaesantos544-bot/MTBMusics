using System.ComponentModel.DataAnnotations;

namespace MTBMusics.DTO;

public class UsuarioDTO
{
    [Required(ErrorMessage = "O nome do usuário é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome não pode ultrapassar 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    [StringLength(150, ErrorMessage = "O e-mail não pode ultrapassar 150 caracteres.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [MinLength(8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres.")]
    public string Senha { get; set; } = string.Empty;

    [StringLength(20)]
    public string? TipoUsuario { get; set; } = "Usuario";
}
