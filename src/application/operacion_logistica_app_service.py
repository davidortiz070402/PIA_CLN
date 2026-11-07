from src.domain.guia_rastreo import GuiaRastreo
from src.domain.repositorio_paquete import IPaqueteRepository

class OperacionLogisticaAppService:
    def __init__(self, repositorio: IPaqueteRepository):
        self._repositorio = repositorio

    async def clasificar_paquete(self, codigo_guia: str):
        guia = GuiaRastreo(codigo_guia)
        paquete = await self._repositorio.obtener_por_guia(guia)
        if not paquete:
            raise KeyError("Paquete no encontrado.")
        paquete.marcar_clasificado()
        await self._repositorio.guardar(paquete)

    async def poner_en_transito(self, codigo_guia: str):
        guia = GuiaRastreo(codigo_guia)
        paquete = await self._repositorio.obtener_por_guia(guia)
        if not paquete:
            raise KeyError("Paquete no encontrado.")
        paquete.marcar_en_transito()
        await self._repositorio.guardar(paquete)

    async def marcar_centro_destino(self, codigo_guia: str):
        guia = GuiaRastreo(codigo_guia)
        paquete = await self._repositorio.obtener_por_guia(guia)
        if not paquete:
            raise KeyError("Paquete no encontrado.")
        paquete.marcar_centro_destino()
        await self._repositorio.guardar(paquete)