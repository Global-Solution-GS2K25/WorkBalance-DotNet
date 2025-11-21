namespace WorkBalanceHub.Domain.Entities;

public class Colaborador
{
    public int Id { get; private set; }
    public string Nome { get; private set; }
    public string Email { get; private set; }
    public string Cargo { get; private set; }
    public int EquipeId { get; private set; }
    public DateTime DataCadastro { get; private set; }
    public bool Ativo { get; private set; }

    // Navegação
    public virtual Equipe Equipe { get; private set; } = null!;
    public virtual ICollection<CheckIn> CheckIns { get; private set; } = new List<CheckIn>();

    private Colaborador() { } // EF Core

    public Colaborador(string nome, string email, string cargo, int equipeId)
    {
        ValidarNome(nome);
        ValidarEmail(email);
        ValidarCargo(cargo);
        
        Nome = nome;
        Email = email;
        Cargo = cargo;
        EquipeId = equipeId;
        DataCadastro = DateTime.UtcNow;
        Ativo = true;
    }

    public void Atualizar(string nome, string email, string cargo, int equipeId)
    {
        ValidarNome(nome);
        ValidarEmail(email);
        ValidarCargo(cargo);
        
        Nome = nome;
        Email = email;
        Cargo = cargo;
        EquipeId = equipeId;
    }

    public void Desativar()
    {
        Ativo = false;
    }

    public void Ativar()
    {
        Ativo = true;
    }

    private static void ValidarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do colaborador é obrigatório.", nameof(nome));
        
        if (nome.Length > 200)
            throw new ArgumentException("O nome do colaborador não pode exceder 200 caracteres.", nameof(nome));
    }

    private static void ValidarEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("O e-mail do colaborador é obrigatório.", nameof(email));
        
        if (!email.Contains('@'))
            throw new ArgumentException("O e-mail deve ter um formato válido.", nameof(email));
        
        if (email.Length > 255)
            throw new ArgumentException("O e-mail não pode exceder 255 caracteres.", nameof(email));
    }

    private static void ValidarCargo(string cargo)
    {
        if (string.IsNullOrWhiteSpace(cargo))
            throw new ArgumentException("O cargo do colaborador é obrigatório.", nameof(cargo));
        
        if (cargo.Length > 100)
            throw new ArgumentException("O cargo não pode exceder 100 caracteres.", nameof(cargo));
    }
}

