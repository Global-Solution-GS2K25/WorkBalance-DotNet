namespace WorkBalanceHub.Application.DTOs;

public class LeituraAmbienteDto
{
    public int Id { get; set; }
    public int EstacaoTrabalhoId { get; set; }
    public string? EstacaoTrabalhoNome { get; set; }
    public DateTime DataLeitura { get; set; }
    public decimal Temperatura { get; set; }
    public decimal NivelRuido { get; set; }
    public decimal Luminosidade { get; set; }
    public bool DentroDoConforto { get; set; }
}

public class CriarLeituraAmbienteDto
{
    public int EstacaoTrabalhoId { get; set; }
    public decimal Temperatura { get; set; }
    public decimal NivelRuido { get; set; }
    public decimal Luminosidade { get; set; }
}

public class AtualizarLeituraAmbienteDto
{
    public decimal Temperatura { get; set; }
    public decimal NivelRuido { get; set; }
    public decimal Luminosidade { get; set; }
}

