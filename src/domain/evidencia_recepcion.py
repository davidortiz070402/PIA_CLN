from datetime import datetime

class EvidenciaRecepcion:
    def __init__(self, firma_digital: str, foto_url: str):
        if not firma_digital and not foto_url:
            raise ValueError("Se requiere una firma digital o fotografía como evidencia de recepción.")
        self.firma_digital = firma_digital
        self.foto_url = foto_url
        self.timestamp = datetime.utcnow()