using System.Threading.Tasks;

namespace CargoBox.Domain
{
    public interface IPaqueteRepository
    {
        Task<Paquete> ObtenerPorGuiaAsync(GuiaRastreo guia);
        Task GuardarAsync(Paquete paquete);
    }
}