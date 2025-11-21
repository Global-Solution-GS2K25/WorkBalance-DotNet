namespace WorkBalanceHub.Domain.Entities;

public class EstacaoTrabalho
{
    public int Id { get; private set; }
    public string Nome { get; private set; }
    public string? Localizacao { get; private set; }
    public string? Descricao { get; private set; }
    public DateTime DataCadastro { get; private set; }
    public bool Ativa { get; private set; }

    // Navegação
    public virtual ICollection<LeituraAmbiente> LeiturasAmbiente { get; private set; } = new List<LeituraAmbiente>();

    private EstacaoTrabalho() { } // EF Core

    public EstacaoTrabalho(string nome, string? localizacao = null, string? descricao = null)
    {
        ValidarNome(nome);
        Nome = nome;
        Localizacao = localizacao;
        Descricao = descricao;
        DataCadastro = DateTime.UtcNow;
        Ativa = true;
    }

    public void Atualizar(string nome, string? localizacao = null, string? descricao = null)
    {
        ValidarNome(nome);
        Nome = nome;
        Localizacao = localizacao;
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
            throw new ArgumentException("O nome da estação de trabalho é obrigatório.", nameof(nome));
        
        if (nome.Length > 100)
            throw new ArgumentException("O nome da estação de trabalho não pode exceder 100 caracteres.", nameof(nome));
    }
}

