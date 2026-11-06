using System.Threading.Tasks;
using CargoBox.Domain;

namespace CargoBox.Application
{
    public class OperacionLogisticaAppService
    {
        private readonly IPaqueteRepository _repositorio;

        public OperacionLogisticaAppService(IPaqueteRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task ClasificarPaqueteAsync(string codigoGuia)
        {
            var guia = new GuiaRastreo(codigoGuia);
            var paquete = await _repositorio.ObtenerPorGuiaAsync(guia);
            if (paquete == null) throw new System.Collections.Generic.KeyNotFoundException("Paquete no encontrado.");

            paquete.MarcarClasificado(); // Valida regla de negocio en dominio
            await _repositorio.GuardarAsync(paquete);
        }

        public async Task PonerEnTransitoAsync(string codigoGuia)
        {
            var guia = new GuiaRastreo(codigoGuia);
            var paquete = await _repositorio.ObtenerPorGuiaAsync(guia);
            if (paquete == null) throw new System.Collections.Generic.KeyNotFoundException("Paquete no encontrado.");

            paquete.MarcarEnTransito();
            await _repositorio.GuardarAsync(paquete);
        }

        public async Task MarcarCentroDestinoAsync(string codigoGuia)
        {
            var guia = new GuiaRastreo(codigoGuia);
            var paquete = await _repositorio.ObtenerPorGuiaAsync(guia);
            if (paquete == null) throw new System.Collections.Generic.KeyNotFoundException("Paquete no encontrado.");

            paquete.MarcarCentroDestino();
            await _repositorio.GuardarAsync(paquete);
        }
    }
}