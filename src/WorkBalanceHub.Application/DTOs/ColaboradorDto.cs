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
    public IEnumerable<LinkDto>? Links { get; set; }
}

public class CriarColaboradorDto
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Cargo { get; set; } = string.Empty;
    public int EquipeId { get; set; }
}

public class LinkDto
{
    public string Rel { get; set; } = string.Empty;
    public string Href { get; set; } = string.Empty;
    public string Method { get; set; } = "GET";
}

public class AtualizarColaboradorDto
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Cargo { get; set; } = string.Empty;
    public int EquipeId { get; set; }
}

