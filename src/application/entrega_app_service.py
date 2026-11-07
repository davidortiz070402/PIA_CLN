from src.domain.guia_rastreo import GuiaRastreo
from src.domain.evidencia_recepcion import EvidenciaRecepcion
from src.domain.repositorio_paquete import IPaqueteRepository

class EntregaAppService:
    def __init__(self, repositorio: IPaqueteRepository):
        self._repositorio = repositorio

    async def finalizar_entrega(self, codigo_guia: str, firma: str, foto: str):
        guia = GuiaRastreo(codigo_guia)
        paquete = await self._repositorio.obtener_por_guia(guia)
        if not paquete:
            raise KeyError("Paquete no encontrado.")
        evidencia = EvidenciaRecepcion(firma, foto)
        paquete.finalizar_entrega(evidencia) # Valida BR012
        await self._repositorio.guardar(paquete)