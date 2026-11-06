using System.Threading.Tasks;
using System.Collections.Generic;
using CargoBox.Domain;

namespace CargoBox.Application
{
    public class EntregaAppService
    {
        private readonly IPaqueteRepository _repositorio;

        public EntregaAppService(IPaqueteRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task FinalizarEntregaAsync(string codigoGuia, string firma, string foto)
        {
            var guia = new GuiaRastreo(codigoGuia);
            var paquete = await _repositorio.ObtenerPorGuiaAsync(guia);

            if (paquete == null)
                throw new KeyNotFoundException("Paquete no encontrado.");

            var evidencia = new EvidenciaRecepcion(firma, foto);
            paquete.FinalizarEntrega(evidencia); // Valida la regla BR012 (bloquea si falta evidencia)[cite: 40]

            await _repositorio.GuardarAsync(paquete);
        }
    }
}