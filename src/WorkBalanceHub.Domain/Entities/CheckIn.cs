namespace WorkBalanceHub.Domain.Entities;

public class CheckIn
{
    public int Id { get; private set; }
    public int ColaboradorId { get; private set; }
    public DateTime DataCheckIn { get; private set; }
    public int Humor { get; private set; } // 1 a 5
    public int NivelEstresse { get; private set; } // 1 a 5
    public int QualidadeSono { get; private set; } // 1 a 5
    public string? SintomasFisicos { get; private set; }
    public string? Observacoes { get; private set; }

    // Navegação
    public virtual Colaborador Colaborador { get; private set; } = null!;

    private CheckIn() { } // EF Core

    public CheckIn(int colaboradorId, int humor, int nivelEstresse, int qualidadeSono, 
                   string? sintomasFisicos = null, string? observacoes = null)
    {
        ValidarColaboradorId(colaboradorId);
        ValidarHumor(humor);
        ValidarNivelEstresse(nivelEstresse);
        ValidarQualidadeSono(qualidadeSono);
        
        ColaboradorId = colaboradorId;
        DataCheckIn = DateTime.UtcNow;
        Humor = humor;
        NivelEstresse = nivelEstresse;
        QualidadeSono = qualidadeSono;
        SintomasFisicos = sintomasFisicos;
        Observacoes = observacoes;
    }

    public void Atualizar(int humor, int nivelEstresse, int qualidadeSono, 
                         string? sintomasFisicos = null, string? observacoes = null)
    {
        ValidarHumor(humor);
        ValidarNivelEstresse(nivelEstresse);
        ValidarQualidadeSono(qualidadeSono);
        
        Humor = humor;
        NivelEstresse = nivelEstresse;
        QualidadeSono = qualidadeSono;
        SintomasFisicos = sintomasFisicos;
        Observacoes = observacoes;
    }

    private static void ValidarColaboradorId(int colaboradorId)
    {
        if (colaboradorId <= 0)
            throw new ArgumentException("O ID do colaborador deve ser maior que zero.", nameof(colaboradorId));
    }

    private static void ValidarHumor(int humor)
    {
        if (humor < 1 || humor > 5)
            throw new ArgumentException("O humor deve estar entre 1 e 5.", nameof(humor));
    }

    private static void ValidarNivelEstresse(int nivelEstresse)
    {
        if (nivelEstresse < 1 || nivelEstresse > 5)
            throw new ArgumentException("O nível de estresse deve estar entre 1 e 5.", nameof(nivelEstresse));
    }

    private static void ValidarQualidadeSono(int qualidadeSono)
    {
        if (qualidadeSono < 1 || qualidadeSono > 5)
            throw new ArgumentException("A qualidade do sono deve estar entre 1 e 5.", nameof(qualidadeSono));
    }
}

