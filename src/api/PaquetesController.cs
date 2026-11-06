using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CargoBox.Application;

namespace CargoBox.Api
{
    [ApiController]
    [Route("paquetes")]
    public class PaquetesController : ControllerBase
    {
        private readonly RecepcionAppService _recepcionService;
        private readonly EntregaAppService _entregaService;
        private readonly OperacionLogisticaAppService _operacionLogisticaService;

        public PaquetesController(
            RecepcionAppService recepcionService, 
            EntregaAppService entregaService, 
            OperacionLogisticaAppService operacionLogisticaService)
        {
            _recepcionService = recepcionService;
            _entregaService = entregaService;
            _operacionLogisticaService = operacionLogisticaService;
        }

        // POST /paquetes - Crea un paquete nuevo con estado inicial "Recibido"
        [HttpPost]
        public async Task<IActionResult> CrearPaquete([FromBody] CrearPaqueteRequest request)
        {
            try
            {
                var guia = await _recepcionService.RegistrarNuevoPaqueteAsync(
                    request.IdPaquete, request.Peso, request.Dimensiones, request.Destino, request.NivelServicio);

                return Ok(new { mensaje = "Paquete creado con éxito", guia, estado = "Recibido" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // PUT /paquetes/{guia}/clasificar - Cambia el estado a "Clasificado"
        [HttpPut("{guia}/clasificar")]
        public async Task<IActionResult> ClasificarPaquete(string guia)
        {
            try
            {
                await _operacionLogisticaService.ClasificarPaqueteAsync(guia);
                return Ok(new { mensaje = "Paquete clasificado correctamente", estado = "Clasificado" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // PUT /paquetes/{guia}/transito - Cambia el estado a "EnTránsito"
        [HttpPut("{guia}/transito")]
        public async Task<IActionResult> PonerEnTransito(string guia)
        {
            try
            {
                await _operacionLogisticaService.PonerEnTransitoAsync(guia);
                return Ok(new { mensaje = "Paquete puesto en tránsito", estado = "EnTransito" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // PUT /paquetes/{guia}/centro-destino - Cambia el estado a "CentroDestino"
        [HttpPut("{guia}/centro-destino")]
        public async Task<IActionResult> MarcarCentroDestino(string guia)
        {
            try
            {
                await _operacionLogisticaService.MarcarCentroDestinoAsync(guia);
                return Ok(new { mensaje = "Paquete llegado al centro destino", estado = "CentroDestino" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // PUT /paquetes/{guia}/entregar - Finaliza la entrega validando la evidencia obligatoria (BR012)
        [HttpPut("{guia}/entregar")]
        public async Task<IActionResult> EntregarPaquete(string guia, [FromBody] EntregarPaqueteRequest request)
        {
            try
            {
                await _entregaService.FinalizarEntregaAsync(guia, request.FirmaDigital, request.FotoUrl);
                return Ok(new { mensaje = "Entrega registrada exitosamente", estado = "Entregado" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }

    // Records de soporte para recibir los JSON en los Requests Body
    public record CrearPaqueteRequest(string IdPaquete, decimal Peso, decimal Dimensiones, string Destino, string NivelServicio);
    public record EntregarPaqueteRequest(string FirmaDigital, string FotoUrl);
}