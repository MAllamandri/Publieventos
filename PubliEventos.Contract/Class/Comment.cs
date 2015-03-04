namespace PubliEventos.Contract.Class
{
    using System;

    /// <summary>
    /// Representa un comentario.
    /// </summary>
    public class Comment : BaseClass
    {
        /// <summary>
        /// Evento al que pertence el comentario.
        /// </summary>
        public Event Event { get; set; }

        /// <summary>
        /// Usuario que creo el comentario.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Detalle del comentario.
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// Indica si el comentario esta activo.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Fecha de creación.
        /// </summary>
        public DateTime EffectDate { get; set; }

        /// <summary>
        /// Fecha de baja.
        /// </summary>
        public DateTime? NullDate { get; set; }

        /// <summary>
        /// Identificadores de los usuarios que reportaron el evento.
        /// </summary>
        public string[] UserReportsIds { get; set; }

        /// <summary>
        /// Tiempo transcurrido desde que fue creado.
        /// </summary>
        public string ElapsedTime { get; set; }
    }
}
