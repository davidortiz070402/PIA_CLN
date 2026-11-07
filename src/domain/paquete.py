from .guia_rastreo import GuiaRastreo
from .estado_envio import EstadoEnvio
from .evidencia_recepcion import EvidenciaRecepcion

class Paquete:
    def __init__(self, guia: GuiaRastreo, destino: str, tarifa: float):
        self.guia = guia
        self.destino = destino
        self.tarifa = tarifa
        self.estado = EstadoEnvio.RECIBIDO
        self.intentos_fallidos = 0
        self.evidencia = None

    def marcar_clasificado(self):
        if self.estado != EstadoEnvio.RECIBIDO:
            raise ValueError("El paquete debe estar Recibido para ser Clasificado.")
        self.estado = EstadoEnvio.CLASIFICADO

    def marcar_en_transito(self):
        if self.estado != EstadoEnvio.CLASIFICADO:
            raise ValueError("El paquete debe estar Clasificado para ponerse En Tránsito.")
        self.estado = EstadoEnvio.EN_TRANSITO

    def marcar_centro_destino(self):
        if self.estado != EstadoEnvio.EN_TRANSITO:
            raise ValueError("El paquete debe estar En Tránsito para llegar al Centro Destino.")
        self.estado = EstadoEnvio.CENTRO_DESTINO

    def marcar_en_reparto(self):
        if self.estado not in [EstadoEnvio.CENTRO_DESTINO, EstadoEnvio.SEGUNDO_INTENTO_PENDIENTE]:
            raise ValueError("El paquete no está listo para reparto local.")
        self.estado = EstadoEnvio.EN_REPARTO

    def finalizar_entrega(self, evidencia: EvidenciaRecepcion):
        if self.estado != EstadoEnvio.EN_REPARTO:
            raise ValueError("El paquete debe estar En Reparto para finalizar la entrega.")
        if evidencia is None:
            raise ValueError("BR012: El estado Entregado está bloqueado si no se proporciona evidencia de recepción.")
        self.evidencia = evidencia
        self.estado = EstadoEnvio.ENTREGADO

    def registrar_destinatario_ausente(self):
        if self.estado != EstadoEnvio.EN_REPARTO:
            raise ValueError("Solo se puede registrar ausencia durante el reparto.")
        self.intentos_fallidos += 1
        if self.intentos_fallidos in [1, 2]:
            self.estado = EstadoEnvio.SEGUNDO_INTENTO_PENDIENTE
        else:
            raise ValueError("BR014: Un tercer intento por destinatario ausente requiere autorización explícita de un supervisor.")

    def registrar_rechazo(self):
        self.estado = EstadoEnvio.RECHAZADO_EN_PROCESO_DE_DEVOLUCION