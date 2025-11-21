namespace WorkBalanceHub.Application.DTOs;

public class PlanoAcaoDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int EquipeId { get; set; }
    public string? EquipeNome { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
    public string? Observacoes { get; set; }
    public bool EstaAtivo { get; set; }
}

public class CriarPlanoAcaoDto
{
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int EquipeId { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
}

public class AtualizarPlanoAcaoDto
{
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public string? Observacoes { get; set; }
}

public class AlterarStatusPlanoAcaoDto
{
    public string Status { get; set; } = string.Empty;
}

