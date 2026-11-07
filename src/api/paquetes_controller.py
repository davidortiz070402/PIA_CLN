from fastapi import APIRouter, HTTPException
from pydantic import BaseModel
from src.application.recepcion_app_service import RecepcionAppService
from src.application.entrega_app_service import EntregaAppService
from src.application.operacion_logistica_app_service import OperacionLogisticaAppService
from src.infrastructure.paquete_repository_sqlite import PaqueteRepository
from src.domain.calculadora_tarifa import CalculadoraTarifa

router = APIRouter(prefix="/paquetes", tags=["Paquetes"])

# Instancias compartidas para la inyección de dependencias
repo = PaqueteRepository()
calc = CalculadoraTarifa()
recepcion_service = RecepcionAppService(repo, calc)
entrega_service = EntregaAppService(repo)
logistica_service = OperacionLogisticaAppService(repo)

class CrearPaqueteRequest(BaseModel):
    id_paquete: str
    peso: float
    dimensiones: float
    destino: str
    nivel_servicio: str

class EntregarPaqueteRequest(BaseModel):
    firma_digital: str
    foto_url: str

@router.post("/")
async def crear_paquete(request: CrearPaqueteRequest):
    try:
        guia = await recepcion_service.registrar_nuevo_paquete(
            request.id_paquete, request.peso, request.dimensiones, request.destino, request.nivel_servicio
        )
        return {"mensaje": "Paquete creado con éxito", "guia": guia, "estado": "Recibido"}
    except Exception as e:
        raise HTTPException(status_code=400, detail=str(e))

@router.put("/{guia}/clasificar")
async def clasificar_paquete(guia: str):
    try:
        await logistica_service.clasificar_paquete(guia)
        return {"mensaje": "Paquete clasificado correctamente", "estado": "Clasificado"}
    except Exception as e:
        raise HTTPException(status_code=400, detail=str(e))

@router.put("/{guia}/transito")
async def poner_en_transito(guia: str):
    try:
        await logistica_service.poner_en_transito(guia)
        return {"mensaje": "Paquete puesto en tránsito", "estado": "EnTránsito"}
    except Exception as e:
        raise HTTPException(status_code=400, detail=str(e))

@router.put("/{guia}/centro-destino")
async def marcar_centro_destino(guia: str):
    try:
        await logistica_service.marcar_centro_destino(guia)
        return {"mensaje": "Paquete llegado al centro destino", "estado": "CentroDestino"}
    except Exception as e:
        raise HTTPException(status_code=400, detail=str(e))

@router.put("/{guia}/entregar")
async def entregar_paquete(guia: str, request: EntregarPaqueteRequest):
    try:
        await entrega_service.finalizar_entrega(guia, request.firma_digital, request.foto_url)
        return {"mensaje": "Entrega registrada exitosamente", "estado": "Entregado"}
    except Exception as e:
        raise HTTPException(status_code=400, detail=str(e))