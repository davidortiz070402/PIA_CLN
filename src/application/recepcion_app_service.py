from src.domain.guia_rastreo import GuiaRastreo
from src.domain.paquete import Paquete
from src.domain.calculadora_tarifa import CalculadoraTarifa
from src.domain.repositorio_paquete import IPaqueteRepository

class RecepcionAppService:
    def __init__(self, repositorio: IPaqueteRepository, calculadora: CalculadoraTarifa):
        self._repositorio = repositorio
        self._calculadora = calculadora

    async def registrar_nuevo_paquete(self, codigo_guia: str, peso: float, dimensiones: float, destino: str, nivel_servicio: str) -> str:
        tarifa = self._calculadora.calcular(peso, dimensiones, destino, nivel_servicio) # BR005
        guia = GuiaRastreo(codigo_guia)
        paquete = Paquete(guia, destino, tarifa)
        await self._repositorio.guardar(paquete)
        return guia.valor