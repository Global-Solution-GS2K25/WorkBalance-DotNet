namespace WorkBalanceHub.Application.DTOs;

public class CheckInDto
{
    public int Id { get; set; }
    public int ColaboradorId { get; set; }
    public string? ColaboradorNome { get; set; }
    public DateTime DataCheckIn { get; set; }
    public int Humor { get; set; }
    public int NivelEstresse { get; set; }
    public int QualidadeSono { get; set; }
    public string? SintomasFisicos { get; set; }
    public string? Observacoes { get; set; }
}

public class CriarCheckInDto
{
    public int ColaboradorId { get; set; }
    public int Humor { get; set; }
    public int NivelEstresse { get; set; }
    public int QualidadeSono { get; set; }
    public string? SintomasFisicos { get; set; }
    public string? Observacoes { get; set; }
}

public class AtualizarCheckInDto
{
    public int Humor { get; set; }
    public int NivelEstresse { get; set; }
    public int QualidadeSono { get; set; }
    public string? SintomasFisicos { get; set; }
    public string? Observacoes { get; set; }
}

public class IndicadoresBemEstarDto
{
    public int EquipeId { get; set; }
    public string? EquipeNome { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public double MediaHumor { get; set; }
    public double MediaEstresse { get; set; }
    public double MediaQualidadeSono { get; set; }
    public int TotalCheckIns { get; set; }
}

