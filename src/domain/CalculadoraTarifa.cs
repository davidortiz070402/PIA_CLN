namespace CargoBox.Domain
{
    public class CalculadoraTarifa
    {
        public decimal Calcular(decimal pesoKg, decimal volumenM3, string destino, string nivelServicio)
        {
            decimal tarifaBase = destino.ToLower() == "monterrey" ? 100m : 150m;
            decimal costoPeso = pesoKg * 15m;
            decimal costoVolumen = volumenM3 * 20m;
            
            decimal multiplicadorServicio = nivelServicio.ToLower() == "expres" ? 1.5m : 1.0m;

            return (tarifaBase + costoPeso + costoVolumen) * multiplicadorServicio;
        }
    }
}