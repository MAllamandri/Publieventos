namespace PubliEventos.Domain.Domain
{
    using System;
    using PubliEventos.DataAccess.Infrastructure;
    using PubliEventos.Domain.Enums;

    /// <summary>
    /// Representa un contenido multimedia usado en la aplicación (imagen, video, evento o comentario).
    /// </summary>
    public class MultimediaContent : BaseIdentifier<int>
    {
        /// <summary>
        /// Evento al que pertence el contenido.
        /// </summary>
        public virtual Event Event { get; set; }

        /// <summary>
        /// Nombre del contenido.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Tipo de contenido.
        /// </summary>
        public virtual ContentTypes ContentType { get; set; }

        /// <summary>
        /// Indica si está activo.
        /// </summary>
        public virtual bool Active { get; set; }

        /// <summary>
        /// Indica si está dado de baja.
        /// </summary>
        public virtual DateTime NullDate { get; set; }

        /// <summary>
        /// Fecha de alta del contenido.
        /// </summary>
        public virtual DateTime EffectDate { get; set; }
    }
}
