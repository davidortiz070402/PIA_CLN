using System;

namespace CargoBox.Domain
{
    public class Paquete
    {
        public GuiaRastreo Guia { get; private set; }
        public EstadoEnvio Estado { get; private set; }
        public string Destino { get; private set; }
        public decimal Tarifa { get; private set; }
        public int IntentosFallidos { get; private set; }
        public EvidenciaRecepcion Evidencia { get; private set; }

        private Paquete() { } // Constructor para ORM / Infraestructura

        public Paquete(GuiaRastreo guia, string destino, decimal tarifaCalculada)
        {
            Guia = guia ?? throw new ArgumentNullException(nameof(guia));
            Destino = destino ?? throw new ArgumentNullException(nameof(destino));
            Tarifa = tarifaCalculada;
            Estado = EstadoEnvio.Recibido; // BR007[cite: 40]
            IntentosFallidos = 0;
        }

        public void MarcarClasificado()
        {
            if (Estado != EstadoEnvio.Recibido)
                throw new InvalidOperationException("El paquete debe estar Recibido para ser Clasificado.");
            Estado = EstadoEnvio.Clasificado;
        }

        public void MarcarEnTransito()
        {
            if (Estado != EstadoEnvio.Clasificado)
                throw new InvalidOperationException("El paquete debe estar Clasificado para ponerse En Tránsito.");
            Estado = EstadoEnvio.EnTransito;
        }

        public void MarcarCentroDestino()
        {
            if (Estado != EstadoEnvio.EnTransito)
                throw new InvalidOperationException("El paquete debe estar En Tránsito para llegar al Centro Destino.");
            Estado = EstadoEnvio.CentroDestino;
        }

        public void MarcarEnReparto()
        {
            if (Estado != EstadoEnvio.CentroDestino && Estado != EstadoEnvio.SegundoIntentoPendiente)
                throw new InvalidOperationException("El paquete no está listo para reparto local.");
            Estado = EstadoEnvio.EnReparto;
        }

        public void FinalizarEntrega(EvidenciaRecepcion evidencia)
        {
            if (Estado != EstadoEnvio.EnReparto)
                throw new InvalidOperationException("El paquete debe estar En Reparto para finalizar la entrega.");
            
            if (evidencia == null)
                throw new InvalidOperationException("BR012: El estado Entregado está bloqueado si no se proporciona evidencia de recepción."); //[cite: 40]

            Evidencia = evidencia;
            Estado = EstadoEnvio.Entregado;
        }

        public void RegistrarDestinatarioAusente()
        {
            if (Estado != EstadoEnvio.EnReparto)
                throw new InvalidOperationException("Solo se puede registrar ausencia durante el reparto.");

            IntentosFallidos++;

            if (IntentosFallidos == 1 || IntentosFallidos == 2)
            {
                Estado = EstadoEnvio.SegundoIntentoPendiente; // BR014[cite: 40]
            }
            else
            {
                throw new InvalidOperationException("BR014: Un tercer intento por destinatario ausente requiere autorización explícita de un supervisor."); //[cite: 40]
            }
        }

        public void RegistrarRechazo()
        {
            Estado = EstadoEnvio.RechazadoEnProcesoDeDevolucion; //[cite: 45]
        }
    }
}