from abc import ABC, abstractmethod
from .paquete import Paquete
from .guia_rastreo import GuiaRastreo

class IPaqueteRepository(ABC):
    @abstractmethod
    async def obtener_por_guia(self, guia: GuiaRastreo) -> Paquete:
        pass

    @abstractmethod
    async def guardar(self, paquete: Paquete):
        pass