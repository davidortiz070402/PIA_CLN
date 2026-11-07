class GuiaRastreo:
    def __init__(self, valor: str):
        if not valor or not valor.strip():
            raise ValueError("La guía de rastreo no puede estar vacía.")
        self.valor = valor

    def __eq__(self, other):
        return isinstance(other, GuiaRastreo) and self.valor == other.valor

    def __hash__(self):
        return hash(self.valor)

    def __str__(self):
        return self.valor