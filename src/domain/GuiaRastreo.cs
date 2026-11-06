namespace CargoBox.Domain
{
    public sealed class GuiaRastreo
    {
        public string Valor { get; }

        public GuiaRastreo(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new System.ArgumentException("La guía de rastreo no puede estar vacía.");
            Valor = valor;
        }

        public override bool Equals(object obj) => obj is GuiaRastreo other && Valor == other.Valor;
        public override int GetHashCode() => Valor.GetHashCode();
        public override string ToString() => Valor;
    }
}