namespace PubliEventos.Domain.Domain
{
    using PubliEventos.DataAccess.Infrastructure;
    using System;

    /// <summary>
    /// Representa un código de verificación del usuario para recuperar su contraseña.
    /// </summary>
    public class RecoverPasswordCode : BaseIdentifier<int>
    {
        /// <summary>
        /// Usuario.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Código de verificación.
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Fecha de alta.
        /// </summary>
        public virtual DateTime EffectDate { get; set; }

        /// <summary>
        /// Fecha de baja.
        /// </summary>
        public virtual DateTime? NullDate { get; set; }
    }
}
