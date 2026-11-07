from .guia_rastreo import GuiaRastreo
from .estado_envio import EstadoEnvio
from .evidencia_recepcion import EvidenciaRecepcion

# Excepciones de Dominio Personalizadas
class EvidenciaObligatoriaException(Exception):
    pass

class TercerIntentoNoAutorizadoException(Exception):
    pass

class TransicionEstadoInvalidaException(Exception):
    pass

class Paquete:
    def __init__(self, guia: GuiaRastreo, destino: str, tarifa: float, peso: float = 1.0, dimensiones: float = 1.0):
        if peso <= 0 or dimensiones <= 0:
            raise ValueError("Excepción de validación: Peso y dimensiones deben ser mayores a cero.")
        
        self.guia = guia
        self.destino = destino
        self.tarifa = tarifa
        self.peso = peso
        self.dimensiones = dimensiones
        self.estado = EstadoEnvio.RECIBIDO
        self.intentos_fallidos = 0
        self.evidencia = None

    def marcar_en_reparto(self):
        if self.estado in [EstadoEnvio.RECHAZADO_EN_PROCESO_DE_DEVOLUCION, EstadoEnvio.ENTREGADO]:
            raise TransicionEstadoInvalidaException("No se puede mover a reparto un paquete cancelado o ya entregado.")
        self.estado = EstadoEnvio.EN_REPARTO

    def finalizar_entrega(self, evidencia: EvidenciaRecepcion):
        if self.estado in [EstadoEnvio.RECHAZADO_EN_PROCESO_DE_DEVOLUCION]:
            raise TransicionEstadoInvalidaException("Transición de estado inválida: el paquete está cancelado.")
        
        if self.estado != EstadoEnvio.EN_REPARTO:
            raise TransicionEstadoInvalidaException("El paquete debe estar En Reparto para finalizar la entrega.")
        
        if evidencia is None:
            raise EvidenciaObligatoriaException("BR012: Evidencia obligatoria faltante.")
        
        self.evidencia = evidencia
        self.estado = EstadoEnvio.ENTREGADO

    def registrar_destinatario_ausente(self, autorizado_por_supervisor: bool = False):
        if self.estado != EstadoEnvio.EN_REPARTO:
            raise TransicionEstadoInvalidaException("Solo se puede registrar ausencia durante el reparto.")

        self.intentos_fallidos += 1

        if self.intentos_fallidos < 3:
            self.estado = EstadoEnvio.SEGUNDO_INTENTO_PENDIENTE
        else:
            if autorizado_por_supervisor:
                self.estado = EstadoEnvio.RETENIDO_EN_SUCURSAL # CP-06
            else:
                raise TercerIntentoNoAutorizadoException("BR014: Tercer intento requiere autorización.") # CP-05

    def registrar_rechazo(self):
        self.estado = EstadoEnvio.RECHAZADO_EN_PROCESO_DE_DEVOLUCION