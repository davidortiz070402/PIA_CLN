class CalculadoraTarifa:
    def calcular(self, peso_kg: float, volumen_m3: float, destino: str, nivel_servicio: str) -> float:
        tarifa_base = 100.0 if destino.lower() == "monterrey" else 150.0
        costo_peso = peso_kg * 15.0
        costo_volumen = volumen_m3 * 20.0
        multiplicador = 1.5 if nivel_servicio.lower() == "expres" else 1.0
        return (tarifa_base + costo_peso + costo_volumen) * multiplicador