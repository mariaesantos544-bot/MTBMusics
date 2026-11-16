using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MTBMusics.DTO;

public class MusicaDTO : IValidatableObject
{
    [Required(ErrorMessage = "O título da música é obrigatório.")]
    [StringLength(150, ErrorMessage = "O título não pode ultrapassar 150 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "O artista da música é obrigatório.")]
    [StringLength(150, ErrorMessage = "O artista não pode ultrapassar 150 caracteres.")]
    public string Artista { get; set; } = string.Empty;

    [Required(ErrorMessage = "A data de lançamento é obrigatória.")]
    public DateOnly DataLancamento { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "A duração em segundos deve ser positiva.")]
    public int DuracaoSegundos { get; set; }

    public string? Letra { get; set; }

    public List<int>? IdGeneros { get; set; } = new();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DataLancamento.Year < 1860 || DataLancamento.Year > DateTime.UtcNow.Year)
        {
            yield return new ValidationResult(
                "O ano de lançamento deve estar entre 1860 e o ano atual.",
                new[] { nameof(DataLancamento) });
        }
    }
}
