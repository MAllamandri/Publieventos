﻿namespace PubliEventos.Domain.Domain
{
    using PubliEventos.DataAccess.Infrastructure;
    using System;
    using System.Collections.Generic;

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
        public virtual int ContentType { get; set; }

        /// <summary>
        /// Indica si está activo.
        /// </summary>
        public virtual bool Active { get; set; }

        /// <summary>
        /// Reportes que recibio el contenido.
        /// </summary>
        public virtual IList<Report> Reports { get; set; }

        /// <summary>
        /// Indica si está dado de baja.
        /// </summary>
        public virtual DateTime? NullDate { get; set; }

        /// <summary>
        /// Fecha de alta del contenido.
        /// </summary>
        public virtual DateTime EffectDate { get; set; }
    }
}
