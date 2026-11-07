from enum import Enum

class EstadoEnvio(Enum):
    RECIBIDO = "Recibido"
    CLASIFICADO = "Clasificado"
    EN_TRANSITO = "EnTránsito"
    CENTRO_DESTINO = "CentroDestino"
    EN_REPARTO = "EnReparto"
    ENTREGADO = "Entregado"
    SEGUNDO_INTENTO_PENDIENTE = "SegundoIntentoPendiente"
    RECHAZADO_EN_PROCESO_DE_DEVOLUCION = "Rechazado - En proceso de devolución"
    RETENIDO_EN_SUCURSAL = "Retenido en sucursal"