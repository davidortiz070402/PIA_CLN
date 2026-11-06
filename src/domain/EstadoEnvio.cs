namespace CargoBox.Domain
{
    public enum EstadoEnvio
    {
        Recibido,
        Clasificado,
        EnTransito,
        CentroDestino,
        EnReparto,
        Entregado,
        SegundoIntentoPendiente,
        RechazadoEnProcesoDeDevolucion
    }
}