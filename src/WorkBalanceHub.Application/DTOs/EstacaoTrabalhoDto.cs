namespace WorkBalanceHub.Application.DTOs;

public class EstacaoTrabalhoDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Localizacao { get; set; }
    public string? Descricao { get; set; }
    public DateTime DataCadastro { get; set; }
    public bool Ativa { get; set; }
}

public class CriarEstacaoTrabalhoDto
{
    public string Nome { get; set; } = string.Empty;
    public string? Localizacao { get; set; }
    public string? Descricao { get; set; }
}

public class AtualizarEstacaoTrabalhoDto
{
    public string Nome { get; set; } = string.Empty;
    public string? Localizacao { get; set; }
    public string? Descricao { get; set; }
}

