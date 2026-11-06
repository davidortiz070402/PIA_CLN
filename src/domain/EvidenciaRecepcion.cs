namespace CargoBox.Domain
{
    public sealed class EvidenciaRecepcion
    {
        public string FirmaDigital { get; }
        public string FotoUrl { get; }
        public System.DateTime Timestamp { get; }

        public EvidenciaRecepcion(string firmaDigital, string fotoUrl)
        {
            if (string.IsNullOrWhiteSpace(firmaDigital) && string.IsNullOrWhiteSpace(fotoUrl))
                throw new System.ArgumentException("Se requiere una firma digital o fotografía como evidencia de recepción.");
            
            FirmaDigital = firmaDigital;
            FotoUrl = fotoUrl;
            Timestamp = System.DateTime.UtcNow;
        }
    }
}