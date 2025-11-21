namespace WorkBalanceHub.Domain.Entities;

public class LeituraAmbiente
{
    public int Id { get; private set; }
    public int EstacaoTrabalhoId { get; private set; }
    public DateTime DataLeitura { get; private set; }
    public decimal Temperatura { get; private set; } // em Celsius
    public decimal NivelRuido { get; private set; } // em dB
    public decimal Luminosidade { get; private set; } // em lux

    // Navegação
    public virtual EstacaoTrabalho EstacaoTrabalho { get; private set; } = null!;

    private LeituraAmbiente() { } // EF Core

    public LeituraAmbiente(int estacaoTrabalhoId, decimal temperatura, decimal nivelRuido, decimal luminosidade)
    {
        ValidarEstacaoTrabalhoId(estacaoTrabalhoId);
        ValidarTemperatura(temperatura);
        ValidarNivelRuido(nivelRuido);
        ValidarLuminosidade(luminosidade);
        
        EstacaoTrabalhoId = estacaoTrabalhoId;
        DataLeitura = DateTime.UtcNow;
        Temperatura = temperatura;
        NivelRuido = nivelRuido;
        Luminosidade = luminosidade;
    }

    public bool EstaDentroDoConforto()
    {
        // Faixas de conforto aproximadas
        // Temperatura: 20-26°C
        // Ruído: < 55 dB (escritório)
        // Luminosidade: 300-500 lux (escritório)
        return Temperatura >= 20 && Temperatura <= 26 &&
               NivelRuido < 55 &&
               Luminosidade >= 300 && Luminosidade <= 500;
    }

    private static void ValidarEstacaoTrabalhoId(int estacaoTrabalhoId)
    {
        if (estacaoTrabalhoId <= 0)
            throw new ArgumentException("O ID da estação de trabalho deve ser maior que zero.", nameof(estacaoTrabalhoId));
    }

    private static void ValidarTemperatura(decimal temperatura)
    {
        if (temperatura < -50 || temperatura > 60)
            throw new ArgumentException("A temperatura deve estar entre -50°C e 60°C.", nameof(temperatura));
    }

    private static void ValidarNivelRuido(decimal nivelRuido)
    {
        if (nivelRuido < 0 || nivelRuido > 150)
            throw new ArgumentException("O nível de ruído deve estar entre 0 e 150 dB.", nameof(nivelRuido));
    }

    private static void ValidarLuminosidade(decimal luminosidade)
    {
        if (luminosidade < 0 || luminosidade > 100000)
            throw new ArgumentException("A luminosidade deve estar entre 0 e 100.000 lux.", nameof(luminosidade));
    }
}

