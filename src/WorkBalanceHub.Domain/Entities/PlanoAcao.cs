namespace WorkBalanceHub.Domain.Entities;

public enum StatusPlanoAcao
{
    Planejado = 1,
    EmAndamento = 2,
    Concluido = 3,
    Cancelado = 4
}

public class PlanoAcao
{
    public int Id { get; private set; }
    public string Titulo { get; private set; }
    public string Descricao { get; private set; }
    public int EquipeId { get; private set; }
    public DateTime DataInicio { get; private set; }
    public DateTime DataFim { get; private set; }
    public StatusPlanoAcao Status { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public string? Observacoes { get; private set; }

    // Navegação
    public virtual Equipe Equipe { get; private set; } = null!;

    private PlanoAcao() { } // EF Core

    public PlanoAcao(string titulo, string descricao, int equipeId, DateTime dataInicio, DateTime dataFim)
    {
        ValidarTitulo(titulo);
        ValidarDescricao(descricao);
        ValidarEquipeId(equipeId);
        ValidarDatas(dataInicio, dataFim);
        
        Titulo = titulo;
        Descricao = descricao;
        EquipeId = equipeId;
        DataInicio = dataInicio;
        DataFim = dataFim;
        Status = StatusPlanoAcao.Planejado;
        DataCriacao = DateTime.UtcNow;
    }

    public void Atualizar(string titulo, string descricao, DateTime dataInicio, DateTime dataFim, string? observacoes = null)
    {
        ValidarTitulo(titulo);
        ValidarDescricao(descricao);
        ValidarDatas(dataInicio, dataFim);
        
        Titulo = titulo;
        Descricao = descricao;
        DataInicio = dataInicio;
        DataFim = dataFim;
        Observacoes = observacoes;
    }

    public void AlterarStatus(StatusPlanoAcao novoStatus)
    {
        if (novoStatus == StatusPlanoAcao.Concluido && Status == StatusPlanoAcao.Planejado)
            throw new InvalidOperationException("Não é possível concluir um plano que ainda está planejado.");
        
        Status = novoStatus;
    }

    public bool EstaAtivo()
    {
        var hoje = DateTime.UtcNow.Date;
        return Status != StatusPlanoAcao.Cancelado && 
               Status != StatusPlanoAcao.Concluido &&
               hoje >= DataInicio.Date && 
               hoje <= DataFim.Date;
    }

    private static void ValidarTitulo(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("O título do plano de ação é obrigatório.", nameof(titulo));
        
        if (titulo.Length > 200)
            throw new ArgumentException("O título não pode exceder 200 caracteres.", nameof(titulo));
    }

    private static void ValidarDescricao(string descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("A descrição do plano de ação é obrigatória.", nameof(descricao));
        
        if (descricao.Length > 1000)
            throw new ArgumentException("A descrição não pode exceder 1000 caracteres.", nameof(descricao));
    }

    private static void ValidarEquipeId(int equipeId)
    {
        if (equipeId <= 0)
            throw new ArgumentException("O ID da equipe deve ser maior que zero.", nameof(equipeId));
    }

    private static void ValidarDatas(DateTime dataInicio, DateTime dataFim)
    {
        if (dataFim < dataInicio)
            throw new ArgumentException("A data de fim deve ser posterior à data de início.", nameof(dataFim));
    }
}

