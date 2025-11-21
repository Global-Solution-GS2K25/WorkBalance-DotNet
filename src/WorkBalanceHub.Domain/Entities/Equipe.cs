namespace WorkBalanceHub.Domain.Entities;

public class Equipe
{
    public int Id { get; private set; }
    public string Nome { get; private set; }
    public string? Descricao { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public bool Ativa { get; private set; }

    // Navegação
    public virtual ICollection<Colaborador> Colaboradores { get; private set; } = new List<Colaborador>();
    public virtual ICollection<PlanoAcao> PlanosAcao { get; private set; } = new List<PlanoAcao>();

    private Equipe() { } // EF Core

    public Equipe(string nome, string? descricao = null)
    {
        ValidarNome(nome);
        Nome = nome;
        Descricao = descricao;
        DataCriacao = DateTime.UtcNow;
        Ativa = true;
    }

    public void Atualizar(string nome, string? descricao = null)
    {
        ValidarNome(nome);
        Nome = nome;
        Descricao = descricao;
    }

    public void Desativar()
    {
        Ativa = false;
    }

    public void Ativar()
    {
        Ativa = true;
    }

    private static void ValidarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome da equipe é obrigatório.", nameof(nome));
        
        if (nome.Length > 100)
            throw new ArgumentException("O nome da equipe não pode exceder 100 caracteres.", nameof(nome));
    }
}

