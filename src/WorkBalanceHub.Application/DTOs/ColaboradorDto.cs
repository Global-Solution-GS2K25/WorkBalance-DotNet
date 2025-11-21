namespace WorkBalanceHub.Application.DTOs;

public class ColaboradorDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Cargo { get; set; } = string.Empty;
    public int EquipeId { get; set; }
    public string? EquipeNome { get; set; }
    public DateTime DataCadastro { get; set; }
    public bool Ativo { get; set; }
}

public class CriarColaboradorDto
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Cargo { get; set; } = string.Empty;
    public int EquipeId { get; set; }
}

public class AtualizarColaboradorDto
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Cargo { get; set; } = string.Empty;
    public int EquipeId { get; set; }
}

