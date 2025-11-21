namespace WorkBalanceHub.Application.DTOs;

public class EquipeDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public DateTime DataCriacao { get; set; }
    public bool Ativa { get; set; }
    public int TotalColaboradores { get; set; }
}

public class CriarEquipeDto
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
}

public class AtualizarEquipeDto
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
}

