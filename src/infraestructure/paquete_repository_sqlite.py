from src.domain.repositorio_paquete import IPaqueteRepository
from src.domain.paquete import Paquete
from src.domain.guia_rastreo import GuiaRastreo

class PaqueteRepository(IPaqueteRepository):
    # Simulación de almacenamiento en memoria / base de datos local para desarrollo y pruebas
    _db_mock = {}

    async def obtener_por_guia(self, guia: GuiaRastreo) -> Paquete:
        return self._db_mock.get(guia.valor)

    async def guardar(self, paquete: Paquete):
        self._db_mock[paquete.guia.valor] = paquete