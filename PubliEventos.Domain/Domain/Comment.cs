namespace PubliEventos.Domain.Domain
{
    using System;
    using PubliEventos.DataAccess.Infrastructure;

    /// <summary>
    /// Representa un comentario.
    /// </summary>
    public class Comment : BaseIdentifier<int>
    {
        /// <summary>
        /// Evento al que corresponde el comentario.
        /// </summary>
        public virtual Event Event { get; set; }

        /// <summary>
        /// Usuario que realiza el comentario.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Detalle del comentario.
        /// </summary>
        public virtual string Detail { get; set; }

        /// <summary>
        /// Fecha de creación.
        /// </summary>
        public virtual DateTime EffectDate { get; set; }

        /// <summary>
        /// Indica si está activo.
        /// </summary>
        public virtual bool Active { get; set; }

        /// <summary>
        /// Indica si está dado de baja.
        /// </summary>
        public virtual DateTime? NullDate { get; set; }

        /// <summary>
        /// Tiempo transcurrido desde que fue creado el comentario.
        /// </summary>
        public virtual string ElapsedTime
        {
            get
            {
                TimeSpan timeStan = DateTime.Now - this.EffectDate;

                var timeDiff = TimeSpan.FromMilliseconds(timeStan.TotalMilliseconds).TotalSeconds;

                var typeTime = "";

                if (timeDiff < 60)
                {
                    typeTime = timeDiff.ToString("0") == "1" ? "segundo." : "segundos.";

                    return string.Format("Hace aproximadamente {0} {1}", timeDiff.ToString("0"), typeTime);
                }
                else if (TimeSpan.FromSeconds(timeDiff).TotalMinutes < 60)
                {
                    typeTime = TimeSpan.FromSeconds(timeDiff).TotalMinutes.ToString("0") == "1" ? "minuto." : "minutos.";

                    return string.Format("Hace aproximadamente {0} {1}", TimeSpan.FromSeconds(timeDiff).TotalMinutes.ToString("0"), typeTime);
                }
                else if (TimeSpan.FromSeconds(timeDiff).TotalHours < 24)
                {
                    typeTime = TimeSpan.FromSeconds(timeDiff).TotalHours.ToString("0") == "1" ? "hora." : "horas.";

                    return string.Format("Hace aproximadamente {0} {1}", TimeSpan.FromSeconds(timeDiff).TotalHours.ToString("0"), typeTime);
                }
                else if (TimeSpan.FromSeconds(timeDiff).TotalDays < 90)
                {
                    typeTime = TimeSpan.FromSeconds(timeDiff).TotalDays.ToString("0") == "1" ? "día." : "días.";

                    return string.Format("Hace aproximadamente {0} {1}", TimeSpan.FromSeconds(timeDiff).TotalDays.ToString("0"), typeTime);
                }
                else
                {
                    var months = Math.Round(TimeSpan.FromSeconds(timeDiff).TotalDays / (double)30, 0, MidpointRounding.ToEven);

                    if (months < 12)
                    {
                        return string.Format("Hace aproximadamente {0} {1}", months, months == 1 ? "mes." : "meses.");
                    }
                    else
                    {
                        return string.Format("Hace más de un año");
                    }
                }
            }
        }
    }
}
