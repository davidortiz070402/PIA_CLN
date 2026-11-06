using System.Threading.Tasks;
using CargoBox.Domain;

namespace CargoBox.Application
{
    public class RecepcionAppService
    {
        private readonly IPaqueteRepository _repositorio;
        private readonly CalculadoraTarifa _calculadora;

        public RecepcionAppService(IPaqueteRepository repositorio, CalculadoraTarifa calculadora)
        {
            _repositorio = repositorio;
            _calculadora = calculadora;
        }

        public async Task<string> RegistrarNuevoPaqueteAsync(string codigoGuia, decimal peso, decimal dimensiones, string destino, string nivelServicio)
        {
            decimal tarifa = _calculadora.Calcular(peso, dimensiones, destino, nivelServicio); // Aplica BR005[cite: 40]
            var guia = new GuiaRastreo(codigoGuia);
            
            var paquete = new Paquete(guia, destino, tarifa);
            await _repositorio.GuardarAsync(paquete);

            return guia.Valor;
        }
    }
}